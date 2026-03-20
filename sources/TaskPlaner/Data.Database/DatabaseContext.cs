using Data.Entities.Administration;
using Data.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Data.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<LogMessageEntity> LogMessageTable { get; set; }
        public DbSet<UserEntity> UserTable { get; set; }
        public DbSet<UserCredentialsEntity> UserCredentialsTable { get; set; }
    }
}
