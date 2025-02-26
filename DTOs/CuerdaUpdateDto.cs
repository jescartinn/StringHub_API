using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class CuerdaUpdateDto
    {
        [MaxLength(50)]
        public string? Marca { get; set; }
        
        [MaxLength(50)]
        public string? Modelo { get; set; }
        
        [MaxLength(20)]
        public string? Calibre { get; set; }
        
        [MaxLength(50)]
        public string? Material { get; set; }
        
        [MaxLength(30)]
        public string? Color { get; set; }
        
        [Range(0.01, 1000)]
        public decimal? Precio { get; set; }
        
        [Range(0, 10000)]
        public int? Stock { get; set; }
        
        public bool? Activo { get; set; }
    }
}