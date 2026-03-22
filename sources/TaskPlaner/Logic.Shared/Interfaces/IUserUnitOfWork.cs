using Shared.Models.User;

namespace Logic.Shared.Interfaces
{
    public interface IUserUnitOfWork: IUnitOfWorkBase
    {
        Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials, bool includeUserRights);
        Task<UserModel?> GetUserById(int userId, bool includeCredentials, bool includeUserRights);
        Task<UserModel?> GetUserByEmail(string email, bool includeCredentials, bool includeUserRights);
        Task AddUser(UserModel user);
        Task UpdateUser(UserModel user, bool updateCredentials);
        Task DeleteUser(int userId);
        Task<List<AccessRightModel>> GetAvailableAccessRights();


    }
}
