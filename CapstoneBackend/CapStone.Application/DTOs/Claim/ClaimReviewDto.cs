using System.ComponentModel.DataAnnotations;

namespace CapStone.Application.DTOs.Claim
{
    public class ClaimReviewDto
    {
        public Guid ClaimId { get; set; }
        public decimal? DisabilityPercentage { get; set; }
        public int? RecoveryWeeks { get; set; }
        public decimal? FraudRiskScore { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public decimal? ApprovedAmount { get; set; }
        public bool Approve { get; set; }
    }
}
