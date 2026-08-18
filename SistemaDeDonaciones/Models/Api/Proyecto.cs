namespace SistemaDeDonaciones.Models.Api;

//Servicio.Proyectos
public class Proyecto
{
    public int IdProyecto { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; }
    public decimal? MetaRecaudacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Estado { get; set; }
    public int? IdFundacion { get; set; }
    public string? FundacionNombre { get; set; }
    public bool? Activo { get; set; }
    public decimal Recaudado { get; set; }

    //Solo para vista
    public int PorcentajeAvance =>
        MetaRecaudacion is null or 0 ? 0 : (int)Math.Round(Recaudado / MetaRecaudacion.Value * 100);
}

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

public class ProyectoUpdateInput
{
    public int IdProyecto { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public decimal? MetaRecaudacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Estado { get; set; }
    public int? IdFundacion { get; set; }
    public bool? Activo { get; set; }
}
