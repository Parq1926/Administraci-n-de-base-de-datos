namespace Servicio.Alertas.Models;

public class Alerta
{
    public int IdAlerta { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Mensaje { get; set; }
    public string? TipoAlerta { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? FechaLectura { get; set; }
    public bool? Leida { get; set; }
    public int? IdUsuario { get; set; }
    public string? UrlRelacionada { get; set; }
}

public class MensajeResponse
{
    public string? Mensaje { get; set; }
    public int? Id { get; set; }
    public bool Exitoso { get; set; }
    public string? CodigoError { get; set; }
}