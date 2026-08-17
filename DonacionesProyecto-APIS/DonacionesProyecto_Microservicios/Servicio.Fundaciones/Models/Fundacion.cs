namespace DonacionesProyecto.Models;

public class Fundacion
{
    public int IdFundacion { get; set; }

    public string Nombre { get; set; } = "";

    public string? Descripcion { get; set; }

    public string? Identificacion { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }
}