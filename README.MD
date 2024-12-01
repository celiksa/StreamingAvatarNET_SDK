# HeyGen Streaming Avatar SDK for .NET

.NET SDK for HeyGen's Interactive Avatar API, enabling real-time streaming and voice chat capabilities.

## Installation

Install via NuGet:

```bash
dotnet add package HeyGen.StreamingAvatar
```

## Features

- Interactive Avatar Streaming
- Real-time Voice Chat
- Text-to-Speech with Emotion Control
- WebRTC Integration
- Event-based Communication
- Configurable Quality Settings
- Knowledge Base Integration

## Quick Start

```csharp
using HeyGen.StreamingAvatar;
using Microsoft.Extensions.Logging;

// Create client
var client = new StreamingAvatarClient(
    token: "YOUR_ACCESS_TOKEN",
    logger: loggerFactory.CreateLogger<StreamingAvatarClient>()
);

// Subscribe to events
client.OnEvent += (sender, e) => {
    switch (e.Event.EventType)
    {
        case nameof(StreamingEvents.AvatarStartTalking):
            Console.WriteLine("Avatar started talking");
            break;
        case nameof(StreamingEvents.AvatarStopTalking):
            Console.WriteLine("Avatar stopped talking");
            break;
    }
};

// Start avatar session
var response = await client.CreateStartAvatarAsync(new StartAvatarRequest
{
    Quality = AvatarQuality.Medium,
    AvatarName = "avatar_id",
    Voice = new VoiceSettings 
    { 
        VoiceId = "voice_id",
        Rate = 1.0f,
        Emotion = VoiceEmotion.Friendly
    },
    Language = "en"
});

// Start voice chat
await client.StartVoiceChatAsync(useSilencePrompt: true);

// Send text to speak
await client.SpeakAsync(new SpeakRequest 
{ 
    Text = "Hello, how can I help you?",
    TaskType = TaskType.Talk
});

// Clean up
await client.StopAvatarAsync();
```

## Getting Started

### Prerequisites

- .NET 6.0 or later
- HeyGen API Access Token

### Getting an Access Token

To generate your access token, you need:
1. Access to your API key (Enterprise customers only)
2. Navigate to: https://app.heygen.com/settings?nav=API
3. Generate access token using:

```bash
curl -X POST https://api.heygen.com/v1/streaming.create_token -H "x-api-key: <api-key>"
```

Note: Tokens are one-time use.

### Available Avatars

- Public Interactive Avatars: Find IDs at labs.heygen.com/interactive-avatar
- Custom Avatars: Create at labs.heygen.com/interactive-avatar

## Advanced Usage

### Voice Chat Configuration

```csharp
// Start voice chat with silence detection
await client.StartVoiceChatAsync(new VoiceChatOptions
{
    UseSilencePrompt = true,
    SampleRate = 16000,
    NumChannels = 1
});
```

### Custom Knowledge Base

```csharp
var response = await client.CreateStartAvatarAsync(new StartAvatarRequest
{
    AvatarName = "avatar_id",
    KnowledgeId = "knowledge_base_id",  // From labs.heygen.com
    KnowledgeBase = "custom content",   // Your custom prompt
    Language = "en"
});
```

### Event Handling

```csharp
client.OnEvent += (sender, e) =>
{
    switch (e.Event.EventType)
    {
        case nameof(StreamingEvents.StreamReady):
            Console.WriteLine("Stream ready for interaction");
            break;
        case nameof(StreamingEvents.UserSilence):
            var silenceEvent = e.Event as UserSilenceEvent;
            Console.WriteLine($"Silence detected: {silenceEvent.SilenceTimes} times");
            break;
    }
};
```

## API Documentation

### Main Classes

- `StreamingAvatarClient`: Main client for API interaction
- `StartAvatarRequest`: Configuration for avatar session
- `SpeakRequest`: Text-to-speech request
- `VoiceSettings`: Voice configuration

### Events

- `AvatarStartTalking`: Avatar begins speaking
- `AvatarStopTalking`: Avatar stops speaking
- `UserTalkingMessage`: User speech detected
- `StreamReady`: Stream is ready for interaction
- `UserSilence`: Silence detected in voice chat

### Quality Settings

```csharp
public enum AvatarQuality
{
    Low,
    Medium,
    High
}
```

### Voice Emotions

```csharp
public enum VoiceEmotion
{
    Excited,
    Serious,
    Friendly,
    Soothing,
    Broadcaster
}
```

## Troubleshooting

### Session Limits

- Trial tokens allow 3 concurrent sessions
- Close unused sessions to avoid limits
- Check active sessions: https://docs.heygen.com/reference/list-sessions

### Common Issues

1. **Connection Errors**
   - Check token validity
   - Verify internet connection
   - Ensure WebRTC ports are open

2. **Audio Issues**
   - Check microphone permissions
   - Verify audio input device
   - Ensure correct sample rate (16kHz)

3. **Performance Issues**
   - Lower quality setting if experiencing lag
   - Check network bandwidth
   - Monitor CPU usage

## License

MIT License - see LICENSE file for details.

## Support

- Documentation: https://docs.heygen.com/
- Issues: https://github.com/HeyGen-Official/StreamingAvatarSDK/issues
- Enterprise Support: Contact HeyGen directly