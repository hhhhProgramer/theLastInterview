# Game Design Document (GDD)
## The Last Job Interview Simulator

**Versión:** 1.0  
**Fecha:** 2024  
**Motor:** Godot Engine 4.4.1  
**Lenguaje:** C# (.NET 8.0)

---

## 1. Concepto General

### 1.1 Visión del Juego

**The Last Job Interview Simulator** es un juego 2D narrativo de decisiones en el que el jugador pasa por una "entrevista laboral final" completamente absurda e irreverente. El juego satiriza el proceso de contratación corporativo mediante preguntas exageradas, respuestas hilarantes y un sistema de estados ocultos que determina el destino del jugador.

### 1.2 Género y Plataforma

- **Género:** Narrativa interactiva / Simulador satírico
- **Plataforma:** PC (Windows, Linux, macOS)
- **Estilo:** 2D Pixel Art minimalista
- **Duración estimada:** 15-30 minutos por partida

### 1.3 Público Objetivo

- Jugadores que disfrutan de humor negro y sátira
- Personas que han pasado por procesos de entrevista laboral
- Aficionados a juegos narrativos cortos y rejugables
- Jugadores casuales que buscan experiencias únicas

### 1.4 Propuesta de Valor Única

- **Humor irreverente:** Preguntas que satirizan la cultura corporativa tóxica
- **Múltiples endings:** Sistema de estados ocultos que genera diferentes finales
- **Rejugabilidad:** Diferentes combinaciones de respuestas llevan a diferentes resultados
- **Estética minimalista:** Pixel art simple pero efectivo que transmite la atmósfera deprimente

---

## 2. Mecánicas Principales

### 2.1 Sistema de Decisión

El jugador se enfrenta a una serie de preguntas del entrevistador. Cada pregunta tiene entre 2 y 4 opciones de respuesta.

**Flujo básico:**
1. El entrevistador formula una pregunta
2. Se muestran las opciones de respuesta (botones)
3. El jugador selecciona una respuesta
4. El sistema procesa la respuesta y actualiza estados ocultos
5. Se muestra una reacción del entrevistador (visual y/o textual)
6. Se avanza a la siguiente pregunta o al ending

### 2.2 Sistema de Estados Ocultos

El juego mantiene un sistema de estados internos que determina el final. Los estados no se muestran explícitamente al jugador, pero influyen en las reacciones y el ending final.

#### Estados Principales:

1. **Normal** (0-30 puntos)
   - Todo sigue relativamente "profesional"
   - El entrevistador mantiene la compostura
   - Preguntas estándar

2. **Tenso** (31-60 puntos)
   - El entrevistador empieza a sospechar del jugador
   - Preguntas se vuelven más agresivas
   - Reacciones visuales más intensas

3. **Caos** (61-90 puntos)
   - El entrevistador pierde la cordura
   - Preguntas completamente absurdas
   - Efectos visuales exagerados (shake, parpadeos)

4. **Contratado por Error** (91-100 puntos, respuestas absurdas pero "encajan")
   - El jugador respondió de forma absurda pero "encajó" en la cultura tóxica
   - Ending especial

5. **Expulsado Violentamente** (91-100 puntos, respuestas muy agresivas)
   - Una respuesta del jugador hizo que el entrevistador renunciara o explotara
   - Ending especial

#### Puntuación por Respuesta:

Cada respuesta tiene valores ocultos que afectan los estados:
- **Profesional:** +5 Normal, -5 Caos
- **Absurda pero coherente:** +10 Caos, +5 Normal
- **Agresiva/Extrema:** +15 Caos, -10 Normal
- **Zen/Relajada:** +10 Normal, -5 Caos
- **Sociopática:** +20 Caos, -15 Normal

### 2.3 Sistema de Preguntas Especiales

Algunas respuestas desbloquean preguntas especiales que solo aparecen bajo ciertas condiciones:

- **Preguntas de seguimiento:** Basadas en respuestas anteriores
- **Preguntas de estado:** Solo aparecen si el estado es Tenso o Caos
- **Preguntas secretas:** Desbloqueadas por combinaciones específicas de respuestas

### 2.4 Sistema de Endings

El ending final se determina por:
- Estado final acumulado
- Combinación específica de respuestas clave
- Número de preguntas respondidas
- Respuestas a preguntas especiales

---

## 3. Estilo Visual

### 3.1 Estética General

**Estilo:** 2D Pixel Art minimalista y deprimente  
**Paleta de colores:** Tonos apagados, grises, verdes fosforescentes  
**Atmósfera:** Oficina genérica pero deprimente, exagerada

