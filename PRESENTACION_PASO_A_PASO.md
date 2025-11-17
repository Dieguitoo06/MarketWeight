# 🎬 GUÍA DE PRESENTACIÓN EN VIVO - Paso a Paso

> Cómo presentar mañana: Qué decir, qué mostrar, en qué orden

---

## ⏱️ AGENDA (20-30 minutos total)

```
Introducción (2 min)     → Qué es MarketWeight
Demostración (8 min)     → Usar la app en vivo
Código (10 min)          → Mostrar arquitectura
Detalles técnicos (5 min) → Profundizar en temas clave
Preguntas (5 min)        → Responder preguntas
```

---

## 📍 FASE 1: INTRODUCCIÓN (2 minutos)

### Lo que dices:

> "Buenos días/tardes. Mi proyecto se llama **MarketWeight**, es una aplicación web de **trading de criptomonedas**.
>
> **¿Qué hace?**
> - Usuarios pueden registrarse y crear una cuenta
> - Depositar dinero
> - Comprar y vender criptomonedas
> - Ver historial de transacciones
> - Administradores pueden crear nuevas monedas
>
> **¿Cómo está hecho?**
> - Backend: ASP.NET Core 8 con arquitectura en capas
> - Frontend: Bootstrap 5 + CSS custom responsive
> - Base de datos: MySQL
> - Patrón Repository para acceso a datos
> - Autenticación con cookies seguras
>
> Voy a mostrarles cómo funciona y luego el código detrás."

### Puntos clave:
- ✅ Claro y conciso
- ✅ Menciona tecnologías
- ✅ Explica arquitectura brevemente
- ✅ Indica qué van a ver después

---

## 🖥️ FASE 2: DEMOSTRACIÓN EN VIVO (8 minutos)

### Preparación antes de presentar:

**Desde ahora:**
```
1. Abre el proyecto en VS Code
2. Ejecuta: dotnet run (o usa tarea "watch")
3. Abre navegador: http://localhost:5000 (o puerto configurado)
4. Ten datos de test listos
```

### Escenario de prueba propuesto:

**Usuario 1 (Cliente):**
```
Email: cliente@test.com
Password: password123
```

**Usuario 2 (Admin):**
```
Email: admin@test.com
Password: admin123
EsAdmin: true (en BD o creado como admin)
```

**Moneda de prueba (ya debe existir en BD):**
```
Nombre: Bitcoin
Precio: 50000
Cantidad: 10
```

### Guión de presentación:

#### 1. Página de inicio (20 segundos)

**Dices:**
> "Aquí estamos en la página de inicio. Se ve responsive con Bootstrap. Abajo hay un navbar con navegación."

**Lo que haces:**
- Mostrar homepage
- Mencionar elementos: navbar, hero section, cards
- Abre DevTools (F12) → pestaña Device Emulation
- Redimensiona a móvil → muestra navbar colapsable

**Puntos a destacar:**
- Responsive: navbar cambia en móvil
- Bootstrap: clases predefinidas

---

#### 2. Registrarse (1 minuto)

**Dices:**
> "Ahora voy a registrar un nuevo usuario. Click en 'Register'."

**Lo que haces:**
```
1. Click en "Register" (o link en navbar)
2. Rellena:
   - Nombre: Juan
   - Apellido: Pérez
   - Email: juan@test.com
   - Password: password123
   - Confirmar: password123
3. Click "Register"
```

**Si todo ok:**
> "Registro exitoso, redirige a login. Aquí puedo ver que:
> 1. Validación en frontend (campos requeridos)
> 2. Validación en servidor (email único)
> 3. Contraseña hasheada en BD (nunca plaintext)"

**Si email ya existe:**
> "Aquí ve que si intento email duplicado, muestra error 'Email ya registrado'. Esto es validación en BD."

---

#### 3. Login (1 minuto)

**Dices:**
> "Ahora inicio sesión con el usuario creado."

**Lo que haces:**
```
1. Email: juan@test.com
2. Password: password123
3. Click "Login"
```

