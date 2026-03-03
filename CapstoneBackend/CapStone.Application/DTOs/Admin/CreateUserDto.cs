using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CapStone.Application.DTOs.Admin
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        //public DateOnly? DateOfBirth { get; set; }
        //public string? Phone { get; set; }
        //public string? Address { get; set; }
        //public string? Occupation { get; set; }
    }

}