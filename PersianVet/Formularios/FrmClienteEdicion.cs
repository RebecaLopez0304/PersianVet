using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de un cliente en una ventana separada.
/// </summary>
public class FrmClienteEdicion : Form
{
    private readonly TextBox txtNombres = CrearCaja();
    private readonly TextBox txtApellidos = CrearCaja();
    private readonly TextBox txtTelefono = CrearCaja();
    private readonly TextBox txtCorreo = CrearCaja();
    private readonly TextBox txtDireccion = CrearCaja();
    private readonly DateTimePicker dtpFecha = new() { Format = DateTimePickerFormat.Short };

    public Cliente Cliente { get; }

    public FrmClienteEdicion(Cliente? cliente = null)
    {
        Cliente = cliente ?? new Cliente();
        Text = cliente is null ? "Nuevo cliente" : "Modificar cliente";
        PrepararVentana();
        CargarDatos();
    }

    private void PrepararVentana()
    {
        TableLayoutPanel panel = new() { Dock = DockStyle.Fill, Padding = new Padding(24), ColumnCount = 2, RowCount = 7 };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        AgregarCampo(panel, "Nombres", txtNombres, 0, 0);
        AgregarCampo(panel, "Apellidos", txtApellidos, 1, 0);
        AgregarCampo(panel, "Teléfono", txtTelefono, 0, 2);
        AgregarCampo(panel, "Correo", txtCorreo, 1, 2);
        AgregarCampo(panel, "Dirección", txtDireccion, 0, 4);
        AgregarCampo(panel, "Fecha de registro", dtpFecha, 1, 4);
        Button guardar = new() { Text = "Guardar", Dock = DockStyle.Fill, Height = 40 };
        Button cancelar = new() { Text = "Cancelar", Dock = DockStyle.Fill, Height = 40, DialogResult = DialogResult.Cancel };
        EstiloControles.ConfigurarBotonPrincipal(guardar);
        EstiloControles.ConfigurarBotonSecundario(cancelar);
        guardar.Click += Guardar;
        panel.Controls.Add(cancelar, 0, 6); panel.Controls.Add(guardar, 1, 6);
        Controls.Add(panel);
        AcceptButton = guardar; CancelButton = cancelar; ClientSize = new Size(620, 350);
        BackColor = Tema.Fondo; Font = new Font("Segoe UI", 9F); FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false; StartPosition = FormStartPosition.CenterParent;
    }

    private void CargarDatos()
    {
        txtNombres.Text = Cliente.Nombres; txtApellidos.Text = Cliente.Apellidos;
        txtTelefono.Text = Cliente.Telefono; txtCorreo.Text = Cliente.Correo;
        txtDireccion.Text = Cliente.Direccion; dtpFecha.Value = Cliente.FechaRegistro;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Cliente.Nombres = txtNombres.Text; Cliente.Apellidos = txtApellidos.Text;
        Cliente.Telefono = txtTelefono.Text; Cliente.Correo = txtCorreo.Text;
        Cliente.Direccion = txtDireccion.Text; Cliente.FechaRegistro = dtpFecha.Value.Date;
        DialogResult = DialogResult.OK;
    }

    private static void AgregarCampo(TableLayoutPanel panel, string texto, Control control, int columna, int fila)
    {
        panel.Controls.Add(new Label { Text = texto, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = Tema.GrisOscuro }, columna, fila);
        control.Dock = DockStyle.Fill; control.Margin = new Padding(3, 4, 12, 12); panel.Controls.Add(control, columna, fila + 1);
    }

    private static TextBox CrearCaja() => new() { Font = new Font("Segoe UI", 10F) };
}
