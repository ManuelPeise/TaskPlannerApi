using Shared.Models.Tasks;

namespace Logic.Tasks.Interfaces
{
    public interface ITaskService
    {
        Task<TaskPageModel> GetTaskPageModel();
        Task<TaskItemBase?> UpdateTaskBase(TaskItemBase model);
        Task<List<TaskItemBase>> AddTask(TaskItemBase taskItemBase);
        Task UpdateTask(TaskModel taskModel);
        Task<List<TaskItemBase>> DeleteTask(int id);
    }
}
