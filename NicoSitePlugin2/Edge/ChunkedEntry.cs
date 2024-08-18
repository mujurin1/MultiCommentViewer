// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: dwango/nicolive/chat/service/edge/ChunkedEntry.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Dwango.Nicolive.Chat.Service.Edge {

  /// <summary>Holder for reflection information generated from dwango/nicolive/chat/service/edge/ChunkedEntry.proto</summary>
  public static partial class ChunkedEntryReflection {

    #region Descriptor
    /// <summary>File descriptor for dwango/nicolive/chat/service/edge/ChunkedEntry.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ChunkedEntryReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjRkd2FuZ28vbmljb2xpdmUvY2hhdC9zZXJ2aWNlL2VkZ2UvQ2h1bmtlZEVu",
            "dHJ5LnByb3RvEiFkd2FuZ28ubmljb2xpdmUuY2hhdC5zZXJ2aWNlLmVkZ2Ua",
            "H2dvb2dsZS9wcm90b2J1Zi90aW1lc3RhbXAucHJvdG8aNmR3YW5nby9uaWNv",
            "bGl2ZS9jaGF0L3NlcnZpY2UvZWRnZS9NZXNzYWdlU2VnbWVudC5wcm90bxo3",
            "ZHdhbmdvL25pY29saXZlL2NoYXQvc2VydmljZS9lZGdlL0JhY2t3YXJkU2Vn",
            "bWVudC5wcm90byLWAgoMQ2h1bmtlZEVudHJ5EkQKB3NlZ21lbnQYASABKAsy",
            "MS5kd2FuZ28ubmljb2xpdmUuY2hhdC5zZXJ2aWNlLmVkZ2UuTWVzc2FnZVNl",
            "Z21lbnRIABJGCghiYWNrd2FyZBgCIAEoCzIyLmR3YW5nby5uaWNvbGl2ZS5j",
            "aGF0LnNlcnZpY2UuZWRnZS5CYWNrd2FyZFNlZ21lbnRIABJFCghwcmV2aW91",
            "cxgDIAEoCzIxLmR3YW5nby5uaWNvbGl2ZS5jaGF0LnNlcnZpY2UuZWRnZS5N",
            "ZXNzYWdlU2VnbWVudEgAEkwKBG5leHQYBCABKAsyPC5kd2FuZ28ubmljb2xp",
            "dmUuY2hhdC5zZXJ2aWNlLmVkZ2UuQ2h1bmtlZEVudHJ5LlJlYWR5Rm9yTmV4",
            "dEgAGhoKDFJlYWR5Rm9yTmV4dBIKCgJhdBgBIAEoA0IHCgVlbnRyeWIGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, global::Dwango.Nicolive.Chat.Service.Edge.MessageSegmentReflection.Descriptor, global::Dwango.Nicolive.Chat.Service.Edge.BackwardSegmentReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry), global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Parser, new[]{ "Segment", "Backward", "Previous", "Next" }, new[]{ "Entry" }, null, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext), global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext.Parser, new[]{ "At" }, null, null, null)})
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  ///*
  /// ストリーミング配信されてくる各チャンク。
  /// 開始時刻を指定してサーバーへストリームをリクエストすると、開始時刻から一定時間内に発生したメッセージがすべてチャンクとして送られてくる。
  /// 開始時刻が過去の時刻の場合は指定時間内のすべてのチャンクが一瞬で送られてきて接続がすぐに終了する。
  /// 開始時刻が現在の時刻の場合は一定時間の間接続が開いたままとなり、発生したメッセージがリアルタイムにストリームで送られてくる。
  /// </summary>
  public sealed partial class ChunkedEntry : pb::IMessage<ChunkedEntry> {
    private static readonly pb::MessageParser<ChunkedEntry> _parser = new pb::MessageParser<ChunkedEntry>(() => new ChunkedEntry());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ChunkedEntry> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntryReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChunkedEntry() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChunkedEntry(ChunkedEntry other) : this() {
      switch (other.EntryCase) {
        case EntryOneofCase.Segment:
          Segment = other.Segment.Clone();
          break;
        case EntryOneofCase.Backward:
          Backward = other.Backward.Clone();
          break;
        case EntryOneofCase.Previous:
          Previous = other.Previous.Clone();
          break;
        case EntryOneofCase.Next:
          Next = other.Next.Clone();
          break;
      }

      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ChunkedEntry Clone() {
      return new ChunkedEntry(this);
    }

    /// <summary>Field number for the "segment" field.</summary>
    public const int SegmentFieldNumber = 1;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment Segment {
      get { return entryCase_ == EntryOneofCase.Segment ? (global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment) entry_ : null; }
      set {
        entry_ = value;
        entryCase_ = value == null ? EntryOneofCase.None : EntryOneofCase.Segment;
      }
    }

    /// <summary>Field number for the "backward" field.</summary>
    public const int BackwardFieldNumber = 2;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dwango.Nicolive.Chat.Service.Edge.BackwardSegment Backward {
      get { return entryCase_ == EntryOneofCase.Backward ? (global::Dwango.Nicolive.Chat.Service.Edge.BackwardSegment) entry_ : null; }
      set {
        entry_ = value;
        entryCase_ = value == null ? EntryOneofCase.None : EntryOneofCase.Backward;
      }
    }

    /// <summary>Field number for the "previous" field.</summary>
    public const int PreviousFieldNumber = 3;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment Previous {
      get { return entryCase_ == EntryOneofCase.Previous ? (global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment) entry_ : null; }
      set {
        entry_ = value;
        entryCase_ = value == null ? EntryOneofCase.None : EntryOneofCase.Previous;
      }
    }

    /// <summary>Field number for the "next" field.</summary>
    public const int NextFieldNumber = 4;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext Next {
      get { return entryCase_ == EntryOneofCase.Next ? (global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext) entry_ : null; }
      set {
        entry_ = value;
        entryCase_ = value == null ? EntryOneofCase.None : EntryOneofCase.Next;
      }
    }

    private object entry_;
    /// <summary>Enum of possible cases for the "entry" oneof.</summary>
    public enum EntryOneofCase {
      None = 0,
      Segment = 1,
      Backward = 2,
      Previous = 3,
      Next = 4,
    }
    private EntryOneofCase entryCase_ = EntryOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EntryOneofCase EntryCase {
      get { return entryCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearEntry() {
      entryCase_ = EntryOneofCase.None;
      entry_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ChunkedEntry);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ChunkedEntry other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Segment, other.Segment)) return false;
      if (!object.Equals(Backward, other.Backward)) return false;
      if (!object.Equals(Previous, other.Previous)) return false;
      if (!object.Equals(Next, other.Next)) return false;
      if (EntryCase != other.EntryCase) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (entryCase_ == EntryOneofCase.Segment) hash ^= Segment.GetHashCode();
      if (entryCase_ == EntryOneofCase.Backward) hash ^= Backward.GetHashCode();
      if (entryCase_ == EntryOneofCase.Previous) hash ^= Previous.GetHashCode();
      if (entryCase_ == EntryOneofCase.Next) hash ^= Next.GetHashCode();
      hash ^= (int) entryCase_;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (entryCase_ == EntryOneofCase.Segment) {
        output.WriteRawTag(10);
        output.WriteMessage(Segment);
      }
      if (entryCase_ == EntryOneofCase.Backward) {
        output.WriteRawTag(18);
        output.WriteMessage(Backward);
      }
      if (entryCase_ == EntryOneofCase.Previous) {
        output.WriteRawTag(26);
        output.WriteMessage(Previous);
      }
      if (entryCase_ == EntryOneofCase.Next) {
        output.WriteRawTag(34);
        output.WriteMessage(Next);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (entryCase_ == EntryOneofCase.Segment) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Segment);
      }
      if (entryCase_ == EntryOneofCase.Backward) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Backward);
      }
      if (entryCase_ == EntryOneofCase.Previous) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Previous);
      }
      if (entryCase_ == EntryOneofCase.Next) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Next);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ChunkedEntry other) {
      if (other == null) {
        return;
      }
      switch (other.EntryCase) {
        case EntryOneofCase.Segment:
          if (Segment == null) {
            Segment = new global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment();
          }
          Segment.MergeFrom(other.Segment);
          break;
        case EntryOneofCase.Backward:
          if (Backward == null) {
            Backward = new global::Dwango.Nicolive.Chat.Service.Edge.BackwardSegment();
          }
          Backward.MergeFrom(other.Backward);
          break;
        case EntryOneofCase.Previous:
          if (Previous == null) {
            Previous = new global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment();
          }
          Previous.MergeFrom(other.Previous);
          break;
        case EntryOneofCase.Next:
          if (Next == null) {
            Next = new global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext();
          }
          Next.MergeFrom(other.Next);
          break;
      }

      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment subBuilder = new global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment();
            if (entryCase_ == EntryOneofCase.Segment) {
              subBuilder.MergeFrom(Segment);
            }
            input.ReadMessage(subBuilder);
            Segment = subBuilder;
            break;
          }
          case 18: {
            global::Dwango.Nicolive.Chat.Service.Edge.BackwardSegment subBuilder = new global::Dwango.Nicolive.Chat.Service.Edge.BackwardSegment();
            if (entryCase_ == EntryOneofCase.Backward) {
              subBuilder.MergeFrom(Backward);
            }
            input.ReadMessage(subBuilder);
            Backward = subBuilder;
            break;
          }
          case 26: {
            global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment subBuilder = new global::Dwango.Nicolive.Chat.Service.Edge.MessageSegment();
            if (entryCase_ == EntryOneofCase.Previous) {
              subBuilder.MergeFrom(Previous);
            }
            input.ReadMessage(subBuilder);
            Previous = subBuilder;
            break;
          }
          case 34: {
            global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext subBuilder = new global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Types.ReadyForNext();
            if (entryCase_ == EntryOneofCase.Next) {
              subBuilder.MergeFrom(Next);
            }
            input.ReadMessage(subBuilder);
            Next = subBuilder;
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the ChunkedEntry message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      /// <summary>
      ///*
      ///　 * 次のストリームの開始時刻を表すチャンク。必ずストリームの末尾に送られてくる。
      ///　
      /// </summary>
      public sealed partial class ReadyForNext : pb::IMessage<ReadyForNext> {
        private static readonly pb::MessageParser<ReadyForNext> _parser = new pb::MessageParser<ReadyForNext>(() => new ReadyForNext());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<ReadyForNext> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::Dwango.Nicolive.Chat.Service.Edge.ChunkedEntry.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public ReadyForNext() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public ReadyForNext(ReadyForNext other) : this() {
          at_ = other.at_;
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public ReadyForNext Clone() {
          return new ReadyForNext(this);
        }

        /// <summary>Field number for the "at" field.</summary>
        public const int AtFieldNumber = 1;
        private long at_;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public long At {
          get { return at_; }
          set {
            at_ = value;
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other) {
          return Equals(other as ReadyForNext);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(ReadyForNext other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (At != other.At) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode() {
          int hash = 1;
          if (At != 0L) hash ^= At.GetHashCode();
          if (_unknownFields != null) {
            hash ^= _unknownFields.GetHashCode();
          }
          return hash;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override string ToString() {
          return pb::JsonFormatter.ToDiagnosticString(this);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void WriteTo(pb::CodedOutputStream output) {
          if (At != 0L) {
            output.WriteRawTag(8);
            output.WriteInt64(At);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize() {
          int size = 0;
          if (At != 0L) {
            size += 1 + pb::CodedOutputStream.ComputeInt64Size(At);
          }
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(ReadyForNext other) {
          if (other == null) {
            return;
          }
          if (other.At != 0L) {
            At = other.At;
          }
          _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(pb::CodedInputStream input) {
          uint tag;
          while ((tag = input.ReadTag()) != 0) {
            switch(tag) {
              default:
                _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
                break;
              case 8: {
                At = input.ReadInt64();
                break;
              }
            }
          }
        }

      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code