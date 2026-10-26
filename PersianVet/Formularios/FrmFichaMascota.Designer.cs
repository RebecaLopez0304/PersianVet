namespace PersianVet.Formularios;

partial class FrmFichaMascota
{
    private Label lblNombre = null!;
    private TableLayoutPanel panelDatos = null!;
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
        panelDatos = new TableLayoutPanel();
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
        // Seis columnas iguales: en la primera fila los datos de la mascota, en la segunda el dueño y las observaciones.
        panelDatos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        panelDatos.BackColor = Color.White;
        panelDatos.ColumnCount = 6;
        panelDatos.RowCount = 2;
        panelDatos.Location = new Point(20, 58);
        panelDatos.Padding = new Padding(10, 6, 10, 6);
        panelDatos.Size = new Size(804, 104);
        for (int columna = 0; columna < 6; columna++)
        {
            panelDatos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 6));
        }
        panelDatos.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        panelDatos.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        pestanas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pestanas.Location = new Point(20, 172);
        pestanas.Size = new Size(804, 344);
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
        Controls.AddRange(new Control[] { lblNombre, panelDatos, pestanas, lblEstado, btnCerrar });
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
