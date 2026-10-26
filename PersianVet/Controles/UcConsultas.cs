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
    }

    public UcConsultas(ConsultaServicio consultaServicio, MascotaServicio mascotaServicio, VeterinarioServicio veterinarioServicio) : this()
    {
        this.consultaServicio = consultaServicio;
        this.mascotaServicio = mascotaServicio;
        this.veterinarioServicio = veterinarioServicio;
        tablaTratamientos.DataSource = tratamientos;
        EstiloControles.OcultarColumnas(tablaTratamientos, "IdTratamiento", "IdConsulta");
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
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void CboMascota_SelectedIndexChanged(object? sender, EventArgs e) => CargarHistorial();

    private void BtnAgregarTratamiento_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMedicamento.Text) || string.IsNullOrWhiteSpace(txtDosis.Text))
        {
            MessageBox.Show("Escriba el medicamento y la dosis.", "PersianVet", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        tratamientos.Add(new Tratamiento
        {
            Medicamento = txtMedicamento.Text,
            Dosis = txtDosis.Text,
            Frecuencia = txtFrecuencia.Text,
            Duracion = txtDuracion.Text,
            Indicaciones = txtIndicaciones.Text
        });
        txtMedicamento.Clear(); txtDosis.Clear(); txtFrecuencia.Clear(); txtDuracion.Clear(); txtIndicaciones.Clear();
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
            Motivo = txtMotivo.Text,
            Diagnostico = txtDiagnostico.Text,
            Observaciones = txtObservaciones.Text,
            ListaTratamientos = tratamientos.ToList()
        };

        Mensajes.EjecutarOperacion(() => consultaServicio.Guardar(consulta), () =>
        {
            txtMotivo.Clear(); txtDiagnostico.Clear(); txtObservaciones.Clear(); tratamientos.Clear();
            CargarHistorial();
        });
    }

    private void CargarHistorial()
    {
        if (cboMascota.SelectedValue is not int idMascota) return;
        try
        {
            tablaHistorial.DataSource = consultaServicio.ListarPorMascota(idMascota);
            EstiloControles.OcultarColumnas(tablaHistorial, "IdConsulta", "IdMascota", "IdVeterinario");
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }
}
