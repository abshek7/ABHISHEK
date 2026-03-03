using CapStone.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CapStone.Application.DTOs.Customer
{
    public class CreatePaymentDto
    {
        public string PolicyId { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
