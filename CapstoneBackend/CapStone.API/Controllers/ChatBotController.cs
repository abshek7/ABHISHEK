using Microsoft.AspNetCore.Mvc;
using CapStone.Application.Services;
using CapStone.Application.DTOs.Chat;

namespace CapStone.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        private readonly IChatBotService _chatBotService;

        public ChatBotController(IChatBotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.UserMessage))
            {
                return BadRequest("Message cannot be empty");
            }

            try 
            {
                var result = await _chatBotService.GetChatResponseAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message, detail = ex.InnerException?.Message });
            }
        }
    }
}
