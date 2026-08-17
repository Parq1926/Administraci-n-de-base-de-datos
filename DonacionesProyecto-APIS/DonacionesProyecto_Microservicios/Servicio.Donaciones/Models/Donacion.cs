namespace DonacionesProyecto.Models;

public class Donacion
{
    public int IdDonacion { get; set; }

    public decimal Monto { get; set; }

    public DateTime? FechaDonacion { get; set; }

    public string? MetodoPago { get; set; }

    public string? Estado { get; set; }

    public string? Comentario { get; set; }

    public int? IdCodigoDonacion { get; set; }

    public int? IdDonante { get; set; }

    public int? IdProyecto { get; set; }

    //JOIN CODIGO DE DONACION
    public string? CodigoDonacionNombre { get; set; }

    //JOIN DONANTE
    public string? DonanteNombre { get; set; }

    public string? DonanteApellidos { get; set; }

    public string? DonanteEmail { get; set; }

    //JOIN PROYECTO
    public string? ProyectoNombre { get; set; }

    public string? ProyectoEstado { get; set; }
}