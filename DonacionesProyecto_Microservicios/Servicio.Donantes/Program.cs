using DonacionesProyecto.Data;
using DonacionesProyecto.GraphQL;
using DonacionesProyecto.GraphQL.Mutations;
using DonacionesProyecto.GraphQL.Queries;
using DonacionesProyecto.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<DatabaseConnection>();

builder.Services.AddScoped<RolContextService>();
builder.Services.AddScoped<DonanteService>();

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddErrorFilter<FriendlyErrorFilter>()
    .AddTypeExtension<DonanteQuery>()
    .AddTypeExtension<DonanteMutation>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGraphQL();

app.Run();
