namespace Persistencia.Entidades;

public class EnvioDatos
{
    public int IdEnvio { get; set; }
    public int IdCliente { get; set; }
    public int IdPaquete { get; set; }
    public int IdModalidad { get; set; }
    public int IdDireccionOrigen { get; set; }
    public int IdDireccionDestino { get; set; }
    public double Distancia { get; set; }
    public double Costo { get; set; }
    public int TiempoEstimado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string Estado { get; set; } = string.Empty;

    // Datos del Cliente
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Datos del Paquete
    public double Peso { get; set; }
    public double Alto { get; set; }
    public double Ancho { get; set; }
    public double Largo { get; set; }

    // Dirección Origen
    public string CalleOrigen { get; set; } = string.Empty;
    public int NumeroOrigen { get; set; }
    public string CiudadOrigen { get; set; } = string.Empty;

    // Dirección Destino
    public string CalleDestino { get; set; } = string.Empty;
    public int NumeroDestino { get; set; }
    public string CiudadDestino { get; set; } = string.Empty;
}