using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapStone.Application.DTOs.Admin
{
    public class CreatePolicyDto
    {
        public Guid RequestId { get; set; }

        public decimal FinalPremium { get; set; }
        public decimal CoverageAmount { get; set; }

        public DateTime StartDate { get; set; }
    }
}
