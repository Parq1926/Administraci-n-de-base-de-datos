using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class MovimientoApiService
{
    private readonly HttpClient _http;

    public MovimientoApiService(HttpClient http)
    {
        _http = http;
    }

    // 📌 QUERY: Obtener todos los movimientos
    private class ObtenerMovimientosResponse
    {
        public List<Movimiento> GetMovimientos { get; set; } = new();
    }

    public async Task<List<Movimiento>> ObtenerMovimientosAsync()
    {
        const string query = """
            query {
              getMovimientos {
                idMovimiento
                tipoMovimiento
                monto
                descripcion
                fechaMovimiento
                idDonacion
                idProyecto
                idUsuario
                saldoAnterior
                saldoPosterior
                comprobante
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerMovimientosResponse>(query);
        return result.GetMovimientos;
    }

    // 📌 QUERY: Obtener movimientos por filtro
    public async Task<List<Movimiento>> ObtenerMovimientosFiltradosAsync(int? idDonacion = null, int? idProyecto = null)
    {
        var filtros = new List<string>();
        if (idDonacion.HasValue) filtros.Add($"idDonacion: {idDonacion.Value}");
        if (idProyecto.HasValue) filtros.Add($"idProyecto: {idProyecto.Value}");

        var filtroStr = filtros.Any() ? $"({string.Join(", ", filtros)})" : "";

        // ✅ CORREGIDO: Usar $@ en lugar de $"""
        var query = $@"
query {{
  getMovimientos{filtroStr} {{
    idMovimiento
    tipoMovimiento
    monto
    descripcion
    fechaMovimiento
    idDonacion
    idProyecto
    idUsuario
    saldoAnterior
    saldoPosterior
    comprobante
  }}
}}";

        var result = await _http.GraphQLAsync<ObtenerMovimientosResponse>(query);
        return result.GetMovimientos;
    }

    // 📌 MUTATION: Insertar movimiento
    private class MutationResponse
    {
        public OperationResponse? InsertarMovimiento { get; set; }
        public OperationResponse? ActualizarMovimiento { get; set; }
        public OperationResponse? EliminarMovimiento { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(MovimientoInput input)
    {
        const string mutation = """
            mutation($input: MovimientoInput!) {
              insertarMovimiento(input: $input) {
                mensaje
                id
                exitoso
              }
            }
            """;

        var variables = new { input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarMovimiento ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    // 📌 MUTATION: Actualizar movimiento
    public async Task<OperationResponse> ActualizarAsync(MovimientoUpdateInput input)
    {
        const string mutation = """
            mutation($input: ActualizarMovimientoInput!) {
              actualizarMovimiento(input: $input) {
                mensaje
                id
                exitoso
              }
            }
            """;

        var variables = new { input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.ActualizarMovimiento ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    // 📌 MUTATION: Eliminar movimiento
    public async Task<OperationResponse> EliminarAsync(int idMovimiento)
    {
        const string mutation = """
            mutation($idMovimiento: Int!) {
              eliminarMovimiento(idMovimiento: $idMovimiento) {
                mensaje
                id
                exitoso
              }
            }
            """;

        var variables = new { idMovimiento };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarMovimiento ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}