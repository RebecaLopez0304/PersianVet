using Microsoft.EntityFrameworkCore;
using PersianVet.Interfaces;
using PersianVet.Modelos;

namespace PersianVet.Datos;

/// <summary>
/// Realiza las operaciones del catálogo de tarifas y de su historial de precios mediante Entity Framework Core.
/// </summary>
public class TarifaDatos : ITarifaDatos
{
    private readonly ConexionBD conexionBD;

    public TarifaDatos(ConexionBD conexionBD)
    {
        this.conexionBD = conexionBD;
    }

    /// <inheritdoc/>
    public List<Tarifa> Listar(string? tipo = null, bool soloActivas = true)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();

        // Include carga el historial que usa la propiedad VigenteDesde.
        return contexto.Tarifas.AsNoTracking()
            .Include(t => t.Historial)
            .Where(t => (tipo == null || t.Tipo == tipo) && (!soloActivas || t.Activa))
            .OrderBy(t => t.Tipo)
            .ThenBy(t => t.Nombre)
            .ToList();
    }

    /// <inheritdoc/>
    public bool ExisteNombre(Tarifa tarifa)
    {
        string nombre = tarifa.Nombre.Trim();
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.Tarifas.Any(t => t.Nombre == nombre && t.IdTarifa != tarifa.IdTarifa);
    }

    /// <inheritdoc/>
    public int Guardar(Tarifa tarifa, string usuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        tarifa.Nombre = tarifa.Nombre.Trim();
        tarifa.Activa = true;
        tarifa.Historial = [new HistorialPrecio { PrecioNuevo = tarifa.Precio, Usuario = usuario }];
        contexto.Tarifas.Add(tarifa);
        contexto.SaveChanges();
        return tarifa.IdTarifa;
    }

    /// <inheritdoc/>
    public decimal Actualizar(Tarifa tarifa, string usuario)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        Tarifa guardada = contexto.Tarifas.Single(t => t.IdTarifa == tarifa.IdTarifa);
        decimal precioAnterior = guardada.Precio;

        guardada.Nombre = tarifa.Nombre.Trim();
        guardada.Tipo = tarifa.Tipo;
        guardada.Precio = tarifa.Precio;
        if (precioAnterior != tarifa.Precio)
        {
            contexto.HistorialPrecios.Add(new HistorialPrecio
            {
                IdTarifa = tarifa.IdTarifa,
                PrecioAnterior = precioAnterior,
                PrecioNuevo = tarifa.Precio,
                Usuario = usuario
            });
        }

        // SaveChanges guarda el precio nuevo y su registro de historial en una sola transacción.
        contexto.SaveChanges();
        return precioAnterior;
    }

    /// <inheritdoc/>
    public void Desactivar(int idTarifa)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        contexto.Tarifas.Where(t => t.IdTarifa == idTarifa)
            .ExecuteUpdate(t => t.SetProperty(x => x.Activa, false));
    }

    /// <inheritdoc/>
    public List<HistorialPrecio> ListarHistorial(int idTarifa)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        return contexto.HistorialPrecios.AsNoTracking()
            .Where(h => h.IdTarifa == idTarifa)
            .OrderByDescending(h => h.Fecha)
            .ThenByDescending(h => h.IdHistorial)
            .ToList();
    }

    /// <inheritdoc/>
    public List<CobroTarifa> ListarCobros(Tarifa tarifa, int maximo)
    {
        using PersianVetContext contexto = conexionBD.CrearContexto();
        List<CobroTarifa> cobros = [];

        // Los registros guardan el nombre de la tarifa, no su Id: se busca por nombre según la categoría.
        if (tarifa.Tipo == Tarifa.Servicio)
        {
            cobros.AddRange(contexto.Citas.AsNoTracking()
                .Where(c => c.Servicio == tarifa.Nombre && c.Estado != "Cancelada")
                .OrderByDescending(c => c.FechaHora).Take(maximo)
                .Select(c => new CobroTarifa { Fecha = c.FechaHora, Registro = "Cita", Mascota = c.Mascota.Nombre, Precio = c.Precio }));
            cobros.AddRange(contexto.Consultas.AsNoTracking()
                .Where(c => c.Servicio == tarifa.Nombre)
                .OrderByDescending(c => c.FechaHora).Take(maximo)
                .Select(c => new CobroTarifa { Fecha = c.FechaHora, Registro = "Consulta", Mascota = c.Mascota.Nombre, Precio = c.Precio }));
        }
        else if (tarifa.Tipo == Tarifa.Vacuna)
        {
            cobros.AddRange(contexto.Vacunas.AsNoTracking()
                .Where(v => v.NombreVacuna == tarifa.Nombre)
                .OrderByDescending(v => v.FechaAplicacion).Take(maximo)
                .Select(v => new CobroTarifa { Fecha = v.FechaAplicacion, Registro = "Vacuna", Mascota = v.Mascota.Nombre, Precio = v.Precio }));
        }
        else
        {
            cobros.AddRange(contexto.Consultas.AsNoTracking()
                .SelectMany(c => c.ListaTratamientos, (c, t) => new { Consulta = c, Tratamiento = t })
                .Where(x => x.Tratamiento.Medicamento == tarifa.Nombre)
                .OrderByDescending(x => x.Consulta.FechaHora).Take(maximo)
                .Select(x => new CobroTarifa
                {
                    Fecha = x.Consulta.FechaHora,
                    Registro = "Tratamiento",
                    Mascota = x.Consulta.Mascota.Nombre,
                    Precio = x.Tratamiento.Precio
                }));
        }

        return cobros.OrderByDescending(c => c.Fecha).Take(maximo).ToList();
    }
}