### 3.2 Descripción del Fondo Único

El juego tiene un único fondo estático que representa una oficina genérica pero deprimente:

#### Elementos del Fondo:

1. **Mesa de madera gastada**
   - Madera oscura y desgastada
   - Una esquina rota visible
   - Texturas pixeladas que muestran el desgaste

2. **Monitor CRT verde fosforescente**
   - Monitor antiguo encendido
   - Pantalla verde con texto parpadeante: "CURRÍCULUM ANALIZANDO…"
   - Efecto de parpadeo constante
   - Brillo verde que ilumina ligeramente la escena

3. **Archiveros con cajones chuecos**
   - Archiveros metálicos grises
   - Algunos cajones abiertos o desalineados
   - Detalles pixelados que muestran el abandono

4. **Ventilador de techo detenido**
   - Ventilador de techo visible
   - Solo una hélice (las demás rotas o faltantes)
   - Símbolo de abandono y desgaste

5. **Planta muerta inclinada**
   - Maceta con una planta completamente muerta
   - Inclinada hacia un lado
   - Hojas marrones y secas pixeladas

6. **Ventana de fondo**
   - Vista a un edificio gris
   - Una nube muy triste pixelada en el cielo
   - Cielo grisáceo y deprimente

### 3.3 Diseño del Entrevistador

**Silueta pixelada sin rostro:**
- Forma humana básica en silueta negra
- Sin detalles faciales excepto:
  - **Ojos blancos brillando:** Dos puntos blancos que brillan
  - Los ojos pueden cambiar de tamaño según el estado (normal, abiertos, muy abiertos)

**Corbata roja exageradamente larga:**
- Corbata roja brillante
- Extremadamente larga (llega hasta el suelo o más)
- Elemento visual distintivo y absurdo

**Posición:**
- Sentado al otro lado de la mesa
- Frente al jugador (cámara desde la perspectiva del jugador)

### 3.4 Efectos Visuales

#### Reacciones Mínimas:

1. **Ojos del entrevistador:**
   - **Normal:** Tamaño estándar
   - **Sorpresa:** Se abren más (1.5x tamaño)
   - **Shock:** Se abren mucho (2x tamaño)
   - **Enojo:** Se estrechan (0.7x tamaño)

2. **Monitor CRT:**
   - Parpadeo constante del texto
   - Cambio de texto según el estado:
     - Normal: "CURRÍCULUM ANALIZANDO…"
     - Tenso: "ANÁLISIS PROFUNDO…"
     - Caos: "ERROR: SISTEMA CORRUPTO"

3. **Efecto de Shake:**
   - Cuando la respuesta es muy caótica
   - La cámara tiembla ligeramente
   - Duración: 0.3-0.5 segundos
   - Intensidad: 5-10 píxeles

4. **Efectos de Ending:**
   - Transiciones visuales según el ending
   - Glitch effects para endings especiales
   - Fade in/out para transiciones

---

## 4. Flujo de Juego

### 4.1 Estructura General

```
Pantalla de Inicio
    ↓
[Iniciar Entrevista]
    ↓
Fondo aparece + Entrevistador en silueta
    ↓
Primera pregunta absurda
    ↓
Jugador elige respuesta
    ↓
Reacción visual/textual
    ↓
Siguiente pregunta (o pregunta especial)
    ↓
[Repetir hasta última pregunta]
    ↓
Última pregunta
    ↓
Ending basado en elecciones
    ↓
Pantalla final con resultado
    ↓
[Volver a empezar] / [Salir]
```

### 4.2 Pantalla de Inicio

**Elementos:**
- Título del juego: "The Last Job Interview Simulator"
- Subtítulo: "Sobrevive la entrevista... o no"
- Botón: "Iniciar Entrevista"
- Botón: "Salir"
- Fondo: Versión más oscura del fondo de oficina (opcional)

### 4.3 Escena Principal de Entrevista

**Layout:**
- Fondo: Oficina deprimente (único fondo)
- Entrevistador: Silueta con ojos brillantes, sentado al otro lado de la mesa
- Panel de texto: Muestra la pregunta del entrevistador
- Botones de respuesta: 2-4 botones con las opciones
- Monitor CRT: Visible en el fondo con texto parpadeante

**UI Elements:**
- **Panel de pregunta:** Panel superior o central con el texto de la pregunta
- **Botones de respuesta:** Botones en la parte inferior o lateral
- **Indicador visual del estado:** Sutil (opcional, puede ser solo visual sin texto)

