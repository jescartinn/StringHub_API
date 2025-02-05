using StringHub.Repositories;
using StringHub.Models;

namespace StringHub.Services
{
    public class ServicioService : IServicioService
    {
        private readonly IServicioRepository _servicioRepository;

        public ServicioService(IServicioRepository servicioRepository)
        {
            _servicioRepository = servicioRepository;
        }

        public async Task<IEnumerable<Servicio>> GetAllServiciosAsync()
        {
            return await _servicioRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Servicio>> GetServiciosActivosAsync()
        {
            return await _servicioRepository.GetActivosAsync();
        }

        public async Task<Servicio?> GetServicioByIdAsync(int id)
        {
            return await _servicioRepository.GetByIdAsync(id);
        }

        public async Task<Servicio> CreateServicioAsync(Servicio servicio)
        {
            ValidateServicio(servicio);
            return await _servicioRepository.CreateAsync(servicio);
        }

        public async Task UpdateServicioAsync(int id, Servicio servicio)
        {
            if (id != servicio.ServicioId)
            {
                throw new ArgumentException("El ID del servicio no coincide con el ID proporcionado");
            }

            ValidateServicio(servicio);
            await _servicioRepository.UpdateAsync(servicio);
        }

        public async Task DeleteServicioAsync(int id)
        {
            var servicio = await _servicioRepository.GetByIdAsync(id);
            if (servicio == null)
            {
                throw new KeyNotFoundException($"No se encontró el servicio con ID {id}");
            }

            await _servicioRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateServicioExistsAsync(int id)
        {
            return await _servicioRepository.ExistsAsync(id);
        }

        private void ValidateServicio(Servicio servicio)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(servicio.NombreServicio))
                errors.Add("El nombre del servicio es requerido");

            if (servicio.PrecioBase <= 0)
                errors.Add("El precio base debe ser mayor que 0");

            if (servicio.TiempoEstimado <= 0)
                errors.Add("El tiempo estimado debe ser mayor que 0");

            if (errors.Any())
                throw new InvalidOperationException(string.Join(", ", errors));
        }
    }
}