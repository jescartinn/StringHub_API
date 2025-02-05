using StringHub.Models;
namespace StringHub.Repositories;

public interface IServicioRepository
{
    Task<IEnumerable<Servicio>> GetAllAsync();
    Task<IEnumerable<Servicio>> GetActivosAsync();
    Task<Servicio?> GetByIdAsync(int id);
    Task<Servicio> CreateAsync(Servicio servicio);
    Task UpdateAsync(Servicio servicio);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}