/*
    PersianVet - Entregable 2
    Este script crea la base de datos, sus tablas y datos de demostración.
    Puede ejecutarse desde SQL Server Management Studio o con sqlcmd usando UTF-8.
*/

IF DB_ID(N'PersianVetDB') IS NULL
BEGIN
    CREATE DATABASE PersianVetDB;
END;
GO

USE PersianVetDB;
GO

IF OBJECT_ID(N'dbo.Clientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        IdCliente INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Clientes PRIMARY KEY,
        Nombres NVARCHAR(80) NOT NULL,
        Apellidos NVARCHAR(80) NOT NULL,
        Telefono NVARCHAR(20) NOT NULL,
        Direccion NVARCHAR(200) NOT NULL,
        Correo NVARCHAR(120) NOT NULL,
        FechaRegistro DATE NOT NULL CONSTRAINT DF_Clientes_FechaRegistro DEFAULT CAST(GETDATE() AS DATE),
        Activo BIT NOT NULL CONSTRAINT DF_Clientes_Activo DEFAULT 1
    );

    CREATE UNIQUE INDEX UX_Clientes_Correo ON dbo.Clientes(Correo);
END;
GO

IF OBJECT_ID(N'dbo.Veterinarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Veterinarios
    (
        IdVeterinario INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Veterinarios PRIMARY KEY,
        Nombres NVARCHAR(80) NOT NULL,
        Apellidos NVARCHAR(80) NOT NULL,
        Especialidad NVARCHAR(100) NOT NULL,
        Telefono NVARCHAR(20) NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Veterinarios_Activo DEFAULT 1
    );
END;
GO

IF OBJECT_ID(N'dbo.Mascotas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Mascotas
    (
        IdMascota INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Mascotas PRIMARY KEY,
        IdCliente INT NOT NULL,
        Nombre NVARCHAR(80) NOT NULL,
        Especie NVARCHAR(40) NOT NULL,
        Raza NVARCHAR(80) NOT NULL,
        Sexo NVARCHAR(10) NOT NULL,
        FechaNacimiento DATE NOT NULL,
        Color NVARCHAR(80) NOT NULL,
        Peso DECIMAL(6,2) NOT NULL,
        Observaciones NVARCHAR(500) NULL,
        Activa BIT NOT NULL CONSTRAINT DF_Mascotas_Activa DEFAULT 1,
        CONSTRAINT FK_Mascotas_Clientes FOREIGN KEY (IdCliente) REFERENCES dbo.Clientes(IdCliente),
        CONSTRAINT CK_Mascotas_Sexo CHECK (Sexo IN (N'Macho', N'Hembra')),
        CONSTRAINT CK_Mascotas_Peso CHECK (Peso > 0)
    );

    CREATE INDEX IX_Mascotas_IdCliente ON dbo.Mascotas(IdCliente);
END;
GO

IF OBJECT_ID(N'dbo.Citas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Citas
    (
        IdCita INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Citas PRIMARY KEY,
        IdMascota INT NOT NULL,
        IdVeterinario INT NOT NULL,
        FechaHora DATETIME2(0) NOT NULL,
        Motivo NVARCHAR(200) NOT NULL,
        Estado NVARCHAR(20) NOT NULL CONSTRAINT DF_Citas_Estado DEFAULT N'Programada',
        CONSTRAINT FK_Citas_Mascotas FOREIGN KEY (IdMascota) REFERENCES dbo.Mascotas(IdMascota),
        CONSTRAINT FK_Citas_Veterinarios FOREIGN KEY (IdVeterinario) REFERENCES dbo.Veterinarios(IdVeterinario),
        CONSTRAINT CK_Citas_Estado CHECK (Estado IN (N'Programada', N'Confirmada', N'Completada', N'Cancelada'))
    );

    CREATE INDEX IX_Citas_FechaHora ON dbo.Citas(FechaHora);
    CREATE INDEX IX_Citas_IdMascota ON dbo.Citas(IdMascota);
END;
GO

IF OBJECT_ID(N'dbo.Consultas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Consultas
    (
        IdConsulta INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Consultas PRIMARY KEY,
        IdMascota INT NOT NULL,
        IdVeterinario INT NOT NULL,
        FechaHora DATETIME2(0) NOT NULL CONSTRAINT DF_Consultas_FechaHora DEFAULT SYSDATETIME(),
        Motivo NVARCHAR(200) NOT NULL,
        Diagnostico NVARCHAR(500) NOT NULL,
        Observaciones NVARCHAR(500) NULL,
        CONSTRAINT FK_Consultas_Mascotas FOREIGN KEY (IdMascota) REFERENCES dbo.Mascotas(IdMascota),
        CONSTRAINT FK_Consultas_Veterinarios FOREIGN KEY (IdVeterinario) REFERENCES dbo.Veterinarios(IdVeterinario)
    );

    CREATE INDEX IX_Consultas_IdMascota ON dbo.Consultas(IdMascota);
END;
GO

IF OBJECT_ID(N'dbo.Tratamientos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tratamientos
    (
        IdTratamiento INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tratamientos PRIMARY KEY,
        IdConsulta INT NOT NULL,
        Medicamento NVARCHAR(120) NOT NULL,
        Dosis NVARCHAR(80) NOT NULL,
        Frecuencia NVARCHAR(100) NOT NULL,
        Duracion NVARCHAR(80) NOT NULL,
        Indicaciones NVARCHAR(300) NULL,
        CONSTRAINT FK_Tratamientos_Consultas FOREIGN KEY (IdConsulta) REFERENCES dbo.Consultas(IdConsulta)
    );

    CREATE INDEX IX_Tratamientos_IdConsulta ON dbo.Tratamientos(IdConsulta);
END;
GO

IF OBJECT_ID(N'dbo.Vacunas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Vacunas
    (
        IdVacuna INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Vacunas PRIMARY KEY,
        IdMascota INT NOT NULL,
        NombreVacuna NVARCHAR(120) NOT NULL,
        FechaAplicacion DATE NOT NULL,
        ProximaDosis DATE NULL,
        Lote NVARCHAR(60) NOT NULL,
        CONSTRAINT FK_Vacunas_Mascotas FOREIGN KEY (IdMascota) REFERENCES dbo.Mascotas(IdMascota),
        CONSTRAINT CK_Vacunas_ProximaDosis CHECK (ProximaDosis IS NULL OR ProximaDosis >= FechaAplicacion)
    );

    CREATE INDEX IX_Vacunas_ProximaDosis ON dbo.Vacunas(ProximaDosis);
END;
GO

IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        IdUsuario INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Usuarios PRIMARY KEY,
        NombreUsuario NVARCHAR(50) NOT NULL,
        ClaveHash CHAR(64) NOT NULL,
        Rol NVARCHAR(40) NOT NULL,
        Activo BIT NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT 1
    );

    CREATE UNIQUE INDEX UX_Usuarios_NombreUsuario ON dbo.Usuarios(NombreUsuario);
END;
GO

-- Catálogo de precios en córdobas (C$) de consultas, vacunas y medicamentos.
IF OBJECT_ID(N'dbo.Tarifas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tarifas
    (
        IdTarifa INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Tarifas PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Tipo NVARCHAR(20) NOT NULL,
        Precio DECIMAL(10,2) NOT NULL,
        Activa BIT NOT NULL CONSTRAINT DF_Tarifas_Activa DEFAULT 1,
        CONSTRAINT CK_Tarifas_Tipo CHECK (Tipo IN (N'Servicio', N'Vacuna', N'Medicamento')),
        CONSTRAINT CK_Tarifas_Precio CHECK (Precio >= 0)
    );

    CREATE UNIQUE INDEX UX_Tarifas_Nombre ON dbo.Tarifas(Nombre);
END;
GO

-- Precio cobrado en cada registro. Se guarda el valor del momento: si la tarifa cambia, el historial no cambia.
IF COL_LENGTH(N'dbo.Citas', N'Servicio') IS NULL
    ALTER TABLE dbo.Citas ADD Servicio NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.Citas', N'Precio') IS NULL
    ALTER TABLE dbo.Citas ADD Precio DECIMAL(10,2) NOT NULL CONSTRAINT DF_Citas_Precio DEFAULT 0;
IF COL_LENGTH(N'dbo.Consultas', N'Servicio') IS NULL
    ALTER TABLE dbo.Consultas ADD Servicio NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.Consultas', N'Precio') IS NULL
    ALTER TABLE dbo.Consultas ADD Precio DECIMAL(10,2) NOT NULL CONSTRAINT DF_Consultas_Precio DEFAULT 0;
IF COL_LENGTH(N'dbo.Tratamientos', N'Precio') IS NULL
    ALTER TABLE dbo.Tratamientos ADD Precio DECIMAL(10,2) NOT NULL CONSTRAINT DF_Tratamientos_Precio DEFAULT 0;
IF COL_LENGTH(N'dbo.Vacunas', N'Precio') IS NULL
    ALTER TABLE dbo.Vacunas ADD Precio DECIMAL(10,2) NOT NULL CONSTRAINT DF_Vacunas_Precio DEFAULT 0;
GO

-- Cada cambio de precio de una tarifa queda registrado: fecha, precio anterior, precio nuevo y usuario.
IF OBJECT_ID(N'dbo.HistorialPrecios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HistorialPrecios
    (
        IdHistorial INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_HistorialPrecios PRIMARY KEY,
        IdTarifa INT NOT NULL,
        PrecioAnterior DECIMAL(10,2) NULL,
        PrecioNuevo DECIMAL(10,2) NOT NULL,
        Fecha DATETIME2(0) NOT NULL CONSTRAINT DF_HistorialPrecios_Fecha DEFAULT SYSDATETIME(),
        Usuario NVARCHAR(50) NOT NULL,
        CONSTRAINT FK_HistorialPrecios_Tarifas FOREIGN KEY (IdTarifa) REFERENCES dbo.Tarifas(IdTarifa)
    );

    CREATE INDEX IX_HistorialPrecios_IdTarifa ON dbo.HistorialPrecios(IdTarifa);
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_ResumenDashboard
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*) FROM dbo.Mascotas WHERE Activa = 1) AS MascotasRegistradas,
        (SELECT COUNT(*) FROM dbo.Citas
         WHERE CAST(FechaHora AS DATE) = CAST(GETDATE() AS DATE)
           AND Estado <> N'Cancelada') AS CitasHoy,
        (SELECT COUNT(*) FROM dbo.Vacunas
         WHERE ProximaDosis BETWEEN CAST(GETDATE() AS DATE)
                                AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE))) AS VacunasProximas,
        (SELECT COUNT(*) FROM dbo.Consultas
         WHERE YEAR(FechaHora) = YEAR(GETDATE())
           AND MONTH(FechaHora) = MONTH(GETDATE())) AS ConsultasMes;
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_HistorialMascota
    @IdMascota INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        c.IdConsulta,
        c.FechaHora,
        c.Motivo,
        c.Diagnostico,
        c.Observaciones,
        v.Nombres + N' ' + v.Apellidos AS Veterinario,
        STRING_AGG(t.Medicamento + N' - ' + t.Dosis, N'; ') AS Tratamientos
    FROM dbo.Consultas c
    INNER JOIN dbo.Veterinarios v ON v.IdVeterinario = c.IdVeterinario
    LEFT JOIN dbo.Tratamientos t ON t.IdConsulta = c.IdConsulta
    WHERE c.IdMascota = @IdMascota
    GROUP BY c.IdConsulta, c.FechaHora, c.Motivo, c.Diagnostico,
             c.Observaciones, v.Nombres, v.Apellidos
    ORDER BY c.FechaHora DESC;
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'admin')
BEGIN
    INSERT INTO dbo.Usuarios (NombreUsuario, ClaveHash, Rol, Activo)
    VALUES (N'admin', '6F05170ADEB309351C4730D0C9E819FA5DD31F0DBB9C9D535F3C0EE38FD16E2E', N'Administrador', 1);
END;
ELSE
BEGIN
    UPDATE dbo.Usuarios
    SET ClaveHash = '6F05170ADEB309351C4730D0C9E819FA5DD31F0DBB9C9D535F3C0EE38FD16E2E',
        Rol = N'Administrador',
        Activo = 1
    WHERE NombreUsuario = N'admin';
END;
GO

-- Usuarios de demostración con otros roles (clave: persian123).
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'recepcion')
BEGIN
    INSERT INTO dbo.Usuarios (NombreUsuario, ClaveHash, Rol, Activo)
    VALUES (N'recepcion', '6F05170ADEB309351C4730D0C9E819FA5DD31F0DBB9C9D535F3C0EE38FD16E2E', N'Recepcionista', 1);
END;
IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'veterinario')
BEGIN
    INSERT INTO dbo.Usuarios (NombreUsuario, ClaveHash, Rol, Activo)
    VALUES (N'veterinario', '6F05170ADEB309351C4730D0C9E819FA5DD31F0DBB9C9D535F3C0EE38FD16E2E', N'Veterinario', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Tarifas)
