using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Reúne en una sola pantalla los datos, consultas, vacunas y citas de una mascota.
/// </summary>
public partial class FrmFichaMascota : Form
{
    private readonly Mascota mascota;
    private readonly ConsultaServicio consultaServicio;
    private readonly VacunaServicio vacunaServicio;
    private readonly CitaServicio citaServicio;

    public FrmFichaMascota() : this(new Mascota(), null!, null!, null!)
    {
    }

    public FrmFichaMascota(
        Mascota mascota,
        ConsultaServicio consultaServicio,
        VacunaServicio vacunaServicio,
        CitaServicio citaServicio)
    {
        InitializeComponent();
        this.mascota = mascota;
        this.consultaServicio = consultaServicio;
        this.vacunaServicio = vacunaServicio;
        this.citaServicio = citaServicio;

        BackColor = Tema.Fondo;
        EstiloControles.ConfigurarBotonSecundario(btnCerrar);
        foreach (DataGridView tabla in new[] { tablaConsultas, tablaVacunas, tablaCitas })
        {
            EstiloControles.ConfigurarTabla(tabla);
        }

        MostrarDatos();
    }

    private void MostrarDatos()
    {
        lblNombre.Text = mascota.Nombre;
        string observaciones = string.IsNullOrWhiteSpace(mascota.Observaciones) ? "Sin observaciones" : mascota.Observaciones;
        lblResumen.Text =
            $"{mascota.Especie} | {mascota.Raza} | {mascota.Sexo} | {CalcularEdad(mascota.FechaNacimiento)} | {mascota.Peso:0.##} kg | {mascota.Color}\r\n"
            + $"Propietario: {mascota.Propietario}\r\n"
            + $"Observaciones: {observaciones}";
    }

    private async void FrmFichaMascota_Load(object? sender, EventArgs e)
    {
        lblEstado.Text = "Cargando historial...";
        try
        {
            // Tres consultas en segundo plano al mismo tiempo; la ventana sigue respondiendo.
            Task<List<Consulta>> tareaConsultas = Task.Run(() => consultaServicio.ListarPorMascota(mascota.IdMascota));
            Task<List<Vacuna>> tareaVacunas = Task.Run(() => vacunaServicio.Listar(mascota.IdMascota));
            Task<List<Cita>> tareaCitas = Task.Run(() => citaServicio.Listar()
                .Where(cita => cita.IdMascota == mascota.IdMascota)
                .ToList());
            await Task.WhenAll(tareaConsultas, tareaVacunas, tareaCitas);

            if (IsDisposed)
            {
                return;
            }

            tablaConsultas.DataSource = tareaConsultas.Result;
            tablaVacunas.DataSource = tareaVacunas.Result;
            tablaCitas.DataSource = tareaCitas.Result;
            EstiloControles.OcultarColumnas(tablaConsultas, "IdConsulta", "IdMascota", "IdVeterinario");
            EstiloControles.OcultarColumnas(tablaVacunas, "IdVacuna", "IdMascota", "NombreMascota");
            EstiloControles.OcultarColumnas(tablaCitas, "IdCita", "IdMascota", "IdVeterinario", "NombreMascota");
            EstiloControles.FormatoMoneda(tablaConsultas, "Precio", "Total");
            EstiloControles.FormatoMoneda(tablaVacunas, "Precio");
            EstiloControles.FormatoMoneda(tablaCitas, "Precio");

            paginaConsultas.Text = $"Consultas ({tareaConsultas.Result.Count})";
            paginaVacunas.Text = $"Vacunas ({tareaVacunas.Result.Count})";
            paginaCitas.Text = $"Citas ({tareaCitas.Result.Count})";
            lblEstado.Text = "Historial cargado.";
        }
        catch (Exception excepcion)
        {
            lblEstado.Text = "No fue posible cargar el historial.";
            Mensajes.MostrarError(excepcion);
        }
    }

    private static string CalcularEdad(DateTime nacimiento)
    {
        int meses = (DateTime.Today.Year - nacimiento.Year) * 12 + DateTime.Today.Month - nacimiento.Month;
        if (DateTime.Today.Day < nacimiento.Day)
        {
            meses--;
        }

        meses = Math.Max(0, meses);
        return meses < 12 ? $"{meses} meses" : $"{meses / 12} años";
    }
}
