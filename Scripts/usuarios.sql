-- root localhost skibidisigma esto es clave
-- USUARIOS DEL SISTEMA                                            Invitado
-- en caso de emergencia dotnet user-secrets set "ConnectionStrings:Admin" "Server=localhost;Port=3306;Database=db_envios;User ID=root;Password=skibidisigma;"
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