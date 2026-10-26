using PersianVet.Interfaces;
using PersianVet.Modelos;
using PersianVet.Utilidades;

namespace PersianVet.Servicios;

/// <summary>
/// Valida y coordina las consultas y sus tratamientos.
/// </summary>
public class ConsultaServicio
{
    private readonly IConsultaDatos consultaDatos;

    public ConsultaServicio(IConsultaDatos consultaDatos)
    {
        this.consultaDatos = consultaDatos;
    }

    /// <inheritdoc cref="IConsultaDatos.ListarPorMascota(int)"/>
    public List<Consulta> ListarPorMascota(int idMascota)
    {
        return idMascota > 0 ? consultaDatos.ListarPorMascota(idMascota) : [];
    }

    /// <summary>
    /// Valida la consulta (servicio y sin fecha futura) y que cada tratamiento tenga medicamento y dosis, y la registra con sus tratamientos.
    /// </summary>
    /// <param name="consulta">Consulta con la lista de tratamientos.</param>
    /// <returns>El resultado de la operación, con el mensaje para mostrar al usuario.</returns>
    public ResultadoOperacion Guardar(Consulta consulta)
    {
        DateTime ahora = DateTime.Now;
        Validador validador = new Validador()
            .Seleccionado(consulta.IdMascota, "Mascota")
            .Seleccionado(consulta.IdVeterinario, "Veterinario")
            .Seleccionado(consulta.Servicio, "Servicio")
            .Regla(consulta.FechaHora <= ahora.AddMinutes(1), "Fecha y hora",
                $"{consulta.FechaHora:dd/MM/yyyy HH:mm} es posterior a ahora ({ahora:dd/MM/yyyy HH:mm}); " +
                "una consulta se registra cuando ya ocurrió.")
            .Requerido(consulta.Motivo, "Motivo")
            .LongitudMaxima(consulta.Motivo, 200, "Motivo")
            .Requerido(consulta.Diagnostico, "Diagnóstico")
            .LongitudMaxima(consulta.Diagnostico, 500, "Diagnóstico")
            .LongitudMaxima(consulta.Observaciones, 500, "Observaciones");

        for (int i = 0; i < consulta.ListaTratamientos.Count; i++)
        {
            Tratamiento tratamiento = consulta.ListaTratamientos[i];
            string campo = $"Tratamiento {i + 1} ({tratamiento.Medicamento})";
            validador
                .Requerido(tratamiento.Medicamento, $"Tratamiento {i + 1} - Medicamento")
                .Requerido(tratamiento.Dosis, $"{campo} - Dosis")
                .LongitudMaxima(tratamiento.Dosis, 80, $"{campo} - Dosis")
                .LongitudMaxima(tratamiento.Frecuencia, 100, $"{campo} - Frecuencia")
                .LongitudMaxima(tratamiento.Duracion, 80, $"{campo} - Duración")
                .LongitudMaxima(tratamiento.Indicaciones, 300, $"{campo} - Indicaciones");
        }

        if (!validador.EsValido)
        {
            return validador.Resultado();
        }

        consulta.IdConsulta = consultaDatos.GuardarConTratamientos(consulta);
        return ResultadoOperacion.Correcto("La consulta se guardó correctamente.");
    }
}

