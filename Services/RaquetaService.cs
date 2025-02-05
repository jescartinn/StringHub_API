using StringHub.Repositories;
using StringHub.Models;

namespace StringHub.Services
{
    public class RaquetaService : IRaquetaService
    {
        private readonly IRaquetaRepository _raquetaRepository;

        public RaquetaService(IRaquetaRepository raquetaRepository)
        {
            _raquetaRepository = raquetaRepository;
        }

        public async Task<IEnumerable<Raqueta>> GetAllRaquetasAsync()
        {
            return await _raquetaRepository.GetAllAsync();
        }

        public async Task<Raqueta?> GetRaquetaByIdAsync(int id)
        {
            return await _raquetaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Raqueta>> GetRaquetasByUserIdAsync(int userId)
        {
            return await _raquetaRepository.GetByUserIdAsync(userId);
        }

        public async Task<Raqueta> CreateRaquetaAsync(Raqueta raqueta)
        {
            return await _raquetaRepository.CreateAsync(raqueta);
        }

        public async Task UpdateRaquetaAsync(int id, Raqueta raqueta)
        {
            await _raquetaRepository.UpdateAsync(raqueta);
        }

        public async Task DeleteRaquetaAsync(int id)
        {
            await _raquetaRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateRaquetaExistsAsync(int id)
        {
            return await _raquetaRepository.ExistsAsync(id);
        }
    }
}