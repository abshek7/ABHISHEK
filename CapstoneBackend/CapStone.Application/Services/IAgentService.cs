using CapStone.Application.DTOs.Agent;
using CapStone.Application.DTOs.Customer;

namespace CapStone.Application.Services
{
    public interface IAgentService
    {
        Task<IReadOnlyList<PolicyRequestResponseDto>> GetAssignedRequestsAsync(Guid agentId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyResponseDto>> GetAssignedPoliciesAsync(Guid agentId, CancellationToken cancellationToken = default);
        Task UpdateUnderwritingAsync(Guid agentId, UnderwritingDto dto, CancellationToken cancellationToken = default);
        Task<AgentCommissionSummaryDto> GetCommissionSummaryAsync(Guid agentId, CancellationToken cancellationToken = default);
        Task CreatePolicyDirectAsync(Guid agentId, CreatePolicyDirectDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<AssignedCustomerDto>> GetAssignedCustomersAsync(Guid agentId, CancellationToken cancellationToken = default);
    }
}
