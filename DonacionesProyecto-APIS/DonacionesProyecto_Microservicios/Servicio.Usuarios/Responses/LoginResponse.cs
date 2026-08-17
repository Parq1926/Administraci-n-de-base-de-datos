namespace DonacionesProyecto.Responses;

public class LoginResponse
{
    public bool Exitoso { get; set; }

    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = "";

    public string TipoUsuario { get; set; } = "";

    public string Mensaje { get; set; } = "";
}
