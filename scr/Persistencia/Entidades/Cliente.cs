namespace Persistencia.Entidades;

    public class Cliente
    {
    public int IdCliente { get; set; }
    private string nombre = null!;  
    public string Nombre
    {
        get => nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("nombre invalido");
            }
            nombre = value;
        }
    }

    private string email = null!;
    public string Email
    {
        get => email;
        set
        {
          if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("email invalido");
            }
             email = value;
        }
        
    }


    public Cliente(string nombre, string email)
    {
        Nombre = nombre;
        Email = email;
    }
    public Cliente()
    {
        
    }
    }