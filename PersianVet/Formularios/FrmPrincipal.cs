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
    private bool cerrandoSesion;

    /// <summary>
    /// Indica si el usuario eligió regresar a la pantalla de acceso.
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

        lblUsuario.Text = usuario.NombreUsuario;
        lblRol.Text = usuario.Rol;
        pictureMascota.Image = RecursosImagen.CargarMascota();
        AsignarIconos();
        AcomodarEncabezado();
        AcomodarMenu();
        ActualizarFechaHora();
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

        foreach (Button boton in panelMenu.Controls.OfType<Button>())
        {
            boton.BackColor = boton == botonActivo ? Tema.Naranja : Color.White;
            boton.ForeColor = boton == botonActivo ? Color.White : Tema.AzulOscuro;
        }
    }

    private void MostrarDashboard()
    {
        MostrarControl(new UcDashboard(dashboardServicio), btnInicio);
    }

    private void BtnInicio_Click(object? sender, EventArgs e) => MostrarDashboard();

    private void BtnClientes_Click(object? sender, EventArgs e)
    {
        MostrarControl(new UcClientes(clienteServicio), btnClientes);
    }

    private void BtnMascotas_Click(object? sender, EventArgs e)
    {
        MostrarControl(new UcMascotas(mascotaServicio, clienteServicio), btnMascotas);
    }

    private void BtnCitas_Click(object? sender, EventArgs e)
    {
        MostrarControl(new UcCitas(citaServicio, mascotaServicio, veterinarioServicio), btnCitas);
    }

    private void BtnConsultas_Click(object? sender, EventArgs e)
    {
        MostrarControl(new UcConsultas(consultaServicio, mascotaServicio, veterinarioServicio), btnConsultas);
    }

    private void BtnVacunas_Click(object? sender, EventArgs e)
    {
        MostrarControl(new UcVacunas(vacunaServicio, mascotaServicio), btnVacunas);
    }

    private void BtnVeterinarios_Click(object? sender, EventArgs e)
    {
        MostrarControl(new UcVeterinarios(veterinarioServicio), btnVeterinarios);
    }

    private void BtnReportes_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(
            "El módulo de reportes se incluirá en el siguiente entregable.",
            "PersianVet",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
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
        lblFechaHora.Location = new Point(Math.Max(540, panelEncabezado.ClientSize.Width - 505), 22);
        lblUsuario.Location = new Point(Math.Max(880, panelEncabezado.ClientSize.Width - 165), 17);
        lblRol.Location = new Point(Math.Max(880, panelEncabezado.ClientSize.Width - 165), 43);
    }

    private void AcomodarMenu()
    {
        btnCerrarSesion.Location = new Point(14, Math.Max(520, panelMenu.ClientSize.Height - 58));
        pictureMascota.Location = new Point(34, Math.Max(400, panelMenu.ClientSize.Height - 190));
    }

    private void AsignarIconos()
    {
        Button[] botones = [btnInicio, btnClientes, btnMascotas, btnCitas, btnConsultas, btnVacunas, btnVeterinarios, btnReportes, btnCerrarSesion];
        foreach (Button boton in botones)
        {
            boton.Image = Iconos.Crear(boton.Text, Tema.AzulOscuro);
            boton.ImageAlign = ContentAlignment.MiddleLeft;
            boton.TextImageRelation = TextImageRelation.ImageBeforeText;
            boton.Padding = new Padding(8, 0, 0, 0);
        }
    }
}
