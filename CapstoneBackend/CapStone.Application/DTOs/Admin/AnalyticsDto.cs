namespace CapStone.Application.DTOs.Admin
{
    public class AnalyticsDto
    {
        public int TotalPolicies { get; set; }
        public int ActivePolicies { get; set; }
        public int TotalClaims { get; set; }
        public int PendingClaims { get; set; }
        public int TotalPolicyRequests { get; set; }
        public int UnassignedRequests { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }
        public int TotalAdmins { get; set; }
        public int TotalAgents { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalClaimsOfficers { get; set; }
    }
}
