namespace API;
public interface IEnvioApiServicio
{
    EnvioResponseDto Crear(EnvioRequestDto solicitud);
    IReadOnlyList<EnvioResponseDto> Listar();
    EnvioResponseDto? Obtener(int id);
    CotizacionDto? Cotizar(int id);
    EnvioResponseDto? Actualizar(int id, EnvioRequestDto solicitud);
    bool Eliminar(int id);
    bool CambiarEstado(int id, string nuevoEstado);
    bool Cancelar(int id);
}