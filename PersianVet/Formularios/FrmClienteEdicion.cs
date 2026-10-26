using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de un cliente en una ventana separada.
/// </summary>
public partial class FrmClienteEdicion : Form
{
    public Cliente Cliente { get; private set; }

    public FrmClienteEdicion()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        Cliente = new Cliente();
    }

    public FrmClienteEdicion(Cliente? cliente) : this()
    {
        Cliente = cliente ?? new Cliente();
        Text = cliente is null ? "Nuevo cliente" : "Modificar cliente";
        txtNombres.Text = Cliente.Nombres; txtApellidos.Text = Cliente.Apellidos; txtTelefono.Text = Cliente.Telefono; txtCorreo.Text = Cliente.Correo; txtDireccion.Text = Cliente.Direccion; dtpFecha.Value = Cliente.FechaRegistro;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Cliente.Nombres = txtNombres.Text; Cliente.Apellidos = txtApellidos.Text; Cliente.Telefono = txtTelefono.Text; Cliente.Correo = txtCorreo.Text; Cliente.Direccion = txtDireccion.Text; Cliente.FechaRegistro = dtpFecha.Value.Date; DialogResult = DialogResult.OK;
    }
}
