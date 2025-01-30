-- Crear la base de datos
CREATE DATABASE StringHubDB;
GO

USE StringHubDB;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Contraseña VARCHAR(255) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL,
    Telefono VARCHAR(20),
    TipoUsuario VARCHAR(20) NOT NULL CHECK (TipoUsuario IN ('Cliente', 'Encordador', 'Admin')),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    UltimaModificacion DATETIME DEFAULT GETDATE()
);

-- Tabla de Raquetas
CREATE TABLE Raquetas (
    RaquetaId INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    Marca VARCHAR(50) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    NumeroSerie VARCHAR(50),
    Descripcion TEXT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);

-- Tabla de Servicios Disponibles
CREATE TABLE Servicios (
    ServicioId INT IDENTITY(1,1) PRIMARY KEY,
    NombreServicio VARCHAR(100) NOT NULL,
    Descripcion TEXT,
    PrecioBase DECIMAL(10,2) NOT NULL,
    TiempoEstimado INT NOT NULL, -- Tiempo estimado en minutos
    Activo BIT DEFAULT 1
);

-- Tabla de Cuerdas Disponibles
CREATE TABLE Cuerdas (
    CuerdaId INT IDENTITY(1,1) PRIMARY KEY,
    Marca VARCHAR(50) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    Calibre VARCHAR(20) NOT NULL,
    Material VARCHAR(50) NOT NULL,
    Color VARCHAR(30),
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT DEFAULT 0,
    Activo BIT DEFAULT 1
);

-- Tabla de Órdenes de Encordado
CREATE TABLE OrdenesEncordado (
    OrdenId INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioId INT NOT NULL,
    RaquetaId INT NOT NULL,
    ServicioId INT NOT NULL,
    CuerdaId INT,  -- Puede ser NULL si el cliente trae su propia cuerda
    TensionVertical DECIMAL(4,1) NOT NULL,
    TensionHorizontal DECIMAL(4,1),
    Estado VARCHAR(20) NOT NULL CHECK (Estado IN ('Pendiente', 'En Proceso', 'Completado', 'Entregado', 'Cancelado')),
    Comentarios TEXT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaCompletado DATETIME,
    PrecioTotal DECIMAL(10,2) NOT NULL,
    EncordadorId INT,  -- ID del encordador asignado
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId),
    FOREIGN KEY (RaquetaId) REFERENCES Raquetas(RaquetaId),
    FOREIGN KEY (ServicioId) REFERENCES Servicios(ServicioId),
    FOREIGN KEY (CuerdaId) REFERENCES Cuerdas(CuerdaId),
    FOREIGN KEY (EncordadorId) REFERENCES Usuarios(UsuarioId)
);

-- Tabla de Horarios de Disponibilidad
CREATE TABLE Disponibilidad (
    DisponibilidadId INT IDENTITY(1,1) PRIMARY KEY,
    EncordadorId INT NOT NULL,
    DiaSemana TINYINT NOT NULL CHECK (DiaSemana BETWEEN 1 AND 7),
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    FOREIGN KEY (EncordadorId) REFERENCES Usuarios(UsuarioId)
);

-- Tabla de Historial de Tensiones
CREATE TABLE HistorialTensiones (
    HistorialId INT IDENTITY(1,1) PRIMARY KEY,
    RaquetaId INT NOT NULL,
    OrdenId INT NOT NULL,
    TensionVertical DECIMAL(4,1) NOT NULL,
    TensionHorizontal DECIMAL(4,1),
    CuerdaId INT,
    Fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RaquetaId) REFERENCES Raquetas(RaquetaId),
    FOREIGN KEY (OrdenId) REFERENCES OrdenesEncordado(OrdenId),
    FOREIGN KEY (CuerdaId) REFERENCES Cuerdas(CuerdaId)
);

-- Índices para mejorar el rendimiento
CREATE INDEX IX_OrdenesEncordado_Estado ON OrdenesEncordado(Estado);
CREATE INDEX IX_OrdenesEncordado_FechaCreacion ON OrdenesEncordado(FechaCreacion);
CREATE INDEX IX_Raquetas_UsuarioId ON Raquetas(UsuarioId);
CREATE INDEX IX_HistorialTensiones_RaquetaId ON HistorialTensiones(RaquetaId);
GO

-- Trigger para actualizar UltimaModificacion en Usuarios
CREATE TRIGGER trg_Usuarios_ActualizarUltimaModificacion
ON Usuarios
AFTER UPDATE
AS
BEGIN
    UPDATE Usuarios
    SET UltimaModificacion = GETDATE()
    FROM Usuarios u
    INNER JOIN inserted i ON u.UsuarioId = i.UsuarioId
END;
GO