BEGIN
    INSERT INTO dbo.Tarifas (Nombre, Tipo, Precio)
    VALUES
        (N'Consulta general', N'Servicio', 400.00),
        (N'Control y seguimiento', N'Servicio', 250.00),
        (N'Consulta dermatológica', N'Servicio', 600.00),
        (N'Limpieza dental', N'Servicio', 1200.00),
        (N'Radiografía', N'Servicio', 900.00),
        (N'Cirugía menor', N'Servicio', 2500.00),
        (N'Aplicación de vacuna', N'Servicio', 150.00),
        (N'Antirrábica', N'Vacuna', 350.00),
        (N'Parvovirus', N'Vacuna', 450.00),
        (N'Moquillo', N'Vacuna', 450.00),
        (N'Triple felina', N'Vacuna', 500.00),
        (N'Leptospirosis', N'Vacuna', 400.00),
        (N'Amoxicilina', N'Medicamento', 200.00),
        (N'Cetirizina', N'Medicamento', 120.00),
        (N'Ketoconazol', N'Medicamento', 280.00),
        (N'Champú medicado', N'Medicamento', 350.00),
        (N'Clindamicina', N'Medicamento', 300.00),
        (N'Meloxicam', N'Medicamento', 220.00),
        (N'Glucosamina', N'Medicamento', 650.00),
        (N'Prednisolona', N'Medicamento', 180.00);
