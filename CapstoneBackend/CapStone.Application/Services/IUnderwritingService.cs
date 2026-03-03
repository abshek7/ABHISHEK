using CapStone.Domain.Entities;

namespace CapStone.Application.Services
{
    public interface IUnderwritingService
    {
        (decimal RiskScore, decimal CalculatedPremium, bool IsEligible) CalculateRiskAndPremium(User user, PolicyRequest request, PolicyType policyType);
    }
}
