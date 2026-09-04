using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de una mascota en una ventana separada.
/// </summary>
public class FrmMascotaEdicion : Form
{
    private readonly ComboBox cboPropietario = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox txtNombre = new();
    private readonly ComboBox cboEspecie = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly TextBox txtRaza = new();
    private readonly ComboBox cboSexo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    private readonly DateTimePicker dtpNacimiento = new() { Format = DateTimePickerFormat.Short, MaxDate = DateTime.Today };
    private readonly TextBox txtColor = new();
    private readonly NumericUpDown numPeso = new() { DecimalPlaces = 2, Minimum = 0.01M, Maximum = 200, Value = 1 };
    private readonly TextBox txtObservaciones = new();
    public Mascota Mascota { get; }

    public FrmMascotaEdicion(List<Cliente> clientes, Mascota? mascota = null)
    {
        Mascota = mascota ?? new Mascota();
        Text = mascota is null ? "Nueva mascota" : "Modificar mascota";
        cboPropietario.DataSource = clientes; cboPropietario.DisplayMember = "NombreCompleto"; cboPropietario.ValueMember = "IdCliente";
        cboEspecie.Items.AddRange(["Felino", "Canino"]); cboSexo.Items.AddRange(["Macho", "Hembra"]);
        PrepararVentana(); CargarDatos();
    }

    private void PrepararVentana()
    {
        TableLayoutPanel panel = new() { Dock = DockStyle.Fill, Padding = new Padding(24), ColumnCount = 3, RowCount = 7 };
        for (int i = 0; i < 3; i++) panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        Agregar(panel, "Propietario", cboPropietario, 0, 0); Agregar(panel, "Nombre", txtNombre, 1, 0); Agregar(panel, "Especie", cboEspecie, 2, 0);
        Agregar(panel, "Raza", txtRaza, 0, 2); Agregar(panel, "Sexo", cboSexo, 1, 2); Agregar(panel, "Fecha de nacimiento", dtpNacimiento, 2, 2);
        Agregar(panel, "Color", txtColor, 0, 4); Agregar(panel, "Peso (kg)", numPeso, 1, 4); Agregar(panel, "Observaciones", txtObservaciones, 2, 4);
        Button cancelar = new() { Text = "Cancelar", Dock = DockStyle.Fill, DialogResult = DialogResult.Cancel };
        Button guardar = new() { Text = "Guardar", Dock = DockStyle.Fill };
        EstiloControles.ConfigurarBotonSecundario(cancelar); EstiloControles.ConfigurarBotonPrincipal(guardar); guardar.Click += Guardar;
        panel.Controls.Add(cancelar, 1, 6); panel.Controls.Add(guardar, 2, 6); Controls.Add(panel);
        AcceptButton = guardar; CancelButton = cancelar; ClientSize = new Size(760, 380); BackColor = Tema.Fondo;
        Font = new Font("Segoe UI", 9F); FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; StartPosition = FormStartPosition.CenterParent;
    }

    private void CargarDatos()
    {
        cboPropietario.SelectedValue = Mascota.IdCliente; txtNombre.Text = Mascota.Nombre;
        cboEspecie.Text = string.IsNullOrWhiteSpace(Mascota.Especie) ? "Felino" : Mascota.Especie;
        txtRaza.Text = Mascota.Raza; cboSexo.Text = Mascota.Sexo;
        dtpNacimiento.Value = Mascota.FechaNacimiento > DateTime.Today ? DateTime.Today : Mascota.FechaNacimiento;
        txtColor.Text = Mascota.Color; numPeso.Value = Mascota.Peso > 0 ? Math.Min(numPeso.Maximum, Mascota.Peso) : 1;
        txtObservaciones.Text = Mascota.Observaciones;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Mascota.IdCliente = cboPropietario.SelectedValue is int id ? id : 0; Mascota.Nombre = txtNombre.Text;
        Mascota.Especie = cboEspecie.Text; Mascota.Raza = txtRaza.Text; Mascota.Sexo = cboSexo.Text;
        Mascota.FechaNacimiento = dtpNacimiento.Value.Date; Mascota.Color = txtColor.Text;
        Mascota.Peso = numPeso.Value; Mascota.Observaciones = txtObservaciones.Text; DialogResult = DialogResult.OK;
    }

    private static void Agregar(TableLayoutPanel panel, string texto, Control control, int columna, int fila)
    {
        panel.Controls.Add(new Label { Text = texto, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }, columna, fila);
        control.Dock = DockStyle.Fill; control.Margin = new Padding(3, 4, 12, 12); panel.Controls.Add(control, columna, fila + 1);
    }
}
