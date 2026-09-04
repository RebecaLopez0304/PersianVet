namespace PersianVet.Controles;

partial class UcConsultas
{
    private Label lblTitulo = null!, lblDescripcion = null!, lblMascota = null!, lblVeterinario = null!, lblFecha = null!, lblHora = null!;
    private Label lblMotivo = null!, lblDiagnostico = null!, lblObservaciones = null!, lblTratamientos = null!, lblMedicamento = null!;
    private Label lblDosis = null!, lblFrecuencia = null!, lblDuracion = null!, lblIndicaciones = null!, lblHistorial = null!;
    private ComboBox cboMascota = null!, cboVeterinario = null!;
    private DateTimePicker dtpFecha = null!, dtpHora = null!;
    private TextBox txtMotivo = null!, txtDiagnostico = null!, txtObservaciones = null!;
    private TextBox txtMedicamento = null!, txtDosis = null!, txtFrecuencia = null!, txtDuracion = null!, txtIndicaciones = null!;
    private Button btnAgregarTratamiento = null!, btnQuitarTratamiento = null!, btnGuardar = null!;
    private DataGridView tablaTratamientos = null!, tablaHistorial = null!;

    private void InitializeComponent()
    {
        lblTitulo = new Label(); lblDescripcion = new Label(); lblMascota = new Label(); lblVeterinario = new Label(); lblFecha = new Label(); lblHora = new Label();
        lblMotivo = new Label(); lblDiagnostico = new Label(); lblObservaciones = new Label(); lblTratamientos = new Label(); lblMedicamento = new Label();
        lblDosis = new Label(); lblFrecuencia = new Label(); lblDuracion = new Label(); lblIndicaciones = new Label(); lblHistorial = new Label();
        cboMascota = new ComboBox(); cboVeterinario = new ComboBox(); dtpFecha = new DateTimePicker(); dtpHora = new DateTimePicker();
        txtMotivo = new TextBox(); txtDiagnostico = new TextBox(); txtObservaciones = new TextBox(); txtMedicamento = new TextBox(); txtDosis = new TextBox();
        txtFrecuencia = new TextBox(); txtDuracion = new TextBox(); txtIndicaciones = new TextBox(); btnAgregarTratamiento = new Button(); btnQuitarTratamiento = new Button(); btnGuardar = new Button();
        tablaTratamientos = new DataGridView(); tablaHistorial = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)tablaTratamientos).BeginInit(); ((System.ComponentModel.ISupportInitialize)tablaHistorial).BeginInit(); SuspendLayout();
        lblTitulo.AutoSize = true; lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold); lblTitulo.Location = new Point(28, 18); lblTitulo.Text = "Consulta e historial clínico";
        lblDescripcion.AutoSize = true; lblDescripcion.Location = new Point(31, 62); lblDescripcion.Text = "Registra y consulta el historial clínico de los pacientes";
        lblMascota.AutoSize = true; lblMascota.Location = new Point(30, 94); lblMascota.Text = "Mascota";
        lblVeterinario.AutoSize = true; lblVeterinario.Location = new Point(280, 94); lblVeterinario.Text = "Veterinario";
        lblFecha.AutoSize = true; lblFecha.Location = new Point(560, 94); lblFecha.Text = "Fecha";
        lblHora.AutoSize = true; lblHora.Location = new Point(720, 94); lblHora.Text = "Hora";
        lblMotivo.AutoSize = true; lblMotivo.Location = new Point(30, 158); lblMotivo.Text = "Motivo";
        lblDiagnostico.AutoSize = true; lblDiagnostico.Location = new Point(350, 158); lblDiagnostico.Text = "Diagnóstico";
        lblObservaciones.AutoSize = true; lblObservaciones.Location = new Point(670, 158); lblObservaciones.Text = "Observaciones";
        lblTratamientos.AutoSize = true; lblTratamientos.Font = new Font("Segoe UI", 12F, FontStyle.Bold); lblTratamientos.Location = new Point(30, 238); lblTratamientos.Text = "Tratamiento e indicaciones";
        lblMedicamento.AutoSize = true; lblMedicamento.Location = new Point(30, 266); lblMedicamento.Text = "Medicamento";
        lblDosis.AutoSize = true; lblDosis.Location = new Point(225, 266); lblDosis.Text = "Dosis";
        lblFrecuencia.AutoSize = true; lblFrecuencia.Location = new Point(360, 266); lblFrecuencia.Text = "Frecuencia";
        lblDuracion.AutoSize = true; lblDuracion.Location = new Point(525, 266); lblDuracion.Text = "Duración";
        lblIndicaciones.AutoSize = true; lblIndicaciones.Location = new Point(660, 266); lblIndicaciones.Text = "Indicaciones";
        lblHistorial.AutoSize = true; lblHistorial.Font = new Font("Segoe UI", 12F, FontStyle.Bold); lblHistorial.Location = new Point(30, 482); lblHistorial.Text = "Historial de consultas";
        cboMascota.DropDownStyle = ComboBoxStyle.DropDownList; cboMascota.Location = new Point(30, 118); cboMascota.Size = new Size(230, 23);
        cboVeterinario.DropDownStyle = ComboBoxStyle.DropDownList; cboVeterinario.Location = new Point(280, 118); cboVeterinario.Size = new Size(260, 23);
        dtpFecha.Format = DateTimePickerFormat.Short; dtpFecha.Location = new Point(560, 118); dtpFecha.Size = new Size(140, 23); dtpFecha.MaxDate = DateTime.Today;
        dtpHora.Format = DateTimePickerFormat.Time; dtpHora.ShowUpDown = true; dtpHora.Location = new Point(720, 118); dtpHora.Size = new Size(130, 23);
        txtMotivo.Location = new Point(30, 182); txtMotivo.Size = new Size(300, 25); txtDiagnostico.Location = new Point(350, 182); txtDiagnostico.Size = new Size(300, 25); txtObservaciones.Location = new Point(670, 182); txtObservaciones.Size = new Size(335, 25);
        txtMedicamento.Location = new Point(30, 290); txtMedicamento.Size = new Size(180, 25); txtDosis.Location = new Point(225, 290); txtDosis.Size = new Size(120, 25); txtFrecuencia.Location = new Point(360, 290); txtFrecuencia.Size = new Size(150, 25);
        txtDuracion.Location = new Point(525, 290); txtDuracion.Size = new Size(120, 25); txtIndicaciones.Location = new Point(660, 290); txtIndicaciones.Size = new Size(220, 25);
        btnAgregarTratamiento.Location = new Point(895, 286); btnAgregarTratamiento.Size = new Size(110, 38); btnAgregarTratamiento.Text = "Agregar";
        btnQuitarTratamiento.Location = new Point(895, 335); btnQuitarTratamiento.Size = new Size(110, 38); btnQuitarTratamiento.Text = "Quitar";
        btnGuardar.Location = new Point(830, 470); btnGuardar.Size = new Size(175, 38); btnGuardar.Text = "Guardar consulta";
        tablaTratamientos.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; tablaTratamientos.Location = new Point(30, 335); tablaTratamientos.Size = new Size(860, 120);
        tablaHistorial.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; tablaHistorial.Location = new Point(30, 520); tablaHistorial.Size = new Size(975, 120);
        cboMascota.SelectedIndexChanged += CboMascota_SelectedIndexChanged; btnAgregarTratamiento.Click += BtnAgregarTratamiento_Click; btnQuitarTratamiento.Click += BtnQuitarTratamiento_Click; btnGuardar.Click += BtnGuardar_Click;
        Controls.AddRange(new Control[] { lblTitulo, lblDescripcion, lblMascota, lblVeterinario, lblFecha, lblHora, lblMotivo, lblDiagnostico, lblObservaciones, lblTratamientos, lblMedicamento, lblDosis, lblFrecuencia, lblDuracion, lblIndicaciones, lblHistorial, cboMascota, cboVeterinario, dtpFecha, dtpHora, txtMotivo, txtDiagnostico, txtObservaciones, txtMedicamento, txtDosis, txtFrecuencia, txtDuracion, txtIndicaciones, btnAgregarTratamiento, btnQuitarTratamiento, btnGuardar, tablaTratamientos, tablaHistorial });
        BackColor = Color.FromArgb(245, 247, 250); Font = new Font("Segoe UI", 9F); Name = "UcConsultas"; Size = new Size(1040, 680);
        ((System.ComponentModel.ISupportInitialize)tablaTratamientos).EndInit(); ((System.ComponentModel.ISupportInitialize)tablaHistorial).EndInit(); ResumeLayout(false); PerformLayout();
    }

}
