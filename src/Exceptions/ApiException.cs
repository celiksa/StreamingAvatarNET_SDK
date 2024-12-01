using System;
using System.Net;

namespace HeyGen.StreamingAvatar
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string ResponseText { get; }
        public string RequestId { get; }

        public ApiException(string message, HttpStatusCode statusCode, string responseText, string requestId = null)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseText = responseText;
            RequestId = requestId;
        }

        public ApiException(string message, HttpStatusCode statusCode, string responseText, Exception innerException, string requestId = null)
            : base(message, innerException)
        {
            StatusCode = statusCode;
            ResponseText = responseText;
            RequestId = requestId;
        }
    }

    public class WebSocketException : Exception
    {
        public WebSocketException(string message) : base(message)
        {
        }

        public WebSocketException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class StreamingAvatarException : Exception
    {
        public string ErrorCode { get; }

        public StreamingAvatarException(string message, string errorCode = null) : base(message)
        {
            ErrorCode = errorCode;
        }

        public StreamingAvatarException(string message, Exception innerException, string errorCode = null) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }

    public class AudioProcessingException : StreamingAvatarException
    {
        public AudioProcessingException(string message) : base(message, "AUDIO_PROCESSING_ERROR")
        {
        }

        public AudioProcessingException(string message, Exception innerException) 
            : base(message, innerException, "AUDIO_PROCESSING_ERROR")
        {
        }
    }

    public class ConnectionException : StreamingAvatarException
    {
        public ConnectionException(string message) : base(message, "CONNECTION_ERROR")
        {
        }

        public ConnectionException(string message, Exception innerException) 
            : base(message, innerException, "CONNECTION_ERROR")
        {
        }
    }
}