using System.Data;
using MySqlConnector;
using Aplicacion.Servicios;
using Persistencia.Conexion;
using Persistencia.Interfaces;
using Persistencia.Repositorios;
using Aplicacion.InterfaceServicio;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddSingleton<IDbConnectionFactory, MySQLConnectionFactory>();
builder.Services.AddScoped<IEnvioRepositorio, EnvioRepositorio>();
builder.Services.AddScoped<IEnvioServicio, EnvioServicio>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "API Envíos v1"));
}

app.MapEnvioEndpoints();
app.Run();