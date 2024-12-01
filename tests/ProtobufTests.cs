using System;
using Google.Protobuf;
using Xunit;

namespace HeyGen.StreamingAvatar.Tests
{
    public class ProtobufTests
    {
        [Fact]
        public void Frame_WithTextMessage_SerializesAndDeserializesCorrectly()
        {
            // Arrange
            var originalFrame = new Frame
            {
                Text = new TextFrame
                {
                    Id = 1,
                    Name = "test",
                    Text = "Hello, World!"
                }
            };

            // Act
            byte[] serialized = originalFrame.ToByteArray();
            var deserializedFrame = new Frame();
            deserializedFrame.MergeFrom(serialized);

            // Assert
            Assert.Equal(FrameOneofCase.Text, deserializedFrame.FrameCase);
            Assert.Equal(originalFrame.Text.Id, deserializedFrame.Text.Id);
            Assert.Equal(originalFrame.Text.Name, deserializedFrame.Text.Name);
            Assert.Equal(originalFrame.Text.Text, deserializedFrame.Text.Text);
        }

        [Fact]
        public void Frame_WithAudioMessage_SerializesAndDeserializesCorrectly()
        {
            // Arrange
            var audioData = new byte[] { 1, 2, 3, 4 };
            var originalFrame = new Frame
            {
                Audio = new AudioRawFrame
                {
                    Id = 1,
                    Name = "audio",
                    Audio = ByteString.CopyFrom(audioData),
                    SampleRate = 16000,
                    NumChannels = 1
                }
            };

            // Act
            byte[] serialized = originalFrame.ToByteArray();
            var deserializedFrame = new Frame();
            deserializedFrame.MergeFrom(serialized);

            // Assert
            Assert.Equal(FrameOneofCase.Audio, deserializedFrame.FrameCase);
            Assert.Equal(originalFrame.Audio.Id, deserializedFrame.Audio.Id);
            Assert.Equal(originalFrame.Audio.Name, deserializedFrame.Audio.Name);
            Assert.Equal(originalFrame.Audio.Audio, deserializedFrame.Audio.Audio);
            Assert.Equal(originalFrame.Audio.SampleRate, deserializedFrame.Audio.SampleRate);
            Assert.Equal(originalFrame.Audio.NumChannels, deserializedFrame.Audio.NumChannels);
        }

        [Fact]
        public void Frame_WithTranscriptionMessage_SerializesAndDeserializesCorrectly()
        {
            // Arrange
            var originalFrame = new Frame
            {
                Transcription = new TranscriptionFrame
                {
                    Id = 1,
                    Name = "transcription",
                    Text = "Hello, World!",
                    UserId = "user123",
                    Timestamp = DateTime.UtcNow.ToString("O")
                }
            };

            // Act
            byte[] serialized = originalFrame.ToByteArray();
            var deserializedFrame = new Frame();
            deserializedFrame.MergeFrom(serialized);

            // Assert
            Assert.Equal(FrameOneofCase.Transcription, deserializedFrame.FrameCase);
            Assert.Equal(originalFrame.Transcription.Id, deserializedFrame.Transcription.Id);
            Assert.Equal(originalFrame.Transcription.Name, deserializedFrame.Transcription.Name);
            Assert.Equal(originalFrame.Transcription.Text, deserializedFrame.Transcription.Text);
            Assert.Equal(originalFrame.Transcription.UserId, deserializedFrame.Transcription.UserId);
            Assert.Equal(originalFrame.Transcription.Timestamp, deserializedFrame.Transcription.Timestamp);
        }
    }
}