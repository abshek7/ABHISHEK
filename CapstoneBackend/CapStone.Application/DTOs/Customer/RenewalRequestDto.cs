using System;

namespace CapStone.Application.DTOs.Customer
{
    public class RenewalRequestDto
    {
        public Guid PolicyId { get; set; }
        public int DurationMonths { get; set; }
    }
}
