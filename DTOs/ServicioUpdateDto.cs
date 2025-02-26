using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class ServicioUpdateDto
    {
        [MaxLength(100)]
        public string? NombreServicio { get; set; }
        
        public string? Descripcion { get; set; }
        
        [Range(0.01, 1000)]
        public decimal? PrecioBase { get; set; }
        
        [Range(1, 1000)]
        public int? TiempoEstimado { get; set; }
        
        public bool? Activo { get; set; }
    }
}