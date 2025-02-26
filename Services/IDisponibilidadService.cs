using StringHub.DTOs;

namespace StringHub.Services
{
    public interface IDisponibilidadService
    {
        Task<IEnumerable<DisponibilidadDto>> GetAllDisponibilidadesAsync();
        Task<DisponibilidadDto?> GetDisponibilidadByIdAsync(int id);
        Task<IEnumerable<DisponibilidadDto>> GetDisponibilidadesByEncordadorAsync(int encordadorId);
        Task<IEnumerable<DisponibilidadDto>> GetDisponibilidadesByDiaSemanaAsync(byte diaSemana);
        Task<DisponibilidadDto> CreateDisponibilidadAsync(DisponibilidadCreateDto disponibilidad);
        Task UpdateDisponibilidadAsync(int id, DisponibilidadUpdateDto disponibilidad);
        Task DeleteDisponibilidadAsync(int id);
        Task<bool> ValidateDisponibilidadExistsAsync(int id);
    }
}