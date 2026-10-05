namespace Persistencia.Entidades;
    public class EnvioExpress:Envio
    {

    public const int IdModalidad = 2;

    public EnvioExpress(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion origen,
        Direccion destino,
        double distancia)
        : base(
            id,
            cliente,
            paquete,
            origen,
            destino,
            distancia)

    {
        
    }
     public override double CalcularCosto()
    {
        return 2000 + (Paquete.Peso * 150) + (Distancia * 15);
    }
    public override int CalcularTiempoEntrega()
    {
        return 2;
    }

    
    }