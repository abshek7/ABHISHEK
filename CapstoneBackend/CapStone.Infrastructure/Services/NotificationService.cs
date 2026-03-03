using CapStone.Application.DTOs.Customer;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<InsuranceClaim> _claimRepository;
        private readonly IPaymentService _paymentService;

        public NotificationService(
            IRepository<Policy> policyRepository,
            IRepository<InsuranceClaim> claimRepository,
            IPaymentService paymentService)
        {
            _policyRepository = policyRepository;
            _claimRepository = claimRepository;
            _paymentService = paymentService;
        }

        public async Task<IReadOnlyList<NotificationDto>> GetNotificationsAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var notifications = new List<NotificationDto>();
            var now = DateTime.UtcNow;

            var policies = await _policyRepository.GetQueryable()
                .Where(p => p.CustomerId == customerId)
                .ToListAsync(cancellationToken);

            foreach (var policy in policies.Where(p => p.Status == PolicyStatus.Active))
            {
                var daysToEnd = (policy.EndDate - now).TotalDays;
                if (daysToEnd >= 0 && daysToEnd <= 30)
                {
                    notifications.Add(new NotificationDto
                    {
                        Type = "PolicyRenewal",
                        Message = $"Policy {policy.PolicyNumber} is due for renewal on {policy.EndDate:d}.",
                        CreatedAt = now
                    });
                }
            }

            var emiPolicies = policies.Where(p => p.Status == PolicyStatus.Active).Select(p => p.Id).ToList();
            foreach (var policyId in emiPolicies)
            {
                var schedule = await _paymentService.GetEmiScheduleAsync(customerId, policyId, cancellationToken);
                foreach (var installment in schedule)
                {
                    var daysToDue = (installment.DueDate - now).TotalDays;
                    if (!installment.IsPaid && daysToDue >= 0 && daysToDue <= 7)
                    {
                        notifications.Add(new NotificationDto
                        {
                            Type = "PaymentDue",
                            Message = $"An installment for policy {policyId} is due on {installment.DueDate:d} for amount {installment.Amount}.",
                            CreatedAt = now
                        });
                    }
                }
            }

            var claims = await _claimRepository.GetQueryable()
                .Where(c => c.CustomerId == customerId)
                .ToListAsync(cancellationToken);

            foreach (var claim in claims)
            {
                if (claim.Status == ClaimStatus.UnderReview || claim.Status == ClaimStatus.Approved || claim.Status == ClaimStatus.Rejected || claim.Status == ClaimStatus.Settled)
                {
                    notifications.Add(new NotificationDto
                    {
                        Type = "ClaimUpdate",
                        Message = $"Claim {claim.Id} status is {claim.Status}.",
                        CreatedAt = now
                    });
                }
            }

            return notifications;
        }
    }
}
