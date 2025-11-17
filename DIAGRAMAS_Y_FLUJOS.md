# Diagramas y Flujos - MarketWeight

> Representaciones visuales de los procesos clave del proyecto

---

## 1. FLUJO DE REGISTRO (Crear Usuario)

```
┌─────────────────────────────────────────────────────────────┐
│  USUARIO NUEVO                                              │
└────────────────┬────────────────────────────────────────────┘
                 │
                 ▼
     ┌───────────────────────┐
     │  /Account/Register    │
     │  GET Request          │
     └───────────┬───────────┘
                 │
                 ▼
     ┌─────────────────────────────┐
     │  Mostrar Formulario         │
     │  Campos:                    │
     │  - Nombre                   │
     │  - Apellido                 │
     │  - Email                    │
     │  - Contraseña               │
     └────────────┬────────────────┘
                  │
                  ▼
     ┌──────────────────────────┐
     │ Usuario completa datos   │
     │ y hace POST              │
     └───────────┬──────────────┘
                 │
                 ▼
    ┌────────────────────────────────┐
    │ AccountController.Register()   │
    │ POST Request                   │
    └──────────┬─────────────────────┘
               │
               ▼
    ╔══════════════════════════════════╗
    ║  VALIDACIÓN 1: Campos nulos      ║
    ║  if (string.IsNullOrWhiteSpace)  ║
    ║  ✓ PASA → continúa              ║
    ║  ✗ FALLA → Mostrar error        ║
    ╚══════════════════════════════════╝
               │
               ▼
    ╔══════════════════════════════════╗
    ║  VALIDACIÓN 2: Email único       ║
    ║  ObtenerPorEmail(email)          ║
    ║  ✓ No existe → continúa          ║
    ║  ✗ Existe → Error "Email usado"  ║
    ╚══════════════════════════════════╝
               │
               ▼
    ╔════════════════════════════════════╗
    ║  Crear objeto Usuario              ║
    ║  {                                 ║
    ║    Nombre = nombre.Trim()          ║
    ║    Apellido = apellido.Trim()      ║
    ║    Email = email.ToLower()         ║
    ║    Password = password (plain)     ║
    ║    Saldo = 0                       ║
    ║    EsAdmin = false                 ║
    ║  }                                 ║
    ╚════════════════┬═══════════════════╝
                     │
                     ▼
    ┌──────────────────────────────────┐
    │ RepoUsuario.Alta(usuario)        │
    │ Llamar repositorio               │
    └────────────┬─────────────────────┘
                 │
                 ▼
    ┌──────────────────────────────────┐
    │ MySQL - Procedimiento            │
    │ CALL AltaUsuario(...)            │
    └────────────┬─────────────────────┘
                 │
                 ▼
    ╔════════════════════════════════════╗
    ║  TRIGGER: MarketWeight_Usuario_   ║
    ║  Insert ejecutado                  ║
    ║                                    ║
    ║  UPDATE Usuario SET                ║
    ║  Password = SHA2(Password, 256)    ║
    ║                                    ║
    ║  El hash SHA-256 se calcula       ║
    ║  automáticamente en la BD          ║
    ╚════════════════┬═══════════════════╝
                     │
                     ▼
    ┌──────────────────────────────────┐
    │ Usuario insertado en BD           │
    │ ✓ ID generado                    │
    │ ✓ Password hasheado              │
    └────────────┬─────────────────────┘
                 │
                 ▼
    ┌──────────────────────────────────┐
    │ TempData["Message"] =             │
    │ "Cuenta creada exitosamente"      │
    └────────────┬─────────────────────┘
                 │
                 ▼
    ┌──────────────────────────────────┐
    │ Redirect → /Account/Login        │
    │ Usuario redirigido para           │
    │ autenticarse                      │
    └──────────────────────────────────┘
```

---

## 2. FLUJO DE RESPONSIVE (Mobile-First)

