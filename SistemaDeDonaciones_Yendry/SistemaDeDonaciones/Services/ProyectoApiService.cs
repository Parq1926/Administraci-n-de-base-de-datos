using SistemaDeDonaciones.Models.Api;

namespace SistemaDeDonaciones.Services;

public class ProyectoApiService
{
    private readonly HttpClient _http;

    public ProyectoApiService(HttpClient http)
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

    private class MutationResponse
    {
        public OperationResponse? InsertarProyecto { get; set; }
        public OperationResponse? ActualizarProyecto { get; set; }
        public OperationResponse? EliminarProyecto { get; set; }
    }

    public async Task<OperationResponse> InsertarAsync(ProyectoInput input)
    {
        const string mutation = """
            mutation($proyecto: ProyectoInput!) {
              insertarProyecto(proyecto: $proyecto) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { proyecto = input };

        if (input.FechaInicio.HasValue)
            input.FechaInicio = DateTime.SpecifyKind(input.FechaInicio.Value, DateTimeKind.Utc);

        if (input.FechaFin.HasValue)
            input.FechaFin = DateTime.SpecifyKind(input.FechaFin.Value, DateTimeKind.Utc);
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.InsertarProyecto ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }

    public async Task<OperationResponse> ActualizarAsync(ProyectoUpdateInput input)
    {
        const string mutation = """
        mutation($proyecto: ProyectoUpdateInput!) {
          actualizarProyecto(proyecto: $proyecto) {
            exito
            id
            mensaje
          }
        }
        """;

        var variables = new
        {
            proyecto = input
        };

        if (input.FechaInicio.HasValue)
            input.FechaInicio = DateTime.SpecifyKind(input.FechaInicio.Value, DateTimeKind.Utc);

        if (input.FechaFin.HasValue)
            input.FechaFin = DateTime.SpecifyKind(input.FechaFin.Value, DateTimeKind.Utc);

        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);

        return result.ActualizarProyecto ??
               new OperationResponse
               {
                   Exito = false,
                   Mensaje = "Sin respuesta del servicio."
               };
    }

    public async Task<OperationResponse> EliminarAsync(int idProyecto)
    {
        const string mutation = """
            mutation($idProyecto: Int!) {
              eliminarProyecto(idProyecto: $idProyecto) {
                exito
                id
                mensaje
              }
            }
            """;

        var variables = new { idProyecto };
        var result = await _http.GraphQLAsync<MutationResponse>(mutation, variables);
        return result.EliminarProyecto ?? new OperationResponse { Exito = false, Mensaje = "Sin respuesta del servicio." };
    }
}
