using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class HistorialCodigoApiService
{
    private readonly HttpClient _http;

    public HistorialCodigoApiService(HttpClient http)
    {
        _http = http;
    }

    private class ObtenerHistorialResponse
    {
        public List<HistorialCodigo> ObtenerHistorial { get; set; } = new();
    }

    public async Task<List<HistorialCodigo>> ObtenerHistorialAsync(int? idCodigo = null, int? idUsuario = null)
    {
        const string query = """
            query($idCodigo: Int, $idUsuario: Int) {
              obtenerHistorial(idCodigo: $idCodigo, idUsuario: $idUsuario) {
                idHistorial
                idCodigo
                codigoNombre
                accion
                fecha
                idUsuario
                usuarioNombre
                descripcionCambio
              }
            }
            """;

        var variables = new { idCodigo, idUsuario };
        var result = await _http.GraphQLAsync<ObtenerHistorialResponse>(query, variables);
        return result.ObtenerHistorial;
    }

    private class InsertarHistorialResponse
    {
        public OperationResponse? InsertarHistorial { get; set; }
    }

    public async Task<OperationResponse> InsertarHistorialAsync(HistorialCodigoInput input)
    {
        const string mutation = """
            mutation($historial: HistorialCodigoInput!) {
              insertarHistorial(historial: $historial) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { historial = input };
        var result = await _http.GraphQLAsync<InsertarHistorialResponse>(mutation, variables);
        return result.InsertarHistorial ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}
