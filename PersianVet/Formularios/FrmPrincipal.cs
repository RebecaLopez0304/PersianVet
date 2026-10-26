using PersianVet.Controles;
using PersianVet.Datos;
using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Contiene el menú principal y muestra los módulos del sistema.
/// </summary>
public partial class FrmPrincipal : Form
{
    private readonly Usuario usuario;
    private readonly DashboardServicio dashboardServicio;
    private readonly ClienteServicio clienteServicio;
    private readonly MascotaServicio mascotaServicio;
    private readonly VeterinarioServicio veterinarioServicio;
    private readonly CitaServicio citaServicio;
    private readonly ConsultaServicio consultaServicio;
    private readonly VacunaServicio vacunaServicio;
    private readonly ReporteServicio reporteServicio;
    private readonly BusquedaServicio busquedaServicio;
    private readonly TarifaServicio tarifaServicio;
    private bool cerrandoSesion;
    private readonly List<Button> botonesPermitidos = [];
    private readonly Dictionary<Button, (Image? Normal, Image? Activo)> iconosMenu = [];

    /// <summary>
    /// Obtiene un valor que indica si el usuario eligió regresar a la pantalla de acceso.
    /// </summary>
    public bool SesionCerrada => cerrandoSesion;

    /// <summary>
    /// Crea el formulario para el diseñador visual de Visual Studio.
    /// </summary>
    public FrmPrincipal()
    {
        InitializeComponent();
        usuario = null!;
        dashboardServicio = null!;
        clienteServicio = null!;
        mascotaServicio = null!;
        veterinarioServicio = null!;
        citaServicio = null!;
        consultaServicio = null!;
        vacunaServicio = null!;
        reporteServicio = null!;
        busquedaServicio = null!;
        tarifaServicio = null!;
    }

    public FrmPrincipal(Usuario usuario, ConexionBD conexionBD) : this()
    {
        this.usuario = usuario;

        clienteServicio = new ClienteServicio(new ClienteDatos(conexionBD));
        mascotaServicio = new MascotaServicio(new MascotaDatos(conexionBD));
        veterinarioServicio = new VeterinarioServicio(new VeterinarioDatos(conexionBD));
        citaServicio = new CitaServicio(new CitaDatos(conexionBD));
        consultaServicio = new ConsultaServicio(new ConsultaDatos(conexionBD));
        vacunaServicio = new VacunaServicio(new VacunaDatos(conexionBD));
        dashboardServicio = new DashboardServicio(new DashboardDatos(conexionBD));
        reporteServicio = new ReporteServicio(new ReporteDatos(conexionBD));
        busquedaServicio = new BusquedaServicio(new BusquedaDatos(conexionBD));
        tarifaServicio = new TarifaServicio(new TarifaDatos(conexionBD));

        lblUsuario.Text = usuario.NombreUsuario;
        lblRol.Text = usuario.Rol;
        pictureMascota.Image = RecursosImagen.CargarMascota();
        AsignarIconos();
        AplicarPermisosMenu();
        AcomodarEncabezado();
        AcomodarMenu();

        // Por debajo del tamaño de diseño los módulos se recortarían; el límite no supera la pantalla.
        Rectangle pantalla = Screen.FromControl(this).WorkingArea;
        MinimumSize = new Size(Math.Min(Width, pantalla.Width), Math.Min(Height, pantalla.Height));

        ActualizarFechaHora();
        timerReloj.Start();
        MostrarDashboard();
    }

    private void MostrarControl(Control control, Button botonActivo)
    {
        foreach (Control anterior in panelContenido.Controls)
        {
            anterior.Dispose();
        }

        panelContenido.Controls.Clear();
        EstiloControles.AplicarEstiloGeneral(control);
        control.Dock = DockStyle.Fill;
        panelContenido.Controls.Add(control);

        foreach (Button boton in panelBotonesMenu.Controls.OfType<Button>())
        {
            bool activo = boton == botonActivo;
            boton.BackColor = activo ? Tema.Naranja : Color.White;
            boton.ForeColor = activo ? Color.White : Tema.AzulOscuro;
            if (iconosMenu.TryGetValue(boton, out var iconos))
            {
                boton.Image = activo ? iconos.Activo : iconos.Normal;
            }
        }
    }

    private void MostrarDashboard() => Navegar("Inicio");

