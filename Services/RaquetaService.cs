using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class RaquetaService : IRaquetaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RaquetaService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RaquetaDto>> GetAllRaquetasAsync()
        {
            var raquetas = await _context.Raquetas.ToListAsync();
            return _mapper.Map<IEnumerable<RaquetaDto>>(raquetas);
        }

        public async Task<RaquetaDto?> GetRaquetaByIdAsync(int id)
        {
            var raqueta = await _context.Raquetas.FindAsync(id);
            if (raqueta == null) return null;
            
            return _mapper.Map<RaquetaDto>(raqueta);
        }

        public async Task<IEnumerable<RaquetaDto>> GetRaquetasByUserIdAsync(int userId)
        {
            var raquetas = await _context.Raquetas
                .Where(r => r.UsuarioId == userId)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<RaquetaDto>>(raquetas);
        }

        public async Task<RaquetaDto> CreateRaquetaAsync(RaquetaCreateDto raquetaDto)
        {
            var raqueta = _mapper.Map<Raqueta>(raquetaDto);
            raqueta.FechaCreacion = DateTime.UtcNow;
            
            _context.Raquetas.Add(raqueta);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<RaquetaDto>(raqueta);
        }

        public async Task UpdateRaquetaAsync(int id, RaquetaUpdateDto raquetaDto)
        {
            var raqueta = await _context.Raquetas.FindAsync(id);
            if (raqueta == null)
            {
                throw new KeyNotFoundException($"No se encontró la raqueta con ID {id}");
            }

            _mapper.Map(raquetaDto, raqueta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRaquetaAsync(int id)
        {
            var raqueta = await _context.Raquetas.FindAsync(id);
            if (raqueta == null)
            {
                throw new KeyNotFoundException($"No se encontró la raqueta con ID {id}");
            }

            _context.Raquetas.Remove(raqueta);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateRaquetaExistsAsync(int id)
        {
            return await _context.Raquetas.AnyAsync(r => r.RaquetaId == id);
        }
    }
}