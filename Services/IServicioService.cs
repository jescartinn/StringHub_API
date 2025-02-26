using StringHub.DTOs;

namespace StringHub.Services
{
    public interface IServicioService
    {
        Task<IEnumerable<ServicioDto>> GetAllServiciosAsync();
        Task<IEnumerable<ServicioDto>> GetServiciosActivosAsync();
        Task<ServicioDto?> GetServicioByIdAsync(int id);
        Task<ServicioDto> CreateServicioAsync(ServicioCreateDto servicio);
        Task UpdateServicioAsync(int id, ServicioUpdateDto servicio);
        Task DeleteServicioAsync(int id);
        Task<bool> ValidateServicioExistsAsync(int id);
    }
}