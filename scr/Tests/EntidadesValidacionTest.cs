using Persistencia.Entidades;
using Xunit;

namespace Tests;


public class EntidadesValidacionTests
{
   

    [Fact]
    public void Paquete_ValoresValidos_SeCreaCorrectamente()
    {
        var paquete = new Paquete(10, 5, 8, 12);

        Assert.Equal(10, paquete.Peso);
        Assert.Equal(5, paquete.Alto);
        Assert.Equal(8, paquete.Ancho);
        Assert.Equal(12, paquete.Largo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.01)]
    [InlineData(50.01)]
    [InlineData(100)]
    public void Paquete_PesoInvalido_LanzaExcepcion(double peso)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Paquete(peso, 5, 5, 5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(20.01)]
    [InlineData(50)]
    public void Paquete_AltoInvalido_LanzaExcepcion(double alto)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Paquete(10, alto, 5, 5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(20.01)]
    public void Paquete_AnchoInvalido_LanzaExcepcion(double ancho)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Paquete(10, 5, ancho, 5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(25)]
    public void Paquete_LargoInvalido_LanzaExcepcion(double largo)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Paquete(10, 5, 5, largo));
    }

    [Fact]
    public void Paquete_LimitesValidos_SeAceptan()
    {
        var paqueteMin = new Paquete(0.01, 0.01, 0.01, 0.01);
        var paqueteMax = new Paquete(50, 20, 20, 20);

        Assert.Equal(0.01, paqueteMin.Peso);
        Assert.Equal(50, paqueteMax.Peso);
        Assert.Equal(20, paqueteMax.Alto);
    }

    [Fact]
    public void Paquete_SetterPesoInvalido_LanzaExcepcion()
    {
        var paquete = new Paquete(10, 5, 5, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => paquete.Peso = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => paquete.Peso = 51);
    }

   

    [Fact]
    public void Cliente_ValoresValidos_SeCreaCorrectamente()
    {
        var cliente = new Cliente("alex The Final Boss", "ale@mail.com");

        Assert.Equal("alex The Final Boss", cliente.Nombre);
        Assert.Equal("ale@mail.com", cliente.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Cliente_NombreInvalido_LanzaExcepcion(string? nombre)
    {
        Assert.Throws<ArgumentException>(() =>
            new Cliente(nombre!, "mail@mail.com"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Cliente_EmailInvalido_LanzaExcepcion(string? email)
    {
        Assert.Throws<ArgumentException>(() =>
            new Cliente("Nombre", email!));
    }

    [Fact]
    public void Cliente_SetterNombreInvalido_LanzaExcepcion()
    {
        var cliente = new Cliente("Ok", "ok@mail.com");
        Assert.Throws<ArgumentException>(() => cliente.Nombre = "");
        Assert.Throws<ArgumentException>(() => cliente.Nombre = "  ");
    }

    [Fact]
    public void Cliente_SetterEmailInvalido_LanzaExcepcion()
    {
        var cliente = new Cliente("Ok", "ok@mail.com");
        Assert.Throws<ArgumentException>(() => cliente.Email = "");
    }

    

    [Fact]
    public void Direccion_ValoresValidos_SeCreaCorrectamente()
    {
        var direccion = new Direccion("Av. Corrientes", 1234, "Buenos Aires");

        Assert.Equal("Av. Corrientes", direccion.Calle);
        Assert.Equal(1234, direccion.Numero);
        Assert.Equal("Buenos Aires", direccion.Ciudad);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Direccion_CalleInvalida_LanzaExcepcion(string? calle)
    {
        Assert.Throws<ArgumentException>(() =>
            new Direccion(calle!, 100, "Ciudad"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(5001)]
    [InlineData(10000)]
    public void Direccion_NumeroInvalido_LanzaExcepcion(int numero)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Direccion("Calle", numero, "Ciudad"));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Direccion_CiudadInvalida_LanzaExcepcion(string? ciudad)
    {
        Assert.Throws<ArgumentException>(() => 
            new Direccion("Calle", 100, ciudad!));
    }

    [Fact]
    public void Direccion_LimitesNumeroValidos_SeAceptan()
    {
        var d1 = new Direccion("Calle", 1, "Ciudad");
        var d2 = new Direccion("Calle", 5000, "Ciudad");

        Assert.Equal(1, d1.Numero);
        Assert.Equal(5000, d2.Numero);
    }

    [Fact]
    public void Direccion_SetterCalleInvalida_LanzaExcepcion()
    {
        var d = new Direccion("Calle", 100, "Ciudad");
        Assert.Throws<ArgumentException>(() => d.Calle = "");
    }

    [Fact]
    public void Direccion_SetterNumeroInvalido_LanzaExcepcion()
    {
        var d = new Direccion("Calle", 100, "Ciudad");
        Assert.Throws<ArgumentOutOfRangeException>(() => d.Numero = 0);
        Assert.Throws<ArgumentOutOfRangeException>(() => d.Numero = 5001);
    }
}