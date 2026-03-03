using CapStone.Application.DTOs.Admin;
using CapStone.Application.DTOs.Customer;

namespace CapStone.Application.Services
{
    public interface IAdminService
    {
        Task CreateAgentAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
        Task CreateClaimsOfficerAsync(CreateUserDto dto, CancellationToken cancellationToken = default);
        //Task<Guid> CreatePolicyTypeAsync(CreatePolicyTypeDto dto, CancellationToken cancellationToken = default);
        //Task AddPolicyCoverageAsync(CreatePolicyCoverageDto dto, CancellationToken cancellationToken = default);
        Task AssignAgentToRequestAsync(AssignAgentToRequestDto dto, CancellationToken cancellationToken = default);
        Task<Guid> CreatePolicyAsync(CreatePolicyDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AgentWithWorkloadDto>> GetAgentsWithWorkloadAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<OfficerWithWorkloadDto>> GetOfficersWithWorkloadAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyRequestResponseDto>> GetUnassignedRequestsAsync(CancellationToken cancellationToken = default);
        Task AssignAgentByLeastWorkloadAsync(Guid requestId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ClaimResponseDto>> GetUnassignedClaimsAsync(CancellationToken cancellationToken = default);
        Task AssignClaimByLeastWorkloadAsync(Guid claimId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyRequestResponseDto>> GetAllRequestsAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ClaimResponseDto>> GetAllClaimsAsync(CancellationToken cancellationToken = default);
        Task<AnalyticsDto> GetAnalyticsAsync(CancellationToken cancellationToken = default);
        Task<RevenueReportDto> GetRevenueReportAsync(CancellationToken cancellationToken = default);
        Task<AgentPerformanceReportDto> GetAgentPerformanceReportAsync(CancellationToken cancellationToken = default);
        Task UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}