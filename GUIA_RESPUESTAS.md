# Guía de Respuestas - MarketWeight

> Documento de referencia rápida para responder preguntas frecuentes sobre el proyecto MarketWeight

---

## 1. ¿Cómo se crea un usuario en la aplicación?

### Respuesta Técnica:

**Método de Creación:**
El registro de usuarios se realiza a través del formulario de **Register** en la vista `Account/Register.cshtml`. No se crean usuarios manualmente desde la interfaz de administrador, sino que los usuarios se registran a sí mismos.

### Flujo de Creación:

1. **Frontend - Formulario de Registro**
   - Ubicación: `/Account/Register`
   - Campos requeridos:
     - Nombre
     - Apellido
     - Email (validado como único)
     - Contraseña

2. **Backend - Procesamiento en AccountController.cs**
   ```csharp
   [HttpPost]
   [ValidateAntiForgeryToken]
   [AllowAnonymous]
   public async Task<IActionResult> Register(string nombre, string apellido, string email, string password)
   {
       // Validaciones
       // Verificar que el email no esté registrado
       var yaExiste = _repoUsuario.ObtenerPorEmail(emailNorm);
       if (yaExiste is not null)
       {
           ModelState.AddModelError(string.Empty, "El email ya está registrado");
           return View();
       }
       
       // Crear nuevo usuario
       _repoUsuario.Alta(new Usuario
       {
           Nombre = nombre.Trim(),
           Apellido = apellido.Trim(),
           Email = emailNorm,
           Password = password.Trim(),  // El trigger de la BD hará SHA-256
           Saldo = 0,
           EsAdmin = false
       });
       
       return RedirectToAction("Login");
   }
   ```

3. **Base de Datos - Almacenamiento**
   - El procedimiento almacenado `AltaUsuario` en MySQL es llamado por el repositorio
   - El trigger `MarketWeight_Usuario_Insert` aplica hash SHA-256 a la contraseña automáticamente
   - Los usuarios nuevos comienzan con:
     - Saldo inicial: 0
     - Rol: Usuario normal (no admin)

### Puntos Clave:
- ✅ Las contraseñas se hashean con SHA-256 en la base de datos (via trigger)
- ✅ El email es validado como único antes de crear
- ✅ Se normalizan datos: trim() y lowercase para email
- ✅ Los usuarios nuevos no pueden ser admin (solo por promoción manual)

### Comandos SQL relevantes:
```sql
CALL AltaUsuario('Juan', 'Pérez', 'juan@example.com', 'password123', FALSE);
```

---

## 2. ¿Cómo hiciste que la página sea responsive?

### Respuesta Técnica:

Utilicé un **enfoque Mobile-First con Media Queries** en CSS, implementado en dos archivos principales:

### Estrategia Mobile-First:

1. **Base Mobile (Todos los estilos comienzan aquí)**
   - Estilos por defecto optimizados para pantallas pequeñas
   - Tamaño de fuente reducido (13px en móviles)
   - Padding y márgenes ajustados
   - Componentes apilados en vertical

2. **Media Queries Progresivas**
   
   **Archivo: `responsive.css`**
   ```css
   /* Dispositivos extra pequeños (< 576px) - Smartphones */
   @media (max-width: 575.98px) {
     html { font-size: 13px; }
     .card { padding: 12px; }
     .navbar { padding: 0.5rem 0; }
     /* ... más ajustes */
   }
   
   /* Dispositivos pequeños (576px - 767px) - Landscape phones */
   @media (min-width: 576px) and (max-width: 767.98px) {
     html { font-size: 14px; }
     /* Ajustes progresivos */
   }
   
   /* Tablets (768px+) */
   @media (min-width: 768px) {
     html { font-size: 16px; }
     .mw-shell { max-width: 1200px; }
   }
   
   /* Desktops (992px+) */
   @media (min-width: 992px) {
     .container { padding-left: 1.5rem; padding-right: 1.5rem; }
   }
   
   /* Desktops grandes (1200px+) */
   @media (min-width: 1200px) {
     /* Optimizaciones finales */
   }
   ```

### Técnicas Utilizadas:

#### a) **Unidades Relativas**
- `rem` para fuentes y espaciado (escalable)
- `%` para anchos de contenedores
- `max-width` en contenedores principales

#### b) **Flexbox y Grid**
```css
.section-title {
  display: flex;
  align-items: center;
  justify-content: space-between;
  flex-direction: column;  /* Mobile */
}

@media (min-width: 768px) {
  .section-title {
    flex-direction: row;   /* Desktop */
  }
}
```

