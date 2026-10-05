using Persistencia.Entidades;
using Xunit;
namespace Test;

public class EnvioPolimorfismotests 
//Cálculos de costo/tiempo, polimorfismo, validaciones del constructor 
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
    
    [Fact]

    public void Polimorfismo_ListaCalculaCostoSinConocerTipoConcreto()
    {
        var envios = new List<Envio>
        {
            new EnvioEstandar(1, _cliente, _paquete, _origen, _destino, 100),
            new EnvioExpress(2, _cliente, _paquete, _origen, _destino, 100),
            new EnvioPrioritario(3, _cliente, _paquete, _origen, _destino, 100)
        };

        double[] costosEsperados = { 3000, 5000, 7500 }; // fórmulas con distancia 100, peso 10
        int[] tiemposEsperados = { 5, 2, 1 };

        for (int i = 0; i < envios.Count; i++)
        {
            Assert.Equal(costosEsperados[i], envios[i].CalcularCosto());
            Assert.Equal(tiemposEsperados[i], envios[i].CalcularTiempoEntrega());
        }
    }
    [Fact]
    public void DistanciaCero_LanzaExcepcion()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new EnvioEstandar(0, _cliente, _paquete, _origen, _destino, 0));
    }

    [Fact]
    public void DistanciaNegativa_LanzaExcepcion()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new EnvioExpress(0, _cliente, _paquete, _origen, _destino, -10));
    }

    [Fact]
    public void ClienteNulo_LanzaExcepcion()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new EnvioEstandar(0, null!, _paquete, _origen, _destino, Distancia));
    }

    [Fact]
    public void PaqueteNulo_LanzaExcepcion()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new EnvioEstandar(0, _cliente, null!, _origen, _destino, Distancia));
    }

    [Fact]
    public void OrigenNulo_LanzaExcepcion()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new EnvioEstandar(0, _cliente, _paquete, null!, _destino, Distancia));
    }

    [Fact]
    public void DestinoNulo_LanzaExcepcion()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new EnvioEstandar(0, _cliente, _paquete, _origen, null!, Distancia));
    }

    [Fact]
    public void EstadoInicial_EsPendiente()
    {
        var envio = new EnvioEstandar(0, _cliente, _paquete, _origen, _destino, Distancia);
        Assert.Equal("PENDIENTE", envio.Estado);
    }

    [Fact]
    public void ConstantesModalidad_SonCorrectas()
    {
        Assert.Equal(1, EnvioEstandar.IdModalidad);
        Assert.Equal(2, EnvioExpress.IdModalidad);
        Assert.Equal(3, EnvioPrioritario.IdModalidad);
    }
     [Theory]
    [InlineData(0.01, 1, 1000 + (0.01 * 100) + (1 * 10))]
    [InlineData(50, 1000, 1000 + (50 * 100) + (1000 * 10))]
    [InlineData(1, 0.01, 1000 + (1 * 100) + (0.01 * 10))]
    public void EnvioEstandar_CasosBorde_CalculaCorrectamente(double peso, double distancia, double costoEsperado)
    {
        var paquete = new Paquete(peso, 1, 1, 1);
        var envio = new EnvioEstandar(0, _cliente, paquete, _origen, _destino, distancia);

        Assert.Equal(costoEsperado, envio.CalcularCosto(), 2);
        Assert.Equal(5, envio.CalcularTiempoEntrega());
    }

    [Theory]
    [InlineData(0.01, 1, 2000 + (0.01 * 150) + (1 * 15))]
    [InlineData(50, 1000, 2000 + (50 * 150) + (1000 * 15))]
    public void EnvioExpress_CasosBorde_CalculaCorrectamente(double peso, double distancia, double costoEsperado)
    {
        var paquete = new Paquete(peso, 1, 1, 1);
        var envio = new EnvioExpress(0, _cliente, paquete, _origen, _destino, distancia);

        Assert.Equal(costoEsperado, envio.CalcularCosto(), 2);
        Assert.Equal(2, envio.CalcularTiempoEntrega());
    }

    [Theory]
    [InlineData(0.01, 1, 3500 + (0.01 * 200) + (1 * 20))]
    [InlineData(50, 1000, 3500 + (50 * 200) + (1000 * 20))]
    public void EnvioPrioritario_CasosBorde_CalculaCorrectamente(double peso, double distancia, double costoEsperado)
    {
        var paquete = new Paquete(peso, 1, 1, 1);
        var envio = new EnvioPrioritario(0, _cliente, paquete, _origen, _destino, distancia);

        Assert.Equal(costoEsperado, envio.CalcularCosto(), 2);
        Assert.Equal(1, envio.CalcularTiempoEntrega());
    }
}
