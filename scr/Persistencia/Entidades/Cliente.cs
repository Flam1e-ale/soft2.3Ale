namespace Persistencia.entidades;

    public class Cliente
    {
    public int Id { get; set; }
    private string nombre = null!;  
    public string Nombre
    {
        get => nombre;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("nombre invalido");
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
          if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("email invalido");
            }
             email = value;
        }
        
    }


    public Cliente(string nombre, string email)
    {
        Nombre = nombre;
        Email = email;
    }
    }