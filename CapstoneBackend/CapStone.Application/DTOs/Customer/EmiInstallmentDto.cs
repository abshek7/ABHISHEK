namespace CapStone.Application.DTOs.Customer
{
    public class EmiInstallmentDto
    {
        public Guid PolicyId { get; set; }
        public int InstallmentNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
    }
}

