using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Tasks.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Tasks;
using Shared.Models.Ui;

namespace Logic.Tasks
{
    public class TaskService : LogicBase, ITaskService
    {
        private readonly ILogger<TaskService> _logger;
        private readonly ITaskUnitOfWork _taskUnitOfWork;
        private readonly IUserUnitOfWork _userUnitOfWork;
        public TaskService(
            IHttpContextAccessor httpContextAccessor,
            ITaskUnitOfWork tasknitOfWork,
            IUserUnitOfWork userUnitOfWork,
            ILogger<TaskService> logger) : base(httpContextAccessor, userUnitOfWork)
        {
            _taskUnitOfWork = tasknitOfWork;
            _userUnitOfWork = userUnitOfWork;
            _logger = logger;
        }

        public async Task<TaskPageModel> GetTaskPageModel()
        {
            var pageModel = new TaskPageModel();

            try
            {
                var tasks = await _taskUnitOfWork.GetAllTasks();

                pageModel.TaskModels = tasks?.Select(t => new TaskItemBase
                {
                    Id = t.TaskId,
                    Title = t.Title,
                    ShortDescription = t.ShortDescription,
                    TaskTypeId = t.TaskType,
                    AssignedUserId = t.AssignedUserId,
                    Status = t.Status
                }).ToList() ?? new List<TaskItemBase>();

                pageModel.UserDropdownItems = await GetUserDropdownItemsAsync();

                return pageModel;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while loading task page model.", LogMessageTypeEnum.Error, exception);

                return pageModel;
            }
        }

        public async Task<List<TaskItemBase>> AddTask(TaskItemBase TaskItemBase)
        {
            try
            {
                var taskModel = new TaskModel
                {
                    Title = TaskItemBase.Title,
                    ShortDescription = TaskItemBase.ShortDescription,
                    TaskType = TaskItemBase.TaskTypeId,
                    Status = TaskItemBase.Status,
                    AssignedUserId = TaskItemBase.AssignedUserId == 0 ? null : TaskItemBase.AssignedUserId,
                    AcceptanceCreteria = string.Empty,
                    AssignedUser = null,
                    DeadLineDate = null,
                    Description = string.Empty,
                    ParentTaskId = null,
                    Priority = TaskPriorityEnum.Medium,
                    SubTasks = new List<TaskModel>()
                };

                await _taskUnitOfWork.AddTask(taskModel);

                await _taskUnitOfWork.SaveChangesAsync();

                var tasks = await _taskUnitOfWork.GetAllTasks();

                var taskModels = tasks?.Select(t => new TaskItemBase
                {
                    Id = t.TaskId,
                    Title = t.Title,
                    ShortDescription = t.ShortDescription,
                    TaskTypeId = t.TaskType,
                    AssignedUserId = t.AssignedUserId,
                    Status = t.Status
                }).ToList() ?? new List<TaskItemBase>();

                return taskModels;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while adding a new task.", LogMessageTypeEnum.Error, exception);

                return new List<TaskItemBase>();
            }
        }

        public async Task<TaskItemBase?> UpdateTaskBase(TaskItemBase model)
        {
            try
            {
                var task = await _taskUnitOfWork.GetTaskById(model.Id);

                if (task == null)
                {
                    throw new InvalidOperationException($"Updating task [{model.Id}] failed, task not found in database.");
                }

                task.Title = model.Title;
                task.ShortDescription = model.ShortDescription;
                task.TaskType = model.TaskTypeId;
                task.Status = model.Status;
                task.UserId = model.AssignedUserId == 0 ? null : model.AssignedUserId;

                await _taskUnitOfWork.SaveChangesAsync();

                return new TaskItemBase
                {
                    Id = task.Id,
                    Title = model.Title,
                    ShortDescription = task.ShortDescription,
                    AssignedUserId = model.AssignedUserId,
                    Status = model.Status,
                    TaskTypeId = model.TaskTypeId,
                };
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync($"Could not update task item [{model.Id}].", LogMessageTypeEnum.Error, exception);

                return null;
            }
        }

        public async Task UpdateTask(TaskModel taskModel)
        {
            try
            {
                await _taskUnitOfWork.UpdateTask(taskModel);

                await _taskUnitOfWork.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while updating a task.", LogMessageTypeEnum.Error, exception);
            }
        }

        public async Task<List<TaskItemBase>> DeleteTask(int id)
        {
            try
            {
                await _taskUnitOfWork.DeleteTask(id);
                await _taskUnitOfWork.SaveChangesAsync();

                var tasks = await _taskUnitOfWork.GetAllTasks();

                var taskModels = tasks?.Select(t => new TaskItemBase
                {
                    Id = t.TaskId,
                    Title = t.Title,
                    ShortDescription = t.ShortDescription,
                    TaskTypeId = t.TaskType,
                    AssignedUserId = t.AssignedUserId,
                    Status = t.Status
                }).ToList() ?? new List<TaskItemBase>();

                return taskModels;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while deleting a task.", LogMessageTypeEnum.Error, exception);

                return new List<TaskItemBase>();
            }
        }

        private async Task<List<DropdownItem>> GetUserDropdownItemsAsync()
        {
            var dropdownItems = new List<DropdownItem> { new DropdownItem { Id = 0, Label = "labelSelectUser" } };

            var users = await _userUnitOfWork.GetUsers(false, false);

            dropdownItems.AddRange(users.Where(user => user.IsActive).Select(user => new DropdownItem
            {
                Id = user.Id,
                Label = $"{user.Name} {user.LastName}"
            }));

            return dropdownItems;
        }
    }
}
