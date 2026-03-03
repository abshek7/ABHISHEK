namespace CapStone.Application.DTOs.Customer
{
    public class PolicyTypeResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal BasePremium { get; set; }
        public decimal BaseCoverageAmount { get; set; }
        public int DurationMonths { get; set; }
    }
}
