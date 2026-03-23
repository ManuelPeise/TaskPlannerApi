using Data.Accessor;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities;
using Data.Entities.Tasks;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Tasks;
using Shared.Models.User;

namespace Logic.Shared
{
    public class TaskUnitOfWork : ITaskUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private readonly HttpContext _httpContext;

        private readonly IDbRepositoryBase<UserEntity> _userRepository;
        private readonly IDbRepositoryBase<TaskEntity> _taskRepository;
        private readonly Func<IQueryable<TaskEntity>, IQueryable<TaskEntity>>[] includeExpression = { q => q.Include(t => t.SubTasks), q => q.Include(t => t.AssignedUser) };

        public TaskUnitOfWork(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = new DbRepositoryBase<UserEntity>(_databaseContext);
            _taskRepository = new DbRepositoryBase<TaskEntity>(_databaseContext);
        }

        public async Task<List<TaskModel>> GetAllTasks()
        {
            var tasks = await _taskRepository.GetAll(false, includeExpression);

            return tasks.Select(t => new TaskModel
            {
                TaskId = t.Id,
                Title = t.Title,
                ShortDescription = t.ShortDescription,
                AcceptanceCreteria = t.AcceptanceCriteria,
                Description = t.Description,
                TaskType = t.TaskType,
                Status = t.Status,
                Priority = t.Priority,
                DeadLineDate = t.DeadLineDate,
                AssignedUserId = t.UserId,
                CreatedAt = t.CreatedAt,
                CreatedBy = t.CreatedBy,
                UpdatedAt = t.UpdatedAt,
                UpdatedBy = t.UpdatedBy,
                AssignedUser = t.AssignedUser != null ? new UserModel
                {
                    Id = t.AssignedUser.Id,
                    Name = t.AssignedUser.Name,
                    LastName = t.AssignedUser.LastName,
                    EmailAddress = t.AssignedUser.EmailAddress,
                    IsActive = t.AssignedUser.IsActive,
                } : null,
                ParentTaskId = t.ParentTaskId,
                SubTasks = t.SubTasks.Any() ? t.SubTasks.Select(st => new TaskModel
                {
                    TaskId = st.Id,
                    Title = st.Title,
                    Description = st.Description,
                    TaskType = st.TaskType,
                    Status = st.Status,
                    Priority = st.Priority,
                    DeadLineDate = st.DeadLineDate,
                    AssignedUserId = st.UserId,
                    CreatedAt = st.CreatedAt,
                    CreatedBy = st.CreatedBy,
                    UpdatedAt = st.UpdatedAt,
                    UpdatedBy = st.UpdatedBy

                }).ToList() : new List<TaskModel>(),
            }).ToList();
        }

        public async Task<TaskEntity?> GetTaskById(int id)
        {
            var task = await _taskRepository.GetById(id, false, includeExpression);

            if (task == null)
            {
                return null;
            }

            return task;
        }

        public async Task AddTask(TaskModel taskModel)
        {
            var taskEntity = new TaskEntity
            {
                Title = taskModel.Title,
                ShortDescription = taskModel.ShortDescription,
                Description = taskModel.Description,
                AcceptanceCriteria = taskModel.AcceptanceCreteria ?? string.Empty,
                TaskType = taskModel.TaskType,
                Status = taskModel.Status,
                Priority = taskModel.Priority,
                DeadLineDate = taskModel.DeadLineDate,
                CreatedAt = taskModel.CreatedAt,
                CreatedBy = taskModel.CreatedBy,
                UpdatedAt = taskModel.UpdatedAt,
                UpdatedBy = taskModel.UpdatedBy,
                UserId = taskModel?.AssignedUserId ?? null,
                ParentTaskId = taskModel?.ParentTaskId ?? null,
                SubTasks = taskModel?.SubTasks.Select(st => new TaskEntity
                {
                    Title = st.Title,
                    Description = st.Description,
                    TaskType = st.TaskType,
                    Status = st.Status,
                    Priority = st.Priority,
                    DeadLineDate = st.DeadLineDate,
                    UserId = st.AssignedUserId,
                    CreatedAt = st.CreatedAt,
                    CreatedBy = st.CreatedBy,
                    UpdatedAt = st.UpdatedAt,
                    UpdatedBy = st.UpdatedBy
                }).ToList() ?? new List<TaskEntity>(),

            };

            await _taskRepository.Insert(taskEntity, null);
        }

        public async Task UpdateTask(TaskModel taskModel)
        {
            var existingTask = await _taskRepository.GetById(taskModel.TaskId, false, includeExpression);

            if (existingTask == null)
            {
                throw new InvalidOperationException($"Task with ID {taskModel.TaskId} not found or multiple tasks found.");
            }

            existingTask.Title = taskModel.Title;
            existingTask.Description = taskModel.Description;
            existingTask.TaskType = taskModel.TaskType;
            existingTask.Status = taskModel.Status;
            existingTask.Priority = taskModel.Priority;
            existingTask.DeadLineDate = taskModel.DeadLineDate;
            existingTask.UserId = taskModel.AssignedUserId;
            existingTask.ParentTaskId = taskModel.ParentTaskId;


            foreach (var taskEntity in existingTask.SubTasks)
            {
                var subTaskModel = taskModel.SubTasks.FirstOrDefault(st => st.TaskId == taskEntity.Id);

                if (subTaskModel != null)
                {
                    taskEntity.Title = subTaskModel.Title;
                    taskEntity.Description = subTaskModel.Description;
                    taskEntity.TaskType = subTaskModel.TaskType;
                    taskEntity.Status = subTaskModel.Status;
                    taskEntity.Priority = subTaskModel.Priority;
                    taskEntity.DeadLineDate = subTaskModel.DeadLineDate;
                    taskEntity.UserId = subTaskModel.AssignedUserId;
                }
                else
                {
                    // a subtask that exists in the database but not in the incoming model should be deleted,
                    // a sub task could never assigned to another task,
                    // so we can safely delete it without worrying about orphaned records
                    await _taskRepository.Delete(taskEntity.Id);
                }
            }

        }

        public async Task DeleteTask(int taskId)
        {
            await _taskRepository.Delete(taskId);
        }

        public async Task SaveChangesAsync(string userName = "System")
        {
            if (_databaseContext == null) throw new ObjectDisposedException(nameof(UserUnitOfWork));

            var user = _httpContext.User.Identity?.Name ?? userName;

            var now = DateTime.UtcNow;

            var entries = _databaseContext.ChangeTracker.Entries<AEntityBase>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.CreatedBy = user;
                    entry.Entity.UpdatedBy = user;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(AEntityBase.CreatedAt)).IsModified = false;
                    entry.Property(nameof(AEntityBase.CreatedBy)).IsModified = false;

                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = user;
                }
            }

            await _databaseContext.SaveChangesAsync();
        }
    }
}
