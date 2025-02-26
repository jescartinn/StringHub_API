using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StringHub.Data;
using StringHub.DTOs;
using StringHub.Models;

namespace StringHub.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DisponibilidadService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DisponibilidadDto>> GetAllDisponibilidadesAsync()
        {
            var disponibilidades = await _context.Disponibilidades
                .OrderBy(d => d.DiaSemana)
                .ThenBy(d => d.HoraInicio)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<DisponibilidadDto>>(disponibilidades);
        }

        public async Task<DisponibilidadDto?> GetDisponibilidadByIdAsync(int id)
        {
            var disponibilidad = await _context.Disponibilidades.FindAsync(id);
            if (disponibilidad == null) return null;
            
            return _mapper.Map<DisponibilidadDto>(disponibilidad);
        }

        public async Task<IEnumerable<DisponibilidadDto>> GetDisponibilidadesByEncordadorAsync(int encordadorId)
        {
            var disponibilidades = await _context.Disponibilidades
                .Where(d => d.EncordadorId == encordadorId)
                .OrderBy(d => d.DiaSemana)
                .ThenBy(d => d.HoraInicio)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<DisponibilidadDto>>(disponibilidades);
        }

        public async Task<IEnumerable<DisponibilidadDto>> GetDisponibilidadesByDiaSemanaAsync(byte diaSemana)
        {
            if (diaSemana < 1 || diaSemana > 7)
            {
                throw new ArgumentException("El día de la semana debe estar entre 1 y 7");
            }

            var disponibilidades = await _context.Disponibilidades
                .Where(d => d.DiaSemana == diaSemana)
                .OrderBy(d => d.HoraInicio)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<DisponibilidadDto>>(disponibilidades);
        }

        public async Task<DisponibilidadDto> CreateDisponibilidadAsync(DisponibilidadCreateDto disponibilidadDto)
        {
            ValidateDisponibilidad(disponibilidadDto.DiaSemana, disponibilidadDto.HoraInicio, disponibilidadDto.HoraFin);

            if (await HasConflictingScheduleAsync(
                disponibilidadDto.EncordadorId,
                disponibilidadDto.DiaSemana,
                disponibilidadDto.HoraInicio,
                disponibilidadDto.HoraFin))
            {
                throw new InvalidOperationException("El horario se solapa con otro existente para este encordador");
            }

            var disponibilidad = _mapper.Map<Disponibilidad>(disponibilidadDto);
            
            _context.Disponibilidades.Add(disponibilidad);
            await _context.SaveChangesAsync();
            
            return _mapper.Map<DisponibilidadDto>(disponibilidad);
        }

        public async Task UpdateDisponibilidadAsync(int id, DisponibilidadUpdateDto disponibilidadDto)
        {
            var disponibilidad = await _context.Disponibilidades.FindAsync(id);
            if (disponibilidad == null)
            {
                throw new KeyNotFoundException($"No se encontró la disponibilidad con ID {id}");
            }

            byte diaSemana = disponibilidadDto.DiaSemana ?? disponibilidad.DiaSemana;
            TimeSpan horaInicio = disponibilidadDto.HoraInicio ?? disponibilidad.HoraInicio;
            TimeSpan horaFin = disponibilidadDto.HoraFin ?? disponibilidad.HoraFin;

            ValidateDisponibilidad(diaSemana, horaInicio, horaFin);

            if (await HasConflictingScheduleAsync(
                disponibilidad.EncordadorId,
                diaSemana,
                horaInicio,
                horaFin,
                id))
            {
                throw new InvalidOperationException("El horario se solapa con otro existente para este encordador");
            }

            _mapper.Map(disponibilidadDto, disponibilidad);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDisponibilidadAsync(int id)
        {
            var disponibilidad = await _context.Disponibilidades.FindAsync(id);
            if (disponibilidad == null)
            {
                throw new KeyNotFoundException($"No se encontró la disponibilidad con ID {id}");
            }

            _context.Disponibilidades.Remove(disponibilidad);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateDisponibilidadExistsAsync(int id)
        {
            return await _context.Disponibilidades.AnyAsync(d => d.DisponibilidadId == id);
        }

        private void ValidateDisponibilidad(byte diaSemana, TimeSpan horaInicio, TimeSpan horaFin)
        {
            var errors = new List<string>();

            if (diaSemana < 1 || diaSemana > 7)
                errors.Add("El día de la semana debe estar entre 1 y 7");

            if (horaInicio >= horaFin)
                errors.Add("La hora de inicio debe ser anterior a la hora de fin");

            // Validar que las horas estén dentro de un rango razonable (por ejemplo, 6:00 a 22:00)
            var minHora = new TimeSpan(6, 0, 0);
            var maxHora = new TimeSpan(22, 0, 0);

            if (horaInicio < minHora || horaInicio > maxHora)
                errors.Add("La hora de inicio debe estar entre las 6:00 y las 22:00");

            if (horaFin < minHora || horaFin > maxHora)
                errors.Add("La hora de fin debe estar entre las 6:00 y las 22:00");

            if (errors.Any())
                throw new InvalidOperationException(string.Join(", ", errors));
        }

        private async Task<bool> HasConflictingScheduleAsync(int encordadorId, byte diaSemana, TimeSpan horaInicio, TimeSpan horaFin, int? disponibilidadId = null)
        {
            var query = _context.Disponibilidades
                .Where(d => d.EncordadorId == encordadorId &&
                            d.DiaSemana == diaSemana &&
                            ((d.HoraInicio < horaFin && d.HoraFin > horaInicio) ||
                             (d.HoraInicio == horaInicio && d.HoraFin == horaFin)));

            if (disponibilidadId.HasValue)
            {
                query = query.Where(d => d.DisponibilidadId != disponibilidadId);
            }

            return await query.AnyAsync();
        }
    }
}