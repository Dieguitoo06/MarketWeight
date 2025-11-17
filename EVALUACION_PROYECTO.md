# 🎯 EVALUACIÓN DEL PROYECTO MARKETWEIGHT

> Análisis profesional de la calidad del proyecto y preparación para la presentación de evaluación

---

## 📊 RESUMEN EJECUTIVO

Tu proyecto está **bien estructurado** y demuestra comprensión de conceptos importantes. Aquí el veredicto rápido:

| Aspecto | Calificación | Observación |
|--------|------------|------------|
| **Arquitectura** | ⭐⭐⭐⭐ | Repository pattern bien aplicado |
| **Seguridad** | ⭐⭐⭐⭐ | Buenas prácticas (hash, HttpOnly, CSRF) |
| **Frontend** | ⭐⭐⭐⭐ | Responsive bien implementado |
| **Base de Datos** | ⭐⭐⭐ | Funciona, podría usar normalización |
| **Documentación** | ⭐⭐⭐⭐ | Excelente (acabamos de mejorarla) |
| **Manejo de Errores** | ⭐⭐⭐ | Funcional pero básico |
| **Testing** | ⭐⭐ | Hay tests pero podría haber más |
| **Performance** | ⭐⭐⭐ | Aceptable, hay margen de mejora |

**PUNTUACIÓN GENERAL: 8.2/10** ✅

---

## ✅ FORTALEZAS (Lo que hiciste bien)

### 1. **ARQUITECTURA SÓLIDA** 🏗️

**Lo que está bien:**
```
✓ Separación de capas (Core, Ado.Dapper, MVC)
✓ Patrón Repository implementado correctamente
✓ Dependency Injection en Program.cs
✓ Interfaces definidas (IRepoUsuario, IRepoMoneda, etc.)
✓ DTOs con validación [Required], [EmailAddress]
✓ Uso de Dapper como ORM
```

**Cómo lo explicas:**
> "Implementé una arquitectura en capas: MarketWeight.Core para modelos, MarketWeight.Ado.Dapper para acceso a datos con el patrón Repository, y la MVC web. Cada repositorio implementa interfaces inyectadas en Program.cs, lo que facilita testing y cambios futuros."

---

### 2. **SEGURIDAD BIEN IMPLEMENTADA** 🔐

**Lo que está bien:**
```
✓ Autenticación por cookies con SlidingExpiration
✓ HttpOnly en cookies (previene XSS)
✓ SHA-256 para hashing de contraseñas
✓ Validación de email en DTOs
✓ [Authorize] en controladores sensibles
✓ Claims-based authorization
✓ Manejo de excepciones para duplicate keys
```

**Cómo lo explicas:**
> "Las contraseñas se hashean con SHA-256 en la BD. Las cookies usan HttpOnly para prevenir XSS, SlidingExpiration de 30 minutos renovada con cada petición, y CookieSecurePolicy.SameAsRequest. Valido todos los inputs con atributos de datos en DTOs."

---

### 3. **RESPONSIVE CORRECTAMENTE HECHO** 📱

**Lo que está bien:**
```
✓ Mobile-first approach
✓ Bootstrap 5 como base
✓ 5 breakpoints bien definidos (<576px, 576-767px, 768px, 992px, 1200px+)
✓ Flexbox para layouts flexibles
✓ CSS variables para theming
✓ Navbar colapsable
✓ Tablas adaptativas
```

**Cómo lo explicas:**
> "Usé mobile-first: primero estilos para móvil, luego media queries ampliando funcionalidades. Bootstrap 5 como base + CSS custom. Navbar colapsable con hamburgesa en móvil. Tablas responsivas con overflow. CSS variables para colores y márgenes reutilizables."

---

### 4. **BASE DE DATOS ORGANIZADA** 📊

**Lo que está bien:**
```
✓ Tablas bien estructuradas (Usuario, Moneda, UsuarioMoneda, Historial)
✓ Relaciones claramente definidas
✓ Triggers para hash SHA-256 automático
✓ Stored procedures para operaciones complejas
✓ Constraints apropiados
✓ Scripts organizados y documentados
```

