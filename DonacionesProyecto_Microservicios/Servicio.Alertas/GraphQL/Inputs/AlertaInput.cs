namespace Servicio.Alertas.GraphQL.Inputs;

public class AlertaInput
{
    public string Titulo { get; set; } = string.Empty;
    public string? Mensaje { get; set; }
    public string? TipoAlerta { get; set; }
    public DateTime? FechaLectura { get; set; }
    public bool? Leida { get; set; }
    public int? IdUsuario { get; set; }
    public string? UrlRelacionada { get; set; }
}

public class ActualizarAlertaInput
{
    public int IdAlerta { get; set; }
    public string? Titulo { get; set; }
    public string? Mensaje { get; set; }
    public string? TipoAlerta { get; set; }
    public DateTime? FechaLectura { get; set; }
    public bool? Leida { get; set; }
    public int? IdUsuario { get; set; }
    public string? UrlRelacionada { get; set; }
}