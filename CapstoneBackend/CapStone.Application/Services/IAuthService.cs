using CapStone.Application.DTOs.Auth;

namespace CapStone.Application.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}