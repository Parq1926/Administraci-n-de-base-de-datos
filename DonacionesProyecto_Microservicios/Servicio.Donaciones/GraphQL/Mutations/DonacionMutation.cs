using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class DonacionMutation
{
    public OperationResponse InsertarDonacion(
        DonacionInput donacion,
        [Service] DonacionService service)
    {
        return service.InsertarDonacion(donacion);
    }

    public OperationResponse ActualizarDonacion(
        DonacionUpdateInput donacion,
        [Service] DonacionService service)
    {
        return service.ActualizarDonacion(donacion);
    }

    public OperationResponse EliminarDonacion(
        int idDonacion,
        [Service] DonacionService service)
    {
        return service.EliminarDonacion(idDonacion);
    }
}