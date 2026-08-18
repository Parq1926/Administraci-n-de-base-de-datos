using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class FundacionPublicApiService
{
    private readonly HttpClient _http;

    public FundacionPublicApiService(HttpClient http)
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
}
