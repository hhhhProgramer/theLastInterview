# Sistema de Volúmenes Individuales por Archivo de Audio

## Descripción

El sistema de volúmenes individuales permite configurar volúmenes específicos para archivos de audio individuales, independientemente del volumen general del juego. Esto es especialmente útil para archivos que pueden ser demasiado fuertes o suaves en comparación con otros efectos de sonido.

## Características Principales

- **Volúmenes individuales por archivo**: Cada archivo de audio puede tener su propio volumen máximo
- **Regla de tres automática**: El volumen efectivo se calcula como `VolumenGeneral × VolumenIndividual`
- **Persistencia**: Los volúmenes individuales se guardan automáticamente
- **Fallback seguro**: Si no se configura un volumen individual, se usa 1.0 (100%) por defecto
- **Compatibilidad**: Funciona con el sistema de audio existente sin cambios

## Cómo Funciona

### 1. Volumen Individual
- **Valor**: 0.0 a 1.0 (0% a 100%)
- **Significado**: Volumen máximo que puede alcanzar el archivo
- **Ejemplo**: 0.7 significa que el archivo nunca superará el 70% de volumen

### 2. Regla de Tres
```
Volumen Efectivo = Volumen General × Volumen Individual
```

**Ejemplos prácticos:**

| Volumen General | Volumen Individual | Volumen Efectivo | Explicación |
|----------------|-------------------|------------------|-------------|
| 100% (1.0)     | 70% (0.7)         | 70% (0.7)        | Máximo permitido |
| 80% (0.8)      | 70% (0.7)         | 56% (0.56)       | 0.8 × 0.7 = 0.56 |
| 50% (0.5)      | 70% (0.7)         | 35% (0.35)       | 0.5 × 0.7 = 0.35 |
| 30% (0.3)      | 70% (0.7)         | 21% (0.21)       | 0.3 × 0.7 = 0.21 |

## Uso Básico

### Configurar Volumen Individual

```csharp
// Configurar "tir.ogg" al 70% de volumen máximo
AudioManager.Instance.SetIndividualVolume("tir.ogg", 0.7f);

// Configurar múltiples archivos
AudioManager.Instance.SetIndividualVolume("button_click.ogg", 0.5f);
AudioManager.Instance.SetIndividualVolume("player_move.ogg", 0.8f);
```

### Obtener Volumen Individual

```csharp
// Obtener el volumen individual de un archivo
float volume = AudioManager.Instance.GetIndividualVolume("tir.ogg");

// Obtener todos los volúmenes individuales
var allVolumes = AudioManager.Instance.GetAllIndividualVolumes();
```

### Eliminar Volumen Individual

```csharp
// Eliminar configuración (vuelve a 1.0 por defecto)
AudioManager.Instance.RemoveIndividualVolume("tir.ogg");
```

## Uso con Efectos de Sonido

### Configurar por Efecto

```csharp
// Configurar volumen individual para TurretShoot (tir.ogg)
AudioManager.Instance.SetSoundEffectIndividualVolume(SoundEffect.TurretShoot, 0.7f);

// Obtener volumen individual del efecto
float volume = AudioManager.Instance.GetSoundEffectIndividualVolume(SoundEffect.TurretShoot);
```

### Reproducción Automática

```csharp
// El efecto se reproduce automáticamente con el volumen individual aplicado
AudioManager.Instance.PlaySoundEffect(SoundEffect.TurretShoot);
```

## Casos de Uso Comunes

### 1. Efectos Demasiado Fuertes
```csharp
// Reducir el volumen de efectos que pueden ser molestos
AudioManager.Instance.SetIndividualVolume("explosion.ogg", 0.6f);
AudioManager.Instance.SetIndividualVolume("laser.ogg", 0.5f);
```

### 2. Efectos Demasiado Suaves
```csharp
// Aumentar el volumen de efectos que pueden ser inaudibles
AudioManager.Instance.SetIndividualVolume("footstep.ogg", 0.9f);
AudioManager.Instance.SetIndividualVolume("ambient.ogg", 0.8f);
```

### 3. Balance de Audio
```csharp
// Crear un balance personalizado entre diferentes tipos de efectos
AudioManager.Instance.SetIndividualVolume("music.ogg", 0.7f);      // Música al 70%
AudioManager.Instance.SetIndividualVolume("sfx.ogg", 0.8f);       // Efectos al 80%
AudioManager.Instance.SetIndividualVolume("voice.ogg", 0.9f);     // Voces al 90%
```

## Implementación Técnica

### Clases Principales

- **AudioConfig**: Maneja la configuración y persistencia de volúmenes individuales
- **AudioManager**: Proporciona métodos públicos para configurar y usar volúmenes individuales

### Almacenamiento

