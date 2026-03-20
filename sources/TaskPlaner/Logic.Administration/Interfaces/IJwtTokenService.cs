using Logic.Shared.Interfaces;
using Shared.Models.Administartion;
using Shared.Models.User;

namespace Logic.Administration.Interfaces
{
    public interface IJwtTokenService
    {
        (string Jwt, string RefreshToken) GenerateTokens(UserModel user);
        Task<TokenResponse> RefreshToken(TokenResponse request, IUserUnitOfWork unitOfWork);
        int GetJwtExpireSeconds();
        JwtTokenModel GetJwtOptions();
    }
}
