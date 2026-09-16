namespace Persistencia.entidades;

    public abstract class Envio
    {
    public int Id { get; set; }
    
    public Cliente Cliente { get; set; }

    public Paquete Paquete { get; set; }

    public Direccion Origen { get; set; }

    public Direccion Destino { get; set; }

    public double Distancia { get; set; }
    protected Envio(
        Cliente cliente,
        Paquete paquete,
        Direccion origen,
        Direccion destino,
        double distancia)
    {
        Cliente = cliente;
        Paquete = paquete;
        Origen = origen;
        Destino = destino;
        Distancia = distancia;
    }
        public abstract double CalcularCosto();

        public abstract int CalcularTiempoEntrega();
    }
