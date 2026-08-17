using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SistemaDeDonaciones.Services;

public class AuthService
{
    // Usuarios de prueba (solo para desarrollo)
    private readonly Dictionary<string, (string Password, string Rol, string Nombre)> _usuariosPrueba = new()
    {
        ["admin@donaciones.com"] = ("Admin123", "Empleado", "Administrador"),
        ["empleado@donaciones.com"] = ("Empleado123", "Empleado", "Empleado"),
        ["cliente@donaciones.com"] = ("Cliente123", "Cliente", "Cliente")
    };

    public bool ValidarLogin(string email, string password, out string rol, out string nombre)
    {
        rol = string.Empty;
        nombre = string.Empty;

        if (_usuariosPrueba.TryGetValue(email, out var usuario))
        {
            if (usuario.Password == password)
            {
                rol = usuario.Rol;
                nombre = usuario.Nombre;
                return true;
            }
        }

        return false;
    }

    public ClaimsPrincipal CrearClaimsPrincipal(string email, string rol, string nombre)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, rol),
            new Claim("Nombre", nombre)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}