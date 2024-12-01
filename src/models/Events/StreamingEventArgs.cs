using System;
using System.Text.Json.Serialization;

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
}