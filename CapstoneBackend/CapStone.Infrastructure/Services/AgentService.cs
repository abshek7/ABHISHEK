using AutoMapper;
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
    public class AgentService : IAgentService
    {
        private readonly IRepository<PolicyRequest> _policyRequestRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnderwritingService _underwritingService;
        private readonly IPolicyService _policyService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AgentService(
            IRepository<PolicyRequest> policyRequestRepository,
            IRepository<User> userRepository,
            IUnderwritingService underwritingService,
            IPolicyService policyService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _policyRequestRepository = policyRequestRepository;
            _userRepository = userRepository;
            _underwritingService = underwritingService;
            _policyService = policyService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<PolicyRequestResponseDto>> GetAssignedRequestsAsync(Guid agentId, CancellationToken cancellationToken = default)
        {
            var query = _policyRequestRepository.GetQueryable()
                .Where(r => r.AssignedAgentId == agentId)
                .Include(r => r.PolicyType)
                .Include(r => r.AssignedAgent);
            var list = await query.ToListAsync(cancellationToken);
            var dtoList = _mapper.Map<List<PolicyRequestResponseDto>>(list);

            foreach (var dto in dtoList)
            {
                var request = list.First(r => r.Id == dto.Id);
                
                dto.PersonalHabits = request.PersonalHabits;
                dto.MedicalHistory = request.MedicalHistory;

                var user = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken);
                if (user != null)
                {
                    dto.CustomerName = user.Name;
                    dto.CustomerEmail = user.Email;
                    dto.CustomerPhone = user.Phone;
                    dto.CustomerDateOfBirth = user.DateOfBirth;
                    dto.CustomerOccupation = user.Occupation;

                    if (request.PolicyType != null)
                    {
                        var (riskScore, premium, isEligible) = _underwritingService.CalculateRiskAndPremium(user, request, request.PolicyType);
                        dto.SuggestedRiskScore = riskScore;
                        dto.SuggestedPremium = premium;
                        dto.SuggestedCoverage = Math.Round(request.PolicyType.BaseCoverageAmount / (riskScore > 0 ? riskScore : 1), 2);
                    }
                }
            }

            return dtoList;
        }

        public Task<IReadOnlyList<PolicyResponseDto>> GetAssignedPoliciesAsync(Guid agentId, CancellationToken cancellationToken = default) 
            => _policyService.GetAssignedPoliciesAsync(agentId, cancellationToken);

        public async Task UpdateUnderwritingAsync(Guid agentId, UnderwritingDto dto, CancellationToken cancellationToken = default)
        {
            var request = await _policyRequestRepository.GetQueryable()
                .Include(r => r.PolicyType)
                .FirstOrDefaultAsync(r => r.Id == dto.RequestId, cancellationToken);
                
            if (request == null)
                throw new NotFoundException("Policy request not found");
            if (request.AssignedAgentId != agentId)
                throw new UnauthorizedException("Request is not assigned to you");

            var user = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (user == null)
                throw new NotFoundException("Customer not found");

            // Automated calculation based on user data and request details
            var (riskScore, premium, isEligible) = _underwritingService.CalculateRiskAndPremium(user, request, request.PolicyType!);

            if (dto.IsEligible)
            {
                var finalRiskScore = dto.OverrideRiskScore ?? riskScore;
                var finalPremium = dto.OverridePremium ?? premium;
                if (finalPremium <= 0 && request.PolicyType != null)
                    finalPremium = request.PolicyType.BasePremium;
                
                var finalCoverage = dto.OverrideCoverage ?? (request.PolicyType!.BaseCoverageAmount / (riskScore > 0 ? riskScore : 1));
                
                request.Approve(finalRiskScore, finalPremium, finalCoverage);
            }
            else
            {
                request.Reject();
            }
            
            _policyRequestRepository.Update(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AgentCommissionSummaryDto> GetCommissionSummaryAsync(Guid agentId, CancellationToken cancellationToken = default)
        {
            var policies = await _policyService.GetAssignedPoliciesAsync(agentId, cancellationToken);
            var totalCommission = policies.Sum(p => p.AgentCommissionAmount);
            return new AgentCommissionSummaryDto
            {
                TotalCommission = totalCommission,
                PoliciesSold = policies.Count
            };
        }

        public Task CreatePolicyDirectAsync(Guid agentId, CreatePolicyDirectDto dto, CancellationToken cancellationToken = default) 
            => _policyService.CreatePolicyDirectAsync(agentId, dto, cancellationToken);

        public async Task<IReadOnlyList<AssignedCustomerDto>> GetAssignedCustomersAsync(Guid agentId, CancellationToken cancellationToken = default)
        {
            var requests = await _policyRequestRepository.GetQueryable()
                .Where(r => r.AssignedAgentId == agentId)
                .ToListAsync(cancellationToken);

            var customerIds = requests.Select(r => r.CustomerId).Distinct();
            var customers = await _userRepository.GetQueryable()
                .Where(u => customerIds.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var result = new List<AssignedCustomerDto>();
            foreach (var customer in customers)
            {
                //var latestRequest = requests
                //    .Where(r => r.CustomerId == customer.Id)
                //    .OrderByDescending(r => r.CreatedAt)
                //    .FirstOrDefault();

                //result.Add(new AssignedCustomerDto
                //{
                //    CustomerId = customer.Id,
                //    Name = customer.Name,
                //    RiskScore = latestRequest?.TotalRiskScore ?? latestRequest?.SuggestedRiskScore
                //});

                var latestRequest = requests
    .Where(r => r.CustomerId == customer.Id)
    .OrderByDescending(r => r.RequestDate)
    .FirstOrDefault();

                result.Add(new AssignedCustomerDto
                {
                    CustomerId = customer.Id,
                    Name = customer.Name,
                    RiskScore = latestRequest?.TotalRiskScore
                });
            }

            return result;
        }
    }
}
