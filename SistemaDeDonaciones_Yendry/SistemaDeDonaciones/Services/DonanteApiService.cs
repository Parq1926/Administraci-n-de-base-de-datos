using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class DonanteApiService
{
    private readonly HttpClient _http;

    public DonanteApiService(HttpClient http)
    {
        _http = http;
    }

    private class ObtenerDonantesResponse
    {
        public List<Donante> ObtenerDonantes { get; set; } = new();
    }

    public async Task<List<Donante>> ObtenerDonantesAsync()
    {
        const string query = """
            query {
              obtenerDonantes {
                idDonante
                nombre
                apellidos
                email
                telefono
                direccion
                tipoDonante
                fechaRegistro
                activo
                nombreCompleto
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerDonantesResponse>(query);
        return result.ObtenerDonantes;
    }

    private class MutationResponse
    {
        public OperationResponse? InsertarDonante { get; set; }
        public OperationResponse? ActualizarDonante { get; set; }
        public OperationResponse? EliminarDonante { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(DonanteInput input)
    {
        const string mutation = """
            mutation($donante: DonanteInput!) {
              insertarDonante(donante: $donante) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { donante = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarDonante ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> ActualizarAsync(DonanteUpdateInput input)
    {
        const string mutation = """
            mutation($donante: DonanteUpdateInput!) {
              actualizarDonante(donante: $donante) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { donante = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.ActualizarDonante ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> EliminarAsync(int idDonante)
    {
        const string mutation = """
            mutation($idDonante: Int!) {
              eliminarDonante(idDonante: $idDonante) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { idDonante };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarDonante ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}
