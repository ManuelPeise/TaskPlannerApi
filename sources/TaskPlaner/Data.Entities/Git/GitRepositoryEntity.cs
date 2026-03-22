using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Git
{
    public class GitRepositoryEntity : AEntityBase
    {
        public int Url { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GitRepositoryCredentialsId { get; set; }
        [ForeignKey(nameof(GitRepositoryCredentialsId))]
        public GitRepositoryCredentialsEntity GitRepositoryCredentials { get; set; }
    }
}
