using CapStone.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class PolicyRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CustomerId { get; set; }
        public Guid PolicyTypeId { get; set; }
        public Guid? AssignedAgentId { get; set; }
        public User? AssignedAgent { get; set; }
        public PolicyType? PolicyType { get; set; }

        public DateTime RequestDate { get; set; }

        public RequestStatus Status { get; set; } = RequestStatus.New;

        [MaxLength(500)]
        public string? PersonalHabits { get; set; }

        [MaxLength(500)]
        public string? MedicalHistory { get; set; }

        [MaxLength(500)]
        public string? DocumentFilePath { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? TotalRiskScore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CalculatedPremium { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CoverageAmount { get; set; }

        public bool? IsEligible { get; set; }

        // Nominee and Payment details for final issuance
        [MaxLength(100)]
        public string? NomineeName { get; set; }
        [MaxLength(50)]
        public string? NomineeRelation { get; set; }
        public bool IsPaid { get; set; } = false;
        public DateTime? PaymentDate { get; set; }

        public void Approve(decimal riskScore, decimal premium, decimal coverage)
        {
            TotalRiskScore = riskScore;
            CalculatedPremium = premium;
            CoverageAmount = coverage;
            IsEligible = true;
            Status = RequestStatus.Approved;
        }

        public void Reject()
        {
            IsEligible = false;
            Status = RequestStatus.Rejected;
        }

        public void CompletePurchase(string nomineeName, string nomineeRelation)
        {
            if (Status != RequestStatus.Approved)
                throw new InvalidOperationException("Request must be approved before completion.");

            NomineeName = nomineeName;
            NomineeRelation = nomineeRelation;
            IsPaid = true;
            PaymentDate = DateTime.UtcNow;
            Status = RequestStatus.Accepted;
        }
    }
}
