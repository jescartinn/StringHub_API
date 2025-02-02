using StringHub.Repositories;
using Models;

namespace StringHub.Services
{
    public class DisponibilidadService : IDisponibilidadService
    {
        private readonly IDisponibilidadRepository _disponibilidadRepository;

        public DisponibilidadService(IDisponibilidadRepository disponibilidadRepository)
        {
            _disponibilidadRepository = disponibilidadRepository;
        }

        public async Task<IEnumerable<Disponibilidad>> GetAllDisponibilidadesAsync()
        {
            return await _disponibilidadRepository.GetAllAsync();
        }

        public async Task<Disponibilidad?> GetDisponibilidadByIdAsync(int id)
        {
            return await _disponibilidadRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Disponibilidad>> GetDisponibilidadesByEncordadorAsync(int encordadorId)
        {
            return await _disponibilidadRepository.GetByEncordadorIdAsync(encordadorId);
        }

        public async Task<IEnumerable<Disponibilidad>> GetDisponibilidadesByDiaSemanaAsync(byte diaSemana)
        {
            if (diaSemana < 1 || diaSemana > 7)
            {
                throw new ArgumentException("El día de la semana debe estar entre 1 y 7");
            }

            return await _disponibilidadRepository.GetByDiaSemanaAsync(diaSemana);
        }

        public async Task<Disponibilidad> CreateDisponibilidadAsync(Disponibilidad disponibilidad)
        {
            ValidateDisponibilidad(disponibilidad);

            if (await _disponibilidadRepository.HasConflictingScheduleAsync(
                disponibilidad.EncordadorId,
                disponibilidad.DiaSemana,
                disponibilidad.HoraInicio,
                disponibilidad.HoraFin))
            {
                throw new InvalidOperationException("El horario se solapa con otro existente para este encordador");
            }

            return await _disponibilidadRepository.CreateAsync(disponibilidad);
        }

        public async Task UpdateDisponibilidadAsync(int id, Disponibilidad disponibilidad)
        {
            if (id != disponibilidad.DisponibilidadId)
            {
                throw new ArgumentException("El ID de la disponibilidad no coincide con el ID proporcionado");
            }

            ValidateDisponibilidad(disponibilidad);

            if (await _disponibilidadRepository.HasConflictingScheduleAsync(
                disponibilidad.EncordadorId,
                disponibilidad.DiaSemana,
                disponibilidad.HoraInicio,
                disponibilidad.HoraFin,
                disponibilidad.DisponibilidadId))
            {
                throw new InvalidOperationException("El horario se solapa con otro existente para este encordador");
            }

            await _disponibilidadRepository.UpdateAsync(disponibilidad);
        }

        public async Task DeleteDisponibilidadAsync(int id)
        {
            if (!await _disponibilidadRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"No se encontró la disponibilidad con ID {id}");
            }

            await _disponibilidadRepository.DeleteAsync(id);
        }

        public async Task<bool> ValidateDisponibilidadExistsAsync(int id)
        {
            return await _disponibilidadRepository.ExistsAsync(id);
        }

        private void ValidateDisponibilidad(Disponibilidad disponibilidad)
        {
            var errors = new List<string>();

            if (disponibilidad.DiaSemana < 1 || disponibilidad.DiaSemana > 7)
                errors.Add("El día de la semana debe estar entre 1 y 7");

            if (disponibilidad.HoraInicio >= disponibilidad.HoraFin)
                errors.Add("La hora de inicio debe ser anterior a la hora de fin");

            // Validar que las horas estén dentro de un rango razonable (por ejemplo, 6:00 a 22:00)
            var minHora = new TimeSpan(6, 0, 0);
            var maxHora = new TimeSpan(22, 0, 0);

            if (disponibilidad.HoraInicio < minHora || disponibilidad.HoraInicio > maxHora)
                errors.Add("La hora de inicio debe estar entre las 6:00 y las 22:00");

            if (disponibilidad.HoraFin < minHora || disponibilidad.HoraFin > maxHora)
                errors.Add("La hora de fin debe estar entre las 6:00 y las 22:00");

            if (errors.Any())
                throw new InvalidOperationException(string.Join(", ", errors));
        }
    }
}