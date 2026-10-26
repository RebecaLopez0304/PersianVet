namespace PersianVet.Utilidades;

/// <summary>
/// Aplica una apariencia uniforme a los controles principales.
/// </summary>
public static class EstiloControles
{
    /// <summary>
    /// Aplica el estilo de la acción principal de una pantalla: fondo naranja y texto blanco.
    /// </summary>
    /// <param name="boton">Botón a configurar.</param>
    public static void ConfigurarBotonPrincipal(Button boton)
    {
        boton.BackColor = Tema.Naranja;
        boton.ForeColor = Color.White;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderSize = 0;
        boton.Cursor = Cursors.Hand;
        boton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        boton.FlatAppearance.MouseOverBackColor = Color.FromArgb(226, 116, 18);
        boton.FlatAppearance.MouseDownBackColor = Color.FromArgb(194, 92, 7);
    }

    /// <summary>
    /// Aplica el estilo de las acciones secundarias: fondo blanco con borde azul.
    /// </summary>
    /// <param name="boton">Botón a configurar.</param>
    public static void ConfigurarBotonSecundario(Button boton)
    {
        boton.BackColor = Color.White;
        boton.ForeColor = Tema.Azul;
        boton.FlatStyle = FlatStyle.Flat;
        boton.FlatAppearance.BorderColor = Tema.Azul;
        boton.Cursor = Cursors.Hand;
        boton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        boton.FlatAppearance.BorderSize = 1;
        boton.FlatAppearance.MouseOverBackColor = Tema.AzulClaro;
        boton.FlatAppearance.MouseDownBackColor = Color.FromArgb(215, 230, 241);
    }

    /// <summary>
    /// Aplica el estilo común de las tablas: solo lectura, selección de fila completa y encabezado azul.
    /// </summary>
    /// <param name="tabla">Tabla a configurar.</param>
    public static void ConfigurarTabla(DataGridView tabla)
    {
        tabla.AutoGenerateColumns = true;
        tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        tabla.BackgroundColor = Color.White;
        tabla.BorderStyle = BorderStyle.None;
        tabla.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        tabla.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        tabla.GridColor = Tema.Borde;
        tabla.ReadOnly = true;
        tabla.AllowUserToAddRows = false;
        tabla.AllowUserToDeleteRows = false;
        tabla.MultiSelect = false;
        tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        tabla.RowHeadersVisible = false;
        tabla.EnableHeadersVisualStyles = false;
        tabla.ColumnHeadersHeight = 42;
        tabla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        tabla.RowTemplate.Height = 38;
        tabla.ColumnHeadersDefaultCellStyle.BackColor = Tema.Azul;
        tabla.ColumnHeadersDefaultCellStyle.SelectionBackColor = Tema.Azul;
        tabla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        tabla.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        tabla.DefaultCellStyle.BackColor = Color.White;
        tabla.DefaultCellStyle.ForeColor = Tema.GrisOscuro;
        tabla.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        tabla.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 252);
        tabla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 226, 190);
        tabla.DefaultCellStyle.SelectionForeColor = Tema.GrisOscuro;
    }

    /// <summary>
    /// Oculta las columnas indicadas (por ejemplo los Id) si la tabla las tiene.
    /// </summary>
    /// <param name="tabla">Tabla con los datos ya asignados.</param>
    /// <param name="columnas">Nombres de las propiedades cuyas columnas se ocultan.</param>
    public static void OcultarColumnas(DataGridView tabla, params string[] columnas)
    {
        foreach (string nombre in columnas)
        {
            if (tabla.Columns.Contains(nombre))
            {
                tabla.Columns[nombre].Visible = false;
            }
        }
    }

    /// <summary>
    /// Aplica el estilo general a los campos de un formulario o módulo.
    /// </summary>
    /// <param name="contenedor">Control que contiene los campos que se desean preparar.</param>
    public static void AplicarEstiloGeneral(Control contenedor)
    {
        contenedor.BackColor = Tema.Fondo;

        foreach (Control control in contenedor.Controls)
        {
            if (control is TextBox caja)
            {
                caja.BorderStyle = BorderStyle.FixedSingle;
                caja.BackColor = Color.White;
                caja.ForeColor = Tema.GrisOscuro;
            }
            else if (control is ComboBox lista)
            {
                lista.FlatStyle = FlatStyle.Flat;
                lista.BackColor = Color.White;
                lista.ForeColor = Tema.GrisOscuro;
            }
            else if (control is NumericUpDown numero)
            {
                numero.BorderStyle = BorderStyle.FixedSingle;
                numero.BackColor = Color.White;
            }
            else if (control is DateTimePicker fecha)
            {
                fecha.CalendarForeColor = Tema.GrisOscuro;
                fecha.CalendarMonthBackground = Color.White;
            }
            else if (control is CheckBox opcion)
            {
                opcion.ForeColor = Tema.GrisOscuro;
            }

            if (control.HasChildren && control is not DataGridView)
            {
                AplicarEstiloGeneralEnHijos(control);
            }
        }
    }

    private static void AplicarEstiloGeneralEnHijos(Control contenedor)
    {
        foreach (Control control in contenedor.Controls)
        {
            if (control is TextBox caja)
            {
                caja.BorderStyle = BorderStyle.FixedSingle;
                caja.BackColor = Color.White;
            }
            else if (control is ComboBox lista)
            {
                lista.FlatStyle = FlatStyle.Flat;
                lista.BackColor = Color.White;
            }

            if (control.HasChildren && control is not DataGridView)
            {
                AplicarEstiloGeneralEnHijos(control);
            }
        }
    }
}
