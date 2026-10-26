using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de un veterinario en una ventana separada.
/// </summary>
/// <remarks>
/// La ventana valida y guarda antes de cerrarse: si hay errores, se queda abierta con lo escrito.
/// </remarks>
public partial class FrmVeterinarioEdicion : Form
{
    private readonly Func<Veterinario, ResultadoOperacion> guardar;

    public Veterinario Veterinario { get; private set; }

    public FrmVeterinarioEdicion() : this(_ => ResultadoOperacion.Correcto(string.Empty))
    {
    }

    /// <param name="guardar">Operación del servicio que valida y guarda, por ejemplo <c>veterinarioServicio.Guardar</c>.</param>
    /// <param name="veterinario">Veterinario a modificar, o <see langword="null"/> para registrar uno nuevo.</param>
    public FrmVeterinarioEdicion(Func<Veterinario, ResultadoOperacion> guardar, Veterinario? veterinario = null)
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnCancelar);
        this.guardar = guardar;
        Veterinario = veterinario ?? new Veterinario();
        Text = veterinario is null ? "Nuevo veterinario" : "Modificar veterinario";
        txtNombres.Text = Veterinario.Nombres;
        txtApellidos.Text = Veterinario.Apellidos;
        txtEspecialidad.Text = Veterinario.Especialidad;
        txtTelefono.Text = Veterinario.Telefono;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Veterinario.Nombres = txtNombres.Text;
        Veterinario.Apellidos = txtApellidos.Text;
        Veterinario.Especialidad = txtEspecialidad.Text;
        Veterinario.Telefono = txtTelefono.Text;

        if (Mensajes.EjecutarOperacion(() => guardar(Veterinario)))
        {
            DialogResult = DialogResult.OK;
        }
    }
}
