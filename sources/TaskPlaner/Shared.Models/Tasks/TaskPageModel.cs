using Shared.Models.Ui;

namespace Shared.Models.Tasks
{
    public class TaskPageModel
    {
        public List<TaskModel> TaskModels { get; set; } = new List<TaskModel>();
        public List<DropdownItem> GitDropdownItems { get; set; } = new List<DropdownItem>();
        public List<DropdownItem> UserDropdownItems { get; set; } = new List<DropdownItem>();
    }
}
