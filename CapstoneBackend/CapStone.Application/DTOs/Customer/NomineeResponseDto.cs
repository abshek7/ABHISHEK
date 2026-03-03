namespace CapStone.Application.DTOs.Customer
{
    public class NomineeResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; } = string.Empty;
    }
}
