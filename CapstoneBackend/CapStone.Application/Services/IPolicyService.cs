using CapStone.Application.DTOs.Admin;
using CapStone.Application.DTOs.Agent;
using CapStone.Application.DTOs.Customer;
using CapStone.Domain.Enums;

namespace CapStone.Application.Services
{
    public interface IPolicyService
    {
        Task<Guid> CreatePolicyAsync(CreatePolicyDto dto, CancellationToken cancellationToken = default);
        Task<Guid> CreatePolicyDirectAsync(Guid agentId, CreatePolicyDirectDto dto, CancellationToken cancellationToken = default);
        Task RenewPolicyAsync(Guid customerId, RenewalRequestDto dto, CancellationToken cancellationToken = default);
        Task<Guid> ConfirmPurchaseAsync(Guid customerId, ConfirmPurchaseDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyResponseDto>> GetMyPoliciesAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyResponseDto>> GetAssignedPoliciesAsync(Guid agentId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyTypeResponseDto>> GetPolicyTypesAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<NomineeResponseDto>> GetNomineesAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default);
        Task AddNomineeAsync(Guid customerId, CreateNomineeDto dto, CancellationToken cancellationToken = default);
    }
}
