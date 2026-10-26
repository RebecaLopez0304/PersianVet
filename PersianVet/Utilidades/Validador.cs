using PersianVet.Modelos;

namespace PersianVet.Utilidades;

/// <summary>
/// Reúne todos los errores de un formulario para mostrarlos juntos, cada uno con su campo.
/// </summary>
/// <remarks>
/// Ejemplo de mensaje: "Teléfono: debe tener al menos 8 dígitos; tiene 5."
/// </remarks>
public class Validador
{
    private readonly List<string> errores = [];

    /// <summary>
    /// Obtiene un valor que indica si no se encontró ningún error.
    /// </summary>
    public bool EsValido => errores.Count == 0;

    /// <summary>
    /// Registra un error si el texto está vacío.
    /// </summary>
    public Validador Requerido(string? valor, string campo)
    {
        return Regla(!string.IsNullOrWhiteSpace(valor), campo, "este campo es obligatorio.");
    }

    /// <summary>
    /// Registra un error si no se eligió una opción de una lista (identificador 0 o texto vacío).
    /// </summary>
    public Validador Seleccionado(int id, string campo)
    {
        return Regla(id > 0, campo, "seleccione una opción de la lista.");
    }

    /// <inheritdoc cref="Seleccionado(int, string)"/>
    public Validador Seleccionado(string? valor, string campo)
    {
        return Regla(!string.IsNullOrWhiteSpace(valor), campo, "seleccione una opción de la lista.");
    }

    /// <summary>
    /// Registra un error si el texto supera el tamaño de su columna en la base de datos.
    /// </summary>
    public Validador LongitudMaxima(string? valor, int maximo, string campo)
    {
        int longitud = (valor ?? string.Empty).Trim().Length;
        return Regla(longitud <= maximo, campo, $"admite como máximo {maximo} caracteres; tiene {longitud}.");
    }

    /// <summary>
    /// Revisa que el teléfono exista, use solo números, espacios, guiones, + o paréntesis,
    /// y tenga al menos 8 dígitos.
    /// </summary>
    public Validador Telefono(string? valor, string campo = "Teléfono")
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return Requerido(valor, campo);
        }

        int digitos = valor.Count(char.IsDigit);
        return Regla(valor.All(c => char.IsDigit(c) || " -+()".Contains(c)), campo,
                $"\"{valor.Trim()}\" tiene caracteres no válidos; use solo números, espacios, guiones, + o paréntesis.")
            .Regla(digitos >= 8, campo, $"debe tener al menos 8 dígitos; tiene {digitos}.")
            .LongitudMaxima(valor, 20, campo);
    }

    /// <summary>
    /// Revisa que el correo exista y tenga un formato válido.
    /// </summary>
    public Validador Correo(string? valor, string campo = "Correo")
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return Requerido(valor, campo);
        }

        string correo = valor.Trim();
        bool formatoValido = Validaciones.EsCorreoValido(correo);
        string dominio = correo.Contains('@') ? correo[(correo.LastIndexOf('@') + 1)..] : string.Empty;

        // MailAddress acepta "ana@correo"; se exige además un dominio con punto, como "correo.com".
        return Regla(formatoValido, campo, $"\"{correo}\" no tiene formato de correo (ejemplo: nombre@correo.com).")
            .Regla(!formatoValido || dominio.Contains('.'), campo,
                $"\"{correo}\" está incompleto: después de @ falta la terminación del dominio (ejemplo: @{dominio}.com).")
            .LongitudMaxima(valor, 120, campo);
    }

    /// <summary>
    /// Registra el error indicado si la condición no se cumple.
    /// </summary>
    /// <param name="esValido">Condición que debe cumplirse.</param>
    /// <param name="campo">Nombre del campo tal como aparece en pantalla.</param>
    /// <param name="detalle">Qué está mal y, si se puede, cómo corregirlo.</param>
    public Validador Regla(bool esValido, string campo, string detalle)
    {
        if (!esValido)
        {
            errores.Add($"{campo}: {detalle}");
        }

        return this;
    }

    /// <summary>
    /// Convierte los errores encontrados en el resultado que se muestra al usuario.
    /// </summary>
    /// <param name="encabezado">Primera línea del mensaje, por ejemplo "No se pudo guardar.".</param>
    /// <returns>Un resultado correcto, o uno con la lista de errores por campo.</returns>
    public ResultadoOperacion Resultado(string encabezado = "No se pudo guardar.")
    {
        return EsValido
            ? ResultadoOperacion.Correcto(string.Empty)
            : ResultadoOperacion.Error($"{encabezado} Revise los siguientes datos:\n\n• " + string.Join("\n• ", errores));
    }
}
