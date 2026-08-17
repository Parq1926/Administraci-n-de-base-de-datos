using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Servicio.Movimientos.Data;

public class ConnectionManager
{
    private readonly IConfiguration _configuration;

    public ConnectionManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SqlConnection GetConnection(string rol = "empleado")
    {
        var connectionString = rol switch
        {
            "generico" => _configuration.GetConnectionString("Generico"),
            "cliente" => _configuration.GetConnectionString("Cliente"),
            "empleado" => _configuration.GetConnectionString("Empleado"),
            _ => _configuration.GetConnectionString("Empleado")
        };

        return new SqlConnection(connectionString);
    }

    public async Task<SqlConnection> GetConnectionAsync(string rol = "empleado")
    {
        var conn = GetConnection(rol);
        await conn.OpenAsync();
        return conn;
    }
}