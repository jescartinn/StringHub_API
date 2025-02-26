using StringHub.DTOs;

namespace StringHub.Services
{
    public interface IOrdenEncordadoService
    {
        Task<IEnumerable<OrdenEncordadoDto>> GetAllOrdenesAsync();
        Task<OrdenEncordadoDto?> GetOrdenByIdAsync(int id);
        Task<IEnumerable<OrdenEncordadoDto>> GetOrdenesByUsuarioAsync(int usuarioId);
        Task<IEnumerable<OrdenEncordadoDto>> GetOrdenesByEncordadorAsync(int encordadorId);
        Task<IEnumerable<OrdenEncordadoDto>> GetOrdenesByEstadoAsync(string estado);
        Task<OrdenEncordadoDto> CreateOrdenAsync(OrdenEncordadoCreateDto orden);
        Task UpdateOrdenAsync(int id, OrdenEncordadoUpdateDto orden);
        Task DeleteOrdenAsync(int id);
        Task UpdateEstadoOrdenAsync(int id, string estado, int? encordadorId = null);
        Task<bool> ValidateOrdenExistsAsync(int id);
    }
}