namespace HeyGen.StreamingAvatar
{
    public class StartAvatarResponse
    {
        public string SessionId { get; set; }
        public string AccessToken { get; set; }
        public string Url { get; set; }
        public bool IsPaid { get; set; }
        public int SessionDurationLimit { get; set; }
    }

    internal class ApiResponse<T>
    {
        public T Data { get; set; }
    }
}