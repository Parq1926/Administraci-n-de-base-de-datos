namespace DonacionesProyecto.Models.Inputs;

public class CodigoDonacionUpdateInput
{
    public int IdCodigo { get; set; }

    public int? IdFundacion { get; set; }

    public string? NombreCodigo { get; set; }

    public string? Descripcion { get; set; }

    public bool? PermiteRedistribucion { get; set; }

    public bool? Estado { get; set; }
}
