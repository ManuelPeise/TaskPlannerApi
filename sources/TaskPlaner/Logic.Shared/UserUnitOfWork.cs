using Data.Accessor;
using Data.Accessor.Interfaces;
using Data.Database;
using Data.Entities;
using Data.Entities.Administration;
using Data.Entities.User;
using Logic.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Models.User;

namespace Logic.Shared
{
    public class UserUnitOfWork : IUserUnitOfWork
    {
        private readonly DatabaseContext _databaseContext;
        private readonly HttpContext _httpContext;

        private readonly IDbRepositoryBase<UserEntity> _userRepository;
        private readonly IDbRepositoryBase<AccessRightsEntity> _accessRightsRepository;
        private readonly Func<IQueryable<UserEntity>, IQueryable<UserEntity>>[] includeExpression = { q => q.Include(u => u.Credentials), a => a.Include(u => u.AccessRights).ThenInclude(ur => ur.AccessRight) };
        private readonly Func<IQueryable<UserEntity>, IQueryable<UserEntity>>[] includeCredentialsExpression = { q => q.Include(u => u.Credentials) };
        private readonly Func<IQueryable<UserEntity>, IQueryable<UserEntity>>[] includeAccessRightsExpression = { q => q.Include(u => u.AccessRights).ThenInclude(ur => ur.AccessRight) };


        public UserUnitOfWork(DatabaseContext databaseContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseContext = databaseContext;
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = new DbRepositoryBase<UserEntity>(_databaseContext);
            _accessRightsRepository = new DbRepositoryBase<AccessRightsEntity>(_databaseContext);
        }

        public async Task<IEnumerable<UserModel>> GetUsers(bool includeCredentials, bool includeUserRights)
        {
            var userEntities = await _userRepository.GetAll(
                false,
                includeCredentials && includeUserRights ?
                    includeExpression :
                    includeCredentials ?
                    includeCredentialsExpression :
                    includeUserRights ?
                    includeAccessRightsExpression :
                    null);

            if (!userEntities.Any())
            {
                return Enumerable.Empty<UserModel>();
            }

            return userEntities.Select(e => new UserModel
            {
                Id = e.Id,
                Name = e.Name,
                LastName = e.LastName,
                EmailAddress = e.EmailAddress,
                IsActive = e.IsActive,
                UserRole = e.UserRole,
                CredentialsId = includeCredentials ? e.CredentialsId : null,
                Credentials = includeCredentials ? new UserCredentialsModel
                {
                    Id = e.Credentials?.Id ?? 0,
                    PasswordHash = e.Credentials?.PasswordHash ?? string.Empty,
                    RefreshToken = e.Credentials?.RefreshToken ?? string.Empty,
                    CreatedAt = e.Credentials?.CreatedAt ?? DateTime.MinValue,
                    CreatedBy = e.Credentials?.CreatedBy ?? string.Empty,
                    UpdatedAt = e.Credentials?.UpdatedAt ?? DateTime.MinValue,
                    UpdatedBy = e.Credentials?.UpdatedBy ?? string.Empty

                } : null,
                AccessRights = includeUserRights ? e.AccessRights.Select(ar => new AccessRightModel
                {
                    Id = ar.Id,
                    Name = ar.AccessRight.Name,
                    Deny = ar.Deny,
                    CanCreate = ar.CanCreate,
                    CanView = ar.CanView,
                    CanEdit = ar.CanEdit,
                    CanDelete = ar.CanDelete,
                    IsActive = ar.IsActive,
                    CreatedAt = ar.CreatedAt,
                    CreatedBy = ar.CreatedBy,
                    UpdatedAt = ar.UpdatedAt,
                    UpdatedBy = ar.UpdatedBy
                }).ToList() : new List<AccessRightModel>(),
                CreatedBy = e.CreatedBy,
                CreatedAt = e.CreatedAt,
                UpdatedBy = e.UpdatedBy,
                UpdatedAt = e.UpdatedAt
            });
        }

