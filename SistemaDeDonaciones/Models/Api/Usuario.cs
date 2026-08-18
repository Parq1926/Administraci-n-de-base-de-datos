namespace SistemaDeDonaciones.Models.Api;

//Servicio.Usuarios
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

public class UsuarioInput
{
    public string Nombre { get; set; } = "";
    public string? Apellidos { get; set; }
    public string Email { get; set; } = "";
    public string Contrasena { get; set; } = "";
    public string TipoUsuario { get; set; } = "Usuario";
    public bool Activo { get; set; } = true;
}

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

public class LoginInput
{
    public string Email { get; set; } = "";
    public string Contrasena { get; set; } = "";
}

public class LoginResponse
{
    public bool Exitoso { get; set; }
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = "";
    public string TipoUsuario { get; set; } = "";
    public string Mensaje { get; set; } = "";
}
