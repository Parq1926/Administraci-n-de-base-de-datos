using Dapper;
using Servicio.Alertas.Models;
using System.Data;

namespace Servicio.Alertas.Data.Repositories;

public class AlertaRepository : BaseRepository, IAlertaRepository
{
    private readonly ILogger<AlertaRepository> _logger;

    public AlertaRepository(
        ConnectionManager connectionManager,
        ILogger<AlertaRepository> logger)
        : base(connectionManager)
    {
        _logger = logger;
    }

    // ============================================
    // LEER ALERTAS
    // ============================================

    public async Task<IEnumerable<Alerta>> LeerAlertasAsync(
        int? idAlerta = null,
        int? idUsuario = null,
        bool? leida = null)
    {
        try
        {
            _logger.LogInformation("Consultando alertas - ID: {Id}, Usuario: {Usuario}, Leida: {Leida}",
                idAlerta, idUsuario, leida);

            var parameters = new
            {
                IdAlerta = idAlerta,
                IdUsuario = idUsuario,
                Leida = leida
            };

            var result = await ExecuteStoredProcedureAsync<Alerta>("sp_LeerAlertas", parameters);

            if (idAlerta.HasValue && !result.Any())
            {
                _logger.LogWarning("Alerta con ID {Id} no encontrada", idAlerta.Value);
                throw new Exception($"La alerta con ID {idAlerta.Value} no existe");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al leer alertas");
            throw;
        }
    }

    // ============================================
    // INSERTAR ALERTA
    // ============================================

    public async Task<MensajeResponse> InsertarAlertaAsync(Alerta alerta)
    {
        try
        {
            _logger.LogInformation("=== INSERTANDO EN BASE DE DATOS ===");
            _logger.LogInformation("Título: {Titulo}", alerta.Titulo);

            var parameters = new DynamicParameters();
            parameters.Add("@Titulo", alerta.Titulo);
            parameters.Add("@Mensaje", alerta.Mensaje);
            parameters.Add("@TipoAlerta", alerta.TipoAlerta ?? "Info");
            parameters.Add("@IdUsuario", alerta.IdUsuario);
            parameters.Add("@UrlRelacionada", alerta.UrlRelacionada);
            parameters.Add("@IdAlerta", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@MensajeSalida", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            _logger.LogInformation("Ejecutando sp_InsertarAlerta...");

            var result = await ExecuteStoredProcedureWithOutputAsync("sp_InsertarAlerta", parameters, "empleado");

            _logger.LogInformation("Resultado del SP - Keys: {Keys}", string.Join(", ", result.Keys));

            foreach (var key in result.Keys)
            {
                _logger.LogInformation("   {Key} = {Value}", key, result[key]);
            }

            var mensaje = result["MensajeSalida"] as string ?? "Alerta insertada correctamente";
            var id = result["IdAlerta"] as int?;

            _logger.LogInformation("Mensaje: {Mensaje}, ID: {Id}", mensaje, id);

            if (mensaje.ToLower().Contains("error") || id == 0 || id == null)
            {
                _logger.LogError("Error del SP al insertar: {Mensaje}", mensaje);
                return new MensajeResponse
                {
                    Mensaje = mensaje ?? "Error al insertar la alerta",
                    Exitoso = false,
                    Id = 0,
                    CodigoError = "INSERT_ERROR"
                };
            }

            return new MensajeResponse
            {
                Mensaje = mensaje,
                Id = id,
                Exitoso = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ ERROR en InsertarAlertaAsync");
            _logger.LogError("   Message: {Message}", ex.Message);
            _logger.LogError("   StackTrace: {StackTrace}", ex.StackTrace);

            if (ex.InnerException != null)
            {
                _logger.LogError("   Inner Message: {InnerMessage}", ex.InnerException.Message);
            }

            return new MensajeResponse
            {
                Mensaje = $"Error al insertar la alerta: {ex.Message}",
                Exitoso = false,
                Id = 0,
                CodigoError = "EXCEPTION_ERROR"
            };
        }
    }

    // ============================================
    // ACTUALIZAR ALERTA
    // ============================================

    public async Task<MensajeResponse> ActualizarAlertaAsync(Alerta alerta)
    {
        try
        {
            _logger.LogInformation("Actualizando alerta ID: {Id}", alerta.IdAlerta);

            // Verificar que la alerta existe primero
            var existe = await ExisteAlertaAsync(alerta.IdAlerta);
            if (!existe)
            {
                _logger.LogWarning("Alerta ID {Id} no existe", alerta.IdAlerta);
                return new MensajeResponse
                {
                    Mensaje = $"La alerta con ID {alerta.IdAlerta} no existe",
                    Exitoso = false,
                    Id = 0,
                    CodigoError = "NOT_FOUND"
                };
            }

            var parameters = new DynamicParameters();
            parameters.Add("@IdAlerta", alerta.IdAlerta);
            parameters.Add("@Titulo", alerta.Titulo);
            parameters.Add("@Mensaje", alerta.Mensaje);
            parameters.Add("@TipoAlerta", alerta.TipoAlerta);
            parameters.Add("@FechaLectura", alerta.FechaLectura);
            parameters.Add("@Leida", alerta.Leida);
            parameters.Add("@IdUsuario", alerta.IdUsuario);
            parameters.Add("@UrlRelacionada", alerta.UrlRelacionada);
            parameters.Add("@MensajeSalida", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var result = await ExecuteStoredProcedureWithOutputAsync("sp_ActualizarAlerta", parameters, "empleado");

            var mensaje = result["MensajeSalida"] as string ?? "Alerta actualizada correctamente";

            if (mensaje.ToLower().Contains("error") || mensaje.ToLower().Contains("no existe"))
            {
                _logger.LogError("Error del SP al actualizar: {Mensaje}", mensaje);
                return new MensajeResponse
                {
                    Mensaje = mensaje,
                    Exitoso = false,
                    Id = alerta.IdAlerta,
                    CodigoError = "UPDATE_ERROR"
                };
            }

            _logger.LogInformation("Alerta ID {Id} actualizada correctamente", alerta.IdAlerta);

            return new MensajeResponse
            {
                Mensaje = mensaje,
                Id = alerta.IdAlerta,
                Exitoso = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar alerta ID: {Id}", alerta.IdAlerta);
            return new MensajeResponse
            {
                Mensaje = $"Error al actualizar la alerta: {ex.Message}",
                Exitoso = false,
                Id = 0,
                CodigoError = "EXCEPTION_ERROR"
            };
        }
    }

    // ============================================
    // ELIMINAR ALERTA
    // ============================================

    public async Task<MensajeResponse> EliminarAlertaAsync(int idAlerta)
    {
        try
        {
            _logger.LogInformation("Eliminando alerta ID: {Id}", idAlerta);

            // Verificar que la alerta existe primero
            var existe = await ExisteAlertaAsync(idAlerta);
            if (!existe)
            {
                _logger.LogWarning("Alerta ID {Id} no existe", idAlerta);
                return new MensajeResponse
                {
                    Mensaje = $"La alerta con ID {idAlerta} no existe",
                    Exitoso = false,
                    Id = 0,
                    CodigoError = "NOT_FOUND"
                };
            }

            var parameters = new DynamicParameters();
            parameters.Add("@IdAlerta", idAlerta);
            parameters.Add("@MensajeSalida", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

            var result = await ExecuteStoredProcedureWithOutputAsync("sp_EliminarAlerta", parameters, "empleado");

            var mensaje = result["MensajeSalida"] as string ?? "Alerta eliminada correctamente";

            if (mensaje.ToLower().Contains("error") || mensaje.ToLower().Contains("no existe"))
            {
                _logger.LogError("Error del SP al eliminar: {Mensaje}", mensaje);
                return new MensajeResponse
                {
                    Mensaje = mensaje,
                    Exitoso = false,
                    Id = 0,
                    CodigoError = "DELETE_ERROR"
                };
            }

            _logger.LogInformation("Alerta ID {Id} eliminada correctamente", idAlerta);

            return new MensajeResponse
            {
                Mensaje = mensaje,
                Id = idAlerta,
                Exitoso = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar alerta ID: {Id}", idAlerta);
            return new MensajeResponse
            {
                Mensaje = $"Error al eliminar la alerta: {ex.Message}",
                Exitoso = false,
                Id = 0,
                CodigoError = "EXCEPTION_ERROR"
            };
        }
    }

    // ============================================
    // MÉTODO AUXILIAR: VERIFICAR EXISTENCIA
    // ============================================

    private async Task<bool> ExisteAlertaAsync(int idAlerta)
    {
        try
        {
            var result = await ExecuteStoredProcedureAsync<Alerta>(
                "sp_LeerAlertas",
                new { IdAlerta = idAlerta });

            return result.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de alerta ID: {Id}", idAlerta);
            return false;
        }
    }
}