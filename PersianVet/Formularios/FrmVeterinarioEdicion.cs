using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de un veterinario en una ventana separada.
/// </summary>
public partial class FrmVeterinarioEdicion : Form
{
    public Veterinario Veterinario { get; private set; }

    public FrmVeterinarioEdicion() : this(null) { }

    public FrmVeterinarioEdicion(Veterinario? veterinario)
    {
        InitializeComponent(); EstiloControles.ConfigurarBotonPrincipal(btnGuardar); EstiloControles.ConfigurarBotonSecundario(btnCancelar); Veterinario = veterinario ?? new Veterinario(); Text = veterinario is null ? "Nuevo veterinario" : "Modificar veterinario"; txtNombres.Text = Veterinario.Nombres; txtApellidos.Text = Veterinario.Apellidos; txtEspecialidad.Text = Veterinario.Especialidad; txtTelefono.Text = Veterinario.Telefono;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Veterinario.Nombres = txtNombres.Text; Veterinario.Apellidos = txtApellidos.Text; Veterinario.Especialidad = txtEspecialidad.Text; Veterinario.Telefono = txtTelefono.Text; DialogResult = DialogResult.OK;
    }
}
