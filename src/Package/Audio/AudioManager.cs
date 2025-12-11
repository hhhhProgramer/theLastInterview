using System;
using System.Collections.Generic;
using Godot;

namespace Package.Audio
{
    /// <summary>
    /// Gestor principal de audio del juego que coordina música y efectos de sonido
    /// Implementa el patrón Singleton para acceso global desde cualquier escena
    /// Maneja la carga automática de archivos de audio, control de volumen y navegación entre tracks
    /// </summary>
    public partial class AudioManager : Node
    {
        /// <summary>
        /// Instancia singleton del AudioManager
        /// </summary>
        public static AudioManager Instance { get; private set; }
        
        /// <summary>
        /// Configuración de audio del juego
        /// </summary>
        public AudioConfig Config { get; private set; }
        
        /// <summary>
        /// Evento que se dispara cuando cambia la música actual
        /// </summary>
        public event Action<MusicTrack> MusicChanged;
        
        /// <summary>
        /// Evento que se dispara cuando cambia el volumen de la música
        /// </summary>
        public event Action<float> MusicVolumeChanged;
        
        /// <summary>
        /// Evento que se dispara cuando cambia el volumen de los efectos de sonido
        /// </summary>
        public event Action<float> SoundEffectsVolumeChanged;
        
        /// <summary>
        /// Evento que se dispara cuando cambia el estado de silenciamiento de la música
        /// </summary>
        public event Action<bool> MusicMutedChanged;
        
        /// <summary>
        /// Evento que se dispara cuando cambia el estado de silenciamiento de los efectos
        /// </summary>
        public event Action<bool> SoundEffectsMutedChanged;
        
        // Nodos de audio
        private AudioStreamPlayer _musicPlayer;
        
        // Pool de players para efectos de sonido (evita que se corten)
        private List<AudioStreamPlayer> _soundEffectPlayers;
        private int _currentSoundEffectPlayerIndex = 0;
        private const int MAX_SOUND_EFFECT_PLAYERS = 8; // Máximo 8 efectos simultáneos
        
        // Diccionarios de archivos de audio
        private Dictionary<MusicTrack, AudioStream> _musicTracks;
        private Dictionary<SoundEffect, AudioStream> _soundEffects;
        
        // Estado de la música
        private MusicTrack _currentMusicTrack;
        private List<MusicTrack> _musicPlaylist;
        private int _currentMusicIndex;
        
        // Historial de reproducción (para poder volver atrás)
        private List<MusicTrack> _musicHistory;
        private const int MAX_HISTORY_SIZE = 10; // Máximo 10 canciones en el historial
        
        /// <summary>
        /// Música actualmente reproduciéndose
        /// </summary>
        public MusicTrack CurrentMusicTrack => _currentMusicTrack;
        
        /// <summary>
        /// Indica si la música está reproduciéndose actualmente
        /// </summary>
        public bool IsMusicPlaying => _musicPlayer?.Playing ?? false;
        
        /// <summary>
        /// Indica si hay música cargada
        /// </summary>
        public bool HasMusicLoaded => _musicTracks?.Count > 0;
        
        /// <summary>
        /// Indica si hay efectos de sonido cargados
        /// </summary>
        public bool HasSoundEffectsLoaded => _soundEffects?.Count > 0;
        
        /// <summary>
        /// Inicializa el AudioManager y sus componentes
        /// </summary>
        public override void _Ready()
        {
             
            
            // Configurar como singleton global
            Instance = this;
            ProcessMode = ProcessModeEnum.Always; // Mantener activo entre escenas
            
 
 
            
            InitializeAudioSystem();
            LoadAudioFiles();
            SetupEventHandlers();
            LoadAudioConfig();
        }
        
        public override void _Process(double delta)
        {
            // CRÍTICO: Verificar periódicamente que la música se esté reproduciendo
            // Esto previene que la música se detenga cuando se muestran UI como poemas
            // PERO solo si hay una música configurada y no está pausada
            if (_musicPlayer != null && 
                _musicPlayer.Stream != null && 
                !_musicPlayer.Playing && 
                !_musicPlayer.StreamPaused &&
                _currentMusicTrack != default(MusicTrack))
            {
                // Si hay música configurada pero no se está reproduciendo y no está pausada,
                // intentar reproducirla nuevamente (sin cambiar _currentMusicTrack)
                _musicPlayer.Play();
            }
        }
        
