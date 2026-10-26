using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Muestra reportes dinámicos generados en segundo plano con datos reales de la base.
/// </summary>
public partial class UcReportes : UserControl
{
    private readonly ReporteServicio reporteServicio;
    private CancellationTokenSource? cancelacion;

    public UcReportes()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGenerar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        EstiloControles.ConfigurarTabla(tablaVeterinarios);
        EstiloControles.ConfigurarTabla(tablaEspecies);
        dtpDesde.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpHasta.Value = DateTime.Today;
        reporteServicio = null!;
    }

    public UcReportes(ReporteServicio reporteServicio) : this()
    {
        this.reporteServicio = reporteServicio;
        Load += async (_, _) => await GenerarAsync();
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        cancelacion?.Cancel();
        base.OnHandleDestroyed(e);
    }

    private async void BtnGenerar_Click(object? sender, EventArgs e)
    {
        await GenerarAsync();
    }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        cancelacion?.Cancel();
    }

    private async Task GenerarAsync()
    {
        cancelacion?.Cancel();
        CancellationTokenSource actual = new();
        cancelacion = actual;

        EstablecerOcupado(true);
        lblEstado.Text = "Generando reporte...";

        try
        {
            var (veterinarios, especies) = await reporteServicio.GenerarAsync(dtpDesde.Value, dtpHasta.Value, actual.Token);
            if (IsDisposed)
            {
                return;
            }

            tablaVeterinarios.DataSource = veterinarios;
            tablaEspecies.DataSource = especies;
            EstiloControles.FormatoMoneda(tablaVeterinarios, "Ingresos");
            EstiloControles.FormatoMoneda(tablaEspecies, "Ingresos");
            graficoEspecies.AsignarDatos(especies.Select(e => new PuntoGrafico { Etiqueta = e.Especie, Valor = e.Mascotas }));

            // El total sale de las especies porque incluye las vacunas, que no tienen veterinario asignado.
            lblIngresos.Text = $"Ingresos: {Moneda.Formatear(especies.Sum(e => e.Ingresos))}";
            lblEstado.Text = $"Reporte generado: {veterinarios.Sum(v => v.Consultas)} consultas y "
                + $"{veterinarios.Sum(v => v.Citas)} citas entre {dtpDesde.Value:dd/MM/yyyy} y {dtpHasta.Value:dd/MM/yyyy}.";
        }
        catch (OperationCanceledException)
        {
            if (!IsDisposed && ReferenceEquals(cancelacion, actual))
            {
                lblEstado.Text = "La generación del reporte fue cancelada.";
            }
        }
        catch (ArgumentException excepcion)
        {
            lblEstado.Text = excepcion.Message;
        }
        catch (Exception excepcion) when (!IsDisposed)
        {
            lblEstado.Text = "No fue posible generar el reporte.";
            Mensajes.MostrarError(excepcion);
        }
        finally
        {
            if (!IsDisposed && ReferenceEquals(cancelacion, actual))
            {
                EstablecerOcupado(false);
            }
        }
    }

    private void EstablecerOcupado(bool ocupado)
    {
        btnGenerar.Enabled = !ocupado;
        btnCancelar.Enabled = ocupado;
        dtpDesde.Enabled = !ocupado;
        dtpHasta.Enabled = !ocupado;
        barraProgreso.Visible = ocupado;
    }
}