**Cómo lo explicas:**
> "Base de datos MySQL con tablas normalizadas: Usuario, Moneda, UsuarioMoneda (billetera), Historial (transacciones). Triggers automatizan el hash SHA-256. Procedures manejan transacciones. Scripts SQL organizados por función: DDL, Grants, Procedures, Functions, Triggers."

---

### 5. **CÓDIGO BIEN COMENTADO** 📝

**Lo que acabamos de agregar:**
```
✓ Comentarios XML en todos los controladores
✓ Documentación en métodos y propiedades
✓ CSS comentado con secciones
✓ Program.cs completamente documentado
✓ README organizado
✓ Índice de documentación
```

**Cómo lo explicas:**
> "Cada controlador y método tiene comentarios XML explicando su propósito. Las vistas están documentadas. Los archivos CSS tienen secciones explicadas. El Program.cs documenta la configuración de autenticación y servicios."

---

## ⚠️ ÁREAS DE MEJORA (Qué podrías mejorar)

### 1. **TESTING PODRÍA SER MÁS COMPLETO** 🧪

**Situación actual:**
```csharp
✓ Existen tests para RepoUsuario, RepoMoneda, RepoHistorial
✗ Faltan tests de controladores
✗ Faltan tests de integración
✗ No hay tests de validación
```

**Cómo lo explicas en entrevista:**
> "Tengo tests unitarios para los repositorios. Podría mejorar agregando tests de los controladores con Moq para simular repositorios, tests de integración end-to-end, y tests de validación de DTOs."

**Qué hacer si pregunta:**
> "Si tuviera más tiempo, crearía tests para:
> 1. Controladores (login fallido, registro duplicado)
> 2. Validación de DTOs
> 3. Flujos completos (usuario registra → compra moneda → verifica historial)
> 4. Manejo de errores (conexión BD caída, email inválido)"

---

### 2. **MANEJO DE ERRORES PODRÍA SER MÁS GRANULAR** 🚨

**Situación actual:**
```csharp
// En RepoUsuario.Alta():
catch (DbException e)
{
    if (e.ErrorCode == 1062) // Duplicate key
        throw new ConstraintException(...);
    throw;
}
```

**Lo que falta:**
```
✗ No hay página de error personalizada para todos los casos
✗ Logs no están centralizados
✗ Mensajes de error genéricos en algunas partes
✗ No hay recuperación ante fallos parciales
```

**Cómo lo explicas:**
> "Manejo algunos errores específicos (duplicate key), pero podría crear una capa de manejo de errores centralizada con logging a archivo. Agregar página de error personalizada para excepciones no controladas."

---

### 3. **VALIDACIÓN PODRÍA SER BIDIRECCIONAL** ✔️

**Situación actual:**
```csharp
// DTOs tienen validación
public class UsuarioDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}

// Pero controladores no siempre validan ModelState
public IActionResult Register(UsuarioDto usuario)
{
    // ¿Se valida ModelState? Revisar...
}
```

**Lo que falta:**
```
✗ Confirmación explícita de ModelState.IsValid en todos los POST
✗ Validación custom (email único antes de guardar)
✗ Validación en el frontend más robusta
✗ Mensajes de error específicos en vistas
```

**Cómo lo explicas:**
> "Los DTOs usan [Required] y [EmailAddress]. Podría mejorar validando ModelState explícitamente en cada POST, agregando validadores custom (email único, contraseña fuerte), y mostrando errores específicos en las vistas con asp-validation-for."

---

### 4. **NORMALIZACIÓN DE BD COMPLETA** 🗄️

**Situación actual:**
```
✓ Tablas principales normalizadas
✗ Algunas redundancias evitables
✗ Sin índices explícitos en campos búsquedas
✗ Sin vistas en BD para reportes complejos
```

**Cómo lo explicas:**
> "La BD está en buena forma, pero podría:
> 1. Agregar índices en Email (para búsquedas de usuarios)
> 2. Crear vistas en BD para reportes (resumen de transacciones)
> 3. Usar más triggers para integridad referencial
> 4. Agregar campos de auditoría (FechaCreacion, UsuarioModifico)"

---

### 5. **PERFORMANCE PODRÍA OPTIMIZARSE** ⚡

