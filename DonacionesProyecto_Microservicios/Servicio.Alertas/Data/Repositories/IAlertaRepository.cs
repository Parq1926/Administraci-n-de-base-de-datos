using Servicio.Alertas.Models;

namespace Servicio.Alertas.Data.Repositories;

public interface IAlertaRepository
{
    Task<IEnumerable<Alerta>> LeerAlertasAsync(
        int? idAlerta = null,
        int? idUsuario = null,
        bool? leida = null);

    Task<MensajeResponse> InsertarAlertaAsync(Alerta alerta);
    Task<MensajeResponse> ActualizarAlertaAsync(Alerta alerta);
    Task<MensajeResponse> EliminarAlertaAsync(int idAlerta);
}