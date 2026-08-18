using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace SistemaDeDonaciones.Services;

public class AuthService
{
    public ClaimsPrincipal CrearClaimsPrincipal(int idUsuario, string email, string rol, string nombre)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, idUsuario.ToString()),
            new Claim(ClaimTypes.Name, email),
            new Claim(ClaimTypes.Role, rol),
            new Claim("Nombre", nombre)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}