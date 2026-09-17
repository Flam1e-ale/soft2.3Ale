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
             if (value < 0 || value > 50)
            {
                throw new ArgumentOutOfRangeException("peso invalido");
            }
        }
         }
    
    private double alto; 
    public double Alto { 
        get => alto;
        set
        {
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("meidida invalida");
            }
        }
         }

    private double ancho;
    public double Ancho { 
        get=> ancho;
        set
        {
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("medida invalida");
            }
        }
         }
    private double largo;
    public double Largo { 
        get=> largo;
        set
        {
            if (value < 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("medida invalida");
            }
        }
         }

    public Paquete(double peso, double alto, double ancho, double largo)
    {
        Alto=alto;
        Peso=peso;
        Ancho=ancho;
        Largo=largo;
    }
    
    }   
