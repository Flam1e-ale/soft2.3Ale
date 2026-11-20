namespace API;

public interface ICalculadorModalidad
{
    decimal CalcularCosto(decimal peso, decimal distancia);
    int CalcularDias();
}
public class CalculadorEstandar : ICalculadorModalidad
{
    public decimal CalcularCosto(decimal peso, decimal distancia)
    {
        return 1000 + (peso * 200) + (distancia * 10);
    }

    public int CalcularDias()
    {
        return 5;
    }
}
public class CalculadorExpress : ICalculadorModalidad
{
    public decimal CalcularCosto(decimal peso, decimal distancia)
    {
        return 2000 + (peso * 350) + (distancia * 20);
    }

    public int CalcularDias()
    {
        return 2;
    }
}