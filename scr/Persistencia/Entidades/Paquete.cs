using System.IO.Pipes;

namespace Persistencia.entidades;

    public class Paquete
    {
    
    public int Id { get; set; }

    private  double peso;
    public double Peso 
    {
        get=> peso;
        
        set
        {
             if (value < 0 || value > 5000)
            {
                throw new ArgumentOutOfRangeException("peso invalido");
            }
        }
         }
    
    public double Alto { get; set; }

    public double Ancho { get; set; }

    public double Largo { get; set; }

    public Paquete(double peso, double alto, double ancho, double largo)
    {
        Alto=alto;
        Peso=peso;
        Ancho=ancho;
        Largo=largo;
    }
    
    }   
