using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class HistorialCodigoService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public HistorialCodigoService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //LEER 
    public List<HistorialCodigo> ObtenerHistorial(int? idCodigo, int? idUsuario)
    {
        List<HistorialCodigo> historial = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerHistorialCodigos", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdHistorial", DBNull.Value);
        command.Parameters.AddWithValue("@IdCodigo", (object?)idCodigo ?? DBNull.Value);
        command.Parameters.AddWithValue("@IdUsuario", (object?)idUsuario ?? DBNull.Value);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            HistorialCodigo item = new();

            item.IdHistorial = Convert.ToInt32(reader["IdHistorial"]);
            item.IdCodigo = Convert.ToInt32(reader["IdCodigo"]);
            item.CodigoNombre = reader["NombreCodigo"]?.ToString();
            item.Accion = reader["Accion"].ToString()!;

            if (reader["Fecha"] != DBNull.Value)
                item.Fecha = Convert.ToDateTime(reader["Fecha"]);

            item.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
            item.UsuarioNombre = reader["UsuarioNombre"]?.ToString();
            item.DescripcionCambio = reader["DescripcionCambio"]?.ToString();

            historial.Add(item);
        }

        return historial;
    }

    //INSERTAR 
    public OperationResponse InsertarHistorial(HistorialCodigoInput historial)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarHistorialCodigo", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdCodigo", historial.IdCodigo);
        command.Parameters.AddWithValue("@Accion", historial.Accion);
        command.Parameters.AddWithValue("@IdUsuario", historial.IdUsuario);
        command.Parameters.AddWithValue("@DescripcionCambio", (object?)historial.DescripcionCambio ?? DBNull.Value);

        SqlParameter idOutput = new("@IdHistorial", SqlDbType.Int);
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

    private static string MensajeAmigable(SqlException ex)
    {
        if (ex.Number is 229 or 230 or 297)
            return "No tiene permisos para realizar esta operación con el rol actual.";

        return $"Ocurrió un error al procesar la solicitud: {ex.Message}";
    }

}
