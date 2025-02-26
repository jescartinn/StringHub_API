namespace StringHub.DTOs
{
    public class OrdenEncordadoDto
    {
        public int OrdenId { get; set; }
        public int UsuarioId { get; set; }
        public int RaquetaId { get; set; }
        public int ServicioId { get; set; }
        public int? CuerdaId { get; set; }
        public decimal TensionVertical { get; set; }
        public decimal? TensionHorizontal { get; set; }
        public string Estado { get; set; } = null!;
        public string? Comentarios { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public decimal PrecioTotal { get; set; }
        public int? EncordadorId { get; set; }
    }
}