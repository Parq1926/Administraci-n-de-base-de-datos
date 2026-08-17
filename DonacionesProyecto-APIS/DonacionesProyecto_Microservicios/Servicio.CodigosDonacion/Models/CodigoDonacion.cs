namespace DonacionesProyecto.Models;

public class CodigoDonacion
{
    public int IdCodigo { get; set; }

    public int IdFundacion { get; set; }

    public string NombreCodigo { get; set; } = "";

    public string? Descripcion { get; set; }

    public bool? PermiteRedistribucion { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    //JOIN FUNDACION
    public string? FundacionNombre { get; set; }
}
