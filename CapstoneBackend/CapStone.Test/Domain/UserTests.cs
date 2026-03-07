using CapStone.Domain.Entities;
using System;
using Xunit;

namespace CapStone.Test.Domain
{
    public class UserTests
    {
        [Fact]
        public void User_ShouldHaveDefaultValues_WhenInstantiated()
        {
            // Arrange & Act
            var user = new User();

            // Assert
            Assert.NotEqual(Guid.Empty, user.Id);
            Assert.True(user.IsActive);
            Assert.True((DateTime.UtcNow - user.CreatedAt).TotalSeconds < 1);
        }

        [Fact]
        public void User_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var user = new User();
            var name = "John Doe";
            var email = "john@example.com";

            // Act
            user.Name = name;
            user.Email = email;
            user.IsActive = false;

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.False(user.IsActive);
        }
    }
}
