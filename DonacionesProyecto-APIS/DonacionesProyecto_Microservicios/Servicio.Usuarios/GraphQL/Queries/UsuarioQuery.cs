using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using DonacionesProyecto.Services;
using HotChocolate;

namespace DonacionesProyecto.GraphQL.Queries;

[ExtendObjectType(typeof(Query))]
public class UsuarioQuery
{
    public List<Usuario> ObtenerUsuarios(
        [Service] UsuarioService service)
    {
        return service.ObtenerUsuarios();
    }

    public LoginResponse Login(
        LoginInput login,
        [Service] UsuarioService service)
    {
        return service.Login(login);
    }

    public SaldoResponse ConsultarSaldoPorCodigo(
        string codigo,
        [Service] UsuarioService service)
    {
        return service.ConsultarSaldoPorCodigo(codigo);
    }
}