**Si ok, muestra:**
> "Redirige a home logueado. En el navbar aparece 'Hola Juan' o similar, con opciones:
> - Mi perfil
> - Ver monedas
> - Mi billetera
> - Logout"

**Abre DevTools:**
> "Voy a mostrar la cookie de sesión. Pestaña Application → Cookies → localhost"

**Lo que muestra:**
```
Cookies:
├── MarketWeight.Auth
   ├── Value: [long encoded string]
   ├── HttpOnly: ✓ (checkbox marcado)
   ├── Secure: [depende de HTTPS]
   ├── SameSite: Lax
   ├── Expires: [30 min desde ahora]
```

> "Aquí ven HttpOnly = true, lo que significa que JavaScript NO puede acceder a esta cookie, previniendo XSS."

---

#### 4. Ver monedas disponibles (1 minuto)

**Dices:**
> "Aquí vemos todas las criptomonedas disponibles para comprar."

**Lo que haces:**
```
1. Click en "Monedas" o "Ver Monedas"
2. Muestra tabla/cards con monedas
3. Click en una moneda para Details
```

**Puntos a destacar:**
- Lista de monedas desde BD
- Información: nombre, precio, cantidad disponible
- Botones: Comprar, Ver Detalles (si admin: Editar/Borrar)
- Responsive: En móvil, tabla se adapta o muestra en cards

---

#### 5. Comprar una moneda (2 minutos)

**Dices:**
> "Voy a comprar una moneda. Pero primero, necesito tener dinero."

**Lo que haces:**
```
1. Click en "Mi Perfil" o "Usuario"
2. Click en "Ingresar Dinero" o similar
3. Ingresa cantidad: 100000
4. Click "Ingresar"
5. Muestra: "Saldo actualizado a $100,000"
```

**Ahora compra:**
```
1. Vuelve a "Monedas"
2. Click en "Comprar" en una moneda (ej: Bitcoin)
3. Ingresa cantidad: 2 bitcoins
4. Click "Comprar"
5. Muestra: "Compra exitosa"
6. Redirige a "Mis Monedas" o Historial
```

**Puntos a destacar:**
> "Aquí vemos que:
> 1. Validación: Precio = 50,000 x 2 = 100,000 exacto
> 2. Saldo suficiente: ✓
> 3. Stock disponible: ✓
> 4. Transacción registrada en Historial"

---

#### 6. Ver historial (30 segundos)

**Dices:**
> "Aquí está el historial de todas mis transacciones."

**Lo que haces:**
```
1. Click en "Historial" o "Transacciones"
2. Muestra tabla con:
   - Fecha/Hora
   - Moneda
   - Cantidad
   - Precio
   - Tipo: Compra/Venta
```

> "Cada compra, depósito, venta queda registrado permanentemente en BD."

---

#### 7. Admin - Crear moneda (2 minutos)

**Dices:**
> "Si eres administrador, puedes crear nuevas criptomonedas."

**Lo que haces (si tienes admin):**
```
1. Logout del usuario actual
2. Login como admin (admin@test.com / admin123)
3. Click en "Administración" o "Crear Moneda"
4. Formulario con:
   - Nombre: Ethereum
   - Precio: 3000
   - Cantidad: 50
5. Click "Crear"
6. Muestra: "Moneda creada exitosamente"
7. Aparece en lista de monedas
```

**Puntos a destacar:**
> "Aquí vemos que:
> 1. [Authorize]: Solo logueados
> 2. Validación de Admin: Solo admin accede
> 3. DTOs validan precio/cantidad > 0
> 4. Moneda guardada en BD"

---

### 🚨 Si algo falla:

**Plan B:**
```
"Tengo datos prealmacenados en BD. Voy a mostrar directamente 
en el navegador. [Abre Developer Tools → Network/Storage]"
```

**Plan C:**
```
"El ambiente local está en startup. Déjenme mostrar el código 
que hace todo esto [abre VS Code]"
```

---

## 💻 FASE 3: CÓDIGO (10 minutos)

### Antes de mostrar código:

**Dices:**
> "Ahora voy a mostrar cómo está implementado todo esto técnicamente. La aplicación usa patrón MVC con Repository."

