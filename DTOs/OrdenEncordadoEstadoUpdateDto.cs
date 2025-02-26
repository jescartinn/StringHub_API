using System.ComponentModel.DataAnnotations;

namespace StringHub.DTOs
{
    public class OrdenEncordadoEstadoUpdateDto
    {
        [Required]
        public string Estado { get; set; } = null!;
        
        public int? EncordadorId { get; set; }
    }
}