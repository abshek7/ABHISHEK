using System.ComponentModel.DataAnnotations;

namespace CapStone.Application.DTOs.Customer
{
    public class CreatePolicyRequestDto
    {
        public Guid PolicyTypeId { get; set; }

        [MaxLength(500)]
        public string? PersonalHabits { get; set; }

        [MaxLength(500)]
        public string? MedicalHistory { get; set; }

        [MaxLength(500)]
        public string? DocumentFilePath { get; set; }
    }
}
