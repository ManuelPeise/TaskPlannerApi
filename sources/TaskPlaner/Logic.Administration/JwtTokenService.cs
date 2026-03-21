using Logic.Administration.Interfaces;
using Logic.Shared.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Models.Administartion;
using Shared.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Logic.Administration
{
    internal class JwtTokenService: IJwtTokenService
    {
        private readonly JwtTokenModel _jwtTokenModel;
    
        public JwtTokenService(IOptions<JwtTokenModel> options)  
        {
            _jwtTokenModel = options.Value;
        }

        public (string Jwt, string RefreshToken) GenerateTokens(UserModel user)
        {
            return (GenerateJwt(user), GenerateRefreshToken());
        }

        public async Task<TokenResponse> RefreshToken(TokenResponse request, IUserUnitOfWork unitOfWork)
        {
            var principal = GetPrincipalFromExpiredToken(request.Jwt);
         
            var email = principal.Identity!.Name ?? string.Empty;

            var user = await unitOfWork.GetUserByEmail(email, true);

            if (user == null || user?.Credentials == null || user.Credentials.RefreshToken != request.RefreshToken)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var newAccessToken = GenerateJwt(user);
            var newRefreshToken = GenerateRefreshToken();

            user.Credentials.RefreshToken = newRefreshToken;

            await unitOfWork.UpdateUser(user, true);

            await unitOfWork.SaveChangesAsync();

            return new TokenResponse
            {
                Jwt = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        public int GetJwtExpireSeconds()
        {
            return _jwtTokenModel.ExpiresInSeconds;
        }

        public JwtTokenModel GetJwtOptions()
        {
            return _jwtTokenModel;
        }

        private string GenerateJwt(UserModel appUserEntity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokenModel.SecurityKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = GetUserClaims(appUserEntity);

            var token = new JwtSecurityToken(
                issuer: _jwtTokenModel.Issuer,
                audience: _jwtTokenModel.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(_jwtTokenModel.ExpiresInSeconds),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtTokenModel.Audience,

                ValidateIssuer = true,
                ValidIssuer = _jwtTokenModel.Issuer,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtTokenModel.SecurityKey)
                ),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken)
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        private static List<Claim> GetUserClaims(UserModel user)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.EmailAddress),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddHours(1).ToString("o"))
            };
        }
    }
}
