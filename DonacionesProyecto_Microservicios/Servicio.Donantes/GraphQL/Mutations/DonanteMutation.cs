using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class DonanteMutation
{
    public OperationResponse InsertarDonante(
        DonanteInput donante,
        [Service] DonanteService service)
    {
        return service.InsertarDonante(donante);
    }

    public OperationResponse ActualizarDonante(
        DonanteUpdateInput donante,
        [Service] DonanteService service)
    {
        return service.ActualizarDonante(donante);
    }

    public OperationResponse EliminarDonante(
        int idDonante,
        [Service] DonanteService service)
    {
        return service.EliminarDonante(idDonante);
    }
}