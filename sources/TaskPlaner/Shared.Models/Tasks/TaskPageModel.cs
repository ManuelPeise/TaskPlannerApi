using Shared.Models.Ui;

namespace Shared.Models.Tasks
{
    public class TaskPageModel
    {
        public List<TaskItemBase> TaskModels { get; set; } = new List<TaskItemBase>();
        public List<DropdownItem> UserDropdownItems { get; set; } = new List<DropdownItem>();
    }
}
