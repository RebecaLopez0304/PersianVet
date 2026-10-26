using PersianVet.Formularios;
using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Permite programar, consultar y cancelar citas.
/// </summary>
public partial class UcCitas : UserControl
{
    private readonly CitaServicio citaServicio;
    private readonly MascotaServicio mascotaServicio;
    private readonly VeterinarioServicio veterinarioServicio;
    private readonly TarifaServicio tarifaServicio;
    private int idSeleccionado;

    public UcCitas()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnProgramar);
        EstiloControles.ConfigurarBotonSecundario(btnReprogramar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        EstiloControles.ConfigurarBotonSecundario(btnFiltrar);
        EstiloControles.ConfigurarBotonSecundario(btnCalendario);
        EstiloControles.ConfigurarTabla(tablaCitas);
        citaServicio = null!;
        mascotaServicio = null!;
        veterinarioServicio = null!;
        tarifaServicio = null!;
    }

    public UcCitas(
        CitaServicio citaServicio,
        MascotaServicio mascotaServicio,
        VeterinarioServicio veterinarioServicio,
        TarifaServicio tarifaServicio) : this()
    {
        this.citaServicio = citaServicio;
        this.mascotaServicio = mascotaServicio;
        this.veterinarioServicio = veterinarioServicio;
        this.tarifaServicio = tarifaServicio;
        Load += UcCitas_Load;
    }

    private void UcCitas_Load(object? sender, EventArgs e)
    {
        CargarListas();
        CargarCitas();
    }

    private void BtnProgramar_Click(object? sender, EventArgs e) => Ejecutar(() => citaServicio.Guardar(CrearCita()));
    private void BtnReprogramar_Click(object? sender, EventArgs e) => Ejecutar(() => citaServicio.Actualizar(CrearCita()));
    private void BtnFiltrar_Click(object? sender, EventArgs e) => CargarCitas();

    private void BtnCalendario_Click(object? sender, EventArgs e)
    {
        using FrmCalendarioCitas calendario = new(citaServicio);
        calendario.ShowDialog(this);
    }
    private void CboServicio_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cboServicio.SelectedItem is Tarifa servicio)
        {
            lblPrecioValor.Text = Moneda.Formatear(servicio.Precio);
        }
    }

    private void ChkFiltrar_CheckedChanged(object? sender, EventArgs e) { dtpFiltro.Enabled = chkFiltrar.Checked; CargarCitas(); }

    private void BtnCancelar_Click(object? sender, EventArgs e)
    {
        if (idSeleccionado <= 0)
        {
            MessageBox.Show("Seleccione una cita de la lista.", "PersianVet");
            return;
        }

        if (MessageBox.Show("¿Desea cancelar la cita?", "PersianVet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { citaServicio.Cancelar(idSeleccionado); Limpiar(); CargarCitas(); }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void TablaCitas_SelectionChanged(object? sender, EventArgs e)
    {
        if (tablaCitas.CurrentRow?.DataBoundItem is not Cita cita) return;
        idSeleccionado = cita.IdCita;
        cboMascota.SelectedValue = cita.IdMascota;
        cboVeterinario.SelectedValue = cita.IdVeterinario;
        dtpFecha.Value = cita.FechaHora.Date;
        dtpHora.Value = DateTime.Today.Add(cita.FechaHora.TimeOfDay);
        cboServicio.SelectedValue = cita.Servicio;
        lblPrecioValor.Text = Moneda.Formatear(cita.Precio);
        txtMotivo.Text = cita.Motivo;
        cboEstado.Text = cita.Estado;
    }

    private Cita CrearCita()
    {
        return new Cita
        {
            IdCita = idSeleccionado,
            IdMascota = cboMascota.SelectedValue is int idMascota ? idMascota : 0,
            IdVeterinario = cboVeterinario.SelectedValue is int idVeterinario ? idVeterinario : 0,
            FechaHora = dtpFecha.Value.Date.Add(dtpHora.Value.TimeOfDay),
            Servicio = cboServicio.SelectedItem is Tarifa servicio ? servicio.Nombre : string.Empty,
            Precio = cboServicio.SelectedItem is Tarifa tarifa ? tarifa.Precio : 0,
            Motivo = txtMotivo.Text,
            Estado = cboEstado.Text
        };
    }

    private void Ejecutar(Func<ResultadoOperacion> operacion)
    {
        Mensajes.EjecutarOperacion(operacion, () => { Limpiar(); CargarCitas(); });
    }

    private void CargarListas()
    {
        cboMascota.DataSource = mascotaServicio.Listar(string.Empty, true);
        cboMascota.DisplayMember = "Nombre"; cboMascota.ValueMember = "IdMascota"; cboMascota.SelectedIndex = -1;
        cboVeterinario.DataSource = veterinarioServicio.Listar(string.Empty, true);
        cboVeterinario.DisplayMember = "NombreCompleto"; cboVeterinario.ValueMember = "IdVeterinario"; cboVeterinario.SelectedIndex = -1;
        cboServicio.DataSource = tarifaServicio.Listar(Tarifa.Servicio);
        cboServicio.DisplayMember = "Nombre"; cboServicio.ValueMember = "Nombre"; cboServicio.SelectedIndex = -1;
    }

    private void CargarCitas()
    {
        try
        {
            tablaCitas.DataSource = citaServicio.Listar(chkFiltrar.Checked ? dtpFiltro.Value.Date : null);
            EstiloControles.OcultarColumnas(tablaCitas, "IdMascota", "IdVeterinario");
            EstiloControles.FormatoMoneda(tablaCitas, "Precio");
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void Limpiar()
    {
        idSeleccionado = 0; cboMascota.SelectedIndex = -1; cboVeterinario.SelectedIndex = -1;
        dtpFecha.Value = DateTime.Today.AddDays(1); dtpHora.Value = DateTime.Today.AddHours(9);
        cboServicio.SelectedIndex = -1; lblPrecioValor.Text = Moneda.Formatear(0);
        txtMotivo.Clear(); cboEstado.SelectedIndex = 0; tablaCitas.ClearSelection();
    }
}
