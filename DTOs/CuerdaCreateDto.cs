using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class CuerdaCreateDto
    {
        [Required, MaxLength(50)]
        public string Marca { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Modelo { get; set; } = null!;
        
        [Required, MaxLength(20)]
        public string Calibre { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Material { get; set; } = null!;
        
        [MaxLength(30)]
        public string? Color { get; set; }
        
        [Required, Range(0.01, 1000)]
        public decimal Precio { get; set; }
        
        [Range(0, 10000)]
        public int Stock { get; set; }
    }
}