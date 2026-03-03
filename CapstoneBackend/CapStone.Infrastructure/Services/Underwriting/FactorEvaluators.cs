using CapStone.Application.Configuration;
using CapStone.Application.Services;
using CapStone.Domain.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace CapStone.Infrastructure.Services.Underwriting
{
    public class AgeEvaluator : IRiskEvaluator
    {
        public decimal CalculateFactor(User user, PolicyRequest request, UnderwritingSettings settings)
        {
            if (!user.DateOfBirth.HasValue) return 1.0m;

            var age = DateTime.UtcNow.Year - user.DateOfBirth.Value.Year;
            if (user.DateOfBirth.Value > DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-age))) age--;

            var factor = settings.AgeFactors.FirstOrDefault(f => age >= f.Min && age <= f.Max);
            return factor?.Score ?? 1.0m;
        }
    }

    public static class EvaluatorExtensions
    {
        public static string NormalizeRiskKey(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            // Remove all non-alphanumeric characters and lowercase
            return Regex.Replace(input, @"[^a-zA-Z0-9]", "").ToLowerInvariant();
        }
    }

    public class OccupationEvaluator : IRiskEvaluator
    {
        public decimal CalculateFactor(User user, PolicyRequest request, UnderwritingSettings settings)
        {
            var normalizedInput = user.Occupation.NormalizeRiskKey();
            if (string.IsNullOrEmpty(normalizedInput) || normalizedInput == "none") return 1.0m;

            var occKey = settings.OccupationFactors.Keys.FirstOrDefault(k => 
                k.NormalizeRiskKey() == normalizedInput);
            
            return occKey != null ? settings.OccupationFactors[occKey] : 1.0m;
        }
    }

    public class HabitEvaluator : IRiskEvaluator
    {
        public decimal CalculateFactor(User user, PolicyRequest request, UnderwritingSettings settings)
        {
            if (string.IsNullOrEmpty(request.PersonalHabits)) return 1.0m;

            decimal habitFactor = 1.0m;
            var habits = request.PersonalHabits.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var habit in habits)
            {
                var normalizedInput = habit.NormalizeRiskKey();
                if (string.IsNullOrEmpty(normalizedInput) || normalizedInput == "none") continue;

                var habitKey = settings.HabitFactors.Keys.FirstOrDefault(k => 
                    k.NormalizeRiskKey() == normalizedInput);
                
                if (habitKey != null)
                {
                    habitFactor *= settings.HabitFactors[habitKey];
                }
            }
            return habitFactor;
        }
    }

    public class MedicalEvaluator : IRiskEvaluator
    {
        public decimal CalculateFactor(User user, PolicyRequest request, UnderwritingSettings settings)
        {
            if (string.IsNullOrEmpty(request.MedicalHistory)) return 1.0m;

            decimal medicalFactor = 1.0m;
            var conditions = request.MedicalHistory.Split(',', StringSplitOptions.RemoveEmptyEntries);
            foreach (var condition in conditions)
            {
                var normalizedInput = condition.NormalizeRiskKey();
                if (string.IsNullOrEmpty(normalizedInput) || normalizedInput == "none") continue;

                var medKey = settings.MedicalFactors.Keys.FirstOrDefault(k => 
                    k.NormalizeRiskKey() == normalizedInput);
                
                if (medKey != null)
                {
                    medicalFactor *= settings.MedicalFactors[medKey];
                }
            }
            return medicalFactor;
        }
    }
}
