using CapStone.Application.DTOs.Customer;

namespace CapStone.Application.Services
{
    public interface IPaymentService
    {
        Task CreatePaymentAsync(Guid customerId, CreatePaymentDto dto, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<PaymentResponseDto>> GetMyPaymentsAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<EmiInstallmentDto>> GetEmiScheduleAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default);
        Task<InvoiceDto> GetInvoiceAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default);
    }
}
