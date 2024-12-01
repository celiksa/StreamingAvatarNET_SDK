using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace HeyGen.StreamingAvatar
{
    public sealed class AudioRawFrame : IMessage<AudioRawFrame>
    {
        private static readonly MessageParser<AudioRawFrame> _parser = new MessageParser<AudioRawFrame>(() => new AudioRawFrame());

        private ulong id_;
        private string name_ = "";
        private ByteString audio_ = ByteString.Empty;
        private uint sampleRate_;
        private uint numChannels_;

        public static MessageParser<AudioRawFrame> Parser => _parser;

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

        public ByteString Audio
        {
            get => audio_;
            set => audio_ = value ?? ByteString.Empty;
        }

        public uint SampleRate
        {
            get => sampleRate_;
            set => sampleRate_ = value;
        }

        public uint NumChannels
        {
            get => numChannels_;
            set => numChannels_ = value;
        }

        public void MergeFrom(AudioRawFrame other)
        {
            if (other == null) return;

            if (other.Id != 0UL)
                Id = other.Id;
            if (other.Name.Length != 0)
                Name = other.Name;
            if (other.Audio.Length != 0)
                Audio = other.Audio;
            if (other.SampleRate != 0)
                SampleRate = other.SampleRate;
            if (other.NumChannels != 0)
                NumChannels = other.NumChannels;
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
            if (Audio.Length != 0)
            {
                output.WriteRawTag(26);
                output.WriteBytes(Audio);
            }
            if (SampleRate != 0)
            {
                output.WriteRawTag(32);
                output.WriteUInt32(SampleRate);
            }
            if (NumChannels != 0)
            {
                output.WriteRawTag(40);
                output.WriteUInt32(NumChannels);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (Id != 0UL)
                size += 1 + CodedOutputStream.ComputeUInt64Size(Id);
            if (Name.Length != 0)
                size += 1 + CodedOutputStream.ComputeStringSize(Name);
            if (Audio.Length != 0)
                size += 1 + CodedOutputStream.ComputeBytesSize(Audio);
            if (SampleRate != 0)
                size += 1 + CodedOutputStream.ComputeUInt32Size(SampleRate);
            if (NumChannels != 0)
                size += 1 + CodedOutputStream.ComputeUInt32Size(NumChannels);
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
                        Audio = input.ReadBytes();
                        break;
                    case 32:
                        SampleRate = input.ReadUInt32();
                        break;
                    case 40:
                        NumChannels = input.ReadUInt32();
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }
    }
}