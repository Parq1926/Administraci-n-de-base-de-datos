namespace SistemaDeDonaciones.Models.Api;

//Servicio.Donantes
public class Donante
{
    public int IdDonante { get; set; }
    public string Nombre { get; set; } = "";
    public string? Apellidos { get; set; }
    public string Email { get; set; } = "";
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? TipoDonante { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public bool? Activo { get; set; }
    public string? NombreCompleto { get; set; }
}

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

public class DonanteUpdateInput
{
    public int IdDonante { get; set; }
    public string? Nombre { get; set; }
    public string? Apellidos { get; set; }
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? TipoDonante { get; set; }
    public bool? Activo { get; set; }
}
