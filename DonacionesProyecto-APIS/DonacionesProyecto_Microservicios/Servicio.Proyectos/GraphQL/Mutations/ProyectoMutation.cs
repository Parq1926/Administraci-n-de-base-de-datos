using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class ProyectoMutation
{
    public OperationResponse InsertarProyecto(
        ProyectoInput proyecto,
        [Service] ProyectoService service)
    {
        return service.InsertarProyecto(proyecto);
    }

    public OperationResponse ActualizarProyecto(
        ProyectoUpdateInput proyecto,
        [Service] ProyectoService service)
    {
        return service.ActualizarProyecto(proyecto);
    }

    public OperationResponse EliminarProyecto(
        int idProyecto,
        [Service] ProyectoService service)
    {
        return service.EliminarProyecto(idProyecto);
    }
}