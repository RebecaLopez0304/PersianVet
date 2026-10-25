using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Muestra un resumen general de la veterinaria.
/// </summary>
public partial class UcDashboard : UserControl
{
    private readonly DashboardServicio dashboardServicio;
    private CancellationTokenSource? cancelacion;

    public UcDashboard()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonSecundario(btnActualizar);
        EstiloControles.ConfigurarTabla(tablaCitas);
        dashboardServicio = null!;
    }

    public UcDashboard(DashboardServicio dashboardServicio) : this()
    {
        this.dashboardServicio = dashboardServicio;
        Load += UcDashboard_Load;
    }

    private async void UcDashboard_Load(object? sender, EventArgs e)
    {
        await CargarDatosAsync();
    }

    private async void BtnActualizar_Click(object? sender, EventArgs e)
    {
        await CargarDatosAsync();
    }

    /// <summary>
    /// Cancela la carga en curso si el usuario cambia de módulo.
    /// </summary>
    protected override void OnHandleDestroyed(EventArgs e)
    {
        cancelacion?.Cancel();
        base.OnHandleDestroyed(e);
    }

    private async Task CargarDatosAsync()
    {
        cancelacion?.Cancel();
        CancellationTokenSource actual = new();
        cancelacion = actual;

        btnActualizar.Enabled = false;
        lblEstado.Text = "Cargando información...";

        try
        {
            // El resumen y las citas se consultan al mismo tiempo, fuera del hilo de la interfaz.
            Task<ResumenDashboard> tareaResumen = dashboardServicio.ObtenerResumenAsync(actual.Token);
            Task<List<Cita>> tareaCitas = dashboardServicio.ListarProximasCitasAsync(actual.Token);
            await Task.WhenAll(tareaResumen, tareaCitas);

            ResumenDashboard resumen = tareaResumen.Result;
            lblMascotasValor.Text = resumen.MascotasRegistradas.ToString();
            lblCitasValor.Text = resumen.CitasHoy.ToString();
            lblVacunasValor.Text = resumen.VacunasProximas.ToString();
            lblConsultasValor.Text = resumen.ConsultasMes.ToString();
            tablaCitas.DataSource = tareaCitas.Result;
            OcultarColumnas();
            lblEstado.Text = "La información está actualizada.";
        }
        catch (OperationCanceledException)
        {
            // Otra carga o un cambio de módulo reemplazó esta operación.
            return;
        }
        catch (Exception excepcion)
        {
            lblEstado.Text = "No fue posible cargar el resumen.";
            Mensajes.MostrarError(excepcion);
        }
        finally
        {
            if (ReferenceEquals(cancelacion, actual) && !IsDisposed)
            {
                btnActualizar.Enabled = true;
            }
        }
    }

    private void OcultarColumnas()
    {
        string[] columnas = ["IdCita", "IdMascota", "IdVeterinario"];
        foreach (string nombre in columnas)
        {
            if (tablaCitas.Columns.Contains(nombre))
            {
                tablaCitas.Columns[nombre].Visible = false;
            }
        }
    }
}
