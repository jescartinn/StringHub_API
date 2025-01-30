-- Crear la base de datos
CREATE DATABASE StringHubDB;
GO

USE StringHubDB;
GO

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
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
    RaquetaID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    Marca VARCHAR(50) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    NumeroSerie VARCHAR(50),
    Descripcion TEXT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Servicios Disponibles
CREATE TABLE Servicios (
    ServicioID INT IDENTITY(1,1) PRIMARY KEY,
    NombreServicio VARCHAR(100) NOT NULL,
    Descripcion TEXT,
    PrecioBase DECIMAL(10,2) NOT NULL,
    TiempoEstimado INT NOT NULL, -- Tiempo estimado en minutos
    Activo BIT DEFAULT 1
);

-- Tabla de Cuerdas Disponibles
CREATE TABLE Cuerdas (
    CuerdaID INT IDENTITY(1,1) PRIMARY KEY,
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
    OrdenID INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioID INT NOT NULL,
    RaquetaID INT NOT NULL,
    ServicioID INT NOT NULL,
    CuerdaID INT,  -- Puede ser NULL si el cliente trae su propia cuerda
    TensionVertical DECIMAL(4,1) NOT NULL,
    TensionHorizontal DECIMAL(4,1),
    Estado VARCHAR(20) NOT NULL CHECK (Estado IN ('Pendiente', 'En Proceso', 'Completado', 'Entregado', 'Cancelado')),
    Comentarios TEXT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FechaCompletado DATETIME,
    PrecioTotal DECIMAL(10,2) NOT NULL,
    EncordadorID INT,  -- ID del encordador asignado
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID),
    FOREIGN KEY (RaquetaID) REFERENCES Raquetas(RaquetaID),
    FOREIGN KEY (ServicioID) REFERENCES Servicios(ServicioID),
    FOREIGN KEY (CuerdaID) REFERENCES Cuerdas(CuerdaID),
    FOREIGN KEY (EncordadorID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Horarios de Disponibilidad
CREATE TABLE Disponibilidad (
    DisponibilidadID INT IDENTITY(1,1) PRIMARY KEY,
    EncordadorID INT NOT NULL,
    DiaSemana TINYINT NOT NULL CHECK (DiaSemana BETWEEN 1 AND 7),
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    FOREIGN KEY (EncordadorID) REFERENCES Usuarios(UsuarioID)
);

-- Tabla de Historial de Tensiones
CREATE TABLE HistorialTensiones (
    HistorialID INT IDENTITY(1,1) PRIMARY KEY,
    RaquetaID INT NOT NULL,
    OrdenID INT NOT NULL,
    TensionVertical DECIMAL(4,1) NOT NULL,
    TensionHorizontal DECIMAL(4,1),
    CuerdaID INT,
    Fecha DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RaquetaID) REFERENCES Raquetas(RaquetaID),
    FOREIGN KEY (OrdenID) REFERENCES OrdenesEncordado(OrdenID),
    FOREIGN KEY (CuerdaID) REFERENCES Cuerdas(CuerdaID)
);

-- Índices para mejorar el rendimiento
CREATE INDEX IX_OrdenesEncordado_Estado ON OrdenesEncordado(Estado);
CREATE INDEX IX_OrdenesEncordado_FechaCreacion ON OrdenesEncordado(FechaCreacion);
CREATE INDEX IX_Raquetas_UsuarioID ON Raquetas(UsuarioID);
CREATE INDEX IX_HistorialTensiones_RaquetaID ON HistorialTensiones(RaquetaID);
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
    INNER JOIN inserted i ON u.UsuarioID = i.UsuarioID
END;
GO