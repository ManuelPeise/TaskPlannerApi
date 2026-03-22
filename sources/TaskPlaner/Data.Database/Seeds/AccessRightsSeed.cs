using Data.Entities.Administration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Database.Seeds
{
    internal class AccessRightsSeed : IEntityTypeConfiguration<AccessRightsEntity>
    {
        public void Configure(EntityTypeBuilder<AccessRightsEntity> builder)
        {
            var createdAt = DateTime.Parse("2026-03-11T00:00:00");

            builder.HasData(
                new AccessRightsEntity
                {
                    Id = 1,
                    AccessRightGuid = Guid.Parse(AccessRightConstants.AccessRightTasksGuid),
                    Name = "TasksAdministration",
                    CreatedAt = createdAt,
                    CreatedBy = "System",
                    UpdatedAt = createdAt,
                    UpdatedBy = "System"
                },
                new AccessRightsEntity
                {
                    Id = 2,
                    AccessRightGuid = Guid.Parse(AccessRightConstants.AccessRightUserAdministrationGuid),
                    Name = "UserAdministration",
                    CreatedAt = createdAt,
                    CreatedBy = "System",
                    UpdatedAt = createdAt,
                    UpdatedBy = "System"
                },
                new AccessRightsEntity
                {
                    Id = 3,
                    AccessRightGuid = Guid.Parse(AccessRightConstants.AccessRightDashboardGuid),
                    Name = "Dashboard",
                    CreatedAt = createdAt,
                    CreatedBy = "System",
                    UpdatedAt = createdAt,
                    UpdatedBy = "System"
                }
            );
        }
    }
}
