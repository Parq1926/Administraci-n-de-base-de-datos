using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class DonanteService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public DonanteService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //LEER
    public List<Donante> ObtenerDonantes()
    {
        List<Donante> donantes = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerDonantes", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Donante donante = new();

            donante.IdDonante = Convert.ToInt32(reader["IdDonante"]);
            donante.Nombre = reader["Nombre"].ToString()!;
            donante.Apellidos = reader["Apellidos"]?.ToString();
            donante.Email = reader["Email"].ToString()!;
            donante.Telefono = reader["Telefono"]?.ToString();
            donante.Direccion = reader["Direccion"]?.ToString();
            donante.TipoDonante = reader["TipoDonante"]?.ToString();
            donante.NombreCompleto = reader["NombreCompleto"]?.ToString();

            if (reader["FechaRegistro"] != DBNull.Value)
                donante.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);

            if (reader["Activo"] != DBNull.Value)
                donante.Activo = Convert.ToBoolean(reader["Activo"]);

            donantes.Add(donante);
        }

        return donantes;
    }

    //INSERTAR
    public OperationResponse InsertarDonante(DonanteInput donante)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarDonante", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Nombre", donante.Nombre);
        command.Parameters.AddWithValue("@Apellidos", (object?)donante.Apellidos ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", donante.Email);
        command.Parameters.AddWithValue("@Telefono", (object?)donante.Telefono ?? DBNull.Value);
        command.Parameters.AddWithValue("@Direccion", (object?)donante.Direccion ?? DBNull.Value);
        command.Parameters.AddWithValue("@TipoDonante", donante.TipoDonante);
        command.Parameters.AddWithValue("@Activo", donante.Activo);

        SqlParameter idOutput = new("@IdDonante", SqlDbType.Int);
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
    public OperationResponse ActualizarDonante(DonanteUpdateInput donante)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ActualizarDonante", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdDonante", donante.IdDonante);
        command.Parameters.AddWithValue("@Nombre", (object?)donante.Nombre ?? DBNull.Value);
        command.Parameters.AddWithValue("@Apellidos", (object?)donante.Apellidos ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)donante.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Telefono", (object?)donante.Telefono ?? DBNull.Value);
        command.Parameters.AddWithValue("@Direccion", (object?)donante.Direccion ?? DBNull.Value);
        command.Parameters.AddWithValue("@TipoDonante", (object?)donante.TipoDonante ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", (object?)donante.Activo ?? DBNull.Value);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = donante.IdDonante;
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
    public OperationResponse EliminarDonante(int idDonante)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_EliminarDonante", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdDonante", idDonante);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = idDonante;
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