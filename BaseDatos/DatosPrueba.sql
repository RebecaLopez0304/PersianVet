/*
    PersianVet - Datos de prueba
    Agrega clientes, veterinarios, mascotas, citas, consultas y vacunas con fechas
    pasadas, de hoy y futuras. Las fechas son relativas al día en que se ejecuta,
    así el dashboard, los avisos y los reportes siempre tienen información.
    Se ejecuta después de PersianVetDB.sql y solo una vez (revisa si ya existen).
*/

USE PersianVetDB;
GO

IF EXISTS (SELECT 1 FROM dbo.Clientes WHERE Correo = N'valeria.mendoza@correo.com')
BEGIN
    PRINT N'Los datos de prueba ya existen.';
    SET NOEXEC ON;
END;
GO

DECLARE @Hoy DATETIME2(0) = CAST(CAST(GETDATE() AS DATE) AS DATETIME2(0));

-- Veterinarios
INSERT INTO dbo.Veterinarios (Nombres, Apellidos, Especialidad, Telefono, Activo)
VALUES
    (N'Sofía', N'Herrera', N'Cirugía', N'8777-3300', 1),
    (N'Diego', N'Castillo', N'Dermatología', N'8777-4400', 1);

-- Clientes registrados en distintos meses
INSERT INTO dbo.Clientes (Nombres, Apellidos, Telefono, Direccion, Correo, FechaRegistro, Activo)
VALUES
    (N'Valeria', N'Mendoza Ruiz', N'8811-1001', N'Reparto San Juan, casa 5', N'valeria.mendoza@correo.com', DATEADD(MONTH, -8, @Hoy), 1),
    (N'Jorge Luis', N'Pérez Salinas', N'8811-1002', N'Barrio El Carmen, 3 c. al sur', N'jorge.perez@correo.com', DATEADD(MONTH, -6, @Hoy), 1),
    (N'Daniela', N'Rocha Vega', N'8811-1003', N'Residencial Bolonia, casa 21', N'daniela.rocha@correo.com', DATEADD(MONTH, -4, @Hoy), 1),
    (N'Ricardo', N'Sánchez Mora', N'8811-1004', N'Colonia Centroamérica, F-12', N'ricardo.sanchez@correo.com', DATEADD(MONTH, -2, @Hoy), 1),
    (N'Gabriela', N'Torres Díaz', N'8811-1005', N'Las Colinas, calle 3', N'gabriela.torres@correo.com', DATEADD(DAY, -10, @Hoy), 1),
    (N'Héctor', N'Morales Paz', N'8811-1006', N'Villa Fontana, casa 8', N'hector.morales@correo.com', DATEADD(MONTH, -12, @Hoy), 0);

DECLARE @Valeria INT = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = N'valeria.mendoza@correo.com');
DECLARE @Jorge INT = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = N'jorge.perez@correo.com');
DECLARE @Daniela INT = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = N'daniela.rocha@correo.com');
DECLARE @Ricardo INT = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = N'ricardo.sanchez@correo.com');
DECLARE @Gabriela INT = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = N'gabriela.torres@correo.com');
DECLARE @Hector INT = (SELECT IdCliente FROM dbo.Clientes WHERE Correo = N'hector.morales@correo.com');

-- Mascotas (una desactivada para probar el filtro "Mostrar desactivadas")
INSERT INTO dbo.Mascotas (IdCliente, Nombre, Especie, Raza, Sexo, FechaNacimiento, Color, Peso, Observaciones, Activa)
VALUES
    (@Valeria, N'Luna', N'Felino', N'Siamés', N'Hembra', DATEADD(YEAR, -3, @Hoy), N'Crema', 3.80, N'Alérgica al pollo.', 1),
    (@Valeria, N'Max', N'Canino', N'Labrador', N'Macho', DATEADD(YEAR, -5, @Hoy), N'Negro', 30.20, NULL, 1),
    (@Jorge, N'Toby', N'Canino', N'Beagle', N'Macho', DATEADD(YEAR, -2, @Hoy), N'Tricolor', 12.40, N'Muy nervioso en consulta.', 1),
    (@Jorge, N'Nala', N'Felino', N'Persa', N'Hembra', DATEADD(MONTH, -8, @Hoy), N'Blanco', 2.90, NULL, 1),
    (@Daniela, N'Coco', N'Canino', N'Poodle', N'Hembra', DATEADD(YEAR, -7, @Hoy), N'Café', 6.10, N'Control dental cada 6 meses.', 1),
    (@Ricardo, N'Simba', N'Felino', N'Mestizo', N'Macho', DATEADD(YEAR, -1, @Hoy), N'Atigrado', 4.50, NULL, 1),
    (@Ricardo, N'Bruno', N'Canino', N'Pastor Alemán', N'Macho', DATEADD(YEAR, -4, @Hoy), N'Negro y café', 34.00, N'Displasia leve de cadera.', 1),
    (@Gabriela, N'Mía', N'Felino', N'Maine Coon', N'Hembra', DATEADD(MONTH, -5, @Hoy), N'Gris', 3.20, NULL, 1),
    (@Gabriela, N'Rocky', N'Canino', N'Bulldog Francés', N'Macho', DATEADD(YEAR, -3, @Hoy), N'Atigrado', 11.80, N'Braquicéfalo: evitar calor.', 1),
    (@Hector, N'Pelusa', N'Felino', N'Angora', N'Hembra', DATEADD(YEAR, -9, @Hoy), N'Blanco', 3.60, NULL, 0);