END;
GO

-- Precio inicial de cada tarifa que todavía no tiene historial.
INSERT INTO dbo.HistorialPrecios (IdTarifa, PrecioAnterior, PrecioNuevo, Fecha, Usuario)
SELECT t.IdTarifa, NULL, t.Precio, DATEADD(DAY, -90, CAST(CAST(GETDATE() AS DATE) AS DATETIME2(0))), N'admin'
FROM dbo.Tarifas t
WHERE NOT EXISTS (SELECT 1 FROM dbo.HistorialPrecios h WHERE h.IdTarifa = t.IdTarifa);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Clientes)
BEGIN
    INSERT INTO dbo.Clientes (Nombres, Apellidos, Telefono, Direccion, Correo, FechaRegistro, Activo)
    VALUES
        (N'Luis Fernando', N'Gómez Ramírez', N'8888-1200', N'Residencial Las Flores, casa 12', N'luis.gomez@example.com', CAST(GETDATE() AS DATE), 1),
        (N'María Camila', N'Fernández López', N'8888-2300', N'Barrio Central, avenida 4', N'maria.fernandez@example.com', CAST(GETDATE() AS DATE), 1),
        (N'Andrés Felipe', N'Martínez Peña', N'8888-3400', N'Colonia San José, casa 18', N'andres.martinez@example.com', CAST(GETDATE() AS DATE), 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Veterinarios)
