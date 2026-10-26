using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Muestra las coincidencias de la búsqueda global y permite abrir una.
/// </summary>
public partial class FrmBusquedaGlobal : Form
{
    private readonly BusquedaServicio busquedaServicio;
    private readonly string texto;
    private readonly CancellationTokenSource cancelacion = new();

    /// <summary>
    /// Obtiene el resultado elegido por el usuario, o <see langword="null"/> si cerró la ventana sin elegir.
    /// </summary>
    public ResultadoBusqueda? Seleccion { get; private set; }

    public FrmBusquedaGlobal() : this(null!, string.Empty)
    {
    }

    public FrmBusquedaGlobal(BusquedaServicio busquedaServicio, string texto)
    {
        InitializeComponent();
        this.busquedaServicio = busquedaServicio;
        this.texto = texto;
        BackColor = Tema.Fondo;
        EstiloControles.ConfigurarTabla(tablaResultados);
        EstiloControles.ConfigurarBotonPrincipal(btnAbrir);
        EstiloControles.ConfigurarBotonSecundario(btnCerrar);
        lblTitulo.Text = $"Resultados para \"{texto}\"";
    }

    private async void FrmBusquedaGlobal_Load(object? sender, EventArgs e)
    {
        lblEstado.Text = "Buscando...";
        try
        {
            List<ResultadoBusqueda> resultados = await busquedaServicio.BuscarAsync(texto, cancelacion.Token);
            tablaResultados.DataSource = resultados;
            EstiloControles.OcultarColumnas(tablaResultados, "Id");
            tablaResultados.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            tablaResultados.Columns["Tipo"].FillWeight = 60;
            tablaResultados.Columns["Descripcion"].FillWeight = 150;

            lblEstado.Text = resultados.Count == 0
                ? "No se encontraron coincidencias."
                : $"{resultados.Count} coincidencias. Haga doble clic para abrir.";
        }
        catch (OperationCanceledException)
        {
            // La ventana se cerró antes de terminar la búsqueda.
        }
        catch (Exception excepcion)
        {
            lblEstado.Text = "No fue posible realizar la búsqueda.";
            Mensajes.MostrarError(excepcion);
        }
    }

    private void FrmBusquedaGlobal_FormClosing(object? sender, FormClosingEventArgs e)
    {
        cancelacion.Cancel();
    }

    private void BtnAbrir_Click(object? sender, EventArgs e)
    {
        Elegir();
    }

    private void TablaResultados_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            Elegir();
        }
    }

    private void Elegir()
    {
        if (tablaResultados.CurrentRow?.DataBoundItem is not ResultadoBusqueda resultado)
        {
            return;
        }

        Seleccion = resultado;
        DialogResult = DialogResult.OK;
        Close();
    }
}
