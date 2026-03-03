using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapStone.Application.DTOs.Admin
{
    public class CreatePolicyTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal BasePremium { get; set; }
        public decimal BaseCoverageAmount { get; set; }
        public int DurationMonths { get; set; }
    }
}