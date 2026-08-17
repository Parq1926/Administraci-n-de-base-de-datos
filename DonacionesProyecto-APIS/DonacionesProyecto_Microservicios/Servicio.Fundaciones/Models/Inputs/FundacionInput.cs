namespace DonacionesProyecto.Models.Inputs;

public class FundacionInput
{
    public string Nombre { get; set; } = "";

    public string? Descripcion { get; set; }

    public string? Identificacion { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public bool Activo { get; set; } = true;
}