        public async Task<UserModel?> GetUserById(int userId, bool includeCredentials, bool includeUserRights)
        {
            var userEntities = await _userRepository.GetById(
                userId,
                false,
                includeCredentials && includeUserRights ?
                    includeExpression :
                    includeCredentials ?
                    includeCredentialsExpression :
                    includeUserRights ?
                    includeAccessRightsExpression :
                    null);

            if (!userEntities.Any() || userEntities.Count > 1)
            {
                return null;
            }

            var userEntity = userEntities.FirstOrDefault();

            if (userEntity == null)
            {
                return null;
            }

            return new UserModel
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                LastName = userEntity.LastName,
                EmailAddress = userEntity.EmailAddress,
                IsActive = userEntity.IsActive,
                UserRole = userEntity.UserRole,
                CredentialsId = includeCredentials ? userEntity.CredentialsId : 0,
                Credentials = includeCredentials ? new UserCredentialsModel
                {
                    Id = userEntity.Credentials?.Id ?? 0,
                    PasswordHash = userEntity.Credentials?.PasswordHash ?? string.Empty,
                    RefreshToken = userEntity.Credentials?.RefreshToken ?? string.Empty,
                    CreatedAt = userEntity.Credentials?.CreatedAt ?? DateTime.MinValue,
                    CreatedBy = userEntity.Credentials?.CreatedBy ?? string.Empty,
                    UpdatedAt = userEntity.Credentials?.UpdatedAt ?? DateTime.MinValue,
                    UpdatedBy = userEntity.Credentials?.UpdatedBy ?? string.Empty
                } : null,
                AccessRights = includeUserRights ? userEntity.AccessRights.Select(ar => new AccessRightModel
                {
                    Id = ar.Id,
                    Name = ar.AccessRight.Name,
                    Deny = ar.Deny,
                    CanCreate = ar.CanCreate,
                    CanView = ar.CanView,
                    CanEdit = ar.CanEdit,
                    CanDelete = ar.CanDelete,
                    IsActive = ar.IsActive,
                    CreatedAt = ar.CreatedAt,
                    CreatedBy = ar.CreatedBy,
                    UpdatedAt = ar.UpdatedAt,
                    UpdatedBy = ar.UpdatedBy
                }).ToList() : new List<AccessRightModel>(),
                CreatedBy = userEntity.CreatedBy,
                CreatedAt = userEntity.CreatedAt,
                UpdatedBy = userEntity.UpdatedBy,
                UpdatedAt = userEntity.UpdatedAt
            };
        }

