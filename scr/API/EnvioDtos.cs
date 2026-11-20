using Persistencia.Entidades;

namespace API;

public class EnvioRequestDto
{
    public Cliente Cliente { get; set; } = new();
    public Paquete Paquete { get; set; } = new();
    public Direccion Origen { get; set; } = new();
    public Direccion Destino { get; set; } = new();

    public int IdModalidad { get; set; }
    public double Distancia { get; set; }
}

public class EnvioResponseDto
{
    public int Id { get; set; }
    public int IdModalidad { get; set; }

    public Cliente Cliente { get; set; } = new();
    public Paquete Paquete { get; set; } = new();
    public Direccion Origen { get; set; } = new();
    public Direccion Destino { get; set; } = new();

    public double Distancia { get; set; }
    public string Estado { get; set; } = string.Empty;
    public double Costo { get; set; }
    public int DiasEstimados { get; set; }
}

public class CotizacionDto
{
    public int Id { get; set; }
    public double Costo { get; set; }
    public int DiasEstimados { get; set; }
}

public class CambiarEstadoDto
{
    public string NuevoEstado { get; set; } = string.Empty;
}