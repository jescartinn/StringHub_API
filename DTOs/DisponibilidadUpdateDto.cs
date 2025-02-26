using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class DisponibilidadUpdateDto
    {
        [Range(1, 7)]
        public byte? DiaSemana { get; set; }
        
        public TimeSpan? HoraInicio { get; set; }
        
        public TimeSpan? HoraFin { get; set; }
    }
}