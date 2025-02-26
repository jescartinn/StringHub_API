using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class HistorialTensionCreateDto
    {
        [Required]
        public int RaquetaId { get; set; }
        
        [Required]
        public int OrdenId { get; set; }
        
        [Required, Range(10, 40)]
        public decimal TensionVertical { get; set; }
        
        [Range(10, 40)]
        public decimal? TensionHorizontal { get; set; }
        
        public int? CuerdaId { get; set; }
    }
}