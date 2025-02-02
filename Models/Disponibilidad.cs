namespace Models;

public class Disponibilidad
{
    public int DisponibilidadId { get; set; }
    public int EncordadorId { get; set; }
    public byte DiaSemana { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
}