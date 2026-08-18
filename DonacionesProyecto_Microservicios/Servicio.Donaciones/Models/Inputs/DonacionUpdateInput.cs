namespace DonacionesProyecto.Models.Inputs;

public class DonacionUpdateInput
{
    public int IdDonacion { get; set; }

    public decimal? Monto { get; set; }

    public string? MetodoPago { get; set; }

    public string? Estado { get; set; }

    public string? Comentario { get; set; }

    public int? IdDonante { get; set; }

    public int? IdProyecto { get; set; }

    public int? IdCodigoDonacion { get; set; }
}