using System.ComponentModel.DataAnnotations.Schema;

namespace CapStone.Domain.Entities
{
    public class Settlement
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClaimId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal SettlementAmount { get; set; }

        public DateTime SettlementDate { get; set; }
    }
}
