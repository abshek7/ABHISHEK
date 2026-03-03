using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapStone.Domain.Enums;

namespace CapStone.Application.DTOs.Admin
{
    public class CreatePolicyCoverageDto
    {
        public Guid PolicyTypeId { get; set; }
        public CoverageCategory CoverageCategory { get; set; }

        public decimal PercentageOfCoverage { get; set; }
        public decimal? WeeklyCompensationPercentage { get; set; }

        public int? MaxWeeks { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}