using Shared.Models.User;

namespace Logic.Shared.Interfaces
{
    public interface IUserUnitOfWork: IUnitOfWorkBase
    {
        Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials);
        Task<UserModel?> GetUserById(int userId, bool includeCredentials);
        Task<UserModel?> GetUserByEmail(string email, bool includeCredentials);
        Task AddUser(UserModel user);
        Task UpdateUser(UserModel user, bool updateCredentials);
        Task DeleteUser(int userId);
       
    }
}
