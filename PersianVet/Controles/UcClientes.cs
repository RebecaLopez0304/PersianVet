using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;
using PersianVet.Formularios;

namespace PersianVet.Controles;

/// <summary>
/// Permite registrar, consultar y modificar clientes.
/// </summary>
public partial class UcClientes : UserControl
{
    private readonly ClienteServicio clienteServicio;
    private int idSeleccionado;

    public UcClientes()
    {
        InitializeComponent();
        EstiloControles.ConfigurarTabla(tablaClientes);
        EstiloControles.ConfigurarBotonPrincipal(btnNuevo);
        EstiloControles.ConfigurarBotonPrincipal(btnGuardar);
        EstiloControles.ConfigurarBotonSecundario(btnEditar);
        EstiloControles.ConfigurarBotonSecundario(btnDesactivar);
        chkMostrarInactivos.CheckedChanged += ChkMostrarInactivos_CheckedChanged;
        clienteServicio = null!;
    }

    public UcClientes(ClienteServicio clienteServicio) : this()
    {
        this.clienteServicio = clienteServicio;
        Load += UcClientes_Load;
    }

    private void UcClientes_Load(object? sender, EventArgs e) => CargarClientes();

    private void TxtBuscar_TextChanged(object? sender, EventArgs e) => CargarClientes();

    private void ChkMostrarInactivos_CheckedChanged(object? sender, EventArgs e) => CargarClientes();

    private void BtnNuevo_Click(object? sender, EventArgs e) => NuevoRegistro();

    /// <summary>
    /// Abre el formulario para registrar un cliente (también se usa desde las acciones rápidas).
    /// </summary>
    public void NuevoRegistro()
    {
        using FrmClienteEdicion formulario = new();
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            EjecutarOperacion(() => clienteServicio.Guardar(formulario.Cliente));
        }
    }

    private void BtnGuardar_Click(object? sender, EventArgs e)
    {
        EjecutarOperacion(() => clienteServicio.Guardar(CrearCliente()));
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        if (tablaClientes.CurrentRow?.DataBoundItem is not Cliente seleccionado)
        {
            MessageBox.Show("Seleccione un cliente de la lista.", "PersianVet");
            return;
        }

        Cliente cliente = CopiarCliente(seleccionado);
        using FrmClienteEdicion formulario = new(cliente);
        if (formulario.ShowDialog(this) == DialogResult.OK)
        {
            EjecutarOperacion(() => clienteServicio.Actualizar(formulario.Cliente));
        }
    }

    private void BtnDesactivar_Click(object? sender, EventArgs e)
    {
        if (idSeleccionado <= 0)
        {
            MessageBox.Show("Seleccione un cliente de la lista.", "PersianVet");
            return;
        }

        DialogResult respuesta = MessageBox.Show(
            "¿Desea desactivar al cliente seleccionado?",
            "PersianVet",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (respuesta == DialogResult.Yes)
        {
            try
            {
                clienteServicio.Desactivar(idSeleccionado);
                LimpiarFormulario();
                CargarClientes();
            }
            catch (Exception excepcion)
            {
                Mensajes.MostrarError(excepcion);
            }
        }
    }

    private void TablaClientes_SelectionChanged(object? sender, EventArgs e)
    {
        if (tablaClientes.CurrentRow?.DataBoundItem is not Cliente cliente)
        {
            return;
        }

        idSeleccionado = cliente.IdCliente;
        txtNombres.Text = cliente.Nombres;
        txtApellidos.Text = cliente.Apellidos;
        txtTelefono.Text = cliente.Telefono;
        txtCorreo.Text = cliente.Correo;
        txtDireccion.Text = cliente.Direccion;
        dtpFechaRegistro.Value = cliente.FechaRegistro;
    }

    private Cliente CrearCliente()
    {
        return new Cliente
        {
            IdCliente = idSeleccionado,
            Nombres = txtNombres.Text,
            Apellidos = txtApellidos.Text,
            Telefono = txtTelefono.Text,
            Correo = txtCorreo.Text,
            Direccion = txtDireccion.Text,
            FechaRegistro = dtpFechaRegistro.Value.Date
        };
    }

    private void EjecutarOperacion(Func<ResultadoOperacion> operacion)
    {
        try
        {
            ResultadoOperacion resultado = operacion();
            MessageBox.Show(
                resultado.Mensaje,
                "PersianVet",
                MessageBoxButtons.OK,
                resultado.Exitoso ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (resultado.Exitoso)
            {
                LimpiarFormulario();
                CargarClientes();
            }
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void CargarClientes()
    {
        try
        {
            tablaClientes.DataSource = clienteServicio.Listar(txtBuscar.Text, !chkMostrarInactivos.Checked);
            if (tablaClientes.Columns.Contains("IdCliente")) tablaClientes.Columns["IdCliente"].HeaderText = "ID";
            if (tablaClientes.Columns.Contains("NombreCompleto")) tablaClientes.Columns["NombreCompleto"].Visible = false;
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void LimpiarFormulario()
    {
        idSeleccionado = 0;
        txtNombres.Clear();
        txtApellidos.Clear();
        txtTelefono.Clear();
        txtCorreo.Clear();
        txtDireccion.Clear();
        dtpFechaRegistro.Value = DateTime.Today;
        tablaClientes.ClearSelection();
        txtNombres.Focus();
    }

    private static Cliente CopiarCliente(Cliente cliente) => new()
    {
        IdCliente = cliente.IdCliente, Nombres = cliente.Nombres, Apellidos = cliente.Apellidos,
        Telefono = cliente.Telefono, Correo = cliente.Correo, Direccion = cliente.Direccion,
        FechaRegistro = cliente.FechaRegistro, Activo = cliente.Activo
    };
}