BEGIN
    INSERT INTO dbo.Veterinarios (Nombres, Apellidos, Especialidad, Telefono, Activo)
    VALUES
        (N'Ana', N'López', N'Medicina general', N'8777-1100', 1),
        (N'Carlos', N'Ruiz', N'Vacunación y prevención', N'8777-2200', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Mascotas)
BEGIN
    DECLARE @ClienteLuis INT = (SELECT TOP 1 IdCliente FROM dbo.Clientes WHERE Correo = N'luis.gomez@example.com');
    DECLARE @ClienteMaria INT = (SELECT TOP 1 IdCliente FROM dbo.Clientes WHERE Correo = N'maria.fernandez@example.com');

    INSERT INTO dbo.Mascotas
        (IdCliente, Nombre, Especie, Raza, Sexo, FechaNacimiento, Color, Peso, Observaciones, Activa)
    VALUES
        (@ClienteLuis, N'Persian', N'Felino', N'Persa', N'Hembra', '2022-03-15', N'Naranja y blanco', 4.20, N'Mascota tranquila y cariñosa.', 1),
        (@ClienteMaria, N'Mishu', N'Felino', N'Persa', N'Hembra', '2021-07-02', N'Gris', 4.10, N'Sensible a cambios de alimento.', 1),
        (@ClienteLuis, N'Rocky', N'Canino', N'Golden Retriever', N'Macho', '2020-11-10', N'Dorado', 28.50, N'Muy activo.', 1);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Citas)
