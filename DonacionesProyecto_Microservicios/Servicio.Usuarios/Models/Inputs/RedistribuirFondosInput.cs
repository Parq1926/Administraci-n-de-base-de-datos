namespace DonacionesProyecto.Models.Inputs;

public class RedistribuirFondosInput
{
    public int IdProyectoOrigen { get; set; }

    public int IdProyectoDestino { get; set; }

    public decimal Monto { get; set; }
}
