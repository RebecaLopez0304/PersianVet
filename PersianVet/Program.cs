using PersianVet.Datos;
using PersianVet.Formularios;
using PersianVet.Servicios;

namespace PersianVet;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        ConexionBD conexion = new();
        UsuarioServicio usuarioServicio = new(new UsuarioDatos(conexion));

        Application.Run(new FrmLogin(usuarioServicio, conexion));
    }
}

