using AutoMapper;
using CapStone.Application.DTOs.Admin;
using CapStone.Application.DTOs.Agent;
using CapStone.Application.DTOs.Customer;
using CapStone.Application.Exceptions;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IRepository<Policy> _policyRepository;
        private readonly IRepository<PolicyRequest> _policyRequestRepository;
        private readonly IRepository<PolicyType> _policyTypeRepository;
        private readonly IRepository<Nominee> _nomineeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private const decimal AgentCommissionRate = 0.10m;

        public PolicyService(
            IRepository<Policy> policyRepository,
            IRepository<PolicyRequest> policyRequestRepository,
            IRepository<PolicyType> policyTypeRepository,
            IRepository<Nominee> nomineeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _policyRepository = policyRepository;
            _policyRequestRepository = policyRequestRepository;
            _policyTypeRepository = policyTypeRepository;
            _nomineeRepository = nomineeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreatePolicyAsync(CreatePolicyDto dto, CancellationToken cancellationToken = default)
        {
            var request = await _policyRequestRepository.GetByIdAsync(dto.RequestId, cancellationToken);
            if (request == null)
                throw new NotFoundException("Policy request not found");
            if (request.IsEligible != true)
                throw new ConflictException("Policy request not approved");
            if (!request.AssignedAgentId.HasValue)
                throw new ConflictException("Request must have an assigned agent before creating policy");

            var policyType = await _policyTypeRepository.GetByIdAsync(request.PolicyTypeId, cancellationToken);
            if (policyType == null)
                throw new NotFoundException("Policy type not found");

            var commission = dto.FinalPremium * AgentCommissionRate;
            var policy = new Policy
            {
                RequestId = request.Id,
                PolicyNumber = $"POL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}",
                PolicyTypeId = request.PolicyTypeId,
                CustomerId = request.CustomerId,
                AssignedAgentId = request.AssignedAgentId!.Value,
                StartDate = dto.StartDate,
                EndDate = dto.StartDate.AddMonths(policyType.DurationMonths),
                FinalPremium = dto.FinalPremium,
                CoverageAmount = dto.CoverageAmount,
                AgentCommissionAmount = commission,
                Status = PolicyStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            await _policyRepository.AddAsync(policy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }

        public async Task<Guid> CreatePolicyDirectAsync(Guid agentId, CreatePolicyDirectDto dto, CancellationToken cancellationToken = default)
        {
            var policy = new Policy
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                PolicyTypeId = dto.PolicyTypeId,
                AssignedAgentId = agentId,
                PolicyNumber = $"POL-DIR-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}",
                StartDate = dto.StartDate,
                EndDate = dto.StartDate.AddMonths(12), // Default or look up from policy type
                FinalPremium = dto.FinalPremium,
                CoverageAmount = dto.CoverageAmount,
                AgentCommissionAmount = dto.FinalPremium * AgentCommissionRate,
                Status = PolicyStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            await _policyRepository.AddAsync(policy, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }

        public async Task<Guid> ConfirmPurchaseAsync(Guid customerId, ConfirmPurchaseDto dto, CancellationToken cancellationToken = default)
        {
            var request = await _policyRequestRepository.GetQueryable()
                .Include(r => r.PolicyType)
                .FirstOrDefaultAsync(r => r.Id == dto.RequestId, cancellationToken);

            if (request == null)
                throw new NotFoundException("Policy request not found");
            if (request.CustomerId != customerId)
                throw new UnauthorizedException("Request does not belong to you");
            if (request.Status != RequestStatus.Approved)
                throw new ConflictException("Request must be Approved before purchase");

            request.CompletePurchase(dto.NomineeName!, dto.NomineeRelation!);
            
            var policy = new Policy
            {
                Id = Guid.NewGuid(),
                RequestId = request.Id,
                PolicyNumber = $"POL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}",
                PolicyTypeId = request.PolicyTypeId,
                CustomerId = request.CustomerId,
                AssignedAgentId = request.AssignedAgentId!.Value,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(request.PolicyType!.DurationMonths),
                FinalPremium = request.CalculatedPremium ?? request.PolicyType.BasePremium,
                CoverageAmount = request.CoverageAmount ?? request.PolicyType.BaseCoverageAmount,
                AgentCommissionAmount = (request.CalculatedPremium ?? request.PolicyType.BasePremium) * AgentCommissionRate,
                Status = PolicyStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            await _policyRepository.AddAsync(policy, cancellationToken);

            var nominee = new Nominee
            {
                Id = Guid.NewGuid(),
                PolicyId = policy.Id,
                Name = dto.NomineeName!,
                Relationship = dto.NomineeRelation!,
                Phone = dto.NomineePhone ?? string.Empty,
                DateOfBirth = dto.NomineeDob ?? DateTime.UtcNow.AddYears(-30)
            };
            await _nomineeRepository.AddAsync(nominee, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policy.Id;
        }

        public async Task RenewPolicyAsync(Guid customerId, RenewalRequestDto dto, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(dto.PolicyId, cancellationToken);
            if (policy == null)
                throw new NotFoundException("Policy not found");
            if (policy.CustomerId != customerId)
                throw new UnauthorizedException("Policy does not belong to you");

            policy.EndDate = policy.EndDate.AddMonths(dto.DurationMonths);
            policy.Status = PolicyStatus.Active;

            _policyRepository.Update(policy);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<PolicyResponseDto>> GetMyPoliciesAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var query = _policyRepository.GetQueryable()
                .Where(p => p.CustomerId == customerId)
                .Include(p => p.PolicyType);
            var list = await query.ToListAsync(cancellationToken);
            
            await ProcessPolicyExpiries(list, cancellationToken);
            return _mapper.Map<List<PolicyResponseDto>>(list);
        }

        private async Task ProcessPolicyExpiries(IEnumerable<Policy> policies, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var updated = false;
            foreach (var policy in policies)
            {
                if (policy.Status == PolicyStatus.Active && policy.EndDate <= now)
                {
                    policy.Expire();
                    _policyRepository.Update(policy);
                    updated = true;
                }
            }
            if (updated)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IReadOnlyList<PolicyResponseDto>> GetAssignedPoliciesAsync(Guid agentId, CancellationToken cancellationToken = default)
        {
            var query = _policyRepository.GetQueryable()
                .Where(p => p.AssignedAgentId == agentId)
                .Include(p => p.PolicyType);
            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<PolicyResponseDto>>(list);
        }

        public async Task<IReadOnlyList<NomineeResponseDto>> GetNomineesAsync(Guid customerId, Guid policyId, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(policyId, cancellationToken);
            if (policy == null)
                throw new NotFoundException("Policy not found");
            if (policy.CustomerId != customerId)
                throw new UnauthorizedException("Policy does not belong to you");

            var nominees = await _nomineeRepository
                .GetQueryable()
                .Where(n => n.PolicyId == policyId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<NomineeResponseDto>>(nominees);
        }

        public async Task AddNomineeAsync(Guid customerId, CreateNomineeDto dto, CancellationToken cancellationToken = default)
        {
            var policy = await _policyRepository.GetByIdAsync(dto.PolicyId, cancellationToken);
            if (policy == null)
                throw new NotFoundException("Policy not found");
            if (policy.CustomerId != customerId)
                throw new UnauthorizedException("Policy does not belong to you");

            var nominee = new Nominee
            {
                Id = Guid.NewGuid(),
                PolicyId = dto.PolicyId,
                Name = dto.Name,
                Relationship = dto.Relationship,
                Phone = dto.Phone ?? string.Empty,
                DateOfBirth = dto.DateOfBirth
            };
            await _nomineeRepository.AddAsync(nominee, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<PolicyTypeResponseDto>> GetPolicyTypesAsync(CancellationToken cancellationToken = default)
        {
            var list = await _policyTypeRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<PolicyTypeResponseDto>>(list);
        }
    }
}
