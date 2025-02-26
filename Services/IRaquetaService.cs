using StringHub.DTOs;

namespace StringHub.Services
{
    public interface IRaquetaService
    {
        Task<IEnumerable<RaquetaDto>> GetAllRaquetasAsync();
        Task<RaquetaDto?> GetRaquetaByIdAsync(int id);
        Task<IEnumerable<RaquetaDto>> GetRaquetasByUserIdAsync(int userId);
        Task<RaquetaDto> CreateRaquetaAsync(RaquetaCreateDto raqueta);
        Task UpdateRaquetaAsync(int id, RaquetaUpdateDto raqueta);
        Task DeleteRaquetaAsync(int id);
        Task<bool> ValidateRaquetaExistsAsync(int id);
    }
}