using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class HistorialTensionService : IHistorialTensionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HistorialTensionService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HistorialTensionDto>> GetAllHistorialAsync()
        {
            var historiales = await _context.HistorialTensiones
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<HistorialTensionDto>>(historiales);
        }

        public async Task<HistorialTensionDto?> GetHistorialByIdAsync(int id)
        {
            var historial = await _context.HistorialTensiones.FindAsync(id);
            if (historial == null) return null;
            
            return _mapper.Map<HistorialTensionDto>(historial);
        }

        public async Task<IEnumerable<HistorialTensionDto>> GetHistorialByRaquetaAsync(int raquetaId)
        {
            var historiales = await _context.HistorialTensiones
                .Where(h => h.RaquetaId == raquetaId)
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<HistorialTensionDto>>(historiales);
        }

        public async Task<IEnumerable<HistorialTensionDto>> GetHistorialByOrdenAsync(int ordenId)
        {
            var historiales = await _context.HistorialTensiones
                .Where(h => h.OrdenId == ordenId)
                .OrderByDescending(h => h.Fecha)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<HistorialTensionDto>>(historiales);
        }

        public async Task<HistorialTensionDto> CreateHistorialAsync(HistorialTensionCreateDto historialDto)
        {
            ValidateHistorial(historialDto);

            var historial = _mapper.Map<HistorialTension>(historialDto);
            historial.Fecha = DateTime.UtcNow;
            
            _context.HistorialTensiones.Add(historial);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<HistorialTensionDto>(historial);
        }

        public async Task UpdateHistorialAsync(int id, HistorialTensionCreateDto historialDto)
        {
            var historial = await _context.HistorialTensiones.FindAsync(id);
            if (historial == null)
            {
                throw new KeyNotFoundException($"No se encontró el historial con ID {id}");
            }

            ValidateHistorial(historialDto);
            
            _mapper.Map(historialDto, historial);
            // Mantener la fecha original
            // historial.Fecha permanece sin cambios
            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHistorialAsync(int id)
        {
            var historial = await _context.HistorialTensiones.FindAsync(id);
            if (historial == null)
            {
                throw new KeyNotFoundException($"No se encontró el historial con ID {id}");
            }

            _context.HistorialTensiones.Remove(historial);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateHistorialExistsAsync(int id)
        {
            return await _context.HistorialTensiones.AnyAsync(h => h.HistorialId == id);
        }

        private void ValidateHistorial(HistorialTensionCreateDto historial)
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