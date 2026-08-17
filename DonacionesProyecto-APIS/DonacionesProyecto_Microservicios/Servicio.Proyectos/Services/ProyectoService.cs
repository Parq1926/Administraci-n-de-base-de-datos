using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class ProyectoService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public ProyectoService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //OBTENER
    public List<Proyecto> ObtenerProyectos()
    {
        List<Proyecto> proyectos = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerProyectos", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Proyecto proyecto = new();

            proyecto.IdProyecto = Convert.ToInt32(reader["IdProyecto"]);
            proyecto.Nombre = reader["Nombre"].ToString()!;
            proyecto.Descripcion = reader["Descripcion"]?.ToString();

            if (reader["MetaRecaudacion"] != DBNull.Value)
                proyecto.MetaRecaudacion = Convert.ToDecimal(reader["MetaRecaudacion"]);

            if (reader["FechaInicio"] != DBNull.Value)
                proyecto.FechaInicio = Convert.ToDateTime(reader["FechaInicio"]);

            if (reader["FechaFin"] != DBNull.Value)
                proyecto.FechaFin = Convert.ToDateTime(reader["FechaFin"]);

            proyecto.Estado = reader["Estado"]?.ToString();

            if (reader["IdFundacion"] != DBNull.Value)
                proyecto.IdFundacion = Convert.ToInt32(reader["IdFundacion"]);

            proyecto.FundacionNombre = reader["FundacionNombre"]?.ToString();

            if (reader["Activo"] != DBNull.Value)
                proyecto.Activo = Convert.ToBoolean(reader["Activo"]);

            if (reader["Recaudado"] != DBNull.Value)
                proyecto.Recaudado = Convert.ToDecimal(reader["Recaudado"]);

            proyectos.Add(proyecto);
        }

        return proyectos;
    }

    //INSERTAR
    public OperationResponse InsertarProyecto(ProyectoInput proyecto)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarProyecto", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Nombre", proyecto.Nombre);
        command.Parameters.AddWithValue("@Descripcion", (object?)proyecto.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@MetaRecaudacion", (object?)proyecto.MetaRecaudacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@FechaInicio", (object?)proyecto.FechaInicio ?? DBNull.Value);
        command.Parameters.AddWithValue("@FechaFin", (object?)proyecto.FechaFin ?? DBNull.Value);
        command.Parameters.AddWithValue("@Estado", proyecto.Estado);
        command.Parameters.AddWithValue("@IdFundacion", (object?)proyecto.IdFundacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", proyecto.Activo);

        SqlParameter idOutput = new("@IdProyecto", SqlDbType.Int);
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
    public OperationResponse ActualizarProyecto(ProyectoUpdateInput proyecto)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ActualizarProyecto", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdProyecto", proyecto.IdProyecto);
        command.Parameters.AddWithValue("@Nombre", (object?)proyecto.Nombre ?? DBNull.Value);
        command.Parameters.AddWithValue("@Descripcion", (object?)proyecto.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@MetaRecaudacion", (object?)proyecto.MetaRecaudacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@FechaInicio", (object?)proyecto.FechaInicio ?? DBNull.Value);
        command.Parameters.AddWithValue("@FechaFin", (object?)proyecto.FechaFin ?? DBNull.Value);
        command.Parameters.AddWithValue("@Estado", (object?)proyecto.Estado ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdFundacion", (object?)proyecto.IdFundacion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", (object?)proyecto.Activo ?? DBNull.Value);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = proyecto.IdProyecto;
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
    public OperationResponse EliminarProyecto(int idProyecto)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_EliminarProyecto", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdProyecto", idProyecto);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = idProyecto;
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