using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class RaquetaCreateDto
    {
        [Required]
        public int UsuarioId { get; set; }
        
        [Required, MaxLength(50)]
        public string Marca { get; set; } = null!;
        
        [Required, MaxLength(50)]
        public string Modelo { get; set; } = null!;
        
        [MaxLength(50)]
        public string? NumeroSerie { get; set; }
        
        public string? Descripcion { get; set; }
    }
}