using Data.Accessor;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities;
using Data.Entities.Git;
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
        private readonly IDbRepositoryBase<GitRepositoryEntity> _gitRepository;
        private readonly Func<IQueryable<TaskEntity>, IQueryable<TaskEntity>>[] includeExpression = { q => q.Include(t => t.SubTasks), q => q.Include(t => t.AssignedUser), q => q.Include(t => t.GitRepository) };

        public TaskUnitOfWork(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _httpContext = httpContextAccessor.HttpContext;
            _gitRepository = new DbRepositoryBase<GitRepositoryEntity>(_databaseContext);
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
                GitRepositoryId = t.GitRepositoryId,
                GitRepository = t.GitRepository != null ? new GitRepositoryModel
                {
                    Id = t.GitRepository.Id,
                    Name = t.GitRepository.Name,
                    Url = t.GitRepository.Url,
                    GitRepositoryCredentialsId = t.GitRepository.GitRepositoryCredentialsId,
                    CreatedAt = t.GitRepository.CreatedAt,
                    CreatedBy = t.GitRepository.CreatedBy,
                    UpdatedAt = t.GitRepository.UpdatedAt,
                    UpdatedBy = t.GitRepository.UpdatedBy
                } : null,
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

        public async Task<TaskModel?> GetTaskById(int id)
        {
            var tasks = await _taskRepository.GetById(id, false, includeExpression);

            if (tasks == null || !tasks.Any() || tasks.Count > 1)
            {
                return null;
            }

            var task = tasks.First();

            return new TaskModel
            {
                TaskId = task.Id,
                Title = task.Title,
                Description = task.Description,
                TaskType = task.TaskType,
                Status = task.Status,
                Priority = task.Priority,
                DeadLineDate = task.DeadLineDate,
                AssignedUserId = task.UserId,
                CreatedAt = task.CreatedAt,
                CreatedBy = task.CreatedBy,
                UpdatedAt = task.UpdatedAt,
                UpdatedBy = task.UpdatedBy,
                AssignedUser = task.AssignedUser != null ? new UserModel
                {
                    Id = task.AssignedUser.Id,
                    Name = task.AssignedUser.Name,
                    LastName = task.AssignedUser.LastName,
                    EmailAddress = task.AssignedUser.EmailAddress,
                    IsActive = task.AssignedUser.IsActive,
                } : null,
                ParentTaskId = task.ParentTaskId,
                GitRepositoryId = task.GitRepositoryId,
                GitRepository = task.GitRepository != null ? new GitRepositoryModel
                {
                    Id = task.GitRepository.Id,
                    Name = task.GitRepository.Name,
                    Url = task.GitRepository.Url,
                    GitRepositoryCredentialsId = task.GitRepository.GitRepositoryCredentialsId,
                    CreatedAt = task.GitRepository.CreatedAt,
                    CreatedBy = task.GitRepository.CreatedBy,
                    UpdatedAt = task.GitRepository.UpdatedAt,
                    UpdatedBy = task.GitRepository.UpdatedBy
                } : null,
                SubTasks = task.SubTasks.Any() ? task.SubTasks.Select(st => new TaskModel
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
            };
        }

        public async Task<List<GitRepositoryModel>> GetGitRepositories(int userId)
        {
            var userEntities = await _userRepository.GetById(userId, false, q => q.Include(u => u.GitRepositories).ThenInclude(gr => gr.GitRepositoryCredentials));

            if (userEntities == null || !userEntities.Any() || userEntities.Count > 1)
            {
                return new List<GitRepositoryModel>();
            }
            var userEntity = userEntities.First();
            return userEntity.GitRepositories.Select(gr => new GitRepositoryModel
            {
                Id = gr.Id,
                Name = gr.Name,
                Url = gr.Url,
                GitRepositoryCredentialsId = gr.GitRepositoryCredentialsId,
                CreatedAt = gr.CreatedAt,
                CreatedBy = gr.CreatedBy,
                UpdatedAt = gr.UpdatedAt,
                UpdatedBy = gr.UpdatedBy
            }).ToList();
        }
       
        
        public async Task AddTask(TaskModel taskModel)
        {
            var gitRepositoryEntities = taskModel.GitRepositoryId.HasValue
                ? await _gitRepository.GetById(taskModel.GitRepositoryId.Value)
                : null;

            var gitRepoEntity = gitRepositoryEntities != null && gitRepositoryEntities.Any() ? gitRepositoryEntities.First() : null;

            var taskEntity = new TaskEntity
            {
                Title = taskModel.Title,
                Description = taskModel.Description,
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
                GitRepositoryId = gitRepoEntity?.Id ?? null,
                GitRepository = gitRepoEntity == null && taskModel?.GitRepository != null ? new GitRepositoryEntity
                {
                    Url = taskModel.GitRepository.Url,
                    Name = taskModel.GitRepository.Name,
                    GitRepositoryCredentials = new GitRepositoryCredentialsEntity
                    {
                        UserName = taskModel.GitRepository.GitRepositoryCredentials.UserName,
                        PasswordHash = PasswordHasher.HashPassword(taskModel.GitRepository.GitRepositoryCredentials.PasswordHash),
                    }

                } : null,
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
            var existingTasks = await _taskRepository.GetById(taskModel.TaskId, false, includeExpression);

            if (existingTasks == null || !existingTasks.Any() || existingTasks.Count > 1)
            {
                throw new InvalidOperationException($"Task with ID {taskModel.TaskId} not found or multiple tasks found.");
            }

            var existingTask = existingTasks.First();

            existingTask.Title = taskModel.Title;
            existingTask.Description = taskModel.Description;
            existingTask.TaskType = taskModel.TaskType;
            existingTask.Status = taskModel.Status;
            existingTask.Priority = taskModel.Priority;
            existingTask.DeadLineDate = taskModel.DeadLineDate;
            existingTask.UserId = taskModel.AssignedUserId;
            existingTask.ParentTaskId = taskModel.ParentTaskId;
            existingTask.GitRepositoryId = taskModel.GitRepositoryId;
            existingTask.GitRepository = taskModel.GitRepositoryId.HasValue ? null : (taskModel.GitRepository != null ? new GitRepositoryEntity
            {
                Url = taskModel.GitRepository.Url,
                Name = taskModel.GitRepository.Name,
                GitRepositoryCredentials = new GitRepositoryCredentialsEntity
                {
                    UserName = taskModel.GitRepository.GitRepositoryCredentials.UserName,
                    PasswordHash = PasswordHasher.HashPassword(taskModel.GitRepository.GitRepositoryCredentials.PasswordHash),
                }
            } : null);

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