```
                    USUARIO ACCEDE AL SITIO
                            │
                            ▼
                 ┌──────────────────────┐
                 │  Navegador carga     │
                 │  Site.css            │
                 │  Responsive.css      │
                 └──────────┬───────────┘
                            │
                ┌───────────┴───────────┐
                │                       │
                ▼                       ▼
        ┌─────────────────┐     ┌──────────────────┐
        │ Detecta width   │     │ Evalúa media     │
        │ del navegador   │     │ queries          │
        │                 │     │                  │
        │ Ej: 375px       │     │ 375px < 576px    │
        │ (iPhone)        │     │ ✓ COINCIDE       │
        └────────┬────────┘     └────────┬─────────┘
                 │                       │
                 └───────────┬───────────┘
                             │
                             ▼
        ╔═══════════════════════════════════════════╗
        ║  MEDIA QUERY (@media max-width: 575.98px)║
        ║                                           ║
        ║  Aplicar estilos MOBILE:                  ║
        ║  - html { font-size: 13px }               ║
        ║  - .navbar { padding: 0.5rem 0 }          ║
        ║  - .card { padding: 12px }                ║
        ║  - .table { font-size: 0.8rem }           ║
        ║  - Flexbox column layout                  ║
        ╚═══════════════════════════════════════════╝
                             │
                             ▼
                ┌────────────────────────┐
                │  Resultado: Layout     │
                │  optimizado para móvil │
                │                        │
                │  ✓ Texto legible      │
                │  ✓ Botones clickeables│
                │  ✓ Sin scroll horiz.  │
                │  ✓ Navbar colapsable  │
                └────────────────────────┘

===============================================

        USUARIO REDIMENSIONA A TABLET (768px)
                            │
                            ▼
                 ┌──────────────────────┐
                 │  Navegador evalúa    │
                 │  media queries de    │
                 │  nuevo               │
                 └──────────┬───────────┘
                            │
                            ▼
        ╔═══════════════════════════════════════════╗
        ║  MEDIA QUERY (@media min-width: 768px)   ║
        ║                                           ║
        ║  Cambiar estilos a TABLET:                ║
        ║  - html { font-size: 16px }               ║
        ║  - .card { padding: 24px }                ║
        ║  - .mw-shell { max-width: 1200px }        ║
        ║  - Grid layout 2-3 columnas                ║
        ║  - Navbar expandido                       ║
        ╚═══════════════════════════════════════════╝
                             │
                             ▼
                ┌────────────────────────┐
                │  Resultado: Layout     │
                │  optimizado para       │
                │  tablet                │
                │                        │
                │  ✓ Más espacio        │
                │  ✓ Múltiples columnas │
                │  ✓ Imágenes grandes   │
                └────────────────────────┘

===============================================

        USUARIO REDIMENSIONA A DESKTOP (992px+)
                            │
                            ▼
                 ┌──────────────────────┐
                 │  Navegador evalúa    │
                 │  media queries de    │
                 │  nuevo               │
                 └──────────┬───────────┘
                            │
                            ▼
        ╔═══════════════════════════════════════════╗
        ║  MEDIA QUERY (@media min-width: 992px)   ║
        ║                                           ║
        ║  Aplicar estilos DESKTOP:                 ║
        ║  - .container { padding: 1.5rem }         ║
        ║  - h1 { font-size: 2rem }                 ║
        ║  - Layout completo 4 columnas             ║
        ║  - Efectos hover avanzados                ║
        ╚═══════════════════════════════════════════╝
                             │
                             ▼
                ┌────────────────────────┐
                │  Resultado: Layout     │
                │  optimizado para       │
                │  desktop               │
                │                        │
                │  ✓ Full experience    │
                │  ✓ Máximo espacio     │
                │  ✓ Todas las features │
                └────────────────────────┘
```

---

## 3. FLUJO DE AUTENTICACIÓN (Cookies)

