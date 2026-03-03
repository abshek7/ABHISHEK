using CapStone.Application.DTOs.Admin;
using CapStone.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CapStone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;

        public AdminController(IAdminService service)
        {
            _service = service;
        }

        [HttpPost("create-agent")]
        public async Task<IActionResult> CreateAgent([FromBody] CreateUserDto dto)
        {
            await _service.CreateAgentAsync(dto);
            return Ok(new { message = "Agent created successfully" });
        }

        [HttpPost("create-claims-officer")]
        public async Task<IActionResult> CreateOfficer([FromBody] CreateUserDto dto)
        {
            await _service.CreateClaimsOfficerAsync(dto);
            return Ok(new { message = "Claims officer created successfully" });
        }

        /*
        [HttpPost("policy-type")]
        public async Task<IActionResult> CreatePolicyType([FromBody] CreatePolicyTypeDto dto)
        {
            var id = await _service.CreatePolicyTypeAsync(dto);
            return Ok(new { PolicyTypeId = id });
        }

        [HttpPost("policy-coverage")]
        public async Task<IActionResult> AddCoverage([FromBody] CreatePolicyCoverageDto dto)
        {
            await _service.AddPolicyCoverageAsync(dto);
            return Ok("Coverage added successfully");
        }
        */

        //[HttpPost("assign-agent")]
        //public async Task<IActionResult> AssignAgent([FromBody] AssignAgentToRequestDto dto)
        //{
        //    await _service.AssignAgentToRequestAsync(dto);
        //    return Ok(new { message = "Agent assigned successfully" });
        //}

        [HttpPost("assign-agent-by-workload/{requestId:guid}")]
        public async Task<IActionResult> AssignAgentByLeastWorkload(Guid requestId)
        {
            await _service.AssignAgentByLeastWorkloadAsync(requestId);
            return Ok(new { message = "Agent assigned by workload" });
        }

        //[HttpPost("create-policy")]
        //public async Task<IActionResult> CreatePolicy([FromBody] CreatePolicyDto dto)
        //{
        //    var policyId = await _service.CreatePolicyAsync(dto);
        //    return Ok(new { PolicyId = policyId });
        //}

        [HttpGet("agents-with-workload")]
        public async Task<IActionResult> GetAgentsWithWorkload()
        {
            var list = await _service.GetAgentsWithWorkloadAsync();
            return Ok(list);
        }

        [HttpGet("officers-with-workload")]
        public async Task<IActionResult> GetOfficersWithWorkload()
        {
            var list = await _service.GetOfficersWithWorkloadAsync();
            return Ok(list);
        }

        [HttpGet("unassigned-requests")]
        public async Task<IActionResult> GetUnassignedRequests()
        {
            var list = await _service.GetUnassignedRequestsAsync();
            return Ok(list);
        }

        [HttpGet("all-requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var list = await _service.GetAllRequestsAsync();
            return Ok(list);
        }

        [HttpGet("unassigned-claims")]
        public async Task<IActionResult> GetUnassignedClaims()
        {
            var list = await _service.GetUnassignedClaimsAsync();
            return Ok(list);
        }

        [HttpGet("all-claims")]
        public async Task<IActionResult> GetAllClaims()
        {
            var list = await _service.GetAllClaimsAsync();
            return Ok(list);
        }

        [HttpPost("assign-claim-by-workload/{claimId:guid}")]
        public async Task<IActionResult> AssignClaimByLeastWorkload(Guid claimId)
        {
            await _service.AssignClaimByLeastWorkloadAsync(claimId);
            return Ok(new { message = "Claim assigned by workload" });
        }

        [HttpGet("analytics")]
        public async Task<IActionResult> GetAnalytics()
        {
            var result = await _service.GetAnalyticsAsync();
            return Ok(result);
        }

        [HttpGet("revenue-report")]
        public async Task<IActionResult> GetRevenueReport()
        {
            var result = await _service.GetRevenueReportAsync();
            return Ok(result);
        }

        [HttpGet("agent-performance")]
        public async Task<IActionResult> GetAgentPerformance()
        {
            var result = await _service.GetAgentPerformanceReportAsync();
            return Ok(result);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            await _service.UpdateUserAsync(dto);
            return Ok(new { message = "User updated successfully" });
        }

        [HttpDelete("user/{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            await _service.DeleteUserAsync(userId);
            return Ok(new { message = "User deleted successfully" });
        }
    }
}