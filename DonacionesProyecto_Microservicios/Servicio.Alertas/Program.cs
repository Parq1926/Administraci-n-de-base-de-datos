using Servicio.Alertas.Data;
using Servicio.Alertas.Data.Repositories;
using Servicio.Alertas.GraphQL.Mutations;
using Servicio.Alertas.GraphQL.Queries;

var builder = WebApplication.CreateBuilder(args);

// ✅ Logging detallado
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Servicios
builder.Services.AddSingleton<ConnectionManager>();
builder.Services.AddScoped<IAlertaRepository, AlertaRepository>();

// GraphQL con filtro de errores
builder.Services
    .AddGraphQLServer()
    .AddQueryType<AlertaQuery>()
    .AddMutationType<AlertaMutation>()
    // ✅ Agregar filtro para capturar errores detallados
    .AddErrorFilter(error =>
    {
        Console.WriteLine($"❌ GRAPHQL ERROR:");
        Console.WriteLine($"   Message: {error.Message}");
        Console.WriteLine($"   Code: {error.Code}");
        Console.WriteLine($"   Path: {error.Path}");

        if (error.Exception != null)
        {
            Console.WriteLine($"   Exception: {error.Exception.Message}");
            Console.WriteLine($"   StackTrace: {error.Exception.StackTrace}");

            if (error.Exception.InnerException != null)
            {
                Console.WriteLine($"   Inner Exception: {error.Exception.InnerException.Message}");
                Console.WriteLine($"   Inner StackTrace: {error.Exception.InnerException.StackTrace}");
            }
        }
        Console.WriteLine("----------------------------------------");

        return error;
    });

var app = builder.Build();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/graphql");
    await Task.CompletedTask;
});

app.MapGraphQL("/graphql");

app.Run();