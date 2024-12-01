using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HeyGen.StreamingAvatar.Examples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            // Setup logging
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .SetMinimumLevel(LogLevel.Debug);
            });

            var logger = loggerFactory.CreateLogger<StreamingAvatarClient>();

            // Get token from configuration
            var token = config["HeyGen:Token"] ?? throw new Exception("Token not found in configuration");

            // Create client
            var client = new StreamingAvatarClient(token, logger: logger);

            try
            {
                // Subscribe to events
                client.OnEvent += (sender, e) =>
                {
                    switch (e.Event.EventType)
                    {
                        case nameof(StreamingEvents.AvatarStartTalking):
                            Console.WriteLine("Avatar started talking");
                            break;
                        case nameof(StreamingEvents.AvatarStopTalking):
                            Console.WriteLine("Avatar stopped talking");
                            break;
                        case nameof(StreamingEvents.StreamReady):
                            Console.WriteLine("Stream is ready");
                            break;
                    }
                };

                // Start avatar session
                Console.WriteLine("Starting avatar session...");
                var response = await client.CreateStartAvatarAsync(new StartAvatarRequest
                {
                    Quality = AvatarQuality.Medium,
                    AvatarName = "avatar_id", // Replace with your avatar ID
                    Voice = new VoiceSettings
                    {
                        VoiceId = "voice_id", // Replace with your voice ID
                        Rate = 1.0f,
                        Emotion = VoiceEmotion.Friendly
                    },
                    Language = "en"
                });

                Console.WriteLine($"Session created with ID: {response.SessionId}");

                // Start voice chat
                Console.WriteLine("Starting voice chat...");
                await client.StartVoiceChatAsync(useSilencePrompt: true);

                // Send some text to speak
                Console.WriteLine("Sending text to speak...");
                await client.SpeakAsync(new SpeakRequest
                {
                    Text = "Hello! How can I help you today?",
                    TaskType = TaskType.Talk
                });

                // Wait for user input
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();

                // Clean up
                await client.StopAvatarAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex is ApiException apiEx)
                {
                    Console.WriteLine($"Status Code: {apiEx.StatusCode}");
                    Console.WriteLine($"Response: {apiEx.ResponseText}");
                }
            }
        }
    }
}