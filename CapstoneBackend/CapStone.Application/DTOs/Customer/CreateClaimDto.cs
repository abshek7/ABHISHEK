using CapStone.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CapStone.Application.DTOs.Customer
{
    public class CreateClaimDto
    {
        public Guid PolicyId { get; set; }
        public CoverageCategory CoverageCategory { get; set; }
        public DateTime IncidentDate { get; set; }

        [Required, MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public decimal ClaimAmount { get; set; }

        [MaxLength(500)]
        public string? DocumentFilePath { get; set; }
    }
}
