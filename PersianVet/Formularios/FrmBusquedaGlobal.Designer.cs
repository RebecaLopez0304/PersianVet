namespace PersianVet.Formularios;

partial class FrmBusquedaGlobal
{
    private Label lblTitulo = null!;
    private DataGridView tablaResultados = null!;
    private Label lblEstado = null!;
    private Button btnAbrir = null!;
    private Button btnCerrar = null!;

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        tablaResultados = new DataGridView();
        lblEstado = new Label();
        btnAbrir = new Button();
        btnCerrar = new Button();
        ((System.ComponentModel.ISupportInitialize)tablaResultados).BeginInit();
        SuspendLayout();
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(8, 52, 90);
        lblTitulo.Location = new Point(20, 14);
        lblTitulo.Text = "Resultados de la búsqueda";
        tablaResultados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tablaResultados.Location = new Point(20, 56);
        tablaResultados.Size = new Size(720, 300);
        tablaResultados.CellDoubleClick += TablaResultados_CellDoubleClick;
        lblEstado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblEstado.Location = new Point(20, 372);
        lblEstado.Size = new Size(430, 24);
        lblEstado.ForeColor = Color.FromArgb(94, 103, 112);
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        btnAbrir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAbrir.Location = new Point(520, 368);
        btnAbrir.Size = new Size(110, 34);
        btnAbrir.Text = "Abrir";
        btnAbrir.Click += BtnAbrir_Click;
        btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCerrar.DialogResult = DialogResult.Cancel;
        btnCerrar.Location = new Point(640, 368);
        btnCerrar.Size = new Size(100, 34);
        btnCerrar.Text = "Cerrar";
        Controls.AddRange(new Control[] { lblTitulo, tablaResultados, lblEstado, btnAbrir, btnCerrar });
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCerrar;
        ClientSize = new Size(760, 416);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(640, 400);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "PersianVet - Búsqueda";
        Load += FrmBusquedaGlobal_Load;
        FormClosing += FrmBusquedaGlobal_FormClosing;
        ((System.ComponentModel.ISupportInitialize)tablaResultados).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
