using AutoMapper;
using CapStone.Application.DTOs.Admin;
using CapStone.Application.Exceptions;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using CapStone.Infrastructure.Services;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CapStone.Test.Services
{
    public class AdminServiceTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        // Other mocks can be created but we only pass them to the constructor.
        private readonly AdminService _adminService;

        public AdminServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            
            // Provide dummy mocks for the rest of the dependencies
            var mockPolicyRequestRepository = new Mock<IRepository<PolicyRequest>>();
            var mockPolicyTypeRepository = new Mock<IRepository<PolicyType>>();
            var mockClaimRepository = new Mock<IRepository<InsuranceClaim>>();
            var mockPolicyRepository = new Mock<IRepository<Policy>>();
            var mockUnderwritingService = new Mock<IUnderwritingService>();
            var mockPolicyService = new Mock<IPolicyService>();
            var mockMapper = new Mock<IMapper>();

            _adminService = new AdminService(
                _mockUserRepository.Object,
                mockPolicyRequestRepository.Object,
                mockPolicyTypeRepository.Object,
                mockClaimRepository.Object,
                mockPolicyRepository.Object,
                _mockUnitOfWork.Object,
                mockUnderwritingService.Object,
                mockPolicyService.Object,
                mockMapper.Object
            );
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync((User?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _adminService.DeleteUserAsync(userId));
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldRemoveUserAndSaveChanges_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { Id = userId, Name = "Existing User" };
            
            _mockUserRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                               .ReturnsAsync(user);

            // Act
            await _adminService.DeleteUserAsync(userId);

            // Assert
            _mockUserRepository.Verify(r => r.Remove(user), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