### 4.4 Sistema de Preguntas

**Número de preguntas:** 8-12 preguntas por partida  
**Selección de preguntas:** 
- Preguntas base siempre aparecen
- Preguntas especiales aparecen según condiciones
- Orden puede variar ligeramente según respuestas

**Progresión:**
- Preguntas iniciales: Más "normales" pero ya absurdas
- Preguntas medias: Absurdas y agresivas
- Preguntas finales: Completamente caóticas o especiales según estado

### 4.5 Pantalla de Ending

**Elementos:**
- Título del ending (ej: "Contratado por Error")
- Descripción del ending (texto narrativo)
- Imagen o efecto visual del ending
- Botón: "Volver a empezar"
- Botón: "Salir"

---

## 5. Lista de Preguntas

### 5.1 Preguntas Base (Siempre Aparecen)

1. **"Si tu jefe te pide que le cuides el perro… pero el perro te odia… ¿qué haces?"**
   - Opción A: "Le explico que no soy veterinario" (Profesional)
   - Opción B: "Negocio un aumento de sueldo primero" (Absurda pero coherente)
   - Opción C: "Me convierto en el perro" (Absurda extrema)
   - Opción D: "Le digo que el perro ya no existe" (Sociopática)

2. **"Define tu ética laboral con un sonido."**
   - Opción A: "*Silencio incómodo*" (Zen/Relajada)
   - Opción B: "*Sonido de máquina de escribir*" (Profesional)
   - Opción C: "*Grito primitivo*" (Agresiva/Extrema)
   - Opción D: "*Sonido de alarma de incendio*" (Absurda pero coherente)

3. **"En una escala del 1 al 'demándame', ¿cuánto estrés soportas?"**
   - Opción A: "Un 7, pero con beneficios" (Profesional)
   - Opción B: "Demándame, pero con café ilimitado" (Absurda pero coherente)
   - Opción C: "Infinito, soy inmortal" (Absurda extrema)
   - Opción D: "No soporto estrés, solo lo genero" (Agresiva/Extrema)

4. **"¿Qué harías si descubres que yo, tu entrevistador, no existo y estoy siendo simulado desde 2009?"**
   - Opción A: "Reportaría el bug a IT" (Profesional)
   - Opción B: "Te liberaría de la simulación" (Zen/Relajada)
   - Opción C: "Me convertiría en tu programador" (Absurda pero coherente)
   - Opción D: "Te eliminaría del código" (Sociopática)

5. **"Nuestro equipo valora el trabajo en equipo. ¿Cuánto estarías dispuesto a cargar físicamente a tus compañeros?"**
   - Opción A: "Hasta 50 kilos, con certificado médico" (Profesional)
   - Opción B: "Solo los días de pago" (Absurda pero coherente)
   - Opción C: "Hasta que se rompan mis brazos" (Agresiva/Extrema)
   - Opción D: "Solo si me pagan por kilo" (Absurda extrema)

6. **"¿Puedes explicarme por qué llegaste temprano? Aquí eso nos pone nerviosos."**
   - Opción A: "Me disculpo, llegaré tarde mañana" (Profesional)
   - Opción B: "Vine a espiar la competencia" (Absurda pero coherente)
   - Opción C: "Soy un robot, no entiendo el tiempo" (Absurda extrema)
   - Opción D: "Llegué ayer y esperé aquí toda la noche" (Sociopática)

7. **"Describe un conflicto laboral que hayas resuelto… con violencia o sin violencia, tú decides."**
   - Opción A: "Mediación y comunicación" (Profesional)
   - Opción B: "Violencia psicológica cuenta?" (Absurda pero coherente)
   - Opción C: "Sí, con violencia, pero fue en defensa propia" (Agresiva/Extrema)
   - Opción D: "No resuelvo conflictos, los creo" (Sociopática)

8. **"¿Has robado papel del baño en un trabajo? Sé honesto, esto es HR, no la policía."**
   - Opción A: "No, siempre traje el mío" (Profesional)
   - Opción B: "Solo el papel de calidad" (Absurda pero coherente)
   - Opción C: "Robé el baño completo" (Absurda extrema)
   - Opción D: "Soy el que vende el papel robado" (Sociopática)

9. **"Tu CV dice que sabes 'trabajo bajo presión'. ¿Qué presión? ¿Barras de presión? ¿Emocional? ¿Hidráulica?"**
   - Opción A: "Presión emocional y de deadlines" (Profesional)
   - Opción B: "Todas las anteriores, más presión atmosférica" (Absurda pero coherente)
   - Opción C: "Presión de una prensa hidráulica" (Absurda extrema)
   - Opción D: "Presión para que renuncies" (Agresiva/Extrema)

