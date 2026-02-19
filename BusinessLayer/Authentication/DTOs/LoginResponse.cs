namespace BusinessLayer.Authentication.DTOs
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string Message {  get; set; }
        public DateTime ExpiresAt {  get; set; }
    }
}
