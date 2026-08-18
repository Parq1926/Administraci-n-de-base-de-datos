using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Servicio.Alertas.Data;

public class ConnectionManager
{
    private readonly IConfiguration _configuration;

    public ConnectionManager(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<SqlConnection> GetConnectionAsync(string rol = "Empleado")
    {
        try
        {
            // ✅ Usar las claves exactas de appsettings.json
            var connectionString = rol.ToLower() switch
            {
                "generico" => _configuration.GetConnectionString("Generico"),
                "cliente" => _configuration.GetConnectionString("Cliente"),
                "empleado" => _configuration.GetConnectionString("Empleado"),
                _ => _configuration.GetConnectionString("Empleado")
            };

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception($"No se encontró la cadena de conexión para el rol: {rol}");
            }

            var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();
            Console.WriteLine($"✅ Conexión establecida para rol: {rol}");
            return conn;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al establecer conexión para rol: {rol} - {ex.Message}");
            throw new Exception($"Error al establecer conexión para rol: {rol} - {ex.Message}", ex);
        }
    }
}