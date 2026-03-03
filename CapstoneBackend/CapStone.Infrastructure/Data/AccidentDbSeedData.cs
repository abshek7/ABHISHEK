using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CapStone.Infrastructure.Data
{
    public static class AccidentDbSeedData
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AccidentDbContext>();

            await context.Database.MigrateAsync();

            // If Santhu exists, we assume the new personalized test suite is already seeded
            if (context.Users.Any(u => u.Name == "Santhu"))
                return;

            Console.WriteLine("Seeding fresh test data...");

            // Wipe old records to start fresh as requested
            context.Settlements.RemoveRange(context.Settlements);
            context.ClaimReviews.RemoveRange(context.ClaimReviews);
            context.Claims.RemoveRange(context.Claims);
            context.Payments.RemoveRange(context.Payments);
            context.Nominees.RemoveRange(context.Nominees);
            context.Policies.RemoveRange(context.Policies);
            context.PolicyRequests.RemoveRange(context.PolicyRequests);
            context.PolicyCoverages.RemoveRange(context.PolicyCoverages);
            context.PolicyTypes.RemoveRange(context.PolicyTypes);
            context.Users.RemoveRange(context.Users);
            
            await context.SaveChangesAsync();
            var adminId = Guid.NewGuid();
            var agentId = Guid.NewGuid();

            var customer1Id = Guid.NewGuid();
            var customer2Id = Guid.NewGuid();
            var customer3Id = Guid.NewGuid();

            var pt1Id = Guid.NewGuid();
            var pt2Id = Guid.NewGuid();
            var pt3Id = Guid.NewGuid();
            var pt4Id = Guid.NewGuid();

            // ===== Users =====
            var admin = new User
            {
                Id = adminId,
                Name = "Abhishek Admin",
                Email = "admin@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var agent = new User
            {
                Id = agentId,
                Name = "Agent Smith",
                Email = "agent@example.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Agent@123"),
                Role = UserRole.Agent,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var customers = new List<User>
            {
                new User
                {
                    Id = customer1Id,
                    Name = "Santhu",
                    Email = "santhu@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@1"),
                    Role = UserRole.Customer,
                    DateOfBirth = new DateOnly(1995, 5, 15),
                    Occupation = "FullStackTrainer",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Id = customer2Id,
                    Name = "Abhigna",
                    Email = "abhigna@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@1"),
                    Role = UserRole.Customer,
                    DateOfBirth = new DateOnly(2003, 8, 20),
                    Occupation = "Student",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Id = customer3Id,
                    Name = "Ranjan",
                    Email = "ranjan@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Customer@1"),
                    Role = UserRole.Customer,
                    DateOfBirth = new DateOnly(1985, 12, 10),
                    Occupation = "Farmer",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            };

            context.Users.AddRange(admin, agent);
            context.Users.AddRange(customers);
 
            var policyTypes = new List<PolicyType>
            {
                new PolicyType { Id = pt1Id, Name = "Basic Accident", BasePremium = 500, BaseCoverageAmount = 50000, DurationMonths = 12, Status = "Active", CreatedBy = adminId },
                new PolicyType { Id = pt2Id, Name = "Standard Accident", BasePremium = 1000, BaseCoverageAmount = 100000, DurationMonths = 12, Status = "Active", CreatedBy = adminId },
                new PolicyType { Id = pt3Id, Name = "Premium Accident", BasePremium = 2000, BaseCoverageAmount = 200000, DurationMonths = 24, Status = "Active", CreatedBy = adminId },
                new PolicyType { Id = pt4Id, Name = "Family Accident", BasePremium = 3000, BaseCoverageAmount = 300000, DurationMonths = 24, Status = "Active", CreatedBy = adminId }
            };

            context.PolicyTypes.AddRange(policyTypes);

            var coverages = new List<PolicyCoverage>
            {
                new PolicyCoverage { Id = Guid.NewGuid(), PolicyTypeId = pt1Id, CoverageCategory = CoverageCategory.AccidentalDeath, PercentageOfCoverage = 100, Description = "Accidental death benefit" },
                new PolicyCoverage { Id = Guid.NewGuid(), PolicyTypeId = pt2Id, CoverageCategory = CoverageCategory.AccidentalDeath, PercentageOfCoverage = 100, Description = "Accidental death benefit" },
                new PolicyCoverage { Id = Guid.NewGuid(), PolicyTypeId = pt3Id, CoverageCategory = CoverageCategory.PermanentTotalDisability, PercentageOfCoverage = 80, Description = "Permanent total disability" },
                new PolicyCoverage { Id = Guid.NewGuid(), PolicyTypeId = pt4Id, CoverageCategory = CoverageCategory.TemporaryTotalDisability, PercentageOfCoverage = 60, WeeklyCompensationPercentage = 70, MaxWeeks = 52, Description = "Temporary disability" }
            };

            context.PolicyCoverages.AddRange(coverages);

            var requests = new List<PolicyRequest>
            {
                new PolicyRequest
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer1Id,
                    PolicyTypeId = pt2Id,
                    AssignedAgentId = agentId,
                    Status = RequestStatus.Assigned,
                    RequestDate = DateTime.UtcNow
                },
                new PolicyRequest
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer2Id,
                    PolicyTypeId = pt1Id,
                    AssignedAgentId = agentId,
                    Status = RequestStatus.Assigned,
                    PersonalHabits = "None",
                    RequestDate = DateTime.UtcNow
                },
                new PolicyRequest
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customer3Id,
                    PolicyTypeId = pt3Id,
                    AssignedAgentId = agentId,
                    Status = RequestStatus.Assigned,
                    PersonalHabits = "Smoking",
                    MedicalHistory = "MinorIssues",
                    RequestDate = DateTime.UtcNow
                }
            };

            context.PolicyRequests.AddRange(requests);

            await context.SaveChangesAsync();
        }
    }
}