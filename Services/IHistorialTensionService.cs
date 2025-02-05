using StringHub.Models;
namespace StringHub.Services;

public interface IHistorialTensionService
{
    Task<IEnumerable<HistorialTension>> GetAllHistorialAsync();
    Task<HistorialTension?> GetHistorialByIdAsync(int id);
    Task<IEnumerable<HistorialTension>> GetHistorialByRaquetaAsync(int raquetaId);
    Task<IEnumerable<HistorialTension>> GetHistorialByOrdenAsync(int ordenId);
    Task<HistorialTension> CreateHistorialAsync(HistorialTension historial);
    Task UpdateHistorialAsync(int id, HistorialTension historial);
    Task DeleteHistorialAsync(int id);
    Task<bool> ValidateHistorialExistsAsync(int id);
}