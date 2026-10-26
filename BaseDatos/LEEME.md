# PersianVet - Entregable 3

## Reconstruir la base de datos (Database First)
1. Ejecutar `BaseDatos/PersianVetDB.sql` en SSMS o con `sqlcmd -i PersianVetDB.sql` (crea la base, tablas y datos de demostración).
2. (Opcional) Ejecutar `BaseDatos/DatosPrueba.sql` para cargar datos de prueba: clientes, mascotas, citas pasadas, de hoy y futuras, consultas y vacunas. Las fechas son relativas al día en que se ejecuta y el script no duplica datos si se corre dos veces.
3. Ajustar `CadenaConexion` en `PersianVet/configuracion.json` si no se usa localhost.

Si la aplicación no encuentra la base de datos, la crea sola ejecutando ambos scripts.

Usuarios (clave `persian123`): `admin` (Administrador), `recepcion` (Recepcionista) y `veterinario` (Veterinario).

## Entity Framework Core
- Paquete: `Microsoft.EntityFrameworkCore.SqlServer` 8.0.11.
- `Datos/PersianVetContext.cs`: DbContext con la configuración Fluent API. Las clases de `Modelos/` son también las entidades (una clase por tabla); las relaciones se cargan con `Include`.
- Las clases `*Datos` conservan sus interfaces; ahora usan LINQ sobre un contexto de vida corta (`ConexionBD.CrearContexto()`).
- `Datos/ReporteDatos.cs`: consultas LINQ de agregación para los reportes.

## Precios en córdobas
- Cada cita, consulta, tratamiento y vacuna guarda el precio del momento en que se registró; cambiar un precio no altera lo ya cobrado.
- La tabla `HistorialPrecios` guarda cada cambio (fecha, precio anterior, nuevo y usuario). `TarifaDatos.Actualizar` actualiza el precio y agrega el historial en un solo `SaveChanges`.
- La pantalla **Productos** (`UcTarifas`) filtra por categoría y muestra el historial y los últimos cobros del producto seleccionado.

## Concurrencia
- Dashboard (`UcDashboard`): resumen y próximas citas se consultan en paralelo con `async/await` y `Task.WhenAll`; la carga se cancela al cambiar de módulo (`CancellationToken`).
- Reportes (`UcReportes`): generación en segundo plano con barra de progreso, botón **Cancelar** y manejo de excepciones. Los reportes solo se visualizan en pantalla, con los ingresos en córdobas.
