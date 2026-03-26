using Data.Database.Seeds;
using Data.Entities.Administration;
using Data.Entities.Statistics;
using Data.Entities.Tasks;
using Data.Entities.User;
using Logic.Shared;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
namespace UnitTests.Database
{
    public class TestDatabaseContext : DbContext
    {
        public TestDatabaseContext(DbContextOptions<TestDatabaseContext> options) : base(options) { }


        public static TestDatabaseContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<TestDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new TestDatabaseContext(options);
            var timestamp = DateTime.UtcNow;

            context.Database.EnsureCreated();

            context.UserTable.AddRange(new List<UserEntity>
            {
                new UserEntity
                {
                    Id = 1,
                    Name = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    UserRole = UserRoleEnum.Admin,
                    IsActive = true,
                    AccessRights = new List<UserAccessRightEntity>
                    {
                        new UserAccessRightEntity
                        {
                            AccessRightId = 1,
                            Deny = false,
                            CanCreate = true,
                            CanView = true,
                            CanEdit = true,
                            CanDelete = true,
                            IsActive = true
                        },
                        new UserAccessRightEntity
                        {
                            AccessRightId = 2,
                            Deny = false,
                            CanCreate = true,
                            CanView = true,
                            CanEdit = true,
                            CanDelete = true,
                            IsActive = true
                        },
                        new UserAccessRightEntity
                        {
                            AccessRightId = 3,
                            Deny = false,
                            CanCreate = true,
                            CanView = true,
                            CanEdit = true,
                            CanDelete = true,
                            IsActive = true
                        },
                        new UserAccessRightEntity
                        {
                            AccessRightId = 4,
                            Deny = false,
                            CanCreate = true,
                            CanView = true,
                            CanEdit = true,
                            CanDelete = true,
                            IsActive = true
                        }
                    },
                    Credentials = new UserCredentialsEntity
                    {
                            Id = 1,
                            PasswordHash = PasswordHasher.HashPassword("Pass@word"),
                            CreatedAt = timestamp,
                            CreatedBy = "System",
                            UpdatedAt = timestamp,
                            UpdatedBy = "System",
                            RefreshToken = string.Empty
                    },
                    CreatedAt = timestamp,
                    CreatedBy = "System",
                    UpdatedAt = timestamp,
                    UpdatedBy = "System"
                },
                new UserEntity
                {
                    Id = 2,
                    Name = "Max",
                    LastName = "Mustermann",
                    EmailAddress = "max@example.com",
                    UserRole = UserRoleEnum.User,
                    IsActive = true,
                    AccessRights = new List<UserAccessRightEntity>
                    {
                        new UserAccessRightEntity
                        {
                            AccessRightId = 5,
                            Deny = true,
                            CanCreate = false,
                            CanView = false,
                            CanEdit = false,
                            CanDelete = false,
                            IsActive = false
                        },
                        new UserAccessRightEntity
                        {
                            AccessRightId = 6,
                            Deny = true,
                            CanCreate = false,
                            CanView = false,
                            CanEdit = false,
                            CanDelete = false,
                            IsActive = false
                        },
                        new UserAccessRightEntity
                        {
                            AccessRightId = 7,
                            Deny = false,
                            CanCreate = false,
                            CanView = true,
                            CanEdit = false,
                            CanDelete = false,
                            IsActive = true
                        },
                        new UserAccessRightEntity
                        {
                            AccessRightId = 8,
                            Deny = true,
                            CanCreate = false,
                            CanView = false,
                            CanEdit = false,
                            CanDelete = false,
                            IsActive = false
                        }
                    },
                    Credentials = new UserCredentialsEntity
                    {
                            Id = 2,
                            PasswordHash = PasswordHasher.HashPassword("Pass@word"),
                            CreatedAt = timestamp,
                            CreatedBy = "System",
                            UpdatedAt = timestamp,
                            UpdatedBy = "System",
                            RefreshToken = string.Empty
                    },
                    CreatedAt = timestamp,
                    CreatedBy = "System",
                    UpdatedAt = timestamp,
                    UpdatedBy = "System"
                }
            });

            context.SaveChanges();

            return context;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.AssignedUser)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new AccessRightsSeed());
        }

        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        public DbSet<UserEntity> UserTable { get; set; }
        public DbSet<UserCredentialsEntity> UserCredentialsTable { get; set; }
        public DbSet<TaskEntity> TaskTable { get; set; }
        public DbSet<EndpointStatisticEntity> EndpointStatisticTable { get; set; }
    }
}