BEGIN
    DECLARE @MascotaPersian INT = (SELECT TOP 1 IdMascota FROM dbo.Mascotas WHERE Nombre = N'Persian');
    DECLARE @MascotaRocky INT = (SELECT TOP 1 IdMascota FROM dbo.Mascotas WHERE Nombre = N'Rocky');
    DECLARE @VeterinariaAna INT = (SELECT TOP 1 IdVeterinario FROM dbo.Veterinarios WHERE Nombres = N'Ana');
    DECLARE @VeterinarioCarlos INT = (SELECT TOP 1 IdVeterinario FROM dbo.Veterinarios WHERE Nombres = N'Carlos');

    INSERT INTO dbo.Citas (IdMascota, IdVeterinario, FechaHora, Servicio, Motivo, Estado, Precio)
    VALUES
        (@MascotaPersian, @VeterinariaAna, DATEADD(HOUR, 10, CAST(DATEADD(DAY, 1, CAST(GETDATE() AS DATE)) AS DATETIME2)), N'Consulta general', N'Consulta general', N'Programada', 400.00),
        (@MascotaRocky, @VeterinarioCarlos, DATEADD(HOUR, 14, CAST(DATEADD(DAY, 2, CAST(GETDATE() AS DATE)) AS DATETIME2)), N'Aplicación de vacuna', N'Control de vacunación', N'Confirmada', 150.00);
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Vacunas)
BEGIN
    DECLARE @MascotaVacuna INT = (SELECT TOP 1 IdMascota FROM dbo.Mascotas WHERE Nombre = N'Persian');
    INSERT INTO dbo.Vacunas (IdMascota, NombreVacuna, FechaAplicacion, ProximaDosis, Lote, Precio)
    VALUES (@MascotaVacuna, N'Triple felina', CAST(GETDATE() AS DATE), DATEADD(DAY, 21, CAST(GETDATE() AS DATE)), N'TF-2026-01', 500.00);
END;
GO

PRINT N'La base de datos PersianVetDB está lista.';
GO
