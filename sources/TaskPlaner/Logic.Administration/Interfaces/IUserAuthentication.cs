using Shared.Models.Administartion;

namespace Logic.Administration.Interfaces
{
    public interface IUserAuthentication
    {
        Task<TokenResponse?> AuthenticateUserAsync(AuthenticationRequestModel request);
        Task<TokenResponse?> RefreshTokenAsync(TokenResponse request);
    }
}
