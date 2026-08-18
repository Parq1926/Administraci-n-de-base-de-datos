using DonacionesProyecto.Models;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class DonacionQuery
{
    public List<Donacion> ObtenerDonaciones(
        [Service] DonacionService service)
    {
        return service.ObtenerDonaciones();
    }
}