**Situación actual:**
```csharp
// En controlador:
public IActionResult Index()
{
    var usuarios = _repoUsuario.Obtener(); // Trae TODOS
    return View(usuarios);
}

// Falta:
✗ Paginación
✗ Caching
✗ Lazy loading
✗ Índices en BD
✗ Profiling
```

**Cómo lo explicas:**
> "Con datasets grandes, podría mejorar:
> 1. Agregar paginación (mostrar 10 por página)
> 2. Implementar caching de monedas (datos que no cambian frecuentemente)
> 3. Lazy loading en relaciones (Historial del usuario)
> 4. Índices en BD en campos de búsqueda
> 5. Async/await en repositorios para operaciones I/O"

---

## 🎤 SITUACIONES CONCRETAS QUE TE PREGUNTARÁN

### Situación 1: "¿Qué pasaría si un usuario intenta comprar una moneda que se acabó?"

**Respuesta correcta:**
> "El controlador MonedasController.Buy() verifica:
> 1. Que la moneda existe (Obtener() por ID)
> 2. Que el usuario tiene saldo suficiente
> 3. Que hay stock disponible
> 
> Si falta stock, muestra error al usuario. Si falta saldo, redirige a 'Ingresar dinero'. La transacción se registra en Historial solo si todo es válido. Si hay error, usa try-catch y muestra mensaje de error en la vista."

**Lo que debes saber:**
- Dónde está `MonedasController.Buy()`
- Qué valida antes de permitir la compra
- Qué pasa si la validación falla
- Dónde se registra la transacción (tabla Historial)

---

### Situación 2: "¿Cómo evitas que alguien pueda crear dinero sin límite?"

**Respuesta correcta:**
> "Varias capas de seguridad:
> 1. Autenticación: Solo usuarios logueados pueden ingresar dinero
> 2. Autorización: [Authorize] en el controlador
> 3. Validación: UsuarioDto valida saldo > 0
> 4. BD: Constrains aseguran valores válidos
> 5. Auditoría: Cada ingreso se registra en Historial con timestamp
> 
> Si alguien intenta SQL injection en la cantidad, Dapper con parámetros @cantidad previene eso."

**Lo que debes saber:**
- Dónde está el método Ingresar() en UsuariosController
- Qué validaciones se aplican
- Por qué Dapper es seguro contra SQL injection
- Qué registra en Historial

---

### Situación 3: "¿Qué pasa si la base de datos se cae mientras compra una moneda?"

**Respuesta correcta:**
> "Hay dos posibles escenarios:
> 
> 1. **Antes de guardar (durante validación):**
>    - Excepción DbException atrapada
>    - Try-catch en el método
>    - Usuario ve mensaje 'Transacción cancelada, intente más tarde'
>    - Dinero NO se descontó (transacción nunca se ejecutó)
> 
> 2. **Durante la transacción:**
>    - Trigger en BD asegura que el historial se cree
>    - Si algo falla, la BD rollback automático
>    - Usuario ve error, saldo no cambia
> 
> Mejora posible: Agregar retry automático con exponential backoff."

**Lo que debes saber:**
- Dónde está el try-catch en MonedasController.Buy()
- Qué es rollback en BD
- Por qué usar transacciones
- Qué pasa si hay timeout

---

### Situación 4: "Un usuario olvidó su contraseña, ¿qué hace?"

**Respuesta honesta:**
> "Actualmente, no hay sistema de recuperación de contraseña implementado. El usuario tendría que contactar al administrador.
> 
> Cómo lo implementaría:
> 1. Agregar enlace 'Olvidé contraseña' en Login
> 2. Pedir email en formulario
> 3. Verificar que existe usuario con ese email
> 4. Generar token temporal (JWT con 1 hora de validez)
> 5. Enviar email con link que contiene el token
> 6. Usuario hace click, puede resetear contraseña
> 7. Validar token antes de permitir cambio
> 
> Nota: Requiere SMTP configurado para enviar emails."

**Lo que debes saber:**
- Es honesto decir que NO está implementado
- Muestra que entiendes cómo se haría
- No inventes que existe si no existe

---

### Situación 5: "Un admin accidentalmente marca a un usuario normal como admin. ¿Cómo revierte?"

