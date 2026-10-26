using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Muestra un resumen general de la veterinaria con accesos rápidos.
/// </summary>
public partial class UcDashboard : UserControl
{
    private readonly DashboardServicio dashboardServicio;
    private CancellationTokenSource? cancelacion;

    /// <summary>
    /// Se dispara cuando el usuario pide ir a otro módulo (tarjeta, botón o aviso).
    /// </summary>
    public event Action<string>? NavegarSolicitado;

    public UcDashboard()
    {
        InitializeComponent();
        EstiloControles.ConfigurarTabla(tablaCitas);
        ConfigurarBotones();
        HacerClicable(tarjetaMascotas, "Mascotas");
        HacerClicable(tarjetaCitas, "Citas");
        HacerClicable(tarjetaVacunas, "Vacunas");
        HacerClicable(tarjetaConsultas, "Consultas");
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
    /// Oculta las acciones y desactiva las tarjetas a las que el rol actual no tiene acceso.
    /// </summary>
    public void AplicarPermisos(Func<string, bool> permitido)
    {
        btnNuevaCita.Visible = permitido("Citas");
        btnCalendario.Visible = permitido("Calendario");
        btnNuevoCliente.Visible = permitido("NuevoCliente");
        btnNuevaMascota.Visible = permitido("NuevaMascota");
        btnNuevaConsulta.Visible = permitido("Consultas");
        btnNuevaVacuna.Visible = permitido("Vacunas");
        tarjetaMascotas.Enabled = permitido("Mascotas");
        tarjetaCitas.Enabled = permitido("Citas");
        tarjetaVacunas.Enabled = permitido("Vacunas");
        tarjetaConsultas.Enabled = permitido("Consultas");
    }

    private void Navegar(string destino)
    {
        NavegarSolicitado?.Invoke(destino);
    }

    private void ListaAlertas_DoubleClick(object? sender, EventArgs e)
    {
        if (listaAlertas.SelectedItem is Alerta alerta && alerta.Modulo != "Inicio")
        {
            Navegar(alerta.Modulo);
        }
    }

    private void ConfigurarBotones()
    {
        Button[] principales = [btnNuevaCita, btnNuevoCliente, btnNuevaMascota, btnNuevaConsulta, btnNuevaVacuna];
        foreach (Button boton in principales)
        {
            EstiloControles.ConfigurarBotonPrincipal(boton);
        }

        EstiloControles.ConfigurarBotonSecundario(btnCalendario);
        EstiloControles.ConfigurarBotonSecundario(btnActualizar);
    }

    /// <summary>
    /// Hace que toda la tarjeta (y sus etiquetas) responda al clic y muestre cursor de mano.
    /// </summary>
    private void HacerClicable(Control control, string destino)
    {
        control.Cursor = Cursors.Hand;
        control.Click += (_, _) => Navegar(destino);
        foreach (Control hijo in control.Controls)
        {
            HacerClicable(hijo, destino);
        }
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
            // Las cuatro consultas se lanzan a la vez y corren fuera del hilo de la interfaz.
            Task<ResumenDashboard> tareaResumen = dashboardServicio.ObtenerResumenAsync(actual.Token);
            Task<List<Cita>> tareaCitas = dashboardServicio.ListarProximasCitasAsync(actual.Token);
            Task<List<Alerta>> tareaAlertas = dashboardServicio.ListarAlertasAsync(actual.Token);
            Task<List<PuntoGrafico>> tareaGrafico = dashboardServicio.ContarCitasProximosDiasAsync(7, actual.Token);
            await Task.WhenAll(tareaResumen, tareaCitas, tareaAlertas, tareaGrafico);

            // Si el usuario ya cambió de módulo, no hay nada que actualizar.
            if (IsDisposed || actual.IsCancellationRequested)
            {
                return;
            }

            ResumenDashboard resumen = tareaResumen.Result;
            lblMascotasValor.Text = resumen.MascotasRegistradas.ToString();
            lblCitasValor.Text = resumen.CitasHoy.ToString();
            lblVacunasValor.Text = resumen.VacunasProximas.ToString();
            lblConsultasValor.Text = resumen.ConsultasMes.ToString();
            tablaCitas.DataSource = tareaCitas.Result;
            EstiloControles.OcultarColumnas(tablaCitas, "IdCita", "IdMascota", "IdVeterinario");

            listaAlertas.DataSource = tareaAlertas.Result;
            listaAlertas.ClearSelected();
            graficoCitas.AsignarDatos(tareaGrafico.Result);
            lblEstado.Text = "La información está actualizada.";
        }
        catch (OperationCanceledException)
        {
            // Otra carga o un cambio de módulo reemplazó esta operación.
            return;
        }
        catch (Exception excepcion) when (!IsDisposed && !actual.IsCancellationRequested)
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
}