-- Inserts tabla Usuarios
INSERT INTO Usuarios (Email, Contraseña, Nombre, Apellido, Telefono, TipoUsuario)
VALUES 
    ('juan@email.com', 'password123', 'Juan', 'Pérez', '666111222', 'Cliente'),
    ('ana@email.com', 'password123', 'Ana', 'García', '666333444', 'Cliente'),
    ('carlos@email.com', 'password123', 'Carlos', 'Martínez', '666555666', 'Cliente');
GO

-- Inserts tabla Raquetas
INSERT INTO Raquetas (UsuarioId, Marca, Modelo, NumeroSerie, Descripcion, FechaCreacion)
VALUES
    -- Raquetas de Juan (UsuarioId = 1)
    (1, 'Babolat', 'Pure Drive', 'BD2023001', 'Raqueta principal para competición', GETDATE()),
    (1, 'Head', 'Speed Pro', 'HS2023101', 'Raqueta de respaldo', GETDATE()),
    
    -- Raquetas de Ana (UsuarioId = 2)
    (2, 'Wilson', 'Blade', 'WB2023201', 'Raqueta para tierra batida', GETDATE()),
    (2, 'Yonex', 'EZONE', 'YE2023301', 'Nueva raqueta, en período de adaptación', GETDATE()),
    (2, 'Wilson', 'Pro Staff', 'WP2023401', 'Raqueta antigua', GETDATE()),
    
    -- Raquetas de Carlos (UsuarioId = 3)
    (3, 'Head', 'Prestige', 'HP2023501', 'Raqueta principal', GETDATE()),
    (3, 'Babolat', 'Pure Aero', 'BA2023601', 'Raqueta para entrenamiento', GETDATE()),
    (3, 'Dunlop', 'FX500', 'DF2023701', 'Raqueta nueva sin estrenar', GETDATE()),
    (3, 'Prince', 'Phantom', 'PP2023801', 'Raqueta para pista rápida', GETDATE());
GO

-- Inserts para cuerdas Babolat
INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo)
VALUES 
    ('Babolat', 'RPM Blast', '1.25', 'Poliéster', 'Negro', 15.99, 50, 1),
    ('Babolat', 'VS Touch', '1.30', 'Tripa Natural', 'Natural', 39.99, 20, 1),
    ('Babolat', 'Xcel', '1.30', 'Multifilamento', 'Azul', 19.99, 30, 1),
    ('Babolat', 'Pro Hurricane', '1.25', 'Poliéster', 'Negro', 14.99, 40, 1);

-- Inserts para cuerdas Wilson
INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo)
VALUES 
    ('Wilson', 'NXT', '1.30', 'Multifilamento', 'Natural', 24.99, 35, 1),
    ('Wilson', 'Revolve', '1.25', 'Poliéster', 'Gris', 16.99, 45, 1),
    ('Wilson', 'Champions Choice', '1.30', 'Híbrido', 'Natural/Gris', 34.99, 25, 1);

-- Inserts para cuerdas Luxilon
INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo)
VALUES 
    ('Luxilon', 'Alu Power', '1.25', 'Poliéster', 'Plata', 19.99, 40, 1),
    ('Luxilon', 'Big Banger Original', '1.30', 'Poliéster', 'Natural', 18.99, 35, 1),
    ('Luxilon', '4G', '1.25', 'Poliéster', 'Oro', 19.99, 30, 1);

-- Inserts para cuerdas Head
INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo)
VALUES 
    ('Head', 'Hawk', '1.25', 'Poliéster', 'Gris', 15.99, 40, 1),
    ('Head', 'Velocity MLT', '1.30', 'Multifilamento', 'Natural', 21.99, 30, 1),
    ('Head', 'Lynx', '1.25', 'Poliéster', 'Negro', 16.99, 35, 1);

-- Inserts para cuerdas Yonex
INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo)
VALUES 
    ('Yonex', 'Poly Tour Pro', '1.25', 'Poliéster', 'Negro', 17.99, 35, 1),
    ('Yonex', 'Multi-Sensa', '1.30', 'Multifilamento', 'Natural', 22.99, 25, 1);

-- Inserts para cuerdas Tecnifibre
INSERT INTO Cuerdas (Marca, Modelo, Calibre, Material, Color, Precio, Stock, Activo)
VALUES 
    ('Tecnifibre', 'Black Code', '1.24', 'Poliéster', 'Negro', 16.99, 30, 1),
    ('Tecnifibre', 'NRG2', '1.30', 'Multifilamento', 'Natural', 23.99, 25, 1),
    ('Tecnifibre', 'X-One Biphase', '1.30', 'Multifilamento', 'Rojo', 24.99, 20, 1);
GO