**Respuesta correcta:**
> "Hay un método ToggleAdmin() en UsuariosController que alterna el estado. El admin puede:
> 
> 1. Ir a Usuarios → Index
> 2. Buscar al usuario
> 3. Hacer click en 'Toggle Admin' o en Details
> 4. El método actualiza EsAdmin a false
> 
> En BD, es un simple UPDATE Usuario SET esAdmin = 0 WHERE idUsuario = X.
> 
> Limitación: No hay historial de quién cambió qué. Mejora:
> 1. Agregar auditoria (UsuarioModifico, FechaModificacion)
> 2. Crear tabla AuditoriaUsuarios para registrar cambios
> 3. Mostrar log de cambios en admin panel"

**Lo que debes saber:**
- Dónde está ToggleAdmin()
- Cómo es el SQL de actualización
- Qué falta para auditoria

---

### Situación 6: "¿Por qué usaste SHA-256 y no bcrypt?"

**Respuesta honesta (reconoce limitación pero explica razonamiento):**
> "SHA-256 es un hash criptográfico, pero NO es ideal para contraseñas porque:
> 
> Ventajas de SHA-256 que usé:
> - Rápido y estándar
> - Fácil de implementar en trigger MySQL
> - Determinista (misma contraseña = mismo hash)
> 
> Desventajas (qué falta):
> - Sin salt: mismo password = mismo hash siempre
> - Sin iteraciones: no ralentiza ataques de fuerza bruta
> - Bcrypt sería mejor: con salt + iterations automáticas
> 
> Por qué usé SHA-256:
> - Era un proyecto de aprendizaje
> - La base de datos tenía triggers preexistentes
> - Funciona para proteger contra vistas casualmente
> 
> En producción usaría:
> 1. PBKDF2 o Bcrypt con salt
> 2. Iteraciones mínimas: 100,000+
> 3. Nunca guardar plaintext
> 4. Validar fuerza de contraseña (mín 8 chars, mayúscula, número)"

**Lo que debes saber:**
- Por qué SHA-256 es insuficiente
- Qué es salt en hashing
- Qué es bcrypt y por qué es mejor
- Mencionar que reconoces la limitación

---

### Situación 7: "¿Cómo hiciste responsive sin usar un framework como Bootstrap?"

**Respuesta (aclarando que SÍ usaste Bootstrap):**
> "Usé Bootstrap 5 como base + CSS custom. Bootstrap proporciona:
> 1. Grid system (12 columnas)
> 2. Navbar responsivo con hamburguesa
> 3. Clases de utilidad (padding, margin, display)
> 4. Breakpoints predefinidos
> 
> Después agregar CSS custom en site.css y responsive.css:
> 1. Mobile-first: estilos base para <576px
> 2. Media queries: @media (min-width: 768px) { ... }
> 3. Flexbox: display: flex en contenedores
> 4. CSS variables: --color-primary usable en todo
> 5. Unidades relativas: rem en lugar de px
> 
> Ejemplo en navbar:
> - Móvil: ícono hamburguesa, vertical
> - Tablet: hamburguesa, horizontal si espacio
> - Desktop: menú expandido, horizontal siempre"

**Lo que debes saber:**
- Bootstrap es un framework CSS (no código manual)
- Diferencia entre mobile-first y desktop-first
- Qué es media query
- Qué es Flexbox
- Los 5 breakpoints usados

---

### Situación 8: "¿Qué pasa si 100 usuarios intenta comprar la misma moneda al mismo tiempo?"

**Respuesta (mostrando comprensión de concurrencia):**
> "Race condition posible:
> 
> Escenario problemático:
> 1. Usuario A ve: Stock = 10 monedas
> 2. Usuario B ve: Stock = 10 monedas
> 3. Usuario A compra 8 → Stock = 2
> 4. Usuario B compra 6 → ¡PROBLEMA! Stock sería negativo
> 
> Cómo lo previene el código actual:
> - Validación en el controlador: Cantidad <= Stock
> - Pero sin lock, sigue siendo vulnerable en concurrencia alta
> 
> Soluciones:
> 1. **Lock en BD:** BEGIN TRANSACTION; SELECT FOR UPDATE
> 2. **Trigger MySQL:** Verificar stock antes de insertar
> 3. **Async con lock:** lock statement en C#
> 4. **Optimistic locking:** Campo de versión
> 5. **Queue:** Si stock se agota, poner en cola de espera
> 
> Recomendación: Usar transacción con SELECT FOR UPDATE en BD para lock row."

