using System;

namespace CapStone.Application.DTOs.Admin
{
    public class AgentPerformanceReportDto
    {
        public List<AgentPerformanceDto> Agents { get; set; } = new();
    }

    public class AgentPerformanceDto
    {
        public Guid AgentId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public int PoliciesSold { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal TotalRevenueGenerated { get; set; }
    }
}
