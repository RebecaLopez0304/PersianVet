# PersianVet - Entregable 2

PersianVet es una aplicación de escritorio para administrar la información básica de una clínica veterinaria. Este segundo entregable incluye una interfaz en Windows Forms y una conexión real con SQL Server mediante ADO.NET.

El proyecto conserva la identidad visual y los módulos planteados en el Entregable 1. Algunas funciones se dejaron pendientes de forma intencional porque pertenecen a un entregable posterior.

## Requisitos

- Windows 10 u 11.
- Visual Studio 2022 con el componente de desarrollo de escritorio con .NET.
- .NET 8 SDK.
- SQL Server LocalDB o una instancia de SQL Server.
- SQL Server Management Studio es opcional.

## Preparar la base de datos

1. Abra `BaseDatos/PersianVetDB.sql` en SQL Server Management Studio.
2. Conéctese a `(localdb)\MSSQLLocalDB` con autenticación de Windows.
3. Ejecute el script completo.
4. Compruebe que se creó la base `PersianVetDB`.

También puede ejecutarse desde PowerShell, ubicado en la carpeta principal:

```powershell
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -b -f 65001 -i ".\BaseDatos\PersianVetDB.sql"
```

La cadena de conexión se encuentra en `PersianVet/configuracion.json`. Si se usa otra instancia, solamente debe cambiarse el valor `CadenaConexion`.

Al conectarse desde SQL Server Management Studio a LocalDB, seleccione `Opcional` en el campo `Cifrar`. Esta instancia local no admite el cifrado obligatorio de SSMS.

## Ejecutar el proyecto

1. Abra `PersianVet.sln` en Visual Studio.
2. Espere que Visual Studio restaure el paquete `Microsoft.Data.SqlClient`.
3. Confirme que `PersianVet` sea el proyecto de inicio.
4. Presione `F5`.

Credenciales de demostración:

- Usuario: `admin`
- Contraseña: `admin`

## Organización

- `Modelos`: clases que representan los datos principales.
- `Interfaces`: indican qué operaciones debe ofrecer cada clase de datos.
- `Datos`: contiene la conexión y los comandos de SQL Server.
- `Servicios`: revisa datos y coordina las operaciones.
- `Formularios`: contiene el acceso y la ventana principal.
- `Controles`: contiene los módulos mostrados dentro de la ventana principal.
- `Utilidades`: reúne validaciones, colores y funciones compartidas.
- `Recursos`: contiene la mascota gráfica de PersianVet.

El recorrido de una operación es sencillo:

```text
Formulario o control -> Servicio -> Clase de datos -> SQL Server
```

## Programación orientada a objetos

Cada entidad se representa con una clase. Por ejemplo, la clase `Cliente` reúne los datos de una persona y la clase `Mascota` reúne los datos del paciente. Las propiedades se usan para mantener organizada la información.

Cada clase tiene una tarea clara. Los formularios muestran información, los servicios validan y las clases de datos trabajan con SQL Server. Esto evita colocar toda la aplicación en los eventos de los botones.

## Principios SOLID utilizados

- Responsabilidad única: cada clase tiene un trabajo principal.
- Abierto y cerrado: los formularios usan servicios e interfaces, por lo que una clase puede cambiar sin reescribir toda la pantalla.
- Sustitución de Liskov: no se creó herencia innecesaria.
- Separación de interfaces: cada módulo tiene una interfaz pequeña.
- Inversión de dependencias: los servicios reciben sus clases de datos por medio del constructor.

No se usaron contenedores ni patrones avanzados. Las dependencias se crean de forma manual para que el código sea fácil de seguir.

## ADO.NET

La aplicación usa estas clases principales:

- `SqlConnection`: abre la comunicación con SQL Server.
- `SqlCommand`: ejecuta instrucciones y procedimientos.
- `SqlDataReader`: lee los resultados de una consulta.
- `SqlTransaction`: asegura que la consulta y sus tratamientos se guarden juntos.

Todas las consultas que reciben información del usuario utilizan parámetros. Esto evita unir directamente los textos escritos con una instrucción SQL.

## Módulos disponibles

- Inicio de sesión conectado con la tabla Usuarios.
- Creación de usuarios desde la pantalla de acceso.
- Cambio sencillo de contraseña para usuarios registrados.
- Dashboard con información real.
- Registro, búsqueda, modificación y desactivación de clientes.
- Registro, búsqueda, modificación y desactivación de mascotas.
- Registro, búsqueda, modificación y desactivación de veterinarios.
- Programación, consulta, reprogramación y cancelación de citas.
- Registro de consultas y tratamientos.
- Consulta del historial clínico por mascota.
- Registro de vacunas y consulta de próximas dosis.

## Funciones pendientes

- Reportes e impresión.
- Permisos diferentes por rol.
- Fotografías personalizadas de mascotas.
- Edición y eliminación de consultas, tratamientos y vacunas.
- Instalador de Windows.

## Recorrido para la demostración

1. Inicie sesión con el usuario de demostración.
2. Revise los datos del dashboard.
3. Registre un cliente y búsquelo por su nombre.
4. Registre una mascota para ese cliente.
5. Programe una cita para la mascota.
6. Registre una consulta y agregue un tratamiento.
7. Revise el historial clínico.
8. Registre una vacuna con una próxima dosis.
9. Muestre una modificación y una cancelación.
10. Cierre la sesión desde el menú lateral.

## Alcance académico

Esta versión demuestra interfaz gráfica, POO, SOLID, validaciones, ADO.NET, consultas parametrizadas, procedimientos almacenados y transacciones. Las funciones pendientes se conservaron para continuar el proyecto en el siguiente entregable.
