using System.Globalization;
using System.Text;
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
    private List<ReporteVeterinario> veterinarios = [];
    private List<ReporteEspecie> especies = [];

    public UcReportes()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGenerar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        EstiloControles.ConfigurarBotonSecundario(btnExportar);
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
            var resultado = await reporteServicio.GenerarAsync(dtpDesde.Value, dtpHasta.Value, actual.Token);
            if (IsDisposed)
            {
                return;
            }

            veterinarios = resultado.Veterinarios;
            especies = resultado.Especies;

            tablaVeterinarios.DataSource = veterinarios;
            tablaEspecies.DataSource = especies;
            graficoEspecies.AsignarDatos(especies.Select(e => new PuntoGrafico { Etiqueta = e.Especie, Valor = e.Mascotas }));
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
        btnExportar.Enabled = !ocupado && veterinarios.Count > 0;
        dtpDesde.Enabled = !ocupado;
        dtpHasta.Enabled = !ocupado;
        barraProgreso.Visible = ocupado;
    }

    private async void BtnExportar_Click(object? sender, EventArgs e)
    {
        using SaveFileDialog dialogo = new()
        {
            Filter = "Archivo CSV (*.csv)|*.csv",
            FileName = $"reporte-persianvet-{DateTime.Today:yyyyMMdd}.csv"
        };

        if (dialogo.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        try
        {
            btnExportar.Enabled = false;
            string ruta = dialogo.FileName;
            string contenido = ConstruirCsv();
            // La escritura en disco se hace en segundo plano para no bloquear la interfaz.
            await Task.Run(() => File.WriteAllTextAsync(ruta, contenido, new UTF8Encoding(true)));
            lblEstado.Text = $"Reporte exportado a {ruta}.";
        }
        catch (Exception)
        {
            lblEstado.Text = "No fue posible exportar el archivo.";
            MessageBox.Show("No fue posible guardar el archivo.", "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                btnExportar.Enabled = veterinarios.Count > 0;
            }
        }
    }

    private string ConstruirCsv()
    {
        CultureInfo cultura = CultureInfo.InvariantCulture;
        StringBuilder csv = new();
        csv.AppendLine($"Periodo,{dtpDesde.Value.ToString("yyyy-MM-dd", cultura)},{dtpHasta.Value.ToString("yyyy-MM-dd", cultura)}");
        csv.AppendLine();
        csv.AppendLine("Veterinario,Especialidad,Citas,Completadas,Canceladas,Consultas,Tratamientos");
        foreach (ReporteVeterinario fila in veterinarios)
        {
            csv.AppendLine($"{Escapar(fila.Veterinario)},{Escapar(fila.Especialidad)},{fila.Citas},{fila.Completadas},{fila.Canceladas},{fila.Consultas},{fila.Tratamientos}");
        }

        csv.AppendLine();
        csv.AppendLine("Especie,Mascotas,Consultas,Vacunas");
        foreach (ReporteEspecie fila in especies)
        {
            csv.AppendLine($"{Escapar(fila.Especie)},{fila.Mascotas},{fila.Consultas},{fila.Vacunas}");
        }

        return csv.ToString();
    }

    private static string Escapar(string valor)
    {
        return valor.Contains(',') || valor.Contains('"')
            ? $"\"{valor.Replace("\"", "\"\"")}\""
            : valor;
    }
}
