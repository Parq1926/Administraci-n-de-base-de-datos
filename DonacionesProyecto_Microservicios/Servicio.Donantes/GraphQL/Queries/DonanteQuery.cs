using DonacionesProyecto.Models;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class DonanteQuery
{
    public List<Donante> ObtenerDonantes(
        [Service] DonanteService service)
    {
        return service.ObtenerDonantes();
    }
}