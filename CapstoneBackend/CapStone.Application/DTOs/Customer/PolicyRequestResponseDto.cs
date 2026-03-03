using CapStone.Domain.Enums;

namespace CapStone.Application.DTOs.Customer
{
    public class PolicyRequestResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; } = string.Empty;
        public Guid? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal? TotalRiskScore { get; set; }
        public decimal? CalculatedPremium { get; set; }
        public decimal? CoverageAmount { get; set; }
        public bool? IsEligible { get; set; }
        
        public decimal? SuggestedRiskScore { get; set; }
        public decimal? SuggestedPremium { get; set; }
        public decimal? SuggestedCoverage { get; set; }

        public string? NomineeName { get; set; }
        public string? NomineeRelation { get; set; }
        public bool IsPaid { get; set; }
        public DateTime? PaymentDate { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public DateOnly? CustomerDateOfBirth { get; set; }
        public string? CustomerOccupation { get; set; }
        public string? PersonalHabits { get; set; }
        public string? MedicalHistory { get; set; }
    }
}
