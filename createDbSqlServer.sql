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

-- Inserts para servicios básicos y premium
INSERT INTO Servicios (
    NombreServicio, 
    Descripcion, 
    PrecioBase, 
    TiempoEstimado, 
    Activo
) VALUES 
    -- Servicios Básicos
    ('Encordado Estándar', 'Servicio de encordado básico con instalación profesional. Incluye ajuste de tensión según preferencias.', 25.00, 60,  -- 60 minutos
    1),
    
    ('Encordado Premium', 'Servicio de encordado premium con atención especial a los detalles, protectores de marco y ajuste preciso de tensión.', 35.00, 90,  -- 90 minutos
    1),
    
    ('Encordado Urgente', 'Servicio de encordado prioritario, realizado en el mismo día (sujeto a disponibilidad).', 45.00, 60,  -- 60 minutos
    1),
    
    ('Encordado Híbrido', 'Instalación de dos tipos diferentes de cuerdas para vertical y horizontal.', 40.00, 90,  -- 90 minutos
    1),
    
    ('Reencordado con Diagnóstico', 'Incluye evaluación del estado de la raqueta, recomendaciones y encordado personalizado.', 50.00, 120,  -- 120 minutos
    1),
    
    ('Encordado + Personalización', 'Servicio completo de encordado con ajuste de peso y balance de la raqueta.', 60.00, 150,  -- 150 minutos
    1);
GO

-- Inserts para órdenes de encordado
INSERT INTO OrdenesEncordado (
    UsuarioId, RaquetaId, ServicioId, CuerdaId,
    TensionVertical, TensionHorizontal, Estado,
    Comentarios, FechaCreacion, PrecioTotal, EncordadorId
) VALUES 
    -- Órdenes Pendientes
    (1, 1, 1, 1, 24.5, 24.5, 'Pendiente', 
    'Preferencia por tensión uniforme', GETDATE(), 35.99, NULL),
    
    (2, 3, 1, 3, 25.0, 24.0, 'Pendiente',
    'Cliente habitual, cuidado extra', GETDATE(), 39.99, NULL),
    
    -- Órdenes En Proceso
    (1, 2, 2, 4, 26.0, 25.0, 'En Proceso',
    'Urgente para torneo del fin de semana', 
    DATEADD(day, -1, GETDATE()), 45.99, 1),
    
    (3, 6, 1, 2, 23.5, NULL, 'En Proceso',
    'Primera vez con estas cuerdas', 
    DATEADD(day, -1, GETDATE()), 29.99, 2),
    
    -- Órdenes Completadas
    (2, 4, 2, 5, 25.5, 25.5, 'Completado',
    'Cliente satisfecho con el resultado anterior',
    DATEADD(day, -3, GETDATE()), 42.99, 1),
    
    (3, 7, 1, 6, 24.0, 23.0, 'Completado',
    'Tensión diferencial solicitada por el cliente',
    DATEADD(day, -2, GETDATE()), 38.99, 2),
    
    -- Órdenes Entregadas
    (1, 1, 1, 1, 25.0, 25.0, 'Entregado',
    'Encordado mensual',
    DATEADD(day, -7, GETDATE()), 35.99, 1),
    
    (2, 5, 2, 3, 26.5, 26.5, 'Entregado',
    'Cliente pidió máxima tensión posible',
    DATEADD(day, -5, GETDATE()), 44.99, 2),
    
    -- Órdenes Canceladas
    (3, 8, 1, NULL, 24.0, 24.0, 'Cancelado',
    'Cliente canceló por urgencia',
    DATEADD(day, -4, GETDATE()), 29.99, NULL);
GO

-- Inserts para tabla HistorialTensiones
INSERT INTO HistorialTensiones (
    RaquetaId, OrdenId, TensionVertical, TensionHorizontal, CuerdaId, Fecha
) VALUES 
    -- Historial para la raqueta 1 (Babolat Pure Drive)
    (1, 1, 24.5, 24.5, 1, DATEADD(month, -3, GETDATE())),  -- RPM Blast
    (1, 2, 25.0, 25.0, 2, DATEADD(month, -2, GETDATE())),  -- VS Touch
    (1, 7, 25.5, 25.5, 1, DATEADD(month, -1, GETDATE())),  -- RPM Blast

    -- Historial para la raqueta 2 (Head Speed Pro)
    (2, 3, 26.0, 25.0, 4, DATEADD(month, -2, GETDATE())),  -- Pro Hurricane
    (2, 4, 26.5, 25.5, 11, DATEADD(month, -1, GETDATE())), -- Head Hawk

    -- Historial para la raqueta 3 (Wilson Blade)
    (3, 5, 23.5, 23.5, 5, DATEADD(month, -3, GETDATE())),  -- Wilson NXT
    (3, 6, 24.0, 24.0, 6, DATEADD(month, -1, GETDATE())),  -- Wilson Revolve

    -- Historial para la raqueta 4 (Yonex EZONE)
    (4, 8, 25.0, 25.0, 14, DATEADD(month, -2, GETDATE())), -- Yonex Poly Tour Pro
    (4, 9, 25.5, 25.5, 15, DATEADD(month, -1, GETDATE())); -- Yonex Multi-Sensa
GO

-- Inserts para tabla Disponibilidad
INSERT INTO Disponibilidad (
    EncordadorId, DiaSemana, HoraInicio, HoraFin
) VALUES 
    -- Horarios para Encordador 1
    -- Lunes
    (1, 1, '09:00', '14:00'),
    (1, 1, '16:00', '20:00'),
    -- Martes
    (1, 2, '09:00', '14:00'),
    (1, 2, '16:00', '20:00'),
    -- Miércoles
    (1, 3, '09:00', '14:00'),
    (1, 3, '16:00', '20:00'),
    -- Jueves
    (1, 4, '09:00', '14:00'),
    (1, 4, '16:00', '20:00'),
    -- Viernes
    (1, 5, '09:00', '15:00'),

    -- Horarios para Encordador 2
    -- Lunes
    (2, 1, '10:00', '15:00'),
    (2, 1, '17:00', '21:00'),
    -- Martes
    (2, 2, '10:00', '15:00'),
    (2, 2, '17:00', '21:00'),
    -- Miércoles
    (2, 3, '10:00', '15:00'),
    -- Jueves
    (2, 4, '10:00', '15:00'),
    (2, 4, '17:00', '21:00'),
    -- Viernes
    (2, 5, '10:00', '15:00'),
    -- Sábado
    (2, 6, '10:00', '14:00');
GO