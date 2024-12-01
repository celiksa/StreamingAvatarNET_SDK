using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace HeyGen.StreamingAvatar
{
    public sealed class TranscriptionFrame : IMessage<TranscriptionFrame>
    {
        private static readonly MessageParser<TranscriptionFrame> _parser = new MessageParser<TranscriptionFrame>(() => new TranscriptionFrame());

        private ulong id_;
        private string name_ = "";
        private string text_ = "";
        private string userId_ = "";
        private string timestamp_ = "";

        public static MessageParser<TranscriptionFrame> Parser => _parser;

        public ulong Id
        {
            get => id_;
            set => id_ = value;
        }

        public string Name
        {
            get => name_;
            set => name_ = value ?? "";
        }

        public string Text
        {
            get => text_;
            set => text_ = value ?? "";
        }

        public string UserId
        {
            get => userId_;
            set => userId_ = value ?? "";
        }

        public string Timestamp
        {
            get => timestamp_;
            set => timestamp_ = value ?? "";
        }

        public void MergeFrom(TranscriptionFrame other)
        {
            if (other == null) return;

            if (other.Id != 0UL)
                Id = other.Id;
            if (other.Name.Length != 0)
                Name = other.Name;
            if (other.Text.Length != 0)
                Text = other.Text;
            if (other.UserId.Length != 0)
                UserId = other.UserId;
            if (other.Timestamp.Length != 0)
                Timestamp = other.Timestamp;
        }

        public void WriteTo(CodedOutputStream output)
        {
            if (Id != 0UL)
            {
                output.WriteRawTag(8);
                output.WriteUInt64(Id);
            }
            if (Name.Length != 0)
            {
                output.WriteRawTag(18);
                output.WriteString(Name);
            }
            if (Text.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteString(Text);
            }
            if (UserId.Length != 0)
            {
                output.WriteRawTag(34);
                output.WriteString(UserId);
            }
            if (Timestamp.Length != 0)
            {
                output.WriteRawTag(42);
                output.WriteString(Timestamp);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (Id != 0UL)
                size += 1 + CodedOutputStream.ComputeUInt64Size(Id);
            if (Name.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Name);
            if (Text.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Text);
            if (UserId.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(UserId);
            if (Timestamp.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Timestamp);
            return size;
        }

        public void MergeFrom(CodedInputStream input)
        {
            while (input.ReadTag() is uint tag)
            {
                switch (tag)
                {
                    case 8:
                        Id = input.ReadUInt64();
                        break;
                    case 18:
                        Name = input.ReadString();
                        break;
                    case 26:
                        Text = input.ReadString();
                        break;
                    case 34:
                        UserId = input.ReadString();
                        break;
                    case 42:
                        Timestamp = input.ReadString();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}