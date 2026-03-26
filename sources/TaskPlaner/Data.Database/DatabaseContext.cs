using Data.Database.Seeds;
using Data.Entities.Administration;
using Data.Entities.Statistics;
using Data.Entities.Tasks;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

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
