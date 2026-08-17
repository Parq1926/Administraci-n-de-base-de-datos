using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class DonacionService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public DonacionService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //LEER
    public List<Donacion> ObtenerDonaciones()
    {
        List<Donacion> donaciones = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerDonaciones", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Donacion donacion = new();

            donacion.IdDonacion = Convert.ToInt32(reader["IdDonacion"]);
            donacion.Monto = Convert.ToDecimal(reader["Monto"]);

            if (reader["FechaDonacion"] != DBNull.Value)
                donacion.FechaDonacion = Convert.ToDateTime(reader["FechaDonacion"]);

            donacion.MetodoPago = reader["MetodoPago"]?.ToString();
            donacion.Estado = reader["Estado"]?.ToString();
            donacion.Comentario = reader["Comentario"]?.ToString();

            if (reader["IdCodigoDonacion"] != DBNull.Value)
                donacion.IdCodigoDonacion = Convert.ToInt32(reader["IdCodigoDonacion"]);

            donacion.CodigoDonacionNombre = reader["CodigoDonacionNombre"]?.ToString();

            if (reader["IdDonante"] != DBNull.Value)
                donacion.IdDonante = Convert.ToInt32(reader["IdDonante"]);

            if (reader["IdProyecto"] != DBNull.Value)
                donacion.IdProyecto = Convert.ToInt32(reader["IdProyecto"]);

            donacion.DonanteNombre = reader["DonanteNombre"]?.ToString();
            donacion.DonanteApellidos = reader["DonanteApellidos"]?.ToString();
            donacion.DonanteEmail = reader["DonanteEmail"]?.ToString();

            donacion.ProyectoNombre = reader["ProyectoNombre"]?.ToString();
            donacion.ProyectoEstado = reader["ProyectoEstado"]?.ToString();

            donaciones.Add(donacion);
        }

        return donaciones;
    }

    //INSERTAR
    public OperationResponse InsertarDonacion(DonacionInput donacion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Monto", donacion.Monto);
        command.Parameters.AddWithValue("@FechaDonacion", (object?)donacion.FechaDonacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@MetodoPago", donacion.MetodoPago);
        command.Parameters.AddWithValue("@Estado", donacion.Estado);
        command.Parameters.AddWithValue("@Comentario", (object?)donacion.Comentario ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdDonante", (object?)donacion.IdDonante ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdProyecto", (object?)donacion.IdProyecto ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdCodigoDonacion", (object?)donacion.IdCodigoDonacion ?? DBNull.Value);

        SqlParameter idOutput = new("@IdDonacion", SqlDbType.Int);
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
    public OperationResponse ActualizarDonacion(DonacionUpdateInput donacion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ActualizarDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdDonacion", donacion.IdDonacion);
        command.Parameters.AddWithValue("@Monto", (object?)donacion.Monto ?? DBNull.Value);
        command.Parameters.AddWithValue("@MetodoPago", (object?)donacion.MetodoPago ?? DBNull.Value);
        command.Parameters.AddWithValue("@Estado", (object?)donacion.Estado ?? DBNull.Value);
        command.Parameters.AddWithValue("@Comentario", (object?)donacion.Comentario ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdDonante", (object?)donacion.IdDonante ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdProyecto", (object?)donacion.IdProyecto ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdCodigoDonacion", (object?)donacion.IdCodigoDonacion ?? DBNull.Value);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = donacion.IdDonacion;
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
    public OperationResponse EliminarDonacion(int idDonacion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_EliminarDonacion", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdDonacion", idDonacion);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = idDonacion;
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

    // Traduce errores de SQL Server (permisos, etc.) a un mensaje entendible para el cliente
    private static string MensajeAmigable(SqlException ex)
    {
        if (ex.Number is 229 or 230 or 297)
            return "No tiene permisos para realizar esta operación con el rol actual.";

        return $"Ocurrió un error al procesar la solicitud: {ex.Message}";
    }

}