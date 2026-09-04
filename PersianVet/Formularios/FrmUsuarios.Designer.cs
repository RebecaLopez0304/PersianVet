using PersianVet.Utilidades;

namespace PersianVet.Formularios;

partial class FrmUsuarios
{
    private TabControl tabUsuarios = null!;
    private TabPage tabCrear = null!;
    private TabPage tabCambiar = null!;
    private TabPage tabLista = null!;
    private DataGridView tablaUsuarios = null!;
    private CheckBox chkMostrarDesactivados = null!;
    private Button btnDesactivarUsuario = null!;
    private TextBox txtNuevoUsuario = null!;
    private TextBox txtNuevaClave = null!;
    private TextBox txtConfirmarNueva = null!;
    private ComboBox cboRol = null!;
    private Button btnCrear = null!;
    private TextBox txtUsuarioCambio = null!;
    private TextBox txtClaveCambio = null!;
    private TextBox txtConfirmarCambio = null!;
    private Button btnCambiar = null!;
    private Label lblNuevoUsuario = null!, lblNuevaClave = null!, lblConfirmarNueva = null!, lblRol = null!;
    private Label lblUsuarioCambio = null!, lblClaveCambio = null!, lblConfirmarCambio = null!;

    private void InitializeComponent()
    {
        tabUsuarios = new TabControl();
        tabCrear = new TabPage();
        tabCambiar = new TabPage();
        tabLista = new TabPage();
        tablaUsuarios = new DataGridView();
        tablaUsuarios.Location = new Point(22, 58); tablaUsuarios.Size = new Size(560, 285); tablaUsuarios.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        chkMostrarDesactivados = new CheckBox();
        chkMostrarDesactivados.AutoSize = true; chkMostrarDesactivados.Location = new Point(22, 22); chkMostrarDesactivados.Text = "Mostrar desactivados";
        btnDesactivarUsuario = new Button();
        btnDesactivarUsuario.Location = new Point(422, 15); btnDesactivarUsuario.Size = new Size(160, 35); btnDesactivarUsuario.Text = "Desactivar usuario";
        EstiloControles.ConfigurarTabla(tablaUsuarios);
        EstiloControles.ConfigurarBotonSecundario(btnDesactivarUsuario);
        lblNuevoUsuario = new Label(); lblNuevaClave = new Label(); lblConfirmarNueva = new Label(); lblRol = new Label();
        lblUsuarioCambio = new Label(); lblClaveCambio = new Label(); lblConfirmarCambio = new Label();
        txtNuevoUsuario = new TextBox(); txtNuevoUsuario.Font = new Font("Segoe UI", 10F); txtNuevoUsuario.Location = new Point(42, 68); txtNuevoUsuario.Size = new Size(325, 25);
        txtNuevaClave = new TextBox(); txtNuevaClave.Font = new Font("Segoe UI", 10F); txtNuevaClave.Location = new Point(42, 132); txtNuevaClave.Size = new Size(325, 25);
        txtConfirmarNueva = new TextBox(); txtConfirmarNueva.Font = new Font("Segoe UI", 10F); txtConfirmarNueva.Location = new Point(42, 196); txtConfirmarNueva.Size = new Size(325, 25);
        cboRol = new ComboBox(); cboRol.DropDownStyle = ComboBoxStyle.DropDownList; cboRol.Location = new Point(42, 260); cboRol.Size = new Size(325, 23);
        cboRol.Items.AddRange(new object[] { "Administrador", "Recepcionista" });
        cboRol.SelectedIndex = 1;
        btnCrear = new Button(); btnCrear.Location = new Point(42, 320); btnCrear.Size = new Size(325, 40); btnCrear.Text = "Crear usuario";
        EstiloControles.ConfigurarBotonPrincipal(btnCrear);
        txtUsuarioCambio = new TextBox(); txtUsuarioCambio.Font = new Font("Segoe UI", 10F); txtUsuarioCambio.Location = new Point(42, 68); txtUsuarioCambio.Size = new Size(325, 25);
        txtClaveCambio = new TextBox(); txtClaveCambio.Font = new Font("Segoe UI", 10F); txtClaveCambio.Location = new Point(42, 132); txtClaveCambio.Size = new Size(325, 25);
        txtConfirmarCambio = new TextBox(); txtConfirmarCambio.Font = new Font("Segoe UI", 10F); txtConfirmarCambio.Location = new Point(42, 196); txtConfirmarCambio.Size = new Size(325, 25);
        btnCambiar = new Button(); btnCambiar.Location = new Point(42, 260); btnCambiar.Size = new Size(325, 40); btnCambiar.Text = "Cambiar contraseña";
        EstiloControles.ConfigurarBotonPrincipal(btnCambiar);
        txtNuevaClave.UseSystemPasswordChar = true;
        txtConfirmarNueva.UseSystemPasswordChar = true;
        txtClaveCambio.UseSystemPasswordChar = true;
        txtConfirmarCambio.UseSystemPasswordChar = true;
        btnCrear.Click += BtnCrear_Click;
        btnCambiar.Click += BtnCambiar_Click;
        tabCrear.Text = "Crear usuario";
        tabCrear.BackColor = Tema.Fondo;
        PrepararEtiqueta(lblNuevoUsuario, "Nombre de usuario", 42, 45); PrepararEtiqueta(lblNuevaClave, "Contraseña", 42, 109); PrepararEtiqueta(lblConfirmarNueva, "Confirmar contraseña", 42, 173); PrepararEtiqueta(lblRol, "Rol", 42, 237);
        PrepararEtiqueta(lblUsuarioCambio, "Nombre de usuario", 42, 45); PrepararEtiqueta(lblClaveCambio, "Nueva contraseña", 42, 109); PrepararEtiqueta(lblConfirmarCambio, "Confirmar contraseña", 42, 173);
        lblNuevoUsuario.AutoSize = true; lblNuevoUsuario.Location = new Point(42, 45); lblNuevoUsuario.Text = "Nombre de usuario";
        lblNuevaClave.AutoSize = true; lblNuevaClave.Location = new Point(42, 109); lblNuevaClave.Text = "Contraseña";
        lblConfirmarNueva.AutoSize = true; lblConfirmarNueva.Location = new Point(42, 173); lblConfirmarNueva.Text = "Confirmar contraseña";
        lblRol.AutoSize = true; lblRol.Location = new Point(42, 237); lblRol.Text = "Rol";
        lblUsuarioCambio.AutoSize = true; lblUsuarioCambio.Location = new Point(42, 45); lblUsuarioCambio.Text = "Nombre de usuario";
        lblClaveCambio.AutoSize = true; lblClaveCambio.Location = new Point(42, 109); lblClaveCambio.Text = "Nueva contraseña";
        lblConfirmarCambio.AutoSize = true; lblConfirmarCambio.Location = new Point(42, 173); lblConfirmarCambio.Text = "Confirmar contraseña";
        tabCrear.Controls.AddRange(new Control[] {
            lblNuevoUsuario, txtNuevoUsuario, lblNuevaClave, txtNuevaClave,
            lblConfirmarNueva, txtConfirmarNueva, lblRol, cboRol,
            btnCrear });
        tabCambiar.Text = "Cambiar contraseña";
        tabCambiar.BackColor = Tema.Fondo;
        tabCambiar.Controls.AddRange(new Control[] {
            lblUsuarioCambio, txtUsuarioCambio, lblClaveCambio, txtClaveCambio,
            lblConfirmarCambio, txtConfirmarCambio,
            btnCambiar });
        tabLista.Text = "Usuarios registrados";
        tabLista.BackColor = Tema.Fondo;
        tabLista.Controls.AddRange(new Control[] { tablaUsuarios, chkMostrarDesactivados, btnDesactivarUsuario });
        tabUsuarios.Controls.AddRange(new Control[] { tabCrear, tabCambiar, tabLista });
        tabUsuarios.Dock = DockStyle.Fill;
        tabUsuarios.Font = new Font("Segoe UI", 10F);
        Controls.Add(tabUsuarios);
        BackColor = Tema.Fondo;
        ClientSize = new Size(630, 440);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Administrar usuarios";
    }

    private static void PrepararEtiqueta(Label etiqueta, string texto, int x, int y)
    {
        etiqueta.AutoSize = true; etiqueta.Font = new Font("Segoe UI", 9F, FontStyle.Bold); etiqueta.ForeColor = Tema.GrisOscuro; etiqueta.Location = new Point(x, y); etiqueta.Text = texto;
    }
}
