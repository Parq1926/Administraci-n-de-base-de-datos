using Servicio.Alertas.Data.Repositories;
using Servicio.Alertas.Models;
using HotChocolate;

namespace Servicio.Alertas.GraphQL.Mutations;

public class AlertaMutation
{
    private readonly ILogger<AlertaMutation> _logger;

    public AlertaMutation(ILogger<AlertaMutation> logger)
    {
        _logger = logger;
    }

    // 📌 INSERTAR - Usar InsertarAlertaInput
    public async Task<MensajeResponse> insertarAlerta(
        [Service] IAlertaRepository repository,
        InsertarAlertaInput input)
    {
        try
        {
            _logger.LogInformation("Insertando alerta - Título: {Titulo}", input.Titulo);

            var alerta = new Alerta
            {
                Titulo = input.Titulo,
                Mensaje = input.Mensaje,
                TipoAlerta = input.TipoAlerta,
                FechaLectura = input.FechaLectura,
                Leida = input.Leida,
                IdUsuario = input.IdUsuario,
                UrlRelacionada = input.UrlRelacionada
            };

            return await repository.InsertarAlertaAsync(alerta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en insertarAlerta");
            return new MensajeResponse
            {
                Mensaje = $"Error: {ex.Message}",
                Exitoso = false,
                Id = 0,
                CodigoError = "EXCEPTION_ERROR"
            };
        }
    }

    // 📌 ACTUALIZAR - Usar ActualizarAlertaInput
    public async Task<MensajeResponse> actualizarAlerta(
        [Service] IAlertaRepository repository,
        ActualizarAlertaInput input)
    {
        try
        {
            _logger.LogInformation("Actualizando alerta ID: {Id}", input.IdAlerta);

            var alerta = new Alerta
            {
                IdAlerta = input.IdAlerta,
                Titulo = input.Titulo ?? string.Empty,
                Mensaje = input.Mensaje,
                TipoAlerta = input.TipoAlerta,
                FechaLectura = input.FechaLectura,
                Leida = input.Leida,
                IdUsuario = input.IdUsuario,
                UrlRelacionada = input.UrlRelacionada
            };

            return await repository.ActualizarAlertaAsync(alerta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en actualizarAlerta");
            return new MensajeResponse
            {
                Mensaje = $"Error: {ex.Message}",
                Exitoso = false,
                Id = 0,
                CodigoError = "EXCEPTION_ERROR"
            };
        }
    }

    // 📌 ELIMINAR
    public async Task<MensajeResponse> eliminarAlerta(
        [Service] IAlertaRepository repository,
        int idAlerta)
    {
        try
        {
            _logger.LogInformation("Eliminando alerta ID: {Id}", idAlerta);

            return await repository.EliminarAlertaAsync(idAlerta);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en eliminarAlerta");
            return new MensajeResponse
            {
                Mensaje = $"Error: {ex.Message}",
                Exitoso = false,
                Id = 0,
                CodigoError = "EXCEPTION_ERROR"
            };
        }
    }
}