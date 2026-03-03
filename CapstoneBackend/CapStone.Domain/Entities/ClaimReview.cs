using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class ClaimReview
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ClaimId { get; set; }

        [Required]
        public Guid OfficerId { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? DisabilityPercentage { get; set; }

        public int? RecoveryWeeks { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? FraudRiskScore { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
