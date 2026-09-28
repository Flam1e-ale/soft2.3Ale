namespace Persistencia.Entidades;
    public class Direccion
    {
    public int Id { get; set; }

    private string calle =null!;
     public string Calle 
    {
        get => calle;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("email invalido");
            }
             calle = value;
        }

    }

    private int numero;
    public int Numero
    {
        get
        {
            return numero;
        }
        set
        {
           if (value < 0 || value > 5000)
            {
                throw new ArgumentOutOfRangeException("direccion invalida");
            }

            
            numero = value;
        }
    } 
private string ciudad=null!;   
  public string Ciudad 
    {
        get => ciudad;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("ciudad invalida");
            }
             ciudad = value;
        }

    }

    public Direccion(string calle, int numero, string ciudad)
    {
        Calle = calle;
        Numero = numero;
        Ciudad = ciudad;
    }
    }
