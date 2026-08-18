using DonacionesProyecto.Models;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class FundacionQuery
{
    public List<Fundacion> ObtenerFundaciones(
        [Service] FundacionService service)
    {
        return service.ObtenerFundaciones();
    }
}