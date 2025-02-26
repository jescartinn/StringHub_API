using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class DisponibilidadCreateDto
    {
        [Required]
        public int EncordadorId { get; set; }
        
        [Required, Range(1, 7)]
        public byte DiaSemana { get; set; }
        
        [Required]
        public TimeSpan HoraInicio { get; set; }
        
        [Required]
        public TimeSpan HoraFin { get; set; }
    }
}