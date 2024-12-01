using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace HeyGen.StreamingAvatar
{
    public class Frame : IMessage<Frame>
    {
        private static readonly MessageParser<Frame> _parser = new MessageParser<Frame>(() => new Frame());
        
        private FrameOneofCase frameCase_ = FrameOneofCase.None;
        private object frame_;

        public enum FrameOneofCase
        {
            None = 0,
            Text = 1,
            Audio = 2,
            Transcription = 3
        }

        public static MessageParser<Frame> Parser => _parser;

        public TextFrame Text
        {
            get => frameCase_ == FrameOneofCase.Text ? (TextFrame)frame_ : null;
            set
            {
                frame_ = value;
                frameCase_ = value == null ? FrameOneofCase.None : FrameOneofCase.Text;
            }
        }

        public AudioRawFrame Audio
        {
            get => frameCase_ == FrameOneofCase.Audio ? (AudioRawFrame)frame_ : null;
            set
            {
                frame_ = value;
                frameCase_ = value == null ? FrameOneofCase.None : FrameOneofCase.Audio;
            }
        }

        public TranscriptionFrame Transcription
        {
            get => frameCase_ == FrameOneofCase.Transcription ? (TranscriptionFrame)frame_ : null;
            set
            {
                frame_ = value;
                frameCase_ = value == null ? FrameOneofCase.None : FrameOneofCase.Transcription;
            }
        }

        public FrameOneofCase FrameCase => frameCase_;

        public void ClearFrame()
        {
            frameCase_ = FrameOneofCase.None;
            frame_ = null;
        }

        public void MergeFrom(Frame other)
        {
            if (other == null) return;

            switch (other.FrameCase)
            {
                case FrameOneofCase.Text:
                    if (Text == null)
                        Text = new TextFrame();
                    Text.MergeFrom(other.Text);
                    break;
                case FrameOneofCase.Audio:
                    if (Audio == null)
                        Audio = new AudioRawFrame();
                    Audio.MergeFrom(other.Audio);
                    break;
                case FrameOneofCase.Transcription:
                    if (Transcription == null)
                        Transcription = new TranscriptionFrame();
                    Transcription.MergeFrom(other.Transcription);
                    break;
            }
        }

        public void WriteTo(CodedOutputStream output)
        {
            if (frameCase_ == FrameOneofCase.Text)
            {
                output.WriteRawTag(10);
                output.WriteMessage(Text);
            }
            if (frameCase_ == FrameOneofCase.Audio)
            {
                output.WriteRawTag(18);
                output.WriteMessage(Audio);
            }
            if (frameCase_ == FrameOneofCase.Transcription)
            {
                output.WriteRawTag(26);
                output.WriteMessage(Transcription);
            }
        }

        public int CalculateSize()
        {
            int size = 0;
            if (frameCase_ == FrameOneofCase.Text)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(Text);
            }
            if (frameCase_ == FrameOneofCase.Audio)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(Audio);
            }
            if (frameCase_ == FrameOneofCase.Transcription)
            {
                size += 1 + CodedOutputStream.ComputeMessageSize(Transcription);
            }
            return size;
        }

        public void MergeFrom(CodedInputStream input)
        {
            while (input.ReadTag() is uint tag)
            {
                switch (tag)
                {
                    case 10: // Text
                        var text = new TextFrame();
                        input.ReadMessage(text);
                        Text = text;
                        break;
                    case 18: // Audio
                        var audio = new AudioRawFrame();
                        input.ReadMessage(audio);
                        Audio = audio;
                        break;
                    case 26: // Transcription
                        var transcription = new TranscriptionFrame();
                        input.ReadMessage(transcription);
                        Transcription = transcription;
                        break;
                    default:
                        input.SkipLastField();
                        break;
                }
            }
        }

        public void MergeFrom(byte[] data)
        {
            MergeFrom(new CodedInputStream(data));
        }

        public byte[] ToByteArray()
        {
            var output = new CodedOutputStream(new byte[CalculateSize()]);
            WriteTo(output);
            return output.ToByteArray();
        }
    }
}