-- Active: 1750775323683@@127.0.0.1@3306@5to_marketweight
Use 5to_MarketWeight

CALL AltaCriptoMoneda(100.50, 100, 'Bitcoin');
CALL AltaCriptoMoneda(50.25, 100, 'Ethereum');
CALL AltaCriptoMoneda(75.00, 100, 'Ripple');
CALL AltaCriptoMoneda(20.75, 100, 'Litecoin');
CALL AltaCriptoMoneda(30.10, 100, 'Cardano');
CALL AltaCriptoMoneda(60.80, 100, 'Polkadot');
CALL AltaCriptoMoneda(90.00, 100, 'Chainlink');
CALL AltaCriptoMoneda(110.15, 100, 'Stellar');
CALL AltaCriptoMoneda(40.60, 100, 'Dogecoin');
CALL AltaCriptoMoneda(70.85, 100, 'Tron');
CALL AltaUsuario('Ana', 'Garcia', 'ana.garcia@example.com', 'pass1234', FALSE);
CALL AltaUsuario('Luis', 'Martinez', 'luis.martinez@example.com', '1234abcd', FALSE);
CALL AltaUsuario('Marta', 'Fernandez', 'marta.fernandez@example.com', 'abcd1234', FALSE);
CALL AltaUsuario('Carlos', 'Gomez', 'carlos.gomez@example.com', 'qwerty12', FALSE);
CALL AltaUsuario('Laura', 'Rodriguez', 'laura.rodriguez@example.com', 'password', FALSE);
CALL AltaUsuario('Pedro', 'Lopez', 'pedro.lopez@example.com', 'abcd1234', FALSE);
CALL AltaUsuario('Sofia', 'Hernandez', 'sofia.hernandez@example.com', '12345678',FALSE);
CALL AltaUsuario('Daniel', 'Perez', 'daniel.perez@example.com', '1q2w3e4r', FALSE);
CALL AltaUsuario('Maria', 'Torres', 'maria.torres@example.com', 'letmein1', FALSE);
CALL AltaUsuario('Javier', 'Ramirez', 'javier.ramirez@example.com', 'welcome1', FALSE);
CALL AltaUsuario('Admin', 'Sistema', 'admin@marketweight.com', 'admin123', TRUE);

CALL IngresarDinero(2, 0);
CALL IngresarDinero(3, 10000);
CALL IngresarDinero(2, 10000);

CALL ComprarMoneda (2, 3, 2);
CALL Transferencia (2, 0.5, 2, 3);