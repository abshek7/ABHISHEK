using System;

namespace CapStone.Application.DTOs.Customer
{
    public class InvoiceDto
    {
        public string PolicyNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime IssuedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
