using StringHub.Models;
namespace StringHub.Repositories;

public interface IHistorialTensionRepository
{
    Task<IEnumerable<HistorialTension>> GetAllAsync();
    Task<HistorialTension?> GetByIdAsync(int id);
    Task<IEnumerable<HistorialTension>> GetByRaquetaIdAsync(int raquetaId);
    Task<IEnumerable<HistorialTension>> GetByOrdenIdAsync(int ordenId);
    Task<HistorialTension> CreateAsync(HistorialTension historial);
    Task UpdateAsync(HistorialTension historial);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}