DECLARE @Luna INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Luna' AND IdCliente = @Valeria);
DECLARE @Max INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Max' AND IdCliente = @Valeria);
DECLARE @Toby INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Toby' AND IdCliente = @Jorge);
DECLARE @Nala INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Nala' AND IdCliente = @Jorge);
DECLARE @Coco INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Coco' AND IdCliente = @Daniela);
DECLARE @Simba INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Simba' AND IdCliente = @Ricardo);
DECLARE @Bruno INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Bruno' AND IdCliente = @Ricardo);
DECLARE @Mia INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Mía' AND IdCliente = @Gabriela);
DECLARE @Rocky INT = (SELECT IdMascota FROM dbo.Mascotas WHERE Nombre = N'Rocky' AND IdCliente = @Gabriela);

DECLARE @Ana INT = (SELECT TOP 1 IdVeterinario FROM dbo.Veterinarios WHERE Nombres = N'Ana' AND Apellidos = N'López');
DECLARE @Carlos INT = (SELECT TOP 1 IdVeterinario FROM dbo.Veterinarios WHERE Nombres = N'Carlos' AND Apellidos = N'Ruiz');
DECLARE @Sofia INT = (SELECT TOP 1 IdVeterinario FROM dbo.Veterinarios WHERE Nombres = N'Sofía' AND Apellidos = N'Herrera');
DECLARE @Diego INT = (SELECT TOP 1 IdVeterinario FROM dbo.Veterinarios WHERE Nombres = N'Diego' AND Apellidos = N'Castillo');

-- Citas pasadas (completadas y canceladas)
INSERT INTO dbo.Citas (IdMascota, IdVeterinario, FechaHora, Motivo, Estado)
VALUES
    (@Luna, @Ana, DATEADD(HOUR, 9, DATEADD(DAY, -60, @Hoy)), N'Consulta general', N'Completada'),
    (@Max, @Carlos, DATEADD(HOUR, 10, DATEADD(DAY, -45, @Hoy)), N'Vacuna antirrábica', N'Completada'),
    (@Toby, @Diego, DATEADD(HOUR, 11, DATEADD(DAY, -30, @Hoy)), N'Picazón en la piel', N'Completada'),
    (@Coco, @Sofia, DATEADD(HOUR, 8, DATEADD(DAY, -20, @Hoy)), N'Limpieza dental', N'Completada'),
    (@Bruno, @Ana, DATEADD(HOUR, 15, DATEADD(DAY, -12, @Hoy)), N'Dolor al caminar', N'Completada'),
    (@Nala, @Carlos, DATEADD(HOUR, 14, DATEADD(DAY, -9, @Hoy)), N'Primera vacuna', N'Completada'),
    (@Simba, @Ana, DATEADD(HOUR, 16, DATEADD(DAY, -5, @Hoy)), N'Revisión general', N'Cancelada'),
    (@Rocky, @Diego, DATEADD(HOUR, 10, DATEADD(DAY, -2, @Hoy)), N'Dificultad para respirar', N'Completada');

-- Citas de hoy
INSERT INTO dbo.Citas (IdMascota, IdVeterinario, FechaHora, Motivo, Estado)
VALUES
    (@Mia, @Carlos, DATEADD(HOUR, 8, @Hoy), N'Vacuna triple felina', N'Completada'),
    (@Max, @Ana, DATEADD(HOUR, 13, @Hoy), N'Control de peso', N'Confirmada'),
    (@Toby, @Diego, DATEADD(HOUR, 17, @Hoy), N'Revisión de piel', N'Programada'),
    (@Coco, @Sofia, DATEADD(HOUR, 18, @Hoy), N'Control dental', N'Programada');

-- Citas futuras (las de mañana sin confirmar aparecen en "Requiere atención")
INSERT INTO dbo.Citas (IdMascota, IdVeterinario, FechaHora, Motivo, Estado)
VALUES
    (@Luna, @Ana, DATEADD(HOUR, 9, DATEADD(DAY, 1, @Hoy)), N'Control de alergia', N'Programada'),
    (@Bruno, @Sofia, DATEADD(HOUR, 11, DATEADD(DAY, 1, @Hoy)), N'Radiografía de cadera', N'Confirmada'),
    (@Simba, @Carlos, DATEADD(HOUR, 10, DATEADD(DAY, 2, @Hoy)), N'Castración', N'Programada'),
    (@Nala, @Carlos, DATEADD(HOUR, 14, DATEADD(DAY, 3, @Hoy)), N'Segunda vacuna', N'Confirmada'),
    (@Rocky, @Diego, DATEADD(HOUR, 9, DATEADD(DAY, 4, @Hoy)), N'Seguimiento respiratorio', N'Programada'),
    (@Mia, @Ana, DATEADD(HOUR, 16, DATEADD(DAY, 6, @Hoy)), N'Desparasitación', N'Programada'),
    (@Coco, @Sofia, DATEADD(HOUR, 10, DATEADD(DAY, 10, @Hoy)), N'Extracción dental', N'Programada'),
    (@Max, @Carlos, DATEADD(HOUR, 15, DATEADD(DAY, 21, @Hoy)), N'Refuerzo de vacuna', N'Programada');

