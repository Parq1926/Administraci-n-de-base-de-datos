namespace DonacionesProyecto.Models.Inputs;

public class ProyectoInput
{
    public string Nombre { get; set; } = "";

    public string? Descripcion { get; set; }

    public decimal? MetaRecaudacion { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string Estado { get; set; } = "Activo";

    public int? IdFundacion { get; set; }

    public bool Activo { get; set; } = true;
}