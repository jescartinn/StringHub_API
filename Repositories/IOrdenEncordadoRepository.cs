using StringHub.Models;
namespace StringHub.Repositories;

public interface IOrdenEncordadoRepository
{
    Task<IEnumerable<OrdenEncordado>> GetAllAsync();
    Task<OrdenEncordado?> GetByIdAsync(int id);
    Task<IEnumerable<OrdenEncordado>> GetByUsuarioIdAsync(int usuarioId);
    Task<IEnumerable<OrdenEncordado>> GetByEncordadorIdAsync(int encordadorId);
    Task<IEnumerable<OrdenEncordado>> GetByEstadoAsync(string estado);
    Task<OrdenEncordado> CreateAsync(OrdenEncordado orden);
    Task UpdateAsync(OrdenEncordado orden);
    Task DeleteAsync(int id);
    Task UpdateEstadoAsync(int id, string estado, int? encordadorId = null);
    Task<bool> ExistsAsync(int id);
}