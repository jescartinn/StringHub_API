using StringHub.Models;
namespace StringHub.Services;

public interface IRaquetaService
{
    Task<IEnumerable<Raqueta>> GetAllRaquetasAsync();
    Task<Raqueta?> GetRaquetaByIdAsync(int id);
    Task<IEnumerable<Raqueta>> GetRaquetasByUserIdAsync(int userId);
    Task<Raqueta> CreateRaquetaAsync(Raqueta raqueta);
    Task UpdateRaquetaAsync(int id, Raqueta raqueta);
    Task DeleteRaquetaAsync(int id);
    Task<bool> ValidateRaquetaExistsAsync(int id);
}