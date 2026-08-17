using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class AlertaApiService
{
    private readonly HttpClient _http;

    public AlertaApiService(HttpClient http)
    {
        _http = http;
    }

    // 📌 QUERY: Obtener todas las alertas
    private class ObtenerAlertasResponse
    {
        public List<Alerta> GetAlertas { get; set; } = new();
    }

    public async Task<List<Alerta>> ObtenerAlertasAsync()
    {
        const string query = """
            query {
              getAlertas {
                idAlerta
                titulo
                mensaje
                tipoAlerta
                fechaCreacion
                fechaLectura
                leida
                idUsuario
                urlRelacionada
              }
            }
            """;

        var result = await _http.GraphQLAsync<ObtenerAlertasResponse>(query);
        return result.GetAlertas;
    }

    // 📌 MUTATION: Insertar alerta
    private class MutationResponse
    {
        public OperationResponse? InsertarAlerta { get; set; }
        public OperationResponse? ActualizarAlerta { get; set; }
        public OperationResponse? EliminarAlerta { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(InsertarAlertaInput input)
    {
        const string mutation = """
            mutation($input: InsertarAlertaInput!) {
              insertarAlerta(input: $input) {
                mensaje
                id
                exitoso
              }
            }
            """;

        var variables = new { input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarAlerta ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    // 📌 MUTATION: Actualizar alerta
    public async Task<OperationResponse> ActualizarAsync(ActualizarAlertaInput input)
    {
        const string mutation = """
            mutation($input: ActualizarAlertaInput!) {
              actualizarAlerta(input: $input) {
                mensaje
                id
                exitoso
              }
            }
            """;

        var variables = new { input };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.ActualizarAlerta ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    // 📌 MUTATION: Marcar alerta como leída
    public async Task<OperationResponse> MarcarLeidaAsync(int idAlerta)
    {
        var input = new ActualizarAlertaInput
        {
            IdAlerta = idAlerta,
            Leida = true,
            FechaLectura = DateTime.Now
        };

        return await ActualizarAsync(input);
    }

    // 📌 MUTATION: Eliminar alerta
    public async Task<OperationResponse> EliminarAsync(int idAlerta)
    {
        const string mutation = """
            mutation($idAlerta: Int!) {
              eliminarAlerta(idAlerta: $idAlerta) {
                mensaje
                id
                exitoso
              }
            }
            """;

        var variables = new { idAlerta };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarAlerta ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}