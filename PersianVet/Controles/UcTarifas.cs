using PersianVet.Modelos;
using PersianVet.Servicios;
using PersianVet.Utilidades;

namespace PersianVet.Controles;

/// <summary>
/// Permite al administrador mantener el catálogo de productos y sus precios en córdobas,
/// y comprobar en el historial y en los últimos cobros cómo se aplicó cada cambio de precio.
/// </summary>
public partial class UcTarifas : UserControl
{
    private readonly TarifaServicio tarifaServicio;
    private readonly string usuario;
    private readonly (RadioButton Boton, string Texto, string? Tipo)[] categorias;
    private List<Tarifa> tarifas = [];
    private int idSeleccionado;

    public UcTarifas()
    {
        InitializeComponent();
        EstiloControles.ConfigurarBotonPrincipal(btnAgregar);
        EstiloControles.ConfigurarBotonSecundario(btnModificar);
        EstiloControles.ConfigurarBotonSecundario(btnDesactivar);
        EstiloControles.ConfigurarBotonSecundario(btnLimpiar);
        EstiloControles.ConfigurarTabla(tablaTarifas);
        EstiloControles.ConfigurarTabla(tablaHistorial);
        EstiloControles.ConfigurarTabla(tablaCobros);

        categorias =
        [
            (rbTodos, "Todos", null),
            (rbServicios, "Servicios y consultas", Tarifa.Servicio),
            (rbVacunas, "Vacunas", Tarifa.Vacuna),
            (rbMedicamentos, "Medicamentos", Tarifa.Medicamento)
        ];
        foreach ((RadioButton boton, _, _) in categorias)
        {
            boton.FlatAppearance.BorderColor = Tema.Azul;
            boton.FlatAppearance.CheckedBackColor = Tema.Azul;
            boton.FlatAppearance.MouseOverBackColor = Tema.AzulClaro;
            boton.BackColor = Color.White;
            boton.Cursor = Cursors.Hand;
            PintarCategoria(boton);
        }

        tarifaServicio = null!;
        usuario = string.Empty;
    }

    public UcTarifas(TarifaServicio tarifaServicio, string usuario) : this()
    {
        this.tarifaServicio = tarifaServicio;
        this.usuario = usuario;
        Load += (_, _) => CargarTarifas();
    }

    private void BtnAgregar_Click(object? sender, EventArgs e)
    {
        Tarifa tarifa = CrearTarifa(0);
        Ejecutar(() => tarifaServicio.Guardar(tarifa, usuario), tarifa);
    }

    private void BtnModificar_Click(object? sender, EventArgs e)
    {
        Tarifa tarifa = CrearTarifa(idSeleccionado);
        Ejecutar(() => tarifaServicio.Actualizar(tarifa, usuario), tarifa);
    }

    private void BtnLimpiar_Click(object? sender, EventArgs e) => Limpiar();

    private void Filtrar(object? sender, EventArgs e) => CargarTarifas();

    private void Categoria_CheckedChanged(object? sender, EventArgs e)
    {
        if (sender is not RadioButton boton)
        {
            return;
        }

        PintarCategoria(boton);

        // El evento llega también al botón que se desmarca; la lista se refresca solo una vez.
        if (boton.Checked && categorias is not null)
        {
            MostrarTarifas();
        }
    }

