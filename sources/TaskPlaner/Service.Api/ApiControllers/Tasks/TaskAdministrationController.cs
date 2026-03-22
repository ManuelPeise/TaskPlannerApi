using Logic.Tasks.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Tasks;

namespace Service.Api.ApiControllers.Tasks
{
    public class TaskAdministrationController : ApiControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskAdministrationController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [JwtAuthentication]
        [HttpGet(Name = "GetTaskPageModel")]
        public async Task<TaskPageModel> GetTaskPageModel()
        {
            return await _taskService.GetTaskPageModel();
        }

        [JwtAuthentication]
        [HttpPost(Name = "AddTask")]
        public async Task AddTask([FromBody] TaskModel taskModel)
        {
            await _taskService.AddTask(taskModel);
        }

        [JwtAuthentication]
        [HttpPost(Name = "UpdateTask")]
        public async Task UpdateTask([FromBody] TaskModel taskModel)
        {
            await _taskService.UpdateTask(taskModel);
        }

        [JwtAuthentication]
        [HttpPost(Name = "DeleteTask")]
        public async Task DeleteTask([FromQuery] int taskId)
        {
            await _taskService.DeleteTask(taskId);
        }
    }
}
