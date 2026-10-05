namespace Persistencia.Entidades;
  public abstract class Envio
{
    public int Id { get; set; }

    public Cliente Cliente { get; set; }

    public Paquete Paquete { get; set; }

    public Direccion Origen { get; set; }

    public Direccion Destino { get; set; }

    public double Distancia { get; set; }

    public double Costo { get; protected set; }

    public int TiempoEstimado { get; protected set; }

    public string Estado { get; set; }

    protected Envio(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion origen,
        Direccion destino,
        double distancia)
    {
        if (cliente == null)
            throw new ArgumentNullException(nameof(cliente));

        if (paquete == null)
            throw new ArgumentNullException(nameof(paquete));

        if (origen == null)
            throw new ArgumentNullException(nameof(origen));

        if (destino == null)
            throw new ArgumentNullException(nameof(destino));

        if (distancia <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(distancia),
                "La distancia debe ser mayor que cero.");

        Id = id;
        Cliente = cliente;
        Paquete = paquete;
        Origen = origen;
        Destino = destino;
        Distancia = distancia;
        Estado = "PENDIENTE";
    }

    public abstract double CalcularCosto();

    public abstract int CalcularTiempoEntrega();
}
