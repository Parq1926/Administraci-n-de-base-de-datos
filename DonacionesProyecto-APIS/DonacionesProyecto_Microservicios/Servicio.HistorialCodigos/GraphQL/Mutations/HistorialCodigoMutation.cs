using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class HistorialCodigoMutation
{
    public OperationResponse InsertarHistorial(
        HistorialCodigoInput historial,
        [Service] HistorialCodigoService service)
    {
        return service.InsertarHistorial(historial);
    }
}
