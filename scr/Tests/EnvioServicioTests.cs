using Aplicacion.Servicios;
using Moq;
using Persistencia.Entidades;
using Persistencia.Interfaces;
using Xunit;

namespace Tests;

public class EnvioServicioTests
{
    private readonly Mock<IEnvioRepositorio> _mockRepo;
    private readonly EnvioServicio _servicio;

    public EnvioServicioTests()
    {
        _mockRepo = new Mock<IEnvioRepositorio>();
        _servicio = new EnvioServicio(_mockRepo.Object);
    }

    

    [Fact]
    public void RegistrarEnvio_ModalidadEstandar_CalculaCostoYTiempoCorrectos()
    {
       
        _mockRepo
            .Setup(r => r.AltaEnvioCompleto(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(),
                It.IsAny<double>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns(1);

        
        var envio = _servicio.RegistrarEnvio(
            idCliente: 1, idOrigen: 1, idDestino: 2,
            peso: 10, alto: 5, ancho: 5, largo: 10,
            distancia: 500, idModalidad: EnvioEstandar.IdModalidad);

       
        Assert.Equal(7000, envio.Costo);   
        Assert.Equal(5, envio.TiempoEstimado);
        Assert.IsType<EnvioEstandar>(envio);

        _mockRepo.Verify(r => r.AltaEnvioCompleto(
            1, 1, 2, 10, 5, 5, 10, 500,
            EnvioEstandar.IdModalidad, 7000, 5), Times.Once);
    }

    [Fact]
    public void RegistrarEnvio_ModalidadExpress_CalculaCostoYTiempoCorrectos()
    {
        _mockRepo
            .Setup(r => r.AltaEnvioCompleto(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(),
                It.IsAny<double>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns(1);

        var envio = _servicio.RegistrarEnvio(
            1, 1, 2, 10, 5, 5, 10, 500, EnvioExpress.IdModalidad);

        Assert.Equal(11000, envio.Costo);
        Assert.Equal(2, envio.TiempoEstimado);
        Assert.IsType<EnvioExpress>(envio);

        _mockRepo.Verify(r => r.AltaEnvioCompleto(
            1, 1, 2, 10, 5, 5, 10, 500,
            EnvioExpress.IdModalidad, 11000, 2), Times.Once);
    }

    [Fact]
    public void RegistrarEnvio_ModalidadPrioritario_CalculaCostoYTiempoCorrectos()
    {
        _mockRepo
            .Setup(r => r.AltaEnvioCompleto(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(),
                It.IsAny<double>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns(1);

        var envio = _servicio.RegistrarEnvio(
            1, 1, 2, 10, 5, 5, 10, 500, EnvioPrioritario.IdModalidad);

        Assert.Equal(15500, envio.Costo); 
        Assert.Equal(1, envio.TiempoEstimado);
        Assert.IsType<EnvioPrioritario>(envio);

        _mockRepo.Verify(r => r.AltaEnvioCompleto(
            1, 1, 2, 10, 5, 5, 10, 500,
            EnvioPrioritario.IdModalidad, 15500, 1), Times.Once);
    }

    [Fact]
    public void RegistrarEnvio_TresModalidades_ProducenCostosDiferentes()
    {
        _mockRepo
            .Setup(r => r.AltaEnvioCompleto(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(),
                It.IsAny<double>(), It.IsAny<int>(), It.IsAny<double>(), It.IsAny<int>()))
            .Returns(1);

        var e1 = _servicio.RegistrarEnvio(1, 1, 2, 10, 5, 5, 10, 500, 1);
        var e2 = _servicio.RegistrarEnvio(1, 1, 2, 10, 5, 5, 10, 500, 2);
        var e3 = _servicio.RegistrarEnvio(1, 1, 2, 10, 5, 5, 10, 500, 3);

        Assert.NotEqual(e1.Costo, e2.Costo);
        Assert.NotEqual(e2.Costo, e3.Costo);
        Assert.NotEqual(e1.TiempoEstimado, e2.TiempoEstimado);
        Assert.NotEqual(e2.TiempoEstimado, e3.TiempoEstimado);
    }

    

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(50.01)]
    [InlineData(100)]
    public void RegistrarEnvio_PesoInvalido_LanzaExcepcion(double peso)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.RegistrarEnvio(1, 1, 2, peso, 5, 5, 10, 500, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(20.01)]
    public void RegistrarEnvio_AltoInvalido_LanzaExcepcion(double alto)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.RegistrarEnvio(1, 1, 2, 10, alto, 5, 10, 500, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(25)]
    public void RegistrarEnvio_AnchoInvalido_LanzaExcepcion(double ancho)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.RegistrarEnvio(1, 1, 2, 10, 5, ancho, 10, 500, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(30)]
    public void RegistrarEnvio_LargoInvalido_LanzaExcepcion(double largo)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.RegistrarEnvio(1, 1, 2, 10, 5, 5, largo, 500, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(-0.01)]
    public void RegistrarEnvio_DistanciaInvalida_LanzaExcepcion(double distancia)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.RegistrarEnvio(1, 1, 2, 10, 5, 5, 10, distancia, 1));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(-1)]
    [InlineData(99)]
    public void RegistrarEnvio_ModalidadInvalida_LanzaExcepcion(int modalidad)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.RegistrarEnvio(1, 1, 2, 10, 5, 5, 10, 500, modalidad));
    }



    [Fact]
    public void CambiarEstado_EstadoValido_LlamaRepositorio()
    {
        _mockRepo.Setup(r => r.CambiarEstado(It.IsAny<int>(), It.IsAny<string>()));

        _servicio.CambiarEstado(1, "EN_PROCESO");

        _mockRepo.Verify(r => r.CambiarEstado(1, "EN_PROCESO"), Times.Once);
    }

    [Fact]
    public void CambiarEstado_NormalizaAMayusculas()
    {
        _mockRepo.Setup(r => r.CambiarEstado(It.IsAny<int>(), It.IsAny<string>()));

        _servicio.CambiarEstado(1, "en_proceso");

        _mockRepo.Verify(r => r.CambiarEstado(1, "EN_PROCESO"), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CambiarEstado_EstadoVacio_LanzaExcepcion(string? estado)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.CambiarEstado(1, estado!));
    }

    [Theory]
    [InlineData("INVALIDO")]
    [InlineData("ENVIADO")]
    [InlineData("OK")]
    public void CambiarEstado_EstadoDesconocido_LanzaExcepcion(string estado)
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.CambiarEstado(1, estado));
    }

    [Theory]
    [InlineData("PENDIENTE")]
    [InlineData("EN_PROCESO")]
    [InlineData("ENTREGADO")]
    [InlineData("CANCELADO")]
    public void CambiarEstado_EstadosPermitidos_NoLanzaExcepcion(string estado)
    {
        _mockRepo.Setup(r => r.CambiarEstado(It.IsAny<int>(), It.IsAny<string>()));

        var exception = Record.Exception(() => _servicio.CambiarEstado(1, estado));
        Assert.Null(exception);
    }

   

    [Fact]
    public void CancelarEnvio_LlamaRepositorio()
    {
        _mockRepo.Setup(r => r.CancelarEnvio(It.IsAny<int>()));

        _servicio.CancelarEnvio(42);

        _mockRepo.Verify(r => r.CancelarEnvio(42), Times.Once);
    }

  

    [Fact]
    public void ObtenerPorId_DelegaAlRepositorio()
    {
        _mockRepo.Setup(r => r.ObtenerPorId(5)).Returns((Envio?)null);

        var resultado = _servicio.ObtenerPorId(5);

        Assert.Null(resultado);
        _mockRepo.Verify(r => r.ObtenerPorId(5), Times.Once);
    }

    [Fact]
    public void ListarTodos_DelegaAlRepositorio()
    {
        _mockRepo.Setup(r => r.ObtenerTodos()).Returns(new List<Envio>());

        var resultado = _servicio.ListarTodos();

        Assert.Empty(resultado);
        _mockRepo.Verify(r => r.ObtenerTodos(), Times.Once);
    }

  

    [Fact]
    public void ConsultarResumen_FechaInicioMayorQueFin_LanzaExcepcion()
    {
        var desde = new DateTime(2026, 6, 1);
        var hasta = new DateTime(2026, 1, 1);

        Assert.Throws<ArgumentException>(() =>
            _servicio.ConsultarResumen(desde, hasta));
    }

    [Fact]
    public void ConsultarCostos_FechaInicioMayorQueFin_LanzaExcepcion()
    {
        Assert.Throws<ArgumentException>(() =>
            _servicio.ConsultarCostos(new DateTime(2026, 12, 1), new DateTime(2026, 1, 1)));
    }

    [Fact]
    public void ConsultarResumen_FechasValidas_LlamaRepositorio()
    {
        var desde = new DateTime(2026, 1, 1);
        var hasta = new DateTime(2026, 6, 1);

        _mockRepo
            .Setup(r => r.ObtenerResumenEnvios(desde, hasta))
            .Returns(new List<dynamic>());

        var resultado = _servicio.ConsultarResumen(desde, hasta);

        Assert.NotNull(resultado);
        _mockRepo.Verify(r => r.ObtenerResumenEnvios(desde, hasta), Times.Once);
    }

    [Fact]
    public void ConsultarFacturacion_FechasValidas_LlamaRepositorio()
    {
        var desde = new DateTime(2026, 1, 1);
        var hasta = new DateTime(2026, 12, 31);

        _mockRepo
            .Setup(r => r.ObtenerFacturacionPorModalidad(desde, hasta))
            .Returns(new List<dynamic>());

        var resultado = _servicio.ConsultarFacturacion(desde, hasta);

        Assert.NotNull(resultado);
        _mockRepo.Verify(r => r.ObtenerFacturacionPorModalidad(desde, hasta), Times.Once);
    }
}