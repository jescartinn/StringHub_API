using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class ServicioCreateDto
    {
        [Required, MaxLength(100)]
        public string NombreServicio { get; set; } = null!;
        
        public string? Descripcion { get; set; }
        
        [Required, Range(0.01, 1000)]
        public decimal PrecioBase { get; set; }
        
        [Required, Range(1, 1000)]
        public int TiempoEstimado { get; set; }
    }
}