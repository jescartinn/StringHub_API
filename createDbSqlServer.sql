-- Crear la base de datos
CREATE DATABASE StringHub;
GO

USE StringHub;
GO

-- Tabla de Usuarios
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Phone VARCHAR(20),
    UserType VARCHAR(20) NOT NULL CHECK (UserType IN ('Cliente', 'Encordador', 'Admin')),
    Created DATETIME DEFAULT GETDATE(),
    LastModified DATETIME DEFAULT GETDATE()
);

-- Tabla de Raquetas
CREATE TABLE Racquets (
    RacquetID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    Brand VARCHAR(50) NOT NULL,
    Model VARCHAR(50) NOT NULL,
    SerialNumber VARCHAR(50),
    Description TEXT,
    Created DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Tabla de Servicios Disponibles
CREATE TABLE Services (
    ServiceID INT IDENTITY(1,1) PRIMARY KEY,
    ServiceName VARCHAR(100) NOT NULL,
    Description TEXT,
    BasePrice DECIMAL(10,2) NOT NULL,
    EstimatedTime INT NOT NULL, -- Tiempo estimado en minutos
    Active BIT DEFAULT 1
);

-- Tabla de Cuerdas Disponibles
CREATE TABLE Strings (
    StringID INT IDENTITY(1,1) PRIMARY KEY,
    Brand VARCHAR(50) NOT NULL,
    Model VARCHAR(50) NOT NULL,
    Gauge VARCHAR(20) NOT NULL,
    Material VARCHAR(50) NOT NULL,
    Color VARCHAR(30),
    Price DECIMAL(10,2) NOT NULL,
    Stock INT DEFAULT 0,
    Active BIT DEFAULT 1
);

-- Tabla de Órdenes de Encordado
CREATE TABLE StringingOrders (
    OrderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    RacquetID INT NOT NULL,
    ServiceID INT NOT NULL,
    StringID INT,  -- Puede ser NULL si el cliente trae su propia cuerda
    MainTension DECIMAL(4,1) NOT NULL,
    CrossTension DECIMAL(4,1),
    Status VARCHAR(20) NOT NULL CHECK (Status IN ('Pendiente', 'En Proceso', 'Completado', 'Entregado', 'Cancelado')),
    Comments TEXT,
    Created DATETIME DEFAULT GETDATE(),
    Completed DATETIME,
    TotalPrice DECIMAL(10,2) NOT NULL,
    StringerID INT,  -- ID del encordador asignado
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (RacquetID) REFERENCES Racquets(RacquetID),
    FOREIGN KEY (ServiceID) REFERENCES Services(ServiceID),
    FOREIGN KEY (StringID) REFERENCES Strings(StringID),
    FOREIGN KEY (StringerID) REFERENCES Users(UserID)
);

-- Tabla de Horarios de Disponibilidad
CREATE TABLE Availability (
    AvailabilityID INT IDENTITY(1,1) PRIMARY KEY,
    StringerID INT NOT NULL,
    DayOfWeek TINYINT NOT NULL CHECK (DayOfWeek BETWEEN 1 AND 7),
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    FOREIGN KEY (StringerID) REFERENCES Users(UserID)
);

-- Tabla de Historial de Tensiones
CREATE TABLE TensionHistory (
    HistoryID INT IDENTITY(1,1) PRIMARY KEY,
    RacquetID INT NOT NULL,
    OrderID INT NOT NULL,
    MainTension DECIMAL(4,1) NOT NULL,
    CrossTension DECIMAL(4,1),
    StringID INT,
    Date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (RacquetID) REFERENCES Racquets(RacquetID),
    FOREIGN KEY (OrderID) REFERENCES StringingOrders(OrderID),
    FOREIGN KEY (StringID) REFERENCES Strings(StringID)
);

-- Índices para mejorar el rendimiento
CREATE INDEX IX_StringingOrders_Status ON StringingOrders(Status);
CREATE INDEX IX_StringingOrders_Created ON StringingOrders(Created);
CREATE INDEX IX_Racquets_UserID ON Racquets(UserID);
CREATE INDEX IX_TensionHistory_RacquetID ON TensionHistory(RacquetID);
GO

-- Trigger para actualizar LastModified en Users
CREATE TRIGGER trg_Users_UpdateLastModified
ON Users
AFTER UPDATE
AS
BEGIN
    UPDATE Users
    SET LastModified = GETDATE()
    FROM Users u
    INNER JOIN inserted i ON u.UserID = i.UserID
END;
GO