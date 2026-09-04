#nullable enable
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

partial class FrmPrincipal
{
    private System.ComponentModel.IContainer? components;
    private Panel panelEncabezado = null!;
    private Panel panelMenu = null!;
    private Panel panelContenido = null!;
    private Label lblMarca = null!;
    private Label lblEslogan = null!;
    private Label lblFechaHora = null!;
    private Label lblUsuario = null!;
    private Label lblRol = null!;
    private Button btnInicio = null!;
    private Button btnClientes = null!;
    private Button btnMascotas = null!;
    private Button btnCitas = null!;
    private Button btnConsultas = null!;
    private Button btnVacunas = null!;
    private Button btnVeterinarios = null!;
    private Button btnReportes = null!;
    private Button btnCerrarSesion = null!;
    private PictureBox pictureMascota = null!;
    private System.Windows.Forms.Timer timerReloj = null!;

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
        panelEncabezado = new Panel();
        panelMenu = new Panel();
        panelContenido = new Panel();
        lblMarca = new Label();
        lblEslogan = new Label();
        lblFechaHora = new Label();
        lblUsuario = new Label();
        lblRol = new Label();
        pictureMascota = new PictureBox();
        btnInicio = new Button();
        btnClientes = new Button();
        btnMascotas = new Button();
        btnCitas = new Button();
        btnConsultas = new Button();
        btnVacunas = new Button();
        btnVeterinarios = new Button();
        btnReportes = new Button();
        btnCerrarSesion = new Button();
        timerReloj = new System.Windows.Forms.Timer(components);
        panelEncabezado.SuspendLayout();
        panelMenu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureMascota).BeginInit();
        SuspendLayout();
        panelEncabezado.BackColor = Tema.AzulOscuro;
        panelEncabezado.Controls.Add(lblMarca);
        panelEncabezado.Controls.Add(lblEslogan);
        panelEncabezado.Controls.Add(lblFechaHora);
        panelEncabezado.Controls.Add(lblUsuario);
        panelEncabezado.Controls.Add(lblRol);
        panelEncabezado.Dock = DockStyle.Top;
        panelEncabezado.Height = 92;
        lblMarca.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(30, 12);
        lblMarca.Size = new Size(250, 48);
        lblMarca.Text = "PersianVet";
        lblEslogan.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
        lblEslogan.ForeColor = Tema.Crema;
        lblEslogan.Location = new Point(292, 36);
        lblEslogan.Size = new Size(240, 25);
        lblEslogan.Text = "Cuidado, salud y cariño";
        lblFechaHora.ForeColor = Color.White;
        lblFechaHora.Location = new Point(705, 15);
        lblFechaHora.Size = new Size(330, 25);
        lblFechaHora.TextAlign = ContentAlignment.MiddleRight;
        lblUsuario.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblUsuario.ForeColor = Color.White;
        lblUsuario.Location = new Point(1050, 12);
        lblUsuario.Size = new Size(130, 24);
        lblUsuario.TextAlign = ContentAlignment.MiddleRight;
        lblRol.Font = new Font("Segoe UI", 8F);
        lblRol.ForeColor = Tema.Crema;
        lblRol.Location = new Point(1050, 38);
        lblRol.Size = new Size(130, 20);
        lblRol.TextAlign = ContentAlignment.MiddleRight;
        panelMenu.BackColor = Color.White;
        panelMenu.Controls.AddRange(new Control[] { btnInicio, btnClientes, btnMascotas, btnCitas, btnConsultas, btnVacunas, btnVeterinarios, btnReportes, btnCerrarSesion, pictureMascota });
        panelMenu.Dock = DockStyle.Left;
        panelMenu.Width = 224;
        pictureMascota.Location = new Point(25, 490);
        pictureMascota.Size = new Size(155, 120);
        pictureMascota.SizeMode = PictureBoxSizeMode.Zoom;
        ConfigurarBotonMenu(btnInicio, "Inicio", 88);
        ConfigurarBotonMenu(btnClientes, "Clientes", 137);
        ConfigurarBotonMenu(btnMascotas, "Mascotas", 186);
        ConfigurarBotonMenu(btnCitas, "Citas", 235);
        ConfigurarBotonMenu(btnConsultas, "Consultas", 284);
        ConfigurarBotonMenu(btnVacunas, "Vacunas", 333);
        ConfigurarBotonMenu(btnVeterinarios, "Veterinarios", 382);
        ConfigurarBotonMenu(btnReportes, "Reportes", 431);
        ConfigurarBotonMenu(btnCerrarSesion, "Cerrar sesión", 617);
        btnInicio.Location = new Point(14, 88); btnInicio.Size = new Size(196, 44); btnInicio.Text = "Inicio";
        btnClientes.Location = new Point(14, 137); btnClientes.Size = new Size(196, 44); btnClientes.Text = "Clientes";
        btnMascotas.Location = new Point(14, 186); btnMascotas.Size = new Size(196, 44); btnMascotas.Text = "Mascotas";
        btnCitas.Location = new Point(14, 235); btnCitas.Size = new Size(196, 44); btnCitas.Text = "Citas";
        btnConsultas.Location = new Point(14, 284); btnConsultas.Size = new Size(196, 44); btnConsultas.Text = "Consultas";
        btnVacunas.Location = new Point(14, 333); btnVacunas.Size = new Size(196, 44); btnVacunas.Text = "Vacunas";
        btnVeterinarios.Location = new Point(14, 382); btnVeterinarios.Size = new Size(196, 44); btnVeterinarios.Text = "Veterinarios";
        btnReportes.Location = new Point(14, 431); btnReportes.Size = new Size(196, 44); btnReportes.Text = "Reportes";
        btnCerrarSesion.Location = new Point(14, 617); btnCerrarSesion.Size = new Size(196, 44); btnCerrarSesion.Text = "Cerrar sesión";
        panelEncabezado.Resize += PanelEncabezado_Resize;
        panelMenu.Resize += PanelMenu_Resize;
        panelContenido.BackColor = Tema.Fondo;
        panelContenido.Padding = new Padding(18);
        panelContenido.Dock = DockStyle.Fill;
        btnInicio.Click += BtnInicio_Click;
        btnClientes.Click += BtnClientes_Click;
        btnMascotas.Click += BtnMascotas_Click;
        btnCitas.Click += BtnCitas_Click;
        btnConsultas.Click += BtnConsultas_Click;
        btnVacunas.Click += BtnVacunas_Click;
        btnVeterinarios.Click += BtnVeterinarios_Click;
        btnReportes.Click += BtnReportes_Click;
        btnCerrarSesion.Click += BtnCerrarSesion_Click;
        timerReloj.Interval = 1000;
        timerReloj.Tick += TimerReloj_Tick;
        timerReloj.Start();
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 720);
        Controls.Add(panelContenido);
        Controls.Add(panelMenu);
        Controls.Add(panelEncabezado);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(1080, 680);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PersianVet - Sistema de Gestión Veterinaria";
        WindowState = FormWindowState.Normal;
        FormClosing += FrmPrincipal_FormClosing;
        panelEncabezado.ResumeLayout(false);
        panelMenu.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)pictureMascota).EndInit();
        ResumeLayout(false);
    }

    private static void ConfigurarBotonMenu(Button boton, string texto, int posicionY)
    {
        boton.BackColor = Color.Transparent;
        boton.Cursor = Cursors.Hand;
        boton.FlatStyle = FlatStyle.Flat;
        boton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        boton.ForeColor = Tema.AzulOscuro;
        boton.Location = new Point(14, posicionY);
        boton.Size = new Size(196, 44);
        boton.Text = texto;
        boton.TextAlign = ContentAlignment.MiddleLeft;
        boton.UseVisualStyleBackColor = false;
        boton.FlatAppearance.BorderSize = 0;
        boton.FlatAppearance.MouseOverBackColor = Tema.NaranjaClaro;
        boton.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 224, 190);
    }
}
