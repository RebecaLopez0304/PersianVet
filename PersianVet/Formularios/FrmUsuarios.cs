using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite crear usuarios y cambiar sus contraseñas.
/// </summary>
public partial class FrmUsuarios : Form
{
    private readonly UsuarioServicio usuarioServicio;

    public FrmUsuarios()
    {
        InitializeComponent();
        usuarioServicio = null!;
    }

    public FrmUsuarios(UsuarioServicio usuarioServicio) : this()
    {
        this.usuarioServicio = usuarioServicio;
        Load += (_, _) => CargarUsuarios();
        chkMostrarDesactivados.CheckedChanged += (_, _) => CargarUsuarios();
        btnDesactivarUsuario.Click += BtnDesactivarUsuario_Click;
    }

    private void BtnDesactivarUsuario_Click(object? sender, EventArgs e)
    {
        if (tablaUsuarios.CurrentRow?.DataBoundItem is not Usuario usuario)
        {
            MessageBox.Show("Seleccione un usuario de la lista.", "PersianVet");
            return;
        }

        if (MessageBox.Show("¿Desea desactivar al usuario seleccionado?", "PersianVet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        Ejecutar(() => usuarioServicio.Desactivar(usuario), CargarUsuarios);
    }

    private void CargarUsuarios()
    {
        try
        {
            tablaUsuarios.DataSource = usuarioServicio.Listar(chkMostrarDesactivados.Checked);
            if (tablaUsuarios.Columns.Contains("IdUsuario")) tablaUsuarios.Columns["IdUsuario"].Visible = false;
            if (tablaUsuarios.Columns.Contains("ClaveHash")) tablaUsuarios.Columns["ClaveHash"].Visible = false;
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void BtnCrear_Click(object? sender, EventArgs e)
    {
        Ejecutar(() => usuarioServicio.CrearUsuario(
            txtNuevoUsuario.Text,
            txtNuevaClave.Text,
            txtConfirmarNueva.Text,
            cboRol.Text),
            LimpiarCreacion);
    }

    private void BtnCambiar_Click(object? sender, EventArgs e)
    {
        Ejecutar(() => usuarioServicio.CambiarClave(
            txtUsuarioCambio.Text,
            txtClaveCambio.Text,
            txtConfirmarCambio.Text),
            LimpiarCambio);
    }

    private static void Ejecutar(Func<ResultadoOperacion> operacion, Action limpiar)
    {
        try
        {
            ResultadoOperacion resultado = operacion();
            MessageBox.Show(
                resultado.Mensaje,
                "PersianVet",
                MessageBoxButtons.OK,
                resultado.Exitoso ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (resultado.Exitoso)
            {
                limpiar();
            }
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void LimpiarCreacion()
    {
        txtNuevoUsuario.Clear();
        txtNuevaClave.Clear();
        txtConfirmarNueva.Clear();
        cboRol.SelectedIndex = 0;
    }

    private void LimpiarCambio()
    {
        txtUsuarioCambio.Clear();
        txtClaveCambio.Clear();
        txtConfirmarCambio.Clear();
    }
}
