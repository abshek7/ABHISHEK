using System;

namespace CapStone.Application.DTOs.Admin
{
    public class RevenueReportDto
    {
        public decimal TotalRevenue { get; set; }
        public List<MonthlyRevenueDto> MonthlyBreakdown { get; set; } = new();
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }
}
