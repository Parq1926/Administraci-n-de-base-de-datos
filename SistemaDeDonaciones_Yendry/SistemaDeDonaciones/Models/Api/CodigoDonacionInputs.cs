namespace SistemaDeDonaciones.Models.Api;

public class CodigoDonacionInput
{
    public int IdFundacion { get; set; }
    public string NombreCodigo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool? PermiteRedistribucion { get; set; }
    public bool? Estado { get; set; }
}

public class CodigoDonacionUpdateInput
{
    public int IdCodigo { get; set; }
    public int? IdFundacion { get; set; }
    public string? NombreCodigo { get; set; }
    public string? Descripcion { get; set; }
    public bool? PermiteRedistribucion { get; set; }
    public bool? Estado { get; set; }
}