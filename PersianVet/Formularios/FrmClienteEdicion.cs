using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de un cliente en una ventana separada.
/// </summary>
/// <remarks>
/// La ventana valida y guarda antes de cerrarse: si hay errores, se queda abierta con lo escrito.
/// </remarks>
public partial class FrmClienteEdicion : Form
{
    private readonly Func<Cliente, ResultadoOperacion> guardar;

    public Cliente Cliente { get; private set; }

    public FrmClienteEdicion()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        Cliente = new Cliente();
        guardar = _ => ResultadoOperacion.Correcto(string.Empty);
    }

    /// <param name="guardar">Operación del servicio que valida y guarda, por ejemplo <c>clienteServicio.Guardar</c>.</param>
    /// <param name="cliente">Cliente a modificar, o <see langword="null"/> para registrar uno nuevo.</param>
    public FrmClienteEdicion(Func<Cliente, ResultadoOperacion> guardar, Cliente? cliente = null) : this()
    {
        this.guardar = guardar;
        Cliente = cliente ?? new Cliente();
        Text = cliente is null ? "Nuevo cliente" : "Modificar cliente";
        txtNombres.Text = Cliente.Nombres;
        txtApellidos.Text = Cliente.Apellidos;
        txtTelefono.Text = Cliente.Telefono;
        txtCorreo.Text = Cliente.Correo;
        txtDireccion.Text = Cliente.Direccion;
        dtpFecha.Value = Cliente.FechaRegistro;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Cliente.Nombres = txtNombres.Text;
        Cliente.Apellidos = txtApellidos.Text;
        Cliente.Telefono = txtTelefono.Text;
        Cliente.Correo = txtCorreo.Text;
        Cliente.Direccion = txtDireccion.Text;
        Cliente.FechaRegistro = dtpFecha.Value.Date;

        if (Mensajes.EjecutarOperacion(() => guardar(Cliente)))
        {
            DialogResult = DialogResult.OK;
        }
    }
}
