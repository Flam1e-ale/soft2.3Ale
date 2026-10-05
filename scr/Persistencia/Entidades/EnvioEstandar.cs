namespace Persistencia.Entidades;
    public class EnvioEstandar:Envio 
    {
        public const int IdModalidad = 1;
         public EnvioEstandar(
        int id,
        Cliente cliente,
        Paquete paquete,
        Direccion origen,
        Direccion destino,
        double distancia)
        : base(id,cliente, paquete, origen, destino, distancia)
    {
        
    }
    public override double CalcularCosto()
    {
        return 1000 + (Paquete.Peso * 100) + (Distancia * 10);
    }
    public override int CalcularTiempoEntrega()
    {
        return 5;
    }
    }
    