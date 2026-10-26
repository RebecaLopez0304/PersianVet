namespace PersianVet.Modelos;

/// <summary>
/// Aviso mostrado en el panel "Requiere atención" del dashboard.
/// </summary>
public class Alerta
{
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el módulo que se abre al hacer doble clic sobre el aviso.
    /// </summary>
    public string Modulo { get; set; } = "Inicio";

    public override string ToString() => $"{Tipo}: {Mensaje}";
}
