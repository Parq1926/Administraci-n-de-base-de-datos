using Servicio.Movimientos.Data.Repositories;
using Servicio.Movimientos.Models;
using HotChocolate;
using Servicio.Movimientos.Exceptions;

// ❌ ELIMINAR ESTO: [ExtendObjectType("Query")]
// ✅ NO usar ningún atributo
namespace Servicio.Movimientos.GraphQL.Queries;

public class MovimientoQuery
{
    public async Task<IEnumerable<Movimiento>> getMovimientos(
        [Service] IMovimientoRepository repository,
        int? idMovimiento = null,
        int? idDonacion = null,
        int? idProyecto = null)
    {
        try
        {
            return await repository.LeerMovimientosAsync(idMovimiento, idDonacion, idProyecto);
        }
        catch (MovimientoException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new MovimientoException(
                "Error al consultar movimientos",
                ex,
                "QUERY_ERROR",
                500
            );
        }
    }
}