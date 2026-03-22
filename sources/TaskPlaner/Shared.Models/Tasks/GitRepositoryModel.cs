

namespace Shared.Models.Tasks
{
    public class GitRepositoryModel
    {
        public int Id { get; set; }
        public int Url { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GitRepositoryCredentialsId { get; set; }
        public GitRepositoryCredentialsModel GitRepositoryCredentials { get; set; } = new GitRepositoryCredentialsModel();  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
    }

    public class GitRepositoryCredentialsModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
