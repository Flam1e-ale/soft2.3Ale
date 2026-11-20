using Aplicacion.InterfaceServicio;
using Microsoft.AspNetCore.Mvc;
using Persistencia.Entidades;
public static class EnvioEndpoints
{
    public static void MapEnvioEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/envios", ([FromServices] IEnvioServicio servicio,
            int idCliente, int idOrigen, int idDestino,
            double peso, double alto, double ancho, double largo,
            double distancia, int idModalidad) =>
        {
            try
            {
                var creado = servicio.RegistrarEnvio(
                    idCliente, idOrigen, idDestino,
                    peso, alto, ancho, largo,
                    distancia, idModalidad);
                return Results.Created($"/api/envios/{creado.Id}", creado);
            }
            catch (ArgumentException ex)
            {return Results.BadRequest(new { error = ex.Message });}
        });

        app.MapGet("/api/envios", ([FromServices] IEnvioServicio servicio) =>
        {return Results.Ok(servicio.ListarTodos());});

        app.MapGet("/api/envios/{id:int}", (int id, [FromServices] IEnvioServicio servicio) =>
        {
            var envio = servicio.ObtenerPorId(id);
            return envio is null
                ? Results.NotFound(new { error = $"No se encontró el envío con id {id}." })
                : Results.Ok(envio);});

         app.MapGet("/api/envios/{id:int}/cotizacion", (int id, [FromServices] IEnvioServicio servicio) =>
        {
            var cotizacion = servicio.ObtenerPorId(id);
            return cotizacion is null
                ? Results.NotFound(new { error = $"No se encontró el envío con id {id}." })
                : Results.Ok(new{CostoTotal = Envio.CostoTotal, DiasEstimados = Envio.DiasEstimados}); });

        app.MapPut("/api/envios/{id:int}", ([FromServices] IEnvioServicio servicio,
            int idCliente, int idOrigen, int idDestino,
            double peso, double alto, double ancho, double largo,
            double distancia, int idModalidad) =>
        {
            try
            {
               var existente = servicio.ObtenerPorId(id);
                if (existente is null)
                    return Results.NotFound(new { error = $"No se encontró el envío con id {id}." });
                    servicio.CancelarEnvio(id);
                var actualizado = servicio.RegistrarEnvio(
                    idCliente, idOrigen, idDestino,
                    peso, alto, ancho, largo,
                    distancia, idModalidad);
                 return Results.Ok(actualizado);
            }
            catch (ArgumentException ex)
            {return Results.BadRequest(new { error = ex.Message });}
        });

        app.MapDelete("/api/envios/{id:int}", (int id, [FromServices] IEnvioServicio servicio) =>
        {
            try
            {
                servicio.CancelarEnvio(id);
                return Results.NoContent();
            }
            catch
            {
                return Results.NotFound(new { error = $"No se encontró el envío con id {id}." });
            }
        });
  
         app.MapPatch("/api/envios/{id:int}/estado", (int id, string nuevoEstado, [FromServices] IEnvioServicio servicio) =>
        {
            try
            {
                servicio.CambiarEstado(id, nuevoEstado);
                return Results.Ok(new { Mensaje = $"Estado cambiado a {nuevoEstado}" });
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });
        app.MapGet("/api/envios/consultar/resumen", (DateTime desde, DateTime hasta, [FromServices] IEnvioServicio servicio) =>
        {
            return Results.Ok(servicio.ConsultarResumen(desde, hasta));
        });

        app.MapGet("/api/envios/consultar/costos", (DateTime desde, DateTime hasta, [FromServices] IEnvioServicio servicio) =>
        {
            return Results.Ok(servicio.ConsultarCostos(desde, hasta));
        });

        app.MapGet("/api/envios/consultar/estados", (DateTime desde, DateTime hasta, [FromServices] IEnvioServicio servicio) =>
        {
            return Results.Ok(servicio.ConsultarEstados(desde, hasta));
        });

        app.MapGet("/api/envios/consultar/tiempo-promedio", (DateTime desde, DateTime hasta, [FromServices] IEnvioServicio servicio) =>
        {
            return Results.Ok(servicio.ConsultarTiempoPromedio(desde, hasta));
        });

        app.MapGet("/api/envios/consultar/facturacion", (DateTime desde, DateTime hasta, [FromServices] IEnvioServicio servicio) =>
        {
            return Results.Ok(servicio.ConsultarFacturacion(desde, hasta));
        }); }
}