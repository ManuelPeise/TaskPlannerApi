using Shared.Enums;

namespace Shared.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public UserRoleEnum UserRole { get; set; }
        public int? CredentialsId { get; set; }
        public UserCredentialsModel? Credentials { get; set; }
        public List<AccessRightModel> AccessRights { get; set; } = new List<AccessRightModel>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
