using Logic.Administration.Interfaces;
using Logic.Shared;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shared.Enums;
using Shared.Models.Administartion;
using Shared.Models.Email;
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
            IUserUnitOfWork userUnitOfWork,
            IEmailClient emailClient,
            IOptions<ApiOptions> apiOptions) :base(httpContextAccessor, userUnitOfWork)
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

        public async Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials, bool includeUserRights)
        {
            try
            {
                return await _userUnitOfWork.GetUsers(includeCredentials, includeUserRights);
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while retrieving users.", LogMessageTypeEnum.Error, exception);

                return Enumerable.Empty<UserModel>();
            }
        }

        public async Task<UserModel?> GetUserById(int userId, bool includeCredentials, bool includeUserRights)
        {

            var users = await _userUnitOfWork.GetUsers(includeCredentials,includeUserRights);

            if (users == null || !users.Any())
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

#if !DEBUG
                var userEntity = await _userUnitOfWork.GetUserByEmail(userModel.EmailAddress, false, false);
                
                if (userEntity != null)
                {
                    var link = $"{_apiOptions.UiBaseAddress}account/activate/{userEntity.Id}";

                    var emailContent = CreateAccountActivationEmailContent($"{userEntity.Name} {userEntity.LastName}", userEntity.EmailAddress, link);

                    await _emailClient.SendMail(emailContent, userEntity.EmailAddress);
                }
#endif
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

        public async Task<UserAdministrationPageDataModel> GetUserAdministrationPageModel()
        {
            try
            {
                var users = await _userUnitOfWork.GetUsers(false, false);

                return new UserAdministrationPageDataModel
                {
                    Users = users.ToList(),
                    AccessRights = await _userUnitOfWork.GetAvailableAccessRights()
                };
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while loading user administration page data.", LogMessageTypeEnum.Error, exception);
                return new UserAdministrationPageDataModel();
            }
        }
        private EmailContent CreateAccountActivationEmailContent(string fullName, string customerMailAddress, string resetLink)
        {
            var content = new EmailContent
            {
                Subject = "Your TaskPlanner account",
                Text = $@"
<p>Dear {fullName},</p>
<p>Your account for <b>TaskPlanner</b> has been created.</p>
<p><b>Email:</b> {customerMailAddress}</p>
<p>Please set your password using the link below:</p>
<p><a href='{resetLink}'>Set Password</a></p>
<p>Best regards,</p><p/>TaskPlanner Team</p>
"
            };

            return content;

        }
    }
}
