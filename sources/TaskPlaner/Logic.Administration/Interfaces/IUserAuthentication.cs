using Shared.Models.Administartion;
using Shared.Models.User;

namespace Logic.Administration.Interfaces
{
    public interface IUserAuthentication
    {
        Task<TokenResponse?> AuthenticateUserAsync(AuthenticationRequestModel request);
        Task<TokenResponse?> RefreshTokenAsync(TokenResponse request);
        Task<bool> ActivateAccountAsync(AccountActivationModel model);
    }
}
