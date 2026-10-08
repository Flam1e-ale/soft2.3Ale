-- 
-- USUARIOS DEL SISTEMA
-- 

-- Crear usuario administrador
CREATE USER IF NOT EXISTS 'administrador'@'localhost'IDENTIFIED BY 'Admin_Envios_2026';


-- Crear usuario desarrollo
CREATE USER IF NOT EXISTS 'desarrollo'@'localhost'IDENTIFIED BY 'Desarrollo_Envios_2026';


-- 
-- PERMISOS DEL ADMINISTRADOR
-- 

GRANT ALL PRIVILEGES
ON *.*
TO 'administrador'@'localhost'
WITH GRANT OPTION;


-- 
-- PERMISOS DEL USUARIO DESARROLLO
-- 

GRANT SELECT, INSERT, UPDATE, DELETE, EXECUTE
ON db_envios.*
TO 'desarrollo'@'localhost';


-- 
-- ACTUALIZAR PRIVILEGIOS
-- 

FLUSH PRIVILEGES;