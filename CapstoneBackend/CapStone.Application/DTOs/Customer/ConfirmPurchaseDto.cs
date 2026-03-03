namespace CapStone.Application.DTOs.Customer
{
    public class ConfirmPurchaseDto
    {
        public Guid RequestId { get; set; }
        public string NomineeName { get; set; } = string.Empty;
        public string NomineeRelation { get; set; } = string.Empty;
        public string? NomineePhone { get; set; }
        public DateTime? NomineeDob { get; set; }
    }
}
