using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class CodigoDonacionApiService
{
    private readonly HttpClient _http;

    public CodigoDonacionApiService(HttpClient http)
    {
        _http = http;
    }

    // 📌 QUERY: Obtener todos los códigos
    private class ObtenerCodigosResponse
    {
        // ✅ LA QUERY SE LLAMA "codigos"
        public List<CodigoDonacion> codigos { get; set; } = new();
    }

    public async Task<List<CodigoDonacion>> ObtenerCodigosAsync()
    {
        const string query = """
            query {
              codigos {
                idCodigo
                idFundacion
                fundacionNombre
                nombreCodigo
                descripcion
                permiteRedistribucion
                estado
                fechaCreacion
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerCodigosResponse>(query);
        return result.codigos ?? new List<CodigoDonacion>();
    }

    // 📌 QUERY: Obtener códigos por filtro
    public async Task<List<CodigoDonacion>> ObtenerCodigosFiltradosAsync(int? idFundacion = null, bool? estado = null)
    {
        var filtros = new List<string>();
        if (idFundacion.HasValue) filtros.Add($"idFundacion: {idFundacion.Value}");
        if (estado.HasValue) filtros.Add($"estado: {estado.Value.ToString().ToLower()}");

        var filtroStr = filtros.Any() ? $"({string.Join(", ", filtros)})" : "";

        var query = $@"
query {{
  codigos{filtroStr} {{
    idCodigo
    idFundacion
    fundacionNombre
    nombreCodigo
    descripcion
    permiteRedistribucion
    estado
    fechaCreacion
  }}
}}";

        var result = await _http.GraphQLAsync<ObtenerCodigosResponse>(query);
        return result.codigos ?? new List<CodigoDonacion>();
    }

    // 📌 MUTATION: Insertar código
    private class MutationResponse
    {
        public OperationResponse? insertarCodigo { get; set; }
        public OperationResponse? actualizarCodigo { get; set; }
        public OperationResponse? eliminarCodigo { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(CodigoDonacionInput input)
    {
        const string mutation = """
            mutation($codigo: CodigoDonacionInput!) {
              insertarCodigo(codigo: $codigo) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { codigo = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.insertarCodigo ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    // 📌 MUTATION: Actualizar código
    public async Task<OperationResponse> ActualizarAsync(CodigoDonacionUpdateInput input)
    {
        const string mutation = """
            mutation($codigo: CodigoDonacionUpdateInput!) {
              actualizarCodigo(codigo: $codigo) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { codigo = input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.actualizarCodigo ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    // 📌 MUTATION: Eliminar código
    public async Task<OperationResponse> EliminarAsync(int idCodigo)
    {
        const string mutation = """
            mutation($idCodigo: Int!) {
              eliminarCodigo(idCodigo: $idCodigo) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { idCodigo };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.eliminarCodigo ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}