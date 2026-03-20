using Shared.Models.User;

namespace Logic.Administration.Interfaces
{
    public interface IUserAdministration
    {
        Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials);
        Task<UserModel?> GetUserById(int userId, bool includeCredentials);
        Task CreateUser(UserModel userModel);
        Task UpdateUser(UserModel userModel, bool updateCredentials);
        Task DeleteUser(int userId);
    }
}
