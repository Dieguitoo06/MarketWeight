# ⚡ ÚLTIMA HORA - Resumen Visual para Mañana

> Lee esto 15 minutos antes de presentar. Recuerda lo más importante.

---

## 🎯 EN 30 SEGUNDOS

**"MarketWeight es una app web de trading de criptomonedas. Usuarios compran/venden monedas, admins las crean. Hecha con ASP.NET Core, Bootstrap, MySQL. Autenticación segura con cookies, responsivo, arquitectura en capas."**

---

## 📱 RESPONSIVE - PARA EXPLICAR EN 1 MINUTO

```
Mobile-first + Bootstrap 5 + 5 breakpoints

<576px      │ 576-768px │ 768-992px │ 992-1200px │ 1200px+
(móvil)     │ (tablet)  │ (tablet)  │ (desktop)  │ (desktop+)
            │           │           │            │
Hamburguesa │ Hamburguesa│ Menu + Items| Full menu │ Full menu
Stack       │ Stack     │ Flex      │ Grid      │ Grid
            │           │           │            │
```

**Lo importante:** "Mobile-first: primero estilos para móvil, luego media queries para pantallas mayores."

---

## 🔐 AUTENTICACIÓN - PARA EXPLICAR EN 1 MINUTO

```
REGISTRO                 LOGIN                    SOLICITUD
─────────────────────────────────────────────────────────────────
Email + Pass    →  Valida  →  Hash SHA-256  →  BD
                    DTO       (plaintext)         (hashado)
                              
                             ↓
                    Si ok, redirige a login


                             ↓
                    EMAIL + PASSWORD
                         ↓
                    Verifica hash
                    Si coincide:
                         ↓
                    Crea Claims (ID, Email, Rol)
                         ↓
                    SignInAsync()
                         ↓
                    Cookie firmada
                    HttpOnly: ✓
                    Expira: 30 min inactividad
```

**Lo importante:** "SlidingExpiration: renueva cada petición, expira si 30 min inactivo."

---

## 💰 COMPRAR MONEDA - PARA EXPLICAR EN 1 MINUTO

```
USUARIO QUIERE COMPRAR 2 BITCOIN A $50,000

Costo = 2 × $50,000 = $100,000

VALIDACIONES:
├─ ¿Usuario logueado?         → SÍ
├─ ¿Moneda existe?            → SÍ
├─ ¿Stock >= 2?               → SÍ (hay 10)
├─ ¿Saldo >= $100,000?        → SÍ (tiene $150,000)
└─ ✅ Compra APROBADA

RESULTADO:
├─ Saldo usuario: $150,000 - $100,000 = $50,000
├─ Stock moneda:  10 - 2 = 8
├─ Historial:     Nueva entrada (fecha, moneda, cantidad, precio)
└─ UsuarioMoneda: 2 bitcoins agregados a billetera
```

**Lo importante:** "Validación en capas: autorización, existencia, stock, saldo."

---

## 🏗️ ARQUITECTURA - PARA EXPLICAR EN 1 MINUTO

```
FRONTEND                       BACKEND                        BD
────────────────────────────────────────────────────────────────

HTML + CSS           MVC Controller           Repository       MySQL
Bootstrap ────────→ (autorización)  ───────→  (acceso datos) ───→ Tablas
Responsive          (validación)             (Dapper)          Usuarios
                    (lógica)                                    Monedas
                                                                Historial
                                                                UsuarioMoneda
                                      ↑
                                      │
                                    Core
                                  (Modelos)
```

**Lo importante:** "3 capas: Controller (MVC), Repository (Dapper), Core (Modelos). Fácil de cambiar, testeable."

---

## 🔒 SEGURIDAD - 4 CAPAS

```
XSS           SQL INJECTION      AUTENTICACIÓN        AUTORIZACIÓN
────────────────────────────────────────────────────────────────
HttpOnly      Parámetros         Cookie              [Authorize]
Cookies       Dapper             SlidingExpiration   rol Admin
              @email, @password  SHA-256 hash        Claims
              (valor, no SQL)    30 min expiry       Usuario ID
```

---

## ❌ RECONOCE ESTO

**Si preguntan "¿Qué no tienes?":**

