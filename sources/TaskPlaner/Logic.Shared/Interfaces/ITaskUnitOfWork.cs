using Shared.Models.Tasks;

namespace Logic.Shared.Interfaces
{
    public interface ITaskUnitOfWork
    {
        Task<List<TaskModel>> GetAllTasks();
        Task<TaskModel?> GetTaskById(int id);
        Task AddTask(TaskModel taskModel);
        Task UpdateTask(TaskModel taskModel);
        Task DeleteTask(int taskId);
        Task SaveChangesAsync(string userName = "System");
    }
}
