namespace Shared.Models.User
{
    public class UserAdministrationPageDataModel
    {
        public List<UserModel> Users { get; set; } = new();
        public List<AccessRightModel> AccessRights { get; set; } = new();
    }
}
