using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class DonacionApiService
{
    private readonly HttpClient _http;

    public DonacionApiService(HttpClient http)
    {
        _http = http;
    }

    private const string CamposDonacion = """
        idDonacion monto fechaDonacion metodoPago estado comentario idCodigoDonacion codigoDonacionNombre
        idDonante donanteNombre donanteApellidos donanteEmail idProyecto
        proyectoNombre proyectoEstado
        """;

    private class ObtenerDonacionesResponse
    {
        public List<Donacion> ObtenerDonaciones { get; set; } = new();
    }

    public async Task<List<Donacion>> ObtenerDonacionesAsync()
    {
        var query = $$"""
            query {
              obtenerDonaciones {
                {{CamposDonacion}}
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerDonacionesResponse>(query);
        return result.ObtenerDonaciones;
    }

    private class MutationResponse
    {
        public OperationResponse? InsertarDonacion { get; set; }
        public OperationResponse? ActualizarDonacion { get; set; }
        public OperationResponse? EliminarDonacion { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(DonacionInput input)
    {
        const string mutation = """
            mutation($donacion: DonacionInput!) {
              insertarDonacion(donacion: $donacion) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { donacion = input };

        if (input.FechaDonacion.HasValue)
            input.FechaDonacion = DateTime.SpecifyKind(input.FechaDonacion.Value, DateTimeKind.Utc);

        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarDonacion ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> ActualizarAsync(DonacionUpdateInput input)
    {
        const string mutation = """
            mutation($donacion: DonacionUpdateInput!) {
              actualizarDonacion(donacion: $donacion) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { donacion = input };

        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.ActualizarDonacion ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> EliminarAsync(int idDonacion)
    {
        const string mutation = """
            mutation($idDonacion: Int!) {
              eliminarDonacion(idDonacion: $idDonacion) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { idDonacion };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarDonacion ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}
