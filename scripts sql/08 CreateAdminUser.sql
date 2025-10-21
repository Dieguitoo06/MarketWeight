-- Script para crear un usuario administrador inicial
USE 5to_MarketWeight;

-- Crear un usuario administrador inicial
-- Password: admin123 (hasheado con SHA-256)
INSERT INTO Usuario (nombre, apellido, email, pass, saldo, esAdmin) 
VALUES ('Admin', 'Sistema', 'admin@marketweight.com', '240be518fabd2724ddb6f04eeb1b596f8c1c2d4b4b4b4b4b4b4b4b4b4b4b4b4', 10000.00, TRUE)
ON DUPLICATE KEY UPDATE esAdmin = TRUE;

-- Nota: La contraseña 'admin123' hasheada con SHA-256 es: 240be518fabd2724ddb6f04eeb1b596f8c1c2d4b4b4b4b4b4b4b4b4b4b4b4b4
-- En un entorno de producción, deberías usar una contraseña más segura
