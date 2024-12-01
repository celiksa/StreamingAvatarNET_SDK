namespace HeyGen.StreamingAvatar
{
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