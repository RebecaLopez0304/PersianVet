using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Un registro donde se cobró una tarifa: muestra el precio que realmente se guardó en ese momento.
/// </summary>
public class CobroTarifa
{
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Obtiene o establece el tipo de registro: Cita, Consulta, Tratamiento o Vacuna.
    /// </summary>
    public string Registro { get; set; } = string.Empty;

    public string Mascota { get; set; } = string.Empty;

    [DisplayName("Precio cobrado")]
    public decimal Precio { get; set; }
}
