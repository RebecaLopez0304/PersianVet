namespace PersianVet.Controles;

partial class UcCitas
{
    private Label lblTitulo = null!, lblDescripcion = null!, lblMascota = null!, lblVeterinario = null!;
    private Label lblFecha = null!, lblHora = null!, lblEstadoCita = null!, lblMotivo = null!, lblLista = null!;
    private ComboBox cboMascota = null!, cboVeterinario = null!, cboEstado = null!;
    private DateTimePicker dtpFecha = null!, dtpHora = null!, dtpFiltro = null!;
    private TextBox txtMotivo = null!;
    private Button btnProgramar = null!, btnReprogramar = null!, btnCancelar = null!, btnFiltrar = null!;
    private CheckBox chkFiltrar = null!;
    private DataGridView tablaCitas = null!;

    private void InitializeComponent()
    {
        lblTitulo = new Label(); lblDescripcion = new Label(); lblMascota = new Label(); lblVeterinario = new Label();
        lblFecha = new Label(); lblHora = new Label(); lblEstadoCita = new Label(); lblMotivo = new Label(); lblLista = new Label();
        cboMascota = new ComboBox(); cboVeterinario = new ComboBox(); cboEstado = new ComboBox();
        dtpFecha = new DateTimePicker(); dtpHora = new DateTimePicker(); dtpFiltro = new DateTimePicker();
        txtMotivo = new TextBox(); btnProgramar = new Button(); btnReprogramar = new Button(); btnCancelar = new Button(); btnFiltrar = new Button();
        chkFiltrar = new CheckBox(); tablaCitas = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)tablaCitas).BeginInit(); SuspendLayout();
        lblTitulo.AutoSize = true; lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold); lblTitulo.ForeColor = Color.FromArgb(8, 52, 90); lblTitulo.Location = new Point(28, 18); lblTitulo.Text = "Agenda de citas";
        lblDescripcion.AutoSize = true; lblDescripcion.ForeColor = Color.FromArgb(104, 115, 125); lblDescripcion.Location = new Point(31, 62); lblDescripcion.Text = "Programa y administra las citas de los pacientes";
        lblMascota.AutoSize = true; lblMascota.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblMascota.Location = new Point(30, 108); lblMascota.Text = "Mascota";
        lblVeterinario.AutoSize = true; lblVeterinario.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblVeterinario.Location = new Point(260, 108); lblVeterinario.Text = "Veterinario";
        lblFecha.AutoSize = true; lblFecha.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblFecha.Location = new Point(530, 108); lblFecha.Text = "Fecha";
        lblHora.AutoSize = true; lblHora.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblHora.Location = new Point(700, 108); lblHora.Text = "Hora";
        lblEstadoCita.AutoSize = true; lblEstadoCita.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblEstadoCita.Location = new Point(850, 108); lblEstadoCita.Text = "Estado";
        lblMotivo.AutoSize = true; lblMotivo.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblMotivo.Location = new Point(30, 176); lblMotivo.Text = "Motivo";
        lblLista.AutoSize = true; lblLista.Font = new Font("Segoe UI", 13F, FontStyle.Bold); lblLista.ForeColor = Color.FromArgb(38, 50, 56); lblLista.Location = new Point(30, 278); lblLista.Text = "Citas registradas";
        cboMascota.DropDownStyle = ComboBoxStyle.DropDownList; cboMascota.Location = new Point(30, 132); cboMascota.Size = new Size(210, 23);
        cboVeterinario.DropDownStyle = ComboBoxStyle.DropDownList; cboVeterinario.Location = new Point(260, 132); cboVeterinario.Size = new Size(250, 23);
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList; cboEstado.Location = new Point(850, 132); cboEstado.Size = new Size(155, 23);
        cboEstado.Items.AddRange(new object[] { "Programada", "Confirmada", "Completada" }); cboEstado.SelectedIndex = 0;
        dtpFecha.Format = DateTimePickerFormat.Short; dtpFecha.Location = new Point(530, 132); dtpFecha.Size = new Size(150, 30);
        dtpHora.Format = DateTimePickerFormat.Time; dtpHora.ShowUpDown = true; dtpHora.Location = new Point(700, 132); dtpHora.Size = new Size(130, 30);
        txtMotivo.Font = new Font("Segoe UI", 10F); txtMotivo.Location = new Point(30, 200); txtMotivo.Size = new Size(480, 30);
        btnProgramar.Location = new Point(530, 195); btnProgramar.Size = new Size(140, 38); btnProgramar.Text = "Programar";
        btnReprogramar.Location = new Point(680, 195); btnReprogramar.Size = new Size(140, 38); btnReprogramar.Text = "Reprogramar";
        btnCancelar.Location = new Point(830, 195); btnCancelar.Size = new Size(140, 38); btnCancelar.Text = "Cancelar";
        chkFiltrar.AutoSize = true; chkFiltrar.Location = new Point(600, 282); chkFiltrar.Text = "Filtrar por fecha";
        dtpFiltro.Enabled = false; dtpFiltro.Format = DateTimePickerFormat.Short; dtpFiltro.Location = new Point(720, 276); dtpFiltro.Size = new Size(150, 30);
        btnFiltrar.Location = new Point(885, 274); btnFiltrar.Size = new Size(120, 38); btnFiltrar.Text = "Consultar";
        tablaCitas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; tablaCitas.Location = new Point(30, 325); tablaCitas.Size = new Size(975, 305);
        btnProgramar.Click += BtnProgramar_Click; btnReprogramar.Click += BtnReprogramar_Click; btnCancelar.Click += BtnCancelar_Click; btnFiltrar.Click += BtnFiltrar_Click;
        chkFiltrar.CheckedChanged += ChkFiltrar_CheckedChanged; tablaCitas.SelectionChanged += TablaCitas_SelectionChanged;
        Controls.AddRange(new Control[] { lblTitulo, lblDescripcion, lblMascota, lblVeterinario, lblFecha, lblHora, lblEstadoCita, lblMotivo, lblLista, cboMascota, cboVeterinario, cboEstado, dtpFecha, dtpHora, txtMotivo, btnProgramar, btnReprogramar, btnCancelar, chkFiltrar, dtpFiltro, btnFiltrar, tablaCitas });
        BackColor = Color.FromArgb(245, 247, 250); Font = new Font("Segoe UI", 9F); Name = "UcCitas"; Size = new Size(1040, 680);
        ((System.ComponentModel.ISupportInitialize)tablaCitas).EndInit(); ResumeLayout(false); PerformLayout();
    }

}
