namespace PersianVet.Controles;

partial class UcVeterinarios
{
    private Label lblTitulo = null!, lblDescripcion = null!, lblBuscar = null!;
    private TextBox txtNombres = null!, txtApellidos = null!, txtEspecialidad = null!, txtTelefono = null!, txtBuscar = null!;
    private Button btnNuevo = null!, btnGuardar = null!, btnEditar = null!, btnDesactivar = null!;
    private DataGridView tabla = null!;
    private CheckBox chkMostrarInactivos = null!;

    private void InitializeComponent()
    {
        // El tamaño se fija antes de agregar controles para que los anclajes se calculen bien.
        Size = new Size(940, 592);
        lblTitulo = new Label(); lblDescripcion = new Label(); lblBuscar = new Label();
        lblTitulo.AutoSize = true; lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold); lblTitulo.ForeColor = Color.FromArgb(8, 52, 90); lblTitulo.Location = new Point(28, 20); lblTitulo.Text = "Gestión de veterinarios";
        lblDescripcion.AutoSize = true; lblDescripcion.ForeColor = Color.FromArgb(104, 115, 125); lblDescripcion.Location = new Point(31, 65); lblDescripcion.Text = "Administra la información del personal veterinario";
        lblBuscar.AutoSize = true; lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblBuscar.Location = new Point(672, 10); lblBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right; lblBuscar.Text = "Buscar";
        txtBuscar = new TextBox(); txtBuscar.Font = new Font("Segoe UI", 10F); txtBuscar.Location = new Point(672, 30); txtBuscar.Anchor = AnchorStyles.Top | AnchorStyles.Right; txtBuscar.Size = new Size(240, 30);
        txtBuscar.PlaceholderText = "Buscar veterinario...";
        txtNombres = new TextBox(); txtNombres.Font = new Font("Segoe UI", 10F); txtNombres.Location = new Point(30, 140); txtNombres.Size = new Size(225, 30);
        txtApellidos = new TextBox(); txtApellidos.Font = new Font("Segoe UI", 10F); txtApellidos.Location = new Point(275, 140); txtApellidos.Size = new Size(225, 30);
        txtEspecialidad = new TextBox(); txtEspecialidad.Font = new Font("Segoe UI", 10F); txtEspecialidad.Location = new Point(520, 140); txtEspecialidad.Size = new Size(250, 30);
        txtTelefono = new TextBox(); txtTelefono.Font = new Font("Segoe UI", 10F); txtTelefono.Location = new Point(790, 140); txtTelefono.Size = new Size(215, 30);
        btnNuevo = new Button(); btnNuevo.Location = new Point(30, 100); btnNuevo.Size = new Size(110, 38); btnNuevo.Text = "Nuevo";
        btnGuardar = new Button(); btnGuardar.Location = new Point(150, 100); btnGuardar.Size = new Size(110, 38); btnGuardar.Text = "Guardar";
        btnGuardar.Visible = false;
        btnEditar = new Button(); btnEditar.Location = new Point(150, 100); btnEditar.Size = new Size(120, 38); btnEditar.Text = "Modificar";
        btnDesactivar = new Button(); btnDesactivar.Location = new Point(280, 100); btnDesactivar.Size = new Size(120, 38); btnDesactivar.Text = "Desactivar";
        chkMostrarInactivos = new CheckBox();
        chkMostrarInactivos.AutoSize = true; chkMostrarInactivos.Location = new Point(430, 111); chkMostrarInactivos.Text = "Mostrar desactivados";
        tabla = new DataGridView();
        tabla.Location = new Point(30, 160); tabla.Size = new Size(882, 412); tabla.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabla.AllowUserToAddRows = false; tabla.AllowUserToDeleteRows = false; tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; tabla.BackgroundColor = Color.White; tabla.ReadOnly = true; tabla.RowHeadersVisible = false; tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        txtBuscar.TextChanged += Buscar;
        btnNuevo.Click += Nuevo;
        btnGuardar.Click += Guardar;
        btnEditar.Click += Editar;
        btnDesactivar.Click += Desactivar;
        tabla.SelectionChanged += Seleccionar;
        chkMostrarInactivos.CheckedChanged += MostrarInactivos;
        Controls.AddRange(new Control[] { lblTitulo, lblDescripcion, lblBuscar, txtBuscar,
            btnNuevo, btnEditar, btnDesactivar, chkMostrarInactivos, tabla });
        BackColor = Color.FromArgb(248, 250, 252);
        Font = new Font("Segoe UI", 9F);
        AutoScaleDimensions = new SizeF(96F, 96F); AutoScaleMode = AutoScaleMode.Dpi;
    }

}
