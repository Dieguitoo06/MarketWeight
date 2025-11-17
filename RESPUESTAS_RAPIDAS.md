# Respuestas Rápidas - MarketWeight (Cheat Sheet)

> Versión compacta para responder rápidamente en entrevistas

---

## 1️⃣ CREAR UN USUARIO

**¿Cómo?** 
- Formulario `/Account/Register` con: Nombre, Apellido, Email, Contraseña
- Email validado único, contraseña hasheada con SHA-256 en DB

**Archivos clave:**
- `AccountController.cs` → Método `Register()`
- `Views/Account/Register.cshtml` → Formulario
- `Usuario.cs` → Modelo

**Código resumido:**
```csharp
[HttpPost]
public async Task<IActionResult> Register(string nombre, string apellido, string email, string password)
{
    // Validar email único
    var existe = _repoUsuario.ObtenerPorEmail(email);
    if (existe != null) return error;
    
    // Crear usuario
    _repoUsuario.Alta(new Usuario { 
        Nombre = nombre, Apellido = apellido, 
        Email = email, Password = password,
        Saldo = 0, EsAdmin = false 
    });
    
    return RedirectToAction("Login");
}
```

---

## 2️⃣ RESPONSIVE (MOBILE-FIRST)

**¿Cómo?**
- Media queries en `responsive.css`
- 5 breakpoints: <576px, 576-767px, 768px, 992px, 1200px+
- Unidades relativas (rem, %), Flexbox

**Breakpoints:**
```css
/* Mobile: < 576px */
@media (max-width: 575.98px) { html { font-size: 13px; } }

/* Landscape: 576px - 767px */
@media (min-width: 576px) and (max-width: 767.98px) { ... }

/* Tablet: 768px+ */
@media (min-width: 768px) { html { font-size: 16px; } }

/* Desktop: 992px+ */
@media (min-width: 992px) { ... }

/* Large: 1200px+ */
@media (min-width: 1200px) { ... }
```

**Técnicas:**
- ✅ Flexbox `flex-direction: column` en móvil → row en desktop
- ✅ Imágenes: `max-width: 100%`
- ✅ Containers: `max-width` controlada
- ✅ Variables CSS para temas

---

## 3️⃣ COOKIES (AUTENTICACIÓN)

**¿Qué tipo?** 
`CookieAuthenticationDefaults` - Cookies de sesión

**Configuración en Program.cs:**
```csharp
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.SlidingExpiration = true;              // Renovar con cada petición
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "MarketWeight.Auth";
    options.Cookie.HttpOnly = true;                // Protección XSS
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});
```

**Propiedades clave:**
| Propiedad | Valor | Por qué |
|---|---|---|
| HttpOnly | true | Impide acceso desde JavaScript |
| SlidingExpiration | true | Renueva si hay actividad |
| ExpireTimeSpan | 30 min | Inactividad máx |
| Name | MarketWeight.Auth | Identificación |

**Flujo:**
1. **Login** → `SignInAsync()` crea cookie
2. **Peticiones** → Cookie se envía automáticamente
3. **Validación** → `[Authorize]` verifica
4. **Logout** → `SignOutAsync()` elimina cookie

---

## 4️⃣ CREAR MONEDA

**¿Quién?** Solo administradores `[Authorize(Roles = "Admin")]`

**¿Dónde?** `/Monedas/Create`

**Campos:**
- Nombre (string, requerido)
- Precio (decimal, requerido)
- Cantidad (decimal, requerido)

**Código en MonedasController.cs:**
```csharp
[HttpPost]
[Authorize(Roles = "Admin")]
public IActionResult Create(MonedaDto dto)
{
    if (!ModelState.IsValid) return View(dto);
    
    var entity = new Moneda { 
        Nombre = dto.Nombre, 
        Precio = dto.Precio, 
        Cantidad = dto.Cantidad 
    };
    
    _repoMoneda.Alta(entity);
    return RedirectToAction(nameof(Index));
}
```

**SQL equivalente:**
```sql
CALL AltaCriptoMoneda(100.50, 100, 'Bitcoin');
```

---

## 📋 TABLA COMPARATIVA

| Característica | Detalles |
|---|---|
| **Usuarios** | Auto-registro, validación email, SHA-256, saldo inicial 0 |
| **Responsive** | Mobile-first, 5 breakpoints, flexbox, rem units |
| **Cookies** | 30 min inactividad, HttpOnly, SlidingExpiration, Claims-based |
| **Monedas** | Solo admin, DTO con validación, stored procedure |

---

## 🔐 SEGURIDAD IMPLEMENTADA

✅ **Autenticación:** Cookies con encriptación  
✅ **Autorización:** Roles (Admin/User)  
✅ **CSRF Protection:** Token en formularios  
✅ **XSS Prevention:** HttpOnly en cookies  
✅ **Password Hash:** SHA-256 en base de datos  
✅ **Email Validation:** Único por usuario  

---

## 📁 ARCHIVOS MÁS IMPORTANTES

| Archivo | Responsabilidad |
|---|---|
| `Program.cs` | Configuración global, cookies |
| `AccountController.cs` | Login, Register, Logout |
| `MonedasController.cs` | CRUD de monedas |
| `wwwroot/css/site.css` | Estilos base |
| `wwwroot/css/responsive.css` | Media queries |
| `Models/*.cs` | DTOs con validación |

---

## ⚡ TIPS PARA LA ENTREVISTA

**Si dicen: "Cuéntame de tu proyecto"**
→ "MarketWeight es una plataforma de trading de criptomonedas con autenticación segura, interfaz responsive y gestión de usuarios y monedas"

**Si preguntan de seguridad:**
→ "Implementé cookies HttpOnly para XSS, CSRF tokens, hash SHA-256 para contraseñas, validación de emails únicos y control de acceso basado en roles"

**Si preguntan de frontend:**
→ "Enfoque mobile-first con 5 breakpoints, flexbox para layouts, variables CSS para temas claro/oscuro, inspirado en Binance"

**Si preguntan de arquitectura:**
→ "ASP.NET Core MVC con Dapper, inyección de dependencias, DTOs para validación, procedures almacenados, patrón Repository"

---

**Recuerda:** Sé específico, usa ejemplos del código, muestra entendimiento técnico 💪
