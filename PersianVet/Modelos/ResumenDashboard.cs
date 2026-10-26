namespace PersianVet.Modelos;

/// <summary>
/// Guarda las cantidades que se muestran en el inicio.
/// </summary>
public class ResumenDashboard
{
    public int MascotasRegistradas { get; set; }
    public int CitasHoy { get; set; }
    public int VacunasProximas { get; set; }
    public int ConsultasMes { get; set; }

    /// <summary>
    /// Obtiene o establece lo cobrado en C$ este mes por consultas, tratamientos y vacunas.
    /// </summary>
    public decimal IngresosMes { get; set; }
}

