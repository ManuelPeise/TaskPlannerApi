using Shared.Models.User;

namespace Logic.Administration.Interfaces
{
    public interface IUserAdministration
    {
        Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials, bool includeUserRights);
        Task<UserAdministrationPageDataModel> GetUserAdministrationPageModel();
        Task<UserModel?> GetCurrentUser();
        Task<UserModel?> GetUserById(int userId, bool includeCredentials, bool includeUserRights);
        Task CreateUser(UserModel userModel);
        Task UpdateUser(UserModel userModel, bool updateCredentials);
        Task DeleteUser(int userId);
        
    }
}
