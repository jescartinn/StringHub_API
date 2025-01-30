namespace Models;
public class Raqueta
{
    public int RaquetaId { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public double Precio { get; set; }
    public string? NumeroSerie { get; set; }
    public double Peso { get; set; }
    public string? Descripcion { get; set; }
    public int UserId { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Raqueta() {} //parameterless constructor

    public Raqueta(string marca, string modelo, double precio)
    {
        Marca = marca;
        Modelo = modelo;
        Precio = precio;
        FechaCreacion = DateTime.UtcNow;
    }
}