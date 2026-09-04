using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>Permite escribir los datos de una mascota en una ventana separada.</summary>
public partial class FrmMascotaEdicion : Form
{
    public Mascota Mascota { get; private set; }

    public FrmMascotaEdicion()
    {
        InitializeComponent(); EstiloControles.ConfigurarBotonPrincipal(btnGuardar); EstiloControles.ConfigurarBotonSecundario(btnCancelar); Mascota = new Mascota();
    }

    public FrmMascotaEdicion(List<Cliente> clientes, Mascota? mascota = null) : this()
    {
        Mascota = mascota ?? new Mascota(); Text = mascota is null ? "Nueva mascota" : "Modificar mascota"; cboPropietario.DataSource = clientes; cboPropietario.DisplayMember = "NombreCompleto"; cboPropietario.ValueMember = "IdCliente"; CargarDatos();
    }

    private void CargarDatos()
    {
        cboPropietario.SelectedValue = Mascota.IdCliente; txtNombre.Text = Mascota.Nombre; cboEspecie.Text = string.IsNullOrWhiteSpace(Mascota.Especie) ? "Felino" : Mascota.Especie; txtRaza.Text = Mascota.Raza; cboSexo.Text = Mascota.Sexo; dtpNacimiento.Value = Mascota.FechaNacimiento > DateTime.Today ? DateTime.Today : Mascota.FechaNacimiento; txtColor.Text = Mascota.Color; numPeso.Value = Mascota.Peso > 0 ? Math.Min(numPeso.Maximum, Mascota.Peso) : 1; txtObservaciones.Text = Mascota.Observaciones;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Mascota.IdCliente = cboPropietario.SelectedValue is int id ? id : 0; Mascota.Nombre = txtNombre.Text; Mascota.Especie = cboEspecie.Text; Mascota.Raza = txtRaza.Text; Mascota.Sexo = cboSexo.Text; Mascota.FechaNacimiento = dtpNacimiento.Value.Date; Mascota.Color = txtColor.Text; Mascota.Peso = numPeso.Value; Mascota.Observaciones = txtObservaciones.Text; DialogResult = DialogResult.OK;
    }
}
