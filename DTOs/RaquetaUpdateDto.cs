using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class RaquetaUpdateDto
    {
        [MaxLength(50)]
        public string? Marca { get; set; }
        
        [MaxLength(50)]
        public string? Modelo { get; set; }
        
        [MaxLength(50)]
        public string? NumeroSerie { get; set; }
        
        public string? Descripcion { get; set; }
    }
}