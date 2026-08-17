using DonacionesProyecto.Services;
using DonacionesProyecto.Models;
using DonacionesProyecto.Responses;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType("Query")]
public class CodigoDonacionQuery
{
    private readonly CodigoDonacionService _service;

    public CodigoDonacionQuery(CodigoDonacionService service)
    {
        _service = service;
    }

    // ✅ EL NOMBRE DE LA QUERY ES "codigos"
    public List<CodigoDonacion> GetCodigos()
    {
        return _service.ObtenerCodigos();
    }

    // Opcional: Consultar saldo por código
    public List<SaldoCodigoResponse> GetSaldoPorCodigo(int? idFundacion, int? idCodigo)
    {
        return _service.ConsultarSaldoPorCodigo(idFundacion, idCodigo);
    }
}