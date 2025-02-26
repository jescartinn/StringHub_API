using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class CuerdaService : ICuerdaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CuerdaService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CuerdaDto>> GetAllCuerdasAsync()
        {
            var cuerdas = await _context.Cuerdas
                .OrderBy(c => c.Marca)
                .ThenBy(c => c.Modelo)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<CuerdaDto>>(cuerdas);
        }

        public async Task<IEnumerable<CuerdaDto>> GetCuerdasActivasAsync()
        {
            var cuerdas = await _context.Cuerdas
                .Where(c => c.Activo)
                .OrderBy(c => c.Marca)
                .ThenBy(c => c.Modelo)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<CuerdaDto>>(cuerdas);
        }

        public async Task<CuerdaDto?> GetCuerdaByIdAsync(int id)
        {
            var cuerda = await _context.Cuerdas.FindAsync(id);
            if (cuerda == null) return null;
            
            return _mapper.Map<CuerdaDto>(cuerda);
        }

        public async Task<IEnumerable<CuerdaDto>> GetCuerdasByMarcaAsync(string marca)
        {
            var cuerdas = await _context.Cuerdas
                .Where(c => c.Marca.ToLower() == marca.ToLower())
                .OrderBy(c => c.Modelo)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<CuerdaDto>>(cuerdas);
        }

        public async Task<CuerdaDto> CreateCuerdaAsync(CuerdaCreateDto cuerdaDto)
        {
            if (cuerdaDto.Precio <= 0)
            {
                throw new InvalidOperationException("El precio debe ser mayor que 0");
            }

            if (string.IsNullOrWhiteSpace(cuerdaDto.Marca) || 
                string.IsNullOrWhiteSpace(cuerdaDto.Modelo) || 
                string.IsNullOrWhiteSpace(cuerdaDto.Calibre) || 
                string.IsNullOrWhiteSpace(cuerdaDto.Material))
            {
                throw new InvalidOperationException("Todos los campos obligatorios deben estar completos");
            }

            var cuerda = _mapper.Map<Cuerda>(cuerdaDto);
            cuerda.Activo = true;
            
            _context.Cuerdas.Add(cuerda);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<CuerdaDto>(cuerda);
        }

        public async Task UpdateCuerdaAsync(int id, CuerdaUpdateDto cuerdaDto)
        {
            var cuerda = await _context.Cuerdas.FindAsync(id);
            if (cuerda == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuerda con ID {id}");
            }

            if (cuerdaDto.Precio.HasValue && cuerdaDto.Precio <= 0)
            {
                throw new InvalidOperationException("El precio debe ser mayor que 0");
            }

            _mapper.Map(cuerdaDto, cuerda);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCuerdaAsync(int id)
        {
            var cuerda = await _context.Cuerdas.FindAsync(id);
            if (cuerda == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuerda con ID {id}");
            }

            // Marcamos como inactivo en lugar de eliminar físicamente
            cuerda.Activo = false;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStockCuerdaAsync(int id, int cantidad)
        {
            var cuerda = await _context.Cuerdas.FindAsync(id);
            if (cuerda == null)
            {
                throw new KeyNotFoundException($"No se encontró la cuerda con ID {id}");
            }

            if (cuerda.Stock + cantidad < 0)
            {
                throw new InvalidOperationException("No hay suficiente stock disponible");
            }

            cuerda.Stock += cantidad;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateCuerdaExistsAsync(int id)
        {
            return await _context.Cuerdas.AnyAsync(c => c.CuerdaId == id);
        }
    }
}