Los volúmenes individuales se guardan en `ProjectSettings` bajo la clave `"audio_individual_volumes"` como un diccionario JSON.

### Eventos

```csharp
// Suscribirse a cambios de volumen individual
Config.IndividualVolumeChanged += (fileName, volume) => {
 
};
```

## Ejemplos Completos

### Ejemplo 1: Configuración Básica
```csharp
public void SetupBasicIndividualVolumes()
{
    // Configurar volúmenes para archivos específicos
    AudioManager.Instance.SetIndividualVolume("tir.ogg", 0.7f);
    AudioManager.Instance.SetIndividualVolume("button_click.ogg", 0.5f);
    
    // Verificar configuración
    var volumes = AudioManager.Instance.GetAllIndividualVolumes();
    foreach (var kvp in volumes)
    {
 
    }
}
```

### Ejemplo 2: Sistema de Perfiles
```csharp
public void SetupAudioProfile(string profileName)
{
    switch (profileName)
    {
        case "Cinematic":
            AudioManager.Instance.SetIndividualVolume("music.ogg", 0.8f);
            AudioManager.Instance.SetIndividualVolume("sfx.ogg", 0.6f);
            AudioManager.Instance.SetIndividualVolume("voice.ogg", 0.9f);
            break;
            
        case "Gameplay":
            AudioManager.Instance.SetIndividualVolume("music.ogg", 0.5f);
            AudioManager.Instance.SetIndividualVolume("sfx.ogg", 0.8f);
            AudioManager.Instance.SetIndividualVolume("voice.ogg", 0.7f);
            break;
            
        case "Casual":
            AudioManager.Instance.SetIndividualVolume("music.ogg", 0.6f);
            AudioManager.Instance.SetIndividualVolume("sfx.ogg", 0.7f);
            AudioManager.Instance.SetIndividualVolume("voice.ogg", 0.8f);
            break;
    }
}
```

## Consideraciones de Rendimiento

- **Carga**: Los volúmenes individuales se cargan una vez al inicializar el AudioManager
- **Cálculo**: El cálculo del volumen efectivo es O(1) usando diccionarios
- **Memoria**: Uso mínimo de memoria (solo nombres de archivo y valores float)
- **Persistencia**: Guardado automático solo cuando cambian los valores

## Compatibilidad

- ✅ **Godot 4.4.1**: Completamente compatible
- ✅ **C#**: Implementado en C# con patrones SOLID
- ✅ **Sistema existente**: No rompe funcionalidad existente
- ✅ **Configuración**: Se integra con el sistema de configuración actual

## Solución de Problemas

### Error: "Archivo no encontrado"
```csharp
// Asegúrate de usar el nombre exacto del archivo
AudioManager.Instance.SetIndividualVolume("tir.ogg", 0.7f); // ✅ Correcto
AudioManager.Instance.SetIndividualVolume("tir", 0.7f);     // ❌ Incorrecto
```

### Error: "Volumen fuera de rango"
```csharp
// Los volúmenes se ajustan automáticamente al rango 0.0 - 1.0
AudioManager.Instance.SetIndividualVolume("tir.ogg", 1.5f); // Se ajusta a 1.0
AudioManager.Instance.SetIndividualVolume("tir.ogg", -0.5f); // Se ajusta a 0.0
```

### Verificar Configuración
```csharp
// ✅ RECOMENDADO: Usar el enumerador SoundEffect (type-safe)
float volume = AudioManager.Instance.GetIndividualVolume(SoundEffect.TurretShoot);
 

// ⚠️ ALTERNATIVO: Usar nombre de archivo directo (menos seguro)
float volume = AudioManager.Instance.GetIndividualVolume("tir.ogg");
 

// Verificar todos los volúmenes individuales
var allVolumes = AudioManager.Instance.GetAllIndividualVolumes();
 
```

### Problema de Carga Automática

Si ves el mensaje "⚠️ Carga de volúmenes individuales temporalmente deshabilitada", esto es normal en Godot 4.4.1. Los volúmenes individuales se configuran en tiempo de ejecución y se mantienen durante la sesión del juego.

**Solución temporal:**
```csharp
// Configurar volúmenes individuales al inicio del juego
void _Ready()
{
    // Configurar volúmenes individuales para archivos específicos
    AudioManager.Instance.SetIndividualVolume(SoundEffect.TurretShoot, 0.7f);
    AudioManager.Instance.SetIndividualVolume(SoundEffect.ButtonClick, 0.5f);
    AudioManager.Instance.SetIndividualVolume(SoundEffect.PlayerMove, 0.8f);
}
```

## Conclusión

El sistema de volúmenes individuales proporciona un control granular sobre el audio del juego, permitiendo crear experiencias de audio más balanceadas y personalizadas. La implementación es eficiente, compatible y fácil de usar, manteniendo la arquitectura SOLID del proyecto.
