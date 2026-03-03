using CapStone.Domain.Enums;

namespace CapStone.Application.DTOs.Customer
{
    public class PolicyResponseDto
    {
        public Guid Id { get; set; }
        public string PolicyNumber { get; set; } = string.Empty;
        public Guid PolicyTypeId { get; set; }
        public string PolicyTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal FinalPremium { get; set; }
        public decimal CoverageAmount { get; set; }
        public decimal AgentCommissionAmount { get; set; }
        public PolicyStatus Status { get; set; }
    }
}
