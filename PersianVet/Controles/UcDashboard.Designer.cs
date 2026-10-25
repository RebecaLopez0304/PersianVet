namespace PersianVet.Controles;

partial class UcDashboard
{
    private TableLayoutPanel raiz = null!, filaTarjetas = null!, contenido = null!, columnaCitas = null!, columnaDerecha = null!, columnaAlertas = null!;
    private Panel panelTitulo = null!;
    private Label lblTitulo = null!, lblDescripcion = null!, lblProximas = null!, lblAlertas = null!, lblEstado = null!;
    private TableLayoutPanel tarjetaMascotas = null!, tarjetaCitas = null!, tarjetaVacunas = null!, tarjetaConsultas = null!;
    private Label lblMascotasTitulo = null!, lblCitasTitulo = null!, lblVacunasTitulo = null!, lblConsultasTitulo = null!;
    private Label lblMascotasValor = null!, lblCitasValor = null!, lblVacunasValor = null!, lblConsultasValor = null!;
    private Panel barraMascotas = null!, barraCitas = null!, barraVacunas = null!, barraConsultas = null!;
    private FlowLayoutPanel panelAcciones = null!;
    private Button btnNuevaCita = null!, btnCalendario = null!, btnNuevoCliente = null!, btnNuevaMascota = null!;
    private Button btnNuevaConsulta = null!, btnNuevaVacuna = null!, btnActualizar = null!;
    private DataGridView tablaCitas = null!;
    private ListBox listaAlertas = null!;
    private GraficoBarras graficoCitas = null!;

