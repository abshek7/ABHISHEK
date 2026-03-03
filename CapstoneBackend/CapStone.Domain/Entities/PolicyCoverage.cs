using CapStone.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class PolicyCoverage
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid PolicyTypeId { get; set; }

        public CoverageCategory CoverageCategory { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentageOfCoverage { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? WeeklyCompensationPercentage { get; set; }

        public int? MaxWeeks { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}
