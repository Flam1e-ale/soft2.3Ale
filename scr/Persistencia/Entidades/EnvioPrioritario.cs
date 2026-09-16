namespace Persistencia.entidades;

    public class EnvioPrioritario:Envio
    {
        public EnvioPrioritario(int id, Cliente cliente, Paquete paquete, Direccion origen,
         Direccion destino, double distancia):base(
            cliente,
            paquete,
            origen,
            destino,
            distancia)
    {
        
    }
    public override double CalcularCosto()
    {
        return 9;
    }
    public override int CalcularTiempoEntrega()
    {
        return 8;
    }
    }
