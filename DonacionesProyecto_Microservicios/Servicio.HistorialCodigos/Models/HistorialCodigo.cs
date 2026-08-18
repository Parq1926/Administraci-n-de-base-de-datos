namespace DonacionesProyecto.Models;

public class HistorialCodigo
{
    public int IdHistorial { get; set; }

    public int IdCodigo { get; set; }

    public string Accion { get; set; } = "";

    public DateTime? Fecha { get; set; }

    public int IdUsuario { get; set; }

    public string? DescripcionCambio { get; set; }

    //JOIN CODIGO DE DONACION
    public string? CodigoNombre { get; set; }

    //JOIN USUARIO
    public string? UsuarioNombre { get; set; }
}
