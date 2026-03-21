

namespace Data.Entities.Git
{
    public class GitRepositoryCredentialsEntity : AEntityBase
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
