using Shared.Models.Tasks;

namespace Logic.Tasks.Interfaces
{
    public interface ITaskService
    {
        Task<TaskPageModel> GetTaskPageModel();
        Task AddTask(TaskModel taskModel);
        Task UpdateTask(TaskModel taskModel);
        Task DeleteTask(int id);
    }
}