        /// <summary>
        /// Inicializa el sistema de audio
        /// </summary>
        private void InitializeAudioSystem()
        {
            try
            {
                // Crear configuración de audio
                Config = new AudioConfig();
                
                // Crear nodo de música
                _musicPlayer = new AudioStreamPlayer();
                _musicPlayer.Name = "MusicPlayer";
                _musicPlayer.Bus = "Music"; // Bus específico para música
                _musicPlayer.ProcessMode = ProcessModeEnum.Always; // CRÍTICO: Mantener activo incluso si el árbol se pausa
                AddChild(_musicPlayer);
                
                // Crear pool de players para efectos de sonido
                _soundEffectPlayers = new List<AudioStreamPlayer>();

                for (int i = 0; i < MAX_SOUND_EFFECT_PLAYERS; i++)
                {
                    var player = new AudioStreamPlayer();
                    player.Name = $"SoundEffectPlayer_{i}";
                    player.Bus = "SFX"; // Bus específico para efectos
                    player.ProcessMode = ProcessModeEnum.Always; // Mantener activo
                    AddChild(player);
                    _soundEffectPlayers.Add(player);
                }
                
                // Inicializar diccionarios
                _musicTracks = new Dictionary<MusicTrack, AudioStream>();
                _soundEffects = new Dictionary<SoundEffect, AudioStream>();
                
                // Inicializar playlist
                _musicPlaylist = new List<MusicTrack>();
                _currentMusicIndex = 0;
                
                // Inicializar historial
                _musicHistory = new List<MusicTrack>();
                
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Carga todos los archivos de audio desde las carpetas correspondientes
        /// </summary>
        private void LoadAudioFiles()
        {
            try
            {
 
                
                // Cargar música
                LoadMusicFiles();
                
                // Cargar efectos de sonido
                LoadSoundEffectFiles();
                
                // Configurar playlist de música
                SetupMusicPlaylist();
                
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Carga los archivos de música desde la carpeta /Sound
        /// </summary>
        private void LoadMusicFiles()
        {
            try
            {
 
                
                // Mapear enumeradores a nombres de archivo
                var musicFileMap = new Dictionary<MusicTrack, string>
                {
                    { MusicTrack.Forest, "Calm1 - A Place I Call Home.ogg" },
                    { MusicTrack.History, "Cleyton RX - Underwater.wav" },
                    { MusicTrack.Fishing, "A cup of tea.mp3" },
                    { MusicTrack.Farm, "Florist.mp3" },
                    { MusicTrack.Alchemy, "JRPG Piano.mp3" },
                    { MusicTrack.Battle, "Rainy Forest.mp3" },
                    { MusicTrack.IntroObservingStar, "ObservingTheStar.ogg" },
                    { MusicTrack.IntroBedroom, "Dark_Rainy_Night(ambience).ogg" },
                    { MusicTrack.Flashback, "un_understandabl110.ogg" },
                    { MusicTrack.Decision, "Evil5 - Whispers From Beyond.ogg" },
                    { MusicTrack.Searching, "Searching.ogg" },
                    { MusicTrack.Innocence, "Calm6 - Innocence.ogg" }
                };
                
 
                
                foreach (var kvp in musicFileMap)
                {
                    var musicTrack = kvp.Key;
                    var fileName = kvp.Value;
                    var filePath = $"res://src/Sound/{fileName}";
                    
                    if (ResourceLoader.Exists(filePath))
                    {
                        var audioStream = ResourceLoader.Load<AudioStream>(filePath);
                        if (audioStream != null)
                        {
                            _musicTracks[musicTrack] = audioStream;
                        }
                        }
                    }
                
 
                 
            }
            catch (Exception)
            {
                 
                 
            }
        }
        
        /// <summary>
        /// Carga los archivos de efectos de sonido desde la carpeta /EffectsSound
        /// </summary>
        private void LoadSoundEffectFiles()
        {
            try
            {
 
                
                // Mapear enumeradores a nombres de archivo
                var soundEffectFileMap = new Dictionary<SoundEffect, string>
                {
                    { SoundEffect.ButtonClick, "button_click.ogg" },
                    { SoundEffect.Click, "click.wav" },
                    { SoundEffect.ButtonHover, "button_hover.ogg" },
                    { SoundEffect.OptionSelect, "option_select.ogg" },
                    { SoundEffect.Confirm, "confirm.ogg" },
                    { SoundEffect.Cancel, "cancel.ogg" },
                    { SoundEffect.Error, "negative_2.wav" },
                    { SoundEffect.Success, "load.wav" },
                    { SoundEffect.PlayerMove, "player_move.ogg" },
                    { SoundEffect.PlayerCollision, "player_collision.ogg" },
                    { SoundEffect.TurretShoot, "tir.ogg" },
                    { SoundEffect.BulletBounce, "bullet_bounce.ogg" },
                    { SoundEffect.BlackHole, "black_hole.ogg" },
                    { SoundEffect.RotatingWall, "rotating_wall.ogg" },
                    { SoundEffect.LevelComplete, "level_complete.ogg" },
                    { SoundEffect.PlayerDeath, "Lose 3 - Sound effects Pack 2.ogg" },
                    { SoundEffect.LevelStart, "level_start.ogg" },
                    { SoundEffect.Pause, "pause.ogg" },
                    { SoundEffect.Resume, "resume.ogg" },
                    { SoundEffect.Milestone, "1up 1 - Sound effects Pack 2.ogg" },
                    { SoundEffect.ItemCollect, "pop.ogg" },
                    { SoundEffect.ButtonSelect, "beltHandle2.ogg" }
                };
                
                foreach (var kvp in soundEffectFileMap)
                {
                    var soundEffect = kvp.Key;
                    var fileName = kvp.Value;
                    var filePath = $"res://src/EffectsSound/{fileName}";
                    
                    if (ResourceLoader.Exists(filePath))
                    {
                        var audioStream = ResourceLoader.Load<AudioStream>(filePath);
                        if (audioStream != null)
                        {
                            _soundEffects[soundEffect] = audioStream;
 
                        }
                        else
                        {
                             
                        }
                    }
                    else
                    {
 
                    }
                }
                
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Configura la playlist de música para navegación
        /// </summary>
        private void SetupMusicPlaylist()
        {
            try
            {
                _musicPlaylist.Clear();
                _musicPlaylist.AddRange(_musicTracks.Keys);
                _currentMusicIndex = 0;
                
                if (_musicPlaylist.Count > 0)
                {
                    _currentMusicTrack = _musicPlaylist[0];
 
                }
                else
                {
 
                }
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Configura los manejadores de eventos
        /// </summary>
        private void SetupEventHandlers()
        {
            try
            {
                // Suscribirse a eventos de configuración
                Config.MusicVolumeChanged += OnMusicVolumeChanged;
                Config.SoundEffectsVolumeChanged += OnSoundEffectsVolumeChanged;
                Config.MusicMutedChanged += OnMusicMutedChanged;
                Config.SoundEffectsMutedChanged += OnSoundEffectsMutedChanged;
                
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Obtiene el siguiente player disponible para efectos de sonido
        /// </summary>
        /// <returns>Player disponible para reproducir efectos</returns>
        private AudioStreamPlayer GetAvailableSoundEffectPlayer()
        {
            // Buscar un player que no esté reproduciendo
            for (int i = 0; i < _soundEffectPlayers.Count; i++)
            {
                var player = _soundEffectPlayers[i];
                if (!player.Playing)
                {
                    return player;
                }
            }
            
            // Si todos están ocupados, usar el siguiente en round-robin
            _currentSoundEffectPlayerIndex = (_currentSoundEffectPlayerIndex + 1) % _soundEffectPlayers.Count;
            return _soundEffectPlayers[_currentSoundEffectPlayerIndex];
        }
        
        #region Control de Música
        
        /// <summary>
        /// Reproduce una música específica
        /// </summary>
        /// <param name="musicTrack">Track de música a reproducir</param>
        /// <param name="fadeIn">Indica si debe hacer fade in</param>
        public void PlayMusic(MusicTrack musicTrack, bool fadeIn = true)
        {
            PlayMusicInternal(musicTrack, fadeIn, addCurrentToHistory: true);
        }
        
        /// <summary>
        /// Método interno para reproducir música con control sobre el historial
        /// </summary>
        /// <param name="musicTrack">Track de música a reproducir</param>
        /// <param name="fadeIn">Indica si debe hacer fade in</param>
        /// <param name="addCurrentToHistory">Si es true, agrega la música actual al historial antes de cambiar</param>
        private void PlayMusicInternal(MusicTrack musicTrack, bool fadeIn = true, bool addCurrentToHistory = true)
        {
            try
            {
                if (!_musicTracks.ContainsKey(musicTrack))
                {
                    GD.PrintErr($"[AudioManager] No se encontró la música: {musicTrack}");
                    return;
                }
                
                var audioStream = _musicTracks[musicTrack];
                if (audioStream == null)
                {
                    GD.PrintErr($"[AudioManager] El audio stream es null para: {musicTrack}");
                    return;
                }
                
                // Verificar que el music player esté inicializado
                if (_musicPlayer == null)
                {
                    GD.PrintErr("[AudioManager] _musicPlayer es null - no se puede reproducir música");
                    return;
                }
                
                // CRÍTICO: Si ya se está reproduciendo la misma música, no hacer nada
                // Verificar ANTES de detener o cambiar cualquier cosa
                GD.Print($"[AudioManager] Verificando música: solicitada={musicTrack}, actual={_currentMusicTrack}, Playing={_musicPlayer.Playing}");
                
                if (_currentMusicTrack == musicTrack && _musicPlayer.Playing)
                {
                    GD.Print($"[AudioManager] ✅ Ya se está reproduciendo la misma música: {musicTrack} - NO reiniciando");
                    return;
                }
                
                if (_currentMusicTrack == musicTrack && !_musicPlayer.Playing)
                {
                    GD.Print($"[AudioManager] ⚠️ Misma música pero no se está reproduciendo - continuando para reanudar");
                }
                else if (_currentMusicTrack != musicTrack)
                {
                    GD.Print($"[AudioManager] 🔄 Cambiando música: {_currentMusicTrack} -> {musicTrack}");
                }
                
                // Guardar la música actual en el historial antes de cambiar (solo si es diferente y se solicita)
                if (addCurrentToHistory && _currentMusicTrack != musicTrack && _currentMusicTrack != default(MusicTrack))
                {
                    AddToHistory(_currentMusicTrack);
                }
                
                // Detener música actual solo si es diferente
                if (_currentMusicTrack != musicTrack && _musicPlayer.Playing)
                {
                    _musicPlayer.Stop();
                }
                
                // Configurar loop antes de asignar el stream
                if (audioStream is AudioStreamOggVorbis oggStream)
                {
                    oggStream.Loop = true;
                }
                else if (audioStream is AudioStreamMP3 mp3Stream)
                {
                    mp3Stream.Loop = true;
                }
                
                // Configurar y reproducir nueva música
                _musicPlayer.Stream = audioStream;
                _musicPlayer.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.Music));
                
                // Verificar que el stream se asignó correctamente
                if (_musicPlayer.Stream == null)
                {
                    GD.PrintErr($"[AudioManager] ⚠️ El stream no se asignó correctamente para {musicTrack}");
                    return;
                }
                
                // CRÍTICO: Asegurar que el stream no esté pausado antes de reproducir
                if (_musicPlayer.StreamPaused)
                {
                    _musicPlayer.StreamPaused = false;
                }
                
                // CRÍTICO: Actualizar _currentMusicTrack ANTES de reproducir
                // Esto asegura que la verificación funcione correctamente
                _currentMusicTrack = musicTrack;
                _currentMusicIndex = _musicPlaylist.IndexOf(musicTrack);
                
                _musicPlayer.Play();
                
                GD.Print($"[AudioManager] ✅ Play() llamado para {musicTrack}, Playing={_musicPlayer.Playing}, StreamPaused={_musicPlayer.StreamPaused}");
                
                // Verificar que realmente se esté reproduciendo después de un pequeño delay
                // Usar CallDeferred para asegurar que se ejecute en el siguiente frame
                CallDeferred(MethodName.VerifyMusicIsPlaying, (int)musicTrack);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AudioManager] Error al reproducir música {musicTrack}: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Agrega una canción al historial de reproducción
        /// </summary>
        private void AddToHistory(MusicTrack musicTrack)
        {
            if (_musicHistory == null)
            {
                _musicHistory = new List<MusicTrack>();
            }
            
            // Evitar duplicados consecutivos
            if (_musicHistory.Count > 0 && _musicHistory[_musicHistory.Count - 1] == musicTrack)
            {
                return;
            }
            
            _musicHistory.Add(musicTrack);
            
            // Limitar el tamaño del historial
            if (_musicHistory.Count > MAX_HISTORY_SIZE)
            {
                _musicHistory.RemoveAt(0);
            }
        }
        
        /// <summary>
        /// Reproduce la canción anterior del historial
        /// </summary>
        public void PlayPreviousMusic()
        {
            if (_musicHistory == null || _musicHistory.Count == 0)
            {
                GD.Print("[AudioManager] No hay canciones en el historial");
                return;
            }
            
            GD.Print($"[AudioManager] 🔍 PlayPreviousMusic: _currentMusicTrack={_currentMusicTrack}, historial tiene {_musicHistory.Count} canciones");
            
            // Obtener la última canción del historial (la más reciente antes de la actual)
            MusicTrack previousTrack = _musicHistory[_musicHistory.Count - 1];
            _musicHistory.RemoveAt(_musicHistory.Count - 1); // Remover del historial para evitar loops
            
            GD.Print($"[AudioManager] 🔍 PlayPreviousMusic: Obteniendo {previousTrack} del historial, quedan {_musicHistory.Count} canciones");
            
            // CRÍTICO: Cuando se reproduce desde el historial, NO agregar la música actual al historial
            // porque ya está en el historial y causaría loops. Llamar a PlayMusicInternal directamente.
            PlayMusicInternal(previousTrack, addCurrentToHistory: false);
            GD.Print($"[AudioManager] ✅ Reproduciendo canción anterior: {previousTrack}");
        }
        
        /// <summary>
        /// Indica si hay canciones en el historial
        /// </summary>
        public bool HasMusicHistory => _musicHistory != null && _musicHistory.Count > 0;
        
        /// <summary>
        /// Verifica que la música se esté reproduciendo (método diferido)
        /// </summary>
        private void VerifyMusicIsPlaying(int musicTrackValue)
        {
            MusicTrack musicTrack = (MusicTrack)musicTrackValue;
            
            if (_musicPlayer == null)
            {
                return;
            }
            
            // Verificar que el stream esté asignado
            if (_musicPlayer.Stream == null)
            {
                return;
            }
            
            // Verificar si está pausado y reanudar
            if (_musicPlayer.StreamPaused)
            {
                _musicPlayer.StreamPaused = false;
            }
            
            // Verificar si no se está reproduciendo y reproducir nuevamente
            if (!_musicPlayer.Playing)
            {
                _musicPlayer.Play();
                
                // Verificar nuevamente después de otro pequeño delay
                GetTree().CreateTimer(0.1f).Timeout += () => {
                    if (!_musicPlayer.Playing && _musicPlayer.Stream != null)
                    {
                        // Último intento: detener y reproducir desde cero
                        _musicPlayer.Stop();
                        _musicPlayer.StreamPaused = false;
                        _musicPlayer.Play();
                    }
                };
            }
        }
        
        /// <summary>
        /// Detiene la música actual
        /// </summary>
        public void StopMusic()
        {
            try
            {
                if (_musicPlayer != null && _musicPlayer.Playing)
                {
                    _musicPlayer.Stop();
                }
                // CRÍTICO: NO limpiar _currentMusicTrack cuando se detiene la música
                // Mantener el track actual para que PlayPreviousMusic() funcione correctamente
                // Solo se limpiará cuando se reproduzca una nueva música
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AudioManager] Error al detener música: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Pausa la música actual
        /// </summary>
        public void PauseMusic()
        {
            try
            {
                if (_musicPlayer != null && _musicPlayer.Playing)
                {
                    _musicPlayer.StreamPaused = true;
 
                }
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Reanuda la música pausada
        /// </summary>
        public void ResumeMusic()
        {
            try
            {
                if (_musicPlayer != null && _musicPlayer.StreamPaused)
                {
                    _musicPlayer.StreamPaused = false;
 
                }
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Reproduce la siguiente música en la playlist
        /// </summary>
        public void PlayNextMusic()
        {
            try
            {
                if (_musicPlaylist.Count == 0)
                {
 
                    return;
                }
                
                _currentMusicIndex = (_currentMusicIndex + 1) % _musicPlaylist.Count;
                var nextTrack = _musicPlaylist[_currentMusicIndex];
                
 
                PlayMusic(nextTrack);
            }
            catch (Exception)
            {
                 
            }
        }
        
        
        /// <summary>
        /// Reproduce música aleatoria de la playlist
        /// </summary>
        public void PlayRandomMusic()
        {
            try
            {
                if (_musicPlaylist.Count == 0)
                {
 
                    return;
                }
                
                var random = new Random();
                var randomIndex = random.Next(_musicPlaylist.Count);
                var randomTrack = _musicPlaylist[randomIndex];
                
 
                PlayMusic(randomTrack);
            }
            catch (Exception)
            {
                 
            }
        }
        
        #endregion
        
        #region Control de Efectos de Sonido
        
        /// <summary>
        /// Reproduce un efecto de sonido específico
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido a reproducir</param>
        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            try
            {
                if (!_soundEffects.ContainsKey(soundEffect))
                {
                     
                    return;
                }
                
                var audioStream = _soundEffects[soundEffect];
                if (audioStream == null)
                {
                     
                    return;
                }
                
                // Obtener nombre del archivo para calcular volumen individual
                string fileName = GetFileNameForSoundEffect(soundEffect);
                float effectiveVolume = Config.GetEffectiveVolumeForFile(AudioType.SoundEffects, fileName);
                
                // Configurar y reproducir efecto de sonido
                var player = GetAvailableSoundEffectPlayer();
                player.Stream = audioStream;
                player.VolumeDb = LinearToDb(effectiveVolume);
                player.Play();
                
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Reproduce un efecto de sonido con volumen personalizado
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido a reproducir</param>
        /// <param name="volume">Volumen personalizado (0.0 a 1.0)</param>
        public void PlaySoundEffect(SoundEffect soundEffect, float volume)
        {
            try
            {
                if (!_soundEffects.ContainsKey(soundEffect))
                {
                     
                    return;
                }
                
                var audioStream = _soundEffects[soundEffect];
                if (audioStream == null)
                {
                     
                    return;
                }
                
                // Obtener nombre del archivo para calcular volumen individual
                string fileName = GetFileNameForSoundEffect(soundEffect);
                float individualVolume = Config.GetIndividualVolume(fileName);
                
                // Aplicar regla de tres: volumenPersonalizado * volumenIndividual
                float effectiveVolume = Mathf.Min(volume * individualVolume, individualVolume);
                
                // Configurar y reproducir efecto de sonido con volumen personalizado
                var player = GetAvailableSoundEffectPlayer();
                player.Stream = audioStream;
                player.VolumeDb = LinearToDb(effectiveVolume);
                player.Play();
                
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        #endregion
        
        #region Control de Volúmenes Individuales
        
        /// <summary>
        /// Establece el volumen individual para un archivo de audio específico
        /// </summary>
        /// <param name="fileName">Nombre del archivo de audio (ej: "tir.ogg")</param>
        /// <param name="maxVolume">Volumen máximo del archivo (0.0 a 1.0)</param>
        public void SetIndividualVolume(string fileName, float maxVolume)
        {
            try
            {
                Config.SetIndividualVolume(fileName, maxVolume);
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Establece el volumen individual para un efecto de sonido específico usando el enumerador
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido del enumerador</param>
        /// <param name="maxVolume">Volumen máximo del archivo (0.0 a 1.0)</param>
        public void SetIndividualVolume(SoundEffect soundEffect, float maxVolume)
        {
            try
            {
                string fileName = GetFileNameForSoundEffect(soundEffect);
                Config.SetIndividualVolume(fileName, maxVolume);
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Obtiene el volumen individual para un archivo de audio específico
        /// </summary>
        /// <param name="fileName">Nombre del archivo de audio</param>
        /// <returns>Volumen máximo del archivo (1.0 si no está configurado)</returns>
        public float GetIndividualVolume(string fileName)
        {
            try
            {
                return Config.GetIndividualVolume(fileName);
            }
            catch (Exception)
            {
                 
                return 1.0f;
            }
        }
        
        /// <summary>
        /// Obtiene el volumen individual para un efecto de sonido específico usando el enumerador
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido del enumerador</param>
        /// <returns>Volumen máximo del archivo (1.0 si no está configurado)</returns>
        public float GetIndividualVolume(SoundEffect soundEffect)
        {
            try
            {
                string fileName = GetFileNameForSoundEffect(soundEffect);
                return Config.GetIndividualVolume(fileName);
            }
            catch (Exception)
            {
                 
                return 1.0f;
            }
        }
        
        /// <summary>
        /// Elimina la configuración de volumen individual para un archivo
        /// </summary>
        /// <param name="fileName">Nombre del archivo de audio</param>
        public void RemoveIndividualVolume(string fileName)
        {
            try
            {
                Config.RemoveIndividualVolume(fileName);
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Elimina la configuración de volumen individual para un efecto de sonido específico usando el enumerador
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido del enumerador</param>
        public void RemoveIndividualVolume(SoundEffect soundEffect)
        {
            try
            {
                string fileName = GetFileNameForSoundEffect(soundEffect);
                Config.RemoveIndividualVolume(fileName);
                 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Obtiene todos los volúmenes individuales configurados
        /// </summary>
        /// <returns>Diccionario con los volúmenes individuales</returns>
        public Dictionary<string, float> GetAllIndividualVolumes()
        {
            try
            {
                return Config.GetAllIndividualVolumes();
            }
            catch (Exception)
            {
                 
                return new Dictionary<string, float>();
            }
        }
        
        /// <summary>
        /// Configura volúmenes individuales para efectos de sonido específicos
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido</param>
        /// <param name="maxVolume">Volumen máximo (0.0 a 1.0)</param>
        public void SetSoundEffectIndividualVolume(SoundEffect soundEffect, float maxVolume)
        {
            try
            {
                string fileName = GetFileNameForSoundEffect(soundEffect);
                SetIndividualVolume(fileName, maxVolume);
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Obtiene el volumen individual para un efecto de sonido específico
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido</param>
        /// <returns>Volumen máximo del efecto (1.0 si no está configurado)</returns>
        public float GetSoundEffectIndividualVolume(SoundEffect soundEffect)
        {
            try
            {
                string fileName = GetFileNameForSoundEffect(soundEffect);
                return GetIndividualVolume(fileName);
            }
            catch (Exception)
            {
                 
                return 1.0f;
            }
        }
        
        #endregion
        
        #region Control de Volumen
        
        /// <summary>
        /// Establece el volumen de la música
        /// </summary>
        /// <param name="volume">Volumen (0.0 a 1.0)</param>
        public void SetMusicVolume(float volume)
        {
            try
            {
                Config.MusicVolume = volume;
                
                // Aplicar volumen actual si hay música reproduciéndose
                if (_musicPlayer != null && _musicPlayer.Playing)
                {
                    _musicPlayer.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.Music));
                }
                
                // Guardar configuración automáticamente
                SaveAudioConfig();
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Establece el volumen de los efectos de sonido
        /// </summary>
        /// <param name="volume">Volumen (0.0 a 1.0)</param>
        public void SetSoundEffectsVolume(float volume)
        {
            try
            {
                Config.SoundEffectsVolume = volume;
                
                // Aplicar volumen actual si hay efectos reproduciéndose
                if (_soundEffectPlayers != null)
                {
                    foreach (var player in _soundEffectPlayers)
                    {
                        if (player.Playing)
                        {
                            player.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.SoundEffects));
                        }
                    }
                }
                
                // Guardar configuración automáticamente
                SaveAudioConfig();
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Silencia o activa la música
        /// </summary>
        /// <param name="muted">True para silenciar, false para activar</param>
        public void SetMusicMuted(bool muted)
        {
            try
            {
                Config.IsMusicMuted = muted;
                
                // Aplicar silenciamiento actual si hay música reproduciéndose
                if (_musicPlayer != null && _musicPlayer.Playing)
                {
                    _musicPlayer.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.Music));
                }
                
                // Guardar configuración automáticamente
                SaveAudioConfig();
                 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Silencia o activa los efectos de sonido
        /// </summary>
        /// <param name="muted">True para silenciar, false para activar</param>
        public void SetSoundEffectsMuted(bool muted)
        {
            try
            {
                Config.IsSoundEffectsMuted = muted;
                
                // Aplicar silenciamiento actual si hay efectos reproduciéndose
                if (_soundEffectPlayers != null)
                {
                    foreach (var player in _soundEffectPlayers)
                    {
                        if (player.Playing)
                        {
                            player.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.SoundEffects));
                        }
                    }
                }
                
                // Guardar configuración automáticamente
                SaveAudioConfig();
                 
            }
            catch (Exception)
            {
                 
            }
        }
        
        #endregion
        
        #region Manejadores de Eventos
        
        /// <summary>
        /// Maneja el cambio de volumen de la música
        /// </summary>
        /// <param name="volume">Nuevo volumen</param>
        private void OnMusicVolumeChanged(float volume)
        {
            try
            {
                if (_musicPlayer != null && _musicPlayer.Playing)
                {
                    _musicPlayer.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.Music));
                }
                
                MusicVolumeChanged?.Invoke(volume);
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Maneja el cambio de volumen de los efectos de sonido
        /// </summary>
        /// <param name="volume">Nuevo volumen</param>
        private void OnSoundEffectsVolumeChanged(float volume)
        {
            try
            {
                if (_soundEffectPlayers != null)
                {
                    foreach (var player in _soundEffectPlayers)
                    {
                        if (player.Playing)
                        {
                            player.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.SoundEffects));
                        }
                    }
                }
                
                SoundEffectsVolumeChanged?.Invoke(volume);
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Maneja el cambio de estado de silenciamiento de la música
        /// </summary>
        /// <param name="muted">Nuevo estado de silenciamiento</param>
        private void OnMusicMutedChanged(bool muted)
        {
            try
            {
                if (_musicPlayer != null && _musicPlayer.Playing)
                {
                    _musicPlayer.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.Music));
                }
                
                MusicMutedChanged?.Invoke(muted);
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Maneja el cambio de estado de silenciamiento de los efectos
        /// </summary>
        /// <param name="muted">Nuevo estado de silenciamiento</param>
        private void OnSoundEffectsMutedChanged(bool muted)
        {
            try
            {
                if (_soundEffectPlayers != null)
                {
                    foreach (var player in _soundEffectPlayers)
                    {
                        if (player.Playing)
                        {
                            player.VolumeDb = LinearToDb(Config.GetEffectiveVolume(AudioType.SoundEffects));
                        }
                    }
                }
                
                SoundEffectsMutedChanged?.Invoke(muted);
            }
            catch (Exception)
            {
                 
            }
        }
        
        #endregion
        
        #region Utilidades
        
        /// <summary>
        /// Convierte un valor lineal (0.0 a 1.0) a decibeles
        /// </summary>
        /// <param name="linear">Valor lineal</param>
        /// <returns>Valor en decibeles</returns>
        private float LinearToDb(float linear)
        {
            if (linear <= 0.0f)
                return -80.0f; // Silenciado
            
            return Mathf.Log(linear) * 20.0f;
        }
        
        /// <summary>
        /// Obtiene el nombre del archivo para un efecto de sonido
        /// </summary>
        /// <param name="soundEffect">Efecto de sonido</param>
        /// <returns>Nombre del archivo</returns>
        private string GetFileNameForSoundEffect(SoundEffect soundEffect)
        {
            var soundEffectFileMap = new Dictionary<SoundEffect, string>
            {
                { SoundEffect.ButtonClick, "button_click.ogg" },
                { SoundEffect.Click, "click.wav" },
                { SoundEffect.ButtonHover, "button_hover.ogg" },
                { SoundEffect.OptionSelect, "option_select.ogg" },
                { SoundEffect.Confirm, "confirm.ogg" },
                { SoundEffect.Cancel, "cancel.ogg" },
                { SoundEffect.Error, "negative_2.wav" },
                { SoundEffect.Success, "load.wav" },
                { SoundEffect.PlayerMove, "player_move.ogg" },
                { SoundEffect.PlayerCollision, "player_collision.ogg" },
                { SoundEffect.TurretShoot, "tir.ogg" },
                { SoundEffect.BulletBounce, "bullet_bounce.ogg" },
                { SoundEffect.BlackHole, "black_hole.ogg" },
                { SoundEffect.RotatingWall, "rotating_wall.ogg" },
                { SoundEffect.LevelComplete, "level_complete.ogg" },
                { SoundEffect.PlayerDeath, "lose 3 - Sound effects Pack 2.ogg" },
                { SoundEffect.LevelStart, "level_start.ogg" },
                { SoundEffect.Pause, "pause.ogg" },
                { SoundEffect.Resume, "resume.ogg" },
                { SoundEffect.Milestone, "1up 1 - Sound effects Pack 2.ogg" },
                { SoundEffect.ItemCollect, "pop.ogg" },
                { SoundEffect.ButtonSelect, "beltHandle2.ogg" }
            };
            
            if (soundEffectFileMap.TryGetValue(soundEffect, out string fileName))
            {
                return fileName;
            }
            
             
            return "unknown.ogg"; // Fallback
        }
        
        /// <summary>
        /// Obtiene información del estado actual del audio
        /// </summary>
        /// <returns>Información del estado del audio</returns>
        public string GetAudioStatus()
        {
            var status = new System.Text.StringBuilder();
            status.AppendLine("🎵 ESTADO DEL AUDIO:");
            status.AppendLine($"   - Música actual: {_currentMusicTrack}");
            status.AppendLine($"   - Música reproduciéndose: {IsMusicPlaying}");
            status.AppendLine($"   - Volumen música: {Config.MusicVolume}");
            status.AppendLine($"   - Volumen efectos: {Config.SoundEffectsVolume}");
            status.AppendLine($"   - Música silenciada: {Config.IsMusicMuted}");
            status.AppendLine($"   - Efectos silenciados: {Config.IsSoundEffectsMuted}");
            status.AppendLine($"   - Tracks de música cargados: {_musicTracks.Count}");
            status.AppendLine($"   - Efectos de sonido cargados: {_soundEffects.Count}");
            status.AppendLine($"   - Playlist: {_musicPlaylist.Count} tracks");
            status.AppendLine($"   - Índice actual: {_currentMusicIndex}");
            
            return status.ToString();
        }
        
        /// <summary>
        /// Restablece la configuración de audio a los valores por defecto
        /// </summary>
        public void ResetAudioToDefaults()
        {
            try
            {
                Config.ResetToDefaults();
 
            }
            catch (Exception)
            {
                 
            }
        }
        
        /// <summary>
        /// Carga la configuración de audio desde GameData
        /// </summary>
        private void LoadAudioConfig()
        {
            try
            {
                // La configuración de audio se carga automáticamente desde AudioConfig
                // que usa Godot's ConfigFile para persistencia
                // No se requiere SaveManager en este proyecto
                GD.Print("Configuración de audio cargada desde AudioConfig");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error al cargar configuración de audio: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Guarda la configuración de audio usando AudioConfig
        /// </summary>
        private void SaveAudioConfig()
        {
            try
            {
                // La configuración de audio se guarda automáticamente en AudioConfig
                // que usa Godot's ConfigFile para persistencia
                // No se requiere SaveManager en este proyecto
                GD.Print("Configuración de audio guardada en AudioConfig");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Error al guardar configuración de audio: {ex.Message}");
            }
        }
        
        #endregion
        
        #region Helpers para Botones
        
        /// <summary>
        /// Configura un botón para que reproduzca automáticamente el sonido de selección cuando se presione
        /// </summary>
        /// <param name="button">Botón a configurar</param>
        public static void SetupButtonSelectSound(Button button)
        {
            if (button == null)
                return;
            
            // Agregar el handler para reproducir el sonido
            // Nota: No intentamos desconectar primero porque puede causar errores si el handler no existe
            // En la práctica, este método solo se llama una vez por botón, así que no hay riesgo de duplicados
            button.Pressed += OnButtonSelectSound;
        }
        
        /// <summary>
        /// Handler estático para reproducir el sonido de selección de botón
        /// </summary>
        private static void OnButtonSelectSound()
        {
            if (Instance != null)
            {
                Instance.PlaySoundEffect(SoundEffect.ButtonSelect);
            }
        }
        
        #endregion
        
        /// <summary>
        /// Libera los recursos del AudioManager
        /// </summary>
        public override void _ExitTree()
        {
            try
            {
                // Detener reproducción
                StopMusic();
                
                // Limpiar instancia singleton
                if (Instance == this)
                {
                    Instance = null;
 
                }
                
 
            }
            catch (Exception)
            {
                 
            }
        }
    }
}
