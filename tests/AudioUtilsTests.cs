using System;
using Xunit;

namespace HeyGen.StreamingAvatar.Tests
{
    public class AudioUtilsTests
    {
        [Fact]
        public void ConvertFloat32ToS16PCM_WithValidInput_ReturnsCorrectByteArray()
        {
            // Arrange
            var floatSamples = new float[] { 0.5f, -0.5f, 0.0f, 1.0f, -1.0f };

            // Act
            var result = AudioUtils.ConvertFloat32ToS16PCM(floatSamples);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(floatSamples.Length * 2, result.Length); // 16-bit = 2 bytes per sample
        }

        [Fact]
        public void ConvertByteArrayToFloat32_WithValidInput_ReturnsCorrectFloatArray()
        {
            // Arrange
            var pcmData = new byte[] { 0, 0, 255, 127, 0, 128 }; // 16-bit PCM samples

            // Act
            var result = AudioUtils.ConvertByteArrayToFloat32(pcmData);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pcmData.Length / 2, result.Length); // 2 bytes per 16-bit sample
        }

        [Fact]
        public void ResampleAudio_WithValidInput_ReturnsResampledAudio()
        {
            // Arrange
            var sourceRate = 44100;
            var targetRate = 16000;
            var audioData = new byte[44100 * 2]; // 1 second of 16-bit audio at 44.1kHz

            // Act
            var result = AudioUtils.ResampleAudio(audioData, sourceRate, targetRate);

            // Assert
            Assert.NotNull(result);
            // Check expected length based on resampling ratio
            var expectedLength = (int)(audioData.Length * ((double)targetRate / sourceRate));
            Assert.Equal(expectedLength, result.Length);
        }
    }
}