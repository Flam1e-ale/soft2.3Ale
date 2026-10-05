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
}