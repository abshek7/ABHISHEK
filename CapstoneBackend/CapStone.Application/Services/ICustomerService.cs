using CapStone.Application.DTOs.Customer;

namespace CapStone.Application.Services
{
    public interface ICustomerService
    {
        Task CreatePolicyRequestAsync(Guid customerId, CreatePolicyRequestDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyRequestResponseDto>> GetMyPolicyRequestsAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyResponseDto>> GetMyPoliciesAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<EmiInstallmentDto>> GetEmiScheduleAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PolicyTypeResponseDto>> GetPolicyTypesAsync(CancellationToken cancellationToken = default);
        Task CreateClaimAsync(Guid customerId, CreateClaimDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ClaimResponseDto>> GetMyClaimsAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PaymentResponseDto>> GetMyPaymentsAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task CreatePaymentAsync(Guid customerId, CreatePaymentDto dto, CancellationToken cancellationToken = default);
        Task AddNomineeAsync(Guid customerId, CreateNomineeDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<NomineeResponseDto>> GetNomineesAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<NotificationDto>> GetNotificationsAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<Guid> ConfirmPurchaseAsync(Guid customerId, ConfirmPurchaseDto dto, CancellationToken cancellationToken = default);
        Task RenewPolicyAsync(Guid customerId, RenewalRequestDto dto, CancellationToken cancellationToken = default);
        Task<InvoiceDto> GetInvoiceAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default);
    }
}
