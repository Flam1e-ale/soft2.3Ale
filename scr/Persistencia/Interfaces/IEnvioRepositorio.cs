using Persistencia.Entidades;

namespace Persistencia.Interfaces;

public interface IEnvioRepositorio
{
    Envio? ObtenerPorId(int idEnvio);

    List<Envio> ObtenerTodos();
}