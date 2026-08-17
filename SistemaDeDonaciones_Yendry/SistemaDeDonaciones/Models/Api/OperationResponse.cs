namespace SistemaDeDonaciones.Models.Api;

//OperationResponse de DonacionesProyecto 
public class OperationResponse
{
    public bool Exito { get; set; }
    public int Id { get; set; }
    public string Mensaje { get; set; } = "";
    public string? CodigoError { get; set; }
}
