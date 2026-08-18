using DonacionesProyecto.Services;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType("Mutation")]
public class CodigoDonacionMutation
{
    private readonly CodigoDonacionService _service;

    public CodigoDonacionMutation(CodigoDonacionService service)
    {
        _service = service;
    }

    // ✅ INSERTAR
    public OperationResponse InsertarCodigo(CodigoDonacionInput codigo)
    {
        return _service.InsertarCodigo(codigo);
    }

    // ✅ ACTUALIZAR
    public OperationResponse ActualizarCodigo(CodigoDonacionUpdateInput codigo)
    {
        return _service.ActualizarCodigo(codigo);
    }

    // ✅ ELIMINAR
    public OperationResponse EliminarCodigo(int idCodigo)
    {
        return _service.EliminarCodigo(idCodigo);
    }
}