using CapStone.API.Controllers;
using CapStone.Application.DTOs.Admin;
using CapStone.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CapStone.Test.Controllers
{
    public class AdminControllerTests
    {
        private readonly Mock<IAdminService> _mockAdminService;
        private readonly AdminController _controller;

        public AdminControllerTests()
        {
            _mockAdminService = new Mock<IAdminService>();
            _controller = new AdminController(_mockAdminService.Object);
        }

        [Fact]
        public async Task CreateAgent_ShouldReturnOk_WhenAgentCreatedSuccessfully()
        {
            // Arrange
            var dto = new CreateUserDto { Name = "Test Agent", Email = "agent@test.com", Password = "Pass123" };
            _mockAdminService.Setup(s => s.CreateAgentAsync(dto, It.IsAny<CancellationToken>()))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateAgent(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            // Expected dynamic object { message = "Agent created successfully" }
            var property = okResult.Value.GetType().GetProperty("message");
            Assert.NotNull(property);
            var message = property.GetValue(okResult.Value) as string;
            Assert.Equal("Agent created successfully", message);
            
            _mockAdminService.Verify(s => s.CreateAgentAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
