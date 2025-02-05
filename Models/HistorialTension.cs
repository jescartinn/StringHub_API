namespace StringHub.Models;

public class HistorialTension
{
    public int HistorialId { get; set; }
    public int RaquetaId { get; set; }
    public int OrdenId { get; set; }
    public decimal TensionVertical { get; set; }
    public decimal? TensionHorizontal { get; set; }
    public int? CuerdaId { get; set; }
    public DateTime Fecha { get; set; }

    public HistorialTension() 
    {
        Fecha = DateTime.UtcNow;
    }

    public HistorialTension(int raquetaId, int ordenId, decimal tensionVertical)
    {
        RaquetaId = raquetaId;
        OrdenId = ordenId;
        TensionVertical = tensionVertical;
        Fecha = DateTime.UtcNow;
    }
}