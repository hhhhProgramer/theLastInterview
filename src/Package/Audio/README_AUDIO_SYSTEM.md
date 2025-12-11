# Sistema de Audio - Ciberpunk Geometry

## Descripción General

El sistema de audio de Ciberpunk Geometry es un gestor completo y robusto que maneja tanto música de fondo como efectos de sonido. Implementa el patrón Singleton para acceso global desde cualquier escena del juego.

## Características Principales

### ✅ Funcionalidades Implementadas

1. **Gestión de Música**
   - Carga automática desde `/src/Sound/`
   - Control de reproducción (play, pause, stop, resume)
   - Navegación entre tracks (siguiente, anterior, aleatorio)
   - Playlist automática

2. **Gestión de Efectos de Sonido**
   - Carga automática desde `/src/EffectsSound/`
   - Reproducción independiente de la música
   - Control de volumen individual

3. **Control de Volumen**
   - Volumen independiente para música y efectos
   - Silenciamiento individual por tipo
   - Persistencia de configuración
   - Conversión automática a decibeles

4. **Arquitectura Robusta**
   - Patrón Singleton para acceso global
   - Manejo de errores completo
   - Logging detallado para debugging
   - Eventos para sincronización de UI

## Estructura de Archivos

```
src/Core/Audio/
├── AudioEnums.cs          # Enumeradores para música y efectos
├── AudioConfig.cs         # Configuración y persistencia
├── AudioManager.cs        # Gestor principal del sistema
└── README_AUDIO_SYSTEM.md # Esta documentación

src/Core/Achievements/
└── Achievement.cs         # Sistema de logros para desbloqueables de audio
```

## Uso Básico

### 1. Acceso al AudioManager

```csharp
// El AudioManager es un singleton, accesible desde cualquier escena
var audioManager = AudioManager.Instance;

// Verificar si está disponible
if (audioManager != null)
{
    // Usar el sistema de audio
}
```

### 2. Reproducir Música

```csharp
// Reproducir música específica
AudioManager.Instance.PlayMusic(MusicTrack.MainMenu);

// Reproducir música de gameplay
AudioManager.Instance.PlayMusic(MusicTrack.Gameplay);

// Navegar entre tracks
AudioManager.Instance.PlayNextMusic();
AudioManager.Instance.PlayPreviousMusic();
AudioManager.Instance.PlayRandomMusic();
```

### 3. Reproducir Efectos de Sonido

```csharp
// Reproducir efecto con volumen por defecto
AudioManager.Instance.PlaySoundEffect(SoundEffect.ButtonClick);

// Reproducir efecto con volumen personalizado
AudioManager.Instance.PlaySoundEffect(SoundEffect.Confirm, 0.8f);
```

### 4. Control de Volumen

```csharp
// Establecer volumen de música (0.0 a 1.0)
AudioManager.Instance.SetMusicVolume(0.7f);

// Establecer volumen de efectos
AudioManager.Instance.SetSoundEffectsVolume(0.8f);

// Silenciar/activar música
AudioManager.Instance.SetMusicMuted(true);

// Silenciar/activar efectos
AudioManager.Instance.SetSoundEffectsMuted(false);
```

## Enumeradores Disponibles

### MusicTrack
- `MainMenu` - Música del menú principal
- `Gameplay` - Música durante el juego
- `HighIntensity` - Música para niveles intensos
- `LowIntensity` - Música para niveles tranquilos
- `MapEditor` - Música para el editor de mapas
- `Authentication` - Música para autenticación
- `UserProfile` - Música para perfil de usuario
- `MapSelection` - Música para selección de mapas
- `Victory` - Música de victoria
- `GameOver` - Música de game over

### SoundEffect
- `ButtonClick` - Click en botones
- `Click` - Click genérico
- `ButtonHover` - Hover en botones
- `OptionSelect` - Selección de opciones
- `Confirm` - Confirmación
- `Cancel` - Cancelación
- `Error` - Error
- `Success` - Éxito
- `PlayerMove` - Movimiento del jugador
- `PlayerCollision` - Colisión del jugador
- `TurretShoot` - Disparo de lanzadores
- `BulletBounce` - Rebote de bullets
- `BlackHole` - Agujero negro
- `RotatingWall` - Muro giratorio
- `LevelComplete` - Nivel completado
- `PlayerDeath` - Muerte del jugador
- `LevelStart` - Inicio de nivel
- `Pause` - Pausa
- `Resume` - Reanudación

## Eventos Disponibles

### Suscripción a Eventos

```csharp
// Suscribirse a cambios de música
AudioManager.Instance.MusicChanged += OnMusicChanged;

// Suscribirse a cambios de volumen
AudioManager.Instance.MusicVolumeChanged += OnMusicVolumeChanged;
AudioManager.Instance.SoundEffectsVolumeChanged += OnSoundEffectsVolumeChanged;

// Suscribirse a cambios de silenciamiento
AudioManager.Instance.MusicMutedChanged += OnMusicMutedChanged;
AudioManager.Instance.SoundEffectsMutedChanged += OnSoundEffectsMutedChanged;
```

