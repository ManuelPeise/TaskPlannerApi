using Shared.Enums;
using UnitTests.Database;

namespace UnitTests.Tests
{
    public class DatabaseTests
    {
        [Fact]
        public async Task TestDatabase()
        {
            var dbContext = TestDatabaseContext.GetDatabaseContext();

            Assert.NotNull(dbContext);
            Assert.Equal(2, await GetUsersTest(dbContext));
            Assert.Equal(1, await GetAdminUsersCount(dbContext));
        }

        [Fact]
        public async Task TestGetUsersFromContex()
        {
            using var dbContext = TestDatabaseContext.GetDatabaseContext();

            Assert.Equal(2, await GetUsersTest(dbContext));
        }

        [Fact]
        public async Task TestGetAdminUsersCount()
        {
            using var dbContext = TestDatabaseContext.GetDatabaseContext();

            Assert.Equal(1, await GetAdminUsersCount(dbContext));
        }

        private async Task<int> GetUsersTest(TestDatabaseContext context)
        {
            var users = context.UserTable.ToList();

            return users.Count;
        }

        private async Task<int> GetAdminUsersCount(TestDatabaseContext context)
        {
            var adminUsers = context.UserTable.Where(user => user.IsActive && user.UserRole == UserRoleEnum.Admin).ToList();

            return adminUsers.Count;
        }
    }
}