```
PASO 1: LOGIN
┌────────────────────────────────┐
│ Usuario accede a /Account/Login│
└────────────┬───────────────────┘
             │
             ▼
┌─────────────────────────────┐
│ Ingresa email y contraseña  │
│ Hace POST                   │
└────────────┬────────────────┘
             │
             ▼
┌────────────────────────────────┐
│ AccountController.Login()      │
│ POST                           │
└────────────┬───────────────────┘
             │
             ▼
╔════════════════════════════════════════╗
║  Buscar usuario por email              ║
║  ObtenerPorEmail(email)                ║
║  Base de Datos ↔ Repositorio           ║
║                                        ║
║  ✓ Encontrado → continúa               ║
║  ✗ No existe → Error "Usuario no found"║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  Hashear contraseña ingresada          ║
║  HashPassword(password)                ║
║  SHA256.ComputeHash(Encoding.UTF8      ║
║    .GetBytes(password.Trim()))         ║
║                                        ║
║  Ej: "password123" →                   ║
║  "48ca3c3f0da0eace72c5476e05..."      ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  Comparar hashes                       ║
║  Hash_Calculado == Hash_BD             ║
║                                        ║
║  ✓ Coinciden → Autenticado             ║
║  ✗ No coinciden → Error "Contraseña"   ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  CREAR CLAIMS (información usuario)    ║
║                                        ║
║  var claims = new List<Claim>          ║
║  {                                     ║
║    new Claim(ClaimTypes.               ║
║      NameIdentifier, "15"),            ║
║    new Claim(ClaimTypes.Name,          ║
║      "Juan Pérez"),                    ║
║    new Claim(ClaimTypes.Email,         ║
║      "juan@email.com"),                ║
║    new Claim(ClaimTypes.Role,          ║
║      "Admin")  ← Si es admin           ║
║  }                                     ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  CREAR IDENTIDAD                       ║
║  var identity = new ClaimsIdentity(    ║
║    claims,                             ║
║    CookieAuthenticationDefaults        ║
║      .AuthenticationScheme)            ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  FIRMAR AL USUARIO                     ║
║  await HttpContext.SignInAsync(        ║
║    CookieAuthenticationDefaults        ║
║      .AuthenticationScheme,            ║
║    new ClaimsPrincipal(identity),      ║
║    authProperties: IsPersistent=true   ║
║  )                                     ║
║                                        ║
║  → Crear COOKIE                        ║
║  → Enviar al navegador                 ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
┌──────────────────────────────────┐
│ RESPUESTA HTTP                   │
│ Set-Cookie: MarketWeight.Auth=   │
│   eyJhbGci... [cookie cifrada]   │
│ Path=/                           │
│ HttpOnly                         │
│ SameSite=Lax                     │
│ Max-Age=1800 (30 min)            │
└──────────┬───────────────────────┘
           │
           ▼
┌──────────────────────────────────┐
│ Navegador almacena COOKIE        │
│                                  │
│ Cookie persistida:               │
│ MarketWeight.Auth = [encriptado] │
└──────────┬───────────────────────┘
           │
           ▼
┌──────────────────────────────────┐
│ Redirect → /Home/Index           │
│ Usuario autenticado              │
│ ✓ Acceso a rutas [Authorize]    │
└──────────────────────────────────┘


PASO 2: PETICIONES POSTERIORES
┌────────────────────────────────┐
│ Usuario hace petición a        │
│ /Usuarios/Details/5            │
└────────────┬───────────────────┘
             │
             ▼
┌──────────────────────────────────┐
│ Navegador ENVÍA COOKIE           │
│ automáticamente con cada request │
│                                  │
│ Cookie: MarketWeight.Auth=       │
│   eyJhbGci... [encriptado]       │
└────────────┬─────────────────────┘
             │
             ▼
╔════════════════════════════════════╗
║  MIDDLEWARE: Authentication       ║
║  (en app.UseAuthentication())      ║
║                                    ║
║  1. Recibe cookie                  ║
║  2. Desencripta                    ║
║  3. Valida firma                   ║
║  4. Extrae claims                  ║
║  5. Crea User principal            ║
╚════════════════┬═══════════════════╝
                 │
                 ▼
┌────────────────────────────────────┐
│ User.IsAuthenticated = true        │
│ User.FindFirst(ClaimTypes.         │
│   NameIdentifier) = "15"           │
│ User.IsInRole("Admin") = true/false│
└────────────┬───────────────────────┘
             │
             ▼
╔════════════════════════════════════╗
║  VERIFICAR AUTORIZACIÓN            ║
║  [Authorize(Roles = "Admin")]      ║
║                                    ║
║  ✓ IsAdmin? → Acceso permitido    ║
║  ✗ IsUser? → 403 Forbidden        ║
╚════════════════┬═══════════════════╝
                 │
                 ▼
┌────────────────────────────────────┐
│ Ejecutar acción del controlador    │
│ Retornar Vista                     │
└────────────────────────────────────┘


PASO 3: RENOVACIÓN (SlidingExpiration)
┌─────────────────────────────────┐
│ Cookie creada a las 10:00       │
│ Expira a las 10:30 (30 min)     │
└────────────┬────────────────────┘
             │
             ▼
┌─────────────────────────────────┐
│ Usuario activo a las 10:25      │
│ Hace una petición               │
└────────────┬────────────────────┘
             │
             ▼
╔═════════════════════════════════════╗
║  MIDDLEWARE: SlidingExpiration      ║
║  = true                             ║
║                                     ║
║  Cookie vieja:                      ║
║  Expira: 10:30                      ║
║                                     ║
║  → RENOVAR COOKIE                   ║
║                                     ║
║  Cookie nueva:                      ║
║  Expira: 10:55 (nuevos 30 min)     ║
╚═════════════════┬═══════════════════╝
                  │
                  ▼
┌──────────────────────────────────┐
│ Respuesta con NUEVA COOKIE       │
│ Set-Cookie: MarketWeight.Auth=   │
│   [encriptado con nuevo expiry]   │
└──────────────────────────────────┘


PASO 4: LOGOUT
┌────────────────────────────────┐
│ Usuario hace click en LOGOUT    │
│ POST /Account/Logout           │
└────────────┬───────────────────┘
             │
             ▼
┌────────────────────────────────┐
│ AccountController.Logout()     │
│ POST [Authorize]               │
└────────────┬───────────────────┘
             │
             ▼
╔════════════════════════════════════╗
║  await HttpContext.SignOutAsync(   ║
║    CookieAuthenticationDefaults    ║
║      .AuthenticationScheme)        ║
║                                    ║
║  → Eliminar COOKIE                 ║
║  → Limpiar autenticación           ║
╚════════════════┬═══════════════════╝
                 │
                 ▼
┌──────────────────────────────────┐
│ Respuesta HTTP                   │
│ Set-Cookie: MarketWeight.Auth=   │
│   ; Max-Age=0                    │
│   (cookie vacía con expiry=0)    │
└──────────┬───────────────────────┘
           │
           ▼
┌──────────────────────────────────┐
│ Navegador BORRA COOKIE           │
│ MarketWeight.Auth (eliminada)    │
└──────────┬───────────────────────┘
           │
           ▼
┌──────────────────────────────────┐
│ Redirect → /Account/Login       │
│ Usuario desautenticado           │
│ ✗ Sin acceso a [Authorize]      │
└──────────────────────────────────┘
```

