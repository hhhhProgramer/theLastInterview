# Guía de Uso del DialogSystem

Este documento explica cómo usar el `DialogSystem` para crear diálogos interactivos con opciones, siguiendo el patrón establecido en el proyecto.

## Conceptos Básicos

### DialogSystem
- **Autoload**: El `DialogSystem` está configurado como autoload en `project.godot`
- **Acceso**: Usar `DialogSystem.Instance` para acceder a la instancia
- **Propósito**: Maneja el flujo completo de diálogos, opciones, y personajes

### DialogEntry
Representa una entrada de diálogo (una "pantalla" de texto).

```csharp
var dialogEntry = new DialogEntry(
    "Texto del diálogo",
    "CharacterId",        // ID del personaje que habla (opcional)
    Emotion.Neutral,      // Emoción del personaje (opcional)
    null,                 // Posición del personaje (opcional)
    null,                 // Opciones de respuesta (opcional)
    null                  // OnShow callback (opcional)
);
```

### DialogOption
Representa una opción de respuesta del jugador.

```csharp
var dialogOption = new DialogOption(
    "Texto de la opción",
    () => {
        // Acción a ejecutar cuando se selecciona esta opción
        GD.Print("Opción seleccionada");
    },
    null // NextDialogs (opcional) - diálogos siguientes después de seleccionar
);
```

## Flujo Básico de Diálogos

### 1. Iniciar un Diálogo Simple

```csharp
using Package.UI;
using System.Collections.Generic;

// Crear entrada de diálogo
var dialogEntry = new DialogEntry(
    "Hola, ¿cómo estás?",
    "npc1",              // CharacterId
    Emotion.Neutral,     // Emoción
    null,                // Posición
    null,                // Sin opciones
    null                 // Sin callbacks
);

// Iniciar diálogo
var dialogList = new List<DialogEntry> { dialogEntry };
DialogSystem.Instance.StartDialog(dialogList);
```

### 2. Diálogo con Opciones

```csharp
// Crear opciones
var option1 = new DialogOption(
    "Opción 1",
    () => {
        GD.Print("Seleccionaste opción 1");
    },
    null // Sin diálogos siguientes
);

var option2 = new DialogOption(
    "Opción 2",
    () => {
        GD.Print("Seleccionaste opción 2");
    },
    null
);

var options = new List<DialogOption> { option1, option2 };

// Crear entrada de diálogo con opciones
var dialogEntry = new DialogEntry(
    "¿Qué quieres hacer?",
    "npc1",
    Emotion.Neutral,
    null,
    options,  // Opciones de respuesta
    null
);

// Iniciar diálogo
var dialogList = new List<DialogEntry> { dialogEntry };
DialogSystem.Instance.StartDialog(dialogList);
```

## Patrón Avanzado: NextDialogs (Ramas de Diálogo)

Cuando una opción tiene `NextDialogs`, se muestran esos diálogos después de seleccionar la opción, y luego el sistema vuelve al diálogo original.

### Ejemplo: Opción con Reacción

```csharp
// Crear diálogo de reacción
var reactionEntry = new DialogEntry(
    "Interesante elección...",
    "npc1",
    Emotion.Surprised,
    null,
    null,
    null
);

// Crear opción que muestra reacción
var option = new DialogOption(
    "Decir algo",
    () => {
        // Acción cuando se selecciona
        GD.Print("Opción seleccionada");
    },
    new List<DialogEntry> { reactionEntry } // NextDialogs - se muestra después
);

var dialogEntry = new DialogEntry(
    "¿Qué dices?",
    "npc1",
    null,
    null,
    new List<DialogOption> { option },
    null
);

DialogSystem.Instance.StartDialog(new List<DialogEntry> { dialogEntry });
```

**Flujo:**
1. Se muestra "¿Qué dices?"
2. Jugador selecciona "Decir algo"
3. Se ejecuta el callback de la opción
4. Se muestra "Interesante elección..." (NextDialogs)
5. Cuando termina la reacción, el sistema vuelve al diálogo original
6. Si el diálogo original ya terminó, el diálogo termina completamente

## Callbacks Disponibles

### OnShow
Se ejecuta **antes** de mostrar el texto del diálogo.

```csharp
var dialogEntry = new DialogEntry(
    "Texto",
    null, null, null, null,
    () => {
        // Se ejecuta ANTES de mostrar el texto
        GD.Print("Diálogo a punto de mostrarse");
    }
);
```

### OnEnd
Se ejecuta cuando **termina** el diálogo (después de que termine de escribir o cuando se avance).

