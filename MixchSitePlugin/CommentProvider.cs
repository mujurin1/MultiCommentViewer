﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SitePlugin;
using ryu_s.BrowserCookie;
using Common;
using System.Threading;
using System.Net;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;
using SitePluginCommon;

namespace MixchSitePlugin
{
    [Serializable]
    public class InvalidInputException : Exception
    {
        public InvalidInputException() { }
    }
    class CommentProvider : ICommentProvider
    {
        string _liveId;
        Context _context;
        #region ICommentProvider
        #region Events
        public event EventHandler<IMetadata> MetadataUpdated;
        public event EventHandler CanConnectChanged;
        public event EventHandler CanDisconnectChanged;
        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<IMessageContext> MessageReceived;
        #endregion //Events

        #region CanConnect
        private bool _canConnect;
        public bool CanConnect
        {
            get { return _canConnect; }
            set
            {
                if (_canConnect == value)
                    return;
                _canConnect = value;
                CanConnectChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion //CanConnect

        #region CanDisconnect
        private bool _canDisconnect;
        public bool CanDisconnect
        {
            get { return _canDisconnect; }
            set
            {
                if (_canDisconnect == value)
                    return;
                _canDisconnect = value;
                CanDisconnectChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion //CanDisconnect
        protected virtual Task<string> GetLiveId(string input)
        {
            return Tools.GetLiveId(_dataSource, input);
        }
        private void BeforeConnecting()
        {
            CanConnect = false;
            CanDisconnect = true;
            _isExpectedDisconnect = false;
        }
        private void AfterDisconnected()
        {
            _500msTimer.Enabled = false;
            _ws = null;
            CanConnect = true;
            CanDisconnect = false;
        }
        protected virtual List<Cookie> GetCookies(IBrowserProfile browserProfile)
        {
            List<Cookie> cookies = null;
            try
            {
                cookies = browserProfile.GetCookieCollection("mixch.tv");
            }
            catch { }
            return cookies ?? new List<Cookie>();
        }
        protected virtual CookieContainer CreateCookieContainer(IBrowserProfile browserProfile)
        {
            var cc = new CookieContainer();
            try
            {
                var cookies = browserProfile.GetCookieCollection("mixch.tv");
                foreach (var cookie in cookies)
                {
                    cc.Add(cookie);
                }
            }
            catch { }
            return cc;
        }
        //protected virtual Extract()
        private async Task ConnectInternalAsync(string input, IBrowserProfile browserProfile)
        {
            if (_ws != null)
            {
                throw new InvalidOperationException("");
            }
            var cookies = GetCookies(browserProfile);
            _cc = CreateCookieContainer(cookies);
            _context = Tools.GetContext(cookies);
            string liveId;
            try
            {
                liveId = await GetLiveId(input);
                _liveId = liveId;
            }
            catch (InvalidInputException ex)
            {
                _logger.LogException(ex, "無効な入力値", $"input={input}");
                SendSystemInfo("無効な入力値です", InfoType.Error);
                AfterDisconnected();
                return;
            }

            var movieContext2 = await GetMovieInfo(liveId);
            var movieId = movieContext2.MovieId;
            if (movieId == 0)
            {
                SendSystemInfo("存在しないURLまたはIDです", InfoType.Error);
                AfterDisconnected();
                return;
            }
            if (movieContext2.OnairStatus == 2)
            {
                SendSystemInfo("この放送は終了しています", InfoType.Error);
                AfterDisconnected();
                return;
            }
            MetadataUpdated?.Invoke(this, new Metadata { Title = movieContext2.Title });

            _startAt = movieContext2.StartedAt.DateTime;
            _500msTimer.Enabled = true;

            var (chats, raw) = await GetChats(movieContext2);
            try
            {
                foreach (var item in chats)
                {
                    var comment = Tools.Parse(item);
                    var commentData = Tools.CreateCommentData(comment, _startAt, _siteOptions);
                    var messageContext = CreateMessageContext(comment, commentData, true);
                    if (messageContext != null)
                    {
                        MessageReceived?.Invoke(this, messageContext);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex, "", "raw=" + raw);
            }
            foreach (var user in _userStoreManager.GetAllUsers(SiteType.Mixch))
            {
                if (!(user is IUser2 user2)) continue;
                _userDict.AddOrUpdate(user2.UserId, user2, (id, u) => u);
            }
        Reconnect:
            _ws = CreateMixchWebsocket();
            _ws.Received += WebSocket_Received;

            var userAgent = GetUserAgent(browserProfile.Type);
            var wsTask = _ws.ReceiveAsync(movieId.ToString(), userAgent, cookies);
            var blackListProvider = CreateBlacklistProvider();
            blackListProvider.Received += BlackListProvider_Received;
            var blackTask = blackListProvider.ReceiveAsync(movieId.ToString(), _context);

            var tasks = new List<Task>
            {
                wsTask,
                blackTask
            };

            while (tasks.Count > 0)
            {
                var t = await Task.WhenAny(tasks);
                if (t == blackTask)
                {
                    try
                    {
                        await blackTask;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogException(ex);
                    }
                    tasks.Remove(blackTask);
                }
                else
                {
                    blackListProvider.Disconnect();
                    try
                    {
                        await blackTask;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogException(ex);
                    }
                    tasks.Remove(blackTask);
                    SendSystemInfo("ブラックリストタスク終了", InfoType.Debug);
                    try
                    {
                        await wsTask;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogException(ex);
                    }
                    tasks.Remove(wsTask);
                    SendSystemInfo("wsタスク終了", InfoType.Debug);
                }
            }
            _ws.Received -= WebSocket_Received;
            blackListProvider.Received -= BlackListProvider_Received;

            //意図的な切断では無い場合、配信がまだ続いているか確認して、配信中だったら再接続する。
            //2019/03/12 heartbeatを送っているのにも関わらずwebsocketが切断されてしまう場合を確認。ブラウザでも配信中に切断されて再接続するのを確認済み。
            if (!_isExpectedDisconnect)
            {
                var movieInfo = await GetMovieInfo(liveId);
                if (movieInfo.OnairStatus == 1)
                {
                    goto Reconnect;
                }
            }
        }
        public async Task ConnectAsync(string input, IBrowserProfile browserProfile)
        {
            BeforeConnecting();
            try
            {
                await ConnectInternalAsync(input, browserProfile);
            }
            finally
            {
                AfterDisconnected();
            }
        }

        protected virtual IBlackListProvider CreateBlacklistProvider()
        {
            return new BlackListProvider(_dataSource, _logger);
        }

        protected virtual IMixchWebsocket CreateMixchWebsocket()
        {
            return new MixchWebsocket(_logger);
        }

        protected virtual async Task<(Low.Chats.RootObject[], string raw)> GetChats(MovieInfo movieContext2)
        {
            return await API.GetChats(_dataSource, movieContext2.Id, DateTime.Now, _cc);
        }

        protected virtual async Task<MovieInfo> GetMovieInfo(string liveId)
        {
            return await API.GetMovieInfo(_dataSource, liveId, _cc);
        }

        private CookieContainer CreateCookieContainer(List<Cookie> cookies)
        {
            var cc = new CookieContainer();
            try
            {
                foreach (var cookie in cookies)
                {
                    cc.Add(cookie);
                }
            }
            catch { }
            return cc;
        }

        private MixchMessageContext CreateMessageContext(Tools.IComment comment, IMixchCommentData commentData, bool isInitialComment)
        {
            var userId = commentData.UserId;
            var user = GetUser(userId) as IUser2;
            if (!_userDict.ContainsKey(userId))
            {
                _userDict.AddOrUpdate(user.UserId, user, (id, u) => u);
            }
            bool isFirstComment;
            if (_userCommentCountDict.ContainsKey(userId))
            {
                _userCommentCountDict[userId]++;
                isFirstComment = false;
            }
            else
            {
                _userCommentCountDict.Add(userId, 1);
                isFirstComment = true;
            }

            var nameItems = new List<IMessagePart>();
            nameItems.Add(MessagePartFactory.CreateMessageText(commentData.Name));
            nameItems.AddRange(commentData.NameIcons);
            user.Name = nameItems;

            var messageItems = new List<IMessagePart>();
            if (commentData.Message != null)
            {
                messageItems.Add(MessagePartFactory.CreateMessageText(commentData.Message));
            }

            MixchMessageContext messageContext = null;
            IMixchMessage message;
            if (commentData.IsYell)
            {
                message = new MixchYell("")
                {
                    YellPoints = commentData.YellPoints,
                    Id = commentData.Id,
                    NameItems = nameItems,
                    PostTime = commentData.PostTime,
                    UserId = commentData.UserId,
                    Message = commentData.Message,
                };
            }
            else if (commentData.Stamp != null)
            {
                message = new MixchStamp("")
                {
                    Stamp = commentData.Stamp,
                    Id = commentData.Id,
                    NameItems = nameItems,
                    PostTime = commentData.PostTime,
                    UserId = commentData.UserId,
                };
            }
            else
            {
                message = new MixchComment("")
                {
                    MessageItems = messageItems,
                    Id = commentData.Id,
                    NameItems = nameItems,
                    PostTime = commentData.PostTime,
                    UserId = commentData.UserId,
                };

            }
            var metadata = new MessageMetadata(message, _options, _siteOptions, user, this, isFirstComment)
            {
                IsInitialComment = isInitialComment,
                SiteContextGuid = SiteContextGuid,
            };
            var methods = new MixchMessageMethods();
            messageContext = new MixchMessageContext(message, metadata, methods);
            return messageContext;
        }

        private void BlackListProvider_Received(object sender, List<string> e)
        {
            try
            {
                var blackList = e;
                //現状BAN状態のユーザ
                var banned = _userDict.Where(kv => kv.Value.IsSiteNgUser).Select(kv => kv.Key).ToList();// _userViewModelDict.Where(kv => kv.Value.IsNgUser).Select(kv => kv.Key).ToList();

                //ブラックリストに登録されているユーザのBANフラグをONにする
                foreach (var black in blackList)
                {
                    if (_userDict.ContainsKey(black))
                    {
                        var user = _userDict[black];
                        user.IsSiteNgUser = true;
                    }
                    //ブラックリストに入っていることが確認できたためリストから外す
                    banned.Remove(black);
                }

                //ブラックリストに入っていなかったユーザのBANフラグをOFFにする
                foreach (var white in banned)
                {
                    var u = _userDict[white];
                    u.IsSiteNgUser = false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        IMixchWebsocket _ws;
        /// <summary>
        /// ユーザが意図した切断か
        /// </summary>
        bool _isExpectedDisconnect;

        public void Disconnect()
        {
            _isExpectedDisconnect = true;
            if (_ws != null)
            {
                _ws.Disconnect();
            }
        }
        public IUser GetUser(string userId)
        {
            return _userStoreManager.GetUser(SiteType.Mixch, userId);
        }
        #endregion //ICommentProvider


        #region Fields
        private ICommentOptions _options;
        private MixchSiteOptions _siteOptions;
        private ILogger _logger;
        private IUserStoreManager _userStoreManager;
        private readonly IDataSource _dataSource;
        private CookieContainer _cc;
        #endregion //Fields

        #region ctors
        System.Timers.Timer _500msTimer = new System.Timers.Timer();
        public CommentProvider(ICommentOptions options, MixchSiteOptions siteOptions, ILogger logger, IUserStoreManager userStoreManager)
        {
            _options = options;
            _siteOptions = siteOptions;
            _logger = logger;
            _userStoreManager = userStoreManager;
            _dataSource = new DataSource();
            _500msTimer.Interval = 500;
            _500msTimer.Elapsed += _500msTimer_Elapsed;
            _500msTimer.AutoReset = true;

            CanConnect = true;
            CanDisconnect = false;
        }

        private void _500msTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var elapsed = DateTime.Now - _startAt;
            MetadataUpdated?.Invoke(this, new Metadata
            {
                Elapsed = Tools.ElapsedToString(elapsed),
            });
        }
        #endregion //ctors

        #region Events
        #endregion //Events
        private void SendSystemInfo(string message, InfoType type)
        {
            var context = InfoMessageContext.Create(new InfoMessage
            {
                Text = message,
                SiteType = SiteType.Mixch,
                Type = type,
            }, _options);
            MessageReceived?.Invoke(this, context);
        }
        public Guid SiteContextGuid { get; set; }
        Dictionary<string, int> _userCommentCountDict = new Dictionary<string, int>();
        [Obsolete]
        Dictionary<string, UserViewModel> _userViewModelDict = new Dictionary<string, UserViewModel>();
        ConcurrentDictionary<string, IUser2> _userDict = new ConcurrentDictionary<string, IUser2>();
        DateTime _startAt;
        //private MixchCommentViewModel CreateCommentViewModel(IMixchCommentData data)
        //{
        //    var userId = data.UserId;
        //    bool isFirstComment;

        //    if (_userCommentCountDict.ContainsKey(userId))
        //    {
        //        _userCommentCountDict[userId]++;
        //        isFirstComment = false;
        //    }
        //    else
        //    {
        //        _userCommentCountDict.Add(userId, 1);
        //        isFirstComment = true;
        //    }
        //    var user = _userStore.GetUser(userId);
        //    if (!_userViewModelDict.TryGetValue(userId, out UserViewModel userVm))
        //    {
        //        userVm = new UserViewModel(user);
        //        _userViewModelDict.Add(userId, userVm);
        //    }
        //    var cvm = new MixchCommentViewModel(data, _options, _siteOptions,  this, isFirstComment, user);
        //    return cvm;
        //}
        private static string GetUserAgent(BrowserType browser)
        {
            string userAgent;
            switch (browser)
            {
                case BrowserType.Chrome:
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
                    break;
                case BrowserType.Firefox:
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
                    break;
                case BrowserType.IE:
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; rv:11.0) like Gecko";
                    break;
                case BrowserType.Opera:
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36 OPR/43.0.2442.1144";
                    break;
                default:
                    throw new Exception("未対応のブラウザ");
            }
            return userAgent;
        }

        private void WebSocket_Received(object sender, IPacket e)
        {
            try
            {
                if (e is PacketMessageEventMessageChat chat)
                {
                    var comment = Tools.Parse(chat.Comment);
                    var commentData = Tools.CreateCommentData(comment, _startAt, _siteOptions);
                    var messageContext = CreateMessageContext(comment, commentData, false);
                    if (messageContext != null)
                    {
                        MessageReceived?.Invoke(this, messageContext);
                    }
                }
                else if (e is PacketMessageEventMessageAudienceCount audienceCount)
                {
                    var ac = audienceCount.AudienceCount;
                    MetadataUpdated?.Invoke(this, new Metadata
                    {
                        CurrentViewers = ac.live_viewers.ToString(),
                        TotalViewers = ac.viewers.ToString(),
                    });
                }
                else if (e is PacketMessageEventMessageLiveEnd liveEnd)
                {
                    Disconnect();
                }
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }
        public async Task PostCommentAsync(string str)
        {
            await API.PostCommentAsync(_dataSource, _liveId, str, DateTime.Now, _context);
        }

        public async Task<ICurrentUserInfo> GetCurrentUserInfo(IBrowserProfile browserProfile)
        {
            var cc = CreateCookieContainer(browserProfile);
            var me = await API.GetMeAsync(_dataSource, cc);
            return new CurrentUserInfo
            {
                IsLoggedIn = !string.IsNullOrEmpty(me.UserId),
                UserId = me.UserId,
                Username = me.DisplayName,
            };
        }

        public void SetMessage(string raw)
        {
            throw new NotImplementedException();
        }
    }
    class CurrentUserInfo : ICurrentUserInfo
    {
        public string Username { get; set; }
        public string UserId { get; set; }
        public bool IsLoggedIn { get; set; }
    }
    public class MovieContext2
    {
        public string Title { get; set; }
        //"0":予約？
        //"1":放送中
        //"2":放送終了
        public string OnairStatus { get; set; }
        public string MovieId { get; set; }
        public string RecxuserId { get; set; }
        public string Id { get; set; }
        public DateTime StartAt { get; set; }
    }
    class MessageImagePortion : IMessageImagePortion
    {
        public int SrcX { get; set; }

        public int SrcY { get; set; }

        public int SrcWidth { get; set; }

        public int SrcHeight { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public Image Image { get; set; }
        public string Alt { get; set; }
    }
}
