using AutoMapper;
using CapStone.Application.DTOs.Admin;
using CapStone.Application.DTOs.Customer;
using CapStone.Application.Exceptions;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public class AdminService : IAdminService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PolicyRequest> _policyRequestRepository;
        private readonly IRepository<PolicyType> _policyTypeRepository;
        private readonly IRepository<InsuranceClaim> _claimRepository;
        private readonly IRepository<Policy> _policyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnderwritingService _underwritingService;
        private readonly IPolicyService _policyService;
        private readonly IMapper _mapper;

        public AdminService(
            IRepository<User> userRepository,
            IRepository<PolicyRequest> policyRequestRepository,
            IRepository<PolicyType> policyTypeRepository,
            IRepository<InsuranceClaim> claimRepository,
            IRepository<Policy> policyRepository,
            IUnitOfWork unitOfWork,
            IUnderwritingService underwritingService,
            IPolicyService policyService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _policyRequestRepository = policyRequestRepository;
            _policyTypeRepository = policyTypeRepository;
            _claimRepository = claimRepository;
            _policyRepository = policyRepository;
            _unitOfWork = unitOfWork;
            _underwritingService = underwritingService;
            _policyService = policyService;
            _mapper = mapper;
        }

        public async Task CreateAgentAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var exists = await _userRepository.GetQueryable().AnyAsync(x => x.Email == dto.Email, cancellationToken);
            if (exists)
                throw new ConflictException("Email already exists");

            var user = _mapper.Map<User>(dto);
            user.Role = UserRole.Agent;
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateClaimsOfficerAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var exists = await _userRepository.GetQueryable().AnyAsync(x => x.Email == dto.Email, cancellationToken);
            if (exists)
                throw new ConflictException("Email already exists");

            var user = _mapper.Map<User>(dto);
            user.Role = UserRole.ClaimsOfficer;
            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        /*
        public async Task<Guid> CreatePolicyTypeAsync(CreatePolicyTypeDto dto, CancellationToken cancellationToken = default)
        {
            var admin = await _userRepository.GetQueryable().FirstOrDefaultAsync(u => u.Role == UserRole.Admin, cancellationToken);
            if (admin == null)
                throw new NotFoundException("No admin found");
            var policyType = _mapper.Map<PolicyType>(dto);
            policyType.Status = "Active";
            policyType.CreatedBy = admin.Id;
            await _policyTypeRepository.AddAsync(policyType, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return policyType.Id;
        }

        public async Task AddPolicyCoverageAsync(CreatePolicyCoverageDto dto, CancellationToken cancellationToken = default)
        {
            var coverage = _mapper.Map<PolicyCoverage>(dto);
            await _policyCoverageRepository.AddAsync(coverage, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        */

        public async Task AssignAgentToRequestAsync(AssignAgentToRequestDto dto, CancellationToken cancellationToken = default)
        {
            var request = await _policyRequestRepository.GetByIdAsync(dto.RequestId, cancellationToken);
            if (request == null)
                throw new NotFoundException("Policy request not found");

            var agentExists = await _userRepository.GetQueryable()
                .AnyAsync(x => x.Id == dto.AgentId && x.Role == UserRole.Agent, cancellationToken);
            if (!agentExists)
                throw new NotFoundException("Invalid agent");

            request.AssignedAgentId = dto.AgentId;
            request.Status = RequestStatus.Assigned;
            _policyRequestRepository.Update(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public Task<Guid> CreatePolicyAsync(CreatePolicyDto dto, CancellationToken cancellationToken = default) 
            => _policyService.CreatePolicyAsync(dto, cancellationToken);

        public async Task<IReadOnlyList<AgentWithWorkloadDto>> GetAgentsWithWorkloadAsync(CancellationToken cancellationToken = default)
        {
            var agents = await _userRepository.GetQueryable()
                .Where(u => u.Role == UserRole.Agent)
                .ToListAsync(cancellationToken);
            var openStatuses = new[] { RequestStatus.New, RequestStatus.Assigned, RequestStatus.UnderReview };
            var requests = await _policyRequestRepository.GetQueryable()
                .Where(r => r.AssignedAgentId != null && openStatuses.Contains(r.Status))
                .ToListAsync(cancellationToken);
            var countByAgent = requests.GroupBy(r => r.AssignedAgentId!.Value).ToDictionary(g => g.Key, g => g.Count());

            return agents.Select(a => new AgentWithWorkloadDto
            {
                Id = a.Id,
                Name = a.Name,
                Email = a.Email,
                OpenRequestCount = countByAgent.GetValueOrDefault(a.Id, 0)
            }).ToList();
        }

        public async Task<IReadOnlyList<OfficerWithWorkloadDto>> GetOfficersWithWorkloadAsync(CancellationToken cancellationToken = default)
        {
            var officers = await _userRepository.GetQueryable()
                .Where(u => u.Role == UserRole.ClaimsOfficer)
                .ToListAsync(cancellationToken);
            var claims = await _claimRepository.GetQueryable()
                .Where(c => c.OfficerId != null && c.Status != ClaimStatus.Settled && c.Status != ClaimStatus.Rejected)
                .ToListAsync(cancellationToken);
            var countByOfficer = claims.GroupBy(c => c.OfficerId!.Value).ToDictionary(g => g.Key, g => g.Count());

            return officers.Select(o => new OfficerWithWorkloadDto
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email,
                AssignedClaimCount = countByOfficer.GetValueOrDefault(o.Id, 0)
            }).ToList();
        }

        public async Task<IReadOnlyList<PolicyRequestResponseDto>> GetUnassignedRequestsAsync(CancellationToken cancellationToken = default)
        {
            var query = _policyRequestRepository.GetQueryable()
                .Where(r => r.AssignedAgentId == null)
                .Include(r => r.PolicyType)
                .Include(r => r.AssignedAgent);
            var list = await query.ToListAsync(cancellationToken);
            var dtoList = _mapper.Map<List<PolicyRequestResponseDto>>(list);
            
            foreach (var dto in dtoList)
            {
                var request = list.First(r => r.Id == dto.Id);
                var user = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken);
                if (user != null && request.PolicyType != null)
                {
                    var (riskScore, premium, isEligible) = _underwritingService.CalculateRiskAndPremium(user, request, request.PolicyType);
                    dto.SuggestedRiskScore = riskScore;
                    dto.SuggestedPremium = premium;
                    dto.SuggestedCoverage = request.PolicyType.BaseCoverageAmount / (riskScore > 0 ? riskScore : 1);
                }
            }
            return dtoList;
        }

        public async Task<IReadOnlyList<PolicyRequestResponseDto>> GetAllRequestsAsync(CancellationToken cancellationToken = default)
        {
            var query = _policyRequestRepository.GetQueryable()
                .Include(r => r.PolicyType)
                .Include(r => r.AssignedAgent);
            var list = await query.ToListAsync(cancellationToken);
            var dtoList = _mapper.Map<List<PolicyRequestResponseDto>>(list);

            foreach (var dto in dtoList)
            {
                var request = list.First(r => r.Id == dto.Id);
                var user = await _userRepository.GetByIdAsync(request.CustomerId, cancellationToken);
                if (user != null && request.PolicyType != null)
                {
                    var (riskScore, premium, isEligible) = _underwritingService.CalculateRiskAndPremium(user, request, request.PolicyType);
                    dto.SuggestedRiskScore = riskScore;
                    dto.SuggestedPremium = premium;
                    dto.SuggestedCoverage = request.PolicyType.BaseCoverageAmount / (riskScore > 0 ? riskScore : 1);
                }
            }
            return dtoList;
        }

        public async Task AssignAgentByLeastWorkloadAsync(Guid requestId, CancellationToken cancellationToken = default)
        {
            var request = await _policyRequestRepository.GetByIdAsync(requestId, cancellationToken);
            if (request == null)
                throw new NotFoundException("Policy request not found");
            if (request.AssignedAgentId != null)
                throw new ConflictException("Request is already assigned");

            var openStatuses = new[] { RequestStatus.New, RequestStatus.Assigned, RequestStatus.UnderReview };
            var agentCounts = await _policyRequestRepository.GetQueryable()
                .Where(r => r.AssignedAgentId != null && openStatuses.Contains(r.Status))
                .GroupBy(r => r.AssignedAgentId!.Value)
                .Select(g => new { AgentId = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);
            var allAgents = await _userRepository.GetQueryable()
                .Where(u => u.Role == UserRole.Agent)
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
            var countByAgent = agentCounts.ToDictionary(x => x.AgentId, x => x.Count);
            var agentId = allAgents.OrderBy(id => countByAgent.GetValueOrDefault(id, 0)).FirstOrDefault();
            if (agentId == default)
                throw new NotFoundException("No agent available");

            request.AssignedAgentId = agentId;
            request.Status = RequestStatus.Assigned;
            _policyRequestRepository.Update(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<ClaimResponseDto>> GetUnassignedClaimsAsync(CancellationToken cancellationToken = default)
        {
            var query = _claimRepository.GetQueryable()
                .Where(c => c.OfficerId == null);
            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<ClaimResponseDto>>(list);
        }

        public async Task<IReadOnlyList<ClaimResponseDto>> GetAllClaimsAsync(CancellationToken cancellationToken = default)
        {
            var query = _claimRepository.GetQueryable();
            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<ClaimResponseDto>>(list);
        }

        public async Task AssignClaimByLeastWorkloadAsync(Guid claimId, CancellationToken cancellationToken = default)
        {
            var claim = await _claimRepository.GetByIdAsync(claimId, cancellationToken);
            if (claim == null)
                throw new NotFoundException("Claim not found");
            if (claim.OfficerId != null)
                throw new ConflictException("Claim is already assigned");

            var officerCounts = await _claimRepository.GetQueryable()
                .Where(c => c.OfficerId != null && c.Status != ClaimStatus.Settled && c.Status != ClaimStatus.Rejected)
                .GroupBy(c => c.OfficerId!.Value)
                .Select(g => new { OfficerId = g.Key, Count = g.Count() })
                .ToListAsync(cancellationToken);
            var allOfficers = await _userRepository.GetQueryable()
                .Where(u => u.Role == UserRole.ClaimsOfficer)
                .Select(u => u.Id)
                .ToListAsync(cancellationToken);
            var countByOfficer = officerCounts.ToDictionary(x => x.OfficerId, x => x.Count);
            var officerId = allOfficers.OrderBy(id => countByOfficer.GetValueOrDefault(id, 0)).FirstOrDefault();
            if (officerId == default)
                throw new NotFoundException("No claims officer available");

            claim.OfficerId = officerId;
            claim.Status = ClaimStatus.UnderReview;
            _claimRepository.Update(claim);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<AnalyticsDto> GetAnalyticsAsync(CancellationToken cancellationToken = default)
        {
            var policies = await _policyRepository.GetQueryable().ToListAsync(cancellationToken);
            var claims = await _claimRepository.GetQueryable().ToListAsync(cancellationToken);
            var requests = await _policyRequestRepository.GetQueryable().ToListAsync(cancellationToken);
            var users = await _userRepository.GetQueryable().ToListAsync(cancellationToken);

            return new AnalyticsDto
            {
                TotalPolicies = policies.Count,
                ActivePolicies = policies.Count(p => p.Status == PolicyStatus.Active),
                TotalClaims = claims.Count,
                PendingClaims = claims.Count(c => c.Status == ClaimStatus.Submitted || c.Status == ClaimStatus.UnderReview),
                TotalPolicyRequests = requests.Count,
                UnassignedRequests = requests.Count(r => r.AssignedAgentId == null),
                TotalRevenue = policies.Sum(p => p.FinalPremium),
                TotalUsers = users.Count,
                TotalAdmins = users.Count(u => u.Role == UserRole.Admin),
                TotalAgents = users.Count(u => u.Role == UserRole.Agent),
                TotalCustomers = users.Count(u => u.Role == UserRole.Customer),
                TotalClaimsOfficers = users.Count(u => u.Role == UserRole.ClaimsOfficer)
            };
        }

        public async Task<RevenueReportDto> GetRevenueReportAsync(CancellationToken cancellationToken = default)
        {
            var policies = await _policyRepository.GetQueryable().ToListAsync(cancellationToken);
            var monthlyData = policies
                .GroupBy(p => p.CreatedAt.ToString("yyyy-MM"))
                .Select(g => new MonthlyRevenueDto
                {
                    Month = g.Key,
                    Revenue = g.Sum(p => p.FinalPremium)
                })
                .OrderBy(m => m.Month)
                .ToList();

            return new RevenueReportDto
            {
                TotalRevenue = policies.Sum(p => p.FinalPremium),
                MonthlyBreakdown = monthlyData
            };
        }

        public async Task<AgentPerformanceReportDto> GetAgentPerformanceReportAsync(CancellationToken cancellationToken = default)
        {
            var agents = await _userRepository.GetQueryable().Where(u => u.Role == UserRole.Agent).ToListAsync(cancellationToken);
            var policies = await _policyRepository.GetQueryable().ToListAsync(cancellationToken);

            var performance = agents.Select(a => new AgentPerformanceDto
            {
                AgentId = a.Id,
                AgentName = a.Name,
                PoliciesSold = policies.Count(p => p.AssignedAgentId == a.Id),
                TotalCommission = policies.Where(p => p.AssignedAgentId == a.Id).Sum(p => p.AgentCommissionAmount),
                TotalRevenueGenerated = policies.Where(p => p.AssignedAgentId == a.Id).Sum(p => p.FinalPremium)
            }).ToList();

            return new AgentPerformanceReportDto
            {
                Agents = performance
            };
        }

        public async Task UpdateUserAsync(UpdateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(dto.Id, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found");

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.IsActive = dto.IsActive;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new NotFoundException("User not found");

            _userRepository.Remove(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
