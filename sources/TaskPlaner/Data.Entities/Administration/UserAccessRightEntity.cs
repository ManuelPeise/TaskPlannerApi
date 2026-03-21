using Data.Entities.User;
using System.ComponentModel.DataAnnotations.Schema;


namespace Data.Entities.Administration
{
    public class UserAccessRightEntity : AEntityBase
    {
        public bool CanCreate { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public  bool Deny { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }
        public int AccessRightId { get; set; }
        public AccessRightsEntity AccessRight { get; set; }
    }
}
