using DonacionesProyecto.Data;
using DonacionesProyecto.Services;
using DonacionesProyecto.GraphQL.Queries;
using DonacionesProyecto.GraphQL.Mutations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// ✅ DatabaseConnection
builder.Services.AddSingleton<DatabaseConnection>();

builder.Services.AddScoped<RolContextService>();

builder.Services.AddScoped<CodigoDonacionService>();


builder.Services
    .AddGraphQLServer()
    .AddQueryType()
    .AddTypeExtension<CodigoDonacionQuery>()
    .AddMutationType()
    .AddTypeExtension<CodigoDonacionMutation>();


var app = builder.Build();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/graphql");
    await Task.CompletedTask;
});

app.MapGraphQL("/graphql");

app.Run();