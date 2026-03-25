using Logic.Dashboard.Interfaces;
using Logic.Tasks.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.Tasks;

namespace Service.Api.ApiControllers.Tasks
{
    public class TaskAdministrationController : ApiControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskAdministrationController(ITaskService taskService, IEndpointStatisticService endpointStatisticService): base(endpointStatisticService) 
        {
            _taskService = taskService;
        }

        [JwtAuthentication]
        [HttpGet(Name = "GetTaskPageModel")]
        public async Task<TaskPageModel> GetTaskPageModel()
        {
            var result = await _taskService.GetTaskPageModel();

            await base.RecordEndpointStatisticAsync(Shared.Enums.EndpointEnum.GetTaskPageModel);

            return result;
        }

        [JwtAuthentication]
        [HttpGet(Name = "GetTaskDetailsPageModel")]
        public async Task<TaskDetailsPageModel> GetTaskDetailsPageModel([FromQuery] int taskId)
        {
            var result = await _taskService.GetTaskDetailsPageModel(taskId);

            return result;
        }

        [JwtAuthentication]
        [HttpPost(Name = "AddTask")]
        public async Task<List<TaskItemBase>> AddTask([FromBody] TaskItemBase taskItemBase)
        {
            var result = await _taskService.AddTask(taskItemBase);

            await base.RecordEndpointStatisticAsync(Shared.Enums.EndpointEnum.AddTask);

            return result;
        }

        [JwtAuthentication]
        [HttpPost(Name = "UpdateTaskBase")]
        public async Task<TaskItemBase?> UpdateTaskBase([FromBody] TaskItemBase model)
        {
            var result = await _taskService.UpdateTaskBase(model);

            await base.RecordEndpointStatisticAsync(Shared.Enums.EndpointEnum.UpdateTaskBase);

            return result;
        }

        [JwtAuthentication]
        [HttpPost(Name = "UpdateTask")]
        public async Task UpdateTask([FromBody] TaskModel taskModel)
        {
            await _taskService.UpdateTask(taskModel);

            await base.RecordEndpointStatisticAsync(Shared.Enums.EndpointEnum.UpdateTask);

        }

        [JwtAuthentication]
        [HttpPost(Name = "DeleteTask")]
        public async Task<List<TaskItemBase>> DeleteTask([FromQuery] int taskId)
        {
            var result = await _taskService.DeleteTask(taskId);

            await base.RecordEndpointStatisticAsync(Shared.Enums.EndpointEnum.DeleteTask);

            return result;
        }
    }
}
