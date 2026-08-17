using Servicio.Movimientos.Data;
using Servicio.Movimientos.Data.Repositories;
using Servicio.Movimientos.GraphQL.Mutations;
using Servicio.Movimientos.GraphQL.Queries;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


builder.Services.AddSingleton<ConnectionManager>();
builder.Services.AddScoped<IMovimientoRepository, MovimientoRepository>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<MovimientoQuery>()
    .AddMutationType<MovimientoMutation>();

var app = builder.Build();


app.MapGet("/", async context =>
{
    context.Response.Redirect("/graphql");
    await Task.CompletedTask;
});

// ✅ Mapear GraphQL en la raíz
app.MapGraphQL("/graphql");

app.Run();