using Shared.Enums;
namespace Shared.Models.Tasks
{
    public class TaskItemBase
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public TaskStatusEnum Status { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public int? AssignedUserId { get; set; }
        public TaskTypeEnum TaskTypeId { get; set; }
    }
}
