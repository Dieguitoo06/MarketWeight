// ...existing code...
# E.T. Nº12 — MarketWeight
**Administración y Gestión de Bases de Datos — 5° 8°**

Autores:
- Diego Quintero — [Dieguitoo06](https://github.com/Dieguitoo06)  

## Descripción breve
MarketWeight permite visualizar oferta/demanda de criptomonedas con una interfaz web MVC y persistencia en MySQL. Incluye scripts para crear la BD, procedures, triggers (hash SHA‑256 de contraseñas) y pruebas automatizadas.

## Carátula
- Asignatura: Programacion sobre redes
- Curso: 6° 8°  
- Proyecto: MarketWeight  
- Entrega: Trabajo práctico final

## Tecnologías
- C# / ASP.NET MVC (.NET 8)  
- MySQL 8.0  
- MySqlConnector & Dapper (Ado)  
- VS Code

## Estructura clave del repositorio
- [README.md](README.md) (este archivo)  
- [LICENSE](LICENSE)  
- Scripts de base de datos:
  - [scripts sql/Install.sql](scripts sql/Install.sql) — orquesta la instalación
  - [scripts sql/01 MarketWeight-ddl.sql](scripts sql/01 MarketWeight-ddl.sql) — DDL (tablas)
  - [scripts sql/03 Procedures.sql](scripts sql/03 Procedures.sql) — procedimientos almacenados
  - [scripts sql/05 Triggers.sql](scripts sql/05 Triggers.sql) — triggers (hash de pass)
- Backend:
  - `{MarketWeight}.mvc/Program.cs` — configuración de servicios y seguridad ([ver archivo]({MarketWeight}.mvc/Program.cs)) — cookie auth HttpOnly configurada
  - `src/MarketWeight.Ado.Dapper.Test/TestBase.cs` — base para pruebas unitarias y conexión ([`MarketWeight.Ado.Dapper.Test.TestBase`](src/MarketWeight.Ado.Dapper.Test/TestBase.cs))
- Proyecto de pruebas: `src/MarketWeight.Ado.Dapper.Test` (ejecutar pruebas con dotnet)

## Implementación — puntos importantes
- Persistencia: MySQL con tablas normalizadas (Usuario, Moneda, UsuarioMoneda, Historial). Scripts en `scripts sql/` gestionan creación, procedures y triggers.
- Seguridad:
  - Las contraseñas se hashean en la BD vía trigger (SHA‑256).
  - Cookie de autenticación configurada con HttpOnly en [`{MarketWeight}.mvc/Program.cs`]( {MarketWeight}.mvc/Program.cs ).
  - Validaciones en DTOs y ModelState en controladores.
- Repositorios: patrones sencillos ADO/Dapper para ejecutar procedures y consultas parametrizadas — evita SQL injection.
- Tests: proyecto de pruebas con conexión a BD configurable en `appSettings.json` dentro de `src/MarketWeight.Ado.Dapper.Test` — clase base [`MarketWeight.Ado.Dapper.Test.TestBase`](src/MarketWeight.Ado.Dapper.Test/TestBase.cs).

## Cómo ejecutar
1. Clonar el repo:
```sh
git clone  https://github.com/Dieguitoo06/MarketWeight
```
2. Crear la base de datos (desde la carpeta `scripts sql`):
```sh
mysql -u tuUsuario -p
source Install.sql
```
(usa [scripts sql/Install.sql](scripts sql/Install.sql) y [scripts sql/01 MarketWeight-ddl.sql](scripts sql/01 MarketWeight-ddl.sql))

3. Configurar cadena de conexión:
- Editar `{MarketWeight}.mvc/appsettings.Development.json` o la variable de entorno con la conexión MySQL usada por la aplicación.

4. Ejecutar la aplicación:
```sh
dotnet run --project {MarketWeight}.mvc
```

5. Ejecutar pruebas:
```sh
dotnet test src/MarketWeight.Ado.Dapper.Test -v d
```

## Notas para la presentación
- Explicar flujo de creación de usuario: formulario → controlador → repositorio → procedure SQL → trigger hash. (Ver [GUIA_RESPUESTAS.md](GUIA_RESPUESTAS.md) para guión).
- Resaltar: uso de triggers para hash, procedures para transacciones y configuración de seguridad de cookies en [`{MarketWeight}.mvc/Program.cs`]( {MarketWeight}.mvc/Program.cs ).

## Recursos y referencias
- Scripts de instalación: [scripts sql/Install.sql](scripts sql/Install.sql)  
- DDL: [scripts sql/01 MarketWeight-ddl.sql](scripts sql/01 MarketWeight-ddl.sql)  
- Clase de pruebas: [`MarketWeight.Ado.Dapper.Test.TestBase`](src/MarketWeight.Ado.Dapper.Test/TestBase.cs)  
- Program (configuración): [{MarketWeight}.mvc/Program.cs]({MarketWeight}.mvc/Program.cs)  
- Licencia: [LICENSE](LICENSE)

---

Mantener este README actualizado al realizar cambios en la DB o en la configuración de conexión.
// ...existing code...