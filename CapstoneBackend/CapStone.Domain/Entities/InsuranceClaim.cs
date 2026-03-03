using CapStone.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class InsuranceClaim
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PolicyId { get; set; }
        public Policy? Policy { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public User? Customer { get; set; }

        public Guid? OfficerId { get; set; }
        public User? Officer { get; set; }

        [Required]
        public CoverageCategory CoverageCategory { get; set; }

        public DateTime IncidentDate { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal ClaimAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ApprovedAmount { get; set; }

        [MaxLength(500)]
        public string? DocumentFilePath { get; set; }

        public ClaimStatus Status { get; set; }

        public void Review(bool approve, decimal? approvedAmount)
        {
            ApprovedAmount = approvedAmount;
            Status = approve ? ClaimStatus.Approved : ClaimStatus.Rejected;
        }

        public void Settle(decimal? settlementAmount = null)
        {
            if (Status != ClaimStatus.Approved && Status != ClaimStatus.Settled)
                throw new InvalidOperationException("Claim must be approved before settlement.");
            
            ApprovedAmount = settlementAmount ?? ApprovedAmount;
            Status = ClaimStatus.Settled;
        }
    }
}
