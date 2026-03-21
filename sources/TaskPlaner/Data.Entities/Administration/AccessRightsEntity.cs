using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Administration
{
    public class AccessRightsEntity : AEntityBase
    {
        public Guid AccessRightGuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<UserAccessRightEntity> UserAccessRights { get; set; } = new HashSet<UserAccessRightEntity>();
    }
}
