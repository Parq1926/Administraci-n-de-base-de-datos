using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class FundacionApiService
{
    private readonly HttpClient _http;

    public FundacionApiService(HttpClient http)
    {
        _http = http;
    }

    private class ObtenerFundacionesResponse
    {
        public List<Fundacion> ObtenerFundaciones { get; set; } = new();
    }

    public async Task<List<Fundacion>> ObtenerFundacionesAsync()
    {
        const string query = """
            query {
              obtenerFundaciones {
                idFundacion
                nombre
                descripcion
                identificacion
                email
                telefono
                direccion
                fechaRegistro
                activo
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerFundacionesResponse>(query);
        return result.ObtenerFundaciones;
    }

    private class MutationResponse
    {
        public OperationResponse? InsertarFundacion { get; set; }
        public OperationResponse? ActualizarFundacion { get; set; }
        public OperationResponse? EliminarFundacion { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(FundacionInput input)
    {
        const string mutation = """
            mutation($fundacion: FundacionInput!) {
              insertarFundacion(fundacion: $fundacion) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { fundacion = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarFundacion ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> ActualizarAsync(FundacionUpdateInput input)
    {
        const string mutation = """
            mutation($fundacion: FundacionUpdateInput!) {
              actualizarFundacion(fundacion: $fundacion) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { fundacion = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.ActualizarFundacion ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> EliminarAsync(int idFundacion)
    {
        const string mutation = """
            mutation($idFundacion: Int!) {
              eliminarFundacion(idFundacion: $idFundacion) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { idFundacion };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarFundacion ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}
