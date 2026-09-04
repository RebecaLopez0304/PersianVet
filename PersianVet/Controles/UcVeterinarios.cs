using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;
using PersianVet.Formularios;

namespace PersianVet.Controles;

/// <summary>
/// Permite registrar, consultar y modificar veterinarios.
/// </summary>
public partial class UcVeterinarios : UserControl
{
    private readonly VeterinarioServicio servicio;
    private int idSeleccionado;

    public UcVeterinarios()
    {
        InitializeComponent();
        EstiloControles.ConfigurarTabla(tabla);
        EstiloControles.ConfigurarBotonPrincipal(btnNuevo);
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnEditar);
        EstiloControles.ConfigurarBotonSecundario(btnDesactivar);
        servicio = null!;
    }

    public UcVeterinarios(VeterinarioServicio servicio) : this()
    {
        this.servicio = servicio;
        Load += (_, _) => CargarDatos();
    }

    private void Buscar(object? sender, EventArgs e) => CargarDatos();
    private void MostrarInactivos(object? sender, EventArgs e) => CargarDatos();
    private void Nuevo(object? sender, EventArgs e)
    {
        using FrmVeterinarioEdicion formulario = new();
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            Ejecutar(() => servicio.Guardar(formulario.Veterinario));
        }
    }
    private void Guardar(object? sender, EventArgs e) => Ejecutar(() => servicio.Guardar(Crear()));
    private void Editar(object? sender, EventArgs e)
    {
        if (tabla.CurrentRow?.DataBoundItem is not Veterinario seleccionado)
        {
            MessageBox.Show("Seleccione un veterinario de la lista.", "PersianVet");
            return;
        }

        Veterinario veterinario = new()
        {
            IdVeterinario = seleccionado.IdVeterinario, Nombres = seleccionado.Nombres,
            Apellidos = seleccionado.Apellidos, Especialidad = seleccionado.Especialidad,
            Telefono = seleccionado.Telefono, Activo = seleccionado.Activo
        };
        using FrmVeterinarioEdicion formulario = new(veterinario);
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            Ejecutar(() => servicio.Actualizar(formulario.Veterinario));
        }
    }

    private void Desactivar(object? sender, EventArgs e)
    {
        if (idSeleccionado <= 0)
        {
            MessageBox.Show("Seleccione un veterinario de la lista.", "PersianVet");
            return;
        }
        if (MessageBox.Show("¿Desea desactivar al veterinario?", "PersianVet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        try { servicio.Desactivar(idSeleccionado); Limpiar(); CargarDatos(); }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void Seleccionar(object? sender, EventArgs e)
    {
        if (tabla.CurrentRow?.DataBoundItem is not Veterinario veterinario) return;
        idSeleccionado = veterinario.IdVeterinario;
        txtNombres.Text = veterinario.Nombres; txtApellidos.Text = veterinario.Apellidos;
        txtEspecialidad.Text = veterinario.Especialidad; txtTelefono.Text = veterinario.Telefono;
    }

    private Veterinario Crear() => new()
    {
        IdVeterinario = idSeleccionado, Nombres = txtNombres.Text, Apellidos = txtApellidos.Text,
        Especialidad = txtEspecialidad.Text, Telefono = txtTelefono.Text
    };

    private void Ejecutar(Func<ResultadoOperacion> operacion)
    {
        try
        {
            ResultadoOperacion resultado = operacion();
            MessageBox.Show(resultado.Mensaje, "PersianVet", MessageBoxButtons.OK, resultado.Exitoso ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            if (resultado.Exitoso) { Limpiar(); CargarDatos(); }
        }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void CargarDatos()
    {
        try { tabla.DataSource = servicio.Listar(txtBuscar.Text, !chkMostrarInactivos.Checked); }
        catch (Exception excepcion) { Mensajes.MostrarError(excepcion); }
    }

    private void Limpiar()
    {
        idSeleccionado = 0; txtNombres.Clear(); txtApellidos.Clear(); txtEspecialidad.Clear(); txtTelefono.Clear(); tabla.ClearSelection();
    }
}
