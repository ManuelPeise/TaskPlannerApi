namespace Shared.Models.Administartion
{
    public class TokenResponse
    {
        public string? Jwt { get; set; }
        public string? RefreshToken { get; set; }
    }
}