    private void BtnDesactivar_Click(object? sender, EventArgs e)
    {
        if (idSeleccionado <= 0)
        {
            MessageBox.Show("Seleccione un producto de la lista.", "PersianVet");
            return;
        }

        if (MessageBox.Show("¿Desea desactivar el producto? Dejará de ofrecerse al registrar, " +
                "pero lo ya registrado y su historial de precios se conservan.", "PersianVet",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }

        try
        {
            tarifaServicio.Desactivar(idSeleccionado);
            CargarTarifas();
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void TablaTarifas_SelectionChanged(object? sender, EventArgs e)
    {
        // Se usa la fila seleccionada y no CurrentRow, que al cambiar de fila todavía apunta a la anterior.
        if (tablaTarifas.SelectedRows.Count == 0 || tablaTarifas.SelectedRows[0].DataBoundItem is not Tarifa tarifa)
        {
            return;
        }

        idSeleccionado = tarifa.IdTarifa;
        txtNombre.Text = tarifa.Nombre;
        cboTipo.Text = tarifa.Tipo;
        numPrecio.Value = Math.Min(numPrecio.Maximum, tarifa.Precio);
        MostrarDetalle(tarifa);
    }

    private Tarifa CrearTarifa(int idTarifa) => new()
    {
        IdTarifa = idTarifa,
        Nombre = txtNombre.Text,
        Tipo = cboTipo.Text,
        Precio = numPrecio.Value
    };

    /// <summary>
    /// Ejecuta la operación y, si sale bien, recarga la lista y vuelve a seleccionar el producto
    /// para que se vea su precio nuevo y el cambio en el historial.
    /// </summary>
    private void Ejecutar(Func<ResultadoOperacion> operacion, Tarifa tarifa)
    {
        Mensajes.EjecutarOperacion(operacion, () =>
        {
            CargarTarifas();
            Seleccionar(tarifa);
        });
    }

    private void CargarTarifas()
    {
        try
        {
            tarifas = tarifaServicio.Listar(soloActivas: !chkMostrarInactivas.Checked);
            foreach ((RadioButton boton, string texto, string? tipo) in categorias)
            {
                boton.Text = $"{texto} ({tarifas.Count(t => tipo == null || t.Tipo == tipo)})";
            }

            MostrarTarifas();
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    /// <summary>
    /// Muestra en la tabla los productos de la categoría elegida, sin volver a consultar la base.
    /// </summary>
    private void MostrarTarifas()
    {
        string? tipo = categorias.First(c => c.Boton.Checked).Tipo;
        tablaTarifas.DataSource = tarifas.Where(t => tipo == null || t.Tipo == tipo).ToList();
        EstiloControles.OcultarColumnas(tablaTarifas, "IdTarifa");
        EstiloControles.FormatoMoneda(tablaTarifas, "Precio");
        tablaTarifas.Columns["VigenteDesde"].DefaultCellStyle.Format = "dd/MM/yyyy";
        tablaTarifas.Columns["Activa"].Visible = chkMostrarInactivas.Checked;
        tablaTarifas.Columns["Nombre"].FillWeight = 180;
        Limpiar();
    }

    private void Seleccionar(Tarifa tarifa)
    {
        // Si el producto quedó fuera de la categoría elegida, se muestran todos para poder seleccionarlo.
        if (categorias.First(c => c.Boton.Checked).Tipo is string tipo && tipo != tarifa.Tipo)
        {
            rbTodos.Checked = true;
        }

        foreach (DataGridViewRow fila in tablaTarifas.Rows)
        {
            if (fila.DataBoundItem is Tarifa actual && actual.IdTarifa == tarifa.IdTarifa)
            {
                tablaTarifas.CurrentCell = fila.Cells["Nombre"];
                fila.Selected = true;
                return;
            }
        }
    }

    private void MostrarDetalle(Tarifa tarifa)
    {
        try
        {
            lblHistorial.Text = $"Historial de precios: {tarifa.Nombre}";
            tablaHistorial.DataSource = tarifaServicio.ListarHistorial(tarifa.IdTarifa);
            EstiloControles.OcultarColumnas(tablaHistorial, "IdHistorial", "IdTarifa");
            EstiloControles.FormatoMoneda(tablaHistorial, "PrecioAnterior", "PrecioNuevo");
            tablaHistorial.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            tablaHistorial.Columns["Fecha"].FillWeight = 130;
            tablaHistorial.Columns["Cambio"].FillWeight = 130;
            tablaHistorial.Columns["Usuario"].FillWeight = 70;
            tablaHistorial.ClearSelection();

            List<CobroTarifa> cobros = tarifaServicio.ListarCobros(tarifa);
            lblCobros.Text = cobros.Count == 0
                ? "Últimos cobros: todavía no se ha cobrado"
                : "Últimos cobros (precio guardado en cada registro)";
            tablaCobros.DataSource = cobros;
            EstiloControles.FormatoMoneda(tablaCobros, "Precio");
            tablaCobros.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            tablaCobros.Columns["Fecha"].FillWeight = 130;
            tablaCobros.ClearSelection();
        }
        catch (Exception excepcion)
        {
            Mensajes.MostrarError(excepcion);
        }
    }

    private void Limpiar()
    {
        idSeleccionado = 0;
        txtNombre.Clear();
        cboTipo.SelectedIndex = 0;
        numPrecio.Value = 0;
        tablaTarifas.ClearSelection();
        lblHistorial.Text = "Historial de precios";
        lblCobros.Text = "Últimos cobros";
        tablaHistorial.DataSource = null;
        tablaCobros.DataSource = null;
    }

    private static void PintarCategoria(RadioButton boton)
    {
        boton.ForeColor = boton.Checked ? Color.White : Tema.Azul;
    }
}
