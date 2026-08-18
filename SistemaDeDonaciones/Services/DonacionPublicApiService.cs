using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;
public class DonacionPublicApiService
{
    private readonly HttpClient _http;

    public DonacionPublicApiService(HttpClient http)
    {
        _http = http;
    }

    private class ObtenerDonacionesResponse
    {
        public List<Donacion> ObtenerDonaciones { get; set; } = new();
    }

    public async Task<List<Donacion>> ObtenerDonacionesAsync()
    {
        const string query = """
            query {
              obtenerDonaciones {
                idDonacion
                monto
                fechaDonacion
                metodoPago
                estado
                idProyecto
                proyectoNombre
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerDonacionesResponse>(query);
        return result.ObtenerDonaciones;
    }
}
