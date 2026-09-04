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

    private void UcDashboard_Load(object? sender, EventArgs e)
    {
        CargarDatos();
    }

    private void BtnActualizar_Click(object? sender, EventArgs e)
    {
        CargarDatos();
    }

    private void CargarDatos()
    {
        try
        {
            ResumenDashboard resumen = dashboardServicio.ObtenerResumen();
            lblMascotasValor.Text = resumen.MascotasRegistradas.ToString();
            lblCitasValor.Text = resumen.CitasHoy.ToString();
            lblVacunasValor.Text = resumen.VacunasProximas.ToString();
            lblConsultasValor.Text = resumen.ConsultasMes.ToString();
            tablaCitas.DataSource = dashboardServicio.ListarProximasCitas();
            OcultarColumnas();
            lblEstado.Text = "La información está actualizada.";
        }
        catch (Exception excepcion)
        {
            lblEstado.Text = "No fue posible cargar el resumen.";
            Mensajes.MostrarError(excepcion);
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
