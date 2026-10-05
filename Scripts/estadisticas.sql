USE db_envios;

DELIMITER $$


-- 
-- 1. RESUMEN DE ENVÍOS POR PERÍODO, MODALIDAD Y ESTADO
-- 

DROP PROCEDURE IF EXISTS obtenerResumenEnvios $$

CREATE PROCEDURE obtenerResumenEnvios(
    IN p_fechaInicio DATETIME,
    IN p_fechaFin DATETIME
)
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    -- Validar fechas
    IF p_fechaInicio IS NULL OR p_fechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Las fechas no pueden ser nulas';
    END IF;

    IF p_fechaInicio > p_fechaFin THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La fecha de inicio no puede ser mayor a la fecha de fin';
    END IF;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        e.Estado,
        COUNT(e.IdEnvio) AS CantidadEnvios
    FROM Envio e
    INNER JOIN Modalidad m
        ON e.IdModalidad = m.IdModalidad
    WHERE e.FechaCreacion BETWEEN p_fechaInicio AND p_fechaFin
    GROUP BY
        m.IdModalidad,
        m.Nombre,
        e.Estado
    ORDER BY
        m.Nombre,
        e.Estado;

    COMMIT;

END $$



-- 
-- 2. COSTOS ACUMULADOS Y PROMEDIO POR MODALIDAD
-- 

DROP PROCEDURE IF EXISTS obtenerCostosPorModalidad $$

CREATE PROCEDURE obtenerCostosPorModalidad(
    IN p_fechaInicio DATETIME,
    IN p_fechaFin DATETIME
)
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    -- Validar fechas
    IF p_fechaInicio IS NULL OR p_fechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Las fechas no pueden ser nulas';
    END IF;

    IF p_fechaInicio > p_fechaFin THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La fecha de inicio no puede ser mayor a la fecha de fin';
    END IF;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        COUNT(e.IdEnvio) AS CantidadEnvios,
        SUM(e.Costo) AS CostoTotal,
        AVG(e.Costo) AS CostoPromedio
    FROM Envio e
    INNER JOIN Modalidad m
        ON e.IdModalidad = m.IdModalidad
    WHERE e.FechaCreacion BETWEEN p_fechaInicio AND p_fechaFin
    GROUP BY
        m.IdModalidad,
        m.Nombre
    ORDER BY
        m.Nombre;

    COMMIT;

END $$



-- 
-- 3. ENTREGADOS, CANCELADOS Y PENDIENTES POR PERÍODO
-- 

DROP PROCEDURE IF EXISTS obtenerEstadosPorPeriodo $$

CREATE PROCEDURE obtenerEstadosPorPeriodo(
    IN p_fechaInicio DATETIME,
    IN p_fechaFin DATETIME
)
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    -- Validar fechas
    IF p_fechaInicio IS NULL OR p_fechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Las fechas no pueden ser nulas';
    END IF;

    IF p_fechaInicio > p_fechaFin THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La fecha de inicio no puede ser mayor a la fecha de fin';
    END IF;

    START TRANSACTION;

    SELECT
        e.Estado,
        COUNT(e.IdEnvio) AS Cantidad
    FROM Envio e
    WHERE e.FechaCreacion BETWEEN p_fechaInicio AND p_fechaFin
    GROUP BY e.Estado
    ORDER BY e.Estado;

    COMMIT;

END $$



-- 
-- 4. TIEMPO PROMEDIO POR MODALIDAD
-- 

DROP PROCEDURE IF EXISTS obtenerTiempoPromedio $$

CREATE PROCEDURE obtenerTiempoPromedio(
    IN p_fechaInicio DATETIME,
    IN p_fechaFin DATETIME
)
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    -- Validar fechas
    IF p_fechaInicio IS NULL OR p_fechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Las fechas no pueden ser nulas';
    END IF;

    IF p_fechaInicio > p_fechaFin THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La fecha de inicio no puede ser mayor a la fecha de fin';
    END IF;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        COUNT(e.IdEnvio) AS CantidadEnvios,
        AVG(e.TiempoEstimado) AS TiempoPromedio
    FROM Envio e
    INNER JOIN Modalidad m
        ON e.IdModalidad = m.IdModalidad
    WHERE e.FechaCreacion BETWEEN p_fechaInicio AND p_fechaFin
    GROUP BY
        m.IdModalidad,
        m.Nombre
    ORDER BY
        m.Nombre;

    COMMIT;

END $$



-- 
-- 5. FACTURACIÓN TOTAL POR MODALIDAD Y PERÍODO
-- 

DROP PROCEDURE IF EXISTS obtenerFacturacionPorModalidad $$

CREATE PROCEDURE obtenerFacturacionPorModalidad(
    IN p_fechaInicio DATETIME,
    IN p_fechaFin DATETIME
)
BEGIN

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;

    -- Validar fechas
    IF p_fechaInicio IS NULL OR p_fechaFin IS NULL THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'Las fechas no pueden ser nulas';
    END IF;

    IF p_fechaInicio > p_fechaFin THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'La fecha de inicio no puede ser mayor a la fecha de fin';
    END IF;

    START TRANSACTION;

    SELECT
        m.Nombre AS Modalidad,
        COUNT(e.IdEnvio) AS CantidadEnvios,
        SUM(e.Costo) AS FacturacionTotal
    FROM Envio e
    INNER JOIN Modalidad m
        ON e.IdModalidad = m.IdModalidad
    WHERE e.FechaCreacion BETWEEN p_fechaInicio AND p_fechaFin
    GROUP BY
        m.IdModalidad,
        m.Nombre
    ORDER BY
        m.Nombre;

    COMMIT;

END $$


DELIMITER ;