-- Consultas pasadas y de este mes, con sus tratamientos
DECLARE @Consulta INT;

INSERT INTO dbo.Consultas (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
VALUES (@Luna, @Ana, DATEADD(HOUR, 9, DATEADD(DAY, -60, @Hoy)), N'Consulta general', N'Dermatitis alérgica por alimento', N'Cambiar a dieta hipoalergénica.');
SET @Consulta = SCOPE_IDENTITY();
INSERT INTO dbo.Tratamientos (IdConsulta, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones)
VALUES (@Consulta, N'Cetirizina', N'5 mg', N'Cada 24 horas', N'10 días', N'Dar con comida.');

INSERT INTO dbo.Consultas (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
VALUES (@Toby, @Diego, DATEADD(HOUR, 11, DATEADD(DAY, -30, @Hoy)), N'Picazón en la piel', N'Infección por hongos', NULL);
SET @Consulta = SCOPE_IDENTITY();
INSERT INTO dbo.Tratamientos (IdConsulta, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones)
VALUES
    (@Consulta, N'Ketoconazol', N'100 mg', N'Cada 12 horas', N'14 días', NULL),
    (@Consulta, N'Champú medicado', N'1 aplicación', N'Cada 3 días', N'3 semanas', N'Dejar actuar 10 minutos.');

INSERT INTO dbo.Consultas (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
VALUES (@Coco, @Sofia, DATEADD(HOUR, 8, DATEADD(DAY, -20, @Hoy)), N'Limpieza dental', N'Sarro moderado', N'Programar extracción de una pieza.');
SET @Consulta = SCOPE_IDENTITY();
INSERT INTO dbo.Tratamientos (IdConsulta, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones)
VALUES (@Consulta, N'Clindamicina', N'75 mg', N'Cada 12 horas', N'7 días', NULL);

INSERT INTO dbo.Consultas (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
VALUES (@Bruno, @Ana, DATEADD(HOUR, 15, DATEADD(DAY, -12, @Hoy)), N'Dolor al caminar', N'Displasia de cadera leve', N'Evitar ejercicio intenso.');
SET @Consulta = SCOPE_IDENTITY();
INSERT INTO dbo.Tratamientos (IdConsulta, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones)
VALUES
    (@Consulta, N'Meloxicam', N'1.5 mg', N'Cada 24 horas', N'7 días', N'Suspender si vomita.'),
    (@Consulta, N'Glucosamina', N'500 mg', N'Cada 24 horas', N'3 meses', NULL);

INSERT INTO dbo.Consultas (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
VALUES (@Rocky, @Diego, DATEADD(HOUR, 10, DATEADD(DAY, -2, @Hoy)), N'Dificultad para respirar', N'Síndrome braquicefálico', NULL);
SET @Consulta = SCOPE_IDENTITY();
INSERT INTO dbo.Tratamientos (IdConsulta, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones)
VALUES (@Consulta, N'Prednisolona', N'5 mg', N'Cada 24 horas', N'5 días', N'Mantener en lugar fresco.');

INSERT INTO dbo.Consultas (IdMascota, IdVeterinario, FechaHora, Motivo, Diagnostico, Observaciones)
VALUES (@Mia, @Carlos, DATEADD(HOUR, 8, @Hoy), N'Vacuna triple felina', N'Paciente sano', NULL);

-- Vacunas: vencidas, por vencer esta semana, este mes y más adelante
INSERT INTO dbo.Vacunas (IdMascota, NombreVacuna, FechaAplicacion, ProximaDosis, Lote)
VALUES
    (@Max, N'Antirrábica', DATEADD(DAY, -45, @Hoy), DATEADD(DAY, 320, @Hoy), N'AR-2201'),
    (@Max, N'Parvovirus', DATEADD(DAY, -380, @Hoy), DATEADD(DAY, -15, @Hoy), N'PV-1180'),
    (@Nala, N'Triple felina', DATEADD(DAY, -9, @Hoy), DATEADD(DAY, 3, @Hoy), N'TF-3310'),
    (@Mia, N'Triple felina', @Hoy, DATEADD(DAY, 21, @Hoy), N'TF-3311'),
    (@Toby, N'Moquillo', DATEADD(DAY, -350, @Hoy), DATEADD(DAY, 5, @Hoy), N'MQ-0915'),
    (@Coco, N'Antirrábica', DATEADD(DAY, -200, @Hoy), DATEADD(DAY, 25, @Hoy), N'AR-2150'),
    (@Bruno, N'Leptospirosis', DATEADD(DAY, -90, @Hoy), NULL, N'LP-0042');

PRINT N'Datos de prueba agregados.';
GO

SET NOEXEC OFF;
GO
