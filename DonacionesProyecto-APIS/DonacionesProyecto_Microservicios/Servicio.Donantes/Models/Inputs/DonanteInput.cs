namespace DonacionesProyecto.Models.Inputs;

public class DonanteInput
{
    public string Nombre { get; set; } = "";

    public string? Apellidos { get; set; }

    public string Email { get; set; } = "";

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public string TipoDonante { get; set; } = "Persona";

    public bool Activo { get; set; } = true;
}