using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Administartion;
using Shared.Models.User;

namespace Logic.Administration
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly ILogger<UserAuthentication> _logger;
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtTokenModel _jwtTokenModel;

        public UserAuthentication(
            ILogger<UserAuthentication> logger,
            IUserUnitOfWork userUnitOfWork,
            IJwtTokenService jwtTokenService,
            IOptions<JwtTokenModel> options)
        {
            _logger = logger;
            _userUnitOfWork = userUnitOfWork;
            _jwtTokenService = jwtTokenService;
            _jwtTokenModel = options.Value;
        }

        public async Task<TokenResponse?> AuthenticateUserAsync(AuthenticationRequestModel request)
        {
            try
            {
                var user = await _userUnitOfWork.GetUserByEmail(request.EmailAddress ?? string.Empty, true, false);

                if (user == null || user.Credentials == null)
                {
                    return null;
                }

                if (!PasswordHasher.VerifyPassword(request.Password ?? string.Empty, user?.Credentials?.PasswordHash ?? string.Empty))
                {
                    return null;
                }

                var (jwt, refreshToken) = _jwtTokenService.GenerateTokens(user);

                user.Credentials.RefreshToken = refreshToken;

                await _userUnitOfWork.UpdateUser(user, true);

                var tokenResponse = new TokenResponse
                {
                    Jwt = jwt,
                    RefreshToken = refreshToken
                };

                return tokenResponse;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync($"Error authenticating user with email: {request.EmailAddress}", LogMessageTypeEnum.Error, exception);

                return null;
            }
        }

        public async Task<TokenResponse?> RefreshTokenAsync(TokenResponse request)
        {
            try
            {
                var tokenResponse = await _jwtTokenService.RefreshToken(request, _userUnitOfWork);
                return tokenResponse;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync($"Error refreshing token for jwt: {request.Jwt}", LogMessageTypeEnum.Error, exception);
                return null;
            }
        }

        public async Task<bool> ActivateAccountAsync(AccountActivationModel model)
        {
            try
            {
                var user = await _userUnitOfWork.GetUserById(model.UserId, true, false);

                if (user == null || user.IsActive || user.Credentials == null || user.EmailAddress != model.EmailAddress)
                {
                    return false;
                }

                user.IsActive = true;
                user.Credentials.PasswordHash = PasswordHasher.HashPassword(model.Password);

                await _userUnitOfWork.UpdateUser(user, true);
                await _userUnitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync($"Error activating account for email: {model.EmailAddress}", LogMessageTypeEnum.Error, exception);
                return false;
            }
        }

    }
}
