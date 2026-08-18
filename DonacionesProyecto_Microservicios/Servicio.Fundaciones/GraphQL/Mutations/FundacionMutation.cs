using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class FundacionMutation
{
    //INSERTAR
    public OperationResponse InsertarFundacion(
        FundacionInput fundacion,
        [Service] FundacionService service)
    {
        return service.InsertarFundacion(fundacion);
    }

    //ACTUALIZAR
    public OperationResponse ActualizarFundacion(
        FundacionUpdateInput fundacion,
        [Service] FundacionService service)
    {
        return service.ActualizarFundacion(fundacion);
    }

    //ELIMINAR
    public OperationResponse EliminarFundacion(
        int idFundacion,
        [Service] FundacionService service)
    {
        return service.EliminarFundacion(idFundacion);
    }
}