using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class UsuarioApiService
{
    private readonly HttpClient _http;

    public UsuarioApiService(HttpClient http)
    {
        _http = http;
    }

    private class LoginQueryResponse
    {
        public LoginResponse? Login { get; set; }
    }

    public async Task<LoginResponse> LoginAsync(LoginInput input)
    {
        const string query = """
            query($login: LoginInput!) {
              login(login: $login) {
                exitoso
                idUsuario
                nombre
                tipoUsuario
                mensaje
              }
            }
            """;

        var variables = new { login = input };
        var result = await _http.GraphQLAsync<LoginQueryResponse>(query, variables);
        return result.Login ?? new LoginResponse { Exitoso = false, Mensaje = "Sin respuesta del servicio." };
    }

    private class ObtenerUsuariosResponse
    {
        public List<Usuario> ObtenerUsuarios { get; set; } = new();
    }

    public async Task<List<Usuario>> ObtenerUsuariosAsync()
    {
        const string query = """
            query {
              obtenerUsuarios {
                idUsuario
                nombre
                apellidos
                email
                tipoUsuario
                fechaRegistro
                activo
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerUsuariosResponse>(query);
        return result.ObtenerUsuarios;
    }

    private class MutationResponse
    {
        public OperationResponse? InsertarUsuario { get; set; }
        public OperationResponse? ActualizarUsuario { get; set; }
        public OperationResponse? EliminarUsuario { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(UsuarioInput input)
    {
        const string mutation = """
            mutation($usuario: UsuarioInput!) {
              insertarUsuario(usuario: $usuario) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { usuario = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarUsuario ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> ActualizarAsync(UsuarioUpdateInput input)
    {
        const string mutation = """
            mutation($usuario: UsuarioUpdateInput!) {
              actualizarUsuario(usuario: $usuario) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { usuario = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.ActualizarUsuario ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> EliminarAsync(int idUsuario)
    {
        const string mutation = """
            mutation($idUsuario: Int!) {
              eliminarUsuario(idUsuario: $idUsuario) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { idUsuario };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarUsuario ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}
