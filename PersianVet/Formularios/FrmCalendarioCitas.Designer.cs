namespace PersianVet.Formularios;

partial class FrmCalendarioCitas
{
    private Label lblTitulo = null!;
    private Label lblAyuda = null!;
    private MonthCalendar calendario = null!;
    private Label lblDia = null!;
    private DataGridView tablaDia = null!;
    private Label lblEstado = null!;
    private Button btnCerrar = null!;

    private void InitializeComponent()
    {
        lblTitulo = new Label();
        lblAyuda = new Label();
        calendario = new MonthCalendar();
        lblDia = new Label();
        tablaDia = new DataGridView();
        lblEstado = new Label();
        btnCerrar = new Button();
        ((System.ComponentModel.ISupportInitialize)tablaDia).BeginInit();
        SuspendLayout();
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(8, 52, 90);
        lblTitulo.Location = new Point(20, 12);
        lblTitulo.Text = "Calendario de citas";
        lblAyuda.Location = new Point(24, 108);
        lblAyuda.Size = new Size(228, 60);
        lblAyuda.ForeColor = Color.FromArgb(94, 103, 112);
        lblAyuda.Text = "Los días en negrita tienen citas. Seleccione un día para ver el detalle.";
        calendario.Location = new Point(24, 176);
        calendario.MaxSelectionCount = 1;
        calendario.DateSelected += Calendario_DateSelected;
        lblDia.AutoSize = true;
        lblDia.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblDia.ForeColor = Color.FromArgb(8, 52, 90);
        lblDia.Location = new Point(280, 64);
        lblDia.Text = "Citas del día";
        tablaDia.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tablaDia.Location = new Point(280, 100);
        tablaDia.Size = new Size(560, 330);
        lblEstado.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblEstado.ForeColor = Color.FromArgb(94, 103, 112);
        lblEstado.Location = new Point(280, 446);
        lblEstado.Size = new Size(400, 24);
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;
        btnCerrar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCerrar.DialogResult = DialogResult.Cancel;
        btnCerrar.Location = new Point(740, 442);
        btnCerrar.Size = new Size(100, 34);
        btnCerrar.Text = "Cerrar";
        Controls.AddRange(new Control[] { lblTitulo, lblAyuda, calendario, lblDia, tablaDia, lblEstado, btnCerrar });
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnCerrar;
        ClientSize = new Size(864, 490);
        Font = new Font("Segoe UI", 9F);
        MinimumSize = new Size(780, 460);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "PersianVet - Calendario de citas";
        Load += FrmCalendarioCitas_Load;
        ((System.ComponentModel.ISupportInitialize)tablaDia).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}
