using Data.Database;
using Data.Entities.User;
using Logic.Administration.Interfaces;
using Logic.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shared.Models.User;

namespace Core.Api.Bundels
{
    internal static class Database
    {
        internal static void Migrate(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DatabaseContext>>();

                try
                {
                    if (db.Database.GetPendingMigrations().Any())
                    {
                        logger.LogInformation("Applying pending migrations...");
                        db.Database.Migrate();
                        logger.LogInformation("Migrations applied successfully.");
                    }
                    else
                    {
                        logger.LogInformation("No pending migrations.");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database. Continuing with application startup...");
                    // Don't crash the application if migrations fail
                    // This allows the app to start even if there are migration issues
                }
            }
        }

        internal static void SeedDefaultAdminUser(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
             
                var defaultAdminUserOptions = scope.ServiceProvider.GetRequiredService<IOptions<UserModel>>();

                var defaultAdminUser = defaultAdminUserOptions.Value;

                if (db == null || defaultAdminUser == null)
                {
                    return;
                }

                var existingUser = db.UserTable.FirstOrDefault(e => e.Id == defaultAdminUser.Id);

                if (existingUser == null)
                {
                    db.UserTable.Add(new UserEntity
                    {
                        Name = defaultAdminUser.Name,
                        LastName = defaultAdminUser.LastName,
                        EmailAddress = defaultAdminUser.EmailAddress,
                        IsActive = defaultAdminUser.IsActive,
                        UserRole = defaultAdminUser.UserRole,
                        Credentials = new UserCredentialsEntity
                        {
                            PasswordHash = PasswordHasher.HashPassword(defaultAdminUser.Credentials?.PasswordHash),
                            RefreshToken = defaultAdminUser.Credentials?.RefreshToken ?? string.Empty
                        }
                    });
                   
                    db.SaveChanges();
                }
            }
        }
    }
}
