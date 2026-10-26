using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Permite registrar vacunas y consultar las próximas dosis.
/// </summary>
public partial class UcVacunas : UserControl
{
    private readonly VacunaServicio vacunaServicio;
    private readonly MascotaServicio mascotaServicio;

    public UcVacunas()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnConsultar);
        EstiloControles.ConfigurarTabla(tablaVacunas);
        vacunaServicio = null!;
        mascotaServicio = null!;
    }

    public UcVacunas(VacunaServicio vacunaServicio, MascotaServicio mascotaServicio) : this()
    {
        this.vacunaServicio = vacunaServicio;
        this.mascotaServicio = mascotaServicio;
        Load += UcVacunas_Load;
    }

    private void UcVacunas_Load(object? sender, EventArgs e)
    {
        try
        {
            cboMascota.DataSource = mascotaServicio.Listar(string.Empty, true);
            cboMascota.DisplayMember = "Nombre";
            cboMascota.ValueMember = "IdMascota";
            cboMascota.SelectedIndex = -1;
            CargarVacunas();
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        Vacuna vacuna = new()
        {
            IdMascota = cboMascota.SelectedValue is int idMascota ? idMascota : 0,
            NombreVacuna = txtNombreVacuna.Text,
            FechaAplicacion = dtpAplicacion.Value.Date,
            ProximaDosis = chkProximaDosis.Checked ? dtpProximaDosis.Value.Date : null,
            Lote = txtLote.Text
        };

        Mensajes.EjecutarOperacion(() => vacunaServicio.Guardar(vacuna), () =>
        {
            txtNombreVacuna.Clear(); txtLote.Clear(); chkProximaDosis.Checked = false; CargarVacunas();
        });
    }

    private void CambiarFiltro(object? sender, EventArgs e)
    {
        dtpProximaDosis.Enabled = chkProximaDosis.Checked;
        CargarVacunas();
    }

    private void BtnConsultar_Click(object? sender, EventArgs e) => CargarVacunas();

    private void CargarVacunas()
    {
        try
        {
            int? idMascota = chkFiltrarMascota.Checked && cboMascota.SelectedValue is int id ? id : null;
            tablaVacunas.DataSource = vacunaServicio.Listar(idMascota, chkSoloProximas.Checked);
            EstiloControles.OcultarColumnas(tablaVacunas, "IdMascota");
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }
}
