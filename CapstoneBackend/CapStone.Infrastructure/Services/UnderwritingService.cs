using CapStone.Application.Configuration;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using Microsoft.Extensions.Options;
using System.Linq;

namespace CapStone.Infrastructure.Services
{
    public class UnderwritingService : IUnderwritingService
    {
        private readonly UnderwritingSettings _settings;
        private readonly IEnumerable<IRiskEvaluator> _evaluators;

        public UnderwritingService(IOptions<UnderwritingSettings> settings, IEnumerable<IRiskEvaluator> evaluators)
        {
            _settings = settings.Value;
            _evaluators = evaluators;
        }

        public (decimal RiskScore, decimal CalculatedPremium, bool IsEligible) CalculateRiskAndPremium(User user, PolicyRequest request, PolicyType policyType)
        {
            decimal finalRiskScore = 1.0m;

            foreach (var evaluator in _evaluators)
            {
                finalRiskScore *= evaluator.CalculateFactor(user, request, _settings);
            }
            
            bool isEligible = finalRiskScore <= _settings.EligibilityRules.RejectAbove;
            
            decimal calculatedPremium = policyType.BasePremium * finalRiskScore * _settings.PremiumRules.BaseMultiplier;

            return (Math.Round(finalRiskScore, 2), Math.Round(calculatedPremium, 2), isEligible);
        }
    }
}
