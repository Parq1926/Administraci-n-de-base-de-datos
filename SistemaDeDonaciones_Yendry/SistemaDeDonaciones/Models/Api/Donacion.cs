namespace SistemaDeDonaciones.Models.Api;

//Servicio.Donaciones
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

    //JOIN de código de donación
    public string? CodigoDonacionNombre { get; set; }

    //JOIN de donante
    public string? DonanteNombre { get; set; }
    public string? DonanteApellidos { get; set; }
    public string? DonanteEmail { get; set; }

    //JOIN de proyecto
    public string? ProyectoNombre { get; set; }
    public string? ProyectoEstado { get; set; }

    //Solo para vista
    public string DonanteNombreCompleto =>
        string.IsNullOrWhiteSpace(DonanteNombre) ? "—" : $"{DonanteNombre} {DonanteApellidos}".Trim();
}

public class DonacionInput
{
    public decimal Monto { get; set; }
    public DateTime? FechaDonacion { get; set; }
    public string MetodoPago { get; set; } = "";
    public string Estado { get; set; } = "Pendiente";
    public string? Comentario { get; set; }
    public int? IdDonante { get; set; }
    public int? IdProyecto { get; set; }
    public int? IdCodigoDonacion { get; set; }
}

public class DonacionUpdateInput
{
    public int IdDonacion { get; set; }
    public decimal? Monto { get; set; }
    public string? MetodoPago { get; set; }
    public string? Estado { get; set; }
    public string? Comentario { get; set; }
    public int? IdDonante { get; set; }
    public int? IdProyecto { get; set; }
    public int? IdCodigoDonacion { get; set; }
}
