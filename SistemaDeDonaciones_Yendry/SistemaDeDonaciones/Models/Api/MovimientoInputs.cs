namespace SistemaDeDonaciones.Models.Api;

public class MovimientoInput
{
    public string TipoMovimiento { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? FechaMovimiento { get; set; }
    public int? IdDonacion { get; set; }
    public int? IdProyecto { get; set; }
    public int? IdUsuario { get; set; }
    public decimal? SaldoAnterior { get; set; }
    public decimal? SaldoPosterior { get; set; }
    public string? Comprobante { get; set; }
}

public class MovimientoUpdateInput
{
    public int IdMovimiento { get; set; }
    public string? TipoMovimiento { get; set; }
    public decimal? Monto { get; set; }
    public string? Descripcion { get; set; }
    public DateTime? FechaMovimiento { get; set; }
    public int? IdDonacion { get; set; }
    public int? IdProyecto { get; set; }
    public int? IdUsuario { get; set; }
    public decimal? SaldoAnterior { get; set; }
    public decimal? SaldoPosterior { get; set; }
    public string? Comprobante { get; set; }
}