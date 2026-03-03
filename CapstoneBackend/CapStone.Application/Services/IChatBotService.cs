using CapStone.Application.DTOs.Chat;

namespace CapStone.Application.Services
{
    public interface IChatBotService
    {
        Task<ChatResponseDto> GetChatResponseAsync(ChatRequestDto request);
    }
}