> "Reconozco que falta:
> 1. Recovery de contraseña - Con token temporal sería fácil
> 2. Más tests - Tengo unitarios, necesito integración
> 3. Auditoría - Quién cambió qué y cuándo
> 4. Caché - Para mejorar performance
> 5. HTTPS - En producción es obligatorio
> 
> Pero todo es mejorable y sé cómo hacerlo."

**NO digas:** "Es perfecto" ❌  
**SÍ di:** "Aquí hubo trade-offs y estos son los posibles mejoras" ✅

---

## 💬 FRASES DE IMPACTO

### Úsalas cuando corresponda:

**"Separé en capas porque..."**
> "...si mañana cambio de MySQL a SQL Server, solo cambio Repository. Controladores no se tocan."

**"Usé SlidingExpiration porque..."**
> "...balance entre seguridad (expira si inactivo) y usabilidad (no expira si estoy activo)."

**"Validé en DTO y BD porque..."**
> "...defensa en profundidad. Si alguien bypasea validación frontend, BD la detiene."

**"Parámetros en Dapper porque..."**
> "...Dapper trata @email como VALOR, no código SQL. Aunque tenga comillas, es seguro."

**"HttpOnly en cookies porque..."**
> "...previene XSS. JavaScript no accede a la cookie, así que aunque haya XSS, no roba sesión."

---

## 📊 NÚMEROS CLAVE (MEMORIZA)

```
30 minutos      = SlidingExpiration timeout
5               = Breakpoints responsive
6               = Controladores
17              = Vistas
4               = Tablas principales
SHA-256         = Algoritmo hash
100%            = Validación en servidor
```

---

## 🚀 DEMO - 8 MINUTOS

```
1. Homepage                (20 seg)  → Muestra responsive
2. Registrar usuario       (1 min)   → Muestra DTO + BD
3. Login                   (1 min)   → Muestra cookie en DevTools
4. Ver monedas             (30 seg)  → Lista desde BD
5. Ingresar dinero         (1 min)   → Actualiza saldo
6. Comprar moneda          (1 min)   → Validaciones, transacción
7. Ver historial           (30 seg)  → Registro de compras
8. Admin crear moneda      (1 min)   → [Authorize], DTO validation
```

---

## 🎓 PREGUNTAS CLAVE

### P1: "¿Cómo responsive?"
**R:** "Mobile-first + 5 media queries. Bootstrap + Flexbox + CSS variables."

### P2: "¿Cómo auténtico?"
**R:** "Cookies con SlidingExpiration 30 min, HttpOnly, SHA-256 hash."

### P3: "¿Cómo registrar usuario?"
**R:** "DTO valida, hash contraseña, BD rechaza email duplicado (UNIQUE constraint)."

### P4: "¿Cómo compra?"
**R:** "Valida: logueado, moneda existe, stock, saldo. Si todo ok, transacción en Historial."

### P5: "¿Cómo prevines XSS/SQLi?"
**R:** "HttpOnly + Razor escaping / Dapper parámetros seguros."

---

## ✅ ANTES DE ENTRAR

- [ ] Código compilado
- [ ] BD levantada
- [ ] App en http://localhost:5000
- [ ] Usuario de test creado
- [ ] Respiré hondo

---

## 🎬 DURANTE LA PRESENTACIÓN

**HABLA CON ESTOS RITMOS:**

```
Introducción    → Lento y claro (3-4 palabras por segundo)
Demo            → Normal, deja que se vea (pausa para que vean)
Código          → Rápido en navigation, lento explicando
Preguntas       → Pausa 1 segundo antes de responder (piensa)
```

---

## 🏁 CIERRE

**Después de responder preguntas, di:**

> "Para resumir: MarketWeight es un proyecto funcional que muestra:
> - Conceptos fundamentales de web (MVC, autenticación, DB)
> - Buenas prácticas (seguridad, validación, separación de capas)
> - Capacidad de pensar críticamente (reconozco mejoras)
> 
> El código es mantenible, escalable y sé explicar cada decisión. Gracias."

---

## 🎉 LISTO

**Tienes TODO preparado. Ahora solo:**

1. Descansa bien hoy
2. Llega 10 min temprano mañana
3. Abre tu laptop y compila una vez (por seguridad)
4. Respira
5. Entra con confianza

**¡Te va a salir bien! 💪**

---

*Última actualización: 17 de noviembre de 2025, 11:59 PM*

**Próxima lectura:** Lee esto 15 min antes de presentar. No más.
