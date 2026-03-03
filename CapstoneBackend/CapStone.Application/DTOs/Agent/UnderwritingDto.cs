namespace CapStone.Application.DTOs.Agent
{
    public class UnderwritingDto
    {
        public Guid RequestId { get; set; }
        
        // Final decision from the agent
        public bool IsEligible { get; set; }

        // These are now handled automatically by the backend
        public decimal? OverrideRiskScore { get; set; }
        public decimal? OverridePremium { get; set; }
        public decimal? OverrideCoverage { get; set; }
    }
}
