using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class CodigoDonacionService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public CodigoDonacionService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //LEER
    public List<CodigoDonacion> ObtenerCodigos()
    {
        List<CodigoDonacion> codigos = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerCodigosDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            CodigoDonacion codigo = new();

            codigo.IdCodigo = Convert.ToInt32(reader["IdCodigo"]);
            codigo.IdFundacion = Convert.ToInt32(reader["IdFundacion"]);
            codigo.FundacionNombre = reader["FundacionNombre"]?.ToString();
            codigo.NombreCodigo = reader["NombreCodigo"].ToString()!;
            codigo.Descripcion = reader["Descripcion"]?.ToString();

            if (reader["PermiteRedistribucion"] != DBNull.Value)
                codigo.PermiteRedistribucion = Convert.ToBoolean(reader["PermiteRedistribucion"]);

            if (reader["Estado"] != DBNull.Value)
                codigo.Estado = Convert.ToBoolean(reader["Estado"]);

            if (reader["FechaCreacion"] != DBNull.Value)
                codigo.FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]);

            codigos.Add(codigo);
        }

        return codigos;
    }

    public List<SaldoCodigoResponse> ConsultarSaldoPorCodigo(int? idFundacion, int? idCodigo)
    {
        List<SaldoCodigoResponse> saldos = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ConsultarSaldoPorCodigo", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdFundacion", (object?)idFundacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdCodigo", (object?)idCodigo ?? DBNull.Value);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            SaldoCodigoResponse saldo = new();

            saldo.IdCodigo = Convert.ToInt32(reader["IdCodigo"]);
            saldo.NombreCodigo = reader["NombreCodigo"].ToString()!;
            saldo.IdFundacion = Convert.ToInt32(reader["IdFundacion"]);
            saldo.NombreFundacion = reader["NombreFundacion"]?.ToString();

            if (reader["PermiteRedistribucion"] != DBNull.Value)
                saldo.PermiteRedistribucion = Convert.ToBoolean(reader["PermiteRedistribucion"]);

            if (reader["Estado"] != DBNull.Value)
                saldo.Estado = Convert.ToBoolean(reader["Estado"]);

            saldo.SaldoDisponible = Convert.ToDecimal(reader["SaldoDisponible"]);

            saldos.Add(saldo);
        }

        return saldos;
    }

    //INSERTAR
    public OperationResponse InsertarCodigo(CodigoDonacionInput codigo)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarCodigoDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdFundacion", codigo.IdFundacion);
        command.Parameters.AddWithValue("@NombreCodigo", codigo.NombreCodigo);
        command.Parameters.AddWithValue("@Descripcion", (object?)codigo.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@PermiteRedistribucion", codigo.PermiteRedistribucion);
        command.Parameters.AddWithValue("@Estado", codigo.Estado);

        SqlParameter idOutput = new("@IdCodigo", SqlDbType.Int);
        idOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(idOutput);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            if (idOutput.Value != DBNull.Value)
                respuesta.Id = Convert.ToInt32(idOutput.Value);

            respuesta.Mensaje = mensajeOutput.Value?.ToString() ?? "";

            respuesta.Exito = respuesta.Id > 0;

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Exito = false;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    //ACTUALIZAR
    public OperationResponse ActualizarCodigo(CodigoDonacionUpdateInput codigo)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ActualizarCodigoDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdCodigo", codigo.IdCodigo);
        command.Parameters.AddWithValue("@IdFundacion", (object?)codigo.IdFundacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@NombreCodigo", (object?)codigo.NombreCodigo ?? DBNull.Value);
        command.Parameters.AddWithValue("@Descripcion", (object?)codigo.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@PermiteRedistribucion", (object?)codigo.PermiteRedistribucion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Estado", (object?)codigo.Estado ?? DBNull.Value);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = codigo.IdCodigo;
            respuesta.Mensaje = mensajeOutput.Value?.ToString() ?? "";
            respuesta.Exito = respuesta.Mensaje.Contains("exitosamente");

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Exito = false;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    //ELIMINAR
    public OperationResponse EliminarCodigo(int idCodigo)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_EliminarCodigoDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdCodigo", idCodigo);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = idCodigo;
            respuesta.Mensaje = mensajeOutput.Value?.ToString() ?? "";
            respuesta.Exito = respuesta.Mensaje.Contains("exitosamente");

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Exito = false;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    private static string MensajeAmigable(SqlException ex)
    {
        if (ex.Number is 229 or 230 or 297)
            return "No tiene permisos para realizar esta operación con el rol actual.";

        return $"Ocurrió un error al procesar la solicitud: {ex.Message}";
    }

}
