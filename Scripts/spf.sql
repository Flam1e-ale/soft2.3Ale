USE db_envios;

DROP PROCEDURE IF EXISTS altaEnvioCompleto;
DROP PROCEDURE IF EXISTS cambiarEstadoEnvio;
DROP PROCEDURE IF EXISTS cancelarEnvio;

DELIMITER $$



-- 1. REGISTRAR ENVÍO COMPLETO


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


    
    -- MANEJO DE ERRORES
    

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;


    START TRANSACTION;


    
    -- VALIDAR CLIENTE
    

    IF NOT EXISTS (
        SELECT 1
        FROM Cliente
        WHERE IdCliente = p_idCliente
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El cliente no existe';

    END IF;


    
    -- VALIDAR DIRECCION ORIGEN
    

    IF NOT EXISTS (
        SELECT 1
        FROM Direccion
        WHERE IdDireccion = p_idOrigen
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La dirección de origen no existe';

    END IF;


    
    -- VALIDAR DIRECCION DESTINO
    

    IF NOT EXISTS (
        SELECT 1
        FROM Direccion
        WHERE IdDireccion = p_idDestino
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La dirección de destino no existe';

    END IF;


    
    -- VALIDAR PAQUETE
    

    IF p_peso <= 0
       OR p_alto <= 0
       OR p_ancho <= 0
       OR p_largo <= 0 THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El peso y las dimensiones deben ser mayores que cero';

    END IF;


    
    -- VALIDAR DISTANCIA
    

    IF p_distancia <= 0 THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'La distancia debe ser mayor que cero';

    END IF;


    
    -- VALIDAR MODALIDAD
    

    IF NOT EXISTS (
        SELECT 1
        FROM Modalidad
        WHERE IdModalidad = p_idModalidad
    ) THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'La modalidad no existe';

    END IF;


    
    -- VALIDAR COSTO
    

    IF p_costo < 0 THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El costo no puede ser negativo';

    END IF;


    
    -- VALIDAR TIEMPO
    

    IF p_tiempoEstimado <= 0 THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El tiempo estimado debe ser mayor que cero';

    END IF;


    
    -- CREAR PAQUETE
    

    INSERT INTO Paquete
    (
        Peso,
        Alto,
        Ancho,
        Largo
    )
    VALUES
    (
        p_peso,
        p_alto,
        p_ancho,
        p_largo
    );


    SET v_idPaquete = LAST_INSERT_ID();


    
    -- CREAR ENVIO
    

    INSERT INTO Envio
    (
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
    VALUES
    (
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


    
    -- REGISTRAR ESTADO INICIAL
    

    INSERT INTO HistorialEstado
    (
        IdEnvio,
        EstadoAnterior,
        EstadoNuevo
    )
    VALUES
    (
        v_idEnvio,
        NULL,
        'PENDIENTE'
    );


    COMMIT;


    -- DEVOLVER ID DEL ENVIO

    SELECT v_idEnvio AS IdEnvio;

END$$



-- 2. CAMBIAR ESTADO DEL ENVIO


CREATE PROCEDURE cambiarEstadoEnvio(
    IN p_idEnvio INT,
    IN p_nuevoEstado VARCHAR(45)
)
BEGIN

    DECLARE v_estadoActual VARCHAR(45);


    
    -- MANEJO DE ERRORES
    

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;


    START TRANSACTION;


    
    -- OBTENER ESTADO ACTUAL
    

    SELECT Estado
    INTO v_estadoActual
    FROM Envio
    WHERE IdEnvio = p_idEnvio;


    
    -- VERIFICAR EXISTENCIA
    

    IF v_estadoActual IS NULL THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El envío no existe';

    END IF;


    
    -- VALIDAR ESTADO
    

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


    
    -- ENVIO ENTREGADO
    

    IF v_estadoActual = 'ENTREGADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Un envío entregado no puede cambiar de estado';

    END IF;


    
    -- ENVIO CANCELADO
    

    IF v_estadoActual = 'CANCELADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'Un envío cancelado no puede cambiar de estado';

    END IF;


    
    -- TRANSICION DESDE PENDIENTE
    

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


    
    -- TRANSICION DESDE EN_PROCESO
    

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


    
    -- ACTUALIZAR ENVIO
    

    UPDATE Envio
    SET Estado = p_nuevoEstado
    WHERE IdEnvio = p_idEnvio;


    
    -- GUARDAR HISTORIAL
    

    INSERT INTO HistorialEstado
    (
        IdEnvio,
        EstadoAnterior,
        EstadoNuevo
    )
    VALUES
    (
        p_idEnvio,
        v_estadoActual,
        p_nuevoEstado
    );


    COMMIT;

END$$



-- 3. CANCELAR ENVIO


CREATE PROCEDURE cancelarEnvio(
    IN p_idEnvio INT
)
BEGIN

    DECLARE v_estadoActual VARCHAR(45);


    
    -- MANEJO DE ERRORES
    

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;


    START TRANSACTION;


    
    -- OBTENER ESTADO ACTUAL
    

    SELECT Estado
    INTO v_estadoActual
    FROM Envio
    WHERE IdEnvio = p_idEnvio;


    
    -- VERIFICAR EXISTENCIA
    

    IF v_estadoActual IS NULL THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El envío no existe';

    END IF;


    
    -- VERIFICAR SI YA FUE ENTREGADO
    

    IF v_estadoActual = 'ENTREGADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'No se puede cancelar un envío entregado';

    END IF;


    
    -- VERIFICAR SI YA ESTA CANCELADO
    

    IF v_estadoActual = 'CANCELADO' THEN

        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT =
        'El envío ya está cancelado';

    END IF;


    
    -- CAMBIAR ESTADO
    

    UPDATE Envio
    SET Estado = 'CANCELADO'
    WHERE IdEnvio = p_idEnvio;


    
    -- REGISTRAR HISTORIAL
    

    INSERT INTO HistorialEstado
    (
        IdEnvio,
        EstadoAnterior,
        EstadoNuevo
    )
    VALUES
    (
        p_idEnvio,
        v_estadoActual,
        'CANCELADO'
    );


    COMMIT;

END$$


DELIMITER ;