using System;

namespace CapStone.Application.DTOs.Agent
{
    public class CreatePolicyDirectDto
    {
        public Guid CustomerId { get; set; }
        public Guid PolicyTypeId { get; set; }
        public decimal FinalPremium { get; set; }
        public decimal CoverageAmount { get; set; }
        public DateTime StartDate { get; set; }
        public string NomineeName { get; set; } = string.Empty;
        public string NomineeRelation { get; set; } = string.Empty;
    }
}
