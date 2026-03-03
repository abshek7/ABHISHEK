using AutoMapper;
using CapStone.Application.DTOs.Claim;
using CapStone.Application.DTOs.Customer;
using CapStone.Application.Exceptions;
using CapStone.Application.Repositories;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IRepository<InsuranceClaim> _claimRepository;
        private readonly IRepository<ClaimReview> _claimReviewRepository;
        private readonly IRepository<Settlement> _settlementRepository;
        private readonly IRepository<Nominee> _nomineeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClaimService(
            IRepository<InsuranceClaim> claimRepository,
            IRepository<ClaimReview> claimReviewRepository,
            IRepository<Settlement> settlementRepository,
            IRepository<Nominee> nomineeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _claimRepository = claimRepository;
            _claimReviewRepository = claimReviewRepository;
            _settlementRepository = settlementRepository;
            _nomineeRepository = nomineeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ClaimResponseDto>> GetAssignedClaimsAsync(Guid officerId, CancellationToken cancellationToken = default)
        {
            var query = _claimRepository.GetQueryable()
                .Where(c => c.OfficerId == officerId);
            var list = await query.ToListAsync(cancellationToken);
            return _mapper.Map<List<ClaimResponseDto>>(list);
        }

        public async Task<ClaimDetailResponseDto?> GetClaimByIdAsync(Guid officerId, Guid claimId, CancellationToken cancellationToken = default)
        {
            var claim = await _claimRepository.GetQueryable()
                .Include(c => c.Customer)
                .Include(c => c.Policy)
                    .ThenInclude(p => p.PolicyRequest)
                .Where(c => c.Id == claimId && c.OfficerId == officerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (claim == null)
                return null;

            var dto = _mapper.Map<ClaimDetailResponseDto>(claim);

            // Fetch Nominees separately since they are linked to PolicyId
            var nominees = await _nomineeRepository.GetQueryable()
                .Where(n => n.PolicyId == claim.PolicyId)
                .ToListAsync(cancellationToken);

            dto.Nominees = _mapper.Map<List<NomineeResponseDto>>(nominees);

            // Map fields from PolicyRequest if available
            if (claim.Policy?.PolicyRequest != null)
            {
                dto.TotalRiskScore = claim.Policy.PolicyRequest.TotalRiskScore;
                dto.MedicalHistory = claim.Policy.PolicyRequest.MedicalHistory;
                dto.PersonalHabits = claim.Policy.PolicyRequest.PersonalHabits;
            }

            // Map Occupation from Customer if available
            if (claim.Customer != null)
            {
                dto.Occupation = claim.Customer.Occupation;
            }

            return dto;
        }

        public async Task SubmitClaimReviewAsync(Guid officerId, ClaimReviewDto dto, CancellationToken cancellationToken = default)
        {
            var claim = await _claimRepository.GetByIdAsync(dto.ClaimId, cancellationToken);
            if (claim == null)
                throw new NotFoundException("Claim not found");
            if (claim.OfficerId != officerId)
                throw new UnauthorizedException("Claim is not assigned to you");

            var review = new ClaimReview
            {
                ClaimId = dto.ClaimId,
                OfficerId = officerId,
                DisabilityPercentage = dto.DisabilityPercentage,
                RecoveryWeeks = dto.RecoveryWeeks,
                FraudRiskScore = dto.FraudRiskScore,
                Notes = dto.Notes
            };
            await _claimReviewRepository.AddAsync(review, cancellationToken);

            claim.Review(dto.Approve, dto.ApprovedAmount);

            if (dto.Approve)
            {
                claim.Settle(dto.ApprovedAmount); // Settlement on approval as per current design
                var settlement = new Settlement
                {
                    ClaimId = dto.ClaimId,
                    SettlementAmount = dto.ApprovedAmount ?? claim.ClaimAmount,
                    SettlementDate = DateTime.UtcNow
                };
                await _settlementRepository.AddAsync(settlement, cancellationToken);
            }
            _claimRepository.Update(claim);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateSettlementAsync(Guid officerId, SettlementDto dto, CancellationToken cancellationToken = default)
        {
            var claim = await _claimRepository.GetByIdAsync(dto.ClaimId, cancellationToken);
            if (claim == null)
                throw new NotFoundException("Claim not found");
            if (claim.OfficerId != officerId)
                throw new UnauthorizedException("Claim is not assigned to you");
            if (claim.Status != ClaimStatus.Approved)
                throw new ConflictException("Claim must be approved before settlement");

            claim.Settle(dto.SettlementAmount);
            
            var settlement = new Settlement
            {
                ClaimId = dto.ClaimId,
                SettlementAmount = dto.SettlementAmount,
                SettlementDate = DateTime.UtcNow
            };
            await _settlementRepository.AddAsync(settlement, cancellationToken);

            _claimRepository.Update(claim);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