    private void BtnInicio_Click(object? sender, EventArgs e) => Navegar("Inicio");
    private void BtnClientes_Click(object? sender, EventArgs e) => Navegar("Clientes");
    private void BtnMascotas_Click(object? sender, EventArgs e) => Navegar("Mascotas");
    private void BtnCitas_Click(object? sender, EventArgs e) => Navegar("Citas");
    private void BtnConsultas_Click(object? sender, EventArgs e) => Navegar("Consultas");
    private void BtnVacunas_Click(object? sender, EventArgs e) => Navegar("Vacunas");
    private void BtnVeterinarios_Click(object? sender, EventArgs e) => Navegar("Veterinarios");
    private void BtnReportes_Click(object? sender, EventArgs e) => Navegar("Reportes");
    private void BtnTarifas_Click(object? sender, EventArgs e) => Navegar("Tarifas");

    /// <summary>
    /// Punto único de navegación: el menú, el dashboard y la búsqueda usan este método.
    /// </summary>
    private void Navegar(string destino)
    {
        if (!Permisos.PuedeAcceder(usuario.Rol, destino))
        {
            MessageBox.Show(
                $"El rol {usuario.Rol} no tiene acceso a este módulo.",
                "PersianVet",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        switch (destino)
        {
            case "Inicio":
                UcDashboard dashboard = new(dashboardServicio);
                dashboard.AplicarPermisos(modulo => Permisos.PuedeAcceder(usuario.Rol, modulo));
                // BeginInvoke evita destruir el dashboard mientras todavía atiende el clic.
                dashboard.NavegarSolicitado += solicitado => BeginInvoke(() => Navegar(solicitado));
                MostrarControl(dashboard, btnInicio);
                break;
            case "Clientes":
                MostrarControl(new UcClientes(clienteServicio), btnClientes);
                break;
            case "NuevoCliente":
                UcClientes clientes = new(clienteServicio);
                MostrarControl(clientes, btnClientes);
                BeginInvoke(clientes.NuevoRegistro);
                break;
            case "Mascotas":
                MostrarControl(CrearMascotas(), btnMascotas);
                break;
            case "NuevaMascota":
                UcMascotas mascotas = CrearMascotas();
                MostrarControl(mascotas, btnMascotas);
                BeginInvoke(mascotas.NuevoRegistro);
                break;
            case "Citas":
                MostrarControl(new UcCitas(citaServicio, mascotaServicio, veterinarioServicio, tarifaServicio), btnCitas);
                break;
            case "Calendario":
                using (FrmCalendarioCitas calendario = new(citaServicio))
                {
                    calendario.ShowDialog(this);
                }

                break;
            case "Consultas":
                MostrarControl(new UcConsultas(consultaServicio, mascotaServicio, veterinarioServicio, tarifaServicio), btnConsultas);
                break;
            case "Vacunas":
                MostrarControl(new UcVacunas(vacunaServicio, mascotaServicio, tarifaServicio), btnVacunas);
                break;
            case "Veterinarios":
                MostrarControl(new UcVeterinarios(veterinarioServicio), btnVeterinarios);
                break;
            case "Reportes":
                MostrarControl(new UcReportes(reporteServicio), btnReportes);
                break;
            case "Tarifas":
                MostrarControl(new UcTarifas(tarifaServicio, usuario.NombreUsuario), btnTarifas);
                break;
        }
    }

    private UcMascotas CrearMascotas()
    {
        UcMascotas mascotas = new(mascotaServicio, clienteServicio);
        mascotas.FichaSolicitada += AbrirFicha;
        return mascotas;
    }

    private void AbrirFicha(Mascota mascota)
    {
        using FrmFichaMascota ficha = new(mascota, consultaServicio, vacunaServicio, citaServicio);
        ficha.ShowDialog(this);
    }

    private void TxtBusqueda_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
        {
            return;
        }

        // Evita el "ding" de Windows al pulsar Enter en un cuadro de una sola línea.
        e.SuppressKeyPress = true;
        string texto = txtBusqueda.Text.Trim();
        if (texto.Length < 2)
        {
            MessageBox.Show("Escriba al menos 2 letras para buscar.", "PersianVet");
            return;
        }

        using FrmBusquedaGlobal busqueda = new(busquedaServicio, texto);
        if (busqueda.ShowDialog(this) == DialogResult.OK && busqueda.Seleccion is { } resultado)
        {
            txtBusqueda.Clear();
            AbrirResultado(resultado);
        }
    }

    private void AbrirResultado(ResultadoBusqueda resultado)
    {
        switch (resultado.Tipo)
        {
            case "Cliente":
                Navegar("Clientes");
                break;
            case "Cita":
                Navegar("Citas");
                break;
            case "Mascota":
                Mascota? mascota = mascotaServicio.Listar(string.Empty, false)
                    .FirstOrDefault(m => m.IdMascota == resultado.Id);
                if (mascota is not null)
                {
                    AbrirFicha(mascota);
                }

                break;
        }
    }

    private void BtnCerrarSesion_Click(object? sender, EventArgs e)
    {
        DialogResult respuesta = MessageBox.Show(
            "¿Desea cerrar la sesión?",
            "PersianVet",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (respuesta == DialogResult.Yes)
        {
            cerrandoSesion = true;
            Close();
        }
    }

    private void FrmPrincipal_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (cerrandoSesion)
        {
            return;
        }

        DialogResult respuesta = MessageBox.Show(
            "¿Desea salir de PersianVet?",
            "PersianVet",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (respuesta == DialogResult.No)
        {
            e.Cancel = true;
        }
    }

    private void TimerReloj_Tick(object? sender, EventArgs e)
    {
        ActualizarFechaHora();
    }

    private void ActualizarFechaHora()
    {
        lblFechaHora.Text = DateTime.Now.ToString("dddd, dd 'de' MMMM 'de' yyyy  |  hh:mm tt");
    }

    private void PanelEncabezado_Resize(object? sender, EventArgs e)
    {
        AcomodarEncabezado();
    }

    private void PanelMenu_Resize(object? sender, EventArgs e)
    {
        AcomodarMenu();
    }

    private void AcomodarEncabezado()
    {
        // Las distancias se escalan con la configuración de pantalla (100 %, 125 %, 150 %...).
        int ancho = panelEncabezado.ClientSize.Width;
        int columnaFecha = Math.Max(LogicalToDeviceUnits(540), ancho - LogicalToDeviceUnits(505));
        int columnaUsuario = Math.Max(LogicalToDeviceUnits(880), ancho - LogicalToDeviceUnits(165));
        lblFechaHora.Location = new Point(columnaFecha, LogicalToDeviceUnits(20));
        txtBusqueda.Location = new Point(columnaFecha, LogicalToDeviceUnits(52));
        lblUsuario.Location = new Point(columnaUsuario, LogicalToDeviceUnits(17));
        lblRol.Location = new Point(columnaUsuario, LogicalToDeviceUnits(43));
    }

    /// <summary>
    /// Muestra en el menú solo los módulos permitidos para el rol; el panel los apila sin huecos.
    /// </summary>
    private void AplicarPermisosMenu()
    {
        (Button Boton, string Modulo)[] opciones =
        [
            (btnInicio, "Inicio"), (btnClientes, "Clientes"), (btnMascotas, "Mascotas"), (btnCitas, "Citas"),
            (btnConsultas, "Consultas"), (btnVacunas, "Vacunas"), (btnVeterinarios, "Veterinarios"), (btnReportes, "Reportes"),
            (btnTarifas, "Tarifas")
        ];

        botonesPermitidos.Clear();
        foreach ((Button boton, string modulo) in opciones)
        {
            boton.Visible = Permisos.PuedeAcceder(usuario.Rol, modulo);
            if (boton.Visible)
            {
                botonesPermitidos.Add(boton);
            }
        }
    }

    private void AcomodarMenu()
    {
        // Si los botones no caben junto con la imagen de la mascota, se oculta la imagen.
        int alturaBotones = panelBotonesMenu.Padding.Vertical
            + botonesPermitidos.Sum(boton => boton.Height + boton.Margin.Vertical);
        int disponible = panelMenu.ClientSize.Height - panelMenu.Padding.Vertical - btnCerrarSesion.Height;
        pictureMascota.Visible = disponible - pictureMascota.Height >= alturaBotones;
    }

    private void AsignarIconos()
    {
        (Button Boton, string Archivo)[] botones =
        [
            (btnInicio, "inicio"), (btnClientes, "clientes"), (btnMascotas, "mascotas"), (btnCitas, "citas"),
            (btnConsultas, "consultas"), (btnVacunas, "vacunas"), (btnVeterinarios, "veterinarios"),
            (btnReportes, "reportes"), (btnTarifas, "tarifas"), (btnCerrarSesion, "cerrar-sesion")
        ];

        // Cada botón guarda dos versiones del icono: azul (normal) y blanco (botón seleccionado).
        int tamano = LogicalToDeviceUnits(22);
        foreach ((Button boton, string archivo) in botones)
        {
            iconosMenu[boton] = (Iconos.Cargar(archivo, Tema.AzulOscuro, tamano), Iconos.Cargar(archivo, Color.White, tamano));
            boton.Image = iconosMenu[boton].Normal;
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
            boton.Padding = new Padding(8, 0, 0, 0);
        }
    }
}