#### c) **Variables CSS para Temas**
```css
:root {
  --bg-primary: #0f172a;    /* Tema oscuro por defecto */
  --text-primary: #f1f5f9;
  --accent-1: #0ea5e9;
}

[data-theme="light"] {
  --bg-primary: #f8fafc;    /* Tema claro alternativo */
  --text-primary: #0f172a;
}
```

#### d) **Imágenes Responsive**
```css
img {
  max-width: 100%;
  height: auto;
}
```

#### e) **Componentes Adaptativos**
- Tablas: reducen tamaño de fuente en móvil
- Tarjetas: ajustan padding según pantalla
- Botones: tamaños variables
- Navbar: colapsable en móvil

### Breakpoints Utilizados:
| Dispositivo | Ancho | Media Query |
|---|---|---|
| Smartphones | < 576px | `@media (max-width: 575.98px)` |
| Landscape phones | 576px - 767px | `@media (min-width: 576px) and (max-width: 767.98px)` |
| Tablets | 768px+ | `@media (min-width: 768px)` |
| Desktops | 992px+ | `@media (min-width: 992px)` |
| Desktops grandes | 1200px+ | `@media (min-width: 1200px)` |

### Archivos Clave:
- `wwwroot/css/site.css` - Estilos base y componentes
- `wwwroot/css/responsive.css` - Media queries responsive
- `Views/Shared/_Layout.cshtml` - HTML semántico
- Bootstrap 5 - Grid system como base

### Ventajas del Enfoque:
✅ Carga más rápida en móvil (menos CSS)
✅ Mejor UX en todos los dispositivos
✅ Fácil mantenimiento de breakpoints
✅ Compatible con todos los navegadores modernos

---

## 3. ¿Qué cookies utilizas y cómo las configuraste?

### Respuesta Técnica:

Utilizo **autenticación por cookies** para mantener las sesiones de usuario. La configuración está en `Program.cs`.

### Tipo de Cookie: Authentication Cookie (Cookies de Autenticación)

### Configuración en Program.cs:

```csharp
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // 1. RUTAS DE REDIRECCIÓN
        options.LoginPath = "/Account/Login";           // Redirigir si no autenticado
        options.AccessDeniedPath = "/Account/Login";    // Redirigir si acceso denegado
        
        // 2. RENOVACIÓN DE SESIÓN
        options.SlidingExpiration = true;               // Renovar cookie en cada petición
        
        // 3. TIEMPO DE EXPIRACIÓN
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);  // Expira después de 30 min inactivo
        
        // 4. PROPIEDADES DE SEGURIDAD
        options.Cookie.Name = "MarketWeight.Auth";      // Nombre de la cookie
        options.Cookie.HttpOnly = true;                 // No accesible desde JavaScript (XSS)
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        // En producción: CookieSecurePolicy.Always (requiere HTTPS)
    });
```

### ¿Qué es SlidingExpiration?

```
Comportamiento SIN SlidingExpiration (false):
- Usuario se autentica a las 10:00
- Cookie expira a las 10:30
- Si el usuario sigue activo pero la cookie expiró a las 10:29, será desconectado

Comportamiento CON SlidingExpiration (true):
- Usuario se autentica a las 10:00
- Cookie expira a las 10:30
- Si el usuario hace una petición a las 10:25, la cookie se RENUEVA a las 10:55
- Solo se desconecta si hay 30 minutos de inactividad real
```

### Flujo de Autenticación con Cookies:

1. **Login (AccountController.cs)**
   ```csharp
   // Crear claims (información del usuario)
   var claims = new List<Claim>
   {
       new Claim(ClaimTypes.NameIdentifier, usuarioPorEmail.IdUsuario.ToString()),
       new Claim(ClaimTypes.Name, $"{usuarioPorEmail.Nombre} {usuarioPorEmail.Apellido}"),
       new Claim(ClaimTypes.Email, usuarioPorEmail.Email),
       new Claim(ClaimTypes.Role, usuarioPorEmail.EsAdmin ? "Admin" : "User")
   };
   
   // Crear identidad
   var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
   
   // Crear cookie con propiedades
   var authProperties = new AuthenticationProperties { IsPersistent = true };
   
   // Firmar el usuario
   await HttpContext.SignInAsync(
       CookieAuthenticationDefaults.AuthenticationScheme,
       new ClaimsPrincipal(claimsIdentity),
       authProperties);
   ```

