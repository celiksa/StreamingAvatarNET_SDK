using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HeyGen.StreamingAvatar.Tests
{
    public class StreamingAvatarClientTests
    {
        private readonly Mock<ILogger<StreamingAvatarClient>> _loggerMock;
        private readonly Mock<HttpMessageHandler> _httpHandlerMock;
        private const string TestToken = "test_token";

        public StreamingAvatarClientTests()
        {
            _loggerMock = new Mock<ILogger<StreamingAvatarClient>>();
            _httpHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task CreateStartAvatar_WithValidRequest_ReturnsSessionInfo()
        {
            // Arrange
            var client = CreateClient();
            var request = new StartAvatarRequest
            {
                AvatarName = "test_avatar",
                Quality = AvatarQuality.Medium,
                Voice = new VoiceSettings
                {
                    VoiceId = "test_voice",
                    Rate = 1.0f,
                    Emotion = VoiceEmotion.Friendly
                }
            };

            // Act
            var result = await client.CreateStartAvatarAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.SessionId);
            Assert.NotNull(result.AccessToken);
        }

        [Fact]
        public async Task Speak_WithValidText_SendsRequest()
        {
            // Arrange
            var client = CreateClient();
            var request = new SpeakRequest
            {
                Text = "Hello, world!",
                TaskType = TaskType.Talk,
                TaskMode = TaskMode.Async
            };

            // Act
            await client.SpeakAsync(request);

            // Assert
            // Verify HTTP request was made with correct parameters
        }

        [Fact]
        public async Task StartVoiceChat_InitializesAudioCapture()
        {
            // Arrange
            var client = CreateClient();

            // Act
            await client.StartVoiceChatAsync();

            // Assert
            // Verify audio capture was initialized
        }

        private StreamingAvatarClient CreateClient()
        {
            var httpClient = new HttpClient(_httpHandlerMock.Object)
            {
                BaseAddress = new Uri("https://api.heygen.com")
            };

            return new StreamingAvatarClient(TestToken, logger: _loggerMock.Object);
        }
    }
}