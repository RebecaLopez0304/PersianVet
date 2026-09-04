namespace PersianVet.Formularios;

partial class FrmClienteEdicion
{
    private Label lblTitulo = null!, lblNombres = null!, lblApellidos = null!, lblTelefono = null!, lblCorreo = null!, lblDireccion = null!, lblFecha = null!;
    private TextBox txtNombres = null!, txtApellidos = null!, txtTelefono = null!, txtCorreo = null!, txtDireccion = null!;
    private DateTimePicker dtpFecha = null!;
    private Button btnGuardar = null!, btnCancelar = null!;

    private void InitializeComponent()
    {
        lblTitulo = new Label(); lblNombres = new Label(); lblApellidos = new Label(); lblTelefono = new Label(); lblCorreo = new Label(); lblDireccion = new Label(); lblFecha = new Label(); txtNombres = new TextBox(); txtApellidos = new TextBox(); txtTelefono = new TextBox(); txtCorreo = new TextBox(); txtDireccion = new TextBox(); dtpFecha = new DateTimePicker(); btnGuardar = new Button(); btnCancelar = new Button(); SuspendLayout();
        lblTitulo.AutoSize = true; lblTitulo.Font = new Font("Segoe UI", 20F, FontStyle.Bold); lblTitulo.ForeColor = Color.FromArgb(8, 52, 90); lblTitulo.Location = new Point(28, 22); lblTitulo.Text = "Datos del cliente";
        lblNombres.AutoSize = true; lblNombres.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblNombres.Location = new Point(30, 86); lblNombres.Text = "Nombres";
        lblApellidos.AutoSize = true; lblApellidos.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblApellidos.Location = new Point(315, 86); lblApellidos.Text = "Apellidos";
        lblTelefono.AutoSize = true; lblTelefono.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblTelefono.Location = new Point(30, 153); lblTelefono.Text = "Teléfono";
        lblCorreo.AutoSize = true; lblCorreo.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblCorreo.Location = new Point(315, 153); lblCorreo.Text = "Correo";
        lblDireccion.AutoSize = true; lblDireccion.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblDireccion.Location = new Point(30, 220); lblDireccion.Text = "Dirección";
        lblFecha.AutoSize = true; lblFecha.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblFecha.Location = new Point(315, 220); lblFecha.Text = "Fecha de registro";
        txtNombres.Font = new Font("Segoe UI", 10F); txtNombres.Location = new Point(30, 108); txtNombres.Size = new Size(255, 30);
        txtApellidos.Font = new Font("Segoe UI", 10F); txtApellidos.Location = new Point(315, 108); txtApellidos.Size = new Size(255, 30);
        txtTelefono.Font = new Font("Segoe UI", 10F); txtTelefono.Location = new Point(30, 175); txtTelefono.Size = new Size(255, 30);
        txtCorreo.Font = new Font("Segoe UI", 10F); txtCorreo.Location = new Point(315, 175); txtCorreo.Size = new Size(255, 30);
        txtDireccion.Font = new Font("Segoe UI", 10F); txtDireccion.Location = new Point(30, 242); txtDireccion.Size = new Size(255, 30);
        dtpFecha.Format = DateTimePickerFormat.Short; dtpFecha.Location = new Point(315, 242); dtpFecha.Size = new Size(255, 30);
        btnCancelar.BackColor = Color.White; btnCancelar.DialogResult = DialogResult.Cancel; btnCancelar.FlatStyle = FlatStyle.Flat; btnCancelar.ForeColor = Color.FromArgb(31, 78, 121); btnCancelar.Location = new Point(315, 298); btnCancelar.Size = new Size(120, 40); btnCancelar.Text = "Cancelar";
        btnGuardar.BackColor = Color.FromArgb(242, 140, 40); btnGuardar.FlatStyle = FlatStyle.Flat; btnGuardar.ForeColor = Color.White; btnGuardar.Location = new Point(450, 298); btnGuardar.Size = new Size(120, 40); btnGuardar.Text = "Guardar"; btnGuardar.Click += Guardar;
        AcceptButton = btnGuardar; CancelButton = btnCancelar; BackColor = Color.FromArgb(248, 250, 252); ClientSize = new Size(600, 370); Controls.AddRange(new Control[] { lblTitulo, lblNombres, lblApellidos, lblTelefono, lblCorreo, lblDireccion, lblFecha, txtNombres, txtApellidos, txtTelefono, txtCorreo, txtDireccion, dtpFecha, btnCancelar, btnGuardar }); Font = new Font("Segoe UI", 9F); FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; StartPosition = FormStartPosition.CenterParent; Text = "Nuevo cliente"; ResumeLayout(false); PerformLayout();
    }
}
