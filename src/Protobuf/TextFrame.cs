using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace HeyGen.StreamingAvatar
{
    public sealed class TextFrame : IMessage<TextFrame>
    {
        private static readonly MessageParser<TextFrame> _parser = new MessageParser<TextFrame>(() => new TextFrame());
        
        private ulong id_;
        private string name_ = "";
        private string text_ = "";

        public static MessageParser<TextFrame> Parser => _parser;

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

        public void MergeFrom(TextFrame other)
        {
            if (other == null) return;

            if (other.Id != 0UL)
                Id = other.Id;
            if (other.Name.Length != 0)
                Name = other.Name;
            if (other.Text.Length != 0)
                Text = other.Text;
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
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}