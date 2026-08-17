namespace DonacionesProyecto.Responses;

public class SaldoCodigoResponse
{
    public int IdCodigo { get; set; }

    public string NombreCodigo { get; set; } = "";

    public int IdFundacion { get; set; }

    public string? NombreFundacion { get; set; }

    public bool? PermiteRedistribucion { get; set; }

    public bool? Estado { get; set; }

    public decimal SaldoDisponible { get; set; }
}