    private void InitializeComponent()
    {
        raiz = new TableLayoutPanel(); filaTarjetas = new TableLayoutPanel(); contenido = new TableLayoutPanel();
        columnaCitas = new TableLayoutPanel(); columnaDerecha = new TableLayoutPanel(); columnaAlertas = new TableLayoutPanel();
        panelTitulo = new Panel(); panelAcciones = new FlowLayoutPanel();
        lblTitulo = new Label(); lblDescripcion = new Label(); lblProximas = new Label(); lblAlertas = new Label(); lblEstado = new Label();
        tarjetaMascotas = CrearTarjeta(out barraMascotas, out lblMascotasTitulo, out lblMascotasValor, "Mascotas registradas", Color.FromArgb(242, 140, 40));
        tarjetaCitas = CrearTarjeta(out barraCitas, out lblCitasTitulo, out lblCitasValor, "Citas de hoy", Color.FromArgb(31, 78, 121));
        tarjetaVacunas = CrearTarjeta(out barraVacunas, out lblVacunasTitulo, out lblVacunasValor, "Vacunas próximas", Color.FromArgb(91, 140, 90));
        tarjetaConsultas = CrearTarjeta(out barraConsultas, out lblConsultasTitulo, out lblConsultasValor, "Consultas este mes", Color.FromArgb(242, 140, 40));
        btnNuevaCita = new Button(); btnCalendario = new Button(); btnNuevoCliente = new Button(); btnNuevaMascota = new Button();
        btnNuevaConsulta = new Button(); btnNuevaVacuna = new Button(); btnActualizar = new Button();
        tablaCitas = new DataGridView(); listaAlertas = new ListBox(); graficoCitas = new GraficoBarras();
        ((System.ComponentModel.ISupportInitialize)tablaCitas).BeginInit();
        SuspendLayout();

        // Estructura principal: título, tarjetas, acciones rápidas, contenido y barra de estado.
        raiz.ColumnCount = 1;
        raiz.RowCount = 5;
        raiz.Dock = DockStyle.Fill;
        raiz.Padding = new Padding(24, 12, 24, 10);
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 76F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        raiz.Controls.Add(panelTitulo, 0, 0);
        raiz.Controls.Add(filaTarjetas, 0, 1);
        raiz.Controls.Add(panelAcciones, 0, 2);
        raiz.Controls.Add(contenido, 0, 3);
        raiz.Controls.Add(lblEstado, 0, 4);

        panelTitulo.Dock = DockStyle.Fill;
        panelTitulo.Controls.Add(lblTitulo);
        panelTitulo.Controls.Add(lblDescripcion);
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 25F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(8, 52, 90);
        lblTitulo.Location = new Point(0, 0);
        lblTitulo.Text = "Dashboard";
        lblDescripcion.AutoSize = true;
        lblDescripcion.ForeColor = Color.FromArgb(94, 103, 112);
        lblDescripcion.Location = new Point(4, 52);
        lblDescripcion.Text = "Resumen general de la clínica. Haga clic en una tarjeta para abrir su módulo.";

        // Tarjetas de resumen (cuatro columnas iguales).
        filaTarjetas.ColumnCount = 4;
        filaTarjetas.RowCount = 1;
        filaTarjetas.Dock = DockStyle.Fill;
        for (int i = 0; i < 4; i++)
        {
            filaTarjetas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        }

        filaTarjetas.Controls.Add(tarjetaMascotas, 0, 0);
        filaTarjetas.Controls.Add(tarjetaCitas, 1, 0);
        filaTarjetas.Controls.Add(tarjetaVacunas, 2, 0);
        filaTarjetas.Controls.Add(tarjetaConsultas, 3, 0);

        // Acciones rápidas.
        panelAcciones.Dock = DockStyle.Fill;
        panelAcciones.WrapContents = false;
        panelAcciones.Padding = new Padding(0, 8, 0, 0);
        ConfigurarAccion(btnNuevaCita, "Nueva cita", 105);
        ConfigurarAccion(btnCalendario, "Calendario", 100);
        ConfigurarAccion(btnNuevoCliente, "Nuevo cliente", 110);
        ConfigurarAccion(btnNuevaMascota, "Nueva mascota", 115);
        ConfigurarAccion(btnNuevaConsulta, "Nueva consulta", 120);
        ConfigurarAccion(btnNuevaVacuna, "Nueva vacuna", 110);
        ConfigurarAccion(btnActualizar, "Actualizar", 95);
        panelAcciones.Controls.AddRange(new Control[] { btnNuevaCita, btnCalendario, btnNuevoCliente, btnNuevaMascota, btnNuevaConsulta, btnNuevaVacuna, btnActualizar });
        btnNuevaCita.Click += (_, _) => Navegar("Citas");
        btnCalendario.Click += (_, _) => Navegar("Calendario");
        btnNuevoCliente.Click += (_, _) => Navegar("NuevoCliente");
        btnNuevaMascota.Click += (_, _) => Navegar("NuevaMascota");
        btnNuevaConsulta.Click += (_, _) => Navegar("Consultas");
        btnNuevaVacuna.Click += (_, _) => Navegar("Vacunas");
        btnActualizar.Click += BtnActualizar_Click;

        // Zona inferior: próximas citas a la izquierda; avisos y gráfico a la derecha.
        contenido.ColumnCount = 2;
        contenido.RowCount = 1;
        contenido.Dock = DockStyle.Fill;
        contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58F));
        contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42F));
        contenido.Controls.Add(columnaCitas, 0, 0);
        contenido.Controls.Add(columnaDerecha, 1, 0);

        columnaCitas.ColumnCount = 1;
        columnaCitas.RowCount = 2;
        columnaCitas.Dock = DockStyle.Fill;
        columnaCitas.Margin = new Padding(0, 0, 12, 0);
        columnaCitas.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        columnaCitas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        columnaCitas.Controls.Add(lblProximas, 0, 0);
        columnaCitas.Controls.Add(tablaCitas, 0, 1);
        lblProximas.Dock = DockStyle.Fill;
        lblProximas.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblProximas.ForeColor = Color.FromArgb(8, 52, 90);
        lblProximas.Text = "Próximas citas";
        tablaCitas.Dock = DockStyle.Fill;

        columnaDerecha.ColumnCount = 1;
        columnaDerecha.RowCount = 2;
        columnaDerecha.Dock = DockStyle.Fill;
        columnaDerecha.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        columnaDerecha.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        columnaDerecha.Controls.Add(columnaAlertas, 0, 0);
        columnaDerecha.Controls.Add(graficoCitas, 0, 1);

        columnaAlertas.ColumnCount = 1;
        columnaAlertas.RowCount = 2;
        columnaAlertas.Dock = DockStyle.Fill;
        columnaAlertas.Margin = new Padding(0, 0, 0, 6);
        columnaAlertas.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
        columnaAlertas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        columnaAlertas.Controls.Add(lblAlertas, 0, 0);
        columnaAlertas.Controls.Add(listaAlertas, 0, 1);
        lblAlertas.Dock = DockStyle.Fill;
        lblAlertas.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblAlertas.ForeColor = Color.FromArgb(8, 52, 90);
        lblAlertas.Text = "Requiere atención";
        listaAlertas.BorderStyle = BorderStyle.None;
        listaAlertas.Dock = DockStyle.Fill;
        listaAlertas.Font = new Font("Segoe UI", 9.5F);
        listaAlertas.IntegralHeight = false;
        listaAlertas.ItemHeight = 22;
        listaAlertas.DoubleClick += ListaAlertas_DoubleClick;

        graficoCitas.Dock = DockStyle.Fill;
        graficoCitas.Margin = new Padding(0, 6, 0, 0);
        graficoCitas.Titulo = "Citas de los próximos 7 días";

        lblEstado.BackColor = Color.FromArgb(235, 246, 235);
        lblEstado.Dock = DockStyle.Fill;
        lblEstado.ForeColor = Color.FromArgb(91, 140, 90);
        lblEstado.Padding = new Padding(12, 0, 0, 0);
        lblEstado.Text = "Cargando información...";
        lblEstado.TextAlign = ContentAlignment.MiddleLeft;

        Controls.Add(raiz);
        BackColor = Color.FromArgb(248, 250, 252);
        Font = new Font("Segoe UI", 9F);
        Size = new Size(940, 592);
        ((System.ComponentModel.ISupportInitialize)tablaCitas).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    /// <summary>
    /// Crea una tarjeta de resumen: barra de color, título y valor.
    /// </summary>
    private static TableLayoutPanel CrearTarjeta(out Panel barra, out Label titulo, out Label valor, string texto, Color color)
    {
        barra = new Panel { BackColor = color, Dock = DockStyle.Fill };
        titulo = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(8, 52, 90),
            Text = texto,
            TextAlign = ContentAlignment.BottomCenter
        };
        valor = new Label
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 27F, FontStyle.Bold),
            ForeColor = color,
            Text = "0",
            TextAlign = ContentAlignment.MiddleCenter
        };

        TableLayoutPanel tarjeta = new()
        {
            BackColor = Color.White,
            ColumnCount = 1,
            RowCount = 3,
            Dock = DockStyle.Fill,
            Margin = new Padding(0, 0, 12, 0)
        };
        tarjeta.RowStyles.Add(new RowStyle(SizeType.Absolute, 5F));
        tarjeta.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tarjeta.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tarjeta.Controls.Add(barra, 0, 0);
        tarjeta.Controls.Add(titulo, 0, 1);
        tarjeta.Controls.Add(valor, 0, 2);
        return tarjeta;
    }

    private static void ConfigurarAccion(Button boton, string texto, int ancho)
    {
        boton.Size = new Size(ancho, 36);
        boton.Margin = new Padding(0, 0, 6, 0);
        boton.Text = texto;
    }
}