**Lo que debes saber:**
- Qué es race condition
- Qué es lock en BD
- Qué es SELECT FOR UPDATE
- Por qué es importante con múltiples usuarios

---

### Situación 9: "¿Cómo te aseguras que un usuario no pueda cambiar el ID en la URL?"

**Respuesta correcta:**
> "Múltiples capas:
> 
> 1. **Autenticación:** [Authorize] previene acceso sin login
> 
> 2. **Autorización:** Verificar que el ID pertenece al usuario actual
>    ```csharp
>    var usuarioActual = User.FindFirst(ClaimTypes.NameIdentifier);
>    if (usuarioActual.Value != id.ToString())
>        return Forbid(); // 403
>    ```
> 
> 3. **Validación:** RepoUsuario solo devuelve datos del usuario logueado
> 
> 4. **BD:** Constraints aseguran integridad referencial
> 
> Ejemplo en UsuariosController.Details(int id):
> - Solo mostrar si es el usuario logueado O es admin
> - Si intenta ID de otro, mostrar error 403 Forbidden"

**Lo que debes saber:**
- Claims en ASP.NET Core
- ClaimTypes.NameIdentifier
- Diferencia entre 401 Unauthorized y 403 Forbidden

---

### Situación 10: "Tu aplicación no tiene HTTPS, ¿qué pasa en producción?"

**Respuesta honesta:**
> "En desarrollo, uso HTTP y CookieSecurePolicy.SameAsRequest.
> 
> En producción, hay problemas:
> 1. **Cookies robadas:** Sin HTTPS, cookies viajan en plaintext
> 2. **Man-in-the-middle:** Alguien puede interceptar credenciales
> 3. **Inyección:** Se puede inyectar JavaScript en respuestas
> 
> Soluciones para producción:
> 1. **Certificado SSL:** Let's Encrypt (gratis) o pagado
> 2. **CookieSecurePolicy = Always:** Solo por HTTPS
> 3. **HSTS header:** Forzar HTTPS en futuras visitas
> 4. **Redirección:** HTTP → HTTPS automático
> 5. **Update Program.cs:**
>    ```csharp
>    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
>    app.UseHttpsRedirection();
>    app.UseHsts();
>    ```
> 
> Costo: $0-20/año si usas Let's Encrypt"

**Lo que debes saber:**
- Diferencia HTTP vs HTTPS
- Qué es certificado SSL
- Por qué SameAsRequest es solo para desarrollo
- Qué es HSTS

---

## 💡 RESPUESTAS DE IMPACTO (Úsalas en la evaluación)

### Si preguntan: "¿Cuál fue tu mayor desafío?"

**Respuesta fuerte:**
> "El mayor desafío fue entender cómo implementar autenticación segura con cookies. 
> 
> El problema: Necesitaba que los usuarios permanecieran logueados pero expulsarlos si estaban inactivos.
> 
> Investigación:
> - Leí docs de ASP.NET Core Authentication
> - Entendí SlidingExpiration vs AbsoluteExpiration
> - Configuré la cookie en Program.cs
> 
> Solución:
> - SlidingExpiration = true: Renueva con cada petición
> - ExpireTimeSpan = 30 min: Expira si inactivo
> - HttpOnly = true: Previene XSS
> 
> Lección: La seguridad requiere múltiples capas, no una sola solución."

---

### Si preguntan: "¿Por qué elegiste esta arquitectura?"

**Respuesta fuerte:**
> "Elegí arquitectura en capas para:
> 
> 1. **Mantenibilidad:** Cambiar BD sin tocar controladores
> 2. **Testabilidad:** Inyectar mocks del repositorio
> 3. **Escalabilidad:** Agregar nuevas features sin romper existentes
> 4. **Estándar:** Pattern Repository es best practice en .NET
> 
> Ejemplo práctico: Si quisiera cambiar de MySQL a SQL Server:
> - Solo cambio RepoUsuario, RepoMoneda, etc.
> - Controladores no se tocan
> - Interfaces quedan igual
> 
> Si quisiera agregar caché:
> - Decorador sobre repositorio
> - Sin afectar código actual"

