using System.ComponentModel.DataAnnotations;

namespace CapStone.Application.DTOs.Claim
{
    public class SettlementDto
    {
        public Guid ClaimId { get; set; }
        public decimal SettlementAmount { get; set; }
    }
}
