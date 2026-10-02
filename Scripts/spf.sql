use db_envios;
DROP PROCEDURE IF EXISTS altaEnvioCompleto;
DROP PROCEDURE IF EXISTS cambiarEstadoEnvio;
DROP PROCEDURE IF EXISTS cancelarEnvio;

DELIMITER $$

-- REGISTRAR UN ENVÍO COMPLETO


CREATE PROCEDURE altaEnvioCompleto(
    IN p_idCliente INT,
    IN p_idOrigen INT,
    IN p_idDestino INT,
    IN p_peso DECIMAL(10,2),
    IN p_alto DECIMAL(10,2),
    IN p_ancho DECIMAL(10,2),
    IN p_largo DECIMAL(10,2),
    IN p_distancia DECIMAL(10,2),
    IN p_modalidad VARCHAR(20)
)
BEGIN

    DECLARE v_idPaquete INT;
    DECLARE v_idEnvio INT;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;


    -- Validar cliente

    IF NOT EXISTS (
        SELECT 1
        FROM Cliente
        WHERE idCliente = p_idCliente
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El cliente no existe';

    END IF;


    -- Validar dirección de origen

    IF NOT EXISTS (
        SELECT 1
        FROM Direccion
        WHERE idDireccion = p_idOrigen
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La dirección de origen no existe';

    END IF;


    -- Validar dirección de destino

    IF NOT EXISTS (
        SELECT 1
        FROM Direccion
        WHERE idDireccion = p_idDestino
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La dirección de destino no existe';

    END IF;


    -- Validar paquete

    IF p_peso <= 0
       OR p_alto <= 0
       OR p_ancho <= 0
       OR p_largo <= 0 THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Las dimensiones y el peso deben ser mayores que cero';

    END IF;


    -- Validar distancia

    IF p_distancia <= 0 THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'La distancia debe ser mayor que cero';

    END IF;


    -- Validar modalidad

    IF p_modalidad NOT IN
    (
        'ESTANDAR',
        'EXPRESS',
        'PRIORITARIO'
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Modalidad de envío inválida';

    END IF;


    -- Crear paquete

    INSERT INTO Paquete
    (
        peso,
        alto,
        ancho,
        largo
    )
    VALUES
    (
        p_peso,
        p_alto,
        p_ancho,
        p_largo
    );


    SET v_idPaquete = LAST_INSERT_ID();


    -- Crear envío

    INSERT INTO Envio
    (
        idCliente,
        idPaquete,
        idOrigen,
        idDestino,
        distancia,
        modalidad,
        estado
    )
    VALUES
    (
        p_idCliente,
        v_idPaquete,
        p_idOrigen,
        p_idDestino,
        p_distancia,
        p_modalidad,
        'PENDIENTE'
    );


    SET v_idEnvio = LAST_INSERT_ID();


    -- Registrar estado inicial

    INSERT INTO HistorialEstado
    (
        idEnvio,
        estado
    )
    VALUES
    (
        v_idEnvio,
        'PENDIENTE'
    );


    COMMIT;


    SELECT v_idEnvio AS idEnvio;

END$$



-- CAMBIAR ESTADO DEL ENVio

CREATE PROCEDURE cambiarEstadoEnvio(
    IN p_idEnvio INT,
    IN p_nuevoEstado VARCHAR(50)
)
BEGIN

    DECLARE v_estadoActual VARCHAR(50);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;


    -- Obtener estado actual

    SELECT estado
    INTO v_estadoActual
    FROM Envio
    WHERE idEnvio = p_idEnvio;


    -- Verificar que el envío exista

    IF v_estadoActual IS NULL THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El envío no existe';

    END IF;


    -- Validar nuevo estado

    IF p_nuevoEstado NOT IN
    (
        'PENDIENTE',
        'EN_PROCESO',
        'ENTREGADO',
        'CANCELADO'
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Estado inválido';

    END IF;


    -- Un envío entregado no puede cambiar

    IF v_estadoActual = 'ENTREGADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Un envío entregado no puede cambiar de estado';

    END IF;


    -- Un envío cancelado no puede cambiar

    IF v_estadoActual = 'CANCELADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Un envío cancelado no puede cambiar de estado';

    END IF;


    -- Validar transición 

    IF v_estadoActual = 'PENDIENTE'
       AND p_nuevoEstado NOT IN
       (
           'EN_PROCESO',
           'CANCELADO'
       ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Transición de estado no permitida';

    END IF;


    -- Validar transición 

    IF v_estadoActual = 'EN_PROCESO'
       AND p_nuevoEstado NOT IN
       (
           'ENTREGADO',
           'CANCELADO'
       ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Transición de estado no permitida';

    END IF;


    -- Actualizar estado

    UPDATE Envio
    SET estado = p_nuevoEstado
    WHERE idEnvio = p_idEnvio;


    -- Registrar en historial

    INSERT INTO HistorialEstado
    (
        idEnvio,
        estado
    )
    VALUES
    (
        p_idEnvio,
        p_nuevoEstado
    );


    COMMIT;

END$$

-- CANCELAR ENVÍO

CREATE PROCEDURE cancelarEnvio(
    IN p_idEnvio INT
)
BEGIN

    DECLARE v_estadoActual VARCHAR(50);

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
    END;

    START TRANSACTION;


    

    SELECT estado
    INTO v_estadoActual
    FROM Envio
    WHERE idEnvio = p_idEnvio;


   

    IF v_estadoActual IS NULL THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El envío no existe';

    END IF;


   

    IF v_estadoActual = 'ENTREGADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'No se puede cancelar un envío entregado';

    END IF;



    IF v_estadoActual = 'CANCELADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El envío ya está cancelado';

    END IF;


    -- Cambiar estado

    UPDATE Envio
    SET estado = 'CANCELADO'
    WHERE idEnvio = p_idEnvio;


 

    INSERT INTO HistorialEstado
    (
        idEnvio,
        estado
    )
    VALUES
    (
        p_idEnvio,
        'CANCELADO'
    );


    COMMIT;

END$$


DELIMITER ;