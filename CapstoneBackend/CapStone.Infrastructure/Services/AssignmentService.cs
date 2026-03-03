using CapStone.Application.Repositories;
using CapStone.Domain.Entities;
using CapStone.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CapStone.Infrastructure.Services
{
    public interface IAssignmentService
    {
        Task<Guid?> GetAgentWithLeastWorkloadAsync();
    }

    public class AssignmentService : IAssignmentService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<PolicyRequest> _policyRequestRepository;

        public AssignmentService(
            IRepository<User> userRepository,
            IRepository<PolicyRequest> policyRequestRepository)
        {
            _userRepository = userRepository;
            _policyRequestRepository = policyRequestRepository;
        }

        public async Task<Guid?> GetAgentWithLeastWorkloadAsync()
        {
            var agents = await _userRepository.GetQueryable()
                .Where(u => u.Role == UserRole.Agent && u.IsActive)
                .Select(u => u.Id)
                .ToListAsync();

            if (!agents.Any()) return null;

            // Count assigned (active) requests for each agent
            var agentWorkloads = await _policyRequestRepository.GetQueryable()
                .Where(pr => pr.AssignedAgentId != null && pr.Status == RequestStatus.Assigned)
                .GroupBy(pr => pr.AssignedAgentId)
                .Select(g => new { AgentId = g.Key.Value, Count = g.Count() })
                .ToListAsync();

            // Create a dictionary for quick lookup, default to 0 for agents with no requests
            var workloadDict = agents.ToDictionary(id => id, _ => 0);
            foreach (var workload in agentWorkloads)
            {
                if (workloadDict.ContainsKey(workload.AgentId))
                {
                    workloadDict[workload.AgentId] = workload.Count;
                }
            }

            // Return the agent ID with the minimum count
            return workloadDict.OrderBy(kvp => kvp.Value).First().Key;
        }
    }
}
