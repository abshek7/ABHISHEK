using CapStone.Application.DTOs.Agent;
using CapStone.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CapStone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Agent")]
    public class AgentController : ControllerBase
    {
        private readonly IAgentService _service;

        public AgentController(IAgentService service)
        {
            _service = service;
        }

        private Guid GetAgentId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id ?? Guid.Empty.ToString());
        }

        [HttpGet("assigned-requests")]
        public async Task<IActionResult> GetAssignedRequests()
        {
            var list = await _service.GetAssignedRequestsAsync(GetAgentId());
            return Ok(list);
        }

        [HttpGet("assigned-policies")]
        public async Task<IActionResult> GetAssignedPolicies()
        {
            var list = await _service.GetAssignedPoliciesAsync(GetAgentId());
            return Ok(list);
        }

        [HttpGet("assigned-customers")]
        public async Task<IActionResult> GetAssignedCustomers()
        {
            var list = await _service.GetAssignedCustomersAsync(GetAgentId());
            return Ok(list);
        }

        [HttpPut("underwriting")]
        public async Task<IActionResult> UpdateUnderwriting([FromBody] UnderwritingDto dto)
        {
            await _service.UpdateUnderwritingAsync(GetAgentId(), dto);
            return Ok(new { message = "Underwriting updated" });
        }

        [HttpGet("commission-summary")]
        public async Task<IActionResult> GetCommissionSummary()
        {
            var result = await _service.GetCommissionSummaryAsync(GetAgentId());
            return Ok(result);
        }

        [HttpPost("create-policy-direct")]
        public async Task<IActionResult> CreatePolicyDirect([FromBody] CreatePolicyDirectDto dto)
        {
            await _service.CreatePolicyDirectAsync(GetAgentId(), dto);
            return Ok(new { message = "Policy created directly by agent" });
        }
    }
}
