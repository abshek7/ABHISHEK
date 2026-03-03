namespace CapStone.Application.DTOs.Admin
{
    public class OfficerWithWorkloadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int AssignedClaimCount { get; set; }
    }
}