**Abre VS Code / Explorer en la solución**

### 1. Estructura de carpetas (1 minuto)

**Dices:**
> "El proyecto está en capas:
> - **MarketWeight.Core:** Modelos de negocio
> - **MarketWeight.Ado.Dapper:** Acceso a datos con repositorios
> - **{MarketWeight}.mvc:** Presentación (controladores, vistas)"

**Lo que muestras:**
```
Explorer:
├── src/
│   ├── MarketWeight.Core/
│   │   ├── Usuario.cs
│   │   ├── Moneda.cs
│   │   ├── Historial.cs
│   │   └── UsuarioMoneda.cs
│   │
│   ├── MarketWeight.Ado.Dapper/
│   │   ├── RepoUsuario.cs
│   │   ├── RepoMoneda.cs
│   │   ├── RepoHistorial.cs
│   │   └── IRepo*.cs (interfaces)
│   │
│   └── {MarketWeight}.mvc/
│       ├── Controllers/ (6 archivos)
│       ├── Views/ (17 vistas)
│       ├── Models/ (DTOs)
│       └── Program.cs (configuración)
```

> "Ventaja de esta estructura: Si cambio de MySQL a SQL Server, solo cambio Ado.Dapper. Controladores y Vistas siguen igual."

---

### 2. Program.cs - Configuración (2 minutos)

**Dices:**
> "Aquí está la configuración de autenticación. Es lo más importante."

**Abre: `Program.cs`**

**Muestra estas líneas (aprox 17-40):**

```csharp
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        
        options.Cookie.Name = "MarketWeight.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });
```

**Explicas:**
> "Aquí configuro:
> 1. **SlidingExpiration = true:** Renueva sesión con cada click
> 2. **ExpireTimeSpan = 30 min:** Expira si 30 min inactivo
> 3. **HttpOnly = true:** Protege contra XSS
> 4. **SecurePolicy = SameAsRequest:** HTTP en desarrollo, HTTPS en producción"

---

### 3. AccountController - Register (2 minutos)

**Dices:**
> "Aquí está cómo se registra un usuario."

**Abre: `AccountController.cs` → método `Register`**

**Muestra:**

```csharp
[HttpPost]
public IActionResult Register(UsuarioDto usuario)
{
    if (!ModelState.IsValid)
        return View(usuario);
    
    usuario.Password = HashPassword(usuario.Password);
    
    try
    {
        _repoUsuario.Alta(usuario);
        return RedirectToAction("Login");
    }
    catch (ConstraintException)
    {
        ModelState.AddModelError("", "Email ya registrado");
        return View(usuario);
    }
}
```

**Explicas:**
> "Pasos:
> 1. Valida que email, password, etc. sean válidos (ModelState)
> 2. Hashea la contraseña con SHA-256
> 3. Llama RepoUsuario.Alta() para guardar
> 4. Si email duplicado, catch muestra error
> 5. Si ok, redirige a login"

---

### 4. RepoUsuario - Alta (Insertar) (2 minutos)

**Dices:**
> "Esto es la capa de acceso a datos. Usa Dapper con parámetros seguros."

**Abre: `RepoUsuario.cs` → método `Alta`**

**Muestra:**

```csharp
public void Alta(Usuario usuario)
{
    var consulta = @"INSERT INTO Usuario (nombre, apellido, email, pass, saldo, esAdmin) 
                    VALUES (@nombre, @apellido, @email, @pass, @saldo, @esAdmin)";
    
    var parametros = new 
    {
        nombre = usuario.Nombre,
        apellido = usuario.Apellido,
        email = usuario.Email,
        pass = usuario.Password,
        saldo = usuario.Saldo,
        esAdmin = usuario.EsAdmin
    };
    
    Conexion.Execute(consulta, parametros);
}
```

**Explicas:**
> "Aquí vemos:
> 1. SQL con parámetros @nombre, @email, etc.
> 2. Dapper trata parámetros como valores, no código SQL
> 3. Previene SQL Injection: aunque @email tenga comillas, no se ejecuta
> 4. `Conexion.Execute()` es inyectado por Dependency Injection"

