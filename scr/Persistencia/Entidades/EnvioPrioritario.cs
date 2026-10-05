namespace Persistencia.Entidades;
    public class EnvioPrioritario:Envio
    {
        public const int IdModalidad = 3;
         public EnvioPrioritario(
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
        return Costo= 3500 + (Paquete.Peso * 200) + (Distancia * 20);
    }
    public override int CalcularTiempoEntrega()
    {
        return TiempoEstimado= 1;
    }
    }