### Implementación de Eventos

```csharp
private void OnMusicChanged(MusicTrack newTrack)
{
 
    // Actualizar UI, mostrar información, etc.
}

private void OnMusicVolumeChanged(float newVolume)
{
 
    // Actualizar slider de volumen, etc.
}
```

## Configuración Avanzada

### 1. Verificar Estado del Audio

```csharp
// Obtener información completa del estado
string status = AudioManager.Instance.GetAudioStatus();
 
```

### 2. Restablecer a Valores por Defecto

```csharp
// Restablecer toda la configuración de audio
AudioManager.Instance.ResetAudioToDefaults();
```

### 3. Verificar Disponibilidad

```csharp
// Verificar si hay música cargada
if (AudioManager.Instance.HasMusicLoaded)
{
 
}

// Verificar si hay efectos cargados
if (AudioManager.Instance.HasSoundEffectsLoaded)
{
 
}
```

## Integración con Escenas

### 1. Escena de Menú Principal

```csharp
public override void _Ready()
{
    // Reproducir música de menú
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayMusic(MusicTrack.MainMenu);
    }
}
```

### 1.1. Escena de Menú Principal con Música Épica

```csharp
public override void _Ready()
{
    // Reproducir música épica Mythica en el menú principal
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayMusic(MusicTrack.Mythica);
    }
}
```

### 2. Escena de Juego

```csharp
public override void _Ready()
{
    // Reproducir música de gameplay
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayMusic(MusicTrack.Gameplay);
    }
}

private void OnPlayerDeath()
{
    // Reproducir efecto de muerte
    AudioManager.Instance?.PlaySoundEffect(SoundEffect.PlayerDeath);
    
    // Cambiar a música de game over
    AudioManager.Instance?.PlayMusic(MusicTrack.GameOver);
}
```

### 3. Escena de Editor de Mapas

```csharp
public override void _Ready()
{
    // Reproducir música de editor
    if (AudioManager.Instance != null)
    {
        AudioManager.Instance.PlayMusic(MusicTrack.MapEditor);
    }
}

private void OnButtonPressed()
{
    // Reproducir efecto de click
    AudioManager.Instance?.PlaySoundEffect(SoundEffect.ButtonClick);
}
```

## Manejo de Errores

El sistema incluye manejo robusto de errores:

- **Archivos no encontrados**: Se registran como warnings, no detienen el sistema
- **Errores de carga**: Se capturan y registran, permitiendo continuar
- **Fallbacks**: Si un método falla, se intentan alternativas
- **Logging detallado**: Todos los errores se registran con contexto

## Optimización

### 1. Carga Lazy
- Los archivos de audio se cargan solo cuando son necesarios
- La memoria se libera automáticamente al cambiar de escena

### 2. Buses de Audio
- Música y efectos usan buses separados para control independiente
- Permite aplicar efectos de audio globales por tipo

### 3. Gestión de Memoria
- Los streams de audio se reutilizan cuando es posible
- Limpieza automática de recursos no utilizados

## Troubleshooting

### Problema: No se reproduce música
```csharp
// Verificar estado del sistema
string status = AudioManager.Instance.GetAudioStatus();
 

// Verificar si hay archivos cargados
if (!AudioManager.Instance.HasMusicLoaded)
{
    GD.PrintErr("❌ No hay música cargada");
}
```

### Problema: Efectos de sonido no funcionan
```csharp
// Verificar configuración
if (AudioManager.Instance.Config.IsSoundEffectsMuted)
{
 
}

// Verificar volumen
float volume = AudioManager.Instance.Config.SoundEffectsVolume;
 
```

### Problema: Configuración no se guarda
```csharp
// Forzar guardado
AudioManager.Instance.Config.ResetToDefaults();
```

## Notas de Implementación

1. **Formato de Archivos**: Se recomienda usar `.ogg` para mejor compresión
2. **Tamaño de Archivos**: Mantener archivos de música bajo 5MB para mejor rendimiento
3. **Calidad de Audio**: 44.1kHz, 16-bit es suficiente para la mayoría de casos
4. **Streaming**: Para archivos grandes, considerar usar `AudioStreamOggVorbis` con streaming

## Futuras Mejoras

- [ ] Sistema de fade in/out para transiciones suaves
- [ ] Soporte para múltiples pistas de música simultáneas
- [ ] Sistema de categorías de audio para mejor organización
- [ ] Integración con sistema de partículas para audio 3D
- [ ] Soporte para mods de audio
- [ ] Sistema de equalización personalizable

## Conclusión

El sistema de audio de Ciberpunk Geometry proporciona una base sólida y flexible para manejar toda la experiencia auditiva del juego. Con su arquitectura robusta y API intuitiva, permite a los desarrolladores integrar audio de manera eficiente y mantener un código limpio y mantenible.

Para cualquier pregunta o problema, consultar los logs del sistema que proporcionan información detallada sobre el estado y funcionamiento del audio.
