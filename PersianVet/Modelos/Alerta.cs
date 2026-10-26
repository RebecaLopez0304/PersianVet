using System.ComponentModel;

namespace PersianVet.Modelos;

/// <summary>
/// Aviso mostrado en la tabla "Requiere atención" del dashboard.
/// </summary>
public class Alerta
{
    [DisplayName("Aviso")]
    public string Tipo { get; set; } = string.Empty;

    public string Mascota { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la vacuna, el servicio de la cita o la explicación del aviso.
    /// </summary>
    public string Detalle { get; set; } = string.Empty;

    public DateTime? Fecha { get; set; }

    /// <summary>
    /// Obtiene o establece el módulo que se abre al hacer doble clic sobre el aviso.
    /// </summary>
    [Browsable(false)]
    public string Modulo { get; set; } = "Inicio";
}