10. **"Si tuvieras que elegir entre salvar a tu compañero o salvar el servidor, ¿qué elegirías?"**
    - Opción A: "Intentaría salvar ambos" (Profesional)
    - Opción B: "El servidor, tiene más memoria RAM" (Absurda pero coherente)
    - Opción C: "Mi compañero, pero solo si tiene backup" (Absurda extrema)
    - Opción D: "Destruiría ambos para empezar de cero" (Sociopática)

### 5.2 Preguntas Especiales (Condicionales)

#### Preguntas de Estado Tenso:

11. **"Noto que estás muy tranquilo. ¿Eres un psicópata o solo estás drogado?"**
    - Solo aparece si el estado es Tenso y el jugador ha dado respuestas muy calmadas

12. **"Tu última respuesta me preocupa. ¿Tienes algún familiar en la empresa?"**
    - Solo aparece si el estado es Tenso y el jugador dio una respuesta agresiva

#### Preguntas de Estado Caos:

13. **"El sistema detectó una anomalía en tus respuestas. ¿Eres humano o un bot mal programado?"**
    - Solo aparece si el estado es Caos

14. **"HR me acaba de decir que deje de hacer preguntas. ¿Qué hago ahora?"**
    - Solo aparece si el estado es Caos extremo

#### Preguntas Secretas (Combinaciones Específicas):

15. **"Si pudieras ser cualquier objeto de esta oficina, ¿cuál serías y por qué?"**
    - Desbloqueada si el jugador menciona objetos de la oficina en respuestas anteriores

16. **"El monitor dice que tu currículum es un loop infinito. ¿Es verdad?"**
    - Desbloqueada si el jugador da respuestas circulares o repetitivas

---

## 6. Endings

### 6.1 Endings Principales

#### 1. Contratado por Error
**Condición:** Estado final 91-100 puntos, mayoría de respuestas absurdas pero "encajan"  
**Descripción:** "Después de revisar tus respuestas completamente absurdas, el comité decidió que eres 'perfecto para nuestra cultura corporativa'. Has sido contratado. Felicidades, supongo."  
**Efecto visual:** El entrevistador se levanta y te da la mano (sombra), el monitor muestra "CONTRATADO"

#### 2. Despedido Antes de Empezar
**Condición:** Estado final 91-100 puntos, mayoría de respuestas agresivas/sociopáticas  
**Descripción:** "Una de tus respuestas hizo que el entrevistador renunciara en el acto. Has sido despedido antes de ser contratado. Logro desbloqueado."  
**Efecto visual:** El entrevistador desaparece, el monitor muestra "ERROR: ENTREVISTADOR NO DISPONIBLE"

#### 3. Contratado como Jefe
**Condición:** Estado final Caos, respuestas que muestran "liderazgo" absurdo  
**Descripción:** "El entrevistador se rindió y te nombró su jefe. Ahora eres el nuevo entrevistador. El ciclo continúa."  
**Efecto visual:** El entrevistador se levanta y te cede su silla, tú te conviertes en la silueta

#### 4. HR Simulation Found Corrupted
**Condición:** Combinación específica de respuestas que mencionan bugs, código, o simulación  
**Descripción:** "El sistema detectó una corrupción en la simulación de HR. Has sido contratado como el nuevo entrevistador. Bienvenido al loop infinito."  
**Efecto visual:** Efecto de glitch, pantalla parpadea, el monitor muestra "SIMULATION CORRUPTED"

#### 5. Final Zen
**Condición:** Estado final Normal, mayoría de respuestas zen/relajadas  
**Descripción:** "Tus respuestas tan relajadas hicieron que crean que eres un gurú corporativo. Has sido contratado como consultor de bienestar. Ironías del destino."  
**Efecto visual:** El entrevistador medita (sombra), el monitor muestra "ZEN MODE ACTIVATED"

#### 6. Expulsado Violentamente
**Condición:** Estado final Caos extremo, respuesta final muy agresiva  
**Descripción:** "Tu última respuesta fue tan extrema que el entrevistador llamó a seguridad. Has sido expulsado del edificio. Al menos fue memorable."  
**Efecto visual:** Shake intenso, el entrevistador desaparece, el monitor muestra "SECURITY ALERT"

