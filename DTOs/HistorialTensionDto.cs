namespace StringHub.DTOs
{
    public class HistorialTensionDto
    {
        public int HistorialId { get; set; }
        public int RaquetaId { get; set; }
        public int OrdenId { get; set; }
        public decimal TensionVertical { get; set; }
        public decimal? TensionHorizontal { get; set; }
        public int? CuerdaId { get; set; }
        public DateTime Fecha { get; set; }
    }
}