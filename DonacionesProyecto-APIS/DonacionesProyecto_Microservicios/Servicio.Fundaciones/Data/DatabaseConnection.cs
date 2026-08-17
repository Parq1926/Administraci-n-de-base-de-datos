using Microsoft.Data.SqlClient;

namespace DonacionesProyecto.Data;

public class DatabaseConnection
{
    private readonly string _connectionString;
    private readonly string _conexionGenerico;
    private readonly string _conexionCliente;
    private readonly string _conexionEmpleado;

    public DatabaseConnection(IConfiguration configuration)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")!;

        _conexionGenerico =
            configuration.GetConnectionString("ConexionGenerico") ?? _connectionString;

        _conexionCliente =
            configuration.GetConnectionString("ConexionCliente") ?? _connectionString;

        _conexionEmpleado =
            configuration.GetConnectionString("ConexionEmpleado") ?? _connectionString;
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }

    // Método para obtener la conexión según el rol proporcionado
    public SqlConnection GetConnection(string? rol)
    {
        string connectionString = rol?.ToLower() switch
        {
            "cliente" => _conexionCliente,
            "empleado" => _conexionEmpleado,
            "admin" => _conexionEmpleado,
            _ => _conexionGenerico
        };

        return new SqlConnection(connectionString);
    }
}