-- Script para agregar campo esAdmin a la tabla Usuario y actualizar procedimientos
USE 5to_MarketWeight;

-- Agregar columna esAdmin a la tabla Usuario si no existe
ALTER TABLE Usuario ADD COLUMN IF NOT EXISTS esAdmin BOOLEAN DEFAULT FALSE;

-- Actualizar el procedimiento AltaUsuario para incluir esAdmin
DELIMITER $$
DROP PROCEDURE IF EXISTS AltaUsuario $$
CREATE PROCEDURE `AltaUsuario`(xnombre VARCHAR(45), xapellido VARCHAR(45), xemail VARCHAR(45), xpass CHAR(64), xesAdmin BOOLEAN)
BEGIN
    INSERT INTO `Usuario` (nombre, apellido, email, pass, saldo, esAdmin)
        VALUES(xnombre, xapellido, xemail, xpass, 0.0, xesAdmin);
END $$

-- Crear un procedimiento para actualizar el rol de administrador
DROP PROCEDURE IF EXISTS ActualizarRolAdmin $$
CREATE PROCEDURE `ActualizarRolAdmin`(xidUsuario INT UNSIGNED, xesAdmin BOOLEAN)
BEGIN
    UPDATE Usuario 
    SET esAdmin = xesAdmin
    WHERE idUsuario = xidUsuario;
END $$
DELIMITER ;
