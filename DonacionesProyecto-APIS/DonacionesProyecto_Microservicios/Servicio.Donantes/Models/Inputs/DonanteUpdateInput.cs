namespace DonacionesProyecto.Models.Inputs;

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