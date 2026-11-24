using EventApi.Dtos;
using EventsApi.Dtos;

namespace EventApi.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<TokenDto?> LoginAsync(LoginDto dto);
    }
}
