using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;
using PersianVet.Formularios;

namespace PersianVet.Controles;

/// <summary>
/// Permite registrar, consultar y modificar mascotas.
/// </summary>
public partial class UcMascotas : UserControl
{
    private readonly MascotaServicio mascotaServicio;
    private readonly ClienteServicio clienteServicio;
    private int idSeleccionado;

    /// <summary>
    /// Se produce cuando el usuario pide ver la ficha completa de una mascota.
    /// </summary>
    public event Action<Mascota>? FichaSolicitada;

    public UcMascotas()
    {
        InitializeComponent();
        EstiloControles.ConfigurarTabla(tablaMascotas);
        EstiloControles.ConfigurarBotonPrincipal(btnNuevo);
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnEditar);
        EstiloControles.ConfigurarBotonSecundario(btnDesactivar);
        EstiloControles.ConfigurarBotonSecundario(btnFicha);
        mascotaServicio = null!;
        clienteServicio = null!;
    }

    public UcMascotas(MascotaServicio mascotaServicio, ClienteServicio clienteServicio) : this()
    {
        this.mascotaServicio = mascotaServicio;
        this.clienteServicio = clienteServicio;
        Load += UcMascotas_Load;
    }

    private void UcMascotas_Load(object? sender, EventArgs e)
    {
        CargarPropietarios();
        CargarMascotas();
    }

    private void TxtBuscar_TextChanged(object? sender, EventArgs e) => CargarMascotas();
    private void ChkMostrarInactivas_CheckedChanged(object? sender, EventArgs e) => CargarMascotas();
    private void BtnNuevo_Click(object? sender, EventArgs e) => NuevoRegistro();

    private void BtnFicha_Click(object? sender, EventArgs e) => AbrirFicha();
    private void TablaMascotas_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) AbrirFicha();
    }

    private void AbrirFicha()
    {
        if (tablaMascotas.CurrentRow?.DataBoundItem is not Mascota seleccionada)
        {
            MessageBox.Show("Seleccione una mascota de la lista.", "PersianVet");
            return;
        }

        FichaSolicitada?.Invoke(seleccionada);
    }

    /// <summary>
    /// Abre el formulario para registrar una mascota (también se usa desde las acciones rápidas).
    /// </summary>
    public void NuevoRegistro()
    {
        using FrmMascotaEdicion formulario = new(clienteServicio.Listar(string.Empty));
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            Ejecutar(() => mascotaServicio.Guardar(formulario.Mascota));
        }
    }
    private void BtnGuardar_Click(object? sender, EventArgs e) => Ejecutar(() => mascotaServicio.Guardar(CrearMascota()));
    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        if (tablaMascotas.CurrentRow?.DataBoundItem is not Mascota seleccionada)
        {
            MessageBox.Show("Seleccione una mascota de la lista.", "PersianVet");
            return;
        }

        using FrmMascotaEdicion formulario = new(clienteServicio.Listar(string.Empty), CopiarMascota(seleccionada));
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            Ejecutar(() => mascotaServicio.Actualizar(formulario.Mascota));
        }
    }

    private void BtnDesactivar_Click(object? sender, EventArgs e)
    {
        if (idSeleccionado <= 0)
        {
            MessageBox.Show("Seleccione una mascota de la lista.", "PersianVet");
            return;
        }

        if (MessageBox.Show("¿Desea desactivar la mascota?", "PersianVet", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                mascotaServicio.Desactivar(idSeleccionado);
                LimpiarFormulario();
                CargarMascotas();
            }
            catch (Exception excepcion)
            {
                Mensajes.MostrarError(excepcion);
            }
        }
    }

    private void TablaMascotas_SelectionChanged(object? sender, EventArgs e)
    {
        if (tablaMascotas.CurrentRow?.DataBoundItem is not Mascota mascota)
        {
            return;
        }

        idSeleccionado = mascota.IdMascota;
        cboPropietario.SelectedValue = mascota.IdCliente;
        txtNombre.Text = mascota.Nombre;
        cboEspecie.Text = mascota.Especie;
        txtRaza.Text = mascota.Raza;
        cboSexo.Text = mascota.Sexo;
        dtpNacimiento.Value = mascota.FechaNacimiento;
        txtColor.Text = mascota.Color;
        numPeso.Value = Math.Min(numPeso.Maximum, mascota.Peso);
        txtObservaciones.Text = mascota.Observaciones;
    }

    private Mascota CrearMascota()
    {
        return new Mascota
        {
            IdMascota = idSeleccionado,
            IdCliente = cboPropietario.SelectedValue is int idCliente ? idCliente : 0,
            Nombre = txtNombre.Text,
            Especie = cboEspecie.Text,
            Raza = txtRaza.Text,
            Sexo = cboSexo.Text,
            FechaNacimiento = dtpNacimiento.Value.Date,
            Color = txtColor.Text,
            Peso = numPeso.Value,
            Observaciones = txtObservaciones.Text
        };
    }

    private void Ejecutar(Func<ResultadoOperacion> operacion)
    {
        Mensajes.EjecutarOperacion(operacion, () => { LimpiarFormulario(); CargarMascotas(); });
    }

    private void CargarPropietarios()
    {
        cboPropietario.DataSource = clienteServicio.Listar(string.Empty).Where(cliente => cliente.Activo).ToList();
        cboPropietario.DisplayMember = "NombreCompleto";
        cboPropietario.ValueMember = "IdCliente";
        cboPropietario.SelectedIndex = -1;
    }

    private void CargarMascotas()
    {
        try
        {
            tablaMascotas.DataSource = mascotaServicio.Listar(txtBuscar.Text, !chkMostrarInactivas.Checked);
            EstiloControles.OcultarColumnas(tablaMascotas, "IdCliente", "Observaciones");
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void LimpiarFormulario()
    {
        idSeleccionado = 0;
        cboPropietario.SelectedIndex = -1;
        txtNombre.Clear();
        cboEspecie.SelectedIndex = 0;
        txtRaza.Clear();
        cboSexo.SelectedIndex = -1;
        dtpNacimiento.Value = DateTime.Today;
        txtColor.Clear();
        numPeso.Value = 1;
        txtObservaciones.Clear();
        tablaMascotas.ClearSelection();
    }

    private static Mascota CopiarMascota(Mascota mascota) => new()
    {
        IdMascota = mascota.IdMascota, IdCliente = mascota.IdCliente,
        Nombre = mascota.Nombre, Especie = mascota.Especie, Raza = mascota.Raza, Sexo = mascota.Sexo,
        FechaNacimiento = mascota.FechaNacimiento, Color = mascota.Color, Peso = mascota.Peso,
        Observaciones = mascota.Observaciones, Activa = mascota.Activa
    };
}
