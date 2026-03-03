using CapStone.Application.DTOs.Claim;
using CapStone.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CapStone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "ClaimsOfficer")]
    public class ClaimsOfficerController : ControllerBase
    {
        private readonly IClaimService _service;

        public ClaimsOfficerController(IClaimService service)
        {
            _service = service;
        }

        private Guid GetOfficerId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id ?? Guid.Empty.ToString());
        }

        [HttpGet("claims")]
        public async Task<IActionResult> GetAssignedClaims()
        {
            var list = await _service.GetAssignedClaimsAsync(GetOfficerId());
            return Ok(list);
        }

        [HttpGet("claims/{id:guid}")]
        public async Task<IActionResult> GetClaimById(Guid id)
        {
            var claim = await _service.GetClaimByIdAsync(GetOfficerId(), id);
            if (claim == null)
                return NotFound();
            return Ok(claim);
        }

        [HttpPost("review")]
        public async Task<IActionResult> SubmitClaimReview([FromBody] ClaimReviewDto dto)
        {
            await _service.SubmitClaimReviewAsync(GetOfficerId(), dto);
            return Ok(new { message = "Review submitted" });
        }

        [HttpPost("settlement")]
        public async Task<IActionResult> CreateSettlement([FromBody] SettlementDto dto)
        {
            await _service.CreateSettlementAsync(GetOfficerId(), dto);
            return Ok(new { message = "Settlement created" });
        }
    }
}