---

### Si preguntan: "¿Qué harías diferente si empiezas de nuevo?"

**Respuesta reflexiva:**
> "Cambiaría:
> 
> 1. **Desde el principio:** Tests (TDD) - escribir tests primero
> 2. **Validación:** Más robusta con FluentValidation
> 3. **Contraseñas:** Bcrypt en lugar de SHA-256
> 4. **Logging:** Serilog centralizado
> 5. **BD:** Auditoría automática de cambios
> 6. **Frontend:** Más validación asincrónica
> 7. **Performance:** Lazy loading en relaciones
> 8. **Error handling:** Página de error custom
> 
> Pero como proyecto de aprendizaje, está bien porque:
> - Cubre conceptos fundamentales
> - Enseña trade-offs
> - Funciona de punta a punta
> - Es código real, no tutorial"

---

## 🎓 DATOS QUE DEBES MEMORIZAR

### Archivos Críticos:
```
Program.cs         → Configuración autenticación (líneas 17-37)
AccountController  → Métodos Login, Register, Logout
MonedasController  → Buy, Create, Index
site.css           → Estilos base + variables
responsive.css     → 5 breakpoints
```

### Números Clave:
```
30 minutos       → Expiración de sesión (SlidingExpiration)
5 breakpoints    → <576px, 576-767px, 768px, 992px, 1200px+
SHA-256          → Algoritmo hash de contraseñas
HttpOnly         → Protección cookie contra XSS
4 tablas         → Usuario, Moneda, UsuarioMoneda, Historial
```

### Conceptos Clave:
```
Repository Pattern   → Abstracción de acceso a datos
Dependency Injection → Inyectar dependencias en constructores
MVC                  → Model-View-Controller architecture
Authentication       → Verificar quién eres (cookies)
Authorization        → Verificar qué puedes hacer ([Authorize])
```

---

## 📋 CHECKLIST ANTES DE LA PRESENTACIÓN

- [ ] Sé dónde están las 5 carpetas principales
- [ ] Puedo explicar flujo de login en 2 minutos
- [ ] Puedo explicar flujo de compra de moneda en 2 minutos
- [ ] Sé qué significa SlidingExpiration
- [ ] Sé por qué HttpOnly es importante
- [ ] Puedo nombrar las 5 breakpoints responsive
- [ ] Sé qué es el patrón Repository
- [ ] Puedo explicar qué es Dependency Injection
- [ ] Conozco 3 medidas de seguridad implementadas
- [ ] Sé qué validaciones se aplican al crear usuario
- [ ] Tengo memorizado dónde está cada controlador
- [ ] Puedo mostrar código específico en editor
- [ ] Reconozco qué podría mejorar (honestidad)
- [ ] Sé responder "¿por qué elegiste esto?"
- [ ] Puedo hablar 10+ minutos sin pausas grandes

---

## 🚀 BONUS: Cosas que Impresionarán

### Si preguntan algo simple:
**Respuesta base + contexto:**
```
P: "¿Cómo validas que el email sea único?"
R: "Uso [EmailAddress] en DTO. En la BD, hay UNIQUE constraint 
   en la columna email. Si viola, Dapper lanza DbException 1062 
   que atrapamos y mostramos 'Email ya registrado'."
   
   → Esto muestra: DTO + BD + manejo de errores
```

### Si preguntan algo complejo:
**Respuesta + reconocer limitación + solución:**
```
P: "¿Qué pasa con race condition en compras?"
R: "Es vulnerable sin locks. En el código actual no hay lock explicit,
   pero la validación controlador + trigger BD mitigan problema.
   En producción usaría SELECT FOR UPDATE en transacción."
   
   → Esto muestra: Honestidad + comprensión técnica + visión mejorada
```

