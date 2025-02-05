using StringHub.Models;
namespace StringHub.Services;

public interface IOrdenEncordadoService
{
    Task<IEnumerable<OrdenEncordado>> GetAllOrdenesAsync();
    Task<OrdenEncordado?> GetOrdenByIdAsync(int id);
    Task<IEnumerable<OrdenEncordado>> GetOrdenesByUsuarioAsync(int usuarioId);
    Task<IEnumerable<OrdenEncordado>> GetOrdenesByEncordadorAsync(int encordadorId);
    Task<IEnumerable<OrdenEncordado>> GetOrdenesByEstadoAsync(string estado);
    Task<OrdenEncordado> CreateOrdenAsync(OrdenEncordado orden);
    Task UpdateOrdenAsync(int id, OrdenEncordado orden);
    Task DeleteOrdenAsync(int id);
    Task UpdateEstadoOrdenAsync(int id, string estado, int? encordadorId = null);
    Task<bool> ValidateOrdenExistsAsync(int id);
}