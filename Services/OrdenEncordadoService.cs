using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class OrdenEncordadoService : IOrdenEncordadoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrdenEncordadoService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrdenEncordadoDto>> GetAllOrdenesAsync()
        {
            var ordenes = await _context.OrdenesEncordado
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<OrdenEncordadoDto>>(ordenes);
        }

        public async Task<OrdenEncordadoDto?> GetOrdenByIdAsync(int id)
        {
            var orden = await _context.OrdenesEncordado.FindAsync(id);
            if (orden == null) return null;
            
            return _mapper.Map<OrdenEncordadoDto>(orden);
        }

        public async Task<IEnumerable<OrdenEncordadoDto>> GetOrdenesByUsuarioAsync(int usuarioId)
        {
            var ordenes = await _context.OrdenesEncordado
                .Where(o => o.UsuarioId == usuarioId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<OrdenEncordadoDto>>(ordenes);
        }

        public async Task<IEnumerable<OrdenEncordadoDto>> GetOrdenesByEncordadorAsync(int encordadorId)
        {
            var ordenes = await _context.OrdenesEncordado
                .Where(o => o.EncordadorId == encordadorId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<OrdenEncordadoDto>>(ordenes);
        }

        public async Task<IEnumerable<OrdenEncordadoDto>> GetOrdenesByEstadoAsync(string estado)
        {
            if (!IsValidEstado(estado))
            {
                throw new InvalidOperationException($"Estado inválido: {estado}");
            }

            var ordenes = await _context.OrdenesEncordado
                .Where(o => o.Estado == estado)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<OrdenEncordadoDto>>(ordenes);
        }

        public async Task<OrdenEncordadoDto> CreateOrdenAsync(OrdenEncordadoCreateDto ordenDto)
        {
            ValidateOrden(ordenDto);

            var orden = _mapper.Map<OrdenEncordado>(ordenDto);
            orden.Estado = "Pendiente";
            orden.FechaCreacion = DateTime.UtcNow;

            _context.OrdenesEncordado.Add(orden);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<OrdenEncordadoDto>(orden);
        }

        public async Task UpdateOrdenAsync(int id, OrdenEncordadoUpdateDto ordenDto)
        {
            var orden = await _context.OrdenesEncordado.FindAsync(id);
            if (orden == null)
            {
                throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
            }

            // Validar valores antes de mapear
            if (ordenDto.TensionVertical.HasValue && ordenDto.TensionVertical <= 0)
                throw new InvalidOperationException("La tensión vertical debe ser mayor que 0");

            if (ordenDto.TensionHorizontal.HasValue && ordenDto.TensionHorizontal <= 0)
                throw new InvalidOperationException("La tensión horizontal debe ser mayor que 0");

            if (ordenDto.PrecioTotal.HasValue && ordenDto.PrecioTotal <= 0)
                throw new InvalidOperationException("El precio total debe ser mayor que 0");

            _mapper.Map(ordenDto, orden);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrdenAsync(int id)
        {
            var orden = await _context.OrdenesEncordado.FindAsync(id);
            if (orden == null)
            {
                throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
            }

            if (orden.Estado != "Pendiente")
            {
                throw new InvalidOperationException("Solo se pueden eliminar órdenes pendientes");
            }

            // En lugar de eliminar físicamente, actualizamos el estado a "Cancelado"
            orden.Estado = "Cancelado";
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEstadoOrdenAsync(int id, string estado, int? encordadorId = null)
        {
            if (!IsValidEstado(estado))
            {
                throw new InvalidOperationException($"Estado inválido: {estado}");
            }

            var orden = await _context.OrdenesEncordado.FindAsync(id);
            if (orden == null)
            {
                throw new KeyNotFoundException($"No se encontró la orden con ID {id}");
            }

            if (!IsValidEstadoTransition(orden.Estado, estado))
            {
                throw new InvalidOperationException($"Transición de estado inválida: {orden.Estado} -> {estado}");
            }

            if (estado == "En Proceso" && !encordadorId.HasValue)
            {
                throw new InvalidOperationException("Se requiere un encordador para cambiar el estado a 'En Proceso'");
            }

            orden.Estado = estado;
            
            if (encordadorId.HasValue)
            {
                orden.EncordadorId = encordadorId;
            }
            
            if (estado == "Completado")
            {
                orden.FechaCompletado = DateTime.UtcNow;
            }
            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateOrdenExistsAsync(int id)
        {
            return await _context.OrdenesEncordado.AnyAsync(o => o.OrdenId == id);
        }

        private void ValidateOrden(OrdenEncordadoCreateDto orden)
        {
            var errors = new List<string>();

            if (orden.TensionVertical <= 0)
                errors.Add("La tensión vertical debe ser mayor que 0");

            if (orden.TensionHorizontal.HasValue && orden.TensionHorizontal <= 0)
                errors.Add("La tensión horizontal debe ser mayor que 0");

            if (orden.PrecioTotal <= 0)
                errors.Add("El precio total debe ser mayor que 0");

            if (errors.Any())
                throw new InvalidOperationException(string.Join(", ", errors));
        }

        private bool IsValidEstado(string estado)
        {
            var estadosValidos = new[] { "Pendiente", "En Proceso", "Completado", "Entregado", "Cancelado" };
            return estadosValidos.Contains(estado);
        }

        private bool IsValidEstadoTransition(string estadoActual, string nuevoEstado)
        {
            var transicionesValidas = new Dictionary<string, string[]>
            {
                { "Pendiente", new[] { "En Proceso", "Cancelado" } },
                { "En Proceso", new[] { "Completado", "Cancelado" } },
                { "Completado", new[] { "Entregado" } },
                { "Entregado", new string[] { } },
                { "Cancelado", new string[] { } }
            };

            return transicionesValidas.ContainsKey(estadoActual) &&
                   transicionesValidas[estadoActual].Contains(nuevoEstado);
        }
    }
}