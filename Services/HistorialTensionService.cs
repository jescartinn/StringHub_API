using StringHub.Repositories;
using StringHub.Models;

namespace StringHub.Services
{
    public class HistorialTensionService : IHistorialTensionService
    {
        private readonly IHistorialTensionRepository _historialRepository;

        public HistorialTensionService(IHistorialTensionRepository historialRepository)
        {
            _historialRepository = historialRepository;
        }

        public async Task<IEnumerable<HistorialTension>> GetAllHistorialAsync()
        {
            return await _historialRepository.GetAllAsync();
        }

        public async Task<HistorialTension?> GetHistorialByIdAsync(int id)
        {
            return await _historialRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<HistorialTension>> GetHistorialByRaquetaAsync(int raquetaId)
        {
            return await _historialRepository.GetByRaquetaIdAsync(raquetaId);
        }

        public async Task<IEnumerable<HistorialTension>> GetHistorialByOrdenAsync(int ordenId)
        {
            return await _historialRepository.GetByOrdenIdAsync(ordenId);
        }

        public async Task<HistorialTension> CreateHistorialAsync(HistorialTension historial)
        {
            ValidateHistorial(historial);
            return await _historialRepository.CreateAsync(historial);
        }

        public async Task UpdateHistorialAsync(int id, HistorialTension historial)
        {
            if (id != historial.HistorialId)
            {
                throw new ArgumentException("El ID del historial no coincide con el ID proporcionado");
            }

            ValidateHistorial(historial);
            await _historialRepository.UpdateAsync(historial);
        }

        public async Task DeleteHistorialAsync(int id)
        {
            var historial = await _historialRepository.GetByIdAsync(id);
            if (historial == null)
            {
                throw new KeyNotFoundException($"No se encontró el historial con ID {id}");
            }

            await _historialRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateHistorialExistsAsync(int id)
        {
            return await _historialRepository.ExistsAsync(id);
        }

        private void ValidateHistorial(HistorialTension historial)
        {
            var errors = new List<string>();

            if (historial.TensionVertical <= 0)
                errors.Add("La tensión vertical debe ser mayor que 0");

            if (historial.TensionHorizontal.HasValue && historial.TensionHorizontal <= 0)
                errors.Add("La tensión horizontal debe ser mayor que 0");

            if (errors.Any())
                throw new InvalidOperationException(string.Join(", ", errors));
        }
    }
}