using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de una mascota en una ventana separada.
/// </summary>
/// <remarks>
/// La ventana valida y guarda antes de cerrarse: si hay errores, se queda abierta con lo escrito.
/// </remarks>
public partial class FrmMascotaEdicion : Form
{
    private readonly Func<Mascota, ResultadoOperacion> guardar;

    public Mascota Mascota { get; private set; }

    public FrmMascotaEdicion()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        Mascota = new Mascota();
        guardar = _ => ResultadoOperacion.Correcto(string.Empty);
    }

    /// <param name="clientes">Clientes que se ofrecen como propietario.</param>
    /// <param name="guardar">Operación del servicio que valida y guarda, por ejemplo <c>mascotaServicio.Guardar</c>.</param>
    /// <param name="mascota">Mascota a modificar, o <see langword="null"/> para registrar una nueva.</param>
    public FrmMascotaEdicion(List<Cliente> clientes, Func<Mascota, ResultadoOperacion> guardar, Mascota? mascota = null) : this()
    {
        this.guardar = guardar;
        Mascota = mascota ?? new Mascota();
        Text = mascota is null ? "Nueva mascota" : "Modificar mascota";
        cboPropietario.DataSource = clientes;
        cboPropietario.DisplayMember = "NombreCompleto";
        cboPropietario.ValueMember = "IdCliente";
        CargarDatos();
    }

    private void CargarDatos()
    {
        cboPropietario.SelectedValue = Mascota.IdCliente;
        txtNombre.Text = Mascota.Nombre;
        cboEspecie.Text = string.IsNullOrWhiteSpace(Mascota.Especie) ? "Felino" : Mascota.Especie;
        txtRaza.Text = Mascota.Raza;
        cboSexo.Text = Mascota.Sexo;
        dtpNacimiento.Value = Mascota.FechaNacimiento > DateTime.Today ? DateTime.Today : Mascota.FechaNacimiento;
        txtColor.Text = Mascota.Color;
        numPeso.Value = Mascota.Peso > 0 ? Math.Min(numPeso.Maximum, Mascota.Peso) : 1;
        txtObservaciones.Text = Mascota.Observaciones;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Mascota.IdCliente = cboPropietario.SelectedValue is int id ? id : 0;
        Mascota.Nombre = txtNombre.Text;
        Mascota.Especie = cboEspecie.Text;
        Mascota.Raza = txtRaza.Text;
        Mascota.Sexo = cboSexo.Text;
        Mascota.FechaNacimiento = dtpNacimiento.Value.Date;
        Mascota.Color = txtColor.Text;
        Mascota.Peso = numPeso.Value;
        Mascota.Observaciones = txtObservaciones.Text;

        if (Mensajes.EjecutarOperacion(() => guardar(Mascota)))
        {
            DialogResult = DialogResult.OK;
        }
    }
}
