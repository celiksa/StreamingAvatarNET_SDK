using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sharprtc;
using Sharprtc.Peer;
using Sharprtc.MediaSoup;

namespace HeyGen.StreamingAvatar
{
    internal class WebRTCHelper : IDisposable
    {
        private readonly ILogger<WebRTCHelper> _logger;
        private RTCPeerConnection _peerConnection;
        private MediaStream _mediaStream;
        private bool _disposed;

        public event EventHandler<MediaStreamEventArgs> OnMediaStreamReady;
        public event EventHandler<RTCTrackEventArgs> OnTrackAdded;
        public event EventHandler<RTCTrackEventArgs> OnTrackRemoved;
        public event EventHandler<string> OnConnectionStateChanged;

        public WebRTCHelper(ILogger<WebRTCHelper> logger = null)
        {
            _logger = logger;
        }

        public async Task InitializeAsync(string url, string token)
        {
            try
            {
                var config = new RTCConfiguration
                {
                    IceServers = new[]
                    {
                        new RTCIceServer
                        {
                            Urls = new[] { "stun:stun.l.google.com:19302" }
                        }
                    }
                };

                _peerConnection = new RTCPeerConnection(config);
                _mediaStream = new MediaStream();

                SetupEventHandlers();

                await ConnectToSignalingServerAsync(url, token);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to initialize WebRTC connection");
                throw new ConnectionException("Failed to initialize WebRTC connection", ex);
            }
        }

        private void SetupEventHandlers()
        {
            _peerConnection.OnTrack += (sender, e) =>
            {
                if (e.Track.Kind == "video" || e.Track.Kind == "audio")
                {
                    _mediaStream.AddTrack(e.Track);

                    OnTrackAdded?.Invoke(this, e);

                    if (_mediaStream.GetVideoTracks().Length > 0 && 
                        _mediaStream.GetAudioTracks().Length > 0)
                    {
                        OnMediaStreamReady?.Invoke(this, new MediaStreamEventArgs(_mediaStream));
                    }
                }
            };

            _peerConnection.OnConnectionStateChange += (sender, e) =>
            {
                OnConnectionStateChanged?.Invoke(this, e.State.ToString());
            };

            _peerConnection.OnIceConnectionStateChange += (sender, e) =>
            {
                _logger?.LogInformation($"ICE Connection State changed to: {e.State}");
            };
        }

        private async Task ConnectToSignalingServerAsync(string url, string token)
        {
            // Implementation would depend on the specific signaling server protocol
            // This is a placeholder for the actual implementation
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _peerConnection?.Dispose();
            _mediaStream?.Dispose();

            _disposed = true;
        }
    }

    public class MediaStreamEventArgs : EventArgs
    {
        public MediaStream MediaStream { get; }

        public MediaStreamEventArgs(MediaStream mediaStream)
        {
            MediaStream = mediaStream;
        }
    }
}