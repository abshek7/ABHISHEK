using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class PolicyType
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePremium { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BaseCoverageAmount { get; set; }

        public int DurationMonths { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Active";

        public Guid CreatedBy { get; set; }
    }
}
