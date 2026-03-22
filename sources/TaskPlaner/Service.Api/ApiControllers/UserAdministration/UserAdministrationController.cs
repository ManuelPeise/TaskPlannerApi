using Logic.Administration.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Shared.Enums;
using Shared.Models.User;

namespace Service.Api.ApiControllers.UserAdministration
{
    public class UserAdministrationController : ApiControllerBase
    {
        private readonly IUserAdministration _userAdministration;

        public UserAdministrationController(IUserAdministration userAdministration)
        {
            _userAdministration = userAdministration;
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserModel>> GetUsers([FromQuery] bool includeCredentials, bool includeUserRights)
        {
            return await _userAdministration.GetUsers(includeCredentials, includeUserRights);
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpGet(Name = "GetUserAdministrationPageModel")]
        public async Task<UserAdministrationPageDataModel> GetUserAdministrationPageModel()
        {
            return await _userAdministration.GetUserAdministrationPageModel();
        }

        [JwtAuthentication()]
        [HttpPost(Name = "LoadCurrentUser")]
        public async Task<UserModel?> LoadCurrentUser()
        {
            return await _userAdministration.GetCurrentUser();
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpGet(Name = "GetUserById")]
        public async Task<UserModel?> GetUserById([FromQuery] int userId, bool includeCredentials, bool includeUserRights)
        {
            return await _userAdministration.GetUserById(userId, includeCredentials, includeUserRights);
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpPost(Name = "CreateUser")]
        public async Task CreateUser([FromBody] UserModel userModel)
        {
            await _userAdministration.CreateUser(userModel);
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpPost(Name = "UpdateUser")]
        public async Task UpdateUser([FromBody] UserModel userModel, [FromQuery] bool updateCredentials)
        {
            await _userAdministration.UpdateUser(userModel, updateCredentials);
        }

        [JwtAuthentication(UserRole = UserRoleEnum.Admin)]
        [HttpPost(Name = "DeleteUser")]
        public async Task DeleteUser([FromQuery] int userId)
        {
            await _userAdministration.DeleteUser(userId);
        }
    }
}
