using CapStone.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class Policy
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid RequestId { get; set; }
        [ForeignKey("RequestId")]
        public PolicyRequest? PolicyRequest { get; set; }

        [Required]
        public string PolicyNumber { get; set; } = string.Empty;

        public Guid CustomerId { get; set; }
        public User? Customer { get; set; }
        public Guid AssignedAgentId { get; set; }
        public User? AssignedAgent { get; set; }
        public Guid PolicyTypeId { get; set; }
        public PolicyType? PolicyType { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal FinalPremium { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CoverageAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AgentCommissionAmount { get; set; }

        public PolicyStatus Status { get; set; } = PolicyStatus.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public void Expire()
        {
            Status = PolicyStatus.Expired;
        }
    }
}
