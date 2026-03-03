namespace CapStone.Application.DTOs.Chat
{
    public class ChatRequestDto
    {
        public string UserMessage { get; set; } = string.Empty;
        public bool RequestAudio { get; set; } = false;
    }

    public class ChatResponseDto
    {
        public string Response { get; set; } = string.Empty;
        public string? AudioBase64 { get; set; }
        public string? ImageBase64 { get; set; }
    }
}
