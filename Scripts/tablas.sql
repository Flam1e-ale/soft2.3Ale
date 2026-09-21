CREATE DATABASE db_envios;

USE db_envios;
CREATE TABLE Cliente (
    IdCliente INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL ,
    
);
CREATE TABLE Direccion (
    IdDireccion INT AUTO_INCREMENT PRIMARY KEY,
    Calle VARCHAR(150) NOT NULL,
    Numero VARCHAR(10) NOT NULL,
    Ciudad VARCHAR(100) NOT NULL
  
);
CREATE TABLE Paquete (
    IdPaquete INT AUTO_INCREMENT PRIMARY KEY,
    Peso DECIMAL(10,2) NOT NULL,
    Alto DECIMAL(10,2) NOT NULL,
    Ancho DECIMAL(10,2) NOT NULL,
    Largo DECIMAL(10,2) NOT NULL
);
CREATE TABLE Envio (
    IdEnvio INT AUTO_INCREMENT PRIMARY KEY,

    IdCliente INT NOT NULL,
    IdPaquete INT NOT NULL,
    IdModalidad INT NOT NULL,

    IdDireccionOrigen INT NOT NULL,
    IdDireccionDestino INT NOT NULL,

    Costo DECIMAL(10,2) NOT NULL,
    TiempoEstimado INT NOT NULL,

    FechaCreacion DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    Estado VARCHAR(30) NOT NULL DEFAULT 'Pendiente',

    CONSTRAINT FK_Envio_Cliente
        FOREIGN KEY (IdCliente)
        REFERENCES Cliente(IdCliente),

    CONSTRAINT FK_Envio_Paquete
        FOREIGN KEY (IdPaquete)
        REFERENCES Paquete(IdPaquete),

    CONSTRAINT FK_Envio_Modalidad
        FOREIGN KEY (IdModalidad)
        REFERENCES Modalidad(IdModalidad),

    CONSTRAINT FK_Envio_Origen
        FOREIGN KEY (IdDireccionOrigen)
        REFERENCES Direccion(IdDireccion),

    CONSTRAINT FK_Envio_Destino
        FOREIGN KEY (IdDireccionDestino)
        REFERENCES Direccion(IdDireccion),

    CONSTRAINT CK_Envio_Costo
        CHECK (Costo >= 0),

    CONSTRAINT CK_Envio_Tiempo
        CHECK (TiempoEstimado > 0)
);
CREATE TABLE HistorialEstado (
    IdHistorial INT AUTO_INCREMENT PRIMARY KEY,

    IdEnvio INT NOT NULL,

    EstadoAnterior VARCHAR(30),
    EstadoNuevo VARCHAR(30) NOT NULL,

    FechaCambio DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Historial_Envio
        FOREIGN KEY (IdEnvio)
        REFERENCES Envio(IdEnvio)
);
