using System.Collections.Generic;

namespace CapStone.Application.Configuration
{
    public class UnderwritingSettings
    {
        public List<AgeFactor> AgeFactors { get; set; } = new();
        public Dictionary<string, decimal> OccupationFactors { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, decimal> HabitFactors { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, decimal> MedicalFactors { get; set; } = new(StringComparer.OrdinalIgnoreCase);
        public EligibilityRules EligibilityRules { get; set; } = new();
        public PremiumRules PremiumRules { get; set; } = new();
    }

    public class AgeFactor
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public decimal Score { get; set; }
    }

    public class EligibilityRules
    {
        public decimal MaxRiskScore { get; set; }
        public decimal RejectAbove { get; set; }
    }

    public class PremiumRules
    {
        public decimal BaseMultiplier { get; set; }
        public decimal MaxPremiumMultiplier { get; set; }
    }
}
