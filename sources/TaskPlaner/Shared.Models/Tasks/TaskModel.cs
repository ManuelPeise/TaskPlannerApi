using Shared.Enums;
using Shared.Models.User;

namespace Shared.Models.Tasks
{
    public class TaskModel
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AcceptanceCreteria { get; set; }
        public TaskTypeEnum TaskType { get; set; } = TaskTypeEnum.All;
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Created;
        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;
        public DateTime? DeadLineDate { get; set; }
        public int? AssignedUserId { get; set; }
        public UserModel? AssignedUser { get; set; }
        public int? ParentTaskId { get; set; }
        public List<TaskModel> SubTasks { get; set; } = new List<TaskModel>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
