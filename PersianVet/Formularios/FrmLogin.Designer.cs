#nullable enable
namespace PersianVet.Formularios;

partial class FrmLogin
{
    private System.ComponentModel.IContainer? components;
    private Panel panelMarca = null!;
    private PictureBox pictureMascota = null!;
    private Label lblMarca = null!;
    private Label lblSubmarca = null!;
    private Label lblEslogan = null!;
    private Label lblTitulo = null!;
    private Label lblBienvenida = null!;
    private Label lblUsuario = null!;
    private TextBox txtUsuario = null!;
    private Label lblClave = null!;
    private TextBox txtClave = null!;
    private CheckBox chkRecordarme = null!;
    private CheckBox chkMostrarClave = null!;
    private Button btnIngresar = null!;
    private LinkLabel linkUsuarios = null!;
    private Label lblMensaje = null!;
    private ErrorProvider errorProvider = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            components?.Dispose();
            pictureMascota.Image?.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        panelMarca = new Panel();
        pictureMascota = new PictureBox();
        lblMarca = new Label();
        lblSubmarca = new Label();
        lblEslogan = new Label();
        lblTitulo = new Label();
        lblBienvenida = new Label();
        lblUsuario = new Label();
        txtUsuario = new TextBox();
        lblClave = new Label();
        txtClave = new TextBox();
        chkRecordarme = new CheckBox();
        chkMostrarClave = new CheckBox();
        btnIngresar = new Button();
        linkUsuarios = new LinkLabel();
        lblMensaje = new Label();
        errorProvider = new ErrorProvider(components);
        panelMarca.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureMascota).BeginInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
        SuspendLayout();
        panelMarca.BackColor = Color.FromArgb(8, 52, 90);
        panelMarca.Controls.Add(pictureMascota);
        panelMarca.Controls.Add(lblMarca);
        panelMarca.Controls.Add(lblSubmarca);
        panelMarca.Controls.Add(lblEslogan);
        panelMarca.Dock = DockStyle.Left;
        panelMarca.Width = 430;
        pictureMascota.Location = new Point(105, 110);
        pictureMascota.Size = new Size(220, 190);
        pictureMascota.SizeMode = PictureBoxSizeMode.Zoom;
        lblMarca.Font = new Font("Segoe UI", 31F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(90, 310);
        lblMarca.Size = new Size(270, 60);
        lblMarca.Text = "PersianVet";
        lblMarca.TextAlign = ContentAlignment.MiddleCenter;
        lblSubmarca.Font = new Font("Segoe UI", 13F);
        lblSubmarca.ForeColor = Color.White;
        lblSubmarca.Location = new Point(100, 365);
        lblSubmarca.Size = new Size(250, 35);
        lblSubmarca.Text = "Veterinaria Persian";
        lblSubmarca.TextAlign = ContentAlignment.MiddleCenter;
        lblEslogan.Font = new Font("Segoe UI", 12F, FontStyle.Italic);
        lblEslogan.ForeColor = Color.FromArgb(255, 244, 230);
        lblEslogan.Location = new Point(80, 440);
        lblEslogan.Size = new Size(290, 35);
        lblEslogan.Text = "Cuidado, salud y cariño";
        lblEslogan.TextAlign = ContentAlignment.MiddleCenter;
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(8, 52, 90);
        lblTitulo.Location = new Point(555, 125);
        lblTitulo.Text = "Iniciar sesión";
        lblBienvenida.AutoSize = true;
        lblBienvenida.Font = new Font("Segoe UI", 11F);
        lblBienvenida.ForeColor = Color.FromArgb(94, 103, 112);
        lblBienvenida.Location = new Point(560, 180);
        lblBienvenida.Text = "Bienvenido a PersianVet";
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUsuario.Location = new Point(560, 235);
        lblUsuario.Text = "Usuario";
        txtUsuario.Font = new Font("Segoe UI", 11F);
        txtUsuario.Location = new Point(560, 260);
        txtUsuario.Size = new Size(330, 32);
        lblClave.AutoSize = true;
        lblClave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblClave.Location = new Point(560, 315);
        lblClave.Text = "Contraseña";
        txtClave.Font = new Font("Segoe UI", 11F);
        txtClave.Location = new Point(560, 340);
        txtClave.Size = new Size(330, 32);
        txtClave.UseSystemPasswordChar = true;
        chkRecordarme.AutoSize = true;
        chkRecordarme.Location = new Point(560, 390);
        chkRecordarme.Text = "Recordarme";
        chkMostrarClave.AutoSize = true;
        chkMostrarClave.Location = new Point(770, 390);
        chkMostrarClave.Text = "Mostrar contraseña";
        chkMostrarClave.CheckedChanged += ChkMostrarClave_CheckedChanged;
        btnIngresar.Location = new Point(560, 435);
        btnIngresar.Size = new Size(330, 42);
        btnIngresar.Text = "Ingresar";
        btnIngresar.Click += BtnIngresar_Click;
        btnIngresar.BackColor = Color.FromArgb(242, 140, 40);
        btnIngresar.FlatStyle = FlatStyle.Flat;
        btnIngresar.ForeColor = Color.White;
        btnIngresar.FlatAppearance.BorderSize = 0;
        linkUsuarios.AutoSize = true;
        linkUsuarios.LinkColor = Color.FromArgb(31, 78, 121);
        linkUsuarios.Location = new Point(650, 505);
        linkUsuarios.Text = "Administrar usuarios";
        linkUsuarios.LinkClicked += LinkUsuarios_LinkClicked;
        lblMensaje.ForeColor = Color.Firebrick;
        lblMensaje.Location = new Point(545, 545);
        lblMensaje.Size = new Size(365, 45);
        lblMensaje.TextAlign = ContentAlignment.MiddleCenter;
        AcceptButton = btnIngresar;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(248, 250, 252);
        ClientSize = new Size(1000, 650);
        Controls.Add(panelMarca);
        Controls.Add(lblTitulo);
        Controls.Add(lblBienvenida);
        Controls.Add(lblUsuario);
        Controls.Add(txtUsuario);
        Controls.Add(lblClave);
        Controls.Add(txtClave);
        Controls.Add(chkRecordarme);
        Controls.Add(chkMostrarClave);
        Controls.Add(btnIngresar);
        Controls.Add(linkUsuarios);
        Controls.Add(lblMensaje);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PersianVet - Inicio de sesión";
        panelMarca.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureMascota).EndInit();
        ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
