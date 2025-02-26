using StringHub.Models;

namespace StringHub.Services
{
    public interface IJwtService
    {
        AuthResponse GenerateToken(Usuario usuario);
        bool ValidateToken(string token);
    }
}