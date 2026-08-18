using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class ProyectoPublicApiService
{
    private readonly HttpClient _http;

    public ProyectoPublicApiService(HttpClient http)
    {
        _http = http;
    }

    private class ObtenerProyectosResponse
    {
        public List<Proyecto> ObtenerProyectos { get; set; } = new();
    }

    public async Task<List<Proyecto>> ObtenerProyectosAsync()
    {
        const string query = """
            query {
              obtenerProyectos {
                idProyecto
                nombre
                descripcion
                metaRecaudacion
                fechaInicio
                fechaFin
                estado
                idFundacion
                fundacionNombre
                activo
                recaudado
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerProyectosResponse>(query);
        return result.ObtenerProyectos;
    }
}
