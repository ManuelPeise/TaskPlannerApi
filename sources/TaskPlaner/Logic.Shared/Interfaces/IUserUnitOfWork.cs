using Shared.Models.User;

namespace Logic.Shared.Interfaces
{
    public interface IUserUnitOfWork: IUnitOfWorkBase
    {
        Task<IEnumerable<UserModel>> GetUsers(bool include);
        Task<UserModel?> GetUserById(int userId, bool include);
        Task<UserModel?> GetUserByEmail(string email, bool include);
        Task AddUser(UserModel user);
        Task UpdateUser(UserModel user, bool updateCredentials);
        Task DeleteUser(int userId);
       
    }
}