        public async Task<UserModel?> GetUserByEmail(string email, bool includeCredentials, bool includeUserRights)
        {
            var userEntities = await _userRepository.GetBy(
                e => e.EmailAddress == email, 
                false,
                includeCredentials && includeUserRights ?
                    includeExpression :
                    includeCredentials ?
                    includeCredentialsExpression :
                    includeUserRights ?
                    includeAccessRightsExpression :
                    null);

            if (!userEntities.Any() || userEntities.Count > 1)
            {
                return null;
            }

            var userEntity = userEntities.FirstOrDefault();

            if (userEntity == null)
            {
                return null;
            }

            return new UserModel
            {
                Id = userEntity.Id,
                Name = userEntity.Name,
                LastName = userEntity.LastName,
                EmailAddress = userEntity.EmailAddress,
                IsActive = userEntity.IsActive,
                UserRole = userEntity.UserRole,
                CredentialsId = includeCredentials ? userEntity.CredentialsId : 0,
                Credentials = includeCredentials ? new UserCredentialsModel
                {
                    Id = userEntity.Credentials?.Id ?? 0,
                    PasswordHash = userEntity.Credentials?.PasswordHash ?? string.Empty,
                    RefreshToken = userEntity.Credentials?.RefreshToken ?? string.Empty,
                    CreatedAt = userEntity.Credentials?.CreatedAt ?? DateTime.MinValue,
                    CreatedBy = userEntity.Credentials?.CreatedBy ?? string.Empty,
                    UpdatedAt = userEntity.Credentials?.UpdatedAt ?? DateTime.MinValue,
                    UpdatedBy = userEntity.Credentials?.UpdatedBy ?? string.Empty
                } : null,
                AccessRights = includeUserRights ? userEntity.AccessRights.Select(ar => new AccessRightModel
                {
                    Id = ar.Id,
                    Name = ar.AccessRight.Name,
                    Deny = ar.Deny,
                    CanCreate = ar.CanCreate,
                    CanView = ar.CanView,
                    CanEdit = ar.CanEdit,
                    CanDelete = ar.CanDelete,
                    IsActive = ar.IsActive,
                    CreatedAt = ar.CreatedAt,
                    CreatedBy = ar.CreatedBy,
                    UpdatedAt = ar.UpdatedAt,
                    UpdatedBy = ar.UpdatedBy
                }).ToList() : new List<AccessRightModel>(),
                CreatedBy = userEntity.CreatedBy,
                CreatedAt = userEntity.CreatedAt,
                UpdatedBy = userEntity.UpdatedBy,
                UpdatedAt = userEntity.UpdatedAt
            };
        }

        public async Task<List<AccessRightModel>> GetAvailableAccessRights()
        {
            var accessRightEntities = await _accessRightsRepository.GetAll(false);

            return accessRightEntities.Select(right => new AccessRightModel
            {
                Id = right.Id,
                Name = right.Name,
                Deny = true,
                CanView = false,
                CanCreate = false,
                CanEdit = false,
                CanDelete = false,
                IsActive = true,
                CreatedAt = right.CreatedAt,
                CreatedBy = right.CreatedBy,
                UpdatedAt = right.UpdatedAt,
                UpdatedBy = right.UpdatedBy
            }).ToList();

        }

        public async Task AddUser(UserModel user)
        {
            var accessRightEntities = await _accessRightsRepository.GetAll(false);

            var userEntity = new UserEntity
            {
                Name = user.Name,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                IsActive = user.IsActive,
                UserRole = user.UserRole,
                Credentials = new UserCredentialsEntity
                {
                    PasswordHash = string.Empty,
                    RefreshToken = string.Empty
                },
                AccessRights = accessRightEntities.Select(ar => new UserAccessRightEntity
                {
                    AccessRightId = ar.Id,
                    Deny = true,
                    CanCreate = false,
                    CanView = false,
                    CanEdit = false,
                    CanDelete = false,
                    IsActive = true
                }).ToList()
            };

            await _userRepository.Insert(userEntity, e => e.EmailAddress == userEntity.EmailAddress);

        }

        public async Task UpdateUser(UserModel user, bool updateCredentials)
        {
            var userEntities = await _userRepository.GetById(user.Id, false, includes: includeExpression);

            if (userEntities == null || userEntities.Count > 1)
            {
                return;
            }

            var userEntityToUpdate = userEntities.FirstOrDefault();

            if (userEntityToUpdate == null)
            {
                return;
            }

            userEntityToUpdate.Name = user.Name;
            userEntityToUpdate.LastName = user.LastName;
            userEntityToUpdate.EmailAddress = user.EmailAddress;
            userEntityToUpdate.IsActive = user.IsActive;
            userEntityToUpdate.UserRole = user.UserRole;

            if (updateCredentials && userEntityToUpdate.Credentials == null && user.Credentials != null)
            {
                userEntityToUpdate.Credentials = new UserCredentialsEntity
                {
                    PasswordHash = PasswordHasher.HashPassword(user.Credentials.PasswordHash),
                    RefreshToken = user.Credentials.RefreshToken
                };
            }
        }

        public async Task DeleteUser(int userId)
        {
            await _userRepository.Delete(userId);
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
