using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class UsuarioMutation
{
    public OperationResponse InsertarUsuario(
        UsuarioInput usuario,
        [Service] UsuarioService service)
    {
        return service.InsertarUsuario(usuario);
    }

    public OperationResponse ActualizarUsuario(
        UsuarioUpdateInput usuario,
        [Service] UsuarioService service)
    {
        return service.ActualizarUsuario(usuario);
    }

    public OperationResponse EliminarUsuario(
        int idUsuario,
        [Service] UsuarioService service)
    {
        return service.EliminarUsuario(idUsuario);
    }

    public OperationResponse AsignarDonacionAProyecto(
        int idDonacion,
        int idProyecto,
        [Service] UsuarioService service)
    {
        return service.AsignarDonacionAProyecto(idDonacion, idProyecto);
    }

    public OperationResponse RedistribuirFondos(
        RedistribuirFondosInput redistribucion,
        [Service] UsuarioService service)
    {
        return service.RedistribuirFondos(redistribucion);
    }
}
