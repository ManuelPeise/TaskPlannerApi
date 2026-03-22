using Logic.Shared;
using Logic.Shared.Interfaces;
using Logic.Tasks.Interfaces;
using Microsoft.AspNetCore.Http;
using Shared.Enums;
using Shared.Models.Tasks;
using Shared.Models.Ui;
using System.ComponentModel;

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
                pageModel.TaskModels = await _taskUnitOfWork.GetAllTasks();
                pageModel.GitDropdownItems = await GetDropdownItemsAsync();
                pageModel.UserDropdownItems = await GetUserDropdownItemsAsync();

                return pageModel;
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while loading task page model.", LogMessageTypeEnum.Error, exception);

                return pageModel;
            }
        }

        public async Task AddTask(TaskModel taskModel)
        {
            try
            {
                await _taskUnitOfWork.AddTask(taskModel);

                await _taskUnitOfWork.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while adding a new task.", LogMessageTypeEnum.Error, exception);
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

        public async Task DeleteTask(int id)
        {
            try
            {
                await _taskUnitOfWork.DeleteTask(id);
                await _taskUnitOfWork.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                await _logger.LogMessageAsync("An error occurred while deleting a task.", LogMessageTypeEnum.Error, exception);
            }
        }

        private async Task<List<DropdownItem>> GetUserDropdownItemsAsync()
        {
            var dropdownItems = new List<DropdownItem> { new DropdownItem { Id = 0, Label = "labelSelectUser" } };
            
            var users = await _userUnitOfWork.GetUsers(false, false);
            
            dropdownItems.AddRange(users.Select(user => new DropdownItem
            {
                Id = user.Id,
                Label = $"{user.Name} {user.LastName}"
            }));

            return dropdownItems;
        }

        private async Task<List<DropdownItem>> GetDropdownItemsAsync()
        {
            var dropdownItems = new List<DropdownItem> { new DropdownItem { Id = 0, Label = "labelSelectGitRepository" } };
            var gitRepositories = await _taskUnitOfWork.GetGitRepositories(CurrentUser?.Id ?? 0);
            
            dropdownItems.AddRange(gitRepositories.Select(repo => new DropdownItem
            {
                Id = repo.Id,
                Label = repo.Name
            }));

            return dropdownItems;
        }
    }
}
