namespace StringHub.Models;

public class OrdenEncordado
{
    public int OrdenId { get; set; }
    public int UsuarioId { get; set; }
    public int RaquetaId { get; set; }
    public int ServicioId { get; set; }
    public int? CuerdaId { get; set; }
    public decimal TensionVertical { get; set; }
    public decimal? TensionHorizontal { get; set; }
    public string Estado { get; set; }
    public string? Comentarios { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaCompletado { get; set; }
    public decimal PrecioTotal { get; set; }
    public int? EncordadorId { get; set; }

    public OrdenEncordado() 
    {
        Estado = "Pendiente";
        FechaCreacion = DateTime.UtcNow;
    }

    public OrdenEncordado(int usuarioId, int raquetaId, int servicioId, decimal tensionVertical)
    {
        UsuarioId = usuarioId;
        RaquetaId = raquetaId;
        ServicioId = servicioId;
        TensionVertical = tensionVertical;
        Estado = "Pendiente";
        FechaCreacion = DateTime.UtcNow;
    }
}