---

## 4. FLUJO DE CREAR MONEDA

```
┌──────────────────────────────┐
│ Usuario Admin accede a       │
│ /Monedas                     │
└────────────┬────────────────┘
             │
             ▼
╔════════════════════════════════════╗
║  VERIFICAR AUTORIZACIÓN            ║
║  User.IsInRole("Admin")?           ║
║                                    ║
║  ✓ Es Admin →                      ║
║    Mostrar botón "Crear Moneda"   ║
║  ✗ Es Usuario →                    ║
║    Botón oculto/deshabilitado     ║
╚════════════════┬═══════════════════╝
                 │
                 ▼
        ┌────────────────────┐
        │ Click en           │
        │ "Crear Moneda"     │
        └────────┬───────────┘
                 │
                 ▼
        ┌────────────────────────┐
        │ GET /Monedas/Create    │
        └────────┬───────────────┘
                 │
                 ▼
    ╔══════════════════════════════════╗
    ║  MonedasController.Create()      ║
    ║  GET [Authorize(Roles = "Admin")]║
    ║                                  ║
    ║  [Autorización OK]               ║
    ╚═════════════┬════════════════════╝
                  │
                  ▼
        ┌────────────────────────┐
        │ Mostrar Formulario:    │
        │                        │
        │ Nombre: [input text]   │
        │ Precio: [input number] │
        │ Cantidad: [input numer]│
        │ Botones: Guardar/Cancel│
        └────────┬───────────────┘
                 │
                 ▼
        ┌────────────────────────┐
        │ Admin completa datos:  │
        │ - Nombre: "Bitcoin"    │
        │ - Precio: 45000.50     │
        │ - Cantidad: 100        │
        │ Hace click en GUARDAR  │
        └────────┬───────────────┘
                 │
                 ▼
    ┌────────────────────────────────┐
    │ POST /Monedas/Create           │
    │ Envía formulario               │
    └────────┬───────────────────────┘
             │
             ▼
╔════════════════════════════════════════╗
║  MonedasController.Create(MonedaDto)   ║
║  POST [Authorize(Roles = "Admin")]     ║
║  [ValidateAntiForgeryToken]            ║
║                                        ║
║  [Autorización OK]                     ║
║  [Token CSRF válido]                   ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  VALIDACIÓN - ModelState.IsValid?      ║
║                                        ║
║  Verificar DTO:                        ║
║  - [Required] Nombre ✓ ("Bitcoin")    ║
║  - [Required] Precio ✓ (45000.50)     ║
║  - [Required] Cantidad ✓ (100)        ║
║                                        ║
║  ✓ Todos válidos → Continuar           ║
║  ✗ Falta algo → Volver al formulario   ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
╔════════════════════════════════════════╗
║  Crear objeto Moneda:                  ║
║  var entity = new Moneda               ║
║  {                                     ║
║    Nombre = "Bitcoin",                 ║
║    Precio = 45000.50M,                 ║
║    Cantidad = 100M                     ║
║  }                                     ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
        ┌────────────────────────────┐
        │ RepoMoneda.Alta(entity)    │
        │ Llamar repositorio         │
        └────────┬───────────────────┘
                 │
                 ▼
╔════════════════════════════════════════╗
║  RepoMoneda.Alta()                     ║
║  ↓                                     ║
║  _connection.Execute(                  ║
║    "CALL AltaCriptoMoneda",            ║
║    parameters)                         ║
║  ↓                                     ║
║  Usar Dapper ORM                       ║
╚════════════════┬═══════════════════════╝
                 │
                 ▼
    ┌──────────────────────────────┐
    │ MySQL - Stored Procedure     │
    │ CALL AltaCriptoMoneda(        │
    │   45000.50,                  │
    │   100,                       │
    │   'Bitcoin'                  │
    │ )                            │
    └────────┬─────────────────────┘
             │
             ▼
╔═════════════════════════════════════════╗
║  Base de Datos - Tabla Moneda          ║
║                                         ║
║  INSERT INTO Moneda                    ║
║  (Nombre, Precio, Cantidad)            ║
║  VALUES ('Bitcoin', 45000.50, 100)     ║
║                                         ║
║  ✓ IdMoneda = 11 (auto-increment)     ║
║  ✓ Moneda creada en BD                 ║
╚═════════════════┬══════════════════════╝
                  │
                  ▼
        ┌───────────────────────────┐
        │ Volver a MonedasController│
        │                           │
        │ return RedirectToAction(  │
        │   nameof(Index))          │
        └───────┬─────────────────┘
                │
                ▼
        ┌──────────────────────────┐
        │ Redirect → /Monedas      │
        │ GET /Monedas/Index       │
        └───────┬──────────────────┘
                │
                ▼
        ┌──────────────────────────┐
        │ Mostrar listado          │
        │                          │
        │ Tabla de monedas:        │
        │ - Nombre | Precio | Cant │
        │ - Bitcoin|45000.50|100   │
        │ - Ethereum|...          │
        │                          │
        │ ✓ "Bitcoin" aparece    │
        │   en la lista            │
        └──────────────────────────┘
```

