using Servicio.Movimientos.Data.Repositories;
using Servicio.Movimientos.GraphQL.Inputs;
using Servicio.Movimientos.Models;
using HotChocolate;
using Servicio.Movimientos.Exceptions;

// ❌ ELIMINAR ESTO: [ExtendObjectType("Mutation")]
// ✅ NO usar ningún atributo
namespace Servicio.Movimientos.GraphQL.Mutations;

public class MovimientoMutation
{
    public async Task<MensajeResponse> insertarMovimiento(
        [Service] IMovimientoRepository repository,
        MovimientoInput input)
    {
        try
        {
            var movimiento = new Movimiento
            {
                TipoMovimiento = input.TipoMovimiento,
                Monto = input.Monto,
                Descripcion = input.Descripcion,
                FechaMovimiento = input.FechaMovimiento,
                IdDonacion = input.IdDonacion,
                IdProyecto = input.IdProyecto,
                IdUsuario = input.IdUsuario,
                SaldoAnterior = input.SaldoAnterior,
                SaldoPosterior = input.SaldoPosterior,
                Comprobante = input.Comprobante
            };

            return await repository.InsertarMovimientoAsync(movimiento);
        }
        catch (MovimientoException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new MovimientoException(
                "Error al insertar el movimiento",
                ex,
                "MUTATION_ERROR",
                500
            );
        }
    }

    public async Task<MensajeResponse> actualizarMovimiento(
        [Service] IMovimientoRepository repository,
        ActualizarMovimientoInput input)
    {
        try
        {
            var movimiento = new Movimiento
            {
                IdMovimiento = input.IdMovimiento,
                TipoMovimiento = input.TipoMovimiento ?? string.Empty,
                Monto = input.Monto ?? 0,
                Descripcion = input.Descripcion,
                FechaMovimiento = input.FechaMovimiento,
                IdDonacion = input.IdDonacion,
                IdProyecto = input.IdProyecto,
                IdUsuario = input.IdUsuario,
                SaldoAnterior = input.SaldoAnterior,
                SaldoPosterior = input.SaldoPosterior,
                Comprobante = input.Comprobante
            };

            return await repository.ActualizarMovimientoAsync(movimiento);
        }
        catch (MovimientoException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new MovimientoException(
                "Error al actualizar el movimiento",
                ex,
                "MUTATION_ERROR",
                500
            );
        }
    }

    public async Task<MensajeResponse> eliminarMovimiento(
        [Service] IMovimientoRepository repository,
        int idMovimiento)
    {
        try
        {
            return await repository.EliminarMovimientoAsync(idMovimiento);
        }
        catch (MovimientoException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new MovimientoException(
                "Error al eliminar el movimiento",
                ex,
                "MUTATION_ERROR",
                500
            );
        }
    }
}