using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class OrdenEncordadoCreateDto
    {
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public int RaquetaId { get; set; }
        
        [Required]
        public int ServicioId { get; set; }
        
        public int? CuerdaId { get; set; }
        
        [Required, Range(10, 40)]
        public decimal TensionVertical { get; set; }
        
        [Range(10, 40)]
        public decimal? TensionHorizontal { get; set; }
        
        public string? Comentarios { get; set; }
        
        [Required, Range(0.01, 10000)]
        public decimal PrecioTotal { get; set; }
    }
}