using StringHub.Models;
namespace StringHub.Services;

public interface IDisponibilidadService
{
    Task<IEnumerable<Disponibilidad>> GetAllDisponibilidadesAsync();
    Task<Disponibilidad?> GetDisponibilidadByIdAsync(int id);
    Task<IEnumerable<Disponibilidad>> GetDisponibilidadesByEncordadorAsync(int encordadorId);
    Task<IEnumerable<Disponibilidad>> GetDisponibilidadesByDiaSemanaAsync(byte diaSemana);
    Task<Disponibilidad> CreateDisponibilidadAsync(Disponibilidad disponibilidad);
    Task UpdateDisponibilidadAsync(int id, Disponibilidad disponibilidad);
    Task DeleteDisponibilidadAsync(int id);
    Task<bool> ValidateDisponibilidadExistsAsync(int id);
}