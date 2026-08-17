using Dapper;
using Servicio.Movimientos.Exceptions;
using Servicio.Movimientos.Models;
using System.Data;

namespace Servicio.Movimientos.Data.Repositories
{
    public class MovimientoRepository : BaseRepository, IMovimientoRepository
    {
        private readonly ILogger<MovimientoRepository> _logger;

        public MovimientoRepository(
            ConnectionManager connectionManager,
            ILogger<MovimientoRepository> logger)
            : base(connectionManager)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Movimiento>> LeerMovimientosAsync(
            int? idMovimiento = null,
            int? idDonacion = null,
            int? idProyecto = null)
        {
            try
            {
                var parameters = new
                {
                    IdMovimiento = idMovimiento,
                    IdDonacion = idDonacion,
                    IdProyecto = idProyecto
                };

                var result = await ExecuteStoredProcedureAsync<Movimiento>("sp_LeerMovimientos", parameters);

                if (idMovimiento.HasValue && !result.Any())
                {
                    throw MovimientoException.NotFound(idMovimiento.Value);
                }

                return result;
            }
            catch (MovimientoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al leer movimientos");
                throw new MovimientoException(
                    "Error al consultar los movimientos",
                    ex,
                    "READ_ERROR",
                    500
                );
            }
        }

        public async Task<MensajeResponse> InsertarMovimientoAsync(Movimiento movimiento)
        {
            try
            {
                await ValidarMovimientoAsync(movimiento);

                var parameters = new DynamicParameters();
                parameters.Add("@TipoMovimiento", movimiento.TipoMovimiento);
                parameters.Add("@Monto", movimiento.Monto);
                parameters.Add("@Descripcion", movimiento.Descripcion);
                parameters.Add("@FechaMovimiento", movimiento.FechaMovimiento ?? DateTime.Now);
                parameters.Add("@IdDonacion", movimiento.IdDonacion);
                parameters.Add("@IdProyecto", movimiento.IdProyecto);
                parameters.Add("@IdUsuario", movimiento.IdUsuario);
                parameters.Add("@SaldoAnterior", movimiento.SaldoAnterior);
                parameters.Add("@SaldoPosterior", movimiento.SaldoPosterior);
                parameters.Add("@Comprobante", movimiento.Comprobante);
                parameters.Add("@IdMovimiento", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("@Mensaje", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await ExecuteStoredProcedureWithOutputAsync("sp_InsertarMovimiento", parameters);

                var mensaje = result["Mensaje"] as string ?? "Movimiento insertado correctamente";
                var id = result["IdMovimiento"] as int?;

                if (mensaje.ToLower().Contains("error"))
                {
                    throw new MovimientoException(mensaje, "SP_ERROR", 400);
                }

                return new MensajeResponse
                {
                    Mensaje = mensaje,
                    Id = id,
                    Exitoso = true
                };
            }
            catch (MovimientoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar movimiento");
                throw new MovimientoException(
                    "Error al insertar el movimiento",
                    ex,
                    "INSERT_ERROR",
                    500
                );
            }
        }

        public async Task<MensajeResponse> ActualizarMovimientoAsync(Movimiento movimiento)
        {
            try
            {
                var existe = await ExisteMovimientoAsync(movimiento.IdMovimiento);
                if (!existe)
                {
                    throw MovimientoException.NotFound(movimiento.IdMovimiento);
                }

                await ValidarMovimientoAsync(movimiento, false);

                var parameters = new DynamicParameters();
                parameters.Add("@IdMovimiento", movimiento.IdMovimiento);
                parameters.Add("@TipoMovimiento", movimiento.TipoMovimiento);
                parameters.Add("@Monto", movimiento.Monto);
                parameters.Add("@Descripcion", movimiento.Descripcion);
                parameters.Add("@FechaMovimiento", movimiento.FechaMovimiento);
                parameters.Add("@IdDonacion", movimiento.IdDonacion);
                parameters.Add("@IdProyecto", movimiento.IdProyecto);
                parameters.Add("@IdUsuario", movimiento.IdUsuario);
                parameters.Add("@SaldoAnterior", movimiento.SaldoAnterior);
                parameters.Add("@SaldoPosterior", movimiento.SaldoPosterior);
                parameters.Add("@Comprobante", movimiento.Comprobante);
                parameters.Add("@Mensaje", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await ExecuteStoredProcedureWithOutputAsync("sp_ActualizarMovimiento", parameters);

                var mensaje = result["Mensaje"] as string ?? "Movimiento actualizado correctamente";

                if (mensaje.ToLower().Contains("error") || mensaje.ToLower().Contains("no existe"))
                {
                    throw new MovimientoException(mensaje, "SP_ERROR", 400);
                }

                return new MensajeResponse
                {
                    Mensaje = mensaje,
                    Exitoso = true
                };
            }
            catch (MovimientoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar movimiento");
                throw new MovimientoException(
                    "Error al actualizar el movimiento",
                    ex,
                    "UPDATE_ERROR",
                    500
                );
            }
        }

        public async Task<MensajeResponse> EliminarMovimientoAsync(int idMovimiento)
        {
            try
            {
                var existe = await ExisteMovimientoAsync(idMovimiento);
                if (!existe)
                {
                    throw MovimientoException.NotFound(idMovimiento);
                }

                var parameters = new DynamicParameters();
                parameters.Add("@IdMovimiento", idMovimiento);
                parameters.Add("@Mensaje", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                var result = await ExecuteStoredProcedureWithOutputAsync("sp_EliminarMovimiento", parameters);

                var mensaje = result["Mensaje"] as string ?? "Movimiento eliminado correctamente";

                if (mensaje.ToLower().Contains("error"))
                {
                    throw new MovimientoException(mensaje, "SP_ERROR", 400);
                }

                return new MensajeResponse
                {
                    Mensaje = mensaje,
                    Exitoso = true
                };
            }
            catch (MovimientoException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar movimiento");
                throw new MovimientoException(
                    "Error al eliminar el movimiento",
                    ex,
                    "DELETE_ERROR",
                    500
                );
            }
        }

        private async Task ValidarMovimientoAsync(Movimiento movimiento, bool esNuevo = true)
        {
            var errores = new List<string>();

            if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento))
            {
                errores.Add("El tipo de movimiento es requerido");
            }

            if (movimiento.Monto <= 0)
            {
                errores.Add($"El monto {movimiento.Monto} no es válido. Debe ser mayor a 0");
            }

            if (!movimiento.IdDonacion.HasValue && !movimiento.IdProyecto.HasValue)
            {
                errores.Add("El movimiento debe estar asociado a una donación o un proyecto");
            }

            if (errores.Any())
            {
                var mensajeError = string.Join(" | ", errores);
                throw new MovimientoException(mensajeError, "VALIDATION_ERROR", 400);
            }
        }

        private async Task<bool> ExisteMovimientoAsync(int idMovimiento)
        {
            try
            {
                var result = await ExecuteStoredProcedureAsync<Movimiento>(
                    "sp_LeerMovimientos",
                    new { IdMovimiento = idMovimiento });

                return result.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del movimiento");
                return false;
            }
        }
    }
}