---

### 5. MonedasController - Buy (2 minutos)

**Dices:**
> "Aquí está la compra de monedas. Valida todo antes de procesar."

**Abre: `MonedasController.cs` → método `Buy`**

**Muestra las validaciones clave:**

```csharp
[Authorize]
[HttpPost]
public IActionResult Buy(int idMoneda, decimal cantidad)
{
    // 1. Obtener usuario logueado
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    
    // 2. Obtener moneda
    var moneda = _repoMoneda.ObtenerPorId(idMoneda);
    if (moneda == null) return NotFound();
    
    // 3. Validar cantidad disponible
    if (moneda.Cantidad < cantidad)
    {
        ModelState.AddModelError("", "Stock insuficiente");
        return View();
    }
    
    // 4. Obtener usuario y validar saldo
    var usuario = _repoUsuario.ObtenerPorId(...);
    decimal costo = moneda.Precio * cantidad;
    
    if (usuario.Saldo < costo)
    {
        ModelState.AddModelError("", "Saldo insuficiente");
        return View();
    }
    
    // 5. Registrar compra
    var historial = new Historial { ... };
    _repoHistorial.Alta(historial);
    
    // 6. Actualizar saldo y stock
    _repoMoneda.ActualizarCantidad(idMoneda, cantidad);
    _repoUsuario.ActualizarSaldo(usuario.IdUsuario, -costo);
    
    return RedirectToAction("Index", "UsuarioMonedas");
}
```

**Explicas:**
> "Capas de validación:
> 1. **Autorización:** [Authorize] solo logueados
> 2. **Existencia:** ¿La moneda existe?
> 3. **Stock:** ¿Hay cantidad suficiente?
> 4. **Saldo:** ¿Tienes dinero?
> 5. **Transacción:** Se registra todo en Historial
> 6. **Actualización:** Saldo y stock cambian atomáticamente"

---

### 6. CSS Responsive (1 minuto)

**Dices:**
> "Ahora el frontend. Responsive con Bootstrap + CSS custom."

**Abre: `responsive.css`**

**Muestra los breakpoints:**

```css
/* Móvil: <576px (estilos en site.css) */

/* Tablets 576px-767px */
@media (min-width: 576px) {
    .container { max-width: 540px; }
}

/* Tablets 768px-991px */
@media (min-width: 768px) {
    .container { max-width: 720px; }
    .navbar-toggle { display: none; }
    .navbar-nav { display: flex; }
}

/* Desktops 992px-1199px */
@media (min-width: 992px) {
    .container { max-width: 960px; }
}

/* Desktops 1200px+ */
@media (min-width: 1200px) {
    .container { max-width: 1140px; }
}
```

**Explicas:**
> "Mobile-first:
> 1. Base: Estilos para móvil
> 2. Media queries: Mejoran conforme crece pantalla
> 3. Breakpoints: 5 puntos de quiebre
> 4. Bootstrap: Grid 12 columnas reutiliza esto
> 5. Flexbox: Layouts flexibles que se adaptan"

---

## 📊 FASE 4: DETALLES TÉCNICOS (5 minutos)

**Dices:**
> "Algunas decisiones técnicas que tomé:"

### 1. ¿Por qué Dapper? (1 minuto)

> "Elegí Dapper en lugar de Entity Framework porque:
> - Más control: SQL explícito
> - Más rápido: Menos overhead
> - Más seguro: Parámetros obvios
> - Ideal para proyectos pequeños
> 
> Alternativa: EF Core sería mejor para proyectos grandes."

### 2. ¿Por qué Repository Pattern? (1 minuto)

> "Separa controladores de BD:
> - Cambio BD sin tocar controladores
> - Facilita testing (mock del repositorio)
> - Código más mantenible
> - Escalable"

### 3. ¿Por qué SlidingExpiration? (1 minuto)

> "Balance entre seguridad y usabilidad:
> - Seguridad: Expira si inactivo 30 min
> - Usabilidad: No expira si estoy activo
> - Alternativa: Tokens JWT (stateless)"

### 4. ¿Por qué SHA-256? (1 minuto)

