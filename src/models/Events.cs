using System;
using System.Text.Json.Serialization;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace HeyGen.StreamingAvatar
{
    public class StreamingEventArgs : EventArgs
    {
        public WebSocketEvent Event { get; }

        public StreamingEventArgs(WebSocketEvent evt)
        {
            Event = evt;
        }
    }

    public class WebSocketEvent
    {
        [JsonPropertyName("event_type")]
        public string EventType { get; set; }

        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("silence_times")]
        public int? SilenceTimes { get; set; }

        [JsonPropertyName("count_down")]
        public int? CountDown { get; set; }

        [JsonPropertyName("duration_ms")]
        public int? DurationMs { get; set; }
    }

    public class EventHandler<T> where T : EventArgs
    {
        private event EventHandler<T> _handlers;

        public void RegisterHandler(EventHandler<T> handler)
        {
            _handlers += handler;
        }

        public void UnregisterHandler(EventHandler<T> handler)
        {
            _handlers -= handler;
        }

        public void RaiseEvent(object sender, T args)
        {
            _handlers?.Invoke(sender, args);
        }
    }

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

    public class StreamingEvent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("task_id")]
        public string TaskId { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("duration_ms")]
        public int? DurationMs { get; set; }
    }

    public class StreamingStartTalkingEvent : StreamingEvent
    {
        public StreamingStartTalkingEvent()
        {
            Type = StreamingEvents.AvatarStartTalking.ToString();
        }
    }

    public class StreamingStopTalkingEvent : StreamingEvent
    {
        public StreamingStopTalkingEvent()
        {
            Type = StreamingEvents.AvatarStopTalking.ToString();
        }
    }

    public class StreamingTalkingMessageEvent : StreamingEvent
    {
        public StreamingTalkingMessageEvent()
        {
            Type = StreamingEvents.AvatarTalkingMessage.ToString();
        }
    }

    public class StreamingTalkingEndEvent : StreamingEvent
    {
        public StreamingTalkingEndEvent()
        {
            Type = StreamingEvents.AvatarEndMessage.ToString();
        }
    }

    public class UserTalkingMessageEvent : StreamingEvent
    {
        public UserTalkingMessageEvent()
        {
            Type = StreamingEvents.UserTalkingMessage.ToString();
        }
    }

    public class UserTalkingEndEvent : StreamingEvent
    {
        public UserTalkingEndEvent()
        {
            Type = StreamingEvents.UserEndMessage.ToString();
        }
    }

    public class UserStartTalkingEvent : WebSocketEvent
    {
        public UserStartTalkingEvent()
        {
            EventType = StreamingEvents.UserStart.ToString();
        }
    }

    public class UserStopTalkingEvent : WebSocketEvent
    {
        public UserStopTalkingEvent()
        {
            EventType = StreamingEvents.UserStop.ToString();
        }
    }

    public class UserSilenceEvent : WebSocketEvent
    {
        public UserSilenceEvent()
        {
            EventType = StreamingEvents.UserSilence.ToString();
        }
    }
}