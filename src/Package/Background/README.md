# Package Background - SceneBackground

## Descripción

`SceneBackground` es un componente reutilizable para agregar backgrounds de pantalla completa a cualquier escena en Godot 4. Usa `TextureRect` con `FullRect` para ajustarse automáticamente al tamaño del viewport.

## Características

- ✅ **Ajuste automático**: Se ajusta automáticamente al tamaño del viewport usando `FullRect`
- ✅ **Fondo sólido de fallback**: Incluye un `ColorRect` detrás de la imagen para cubrir bordes
- ✅ **Configuración correcta**: Usa `KeepAspectCovered` y `FitWidthProportional` para cubrir toda la pantalla
- ✅ **Fácil de usar**: Solo necesitas crear el componente y llamar `SetBackground()`
- ✅ **Reutilizable**: Puede usarse en cualquier escena (Control o Node2D)

## Uso Básico

### En una Escena Control

```csharp
using Aprendizdemago.Package.Background;
using Godot;

public partial class MyScene : Control
{
    private SceneBackground _background;
    
    public override void _Ready()
    {
        base._Ready();
        
        // Crear background
        _background = new SceneBackground();
        _background.SetBackground("res://src/Image/Background/background.png");
        
        // Agregar a la escena
        AddChild(_background);
        MoveChild(_background, 0); // Mover al principio para que quede detrás
    }
}
```

### En una Escena Node2D

```csharp
using Aprendizdemago.Package.Background;
using Godot;

public partial class MyScene : Node2D
{
    private SceneBackground _background;
    
    public override void _Ready()
    {
        base._Ready();
        
        // Crear CanvasLayer para el background (no se mueve con la cámara)
        var canvasLayer = new CanvasLayer();
        canvasLayer.Layer = -1; // Detrás de todo
        AddChild(canvasLayer);
        
        // Crear background
        _background = new SceneBackground();
        _background.SetBackground("res://src/Image/Background/background.png");
        
        // Agregar al CanvasLayer
        canvasLayer.AddChild(_background);
    }
}
```

## API

### Métodos Principales

#### `SetBackground(string imagePath, Color? fallbackColor = null)`

Establece el background de la escena.

**Parámetros:**
- `imagePath`: Ruta de la imagen de background
- `fallbackColor`: Color de fallback si no se carga la imagen (opcional)

**Ejemplo:**
```csharp
_background.SetBackground("res://src/Image/Background/background.png");
_background.SetBackground("res://src/Image/Background/background.png", new Color(0.1f, 0.1f, 0.2f, 1.0f));
```

#### `ChangeBackground(string imagePath)`

Cambia el background dinámicamente.

**Ejemplo:**
```csharp
_background.ChangeBackground("res://src/Image/Background/new_background.png");
```

#### `SetFallbackColor(Color color)`

Establece el color de fallback.

**Ejemplo:**
```csharp
_background.SetFallbackColor(new Color(0.0f, 0.0f, 0.0f, 1.0f)); // Negro sólido
```

#### `ClearBackground()`

Limpia el background (oculta la imagen, muestra solo el fondo sólido).

**Ejemplo:**
```csharp
_background.ClearBackground();
```

## Configuración Automática

El componente se configura automáticamente con:

- **Anclas**: `FullRect` para ocupar toda la pantalla
- **StretchMode**: `KeepAspectCovered` para cubrir toda la pantalla manteniendo aspecto
- **ExpandMode**: `FitWidthProportional` para ajustarse al ancho
- **ZIndex**: `-1` para estar detrás de todo
- **MouseFilter**: `Ignore` para permitir clicks a través

## Ejemplo Completo

```csharp
using Aprendizdemago.Package.Background;
using Godot;

public partial class ExampleScene : Control
{
    private SceneBackground _background;
    
    public override void _Ready()
    {
        base._Ready();
        
        // Configurar Control raíz
        SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        CustomMinimumSize = new Vector2(Constants.VIEWPORT_WIDTH, Constants.VIEWPORT_HEIGHT);
        
        // Crear background
        _background = new SceneBackground();
        _background.SetBackground(
            "res://src/Image/Background/background.png",
            new Color(0.1f, 0.1f, 0.1f, 1.0f) // Color de fallback
        );
        
        // Agregar y mover al principio
        AddChild(_background);
        MoveChild(_background, 0);
    }
    
    // Cambiar background dinámicamente
    public void ChangeToNewBackground()
    {
        _background.ChangeBackground("res://src/Image/Background/new_background.png");
    }
}
```

## Notas Importantes

1. **Orden de los hijos**: Asegúrate de mover el background al principio de la jerarquía (`MoveChild(background, 0)`) para que quede detrás de todo.

2. **ZIndex**: El background tiene `ZIndex = -1` por defecto, pero puedes ajustarlo si es necesario.

3. **CanvasLayer para Node2D**: Si usas `Node2D` como raíz, agrega el background a un `CanvasLayer` para que no se mueva con la cámara.

4. **Tamaño del viewport**: El componente usa `Constants.VIEWPORT_WIDTH` y `Constants.VIEWPORT_HEIGHT` para el tamaño mínimo, pero se ajusta automáticamente al tamaño real del viewport.

## Solución de Problemas

### El background no cubre toda la pantalla

**Solución**: Asegúrate de que el Control raíz también tenga `FullRect`:
```csharp
SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
```

### El background se mueve con la cámara

**Solución**: Si usas `Node2D`, agrega el background a un `CanvasLayer`:
```csharp
var canvasLayer = new CanvasLayer();
canvasLayer.Layer = -1;
AddChild(canvasLayer);
canvasLayer.AddChild(_background);
```

### El background tiene bordes negros

**Solución**: El componente incluye un `ColorRect` de fallback detrás de la imagen. Si ves bordes, el color de fallback se mostrará. Ajusta el color con `SetFallbackColor()`.

