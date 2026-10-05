USE db_envios;

DELIMITER $$


-- 
-- ALTA DE ENVÍO COMPLETO
-- 

DROP PROCEDURE IF EXISTS altaEnvioCompleto $$

CREATE PROCEDURE altaEnvioCompleto(
    IN p_idCliente INT,
    IN p_idOrigen INT,
    IN p_idDestino INT,
    IN p_peso DECIMAL(10,2),
    IN p_alto DECIMAL(10,2),
    IN p_ancho DECIMAL(10,2),
    IN p_largo DECIMAL(10,2),
    IN p_distancia DECIMAL(10,2),
    IN p_idModalidad INT,
    IN p_costo DECIMAL(10,2),
    IN p_tiempoEstimado INT
)
BEGIN

    DECLARE v_idPaquete INT;
    DECLARE v_idEnvio INT;

    -- 
    -- MANEJO DE ERRORES
    -- 

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;


    -- 
    -- VALIDAR CLIENTE
    -- 

    IF NOT EXISTS (
        SELECT 1
        FROM Cliente
        WHERE IdCliente = p_idCliente
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El cliente no existe';
    END IF;


    -- 
    -- VALIDAR DIRECCION DE ORIGEN
    -- 

    IF NOT EXISTS (
        SELECT 1
        FROM Direccion
        WHERE IdDireccion = p_idOrigen
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La direccion de origen no existe';
    END IF;


    -- 
    -- VALIDAR DIRECCION DE DESTINO
    -- 

    IF NOT EXISTS (
        SELECT 1
        FROM Direccion
        WHERE IdDireccion = p_idDestino
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La direccion de destino no existe';
    END IF;


    -- 
    -- VALIDAR PAQUETE
    -- 

    IF p_peso <= 0 OR p_peso > 50 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Peso invalido';
    END IF;

    IF p_alto <= 0 OR p_alto > 20 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Alto invalido';
    END IF;

    IF p_ancho <= 0 OR p_ancho > 20 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Ancho invalido';
    END IF;

    IF p_largo <= 0 OR p_largo > 20 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Largo invalido';
    END IF;


    -- 
    -- VALIDAR DISTANCIA
    -- 

    IF p_distancia <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La distancia debe ser mayor a cero';
    END IF;


    -- 
    -- VALIDAR MODALIDAD
    -- 

    IF NOT EXISTS (
        SELECT 1
        FROM Modalidad
        WHERE IdModalidad = p_idModalidad
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La modalidad no existe';
    END IF;


    -- 
    -- VALIDAR COSTO
    -- 

    IF p_costo < 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El costo no puede ser negativo';
    END IF;


    -- 
    -- VALIDAR TIEMPO
    -- 

    IF p_tiempoEstimado <= 0 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El tiempo estimado debe ser mayor a cero';
    END IF;


    -- 
    -- INICIAR TRANSACCION
    -- 

    START TRANSACTION;


    -- 
    -- CREAR PAQUETE
    -- 

    INSERT INTO Paquete (
        Peso,
        Alto,
        Ancho,
        Largo
    )
    VALUES (
        p_peso,
        p_alto,
        p_ancho,
        p_largo
    );

    SET v_idPaquete = LAST_INSERT_ID();


    -- 
    -- CREAR ENVIO
    -- 

    INSERT INTO Envio (
        IdCliente,
        IdPaquete,
        IdModalidad,
        IdDireccionOrigen,
        IdDireccionDestino,
        Distancia,
        Costo,
        TiempoEstimado,
        Estado
    )
    VALUES (
        p_idCliente,
        v_idPaquete,
        p_idModalidad,
        p_idOrigen,
        p_idDestino,
        p_distancia,
        p_costo,
        p_tiempoEstimado,
        'PENDIENTE'
    );

    SET v_idEnvio = LAST_INSERT_ID();


    -- 
    -- CREAR HISTORIAL INICIAL
    -- 

    INSERT INTO HistorialEstado (
        IdEnvio,
        EstadoAnterior,
        EstadoNuevo
    )
    VALUES (
        v_idEnvio,
        NULL,
        'PENDIENTE'
    );


    -- 
    -- CONFIRMAR
    -- 

    COMMIT;

END $$



-- 
-- CAMBIAR ESTADO DEL ENVIO
-- 

DROP PROCEDURE IF EXISTS cambiarEstadoEnvio $$

CREATE PROCEDURE cambiarEstadoEnvio(
    IN p_idEnvio INT,
    IN p_nuevoEstado VARCHAR(45)
)
BEGIN

    DECLARE v_estadoActual VARCHAR(45);


    -- 
    -- OBTENER ESTADO ACTUAL
    -- 

    SELECT Estado
    INTO v_estadoActual
    FROM Envio
    WHERE IdEnvio = p_idEnvio;


    -- 
    -- VALIDAR ENVIO
    -- 

    IF v_estadoActual IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El envio no existe';
    END IF;


    -- 
    -- VALIDAR NUEVO ESTADO
    -- 

    IF p_nuevoEstado NOT IN (
        'PENDIENTE',
        'EN_PROCESO',
        'ENTREGADO',
        'CANCELADO'
    ) THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Estado invalido';
    END IF;


    -- 
    -- VALIDAR TRANSICIONES
    -- 

    IF v_estadoActual = 'PENDIENTE' THEN

        IF p_nuevoEstado NOT IN (
            'EN_PROCESO',
            'CANCELADO'
        ) THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT =
                'Desde PENDIENTE solo se puede pasar a EN_PROCESO o CANCELADO';
        END IF;

    ELSEIF v_estadoActual = 'EN_PROCESO' THEN

        IF p_nuevoEstado NOT IN (
            'ENTREGADO',
            'CANCELADO'
        ) THEN
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT =
                'Desde EN_PROCESO solo se puede pasar a ENTREGADO o CANCELADO';
        END IF;

    ELSE

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
            'No se puede cambiar el estado de un envio ENTREGADO o CANCELADO';

    END IF;


    -- 
    -- ACTUALIZAR ENVIO
    -- 

    UPDATE Envio
    SET Estado = p_nuevoEstado
    WHERE IdEnvio = p_idEnvio;


    -- 
    -- GUARDAR HISTORIAL
    -- 

    INSERT INTO HistorialEstado (
        IdEnvio,
        EstadoAnterior,
        EstadoNuevo
    )
    VALUES (
        p_idEnvio,
        v_estadoActual,
        p_nuevoEstado
    );

END $$



-- 
-- CANCELAR ENVIO
-- 

DROP PROCEDURE IF EXISTS cancelarEnvio $$

CREATE PROCEDURE cancelarEnvio(
    IN p_idEnvio INT
)
BEGIN

    DECLARE v_estadoActual VARCHAR(45);


    -- 
    -- OBTENER ESTADO ACTUAL
    -- 

    SELECT Estado
    INTO v_estadoActual
    FROM Envio
    WHERE IdEnvio = p_idEnvio;


    -- 
    -- VALIDAR ENVIO
    -- 

    IF v_estadoActual IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El envio no existe';
    END IF;


    -- 
    -- VALIDAR ESTADO
    -- 

    IF v_estadoActual = 'ENTREGADO' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
            'No se puede cancelar un envio entregado';
    END IF;


    IF v_estadoActual = 'CANCELADO' THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
            'El envio ya esta cancelado';
    END IF;


    -- 
    -- ACTUALIZAR ESTADO
    -- 

    UPDATE Envio
    SET Estado = 'CANCELADO'
    WHERE IdEnvio = p_idEnvio;


    -- 
    -- GUARDAR HISTORIAL
    -- 

    INSERT INTO HistorialEstado (
        IdEnvio,
        EstadoAnterior,
        EstadoNuevo
    )
    VALUES (
        p_idEnvio,
        v_estadoActual,
        'CANCELADO'
    );

END $$


DELIMITER ;