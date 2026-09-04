# Guía breve para explicar PersianVet

## Qué problema resuelve

PersianVet reúne en un solo sistema los datos de propietarios, mascotas, veterinarios, citas, consultas, tratamientos y vacunas. Sustituye registros manuales y facilita la consulta del historial de cada mascota.

## Cómo está organizado

La interfaz recibe los datos. Un servicio los revisa. Después, una clase de datos ejecuta una consulta parametrizada en SQL Server. Este recorrido mantiene separadas las tareas y ayuda a localizar los errores.

## Ejemplo con clientes

1. El usuario escribe los datos en `UcClientes`.
2. El control crea un objeto `Cliente`.
3. `ClienteServicio` revisa nombres, teléfono, correo y dirección.
4. `ClienteDatos` ejecuta el `INSERT` con parámetros.
5. El control muestra el resultado y actualiza el `DataGridView`.

## Por qué se usan interfaces

`IClienteDatos` indica las operaciones que debe tener una clase de datos de clientes. `ClienteServicio` depende de esa interfaz. Esto permite separar la validación del código SQL y demuestra los principios de separación e inversión de dependencias.

## Por qué no se borran datos clínicos

Los clientes, mascotas y veterinarios se desactivan. Las citas se cancelan. Así se conserva la información que ya está relacionada con consultas, tratamientos o vacunas.

## Qué hace la transacción

Una consulta puede contener varios tratamientos. La transacción hace que todo se guarde como una sola operación. Si falla un tratamiento, también se cancela el guardado de la consulta y no quedan datos incompletos.

## Qué deben poder demostrar los integrantes

- La navegación entre módulos.
- Un registro y una modificación.
- Una búsqueda o filtro.
- La relación entre cliente y mascota.
- La relación entre cita, mascota y veterinario.
- El historial clínico de una mascota.
- Una consulta parametrizada.
- El procedimiento del dashboard.
- La transacción de consulta y tratamientos.
- Las funciones que se dejaron para el siguiente entregable.
