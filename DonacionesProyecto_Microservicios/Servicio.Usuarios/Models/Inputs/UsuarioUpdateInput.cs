namespace DonacionesProyecto.Models.Inputs;

public class UsuarioUpdateInput
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellidos { get; set; }

    public string? Email { get; set; }

    public string? Contrasena { get; set; }

    public string? TipoUsuario { get; set; }

    public bool? Activo { get; set; }
}
