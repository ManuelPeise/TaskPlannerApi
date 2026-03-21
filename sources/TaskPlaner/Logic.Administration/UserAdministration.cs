using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.User;

namespace Logic.Administration
{
    public class UserAdministration : LogicBase, IUserAdministration
    {
        private readonly ILogger<UserAdministration> _logger;
        private readonly IUserUnitOfWork _userUnitOfWork;

        public UserAdministration(
            ILogger<UserAdministration> logger, 
            IHttpContextAccessor httpContextAccessor,
            IUserUnitOfWork userUnitOfWork):base(httpContextAccessor, userUnitOfWork)
        {
            _logger = logger;
            _userUnitOfWork = userUnitOfWork;
        }

        public async Task<UserModel?> GetCurrentUser()
        {
            try
            {
                return CurrentUser ?? throw new UnauthorizedAccessException();
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while loading current user.", LogMessageTypeEnum.Error, exception);

                return null;
            }
        }

        public async Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials)
        {
            try
            {
                return await _userUnitOfWork.GetUsers(includeCredentials);
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while retrieving users.", LogMessageTypeEnum.Error, exception);

                return Enumerable.Empty<UserModel>();
            }
        }

        public async Task<UserModel?> GetUserById(int userId, bool includeCredentials)
        {

            var users = await _userUnitOfWork.GetUsers(includeCredentials);

            if (users == null || users.Any())
            {
                return null;
            }

            return users.FirstOrDefault(u => u.Id == userId);
        }

        public async Task CreateUser(UserModel userModel)
        {
            try
            {
                await _userUnitOfWork.AddUser(userModel);

                await _userUnitOfWork.SaveChangesAsync();

                // TODO: Send email to the user with their credentials.
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while creating a new user.", LogMessageTypeEnum.Error, exception);
            }
        }

        public async Task UpdateUser(UserModel userModel, bool updateCredentials)
        {
            try
            {
                await _userUnitOfWork.UpdateUser(userModel, updateCredentials);
                await _userUnitOfWork.SaveChangesAsync();

            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while updating user.", LogMessageTypeEnum.Error, exception);
            }
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                await _userUnitOfWork.DeleteUser(userId);
                await _userUnitOfWork.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while deleting user.", LogMessageTypeEnum.Error, exception);
            }
        }
    }
}
