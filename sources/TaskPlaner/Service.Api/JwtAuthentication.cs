using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Api
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    internal class JwtAuthentication : Attribute, IAuthorizationFilter
    {
        public UserRoleEnum UserRole { get; set; } = UserRoleEnum.None;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var jwtTokenService = context.HttpContext.RequestServices.GetService<IJwtTokenService>();

            if (jwtTokenService == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var authHeader = context.HttpContext.Request.Headers["Authentication"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            var jwtModel = jwtTokenService.GetJwtOptions();

            if (jwtModel == null || string.IsNullOrEmpty(jwtModel.SecurityKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var principal = ValidateJwtToken(token, jwtModel);

            if (principal == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Hier wird die authentifizierte Identity gesetzt
            context.HttpContext.User = principal;

            if (UserRole == UserRoleEnum.None)
            {
                return;
            }

            var userRoleClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (userRoleClaim == null || !Enum.TryParse<UserRoleEnum>(userRoleClaim, out var userRole) || userRole != UserRole)
            {
                context.Result = new ForbidResult();
            }
        }

        private ClaimsPrincipal? ValidateJwtToken(string token, dynamic jwtModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtModel.SecurityKey);

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = true,
                        ValidAudience = jwtModel.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    },
                    out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                // Optional: Logging
                return null;
            }
        }
    }
}
