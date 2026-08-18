using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SistemaDeDonaciones.Services;

//respuesta GraphQL: { "data": y "errors":}
public class GraphQLEnvelope<T>
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<GraphQLError>? Errors { get; set; }
}

public class GraphQLError
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = "";
}

public class GraphQLException : Exception
{
    public GraphQLException(string message) : base(message) { }
}

//Permite enviar consultas a los microservicios y recibir directamente los datos que se necesitan.
public static class GraphQLHttpClientExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<T> GraphQLAsync<T>(this HttpClient client, string query, object? variables = null)
        where T : new()
    {
        var payload = new { query, variables };

        HttpResponseMessage response;
        try
        {
            response = await client.PostAsJsonAsync("graphql", payload);
        }
        catch (HttpRequestException ex)
        {
            throw new GraphQLException(
                $"No se pudo conectar con el servicio ({client.BaseAddress}). " +
                $"Verifique que el microservicio esté en ejecución. Detalle: {ex.Message}");
        }

        var raw = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new GraphQLException(
                $"El servicio respondió {(int)response.StatusCode} ({response.ReasonPhrase}). {raw}");
        }

        GraphQLEnvelope<T>? envelope;
        try
        {
            envelope = JsonSerializer.Deserialize<GraphQLEnvelope<T>>(raw, JsonOptions);
        }
        catch (JsonException ex)
        {
            throw new GraphQLException($"Respuesta inesperada del servicio: {ex.Message}");
        }

        if (envelope?.Errors is { Count: > 0 })
        {
            throw new GraphQLException(string.Join(" | ", envelope.Errors.Select(e => e.Message)));
        }

        return envelope is null || envelope.Data is null ? new T() : envelope.Data;
    }
}
