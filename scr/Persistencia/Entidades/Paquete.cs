using System.IO.Pipes;

namespace Persistencia.Entidades;
    public class Paquete
    {
    
    public int IdPaquete { get; set; }

    private  double peso;
    public double Peso 
    {
        get=> peso;
        
        set
        {
             if (value <= 0 || value > 50)
            {
                throw new ArgumentOutOfRangeException("peso invalido");
            }
            peso=value;
        }
         }
    
    private double alto; 
    public double Alto { 
        get => alto;
        set
        {
            if (value <= 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("meidida invalida");
            }
            alto=value;
        }
         }

    private double ancho;
    public double Ancho { 
        get=> ancho;
        set
        {
            if (value <= 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("medida invalida");
            }
            ancho=value;
        }
         }
    private double largo;
    public double Largo { 
        get=> largo;
        set
        {
            if (value <= 0 || value > 20)
            {
                throw new ArgumentOutOfRangeException("medida invalida");
            }
            largo=value;
        }
         }

    public Paquete(double peso, double alto, double ancho, double largo)
    {
        Alto=alto;
        Peso=peso;
        Ancho=ancho;
        Largo=largo;
    }
    public Paquete() { }
    }   
