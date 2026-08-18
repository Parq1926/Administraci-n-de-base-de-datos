using DonacionesProyecto.Models;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class ProyectoQuery
{
    public List<Proyecto> ObtenerProyectos(
        [Service] ProyectoService service)
    {
        return service.ObtenerProyectos();
    }
}