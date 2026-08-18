namespace SistemaDeDonaciones.Models.Api;

//Servicio.HistorialCodigos
public class HistorialCodigo
{
    public int IdHistorial { get; set; }
    public int IdCodigo { get; set; }
    public string? CodigoNombre { get; set; }
    public string Accion { get; set; } = "";
    public DateTime? Fecha { get; set; }
    public int IdUsuario { get; set; }
    public string? UsuarioNombre { get; set; }
    public string? DescripcionCambio { get; set; }
}

public class HistorialCodigoInput
{
    public int IdCodigo { get; set; }
    public string Accion { get; set; } = "";
    public int IdUsuario { get; set; }
    public string? DescripcionCambio { get; set; }
}
