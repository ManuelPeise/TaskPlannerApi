using Data.Entities.Git;
using Data.Entities.User;
using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities.Tasks
{
    public class TaskEntity : AEntityBase
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskTypeEnum TaskType { get; set; } = TaskTypeEnum.Default;
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Todo;
        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;
        public DateTime? DeadLineDate { get; set; }
        public int? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public UserEntity? AssignedUser { get; set; }
        [ForeignKey(nameof(ParentTaskId))]
        public int? ParentTaskId { get; set; }
        public ICollection<TaskEntity> SubTasks { get; set; } = new HashSet<TaskEntity>();
        public int? GitRepositoryId { get; set; }
        [ForeignKey(nameof(GitRepositoryId))]
        public GitRepositoryEntity? GitRepository { get; set; }
    }
}