```csharp
var dialogEntry = new DialogEntry("Texto", null, null, null, null, null);
dialogEntry.OnEnd = () => {
    // Se ejecuta cuando termina este diálogo
    GD.Print("Diálogo terminado");
};
```

### OnPressNextButton
Se ejecuta cuando el texto **ya está completo** y el usuario **presiona para avanzar**.

```csharp
var dialogEntry = new DialogEntry("Texto", null, null, null, null, null);
dialogEntry.OnPressNextButton = () => {
    // Se ejecuta cuando el usuario hace click después de que termine de escribir
    GD.Print("Usuario presionó continuar");
};
```

## Patrón: Múltiples Diálogos Consecutivos

```csharp
var dialogs = new List<DialogEntry>
{
    new DialogEntry("Primer diálogo")
        .WithCharacter(character)
        .WithEmotion(Emotion.Neutral),
    
    new DialogEntry("Segundo diálogo")
        .WithCharacter(character)
        .WithEmotion(Emotion.Surprised),
    
    new DialogEntry("Tercer diálogo")
        .WithCharacter(character)
        .WithEmotion(Emotion.Sad)
};

DialogSystem.Instance.StartDialog(dialogs);
```

## Patrón: Detectar Cuando Termina un Diálogo

### Usando Señales

```csharp
// Conectar señal
DialogSystem.Instance.DialogFinished += OnDialogFinished;

private void OnDialogFinished()
{
    GD.Print("Diálogo terminado completamente");
    // Continuar con siguiente acción
}
```

### Usando OnEnd en el Último Diálogo

```csharp
var lastDialog = new DialogEntry("Último diálogo", null, null, null, null, null);
lastDialog.OnEnd = () => {
    // Se ejecuta cuando termina el último diálogo
    GD.Print("Todos los diálogos terminaron");
};

var dialogs = new List<DialogEntry> { /* ... otros diálogos ... */, lastDialog };
DialogSystem.Instance.StartDialog(dialogs);
```

## Patrón: Opciones con Diálogos Ramificados

```csharp
// Crear rama de diálogos para una opción
var branchDialogs = new List<DialogEntry>
{
    new DialogEntry("Reacción inicial"),
    new DialogEntry("Seguimiento de la reacción")
        .End(() => {
            // Cuando termina la rama, ejecutar acción
            GD.Print("Rama completada");
        })
};

var option = new DialogOption(
    "Elegir esta opción",
    () => {
        GD.Print("Opción seleccionada");
    },
    branchDialogs // Diálogos que se mostrarán después
);

var dialogEntry = new DialogEntry(
    "Pregunta",
    null, null, null,
    new List<DialogOption> { option },
    null
);
```

**Comportamiento:**
- Cuando se selecciona la opción, se ejecuta `OnSelected`
- Luego se muestran los `NextDialogs` (rama)
- Cuando termina la rama, el sistema intenta volver al diálogo original
- Si el diálogo original ya terminó, se ejecuta `EndDialog()`

## Patrón: Iniciar Nuevo Diálogo desde una Opción

```csharp
var option = new DialogOption(
    "Continuar",
    () => {
        // Iniciar un nuevo diálogo completamente diferente
        var newDialogs = new List<DialogEntry>
        {
            new DialogEntry("Nuevo diálogo"),
            new DialogEntry("Segundo nuevo diálogo")
        };
        DialogSystem.Instance.StartDialog(newDialogs);
    },
    null // Sin NextDialogs
);
```

**Importante:** Cuando se inicia un nuevo diálogo desde una opción, el DialogSystem detecta que cambió `_currentDialog` y **NO avanza automáticamente** al siguiente diálogo del diálogo original.

## Ejemplo Completo: Sistema de Preguntas y Respuestas

```csharp
using Package.UI;
using System.Collections.Generic;

public class QuestionManager
{
    public void ShowQuestion(string questionText, List<string> answers)
    {
        var dialogOptions = new List<DialogOption>();
        
        foreach (var answer in answers)
        {
            var option = new DialogOption(
                answer,
                () => {
                    GD.Print($"Respuesta seleccionada: {answer}");
                    // Procesar respuesta
                },
                null
            );
            dialogOptions.Add(option);
        }
        
        var questionEntry = new DialogEntry(
            questionText,
            "Entrevistador",
            null,
            null,
            dialogOptions,
            null
        );
        
        // Asegurar que solo use opciones normales
        questionEntry.IsTruthLieDecision = false;
        questionEntry.IsTimedDecision = false;
        
        var dialogList = new List<DialogEntry> { questionEntry };
        DialogSystem.Instance.StartDialog(dialogList);
    }
}
```

## Ejemplo del Proyecto: Day1Strategy_School_Hallways

En el archivo de referencia, se usa el siguiente patrón:

