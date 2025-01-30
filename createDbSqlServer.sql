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