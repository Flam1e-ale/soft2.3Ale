using System.Data;
using MySqlConnector;


var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("MySql");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Debe configurar ConnectionStrings:MySql en appsettings.json o la variable de entorno ConnectionStrings__MySql.");
}
builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API de Envíos v1");
        options.EnableTryItOutByDefault();
    });
}

app.UseHttpsRedirection();

app.MapGet("/bienvenida", () => Results.Ok("API de Envíos – SOFTWARE 2.4"));


app.MapEnvioEndpoints();

app.Run();