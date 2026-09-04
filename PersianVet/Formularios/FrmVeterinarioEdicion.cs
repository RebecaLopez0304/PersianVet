using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Formularios;

/// <summary>
/// Permite escribir los datos de un veterinario en una ventana separada.
/// </summary>
public class FrmVeterinarioEdicion : Form
{
    private readonly TextBox txtNombres = new();
    private readonly TextBox txtApellidos = new();
    private readonly TextBox txtEspecialidad = new();
    private readonly TextBox txtTelefono = new();
    public Veterinario Veterinario { get; }

    public FrmVeterinarioEdicion(Veterinario? veterinario = null)
    {
        Veterinario = veterinario ?? new Veterinario();
        Text = veterinario is null ? "Nuevo veterinario" : "Modificar veterinario";
        TableLayoutPanel panel = new() { Dock = DockStyle.Fill, Padding = new Padding(24), ColumnCount = 2, RowCount = 5 };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        Agregar(panel, "Nombres", txtNombres, 0, 0); Agregar(panel, "Apellidos", txtApellidos, 1, 0);
        Agregar(panel, "Especialidad", txtEspecialidad, 0, 2); Agregar(panel, "Teléfono", txtTelefono, 1, 2);
        Button cancelar = new() { Text = "Cancelar", Dock = DockStyle.Fill, DialogResult = DialogResult.Cancel };
        Button guardar = new() { Text = "Guardar", Dock = DockStyle.Fill };
        EstiloControles.ConfigurarBotonSecundario(cancelar); EstiloControles.ConfigurarBotonPrincipal(guardar);
        guardar.Click += Guardar; panel.Controls.Add(cancelar, 0, 4); panel.Controls.Add(guardar, 1, 4); Controls.Add(panel);
        txtNombres.Text = Veterinario.Nombres; txtApellidos.Text = Veterinario.Apellidos;
        txtEspecialidad.Text = Veterinario.Especialidad; txtTelefono.Text = Veterinario.Telefono;
        AcceptButton = guardar; CancelButton = cancelar; ClientSize = new Size(570, 270); BackColor = Tema.Fondo;
        Font = new Font("Segoe UI", 9F); FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false; StartPosition = FormStartPosition.CenterParent;
    }

    private void Guardar(object? sender, EventArgs e)
    {
        Veterinario.Nombres = txtNombres.Text; Veterinario.Apellidos = txtApellidos.Text;
        Veterinario.Especialidad = txtEspecialidad.Text; Veterinario.Telefono = txtTelefono.Text;
        DialogResult = DialogResult.OK;
    }

    private static void Agregar(TableLayoutPanel panel, string texto, Control control, int columna, int fila)
    {
        panel.Controls.Add(new Label { Text = texto, AutoSize = true, Font = new Font("Segoe UI", 9F, FontStyle.Bold) }, columna, fila);
        control.Dock = DockStyle.Fill; control.Font = new Font("Segoe UI", 10F); control.Margin = new Padding(3, 4, 12, 12); panel.Controls.Add(control, columna, fila + 1);
    }
}
