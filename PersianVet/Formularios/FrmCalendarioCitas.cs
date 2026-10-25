using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Calendario mensual que resalta los días con citas y muestra el detalle del día elegido.
/// </summary>
public partial class FrmCalendarioCitas : Form
{
    private readonly CitaServicio citaServicio;
    private List<Cita> citas = [];

    public FrmCalendarioCitas() : this(null!)
    {
    }

    public FrmCalendarioCitas(CitaServicio citaServicio)
    {
        InitializeComponent();
        this.citaServicio = citaServicio;
        BackColor = Tema.Fondo;
        EstiloControles.ConfigurarTabla(tablaDia);
        EstiloControles.ConfigurarBotonSecundario(btnCerrar);
    }

    private async void FrmCalendarioCitas_Load(object? sender, EventArgs e)
    {
        lblEstado.Text = "Cargando citas...";
        try
        {
            citas = await Task.Run(() => citaServicio.Listar());
            if (IsDisposed)
            {
                return;
            }

            // Los días con citas activas se marcan en negrita.
            calendario.BoldedDates = citas
                .Where(cita => cita.Estado != "Cancelada")
                .Select(cita => cita.FechaHora.Date)
                .Distinct()
                .ToArray();
            calendario.UpdateBoldedDates();

            MostrarDia(DateTime.Today);
            lblEstado.Text = $"{citas.Count(cita => cita.Estado != "Cancelada")} citas activas en total.";
        }
        catch (Exception excepcion)
        {
            lblEstado.Text = "No fue posible cargar las citas.";
            Mensajes.MostrarError(excepcion);
        }
    }

    private void Calendario_DateSelected(object? sender, DateRangeEventArgs e)
    {
        MostrarDia(e.Start.Date);
    }

    private void MostrarDia(DateTime dia)
    {
        lblDia.Text = $"Citas del {dia:dddd d 'de' MMMM}";
        tablaDia.DataSource = citas
            .Where(cita => cita.FechaHora.Date == dia)
            .OrderBy(cita => cita.FechaHora)
            .Select(cita => new
            {
                Hora = cita.FechaHora.ToString("HH:mm"),
                cita.Mascota,
                cita.Cliente,
                cita.Veterinario,
                cita.Motivo,
                cita.Estado
            })
            .ToList();
    }
}
