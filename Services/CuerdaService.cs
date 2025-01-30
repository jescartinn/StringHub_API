using StringHub.Repositories;
using Models;

namespace StringHub.Services
{
    public class CuerdaService : ICuerdaService
    {
        private readonly ICuerdaRepository _cuerdaRepository;

        public CuerdaService(ICuerdaRepository cuerdaRepository)
        {
            _cuerdaRepository = cuerdaRepository;
        }

        public async Task<IEnumerable<Cuerda>> GetAllCuerdasAsync()
        {
            return await _cuerdaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Cuerda>> GetCuerdasActivasAsync()
        {
            return await _cuerdaRepository.GetActivasAsync();
        }

        public async Task<Cuerda?> GetCuerdaByIdAsync(int id)
        {
            return await _cuerdaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Cuerda>> GetCuerdasByMarcaAsync(string marca)
        {
            return await _cuerdaRepository.GetByMarcaAsync(marca);
        }

        public async Task<Cuerda> CreateCuerdaAsync(Cuerda cuerda)
        {
            if (cuerda.Precio <= 0)
            {
                throw new InvalidOperationException("El precio debe ser mayor que 0");
            }

            if (string.IsNullOrWhiteSpace(cuerda.Marca) || 
                string.IsNullOrWhiteSpace(cuerda.Modelo) || 
                string.IsNullOrWhiteSpace(cuerda.Calibre) || 
                string.IsNullOrWhiteSpace(cuerda.Material))
            {
                throw new InvalidOperationException("Todos los campos obligatorios deben estar completos");
            }

            cuerda.Activo = true;
            return await _cuerdaRepository.CreateAsync(cuerda);
        }

        public async Task UpdateCuerdaAsync(int id, Cuerda cuerda)
        {
            if (id != cuerda.CuerdaId)
            {
                throw new ArgumentException("El ID de la cuerda no coincide con el ID proporcionado");
            }

            if (cuerda.Precio <= 0)
            {
                throw new InvalidOperationException("El precio debe ser mayor que 0");
            }

            await _cuerdaRepository.UpdateAsync(cuerda);
        }

        public async Task DeleteCuerdaAsync(int id)
        {
            await _cuerdaRepository.DeleteAsync(id);
        }

        public async Task UpdateStockCuerdaAsync(int id, int cantidad)
        {
            var cuerda = await _cuerdaRepository.GetByIdAsync(id);
            if (cuerda == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuerda con ID {id}");
            }

            if (cuerda.Stock + cantidad < 0)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible");
            }

            await _cuerdaRepository.UpdateStockAsync(id, cantidad);
        }

        public async Task<bool> ValidateCuerdaExistsAsync(int id)
        {
            return await _cuerdaRepository.ExistsAsync(id);
        }
    }
}