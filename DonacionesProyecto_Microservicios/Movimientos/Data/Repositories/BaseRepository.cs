using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Servicio.Movimientos.Exceptions;

namespace Servicio.Movimientos.Data.Repositories;

public abstract class BaseRepository
{
    protected readonly ConnectionManager _connectionManager;

    protected BaseRepository(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }

    protected async Task<IEnumerable<T>> ExecuteStoredProcedureAsync<T>(
        string storedProcedure,
        object? parameters = null,
        string rol = "empleado")
    {
        try
        {
            using var conn = await _connectionManager.GetConnectionAsync(rol);
            return await conn.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (SqlException ex)
        {
            throw MovimientoException.DatabaseError(storedProcedure, ex);
        }
        catch (Exception ex)
        {
            throw new MovimientoException(
                $"Error al ejecutar el procedimiento {storedProcedure}: {ex.Message}",
                ex,
                "EXECUTION_ERROR",
                500
            );
        }
    }

    protected async Task<Dictionary<string, object>> ExecuteStoredProcedureWithOutputAsync(
        string storedProcedure,
        DynamicParameters parameters,
        string rol = "empleado")
    {
        try
        {
            using var conn = await _connectionManager.GetConnectionAsync(rol);
            await conn.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);

            var result = new Dictionary<string, object>();
            foreach (var paramName in parameters.ParameterNames)
            {
                var value = parameters.Get<object>(paramName);
                result[paramName] = value ?? DBNull.Value;
            }
            return result;
        }
        catch (SqlException ex)
        {
            throw MovimientoException.DatabaseError(storedProcedure, ex);
        }
        catch (Exception ex)
        {
            throw new MovimientoException(
                $"Error al ejecutar el procedimiento {storedProcedure}: {ex.Message}",
                ex,
                "EXECUTION_ERROR",
                500
            );
        }
    }
}