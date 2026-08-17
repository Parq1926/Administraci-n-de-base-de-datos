namespace Servicio.Alertas.Exceptions;

public class AlertaException : Exception
{
    public string CodigoError { get; }
    public int StatusCode { get; }

    public AlertaException(string mensaje, string codigoError = "ERROR", int statusCode = 400)
        : base(mensaje)
    {
        CodigoError = codigoError;
        StatusCode = statusCode;
    }

    public AlertaException(string mensaje, Exception innerException, string codigoError = "ERROR", int statusCode = 400)
        : base(mensaje, innerException)
    {
        CodigoError = codigoError;
        StatusCode = statusCode;
    }

    // 📌 Errores específicos
    public static AlertaException NotFound(int id)
    {
        return new AlertaException(
            $"La alerta con ID {id} no existe",
            "NOT_FOUND",
            404
        );
    }

    public static AlertaException InvalidTitulo(string titulo)
    {
        return new AlertaException(
            $"El título '{titulo}' no es válido",
            "INVALID_TITULO",
            400
        );
    }

    public static AlertaException InvalidType(string tipo)
    {
        return new AlertaException(
            $"El tipo de alerta '{tipo}' no es válido. Tipos permitidos: Info, Success, Warning, Error",
            "INVALID_TYPE",
            400
        );
    }

    public static AlertaException DatabaseError(string storedProcedure, Exception innerException)
    {
        return new AlertaException(
            $"Error al ejecutar el procedimiento {storedProcedure}",
            innerException,
            "DATABASE_ERROR",
            500
        );
    }
}