using Servicio.Alertas.Data.Repositories;
using Servicio.Alertas.Models;
using HotChocolate;
using Servicio.Alertas.Exceptions;

namespace Servicio.Alertas.GraphQL.Queries;

public class AlertaQuery
{
    public async Task<IEnumerable<Alerta>> getAlertas(
        [Service] IAlertaRepository repository,
        int? idAlerta = null,
        int? idUsuario = null,
        bool? leida = null)
    {
        try
        {
            return await repository.LeerAlertasAsync(idAlerta, idUsuario, leida);
        }
        catch (AlertaException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new AlertaException(
                "Error al consultar alertas",
                ex,
                "QUERY_ERROR",
                500
            );
        }
    }
}