using CapStone.Domain.Entities;
using CapStone.Infrastructure.Data;
using CapStone.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CapStone.Test.Repositories
{
    public class RepositoryTests
    {
        private AccidentDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AccidentDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AccidentDbContext(options);
        }

        [Fact]
        public async Task AddAsync_ShouldAddEntityToDatabase()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository<User>(context);
            var user = new User { Name = "Test User", Email = "test@user.com", PasswordHash = "hash" };

            // Act
            var addedUser = await repository.AddAsync(user);
            await context.SaveChangesAsync(); // Save changes since AddAsync doesn't call it (based on common patterns, though need to check the exact implementation. Wait, Repository<T> doesn't call SaveChanges).

            // Assert
            var result = await context.Users.FindAsync(addedUser.Id);
            Assert.NotNull(result);
            Assert.Equal("Test User", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnEntity_WhenItExists()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var repository = new Repository<User>(context);
            var user = new User { Name = "Test User", Email = "test@user.com", PasswordHash = "hash" };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
        }
    }
}
