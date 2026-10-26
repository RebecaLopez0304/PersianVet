namespace PersianVet.Formularios;

partial class FrmFichaMascota
{
    private Label lblNombre = null!;
    private Label lblResumen = null!;
    private TabControl pestanas = null!;
    private TabPage paginaConsultas = null!;
    private TabPage paginaVacunas = null!;
    private TabPage paginaCitas = null!;
    private DataGridView tablaConsultas = null!;
    private DataGridView tablaVacunas = null!;
    private DataGridView tablaCitas = null!;
    private Label lblEstado = null!;
    private Button btnCerrar = null!;

    private void InitializeComponent()
    {
        lblNombre = new Label();
        lblResumen = new Label();
        pestanas = new TabControl();
        paginaConsultas = new TabPage();
        paginaVacunas = new TabPage();
        paginaCitas = new TabPage();
        tablaConsultas = new DataGridView();
        tablaVacunas = new DataGridView();
        tablaCitas = new DataGridView();
        lblEstado = new Label();
        btnCerrar = new Button();
        ((System.ComponentModel.ISupportInitialize)tablaConsultas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)tablaVacunas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)tablaCitas).BeginInit();
        SuspendLayout();
        lblNombre.AutoSize = true;
        lblNombre.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
        lblNombre.ForeColor = Color.FromArgb(8, 52, 90);
        lblNombre.Location = new Point(20, 10);
        lblNombre.Text = "Ficha de mascota";
        lblResumen.Location = new Point(24, 62);
        lblResumen.Size = new Size(800, 66);
        lblResumen.Font = new Font("Segoe UI", 10F);
        lblResumen.ForeColor = Color.FromArgb(38, 50, 56);
        lblResumen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pestanas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pestanas.Location = new Point(20, 136);
        pestanas.Size = new Size(804, 380);
        pestanas.Controls.AddRange(new Control[] { paginaConsultas, paginaVacunas, paginaCitas });
        paginaConsultas.Text = "Consultas";
        paginaConsultas.Controls.Add(tablaConsultas);
        paginaVacunas.Text = "Vacunas";
        paginaVacunas.Controls.Add(tablaVacunas);
        paginaCitas.Text = "Citas";
        paginaCitas.Controls.Add(tablaCitas);
        tablaConsultas.Dock = DockStyle.Fill;
        tablaVacunas.Dock = DockStyle.Fill;
        tablaCitas.Dock = DockStyle.Fill;
        lblEstado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblEstado.ForeColor = Color.FromArgb(94, 103, 112);
        lblEstado.Location = new Point(20, 530);
        lblEstado.Size = new Size(600, 24);
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCerrar.DialogResult = DialogResult.Cancel;
        btnCerrar.Location = new Point(724, 524);
        btnCerrar.Size = new Size(100, 34);
        btnCerrar.Text = "Cerrar";
        Controls.AddRange(new Control[] { lblNombre, lblResumen, pestanas, lblEstado, btnCerrar });
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCerrar;
        ClientSize = new Size(844, 570);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(700, 500);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "PersianVet - Ficha de mascota";
        Load += FrmFichaMascota_Load;
        ((System.ComponentModel.ISupportInitialize)tablaConsultas).EndInit();
        ((System.ComponentModel.ISupportInitialize)tablaVacunas).EndInit();
        ((System.ComponentModel.ISupportInitialize)tablaCitas).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
