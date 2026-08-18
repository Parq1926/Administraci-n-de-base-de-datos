using DonacionesProyecto.Data;
using DonacionesProyecto.Models;
using DonacionesProyecto.Models.Inputs;
using DonacionesProyecto.Responses;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DonacionesProyecto.Services;

public class UsuarioService
{
    private readonly DatabaseConnection _database;
    private readonly RolContextService _rolContext;

    public UsuarioService(DatabaseConnection database, RolContextService rolContext)
    {
        _database = database;
        _rolContext = rolContext;
    }

    //LEER
    public List<Usuario> ObtenerUsuarios()
    {
        List<Usuario> usuarios = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_LeerUsuarios", connection);

        command.CommandType = CommandType.StoredProcedure;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            Usuario usuario = new();

            usuario.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
            usuario.Nombre = reader["Nombre"].ToString()!;
            usuario.Apellidos = reader["Apellidos"]?.ToString();
            usuario.Email = reader["Email"].ToString()!;
            usuario.TipoUsuario = reader["TipoUsuario"]?.ToString();

            if (reader["FechaRegistro"] != DBNull.Value)
                usuario.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);

            if (reader["Activo"] != DBNull.Value)
                usuario.Activo = Convert.ToBoolean(reader["Activo"]);

            usuarios.Add(usuario);
        }

        return usuarios;
    }

    //INSERTAR
    public OperationResponse InsertarUsuario(UsuarioInput usuario)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_InsertarUsuario", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@Apellidos", (object?)usuario.Apellidos ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", usuario.Email);
        command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
        command.Parameters.AddWithValue("@TipoUsuario", usuario.TipoUsuario);
        command.Parameters.AddWithValue("@Activo", usuario.Activo);

        SqlParameter idOutput = new("@IdUsuario", SqlDbType.Int);
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
    public OperationResponse ActualizarUsuario(UsuarioUpdateInput usuario)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ActualizarUsuario", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
        command.Parameters.AddWithValue("@Nombre", (object?)usuario.Nombre ?? DBNull.Value);
        command.Parameters.AddWithValue("@Apellidos", (object?)usuario.Apellidos ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", (object?)usuario.Email ?? DBNull.Value);
        command.Parameters.AddWithValue("@Contrasena", (object?)usuario.Contrasena ?? DBNull.Value);
        command.Parameters.AddWithValue("@TipoUsuario", (object?)usuario.TipoUsuario ?? DBNull.Value);
        command.Parameters.AddWithValue("@Activo", (object?)usuario.Activo ?? DBNull.Value);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = usuario.IdUsuario;
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
    public OperationResponse EliminarUsuario(int idUsuario)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_EliminarUsuario", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdUsuario", idUsuario);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;

        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = idUsuario;
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

    //LOGIN
    public LoginResponse Login(LoginInput login)
    {
        LoginResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_Login", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Email", login.Email);
        command.Parameters.AddWithValue("@Contrasena", login.Contrasena);

        SqlParameter exitosoOutput = new("@Exitoso", SqlDbType.Bit);
        exitosoOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(exitosoOutput);

        SqlParameter idUsuarioOutput = new("@IdUsuario", SqlDbType.Int);
        idUsuarioOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(idUsuarioOutput);

        SqlParameter nombreOutput = new("@Nombre", SqlDbType.VarChar, 100);
        nombreOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(nombreOutput);

        SqlParameter tipoUsuarioOutput = new("@TipoUsuario", SqlDbType.VarChar, 50);
        tipoUsuarioOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(tipoUsuarioOutput);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Exitoso = exitosoOutput.Value != DBNull.Value && Convert.ToBoolean(exitosoOutput.Value);

            if (idUsuarioOutput.Value != DBNull.Value)
                respuesta.IdUsuario = Convert.ToInt32(idUsuarioOutput.Value);

            respuesta.Nombre = nombreOutput.Value?.ToString() ?? "";
            respuesta.TipoUsuario = tipoUsuarioOutput.Value?.ToString() ?? "";
            respuesta.Mensaje = mensajeOutput.Value?.ToString() ?? "";

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Exitoso = false;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    //CONSULTAR SALDO POR CODIGO
    public SaldoResponse ConsultarSaldoPorCodigo(string codigo)
    {
        SaldoResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_ConsultarSaldoPorCodigo", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@Codigo", codigo);

        SqlParameter montoOutput = new("@Monto", SqlDbType.Decimal);
        montoOutput.Precision = 18;
        montoOutput.Scale = 2;
        montoOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(montoOutput);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            if (montoOutput.Value != DBNull.Value)
                respuesta.Monto = Convert.ToDecimal(montoOutput.Value);

            respuesta.Mensaje = mensajeOutput.Value?.ToString() ?? "";

            return respuesta;
        }
        catch (SqlException ex)
        {
            respuesta.Monto = 0;
            respuesta.Mensaje = MensajeAmigable(ex);
            return respuesta;
        }
    }

    //ASIGNAR DONACION A PROYECTO
    public OperationResponse AsignarDonacionAProyecto(int idDonacion, int idProyecto)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_AsignarDonacionAProyecto", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdDonacion", idDonacion);
        command.Parameters.AddWithValue("@IdProyecto", idProyecto);

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

    //REDISTRIBUIR FONDOS
    public OperationResponse RedistribuirFondos(RedistribuirFondosInput redistribucion)
    {
        OperationResponse respuesta = new();

        using SqlConnection connection = _database.GetConnection(_rolContext.ObtenerRol());

        using SqlCommand command =
            new SqlCommand("sp_RedistribuirFondos", connection);

        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@IdProyectoOrigen", redistribucion.IdProyectoOrigen);
        command.Parameters.AddWithValue("@IdProyectoDestino", redistribucion.IdProyectoDestino);
        command.Parameters.AddWithValue("@Monto", redistribucion.Monto);

        SqlParameter mensajeOutput = new("@Mensaje", SqlDbType.VarChar, 200);
        mensajeOutput.Direction = ParameterDirection.Output;
        command.Parameters.Add(mensajeOutput);

        try
        {
            connection.Open();

            command.ExecuteNonQuery();

            respuesta.Id = redistribucion.IdProyectoOrigen;
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