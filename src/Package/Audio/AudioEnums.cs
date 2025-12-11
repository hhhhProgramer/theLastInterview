using System;

namespace Package.Audio
{
    /// <summary>
    /// Enumerador que define todos los tipos de música disponibles en el juego
    /// Cada valor representa un archivo de música específico en la carpeta /Sound
    /// </summary>
    public enum MusicTrack
    {
        /// <summary>
        /// Música de la historia
        /// </summary>
        History,
        
        /// <summary>
        /// Música del menú principal
        /// </summary>
        Forest,

        /// <summary>
        /// Música de la ciudad
        /// </summary>
        City,

        /// <summary>
        /// Música de la ciudad
        /// </summary>
        Fishing,
        
        /// <summary>
        /// Música de la granja
        /// </summary>
        Farm,
        
        /// <summary>
        /// Música de alquimia con piano JRPG
        /// </summary>
        Alchemy,

        /// <summary>
        /// Música de batalla
        /// </summary>
        Battle,
        
        /// <summary>
        /// Música de introducción - Observando las estrellas
        /// </summary>
        IntroObservingStar,
        
        /// <summary>
        /// Música de introducción - Habitación (ambiente lluvioso oscuro)
        /// </summary>
        IntroBedroom,
        
        /// <summary>
        /// Música para flashback dramático
        /// </summary>
        Flashback,
        
        /// <summary>
        /// Música de decisión (Whispers From Beyond)
        /// </summary>
        Decision,
        
        /// <summary>
        /// Música de búsqueda/tensión (Searching)
        /// </summary>
        Searching,
        
        /// <summary>
        /// Música calmada - Innocence (Calm6)
        /// </summary>
        Innocence,
        
        /// <summary>
        /// Música de entrevista - We Have Time
        /// </summary>
        WeHaveTime
    }
    
    /// <summary>
    /// Enumerador que define todos los tipos de efectos de sonido disponibles
    /// Cada valor representa un archivo de efecto específico en la carpeta /EffectsSound
    /// </summary>
    public enum SoundEffect
    {
        /// <summary>
        /// Sonido de click en botones
        /// </summary>
        ButtonClick,
        
        /// <summary>
        /// Sonido de click genérico
        /// </summary>
        Click,
        
        /// <summary>
        /// Sonido de hover en botones
        /// </summary>
        ButtonHover,
        
        /// <summary>
        /// Sonido de selección de opciones
        /// </summary>
        OptionSelect,
        
        /// <summary>
        /// Sonido de confirmación
        /// </summary>
        Confirm,
        
        /// <summary>
        /// Sonido de cancelación
        /// </summary>
        Cancel,
        
        /// <summary>
        /// Sonido de error
        /// </summary>
        Error,
        
        /// <summary>
        /// Sonido de éxito
        /// </summary>
        Success,
        
        /// <summary>
        /// Sonido de movimiento del jugador
        /// </summary>
        PlayerMove,
        
        /// <summary>
        /// Sonido de colisión del jugador
        /// </summary>
        PlayerCollision,
        
        /// <summary>
        /// Sonido de disparo de lanzadores
        /// </summary>
        TurretShoot,
        
        /// <summary>
        /// Sonido de rebote de bullets
        /// </summary>
        BulletBounce,
        
        /// <summary>
        /// Sonido de agujero negro
        /// </summary>
        BlackHole,
        
        /// <summary>
        /// Sonido de muro giratorio
        /// </summary>
        RotatingWall,
        
        /// <summary>
        /// Sonido de nivel completado
        /// </summary>
        LevelComplete,
        
        /// <summary>
        /// Sonido de muerte del jugador
        /// </summary>
        PlayerDeath,
        
        /// <summary>
        /// Sonido de inicio de nivel
        /// </summary>
        LevelStart,
        
        /// <summary>
        /// Sonido de pausa
        /// </summary>
        Pause,
        
        /// <summary>
        /// Sonido de reanudación
        /// </summary>
        Resume,

        /// <summary>
        /// Sonido de Hito
        /// </summary>
        Milestone,
        
        /// <summary>
        /// Sonido de activación de mina de proximidad
        /// </summary>
        MineActivation,
        
        /// <summary>
        /// Sonido de explosión de mina
        /// </summary>
        MineExplosion,
        
        /// <summary>
        /// Sonido de apertura de puerta temporal
        /// </summary>
        DoorOpen,
        
        /// <summary>
        /// Sonido de cierre de puerta temporal
        /// </summary>
        DoorClose,
        
        /// <summary>
        /// Sonido de paso a través de puerta temporal
        /// </summary>
        DoorPass,
        
        /// <summary>
        /// Sonido de recolección de items
        /// </summary>
        ItemCollect,
        
        /// <summary>
        /// Sonido de selección de botón (beltHandle2)
        /// </summary>
        ButtonSelect
    }
    
    /// <summary>
    /// Enumerador que define los tipos de audio para control de volumen independiente
    /// </summary>
    public enum AudioType
    {
        /// <summary>
        /// Música de fondo
        /// </summary>
        Music,
        
        /// <summary>
        /// Efectos de sonido
        /// </summary>
        SoundEffects
    }
}
