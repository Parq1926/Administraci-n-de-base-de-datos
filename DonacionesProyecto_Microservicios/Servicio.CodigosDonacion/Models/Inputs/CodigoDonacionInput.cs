namespace DonacionesProyecto.Models.Inputs;

public class CodigoDonacionInput
{
    public int IdFundacion { get; set; }

    public string NombreCodigo { get; set; } = "";

    public string? Descripcion { get; set; }

    public bool PermiteRedistribucion { get; set; } = true;

    public bool Estado { get; set; } = true;
}
