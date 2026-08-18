namespace DonacionesProyecto.Models;

public class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = "";

    public string? Apellidos { get; set; }

    public string Email { get; set; } = "";

    public string? TipoUsuario { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public bool? Activo { get; set; }
}
