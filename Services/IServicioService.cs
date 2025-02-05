using StringHub.Models;
namespace StringHub.Services;

public interface IServicioService
{
    Task<IEnumerable<Servicio>> GetAllServiciosAsync();
    Task<IEnumerable<Servicio>> GetServiciosActivosAsync();
    Task<Servicio?> GetServicioByIdAsync(int id);
    Task<Servicio> CreateServicioAsync(Servicio servicio);
    Task UpdateServicioAsync(int id, Servicio servicio);
    Task DeleteServicioAsync(int id);
    Task<bool> ValidateServicioExistsAsync(int id);
}