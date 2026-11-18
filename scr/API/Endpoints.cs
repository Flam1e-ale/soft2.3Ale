using Microsoft.AspNetCore.Mvc;
public static class EnvioEndpoints
{
    public static void MapEnvioEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/envios",([FromServices]IEnvioServicio servicio, EnvioResponseDto dto)=>
        {
            try
            {
                var creado = servicio.Crear(dto);
                return Results.Created($"/api/envios/{creado.Id}", creado);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        app.MapGet("/api/envios", ([FromServices] IEnvioServicio servicio) =>
        {
            return Results.Ok(servicio.Listar());
        });

        app.MapGet("/api/envios/{id:int}", (int id, [FromServices] IEnvioServicio servicio) =>
        {
            var envio = servicio.ObtenerPorId(id);
            return envio is null
                ? Results.NotFound(new { error = $"No se encontró el envío con id {id}." })
                : Results.Ok(envio);
        });

         app.MapGet("/api/envios/{id:int}/cotizacion", (int id, [FromServices] IEnvioServicio servicio) =>
        {
            var cotizacion = servicio.ObtenerCotizacion(id);
            return cotizacion is null
                ? Results.NotFound(new { error = $"No se encontró el envío con id {id}." })
                : Results.Ok(cotizacion);
        });

        app.MapPut("/api/envios/{id:int}", (int id, EnvioRequestDto dto, [FromServices] IEnvioServicio servicio) =>
        {
            try
            {
                var actualizado = servicio.Actualizar(id, dto);
                return actualizado is null
                    ? Results.NotFound(new { error = $"No se encontró el envío con id {id}." })
                    : Results.Ok(actualizado);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

         app.MapDelete("/api/envios/{id:int}", (int id, [FromServices] IEnvioServicio servicio) =>
        {
            return servicio.Eliminar(id)
                ? Results.NoContent()
                : Results.NotFound(new { error = $"No se encontró el envío con id {id}." });
        });

         app.MapPatch("/api/envios/{id:int}/estado", (int id, CambiarEstadoDto dto, [FromServices] IEnvioServicio servicio) =>
        {
            try
            {
                var resultado = servicio.CambiarEstado(id, dto.NuevoEstado);
                return resultado is null
                    ? Results.NotFound(new { error = $"No se encontró el envío con id {id}." })
                    : Results.Ok(resultado);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });


    }
}