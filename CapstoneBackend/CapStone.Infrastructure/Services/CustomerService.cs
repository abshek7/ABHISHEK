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
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<PolicyRequest> _policyRequestRepository;
        private readonly IRepository<InsuranceClaim> _claimRepository;
        private readonly IRepository<PolicyType> _policyTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAdminService _adminService;
        private readonly IPolicyService _policyService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;

        public CustomerService(
            IRepository<PolicyRequest> policyRequestRepository,
            IRepository<InsuranceClaim> claimRepository,
            IRepository<PolicyType> policyTypeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAdminService adminService,
            IPolicyService policyService,
            IPaymentService paymentService,
            INotificationService notificationService)
        {
            _policyRequestRepository = policyRequestRepository;
            _claimRepository = claimRepository;
            _policyTypeRepository = policyTypeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _adminService = adminService;
            _policyService = policyService;
            _paymentService = paymentService;
            _notificationService = notificationService;
        }

        public async Task CreatePolicyRequestAsync(Guid customerId, CreatePolicyRequestDto dto, CancellationToken cancellationToken = default)
        {
            var policyType = await _policyTypeRepository.GetByIdAsync(dto.PolicyTypeId, cancellationToken);
            if (policyType == null)
                throw new NotFoundException("Policy type not found");

            var request = _mapper.Map<PolicyRequest>(dto);
            request.CustomerId = customerId;
            request.RequestDate = DateTime.UtcNow;
            request.Status = RequestStatus.New;
            request.AssignedAgentId = null;
            await _policyRequestRepository.AddAsync(request, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _adminService.AssignAgentByLeastWorkloadAsync(request.Id, cancellationToken);
        }

        public async Task<IReadOnlyList<PolicyRequestResponseDto>> GetMyPolicyRequestsAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var query = _policyRequestRepository.GetQueryable()
                .Where(r => r.CustomerId == customerId)
                .Include(r => r.PolicyType)
                .Include(r => r.AssignedAgent);
            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<PolicyRequestResponseDto>>(list);
        }

        public Task<IReadOnlyList<PolicyResponseDto>> GetMyPoliciesAsync(Guid customerId, CancellationToken cancellationToken = default) 
            => _policyService.GetMyPoliciesAsync(customerId, cancellationToken);

        public Task<IReadOnlyList<PolicyTypeResponseDto>> GetPolicyTypesAsync(CancellationToken cancellationToken = default) 
            => _policyService.GetPolicyTypesAsync(cancellationToken);

        public Task<IReadOnlyList<EmiInstallmentDto>> GetEmiScheduleAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default) 
            => _paymentService.GetEmiScheduleAsync(customerId, policyId, cancellationToken);

        public async Task CreateClaimAsync(Guid customerId, CreateClaimDto dto, CancellationToken cancellationToken = default)
        {
            // Note: Validation could move to a ClaimService if created
            var claim = _mapper.Map<InsuranceClaim>(dto);
            claim.CustomerId = customerId;
            claim.Status = ClaimStatus.Submitted;
            await _claimRepository.AddAsync(claim, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _adminService.AssignClaimByLeastWorkloadAsync(claim.Id, cancellationToken);
        }

        public async Task<IReadOnlyList<ClaimResponseDto>> GetMyClaimsAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var query = _claimRepository.GetQueryable()
                .Where(c => c.CustomerId == customerId);
            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<ClaimResponseDto>>(list);
        }

        public Task<IReadOnlyList<PaymentResponseDto>> GetMyPaymentsAsync(Guid customerId, CancellationToken cancellationToken = default) 
            => _paymentService.GetMyPaymentsAsync(customerId, cancellationToken);

        public Task CreatePaymentAsync(Guid customerId, CreatePaymentDto dto, CancellationToken cancellationToken = default) 
            => _paymentService.CreatePaymentAsync(customerId, dto, cancellationToken);

        public Task AddNomineeAsync(Guid customerId, CreateNomineeDto dto, CancellationToken cancellationToken = default) 
            => _policyService.AddNomineeAsync(customerId, dto, cancellationToken);

        public Task<IReadOnlyList<NomineeResponseDto>> GetNomineesAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default) 
            => _policyService.GetNomineesAsync(customerId, policyId, cancellationToken);

        public Task<IReadOnlyList<NotificationDto>> GetNotificationsAsync(Guid customerId, CancellationToken cancellationToken = default) 
            => _notificationService.GetNotificationsAsync(customerId, cancellationToken);

        public Task<Guid> ConfirmPurchaseAsync(Guid customerId, ConfirmPurchaseDto dto, CancellationToken cancellationToken = default) 
            => _policyService.ConfirmPurchaseAsync(customerId, dto, cancellationToken);

        public Task RenewPolicyAsync(Guid customerId, RenewalRequestDto dto, CancellationToken cancellationToken = default) 
            => _policyService.RenewPolicyAsync(customerId, dto, cancellationToken);

        public Task<InvoiceDto> GetInvoiceAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default) 
            => _paymentService.GetInvoiceAsync(customerId, policyId, cancellationToken);
    }
}
