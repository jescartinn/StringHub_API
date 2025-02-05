namespace StringHub.Models;

public class Cuerda
{
    public int CuerdaId { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Calibre { get; set; }
    public string Material { get; set; }
    public string? Color { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public bool Activo { get; set; }

    public Cuerda() {} //parameterless constructor

    public Cuerda(string marca, string modelo, string calibre, string material, decimal precio)
    {
        Marca = marca;
        Modelo = modelo;
        Calibre = calibre;
        Material = material;
        Precio = precio;
        Stock = 0;
        Activo = true;
    }
}