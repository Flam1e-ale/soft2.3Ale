public class EnvioRequestDto
{
    public string Modalidad { get; set; } = string.Empty;
    public decimal PesoKg { get; set; }
    public decimal DistanciaKm { get; set; }
    public string Destino { get; set; } = string.Empty;
}


public class EnvioResponseDto : EnvioRequestDto
{
    public int Id { get; set; }
    public string Estado { get; set; } = string.Empty;
    public decimal Costo { get; set; }
    public int DiasEstimados { get; set; }
}

public class CotizacionDto
{
    public int Id { get; set; }
    public decimal Costo { get; set; }
    public int DiasEstimados { get; set; }
}

public class CambiarEstadoDto
{
    public string NuevoEstado { get; set; } = string.Empty;
}