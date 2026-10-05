DROP DATABASE IF EXISTS db_envios;
CREATE DATABASE db_envios;

USE db_envios;



-- TABLA CLIENTE


CREATE TABLE Cliente (
    IdCliente INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(45) NOT NULL,
    Email VARCHAR(45) NOT NULL
);



-- TABLA DIRECCION


CREATE TABLE Direccion (
    IdDireccion INT AUTO_INCREMENT PRIMARY KEY,
    Calle VARCHAR(45) NOT NULL,
    Numero VARCHAR(45) NOT NULL,
    Ciudad VARCHAR(45) NOT NULL
);



-- TABLA PAQUETE


CREATE TABLE Paquete (
    IdPaquete INT AUTO_INCREMENT PRIMARY KEY,

    Peso DECIMAL(10,2) NOT NULL,
    Alto DECIMAL(10,2) NOT NULL,
    Ancho DECIMAL(10,2) NOT NULL,
    Largo DECIMAL(10,2) NOT NULL,

    CONSTRAINT CK_Paquete_Peso
        CHECK (Peso > 0),

    CONSTRAINT CK_Paquete_Alto
        CHECK (Alto > 0),

    CONSTRAINT CK_Paquete_Ancho
        CHECK (Ancho > 0),

    CONSTRAINT CK_Paquete_Largo
        CHECK (Largo > 0)
);



-- TABLA MODALIDAD


CREATE TABLE Modalidad (
    IdModalidad INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(45) NOT NULL UNIQUE
);



-- MODALIDADES


INSERT INTO Modalidad (Nombre)
VALUES
    ('ESTANDAR'),
    ('EXPRESS'),
    ('PRIORITARIO');



-- TABLA ENVIO


CREATE TABLE Envio (
    IdEnvio INT AUTO_INCREMENT PRIMARY KEY,

    IdCliente INT NOT NULL,
    IdPaquete INT NOT NULL,
    IdModalidad INT NOT NULL,

    IdDireccionOrigen INT NOT NULL,
    IdDireccionDestino INT NOT NULL,

    Distancia DECIMAL(10,2) NOT NULL,

    Costo DECIMAL(10,2) NOT NULL,
    TiempoEstimado INT NOT NULL,

    FechaCreacion DATETIME NOT NULL
        DEFAULT CURRENT_TIMESTAMP,

    Estado VARCHAR(45) NOT NULL
        DEFAULT 'PENDIENTE',


   
    -- CLAVES FORANEAS
   

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


   
    -- RESTRICCIONES
   

    CONSTRAINT CK_Envio_Distancia
        CHECK (Distancia > 0),

    CONSTRAINT CK_Envio_Costo
        CHECK (Costo >= 0),

    CONSTRAINT CK_Envio_Tiempo
        CHECK (TiempoEstimado > 0)
);



-- TABLA HISTORIAL DE ESTADOS


CREATE TABLE HistorialEstado (
    IdHistorial INT AUTO_INCREMENT PRIMARY KEY,

    IdEnvio INT NOT NULL,

    EstadoAnterior VARCHAR(45),
    EstadoNuevo VARCHAR(45) NOT NULL,

    FechaCambio DATETIME NOT NULL
        DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Historial_Envio
        FOREIGN KEY (IdEnvio)
        REFERENCES Envio(IdEnvio)
);