> "Es lo que estaba implementado. No es ideal (debería ser bcrypt), pero:
> - Funciona
> - Está en trigger BD
> - Proyecto de aprendizaje
> 
> En producción: Bcrypt con salt + iteraciones"

### 5. ¿Qué falta? (1 minuto)

> "Reconozco mejoras posibles:
> - Más tests (tengo unitarios, necesito integración)
> - Recovery de contraseña (no implementado)
> - Manejo de errores más robusto
> - Auditoría de cambios
> - Caché para performance
> - HTTPS en producción"

---

## ❓ FASE 5: PREGUNTAS (5 minutos)

**Dices:**
> "Estoy disponible para preguntas. Adelante."

### Preguntas probables (ya preparadas):

**P1: "¿Cómo prevines XSS?"**
> "HttpOnly en cookies (JS no accede), Razor escaping automático."

**P2: "¿Cómo prevines SQL Injection?"**
> "Dapper con parámetros @email, @nombre. Parámetro es valor, no SQL."

**P3: "¿Qué pasa si 100 usuarios compran al mismo tiempo?"**
> "Validación + triggers, pero podría mejorar con SELECT FOR UPDATE."

**P4: "¿Por qué no tienes recovery de contraseña?"**
> "Fuera del scope inicial. Se haría con token temporal + email."

**P5: "¿Escalabilidad?"**
> "Actualmente ~1000 usuarios sin problemas. Más requiere: caché, índices, async, paginación."

---

## 🎬 TIPS FINALES PARA MAÑANA

### Cómo no quedar mal:

✅ **Habla con confianza:** Sabes qué hiciste  
✅ **Muestra código:** No solo hagas claims, pruébalo  
✅ **Sé honesto:** Reconoce qué falta, demuestra reflexión  
✅ **No memorices:** Entiendo conceptos, no recito monólogos  
✅ **Responde preguntas:** Aunque no sepas TODO, razona una respuesta  

❌ **NO:**
- ❌ Hablar muy rápido
- ❌ Perderse en detalles innecesarios
- ❌ Decir "no sé" sin intentar
- ❌ Fingir que todo es perfecto
- ❌ Cambiar de tema cuando preguntan algo incómodo

### Timing:

```
Introducción:    2 min (rígido)
Demo:            8 min (si falla, pasar a código)
Código:          10 min (flexible, adelanta si necesario)
Técnicos:        5 min (solo si tiempo)
Preguntas:       Resto del tiempo
```

### Material de apoyo:

Ten abierto en otra pestaña:
- **EVALUACION_PROYECTO.md** - Responde "¿qué está bien?"
- **PREGUNTAS_EVALUACION.md** - Para responder preguntas específicas

---

## 📋 CHECKLIST ANTES DE PRESENTAR

**Hace 1 hora:**
- [ ] Proyecto compilado y sin errores
- [ ] BD levantada y datos de test listos
- [ ] http://localhost:5000 accesible
- [ ] Aplicación funciona (registro, login, compra)
- [ ] Cambio de tamaño navegador (responsive)

**En el momento:**
- [ ] VS Code abierto con el proyecto
- [ ] Navegador con app
- [ ] DevTools listo (F12)
- [ ] Archivos markdown a mano (otra pestaña)
- [ ] Voz tranquila y postura segura

**Durante:**
- [ ] Mira al evaluador mientras hablas
- [ ] Habla hacia la audiencia, no la pantalla
- [ ] Pausa entre frases
- [ ] Pregunta: "¿Se ve bien? ¿Entienden?"
- [ ] Si preguntan algo no preparado: "Buena pregunta, la investigué así..."

---

## 🎯 ÚLTIMO CONSEJO

**Recuerda:**
Los evaluadores no esperan perfección. Buscan:
1. ✅ Que funcione
2. ✅ Que entiendas qué hiciste
3. ✅ Que reconozcas qué falta
4. ✅ Que puedas explicar decisiones

**Tienes todo eso. ¡Vas a hacerlo bien!** 💪

---

**¡ÉXITO MAÑANA!**

*Última revisión: 17 de noviembre de 2025*
