using System.ComponentModel;
using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Permite registrar consultas, tratamientos y revisar el historial.
/// </summary>
public partial class UcConsultas : UserControl
{
    private readonly ConsultaServicio consultaServicio;
    private readonly MascotaServicio mascotaServicio;
    private readonly VeterinarioServicio veterinarioServicio;
    private readonly TarifaServicio tarifaServicio;
    private readonly BindingList<Tratamiento> tratamientos = [];

    public UcConsultas()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnAgregarTratamiento);
        EstiloControles.ConfigurarBotonSecundario(btnQuitarTratamiento);
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarTabla(tablaTratamientos);
        EstiloControles.ConfigurarTabla(tablaHistorial);
        consultaServicio = null!;
        mascotaServicio = null!;
        veterinarioServicio = null!;
        tarifaServicio = null!;
    }

    public UcConsultas(
        ConsultaServicio consultaServicio,
        MascotaServicio mascotaServicio,
        VeterinarioServicio veterinarioServicio,
        TarifaServicio tarifaServicio) : this()
    {
        this.consultaServicio = consultaServicio;
        this.mascotaServicio = mascotaServicio;
        this.veterinarioServicio = veterinarioServicio;
        this.tarifaServicio = tarifaServicio;
        tablaTratamientos.DataSource = tratamientos;
        // Agregar o quitar tratamientos recalcula el total automáticamente.
        tratamientos.ListChanged += (_, _) => ActualizarTotal();
        EstiloControles.OcultarColumnas(tablaTratamientos, "IdTratamiento", "IdConsulta");
        EstiloControles.FormatoMoneda(tablaTratamientos, "Precio");
        Load += UcConsultas_Load;
    }

    private void UcConsultas_Load(object? sender, EventArgs e)
    {
        try
        {
            cboMascota.DataSource = mascotaServicio.Listar(string.Empty, true);
            cboMascota.DisplayMember = "Nombre"; cboMascota.ValueMember = "IdMascota"; cboMascota.SelectedIndex = -1;
            cboVeterinario.DataSource = veterinarioServicio.Listar(string.Empty, true);
            cboVeterinario.DisplayMember = "NombreCompleto"; cboVeterinario.ValueMember = "IdVeterinario"; cboVeterinario.SelectedIndex = -1;
            cboServicio.DataSource = tarifaServicio.Listar(Tarifa.Servicio);
            cboServicio.DisplayMember = "Nombre"; cboServicio.SelectedIndex = -1;
            cboMedicamento.DataSource = tarifaServicio.Listar(Tarifa.Medicamento);
            cboMedicamento.DisplayMember = "Nombre"; cboMedicamento.SelectedIndex = -1;
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void CboMascota_SelectedIndexChanged(object? sender, EventArgs e) => CargarHistorial();

    private void BtnAgregarTratamiento_Click(object? sender, EventArgs e)
    {
        Tarifa? medicamento = cboMedicamento.SelectedItem as Tarifa;
        ResultadoOperacion validacion = new Validador()
            .Seleccionado(medicamento?.Nombre, "Medicamento")
            .Requerido(txtDosis.Text, "Dosis")
            .LongitudMaxima(txtDosis.Text, 80, "Dosis")
            .LongitudMaxima(txtFrecuencia.Text, 100, "Frecuencia")
            .LongitudMaxima(txtDuracion.Text, 80, "Duración")
            .LongitudMaxima(txtIndicaciones.Text, 300, "Indicaciones")
            .Resultado("No se pudo agregar el tratamiento.");

        if (!validacion.Exitoso || medicamento is null)
        {
            MessageBox.Show(validacion.Mensaje, "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        tratamientos.Add(new Tratamiento
        {
            Medicamento = medicamento.Nombre,
            Precio = medicamento.Precio,
            Dosis = txtDosis.Text,
            Frecuencia = txtFrecuencia.Text,
            Duracion = txtDuracion.Text,
            Indicaciones = txtIndicaciones.Text
        });
        cboMedicamento.SelectedIndex = -1; txtDosis.Clear(); txtFrecuencia.Clear(); txtDuracion.Clear(); txtIndicaciones.Clear();
    }

    private void BtnQuitarTratamiento_Click(object? sender, EventArgs e)
    {
        if (tablaTratamientos.CurrentRow?.DataBoundItem is Tratamiento tratamiento)
        {
            tratamientos.Remove(tratamiento);
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        Consulta consulta = new()
        {
            IdMascota = cboMascota.SelectedValue is int idMascota ? idMascota : 0,
            IdVeterinario = cboVeterinario.SelectedValue is int idVeterinario ? idVeterinario : 0,
            FechaHora = dtpFecha.Value.Date.Add(dtpHora.Value.TimeOfDay),
            Servicio = cboServicio.SelectedItem is Tarifa servicio ? servicio.Nombre : string.Empty,
            Precio = PrecioServicio(),
            Motivo = txtMotivo.Text,
            Diagnostico = txtDiagnostico.Text,
            Observaciones = txtObservaciones.Text,
            ListaTratamientos = tratamientos.ToList()
        };

        Mensajes.EjecutarOperacion(() => consultaServicio.Guardar(consulta), () =>
        {
            cboServicio.SelectedIndex = -1;
            txtMotivo.Clear(); txtDiagnostico.Clear(); txtObservaciones.Clear(); tratamientos.Clear();
            CargarHistorial();
        });
    }

    private void CboServicio_SelectedIndexChanged(object? sender, EventArgs e) => ActualizarTotal();

    private decimal PrecioServicio() => cboServicio.SelectedItem is Tarifa servicio ? servicio.Precio : 0;

    private void ActualizarTotal()
    {
        decimal total = PrecioServicio() + tratamientos.Sum(t => t.Precio);
        lblTotal.Text = $"Total de la consulta: {Moneda.Formatear(total)}";
    }

    private void CargarHistorial()
    {
        if (cboMascota.SelectedValue is not int idMascota) return;
        try
        {
            tablaHistorial.DataSource = consultaServicio.ListarPorMascota(idMascota);
            EstiloControles.OcultarColumnas(tablaHistorial, "IdConsulta", "IdMascota", "IdVeterinario");
            EstiloControles.FormatoMoneda(tablaHistorial, "Precio", "Total");
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }
}