```csharp
var dialogs = new List<DialogEntry>
{
    // Narrativa inicial
    new DialogEntry("Caminaba por el pasillo...")
        .Start(() => {
            // OnShow: Se ejecuta antes de mostrar
            character.ShowCharacter(0.5f);
        }),
    
    // Diálogo con personaje
    new DialogEntry("Hola, ¿cómo estás?")
        .WithCharacter(character)
        .WithEmotion(Emotion.Timid),
    
    // Diálogo del protagonista
    new DialogEntry("Bien, gracias")
        .AsProtagonistSpeech(),
    
    // Diálogo con opciones
    new DialogEntry("¿Qué quieres hacer?")
        .WithCharacter(character)
        .WithOptions(new List<DialogOption>
        {
            new DialogOption(
                "Opción 1",
                () => {
                    // Acción cuando se selecciona
                    saveFile.SetFlag("flag1", true);
                },
                null // Sin NextDialogs
            ),
            new DialogOption(
                "Opción 2",
                () => {
                    saveFile.SetFlag("flag2", true);
                },
                null
            )
        })
        .End(() => {
            // OnEnd: Se ejecuta cuando termina este diálogo
            ContinueToNextScene();
        })
};

DialogSystem.Instance.StartDialog(dialogs);
```

## Reglas Importantes

### 1. No Usar StartDialog Dentro de OnSelected si Quieres Continuar el Flujo

❌ **INCORRECTO:**
```csharp
var option = new DialogOption(
    "Opción",
    () => {
        // Esto iniciará un nuevo diálogo y el sistema NO avanzará automáticamente
        DialogSystem.Instance.StartDialog(newDialogs);
    },
    null
);
```

✅ **CORRECTO - Usar NextDialogs:**
```csharp
var option = new DialogOption(
    "Opción",
    () => {
        // Acción cuando se selecciona
    },
    newDialogs // Usar NextDialogs en lugar de StartDialog
);
```

✅ **CORRECTO - Usar Señal DialogFinished:**
```csharp
bool waitingForReaction = false;

var option = new DialogOption(
    "Opción",
    () => {
        waitingForReaction = true;
        DialogSystem.Instance.StartDialog(reactionDialogs);
    },
    null
);

// Conectar señal
DialogSystem.Instance.DialogFinished += () => {
    if (waitingForReaction)
    {
        waitingForReaction = false;
        ContinueToNextQuestion();
    }
};
```

### 2. OnEnd vs OnPressNextButton

- **OnEnd**: Se ejecuta cuando el diálogo termina (último diálogo de una lista o rama)
- **OnPressNextButton**: Se ejecuta cada vez que el usuario presiona continuar (en cualquier diálogo)

```csharp
var dialogEntry = new DialogEntry("Texto", null, null, null, null, null);
dialogEntry.OnPressNextButton = () => {
    // Se ejecuta cada vez que el usuario presiona continuar
};
dialogEntry.OnEnd = () => {
    // Se ejecuta solo cuando es el último diálogo
};
```

### 3. NextDialogs y Vuelta al Diálogo Original

Cuando se usan `NextDialogs`:
1. El sistema guarda el diálogo original
2. Muestra los `NextDialogs`
3. Cuando terminan los `NextDialogs`, intenta volver al diálogo original
4. Si el diálogo original ya terminó (solo tenía una entrada), llama a `EndDialog()`

**Solución:** Si necesitas continuar después de `NextDialogs`, usa `OnEnd` en el último diálogo de la rama:

```csharp
var reactionEntry = new DialogEntry("Reacción", null, null, null, null, null);
reactionEntry.OnEnd = () => {
    // Se ejecuta cuando termina la reacción, ANTES de que el sistema intente volver
    ContinueToNextQuestion();
};

var option = new DialogOption("Opción", () => {}, new List<DialogEntry> { reactionEntry });
```

## Métodos Builder (Fluent API)

El `DialogEntry` tiene métodos builder para facilitar la creación:

```csharp
var dialog = new DialogEntry("Texto")
    .WithCharacter(character)
    .WithEmotion(Emotion.Surprised)
    .WithPosition(new Vector2(100, 200))
    .WithOptions(options)
    .Start(() => { /* OnShow */ })
    .End(() => { /* OnEnd */ });
```

## Referencias

- `src/Package/DialogManagerSystem/DialogSystem.cs` - Implementación principal
- `src/Package/DialogManagerSystem/DialogBox.cs` - Componente visual del diálogo
- `src/Interview/Managers/InterviewManager.cs` - Ejemplo de uso en sistema de entrevista
- Archivos de referencia en proyecto noctis para ejemplos avanzados

