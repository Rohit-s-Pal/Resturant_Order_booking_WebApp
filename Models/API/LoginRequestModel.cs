namespace Estonia.Models.API
{
    public class LoginRequestModel
    {
        public string? UserName { get; set; }
        public string? AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        
    }
}
