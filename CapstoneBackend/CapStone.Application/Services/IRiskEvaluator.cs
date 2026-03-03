using CapStone.Application.Configuration;
using CapStone.Domain.Entities;

namespace CapStone.Application.Services
{
    public interface IRiskEvaluator
    {
        decimal CalculateFactor(User user, PolicyRequest request, UnderwritingSettings settings);
    }
}
