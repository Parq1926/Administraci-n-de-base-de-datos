namespace DonacionesProyecto.Models.Inputs;

public class HistorialCodigoInput
{
    public int IdCodigo { get; set; }

    public string Accion { get; set; } = "";

    public int IdUsuario { get; set; }

    public string? DescripcionCambio { get; set; }
}
