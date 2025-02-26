using StringHub.Models;

namespace StringHub.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> LoginAsync(AuthRequest request);
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    }
}