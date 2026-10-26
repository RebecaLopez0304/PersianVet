using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las operaciones del catálogo de tarifas.
/// </summary>
public class TarifaServicio
{
    private readonly ITarifaDatos tarifaDatos;

    public TarifaServicio(ITarifaDatos tarifaDatos)
    {
        this.tarifaDatos = tarifaDatos;
    }

    /// <inheritdoc cref="ITarifaDatos.Listar(string?, bool)"/>
    public List<Tarifa> Listar(string? tipo = null, bool soloActivas = true)
    {
        return tarifaDatos.Listar(tipo, soloActivas);
    }

    /// <inheritdoc cref="ITarifaDatos.ListarHistorial(int)"/>
    public List<HistorialPrecio> ListarHistorial(int idTarifa)
    {
        return tarifaDatos.ListarHistorial(idTarifa);
    }

    /// <inheritdoc cref="ITarifaDatos.ListarCobros(Tarifa, int)"/>
    public List<CobroTarifa> ListarCobros(Tarifa tarifa, int maximo = 10)
    {
        return tarifaDatos.ListarCobros(tarifa, maximo);
    }

    /// <summary>
    /// Valida el nombre, la categoría y el precio, y registra la tarifa con su precio inicial en el historial.
    /// </summary>
    /// <param name="tarifa">Datos de la tarifa.</param>
    /// <param name="usuario">Usuario que la registra.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Tarifa tarifa, string usuario)
    {
        ResultadoOperacion validacion = Validar(tarifa);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        tarifa.IdTarifa = tarifaDatos.Guardar(tarifa, usuario);
        return ResultadoOperacion.Correcto(
            $"Se agregó \"{tarifa.Nombre}\" ({tarifa.Tipo}) con precio {Moneda.Formatear(tarifa.Precio)}.\n\n" +
            "Ya se puede elegir al registrar citas, consultas, tratamientos o vacunas.");
    }

    /// <summary>
    /// Valida los datos y modifica una tarifa existente. Si el precio cambia, el cambio queda en el historial
    /// y solo se aplica a lo que se registre desde ese momento.
    /// </summary>
    /// <param name="tarifa">Datos de la tarifa, con su identificador.</param>
    /// <param name="usuario">Usuario que hace el cambio.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Actualizar(Tarifa tarifa, string usuario)
    {
        if (tarifa.IdTarifa <= 0)
        {
            return ResultadoOperacion.Error("Seleccione un producto de la lista para modificarlo.");
        }

        ResultadoOperacion validacion = Validar(tarifa);
        if (!validacion.Exitoso)
        {
            return validacion;
        }

        decimal precioAnterior = tarifaDatos.Actualizar(tarifa, usuario);
        if (precioAnterior == tarifa.Precio)
        {
            return ResultadoOperacion.Correcto(
                $"Se actualizó \"{tarifa.Nombre}\". El precio no cambió: sigue en {Moneda.Formatear(tarifa.Precio)}.");
        }

        return ResultadoOperacion.Correcto(
            $"Se cambió el precio de \"{tarifa.Nombre}\": {Moneda.Formatear(precioAnterior)} → " +
            $"{Moneda.Formatear(tarifa.Precio)}, vigente desde {DateTime.Now:dd/MM/yyyy HH:mm}.\n\n" +
            "• Lo que se registre desde ahora usará el precio nuevo.\n" +
            "• Lo registrado antes (citas, consultas, tratamientos y vacunas) conserva el precio con que se cobró.\n\n" +
            "Puede comprobarlo en \"Historial de precios\" y \"Últimos cobros\".");
    }

    /// <inheritdoc cref="ITarifaDatos.Desactivar(int)"/>
    public void Desactivar(int idTarifa)
    {
        tarifaDatos.Desactivar(idTarifa);
    }

    private ResultadoOperacion Validar(Tarifa tarifa)
    {
        Validador validador = new Validador()
            .Requerido(tarifa.Nombre, "Nombre")
            .LongitudMaxima(tarifa.Nombre, 100, "Nombre")
            .Regla(tarifa.Tipo is Tarifa.Servicio or Tarifa.Vacuna or Tarifa.Medicamento, "Tipo",
                "seleccione Servicio, Vacuna o Medicamento.")
            .Regla(tarifa.Precio > 0, "Precio (C$)", $"debe ser mayor que {Moneda.Formatear(0)}.");

        // El nombre repetido se revisa solo si lo demás es válido, para no consultar la base sin necesidad.
        if (validador.EsValido)
        {
            validador.Regla(!tarifaDatos.ExisteNombre(tarifa), "Nombre",
                $"ya existe una tarifa llamada \"{tarifa.Nombre.Trim()}\" (revise también las desactivadas).");
        }

        return validador.Resultado();
    }
}
