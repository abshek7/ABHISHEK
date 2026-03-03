using System;

namespace CapStone.Application.DTOs.Agent
{
    public class AssignedCustomerDto
    {
        public Guid CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? RiskScore { get; set; }
    }
}