2. **Cookie en el Navegador**
   - Se almacena en el cliente (navegador)
   - Se envía automáticamente en cada petición HTTP
   - Contiene información cifrada del usuario
   - Durará 30 minutos de inactividad

3. **Validación en Controladores**
   ```csharp
   [Authorize]  // Solo usuarios autenticados
   public IActionResult Details(uint id)
   {
       var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
       // Usuario autenticado validado automáticamente
   }
   
   [Authorize(Roles = "Admin")]  // Solo administradores
   public IActionResult Create()
   {
       // Solo admins pueden acceder
   }
   ```

4. **Logout (AccountController.cs)**
   ```csharp
   [HttpPost]
   [Authorize]
   public async Task<IActionResult> Logout()
   {
       await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
       return RedirectToAction("Login");
   }
   ```

### Propiedades de la Cookie:

| Propiedad | Valor | Propósito |
|---|---|---|
| **Nombre** | `MarketWeight.Auth` | Identificar la cookie |
| **HttpOnly** | `true` | Prevenir acceso desde JavaScript |
| **SlidingExpiration** | `true` | Renovar en cada petición |
| **ExpireTimeSpan** | 30 minutos | Tiempo de expiración de inactividad |
| **IsPersistent** | `true` | Persistir entre sesiones del navegador |
| **SecurePolicy** | `SameAsRequest` | Funciona con HTTP (desarrollo) |

### Protecciones Implementadas:

✅ **HttpOnly**: Protección contra XSS (Cross-Site Scripting)
✅ **Validación CSRF**: Token anti-falsificación en formularios
✅ **Expiración**: La sesión expira después de 30 minutos inactivo
✅ **Cifrado**: Las claims se cifran en la cookie
✅ **Rol-based Access**: Control basado en roles (Admin/User)

### Cambios Recomendados para Producción:

```csharp
// En producción, cambiar a:
options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Solo HTTPS
options.ExpireTimeSpan = TimeSpan.FromHours(8);           // Tiempo más largo
options.Cookie.SameSite = SameSiteMode.Strict;            // Más restrictivo
```

---

## 4. ¿Cómo se crea una moneda en la aplicación?

### Respuesta Técnica:

Las monedas se crean a través de la interfaz web, pero solo los **administradores** pueden hacerlo. El flujo es:

### Opción 1: Desde la UI (Interfaz Gráfica)

1. **Acceso a la Página**
   - Solo administradores pueden ver el botón "Crear Moneda"
   - Ubicación: `/Monedas/Create`
   - Ubicado en la página de listado de monedas

2. **Formulario de Creación**
   ```html
   POST /Monedas/Create
   Parámetros:
   - Nombre: (string) Ej: "Bitcoin"
   - Precio: (decimal) Ej: 45000.50
   - Cantidad: (decimal) Ej: 100
   ```

3. **Validación en Backend (MonedasController.cs)**
   ```csharp
   [HttpPost]
   [ValidateAntiForgeryToken]
   [Authorize(Roles = "Admin")]
   public IActionResult Create(MonedaDto dto)
   {
       // Validar modelo
       if (!ModelState.IsValid)
           return View(dto);

       // Crear entidad
       var entity = new Moneda
       {
           Nombre = dto.Nombre,
           Precio = dto.Precio,
           Cantidad = dto.Cantidad
       };
       
       // Guardar en base de datos
       _repoMoneda.Alta(entity);
       
       // Redirigir al listado
       return RedirectToAction(nameof(Index));
   }
   ```

4. **Almacenamiento en BD**
   - Se llama al repositorio `IRepoMoneda.Alta()`
   - Este ejecuta el procedimiento `AltaCriptoMoneda` en MySQL
   - La moneda se guarda con ID autoincrementado

### Opción 2: Desde SQL (Base de Datos)

```sql
-- Procedimiento almacenado
CALL AltaCriptoMoneda(
    100.50,      -- Precio
    100,         -- Cantidad disponible
    'Bitcoin'    -- Nombre
);

CALL AltaCriptoMoneda(50.25, 100, 'Ethereum');
CALL AltaCriptoMoneda(75.00, 100, 'Ripple');
CALL AltaCriptoMoneda(20.75, 100, 'Litecoin');
```

### Estructura del DTO (MonedaDto.cs):

```csharp
public class MonedaDto
{
    public uint IdMoneda { get; set; }
    
    [Required]
    public decimal Precio { get; set; }
    
    [Required]
    public decimal Cantidad { get; set; }
    
    [Required]
    public string Nombre { get; set; } = string.Empty;
}
```

