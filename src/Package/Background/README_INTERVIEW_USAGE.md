# Uso de SceneBackground en el Sistema de Entrevista

Este documento explica cómo usar `SceneBackground` en el sistema de entrevista, siguiendo el patrón establecido en otros proyectos del estudio.

## Patrón de Implementación

El patrón correcto para usar `SceneBackground` en escenas de entrevista es:

### 1. Crear CanvasLayer para Background

```csharp
var backgroundCanvasLayer = new CanvasLayer();
backgroundCanvasLayer.Name = "BackgroundCanvasLayer";
backgroundCanvasLayer.Layer = -1; // Detrás de todo
AddChild(backgroundCanvasLayer);
```

### 2. Crear Control Contenedor

```csharp
var backgroundContainer = new Control();
backgroundContainer.Name = "BackgroundContainer";
backgroundContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

// CRÍTICO: No bloquear clicks - permitir que pasen a través
backgroundContainer.MouseFilter = Control.MouseFilterEnum.Ignore;

// Obtener tamaño del viewport y establecer tamaño mínimo
var viewport = GetViewport();
var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
backgroundContainer.CustomMinimumSize = viewportSize;

backgroundCanvasLayer.AddChild(backgroundContainer);
```

### 3. Crear y Configurar SceneBackground

```csharp
_background = new SceneBackground();
_background.SetBackground("res://src/Image/Background/backgroun_office.png", new Color(0.1f, 0.1f, 0.1f, 1.0f));
backgroundContainer.AddChild(_background);
```

## Ejemplo Completo

```csharp
using Package.Background;
using Godot;

public partial class InterviewScene : Control
{
    private SceneBackground _background;

    public override void _Ready()
    {
        base._Ready();
        SetupBackground();
    }

    /// <summary>
    /// Configura el background de la escena
    /// </summary>
    private void SetupBackground()
    {
        // Crear CanvasLayer para background (detrás de todo)
        var backgroundCanvasLayer = new CanvasLayer();
        backgroundCanvasLayer.Name = "BackgroundCanvasLayer";
        backgroundCanvasLayer.Layer = -1; // Detrás de todo
        AddChild(backgroundCanvasLayer);
        
        // Crear Control contenedor dentro del CanvasLayer
        var backgroundContainer = new Control();
        backgroundContainer.Name = "BackgroundContainer";
        backgroundContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        
        // CRÍTICO: No bloquear clicks - permitir que pasen a través
        backgroundContainer.MouseFilter = Control.MouseFilterEnum.Ignore;
        
        // Obtener tamaño del viewport y establecer tamaño mínimo
        var viewport = GetViewport();
        var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
        backgroundContainer.CustomMinimumSize = viewportSize;
        
        backgroundCanvasLayer.AddChild(backgroundContainer);
        
        // Crear SceneBackground usando package
        _background = new SceneBackground();
        _background.SetBackground("res://src/Image/Background/backgroun_office.png", new Color(0.1f, 0.1f, 0.1f, 1.0f));
        backgroundContainer.AddChild(_background);
        
        GD.Print("✅ Background configurado");
    }
}
```

## Cambiar Background con Fade

Para cambiar el background con un efecto de fade (útil para transiciones):

```csharp
_background.ChangeBackgroundWithFade(
    "res://src/Image/Background/nuevo_fondo.png",
    1.0f, // Duración del fade en segundos
    () => {
        // Callback que se ejecuta cuando termina el fade
        GD.Print("Fade completado");
    }
);
```

## Fade a Negro

Para hacer un fade a negro sobre el background actual:

```csharp
_background.fadeToBlack(
    2.0f, // Duración del fade en segundos
    () => {
        // Callback que se ejecuta cuando termina el fade
        GD.Print("Fade a negro completado");
    }
);
```

## Obtener SceneBackground desde Otras Clases

Si necesitas acceder al `SceneBackground` desde otra clase (como un Manager o Strategy):

### En la Escena Principal

```csharp
public partial class InterviewScene : Control
{
    private SceneBackground _background;
    
    /// <summary>
    /// Obtiene el SceneBackground de la escena (para uso en managers/strategies)
    /// </summary>
    public SceneBackground GetSceneBackground()
    {
        return _background;
    }
}
```

### Desde un Manager o Strategy

```csharp
private SceneBackground GetSceneBackground()
{
    if (GetTree() == null) return null;

    var root = GetTree().Root;
    if (root == null) return null;

    // Buscar la escena principal
    var interviewScene = root.GetNodeOrNull<InterviewScene>("InterviewScene");
    if (interviewScene == null)
    {
        // Intentar buscar recursivamente
        interviewScene = FindNodeRecursive<InterviewScene>(root);
    }

    if (interviewScene != null)
    {
        return interviewScene.GetSceneBackground();
    }

    GD.PrintErr("No se pudo encontrar InterviewScene");
    return null;
}

/// <summary>
/// Busca un nodo de tipo T recursivamente en el árbol de escena
/// </summary>
private T FindNodeRecursive<T>(Node node) where T : Node
{
    if (node is T result)
    {
        return result;
    }

    foreach (Node child in node.GetChildren())
    {
        var found = FindNodeRecursive<T>(child);
        if (found != null)
        {
            return found;
        }
    }

    return null;
}
```

## Puntos Importantes

1. **CanvasLayer con Layer = -1**: Asegura que el background esté detrás de todo
2. **Control contenedor con FullRect**: Garantiza que el background cubra toda la pantalla
3. **CustomMinimumSize basado en viewport**: Asegura que el background se adapte a diferentes resoluciones
4. **MouseFilter.Ignore**: Permite que los clicks pasen a través del background
5. **SceneBackground dentro del contenedor**: El SceneBackground se agrega al contenedor, no directamente al CanvasLayer

## Referencias

- `src/Package/Background/SceneBackground.cs` - Implementación del componente
- `src/Package/Background/README.md` - Documentación general del componente
- `src/Interview/Managers/InterviewManager.cs` - Ejemplo de uso en el sistema de entrevista

