using PersianVet.Datos;
using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite ingresar al sistema con un usuario registrado.
/// </summary>
public partial class FrmLogin : Form
{
    private readonly UsuarioServicio usuarioServicio;
    private readonly ConexionBD conexionBD;
    private bool ingresoEnProceso;

    /// <summary>
    /// Crea el formulario para el diseñador visual de Visual Studio.
    /// </summary>
    public FrmLogin()
    {
        InitializeComponent();
        usuarioServicio = null!;
        conexionBD = null!;
    }

    public FrmLogin(UsuarioServicio usuarioServicio, ConexionBD conexionBD) : this()
    {
        this.usuarioServicio = usuarioServicio;
        this.conexionBD = conexionBD;
        txtUsuario.Text = PreferenciasUsuario.Leer();
        chkRecordarme.Checked = !string.IsNullOrWhiteSpace(txtUsuario.Text);
        pictureMascota.Image = RecursosImagen.CargarMascota();
    }

    private async void BtnIngresar_Click(object? sender, EventArgs e)
    {
        if (ingresoEnProceso)
        {
            return;
        }

        errorProvider.Clear();
        lblMensaje.Text = string.Empty;
        if (string.IsNullOrWhiteSpace(txtUsuario.Text))
        {
            errorProvider.SetError(txtUsuario, "Escriba el usuario.");
        }

        if (string.IsNullOrWhiteSpace(txtClave.Text))
        {
            errorProvider.SetError(txtClave, "Escriba la contraseña.");
        }

        if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
        {
            lblMensaje.Text = "Escriba el usuario y la contraseña.";
            return;
        }

        try
        {
            ingresoEnProceso = true;
            btnIngresar.Enabled = false;
            btnIngresar.Text = "Ingresando...";

            string nombreUsuario = txtUsuario.Text;
            string clave = txtClave.Text;
            ResultadoInicioSesion resultado = await Task.Run(
                () => usuarioServicio.IniciarSesion(nombreUsuario, clave));
            if (!resultado.Exitoso || resultado.Usuario is null)
            {
                lblMensaje.Text = resultado.Mensaje;
                return;
            }

            if (chkRecordarme.Checked)
            {
                PreferenciasUsuario.Guardar(txtUsuario.Text.Trim());
            }
            else
            {
                PreferenciasUsuario.Borrar();
            }

            lblMensaje.Text = string.Empty;
            Hide();
            using FrmPrincipal principal = new(resultado.Usuario, conexionBD);
            principal.ShowDialog();
            if (!principal.SesionCerrada)
            {
                Close();
                return;
            }

            txtClave.Clear();
            Show();
            txtClave.Focus();
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
        finally
        {
            ingresoEnProceso = false;
            btnIngresar.Enabled = true;
            btnIngresar.Text = "Ingresar";
        }
    }

    private void ChkMostrarClave_CheckedChanged(object? sender, EventArgs e)
    {
        txtClave.UseSystemPasswordChar = !chkMostrarClave.Checked;
    }

    private void LinkUsuarios_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        using FrmUsuarios formulario = new(usuarioServicio);
        formulario.ShowDialog(this);
    }
}
