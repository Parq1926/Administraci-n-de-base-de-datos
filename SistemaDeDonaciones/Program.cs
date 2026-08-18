using SistemaDeDonaciones.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<AuthService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.AccessDeniedPath = "/Cuenta/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });


//Rol de empleado fijo
var microservicios = builder.Configuration.GetSection("Microservicios");

builder.Services.AddHttpClient<FundacionApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Fundaciones"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<ProyectoApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Proyectos"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<DonanteApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Donantes"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<DonacionApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Donaciones"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<UsuarioApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Usuarios"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<HistorialCodigoApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["HistorialCodigos"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

//Solo lectura
builder.Services.AddHttpClient<FundacionPublicApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Fundaciones"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Generico");
});

builder.Services.AddHttpClient<ProyectoPublicApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Proyectos"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Generico");
});

builder.Services.AddHttpClient<DonacionPublicApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Donaciones"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Generico");
});

// Servicios para Alertas, Movimientos y Códigos de Donación
builder.Services.AddHttpClient<AlertaApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Alertas"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<MovimientoApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Movimientos"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

builder.Services.AddHttpClient<CodigoDonacionApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["CodigosDonacion"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

// Servicio de Usuarios
builder.Services.AddHttpClient<UsuarioApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Usuarios"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});
// Donaciones
builder.Services.AddHttpClient<DonacionApiService>(client =>
{
    client.BaseAddress = new Uri(microservicios["Donaciones"]!);
    client.DefaultRequestHeaders.Add("X-Rol", "Empleado");
});

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
