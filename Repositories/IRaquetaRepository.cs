using StringHub.Models;
namespace StringHub.Repositories;

public interface IRaquetaRepository
{
    Task<IEnumerable<Raqueta>> GetAllAsync();
    Task<Raqueta?> GetByIdAsync(int id);
    Task<IEnumerable<Raqueta>> GetByUserIdAsync(int userId);
    Task<Raqueta> CreateAsync(Raqueta raqueta);
    Task UpdateAsync(Raqueta raqueta);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}