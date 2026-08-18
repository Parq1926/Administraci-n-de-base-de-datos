namespace DonacionesProyecto.Models.Inputs;

public class UsuarioInput
{
    public string Nombre { get; set; } = "";

    public string? Apellidos { get; set; }

    public string Email { get; set; } = "";

    public string Contrasena { get; set; } = "";

    public string TipoUsuario { get; set; } = "Usuario";

    public bool Activo { get; set; } = true;
}
