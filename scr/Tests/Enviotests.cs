using Persistencia.Entidades;
using Xunit;
namespace Test;

public class Enviotests  
{   
    private readonly Cliente _cliente = new("alex The Final boss","GOAT@gmail.com");
    private readonly Paquete _paquete=new(10,5,5,10);
    private readonly Direccion _origen=new("calle bautista",123,"Buenos aires");
    private readonly Direccion _destino=new("Av cordoba",742,"buenos aires");
    private const double Distancia=500;

    [Fact]
    public void EnvioEstandar_CalculaCostoYTiempoCorrectamente()
    {
        var envio = new EnvioEstandar(0, _cliente, _paquete, _origen, _destino, Distancia);
        double costo = envio.CalcularCosto();
        int tiempo = envio.CalcularTiempoEntrega();
        Assert.Equal(7000, costo);
        Assert.Equal(5, tiempo);
    }
    [Fact]
    public void EnvioExpress_CalculaCostoYTiempoCorrectamente()
    {
        var envio = new EnvioExpress(0, _cliente, _paquete, _origen, _destino, Distancia);

        double costo = envio.CalcularCosto();
        int tiempo = envio.CalcularTiempoEntrega();
        Assert.Equal(11000, costo);
        Assert.Equal(2, tiempo);
    }
    [Fact]
    public void EnvioPrioritario_CalculaCostoYTiempoCorrectamente()
    {
        var envio = new EnvioPrioritario(0, _cliente, _paquete, _origen, _destino, Distancia);

        double costo = envio.CalcularCosto();
        int tiempo = envio.CalcularTiempoEntrega();

        Assert.Equal(15500, costo);
        Assert.Equal(1, tiempo);
    }
    [Fact]
    public void DistintasModalidades_ProducenResultadosDiferentes()
    {
        var estandar = new EnvioEstandar(0, _cliente, _paquete, _origen, _destino, Distancia);
        var express = new EnvioExpress(0, _cliente, _paquete, _origen, _destino, Distancia);
        var prioritario = new EnvioPrioritario(0, _cliente, _paquete, _origen, _destino, Distancia);

        Assert.NotEqual(estandar.CalcularCosto(), express.CalcularCosto());
        Assert.NotEqual(express.CalcularCosto(), prioritario.CalcularCosto());
        Assert.NotEqual(estandar.CalcularTiempoEntrega(), express.CalcularTiempoEntrega());
    }
    [Fact]
    public void Polimorfismo_SePuedeTratarComoEnvioBase()
    {
        Envio[] envios =
        {
            new EnvioEstandar(0, _cliente, _paquete, _origen, _destino, Distancia),
            new EnvioExpress(0, _cliente, _paquete, _origen, _destino, Distancia),
            new EnvioPrioritario(0, _cliente, _paquete, _origen, _destino, Distancia)
        };
        foreach (var envio in envios)
        {
            Assert.True(envio.CalcularCosto() > 0);
            Assert.True(envio.CalcularTiempoEntrega() > 0);
        }
}
}