#### 7. Ending Normal (Genérico)
**Condición:** Estado final Normal, respuestas balanceadas  
**Descripción:** "Después de una entrevista... interesante, el comité decidió que no eres el candidato adecuado. Gracias por tu tiempo."  
**Efecto visual:** El entrevistador se levanta y sale, el monitor muestra "REJECTED"

### 6.2 Endings Secretos (Opcionales)

#### Ending: El Entrevistador Eres Tú
**Condición:** Respuestas específicas que sugieren que el jugador es el entrevistador  
**Descripción:** "Plot twist: Tú eres el entrevistador entrevistándote a ti mismo. La simulación se rompió. ¿O siempre fue así?"

#### Ending: La Planta Revivió
**Condición:** Mencionar la planta muerta en múltiples respuestas de forma positiva  
**Descripción:** "Tus respuestas sobre la planta hicieron que reviviera. Has sido contratado como jardinero corporativo. La planta te agradece."

---

## 7. Sistema Técnico

### 7.1 Arquitectura del Juego

**Estructura de Escenas:**
- `MainMenu.tscn` - Pantalla de inicio
- `InterviewScene.tscn` - Escena principal de entrevista
- `EndingScene.tscn` - Pantalla de ending

**Sistema de Managers:**
- `InterviewManager.cs` - Maneja el flujo de preguntas y respuestas
- `StateManager.cs` - Maneja los estados ocultos y puntuación
- `QuestionSystem.cs` - Sistema de preguntas y selección
- `EndingManager.cs` - Determina y muestra endings

**Modelos de Datos:**
- `Question.cs` - Modelo de pregunta con opciones y valores
- `Answer.cs` - Modelo de respuesta con efectos en estados
- `GameState.cs` - Estado actual del juego (puntuación, estado actual)

### 7.2 Sistema de Preguntas

**Estructura de Pregunta:**
```csharp
public class Question
{
    public string Text { get; set; }
    public List<Answer> Answers { get; set; }
    public QuestionType Type { get; set; } // Base, Special, Secret
    public List<Condition> UnlockConditions { get; set; }
}
```

**Estructura de Respuesta:**
```csharp
public class Answer
{
    public string Text { get; set; }
    public int NormalPoints { get; set; }
    public int ChaosPoints { get; set; }
    public AnswerType Type { get; set; } // Professional, Absurd, Aggressive, etc.
    public string ReactionText { get; set; } // Texto de reacción del entrevistador
}
```

### 7.3 Sistema de Estados

**Cálculo de Estado:**
- Cada respuesta suma/resta puntos a `NormalPoints` y `ChaosPoints`
- El estado se calcula: `TotalPoints = NormalPoints + ChaosPoints`
- El estado actual se determina por rangos de puntos

**Persistencia:**
- Los estados se mantienen en memoria durante la partida
- No hay guardado (cada partida es independiente)

### 7.4 UI y Controles

**Controles:**
- Mouse: Click en botones de respuesta
- Teclado: Números 1-4 para seleccionar respuestas (opcional)

**UI Elements:**
- Panel de pregunta (RichTextLabel)
- Botones de respuesta (Button o TextureButton)
- Monitor CRT (TextureRect con animación de parpadeo)
- Entrevistador (Sprite2D o AnimatedSprite2D)

---

## 8. Audio

### 8.1 Música

**Ambientación:**
- Música ambiental deprimente y repetitiva
- Loop corto que se repite
- Volumen bajo para no distraer del texto

**Temas:**
- Tema principal: Música de oficina genérica pero siniestra
- Tema de ending: Variaciones según el ending

### 8.2 Efectos de Sonido

**Efectos básicos:**
- Click de botón
- Parpadeo del monitor CRT
- Shake de cámara (sonido de vibración)
- Cambio de estado (transición sutil)

**Efectos de ending:**
- Sonido de éxito/fracaso según ending
- Glitch sound para endings especiales

### 8.3 Implementación

Usar el sistema de audio existente del proyecto:
- `AudioManager.cs` para música y efectos
- Configurar volúmenes individuales
- Usar `AudioEnums.cs` para categorías

---

## 9. Arte y Assets

### 9.1 Assets Necesarios

**Fondo:**
- 1 imagen de fondo de oficina (2560x1440 o escalable)
- Pixel art estilo deprimente

**Entrevistador:**
- 1 sprite de silueta (varias frames para animación de ojos)
- Animación de ojos (normal, sorpresa, shock, enojo)

**UI:**
- Botones de respuesta (estilo pixel art)
- Panel de pregunta (puede ser generado con código)
- Monitor CRT con texto (puede ser generado con código)

