public interface IEnvioServicio
{
    EnvioResponseDto Crear(EnvioRequestDto dto);
    IReadOnlyList<EnvioResponseDto> Listar();
    EnvioResponseDto? ObtenerPorId(int id);
    CotizacionDto? ObtenerCotizacion(int id);
    EnvioResponseDto? Actualizar(int id, EnvioRequestDto dto);
    bool Eliminar(int id);
    EnvioResponseDto? CambiarEstado(int id, string nuevoEstado);
}
