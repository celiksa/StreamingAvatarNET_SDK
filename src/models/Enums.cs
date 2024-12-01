using System;

namespace HeyGen.StreamingAvatar
{
    public enum AvatarQuality
    {
        Low,
        Medium,
        High
    }

    public enum VoiceEmotion
    {
        Excited,
        Serious,
        Friendly,
        Soothing,
        Broadcaster
    }

    public enum TaskType
    {
        Talk,
        Repeat
    }

    public enum TaskMode
    {
        Sync,
        Async
    }

    public enum StreamingEvents
    {
        AvatarStartTalking,
        AvatarStopTalking,
        AvatarTalkingMessage,
        AvatarEndMessage,
        UserTalkingMessage,
        UserEndMessage,
        UserStart,
        UserStop,
        UserSilence,
        StreamReady,
        StreamDisconnected
    }
}