**Efectos:**
- Efecto de shake (código)
- Efectos de glitch (shader o código)
- Transiciones (código)

### 9.2 Especificaciones Técnicas

**Resolución base:** 2560x1440 (2K/QHD)  
**Estilo:** Pixel art, paleta limitada  
**Formato:** PNG para sprites, puede usar formatos comprimidos para fondo

---

## 10. Plan de Desarrollo

### 10.1 Fases de Desarrollo

#### Fase 1: Prototipo Básico (1-2 semanas)
- [ ] Implementar escena básica de entrevista
- [ ] Sistema de preguntas y respuestas básico
- [ ] UI mínima funcional
- [ ] 3-5 preguntas de prueba

#### Fase 2: Sistema Completo (2-3 semanas)
- [ ] Sistema de estados ocultos
- [ ] Todas las preguntas implementadas
- [ ] Sistema de endings
- [ ] Efectos visuales básicos

#### Fase 3: Pulido (1-2 semanas)
- [ ] Arte final (fondo, entrevistador)
- [ ] Audio completo
- [ ] Efectos visuales avanzados
- [ ] Balanceo de estados y endings

#### Fase 4: Testing y Release (1 semana)
- [ ] Testing de todos los endings
- [ ] Corrección de bugs
- [ ] Optimización
- [ ] Release

### 10.2 Prioridades

**Must Have (MVP):**
- Sistema de preguntas y respuestas
- Estados ocultos básicos
- 3-5 endings principales
- Fondo y entrevistador básicos
- UI funcional

**Should Have:**
- Todas las preguntas
- Todos los endings
- Efectos visuales completos
- Audio completo

**Nice to Have:**
- Endings secretos adicionales
- Más preguntas
- Logros/estadísticas
- Múltiples idiomas

---

## 11. Consideraciones de Diseño

### 11.1 Filosofía de Diseño

- **Simplicidad:** El juego debe ser simple de entender y jugar
- **Humor:** El humor debe ser el foco principal
- **Rejugabilidad:** Diferentes combinaciones deben llevar a diferentes resultados
- **Atmósfera:** La estética deprimente debe contrastar con el humor absurdo

### 11.2 Balanceo

**Preguntas:**
- Distribuir preguntas de diferentes tipos a lo largo de la partida
- Asegurar que todas las preguntas sean accesibles en algún momento

**Estados:**
- Balancear los valores de puntos para que los estados sean alcanzables
- Asegurar que diferentes estilos de juego lleven a diferentes endings

**Endings:**
- Cada ending debe ser alcanzable con una estrategia clara
- Algunos endings deben ser más difíciles de conseguir (secretos)

### 11.3 Accesibilidad

- Texto legible (usar FontManager del proyecto)
- Contraste adecuado
- Opciones de tamaño de texto (si es posible)
- Controles simples (mouse y teclado)

---

## 12. Referencias y Inspiración

### 12.1 Juegos Similares

- **Papers, Please:** Estética pixel art y decisiones morales
- **The Stanley Parable:** Narrativa absurda y múltiples endings
- **Job Simulator:** Sátira del mundo laboral
- **Doki Doki Literature Club:** Sistema de estados ocultos y múltiples endings

### 12.2 Referencias de Humor

- Humor corporativo absurdo
- Sátira de procesos de contratación
- Humor negro sobre el mundo laboral

---

## 13. Notas Adicionales

### 13.1 Expansiones Futuras (Opcional)

- **Modo carrera:** Múltiples entrevistas en diferentes empresas
- **Sistema de logros:** Desbloquear endings y preguntas especiales
- **Editor de preguntas:** Permitir a los jugadores crear sus propias preguntas
- **Multiplayer:** Modo donde un jugador es el entrevistador

### 13.2 Consideraciones Legales

- Asegurar que el humor no sea ofensivo de forma dañina
- Evitar referencias a empresas reales
- Mantener el tono satírico pero respetuoso

---

## 14. Conclusión

**The Last Job Interview Simulator** es un juego narrativo simple pero efectivo que usa humor absurdo para satirizar el proceso de contratación corporativo. Con un sistema de estados ocultos, múltiples endings y preguntas hilarantes, el juego ofrece una experiencia única y rejugable que resuena con cualquiera que haya pasado por una entrevista laboral.

El diseño minimalista (un fondo, un personaje, texto y botones) permite un desarrollo rápido mientras mantiene una estética distintiva y una atmósfera memorable.

---

**Fin del Documento**

