using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Administartion;

namespace Service.Api.ApiControllers.Authentication
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IUserAuthentication _userAuthentication;

        public AuthenticationController(IUserAuthentication userAuthentication)
        {
            _userAuthentication = userAuthentication;
        }

        [HttpPost(Name = "Authenticate")]
        public async Task<TokenResponse?> Authenticate([FromBody] AuthenticationRequestModel requestModel)
        {
            return await _userAuthentication.AuthenticateUserAsync(requestModel);
        }

        [HttpPost(Name = "RefreshToken")]
        public async Task<TokenResponse?> RefreshToken([FromBody] TokenResponse model)
        {
            return await _userAuthentication.RefreshTokenAsync(model);
        }
    }
}
