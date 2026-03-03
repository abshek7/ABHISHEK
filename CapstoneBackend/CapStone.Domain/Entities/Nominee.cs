using System.ComponentModel.DataAnnotations;

namespace CapStone.Domain.Entities
{
    public class Nominee
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PolicyId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Relationship { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
    }
}
