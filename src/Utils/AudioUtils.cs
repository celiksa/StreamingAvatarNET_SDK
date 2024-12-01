using System;
using NAudio.Wave;

namespace HeyGen.StreamingAvatar
{
    internal static class AudioUtils
    {
        public static byte[] ConvertFloat32ToS16PCM(float[] floatSamples)
        {
            var pcm16Samples = new byte[floatSamples.Length * 2];
            
            for (int i = 0; i < floatSamples.Length; i++)
            {
                // Clamp the float sample between -1 and 1
                float clampedValue = Math.Max(-1.0f, Math.Min(1.0f, floatSamples[i]));
                
                // Convert to 16-bit PCM
                short pcm16Value = (short)(clampedValue < 0 
                    ? clampedValue * 32768 
                    : clampedValue * 32767);
                
                // Convert to bytes (little-endian)
                pcm16Samples[i * 2] = (byte)(pcm16Value & 0xFF);
                pcm16Samples[i * 2 + 1] = (byte)((pcm16Value >> 8) & 0xFF);
            }
            
            return pcm16Samples;
        }

        public static float[] ConvertByteArrayToFloat32(byte[] input)
        {
            var waveFormat = new WaveFormat(16000, 16, 1);
            using var provider = new RawSourceWaveStream(new MemoryStream(input), waveFormat);
            var samples = new float[input.Length / 2];
            
            using (var reader = new WaveFileReader(provider))
            {
                int samplesRead = reader.Read(samples, 0, samples.Length);
                if (samplesRead < samples.Length)
                {
                    Array.Resize(ref samples, samplesRead);
                }
            }
            
            return samples;
        }

        public static byte[] ResampleAudio(byte[] audioData, int sourceSampleRate, int targetSampleRate)
        {
            using var sourceStream = new MemoryStream(audioData);
            using var sourceReader = new RawSourceWaveStream(sourceStream, new WaveFormat(sourceSampleRate, 16, 1));
            using var resampler = new MediaFoundationResampler(sourceReader, new WaveFormat(targetSampleRate, 16, 1));
            using var outputStream = new MemoryStream();
            
            resampler.ResamplerQuality = 60;
            var buffer = new byte[16384];
            int read;
            
            while ((read = resampler.Read(buffer, 0, buffer.Length)) > 0)
            {
                outputStream.Write(buffer, 0, read);
            }
            
            return outputStream.ToArray();
        }
    }
}