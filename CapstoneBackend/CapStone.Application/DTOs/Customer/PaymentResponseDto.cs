using CapStone.Domain.Enums;

namespace CapStone.Application.DTOs.Customer
{
    public class PaymentResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
    }
}
