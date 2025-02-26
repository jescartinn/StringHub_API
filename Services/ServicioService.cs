using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class ServicioService : IServicioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ServicioService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicioDto>> GetAllServiciosAsync()
        {
            var servicios = await _context.Servicios
                .OrderBy(s => s.ServicioId)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<ServicioDto>>(servicios);
        }

        public async Task<IEnumerable<ServicioDto>> GetServiciosActivosAsync()
        {
            var servicios = await _context.Servicios
                .Where(s => s.Activo)
                .OrderBy(s => s.ServicioId)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<ServicioDto>>(servicios);
        }

        public async Task<ServicioDto?> GetServicioByIdAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null) return null;
            
            return _mapper.Map<ServicioDto>(servicio);
        }

        public async Task<ServicioDto> CreateServicioAsync(ServicioCreateDto servicioDto)
        {
            ValidateServicio(servicioDto);

            var servicio = _mapper.Map<Servicio>(servicioDto);
            servicio.Activo = true;
            
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<ServicioDto>(servicio);
        }

        public async Task UpdateServicioAsync(int id, ServicioUpdateDto servicioDto)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                throw new KeyNotFoundException($"No se encontró el servicio con ID {id}");
            }

            // Validar valores antes de mapear
            if (servicioDto.PrecioBase.HasValue && servicioDto.PrecioBase <= 0)
                throw new InvalidOperationException("El precio base debe ser mayor que 0");

            if (servicioDto.TiempoEstimado.HasValue && servicioDto.TiempoEstimado <= 0)
                throw new InvalidOperationException("El tiempo estimado debe ser mayor que 0");

            _mapper.Map(servicioDto, servicio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServicioAsync(int id)
        {
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                throw new KeyNotFoundException($"No se encontró el servicio con ID {id}");
            }

            // En lugar de eliminar físicamente, marcamos como inactivo
            servicio.Activo = false;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateServicioExistsAsync(int id)
        {
            return await _context.Servicios.AnyAsync(s => s.ServicioId == id);
        }

        private void ValidateServicio(ServicioCreateDto servicio)
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