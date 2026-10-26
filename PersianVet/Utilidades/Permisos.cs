namespace PersianVet.Utilidades;

/// <summary>
/// Define a qué módulos puede entrar cada rol de usuario.
/// </summary>
public static class Permisos
{
    private static readonly Dictionary<string, string[]> modulosPorRol = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Administrador"] = ["Inicio", "Clientes", "Mascotas", "Citas", "Consultas", "Vacunas", "Veterinarios", "Reportes"],
        ["Recepcionista"] = ["Inicio", "Clientes", "Mascotas", "Citas", "Vacunas"],
        ["Veterinario"] = ["Inicio", "Mascotas", "Citas", "Consultas", "Vacunas"]
    };

    /// <summary>
    /// Indica si el rol puede abrir el destino.
    /// </summary>
    /// <param name="rol">Rol del usuario que inició sesión.</param>
    /// <param name="destino">Módulo (por ejemplo "Clientes") o acción rápida (por ejemplo "NuevoCliente").</param>
    /// <returns><see langword="true"/> si el rol tiene acceso; un rol desconocido solo accede al inicio.</returns>
    public static bool PuedeAcceder(string rol, string destino)
    {
        // Un rol desconocido solo puede ver el inicio.
        string[] modulos = modulosPorRol.TryGetValue(rol, out string[]? lista) ? lista : ["Inicio"];
        return modulos.Contains(ModuloDe(destino));
    }

    /// <summary>
    /// Las acciones rápidas se rigen por el permiso del módulo al que pertenecen.
    /// </summary>
    private static string ModuloDe(string destino) => destino switch
    {
        "NuevoCliente" => "Clientes",
        "NuevaMascota" => "Mascotas",
        "Calendario" => "Citas",
        _ => destino
    };
}
