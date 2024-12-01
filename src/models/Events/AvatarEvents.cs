using System.Text.Json.Serialization;

namespace HeyGen.StreamingAvatar
{
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
}