using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Tasks.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Email;
using Shared.Models.Tasks;
using Shared.Models.Ui;
using Shared.Models.User;

namespace Logic.Tasks
{
    public class TaskService : LogicBase, ITaskService
    {
        private readonly ILogger<TaskService> _logger;
        private readonly ITaskUnitOfWork _taskUnitOfWork;
        private readonly IUserUnitOfWork _userUnitOfWork;
        private readonly IEmailClient _emailClient;

        public TaskService(
            IHttpContextAccessor httpContextAccessor,
            ITaskUnitOfWork tasknitOfWork,
            IUserUnitOfWork userUnitOfWork,
            IEmailClient emailClient,
            ILogger<TaskService> logger) : base(httpContextAccessor, userUnitOfWork)
        {
            _taskUnitOfWork = tasknitOfWork;
            _userUnitOfWork = userUnitOfWork;
            _logger = logger;
            _emailClient = emailClient;

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

        public async Task<TaskDetailsPageModel> GetTaskDetailsPageModel(int taskId)
        {
            var pageModel = new TaskDetailsPageModel();

            try
            {
                var task = await _taskUnitOfWork.GetTaskById(taskId);

                if (task == null)
                {
                    throw new InvalidOperationException($"Task with id [{taskId}] was not found in database.");
                }

                pageModel.UserDropdownItems = await GetUserDropdownItemsAsync();
                pageModel.StatusDropdownItems = GetStatusDropdownItems();
                pageModel.PriorityDropdownItems = GetPriorityDropdownItems();

                pageModel.Task = new TaskModel
                {
                    TaskId = task.Id,
                    Title = task.Title,
                    ShortDescription = task.ShortDescription,
                    Description = task.Description,
                    AcceptanceCreteria = task.AcceptanceCriteria,
                    TaskType = task.TaskType,
                    Status = task.Status,
                    Priority = task.Priority,
                    DeadLineDate = task.DeadLineDate,
                    AssignedUserId = task.UserId,
                    AssignedUser = task.AssignedUser == null ? null : new UserModel
                    {
                        Id = task.AssignedUser.Id,
                        Name = task.AssignedUser.Name,
                        LastName = task.AssignedUser.LastName,
                        EmailAddress = task.AssignedUser.EmailAddress,
                        UserRole = task.AssignedUser.UserRole
                    },
                    ParentTaskId = task.ParentTaskId,
                    SubTasks = task.SubTasks?.Select(st => new TaskModel
                    {
                        TaskId = st.Id,
                        ShortDescription = st.ShortDescription,
                        Description = st.Description,
                        AcceptanceCreteria = st.AcceptanceCriteria,
                        TaskType = st.TaskType,
                        Status = st.Status,
                        Priority = st.Priority,
                        DeadLineDate = st.DeadLineDate,
                        AssignedUserId = st.UserId,
                    }).ToList() ?? new List<TaskModel>(),
                    CreatedAt = task.CreatedAt,
                    CreatedBy = task.CreatedBy,
                    UpdatedAt = task.UpdatedAt,
                    UpdatedBy = task.UpdatedBy
                };

                return pageModel;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync($"An error occurred while loading task with id [{taskId}].", LogMessageTypeEnum.Error, exception);

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

                var potentialUnassignedUser = task.UserId;

                task.Title = model.Title;
                task.ShortDescription = model.ShortDescription;
                task.TaskType = model.TaskTypeId;
                task.Status = model.Status;
                task.UserId = model.AssignedUserId == 0 ? null : model.AssignedUserId;

                await _taskUnitOfWork.SaveChangesAsync();

                if (potentialUnassignedUser != task.UserId)
                {
                    await SendTaskNotifications(task.UserId ?? 0, potentialUnassignedUser ?? 0, task.Id);
                }

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

        private async Task SendTaskNotifications(int assignedUserId, int unassignedUserId, int taskId)
        {
            var unassigned = await _userUnitOfWork.GetUserById(unassignedUserId, false, false);

            if (unassigned != null)
            {
#if !DEBUG
                        await _emailClient.SendMail(GetTaskUnassignedEmailContent(task.Id, $"{unassigned.Name} {unassigned.LastName}"), unassigned.EmailAddress);
#endif
            }

            var assignedUser = await _userUnitOfWork.GetUserById(assignedUserId, false, false);

            if (assignedUser != null)
            {
#if !DEBUG
                        await _emailClient.SendMail(GetTaskAssignedEmailContent(task.Id, $"{assignedUser.Name} {assignedUser.LastName}"), assignedUser.EmailAddress);
#endif
            }
        }
        private async Task<List<DropdownItem>> GetUserDropdownItemsAsync()
        {
            var dropdownItems = new List<DropdownItem> { new DropdownItem { Id = 0, Label = "labelUnassigned" } };

            var users = await _userUnitOfWork.GetUsers(false, false);

            dropdownItems.AddRange(users.Where(user => user.IsActive).Select(user => new DropdownItem
            {
                Id = user.Id,
                Label = $"{user.Name} {user.LastName}"
            }));

            return dropdownItems;
        }

        private List<DropdownItem> GetStatusDropdownItems()
        {
            var items = new List<DropdownItem> {
                new DropdownItem { Id = (int)TaskStatusEnum.Created, Label = "labelCreated" },
                new DropdownItem { Id = (int)TaskStatusEnum.ReadyToStart, Label = "labelReadyToStart" },
                new DropdownItem { Id = (int)TaskStatusEnum.InProgress, Label = "labelInProgress" },
                new DropdownItem { Id = (int)TaskStatusEnum.Completed, Label = "labelCompleted" },

            };

            return items;
        }

        private List<DropdownItem> GetPriorityDropdownItems()
        {
            var items = new List<DropdownItem> {
                new DropdownItem { Id = (int)TaskPriorityEnum.None, Label = "labelNone" },
                new DropdownItem { Id = (int)TaskPriorityEnum.Low, Label = "labelLow" },
                new DropdownItem { Id = (int)TaskPriorityEnum.Medium, Label = "labelMedium" },
                new DropdownItem { Id = (int)TaskPriorityEnum.High, Label = "labelHigh" },

            };
            
            return items;
        }
        private EmailContent GetTaskAssignedEmailContent(int taskId, string name)
        {
            return new EmailContent
            {
                Subject = $"New task #{taskId} was assigned to you.",
                Text = $@"
<p>Dear {name},</p>
<p></p>
<p>There is a new task #{taskId} assigned to you.</p>
<p></p>
<p>Please log in to the system to view the task details and start working on it.</p>
"
            };
        }

        private EmailContent GetTaskUnassignedEmailContent(int taskId, string name)
        {
            return new EmailContent
            {
                Subject = $"Task #{taskId} was unassigned from you.",
                Text = $@"
<p>Dear {name},</p>
<p></p>
<p>Task #{taskId} has been unassigned from you.</p>
<p></p>

"
            };
        }
    }
}
