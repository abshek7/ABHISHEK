using CapStone.Application.DTOs.Claim;
using CapStone.Application.DTOs.Customer;

namespace CapStone.Application.Services
{
    public interface IClaimService
    {
        Task<IReadOnlyList<ClaimResponseDto>> GetAssignedClaimsAsync(Guid officerId, CancellationToken cancellationToken = default);
        Task<ClaimDetailResponseDto?> GetClaimByIdAsync(Guid officerId, Guid claimId, CancellationToken cancellationToken = default);
        Task SubmitClaimReviewAsync(Guid officerId, ClaimReviewDto dto, CancellationToken cancellationToken = default);
        Task CreateSettlementAsync(Guid officerId, SettlementDto dto, CancellationToken cancellationToken = default);
    }
}
