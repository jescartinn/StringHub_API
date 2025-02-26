using StringHub.DTOs;

namespace StringHub.Services
{
    public interface IHistorialTensionService
    {
        Task<IEnumerable<HistorialTensionDto>> GetAllHistorialAsync();
        Task<HistorialTensionDto?> GetHistorialByIdAsync(int id);
        Task<IEnumerable<HistorialTensionDto>> GetHistorialByRaquetaAsync(int raquetaId);
        Task<IEnumerable<HistorialTensionDto>> GetHistorialByOrdenAsync(int ordenId);
        Task<HistorialTensionDto> CreateHistorialAsync(HistorialTensionCreateDto historial);
        Task UpdateHistorialAsync(int id, HistorialTensionCreateDto historial);
        Task DeleteHistorialAsync(int id);
        Task<bool> ValidateHistorialExistsAsync(int id);
    }
}