namespace PersianVet.Modelos;

/// <summary>
/// Indica si una operación terminó correctamente.
/// </summary>
public class ResultadoOperacion
{
    public bool Exitoso { get; private set; }
    public string Mensaje { get; private set; } = string.Empty;

    public static ResultadoOperacion Correcto(string mensaje)
    {
        return new ResultadoOperacion { Exitoso = true, Mensaje = mensaje };
    }

    public static ResultadoOperacion Error(string mensaje)
    {
        return new ResultadoOperacion { Exitoso = false, Mensaje = mensaje };
    }
}

