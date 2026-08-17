using HotChocolate;
using HotChocolate.Execution;
using Microsoft.Data.SqlClient;

namespace DonacionesProyecto.GraphQL;
public class FriendlyErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is SqlException sqlEx)
        {
            string mensaje = sqlEx.Number is 229 or 230 or 297
                ? "No tiene permisos para realizar esta operación con el rol actual."
                : "Ocurrió un error al consultar la base de datos.";

            return ConstruirErrorLimpio(error, mensaje);
        }

        if (error.Exception is not null)
        {
            return ConstruirErrorLimpio(error, "Ocurrió un error inesperado al procesar la solicitud.");
        }

        return error;
    }
    private static IError ConstruirErrorLimpio(IError original, string mensaje)
    {
        return ErrorBuilder.New()
            .SetMessage(mensaje)
            .SetPath(original.Path)
            .Build();
    }
}

