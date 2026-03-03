using AutoMapper;
using CapStone.Application.DTOs.Customer;
using CapStone.Application.Exceptions;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(
            IRepository<Payment> paymentRepository,
            IRepository<Policy> policyRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreatePaymentAsync(Guid customerId, CreatePaymentDto dto, CancellationToken cancellationToken = default)
        {
            Policy? policy = null;
            if (Guid.TryParse(dto.PolicyId, out Guid policyId))
            {
                policy = await _policyRepository.GetByIdAsync(policyId, cancellationToken);
            }

            if (policy == null)
            {
                policy = await _policyRepository.GetQueryable()
                    .FirstOrDefaultAsync(p => p.PolicyNumber == dto.PolicyId, cancellationToken);
            }

            if (policy == null)
                throw new NotFoundException("Policy not found");
            if (policy.CustomerId != customerId)
                throw new UnauthorizedException("Policy does not belong to you");

            var payment = new Payment
            {
                PolicyId = policy.Id,
                Amount = dto.Amount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = PaymentStatus.Paid,
                PaymentDate = DateTime.UtcNow
            };
            
            await _paymentRepository.AddAsync(payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<PaymentResponseDto>> GetMyPaymentsAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var policyIds = _policyRepository.GetQueryable()
                .Where(p => p.CustomerId == customerId)
                .Select(p => p.Id);
            var payments = await _paymentRepository.GetQueryable()
                .Where(p => policyIds.Contains(p.PolicyId))
                .ToListAsync(cancellationToken);
            return _mapper.Map<List<PaymentResponseDto>>(payments);
        }

        public async Task<IReadOnlyList<EmiInstallmentDto>> GetEmiScheduleAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetQueryable()
                .Include(p => p.PolicyType)
                .FirstOrDefaultAsync(p => p.Id == policyId, cancellationToken);
            if (policy == null)
                throw new NotFoundException("Policy not found");
            if (policy.CustomerId != customerId)
                throw new UnauthorizedException("Policy does not belong to you");

            var months = policy.PolicyType != null && policy.PolicyType.DurationMonths > 0
                ? policy.PolicyType.DurationMonths
                : Math.Max(1, (int)Math.Round((policy.EndDate - policy.StartDate).TotalDays / 30.0));

            var totalPaid = await _paymentRepository.GetQueryable()
                .Where(p => p.PolicyId == policyId && p.PaymentStatus == PaymentStatus.Paid)
                .SumAsync(p => p.Amount, cancellationToken);

            var baseAmount = months > 0 ? Math.Round(policy.FinalPremium / months, 2) : policy.FinalPremium;
            var installments = new List<EmiInstallmentDto>();
            var remaining = policy.FinalPremium;

            for (var i = 1; i <= months; i++)
            {
                var amount = i < months ? baseAmount : remaining;
                remaining -= amount;
                var dueDate = policy.StartDate.AddMonths(i);
                var threshold = baseAmount > 0 ? (int)Math.Floor((decimal)(totalPaid / baseAmount)) : 0;
                var isPaid = i <= threshold;
                installments.Add(new EmiInstallmentDto
                {
                    PolicyId = policyId,
                    InstallmentNumber = i,
                    DueDate = dueDate,
                    Amount = amount,
                    IsPaid = isPaid
                });
            }

            return installments;
        }

        public async Task<InvoiceDto> GetInvoiceAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetQueryable()
                .Include(p => p.Customer)
                .Include(p => p.PolicyType)
                .FirstOrDefaultAsync(p => p.Id == policyId, cancellationToken);

            if (policy == null)
                throw new NotFoundException("Policy not found");
            if (policy.CustomerId != customerId)
                throw new UnauthorizedException("Policy does not belong to you");

            var totalPaid = await _paymentRepository.GetQueryable()
                .Where(p => p.PolicyId == policyId && p.PaymentStatus == PaymentStatus.Paid)
                .SumAsync(p => p.Amount, cancellationToken);

            return new InvoiceDto
            {
                PolicyNumber = policy.PolicyNumber,
                CustomerName = policy.Customer?.Name ?? "N/A",
                TotalAmount = policy.FinalPremium,
                AmountPaid = totalPaid,
                IssuedDate = DateTime.UtcNow,
                Status = totalPaid >= policy.FinalPremium ? "Fully Paid" : "Partially Paid"
            };
        }
    }
}
