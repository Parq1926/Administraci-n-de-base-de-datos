using Servicio.Movimientos.Models;  // ← IMPORTANTE

namespace Servicio.Movimientos.Data.Repositories
{
    public interface IMovimientoRepository
    {
        Task<IEnumerable<Movimiento>> LeerMovimientosAsync(
            int? idMovimiento = null,
            int? idDonacion = null,
            int? idProyecto = null);

        Task<MensajeResponse> InsertarMovimientoAsync(Movimiento movimiento);
        Task<MensajeResponse> ActualizarMovimientoAsync(Movimiento movimiento);
        Task<MensajeResponse> EliminarMovimientoAsync(int idMovimiento);
    }
}