---

## 5. ARQUITECTURA GENERAL

```
┌─────────────────────────────────────────────────────────────────┐
│                         NAVEGADOR (Cliente)                     │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐  │
│  │  HTML (Views) + CSS + JavaScript                        │  │
│  │                                                          │  │
│  │  Views/                                                 │  │
│  │  ├── Account/                                          │  │
│  │  │   ├── Login.cshtml                                 │  │
│  │  │   └── Register.cshtml                              │  │
│  │  ├── Monedas/                                         │  │
│  │  │   ├── Index.cshtml     (Listado)                  │  │
│  │  │   └── Create.cshtml    (Formulario)               │  │
│  │  └── Shared/                                          │  │
│  │      └── _Layout.cshtml   (Template base)             │  │
│  └──────────────────┬───────────────────────────────────┘  │
│                     │                                        │
│                     │ HTTP Requests/Responses               │
│                     │ (JSON, HTML, Cookies)                │
│                     │                                        │
│                     ▼                                        │
└─────────────────────────────────────────────────────────────────┘
                      │
                      │
┌─────────────────────────────────────────────────────────────────┐
│                   ASP.NET CORE MVC (Servidor)                   │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │  Controllers (Lógica de la aplicación)                 │   │
│  │                                                         │   │
│  │  ├── AccountController                                │   │
│  │  │   ├── Login()      → Autenticación                │   │
│  │  │   ├── Register()   → Crear usuario                │   │
│  │  │   └── Logout()     → Desautenticar                │   │
│  │  │                                                     │   │
│  │  ├── MonedasController                               │   │
│  │  │   ├── Index()      → Listar monedas              │   │
│  │  │   ├── Create()     → Crear moneda (Admin)        │   │
│  │  │   └── Details()    → Ver detalle                 │   │
│  │  │                                                     │   │
│  │  ├── UsuariosController                              │   │
│  │  │   ├── Index()      → Listar usuarios             │   │
│  │  │   ├── Details()    → Ver billetera                │   │
│  │  │   ├── Ingresar()   → Agregar dinero              │   │
│  │  │   └── ToggleAdmin()→ Cambiar rol                 │   │
│  │  │                                                     │   │
│  │  └── HistorialesController                           │   │
│  │      ├── Index()      → Ver transacciones           │   │
│  │      └── Details()    → Ver detalle                 │   │
│  └─────────┬──────────────────────────────────────────┘   │
│            │                                               │
│            ▼                                               │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Models (DTOs - Data Transfer Objects)             │   │
│  │                                                     │   │
│  │  ├── UsuarioDto       (Campos: Nombre, Email, etc) │   │
│  │  ├── MonedaDto        (Campos: Nombre, Precio)    │   │
│  │  ├── HistorialDto     (Campos: Usuario, Moneda)   │   │
│  │  └── UsuarioMonedaDto (Billetera del usuario)     │   │
│  │                                                     │   │
│  │  [Validaciones: [Required], [EmailAddress], etc]   │   │
│  └─────────┬──────────────────────────────────────────┘   │
│            │                                               │
│            ▼                                               │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Repositories (Acceso a datos)                      │   │
│  │                                                     │   │
│  │  ├── IRepoUsuario → RepoUsuario                    │   │
│  │  ├── IRepoMoneda → RepoMoneda                      │   │
│  │  └── IRepoHistorial → RepoHistorial                │   │
│  │                                                     │   │
│  │  Métodos:                                           │   │
│  │  - Obtener()           (SELECT * FROM)              │   │
│  │  - Detalle(id)         (SELECT by ID)              │   │
│  │  - Alta(entity)        (INSERT)                    │   │
│  │  - Modificar(entity)   (UPDATE)                    │   │
│  │  - ObtenerPorEmail()   (Búsqueda específica)        │   │
│  └─────────┬──────────────────────────────────────────┘   │
│            │                                               │
│            │ SQL Queries (Dapper ORM)                    │
│            ▼                                               │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  IDbConnection (MySqlConnection)                   │   │
│  │                                                     │   │
│  │  ✓ Execute(SQL)    → INSERT, UPDATE, DELETE        │   │
│  │  ✓ Query(SQL)      → SELECT                        │   │
│  │  ✓ QueryFirstOrDefault → SELECT con WHERE         │   │
│  └─────────┬──────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
            │
            │
┌───────────┴─────────────────────────────────────────────────────┐
│                 MySQL Base de Datos (BD)                        │
│                                                                 │
│  Tablas:                                                        │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │   Usuario    │  │    Moneda    │  │  Historial   │         │
│  ├──────────────┤  ├──────────────┤  ├──────────────┤         │
│  │ IdUsuario    │  │ IdMoneda     │  │ IdHistorial  │         │
│  │ Nombre       │  │ Nombre       │  │ IdUsuario    │         │
│  │ Apellido     │  │ Precio       │  │ IdMoneda     │         │
│  │ Email        │  │ Cantidad     │  │ Cantidad     │         │
│  │ Password     │  │              │  │ Compra       │         │
│  │ Saldo        │  │ (PK)         │  │ FechaHora    │         │
│  │ EsAdmin      │  └──────────────┘  │ (PK)         │         │
│  │ (PK)         │                     └──────────────┘         │
│  └──────────────┘                                              │
│                                                                 │
│  Procedures:                                                    │
│  - AltaUsuario(nombre, apellido, email, pass, admin)          │
│  - AltaCriptoMoneda(precio, cantidad, nombre)                 │
│                                                                 │
│  Triggers:                                                      │
│  - MarketWeight_Usuario_Insert → Hash SHA-256 password        │
└─────────────────────────────────────────────────────────────────┘
```

---

**Resumen:** Cada componente tiene una responsabilidad clara y se comunica a través de capas bien definidas.
