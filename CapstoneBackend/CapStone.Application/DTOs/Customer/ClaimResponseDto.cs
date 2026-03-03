using CapStone.Domain.Enums;

namespace CapStone.Application.DTOs.Customer
{
    public class ClaimResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public CoverageCategory CoverageCategory { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public ClaimStatus Status { get; set; }
        public Guid? OfficerId { get; set; }
    }
}