### Flujo Completo en Arquitectura:

```
UI (Formulario)
    ↓
MonedasController.Create()
    ↓
RepoMoneda.Alta(entity)
    ↓
Dapper + MySqlConnection
    ↓
Stored Procedure: AltaCriptoMoneda
    ↓
Base de Datos MySQL
    ↓
Tabla: Moneda (IdMoneda, Nombre, Precio, Cantidad)
```

### Validaciones Implementadas:

✅ Solo administradores pueden crear
✅ Campos requeridos: Nombre, Precio, Cantidad
✅ Valores deben ser numéricos válidos
✅ Token CSRF para prevenir ataques
✅ Validación en modelo y servidor

### Vista (Monedas/Create.cshtml):

```html
<form asp-action="Create" method="post">
    <div class="mb-3">
        <label asp-for="Nombre" class="form-label">Nombre</label>
        <input asp-for="Nombre" class="form-control" />
        <span asp-validation-for="Nombre" class="text-danger"></span>
    </div>
    
    <div class="mb-3">
        <label asp-for="Precio" class="form-label">Precio</label>
        <input asp-for="Precio" class="form-control" />
        <span asp-validation-for="Precio" class="text-danger"></span>
    </div>
    
    <div class="mb-3">
        <label asp-for="Cantidad" class="form-label">Cantidad</label>
        <input asp-for="Cantidad" class="form-control" />
        <span asp-validation-for="Cantidad" class="text-danger"></span>
    </div>
    
    <button type="submit" class="btn btn-primary">Guardar</button>
    <a class="btn btn-secondary" asp-action="Index">Cancelar</a>
</form>
```

### Permisos y Seguridad:

```csharp
[Authorize(Roles = "Admin")]  // Solo administradores
public IActionResult Create()
{
    return View(new MonedaDto());
}

// Los usuarios normales ven un botón gris deshabilitado
@if (User.IsInRole("Admin"))
{
    <a class="btn btn-primary btn-sm ms-2" asp-action="Create">
        Crear Moneda
    </a>
}
```

### Modelo de Datos en BD:

```sql
CREATE TABLE Moneda (
    IdMoneda INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Precio DECIMAL(18, 8) NOT NULL,
    Cantidad DECIMAL(18, 8) NOT NULL
);
```

---

## Resumen Rápido para Entrevistas:

### ¿Usuarios?
📝 Sistema de **auto-registro** vía formulario. Contraseñas hasheadas con SHA-256 en la base de datos. Emails únicos validados. Saldo inicial 0, rol de usuario normal por defecto.

### ¿Responsive?
📱 **Mobile-First** con Media Queries. 5 breakpoints (< 576px, 576-767px, 768px, 992px, 1200px+). Flexbox, unidades relativas (rem, %), variables CSS para temas. Componentes adaptativos (tablas, cards, navbar colapsable).

### ¿Cookies?
🍪 **Authentication Cookies** con SlidingExpiration. 30 minutos de inactividad. HttpOnly para XSS, claims basados en roles (Admin/User). SignIn al login, SignOut al logout. Token CSRF en formularios.

### ¿Monedas?
💰 Solo **admins** crean monedas. Formulario con validación. Procedure almacenado `AltaCriptoMoneda`. DTO con propiedades: Nombre, Precio, Cantidad. Autorización `[Authorize(Roles = "Admin")]`.

---

## Archivos Clave del Proyecto:

```
MarketWeight/
├── Program.cs                          ← Configuración de cookies y servicios
├── {MarketWeight}.mvc/
│   ├── Controllers/
│   │   ├── AccountController.cs        ← Login, Register, Logout
│   │   ├── MonedasController.cs        ← Crear, listar monedas
│   │   └── UsuariosController.cs       ← Gestión de usuarios
│   ├── Views/
│   │   ├── Account/Register.cshtml     ← Formulario de registro
│   │   ├── Monedas/Create.cshtml       ← Formulario crear moneda
│   │   └── Shared/_Layout.cshtml       ← Layout base
│   └── wwwroot/css/
│       ├── site.css                    ← Estilos base y temas
│       ├── responsive.css              ← Media queries responsive
│       └── _Layout.cshtml.css          ← Estilos del layout
└── scripts sql/
    └── 06 InsertsPrueba.sql            ← Datos de prueba
```

---

**Última actualización:** 17 de noviembre de 2025
**Versión del Proyecto:** MarketWeight v1.0 - Plataforma de Trading de Criptomonedas
