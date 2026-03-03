using CapStone.Application.DTOs.Customer;
using CapStone.Domain.Enums;

namespace CapStone.Application.DTOs.Claim
{
    public class ClaimDetailResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public string? PolicyNumber { get; set; }
        public Guid CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Occupation { get; set; }
        public CoverageCategory CoverageCategory { get; set; }
        public DateTime IncidentDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal ClaimAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public ClaimStatus Status { get; set; }
        public Guid? OfficerId { get; set; }
        public string? DocumentFilePath { get; set; }
        public decimal? TotalRiskScore { get; set; }
        public string? MedicalHistory { get; set; }
        public string? PersonalHabits { get; set; }
        public List<NomineeResponseDto> Nominees { get; set; } = new();
    }
}
