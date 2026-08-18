namespace DonacionesProyecto.Models.Inputs;

public class DonacionInput
{
    public decimal Monto { get; set; }

    public DateTime? FechaDonacion { get; set; }

    public string MetodoPago { get; set; } = "";

    public string Estado { get; set; } = "Pendiente";

    public string? Comentario { get; set; }

    public int? IdDonante { get; set; }

    public int? IdProyecto { get; set; }

    public int? IdCodigoDonacion { get; set; }
}