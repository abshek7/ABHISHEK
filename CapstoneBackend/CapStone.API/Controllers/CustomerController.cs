using CapStone.Application.DTOs.Customer;
using CapStone.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CapStone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        private Guid GetCustomerId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id ?? Guid.Empty.ToString());
        }

        [HttpPost("policy-request")]
        public async Task<IActionResult> CreatePolicyRequest([FromBody] CreatePolicyRequestDto dto)
        {
            await _service.CreatePolicyRequestAsync(GetCustomerId(), dto);
            return Ok(new { message = "Policy request created" });
        }

        [HttpGet("policy-requests")]
        public async Task<IActionResult> GetMyPolicyRequests()
        {
            var list = await _service.GetMyPolicyRequestsAsync(GetCustomerId());
            return Ok(list);
        }

        [HttpGet("policies")]
        public async Task<IActionResult> GetMyPolicies()
        {
            var list = await _service.GetMyPoliciesAsync(GetCustomerId());
            return Ok(list);
        }

        [HttpGet("policies/{policyId:guid}/emi-schedule")]
        public async Task<IActionResult> GetEmiSchedule(Guid policyId)
        {
            var list = await _service.GetEmiScheduleAsync(GetCustomerId(), policyId);
            return Ok(list);
        }

        [HttpGet("policy-types")]
        public async Task<IActionResult> GetPolicyTypes()
        {
            var list = await _service.GetPolicyTypesAsync();
            return Ok(list);
        }

        [HttpPost("claim")]
        public async Task<IActionResult> CreateClaim([FromBody] CreateClaimDto dto)
        {
            await _service.CreateClaimAsync(GetCustomerId(), dto);
            return Ok(new { message = "Claim created" });
        }

        [HttpGet("claims")]
        public async Task<IActionResult> GetMyClaims()
        {
            var list = await _service.GetMyClaimsAsync(GetCustomerId());
            return Ok(list);
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var list = await _service.GetNotificationsAsync(GetCustomerId());
            return Ok(list);
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetMyPayments()
        {
            var list = await _service.GetMyPaymentsAsync(GetCustomerId());
            return Ok(list);
        }

        [HttpPost("payment")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDto dto)
        {
            await _service.CreatePaymentAsync(GetCustomerId(), dto);
            return Ok(new { message = "Payment recorded" });
        }

        [HttpPost("nominee")]
        public async Task<IActionResult> AddNominee([FromBody] CreateNomineeDto dto)
        {
            await _service.AddNomineeAsync(GetCustomerId(), dto);
            return Ok(new { message = "Nominee added" });
        }

        [HttpGet("policies/{policyId:guid}/nominees")]
        public async Task<IActionResult> GetNominees(Guid policyId)
        {
            var list = await _service.GetNomineesAsync(GetCustomerId(), policyId);
            return Ok(list);
        }
        [HttpPost("confirm-purchase")]
        public async Task<IActionResult> ConfirmPurchase([FromBody] ConfirmPurchaseDto dto)
        {
            var policyId = await _service.ConfirmPurchaseAsync(GetCustomerId(), dto);
            return Ok(new { message = "Purchase confirmed and policy issued", policyId });
        }

        [HttpPost("renew-policy")]
        public async Task<IActionResult> RenewPolicy([FromBody] RenewalRequestDto dto)
        {
            await _service.RenewPolicyAsync(GetCustomerId(), dto);
            return Ok(new { message = "Policy renewed successfully" });
        }

        [HttpGet("policies/{policyId:guid}/invoice")]
        public async Task<IActionResult> GetInvoice(Guid policyId)
        {
            var result = await _service.GetInvoiceAsync(GetCustomerId(), policyId);
            return Ok(result);
        }
    }
}
