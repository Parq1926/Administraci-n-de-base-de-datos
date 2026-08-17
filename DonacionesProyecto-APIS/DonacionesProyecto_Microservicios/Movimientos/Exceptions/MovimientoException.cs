namespace Servicio.Movimientos.Exceptions;

public class MovimientoException : Exception
{
    public string CodigoError { get; }
    public int StatusCode { get; }

    public MovimientoException(string mensaje, string codigoError = "ERROR", int statusCode = 400)
        : base(mensaje)
    {
        CodigoError = codigoError;
        StatusCode = statusCode;
    }

    public MovimientoException(string mensaje, Exception innerException, string codigoError = "ERROR", int statusCode = 400)
        : base(mensaje, innerException)
    {
        CodigoError = codigoError;
        StatusCode = statusCode;
    }

    // Errores específicos
    public static MovimientoException NotFound(int id)
    {
        return new MovimientoException(
            $"El movimiento con ID {id} no existe",
            "NOT_FOUND",
            404
        );
    }

    public static MovimientoException InvalidMonto(decimal monto)
    {
        return new MovimientoException(
            $"El monto {monto} no es válido. Debe ser mayor a 0",
            "INVALID_MONTO",
            400
        );
    }

    public static MovimientoException MissingAssociation()
    {
        return new MovimientoException(
            "El movimiento debe estar asociado a una donación o un proyecto",
            "MISSING_ASSOCIATION",
            400
        );
    }

    public static MovimientoException InvalidType(string tipo)
    {
        return new MovimientoException(
            $"El tipo de movimiento '{tipo}' no es válido",
            "INVALID_TYPE",
            400
        );
    }

    public static MovimientoException DatabaseError(string storedProcedure, Exception innerException)
    {
        return new MovimientoException(
            $"Error al ejecutar el procedimiento {storedProcedure}",
            innerException,
            "DATABASE_ERROR",
            500
        );
    }
}