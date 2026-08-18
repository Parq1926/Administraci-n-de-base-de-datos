using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class FundacionService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public FundacionService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //CONSULTAR
    public List<Fundacion> ObtenerFundaciones()
    {
        List<Fundacion> fundaciones = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerFundaciones", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Fundacion fundacion = new();

            fundacion.IdFundacion = Convert.ToInt32(reader["IdFundacion"]);
            fundacion.Nombre = reader["Nombre"].ToString()!;
            fundacion.Descripcion = reader["Descripcion"]?.ToString();
            fundacion.Identificacion = reader["Identificacion"]?.ToString();
            fundacion.Email = reader["Email"]?.ToString();
            fundacion.Telefono = reader["Telefono"]?.ToString();
            fundacion.Direccion = reader["Direccion"]?.ToString();

            if (reader["FechaRegistro"] != DBNull.Value)
                fundacion.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);

            if (reader["Activo"] != DBNull.Value)
                fundacion.Activo = Convert.ToBoolean(reader["Activo"]);

            fundaciones.Add(fundacion);
        }

        return fundaciones;
    }

    //INSERTAR
    public OperationResponse InsertarFundacion(FundacionInput fundacion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarFundacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Nombre", fundacion.Nombre);
        command.Parameters.AddWithValue("@Descripcion", (object?)fundacion.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Identificacion", (object?)fundacion.Identificacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)fundacion.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Telefono", (object?)fundacion.Telefono ?? DBNull.Value);
        command.Parameters.AddWithValue("@Direccion", (object?)fundacion.Direccion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", fundacion.Activo);

        SqlParameter idOutput = new("@IdFundacion", SqlDbType.Int);
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
    public OperationResponse ActualizarFundacion(FundacionUpdateInput fundacion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ActualizarFundacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdFundacion", fundacion.IdFundacion);
        command.Parameters.AddWithValue("@Nombre", (object?)fundacion.Nombre ?? DBNull.Value);
        command.Parameters.AddWithValue("@Descripcion", (object?)fundacion.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Identificacion", (object?)fundacion.Identificacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)fundacion.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Telefono", (object?)fundacion.Telefono ?? DBNull.Value);
        command.Parameters.AddWithValue("@Direccion", (object?)fundacion.Direccion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", (object?)fundacion.Activo ?? DBNull.Value);

        SqlParameter mensajeOutput =
            new("@Mensaje", SqlDbType.VarChar, 200);

        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = fundacion.IdFundacion;

            respuesta.Mensaje =
                mensajeOutput.Value?.ToString() ?? "";

            respuesta.Exito =
                respuesta.Mensaje.Contains("exitosamente");

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Exito = false;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    //Eliminar
    public OperationResponse EliminarFundacion(int idFundacion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_EliminarFundacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdFundacion", idFundacion);

        SqlParameter mensajeOutput =
            new("@Mensaje", SqlDbType.VarChar, 200);

        mensajeOutput.Direction =
            ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = idFundacion;

            respuesta.Mensaje =
                mensajeOutput.Value?.ToString() ?? "";

            respuesta.Exito =
                respuesta.Mensaje.Contains("exitosamente");

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Exito = false;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    // Traduce errores de SQL Server (permisos, etc.) a un mensaje entendible para el cliente
    private static string MensajeAmigable(SqlException ex)
    {
        if (ex.Number is 229 or 230 or 297)
            return "No tiene permisos para realizar esta operación con el rol actual.";

        return $"Ocurrió un error al procesar la solicitud: {ex.Message}";
    }

}