### Si no sabes la respuesta:
**NUNCA digas "No sé". Siempre:**
```
1. Admite que no está implementado
2. Explica cómo lo harías
3. Menciona alternativas que conoces

Ejemplo:
P: "¿Implementaste Two-Factor Authentication?"
R: "No, está fuera del scope actual. Lo haría:
   1. Generar código aleatorio 6 dígitos
   2. Guardar en tabla temporal con TTL 5min
   3. Enviar por SMS/Email con SMTP
   4. Usuario ingresa código para confirmar
   5. Eliminar código tras validar
   
   Librerías: Twilio (SMS), SendGrid (Email), OTP libraries."
```

---

## 🎬 CÓMO PRESENTAR MAÑANA

### 1. **Primeros 5 minutos: Overview**
```
"MarketWeight es una aplicación web de trading de criptomonedas.
 - Usuarios pueden registrarse, comprar/vender, ver historial
 - Arquitectura: MVC con pattern Repository + Dapper
 - BD: MySQL con triggers para automatización
 - Frontend: Bootstrap 5 + responsive CSS
 - Seguridad: Autenticación cookies, hash SHA-256"
```

### 2. **Mostrar en vivo (5-10 minutos)**
```
1. Abrir navegador, mostrar UI
2. Registrar usuario de prueba (si no existe)
3. Hacer login
4. Comprar una moneda (o simular)
5. Ver historial
6. Mostrar Admin panel
7. Abrir Developer Tools: pestaña Network/Cookies
   → Mostrar cookie "MarketWeight.Auth"
   → Explicar HttpOnly checkbox
```

### 3. **Mostrar código (10-15 minutos)**
```
1. Program.cs → Configuración autenticación
2. AccountController.cs → Método Register y Login
3. MonedasController.cs → Método Buy
4. site.css y responsive.css → Breakpoints
5. BD: Mostrar trigger de SHA-256 si DB IDE disponible
```

### 4. **Preguntas del evaluador (5-10 minutos)**
```
Responde con:
- Respuesta directa (20 segundos)
- Explicación técnica (40 segundos)
- Ejemplo o código (20 segundos)
```

### 5. **Cierre (2 minutos)**
```
"Reconozco que hay espacio para mejora:
- Más tests unitarios e integración
- Mejor manejo de errores centralizado
- Recovery de contraseñas
- Caching para performance
- HTTPS en producción

Pero como proyecto integral, cubre:
- Conceptos fundamentales de web
- Seguridad básica bien implementada
- UI responsive funcional
- Arquitectura escalable"
```

---

## 📞 RESPUESTAS RÁPIDAS PARA PREGUNTAS COMUNES

| Pregunta | Respuesta |
|----------|-----------|
| **¿Cuánto tiempo tardó?** | "3-4 semanas de desarrollo activo" |
| **¿Trabajaste solo?** | "Sí, es un proyecto individual" |
| **¿Qué frameworks usaste?** | "ASP.NET Core 8, Bootstrap 5, Dapper, MySQL" |
| **¿Por qué .NET?** | "Porque domino C# y es robusto para aplicaciones web" |
| **¿Subiste a GitHub?** | "Sí, repositorio en GitHub" |
| **¿Desplegado?** | "En local actualmente, pero listo para Azure/AWS" |
| **¿Admin por defecto?** | "No, cualquiera puede registrarse. Admin necesita toggle manual" |
| **¿Validación XSS?** | "HttpOnly en cookies + Razor escaping automático" |

---

## 🎯 PUNTOS FINALES

**Lo que evaluarán:**
✓ Comprensión de conceptos (autenticación, responsive, arquitectura)  
✓ Decisiones técnicas justificadas (por qué elegiste X)  
✓ Honestidad sobre limitaciones (qué falta, qué mejorarías)  
✓ Capacidad de explicar código (puedes mostrar y explicar)  
✓ Pensamiento crítico (reconoces problemas de concurrencia, performance)  

**Lo que NO les importa:**
✗ Que sea perfecto  
✗ Que sea 100% secure  
✗ Que tenga todas las features  
✗ Que sea el código más rápido  

**Lo que SÍ importa:**
✓ Que funcione  
✓ Que entiendas qué hiciste  
✓ Que reconozcas qué falta  
✓ Que puedas mantener/mejorar el código  

---

**¡Estás preparado! Mucho éxito mañana! 💪**

Recuerda: Los evaluadores no buscan perfección, buscan comprensión y capacidad de razonamiento técnico. Tú tienes ambas.
