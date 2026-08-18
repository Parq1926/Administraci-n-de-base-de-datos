namespace DonacionesProyecto.Services;

public class RolContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public const string HeaderRol = "X-Rol";

    public RolContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string ObtenerRol()
    {
        string? rol = _httpContextAccessor.HttpContext?
            .Request.Headers[HeaderRol]
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(rol) ? "Generico" : rol;
    }
}