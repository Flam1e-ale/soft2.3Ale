using API;
using Aplicacion.InterfaceServicio;
using Microsoft.AspNetCore.Mvc;
using Persistencia.Entidades;
public static class EnvioEndpoints
{
    public static void MapEnvioEndpoints(this IEndpointRouteBuilder app)
    { 
        app.MapPost("/api/envios", ([FromServices] IEnvioApiServicio servicio,
            EnvioRequestDto solicitud) =>
    {   
            try
            {
                var creado = servicio.Crear(solicitud);

                return Results.Created($"/api/envios/{creado.Id}", creado);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
        app.MapGet("/api/envios", ([FromServices] IEnvioApiServicio servicio) =>
        {
            return Results.Ok(servicio.Listar());
        });
        app.MapGet("/api/envios/{id:int}", (int id,
            [FromServices] IEnvioApiServicio servicio) =>
        {
            var envio = servicio.Obtener(id);

            return envio == null
                ? Results.NotFound(new { error = "No se encontró el envío." })
                : Results.Ok(envio);
        });
        app.MapGet("/api/envios/{id:int}/cotizacion", (int id,
            [FromServices] IEnvioApiServicio servicio) =>
        {
            var cotizacion = servicio.Cotizar(id);

            return cotizacion == null
                ? Results.NotFound(new { error = "No se encontró el envío." })
                : Results.Ok(cotizacion);
        });
        app.MapPut("/api/envios/{id:int}", (int id,
            [FromServices] IEnvioApiServicio servicio,
            EnvioRequestDto solicitud) =>
        {
            try
            {
                var actualizado = servicio.Actualizar(id, solicitud);

                return actualizado == null
                    ? Results.NotFound(new { error = "No se encontró el envío." })
                    : Results.Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
        app.MapDelete("/api/envios/{id:int}", (int id,
            [FromServices] IEnvioApiServicio servicio) =>
        {
            bool eliminado = servicio.Eliminar(id);

            return eliminado
                ? Results.NoContent()
                : Results.NotFound(new { error = "No se encontró el envío." });
        });
        app.MapPatch("/api/envios/{id:int}/estado", (int id,
            [FromServices] IEnvioApiServicio servicio,
            CambiarEstadoDto solicitud) =>
        {
            try
            {
                bool cambiado = servicio.CambiarEstado(id, solicitud.NuevoEstado);

                return cambiado
                    ? Results.Ok(new { mensaje = "Estado actualizado correctamente." })
                    : Results.NotFound(new { error = "No se encontró el envío." });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

}   
}