using StringHub.Models;
namespace StringHub.Repositories;

public interface IDisponibilidadRepository
{
    Task<IEnumerable<Disponibilidad>> GetAllAsync();
    Task<Disponibilidad?> GetByIdAsync(int id);
    Task<IEnumerable<Disponibilidad>> GetByEncordadorIdAsync(int encordadorId);
    Task<IEnumerable<Disponibilidad>> GetByDiaSemanaAsync(byte diaSemana);
    Task<Disponibilidad> CreateAsync(Disponibilidad disponibilidad);
    Task UpdateAsync(Disponibilidad disponibilidad);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> HasConflictingScheduleAsync(int encordadorId, byte diaSemana, TimeSpan horaInicio, TimeSpan horaFin, int? disponibilidadId = null);
}