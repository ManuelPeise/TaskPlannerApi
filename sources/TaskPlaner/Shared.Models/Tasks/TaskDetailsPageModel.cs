using Shared.Models.Ui;

namespace Shared.Models.Tasks
{
    public class TaskDetailsPageModel
    {
        public TaskModel Task { get; set; } = new();
        public List<DropdownItem> UserDropdownItems { get; set; } = new();
        public List<DropdownItem> PriorityDropdownItems { get; set; } = new();
        public List<DropdownItem> StatusDropdownItems { get; set; } = new();

    }
}
