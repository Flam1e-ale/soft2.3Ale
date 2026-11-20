
using API;
using Persistencia.Conexion;
using Persistencia.Interfaces;
using Persistencia.Repositorios;
using Aplicacion.InterfaceServicio;
using Aplicacion.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexiones MySQL
var connectionStringInvitado =
    builder.Configuration.GetConnectionString("Invitado");

var connectionStringAdmin =
    builder.Configuration.GetConnectionString("Admin");

if (string.IsNullOrWhiteSpace(connectionStringInvitado) ||
    string.IsNullOrWhiteSpace(connectionStringAdmin))
{
    throw new InvalidOperationException(
        "Faltan las variables de entorno para conectarse a MySQL.");
}

builder.Services.AddSingleton<IDbConnectionFactory>(
    new MySQLConnectionFactory(
        connectionStringInvitado,
        connectionStringAdmin
    )
);

// Repositorios
builder.Services.AddScoped<IEnvioRepositorio, EnvioRepositorio>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IDireccionRepositorio, DireccionRepositorio>();

// Servicios de aplicación
builder.Services.AddScoped<IEnvioServicio, EnvioServicio>();
builder.Services.AddScoped<IClienteServicio, ClienteServicio>();
builder.Services.AddScoped<IDireccionServicio, DireccionServicio>();

// Servicio de API (lo adaptaremos a continuación)
builder.Services.AddScoped<IEnvioApiServicio, EnvioApiServicio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "API de Envíos funcionando.");

app.MapEnvioEndpoints();

app.Run();
