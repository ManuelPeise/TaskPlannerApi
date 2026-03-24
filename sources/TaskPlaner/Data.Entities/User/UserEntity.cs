using Data.Entities.Administration;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.User
{
    public class UserEntity: AEntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public UserRoleEnum UserRole { get; set; }
        public bool IsActive { get; set; }
        public int CredentialsId { get; set; }
        [ForeignKey(nameof(CredentialsId))]
        public UserCredentialsEntity? Credentials { get; set; }
        public ICollection<UserAccessRightEntity> AccessRights { get; set; } = new HashSet<UserAccessRightEntity>();
    }
}
