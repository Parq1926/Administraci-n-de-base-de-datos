using DonacionesProyecto.Models;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class HistorialCodigoQuery
{
    public List<HistorialCodigo> ObtenerHistorial(
        int? idCodigo,
        int? idUsuario,
        [Service] HistorialCodigoService service)
    {
        return service.ObtenerHistorial(idCodigo, idUsuario);
    }
}
