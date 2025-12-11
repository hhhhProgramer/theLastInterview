using Godot;
using Package.Core.Interfaces;
using Package.Core.Enums;

namespace Package.Characters
{
    /// <summary>
    /// Clase base para personajes con emociones y animaciones
    /// Implementa IEmotionalCharacter y contiene funcionalidad común
    /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
    /// </summary>
    public abstract partial class Character : Node2D, IEmotionalCharacter
    {
        /// <summary>
        /// ID único del personaje para el sistema de diálogos
        /// </summary>
        public abstract string CharacterId { get; }
        
        /// <summary>
        /// Nombre del personaje
        /// </summary>
        public abstract string CharacterName { get; }
        
        /// <summary>
        /// Emoción actual del personaje
        /// </summary>
        public Emotion CurrentEmotion { get; protected set; } = Emotion.Neutral;
        
        /// <summary>
        /// Obtiene la emoción actual del personaje
        /// </summary>
        /// <returns>Emoción actual</returns>
        public Emotion GetCurrentEmotion()
        {
            return CurrentEmotion;
        }
        
        /// <summary>
        /// Sprite del personaje (Sprite2D para Node2D)
        /// </summary>
        protected TextureRect _sprite;
        
        /// <summary>
        /// Tween para animaciones continuas
        /// </summary>
        protected Tween _idleTween;
        
        /// <summary>
        /// Tween para movimiento
        /// </summary>
        protected Tween _movementTween;
        
        /// <summary>
        /// Tween para animación de posición speaking
        /// </summary>
        private Tween _speakingPositionTween;
        
        /// <summary>
        /// Tween para animaciones de squash and stretch
        /// </summary>
        protected Tween _squashStretchTween;
        
        /// <summary>
        /// Scale base del personaje (sin animaciones)
        /// </summary>
        protected Vector2 _baseScale;
        
        /// <summary>
        /// Posición base para animaciones
        /// </summary>
        protected Vector2 _basePosition;
        
        /// <summary>
        /// Indica si el personaje está agrandado por el sistema de diálogos
        /// </summary>
        private bool _isEnlargedByDialog = false;
        
        /// <summary>
        /// Indica si se debe iniciar la animación speaking después de que termine el movimiento
        /// </summary>
        private bool _shouldStartSpeakingAfterMovement = false;
        
        /// <summary>
        /// Tamaño agrandado esperado cuando el personaje está hablando
        /// Se usa para asegurar que el pulse use el tamaño correcto
        /// </summary>
        private Vector2 _enlargedScale = Vector2.Zero;
        
        /// <summary>
        /// Obtiene el tamaño base del personaje
        /// </summary>
        /// <returns>Tamaño base del personaje</returns>
        public Vector2 GetBaseScale()
        {
            if (_baseScale != Vector2.Zero && _baseScale.X > 0.1f && _baseScale.Y > 0.1f)
            {
                return _baseScale;
            }
            // Si no hay baseScale establecido, usar el Scale actual o (1, 1) como fallback
            if (Scale != Vector2.Zero && Scale.X > 0.1f && Scale.Y > 0.1f)
            {
                return Scale;
            }
            return new Vector2(1.0f, 1.0f);
        }
        
        /// <summary>
        /// Aumenta ligeramente el tamaño del personaje (para indicar que está hablando)
        /// </summary>
        /// <param name="multiplier">Multiplicador del tamaño (por defecto 1.1 = 10% más grande)</param>
        /// <param name="duration">Duración de la animación (por defecto 0.3 segundos)</param>
        /// <param name="startPulseAfterEnlarge">Si es true, iniciará el pulse después de agrandar (por defecto true)</param>
        public void EnlargeScale(float multiplier = 1.1f, float duration = 0.3f, bool startPulseAfterEnlarge = true)
        {
            if (_isEnlargedByDialog) return; // Ya está agrandado
            
            Vector2 baseScale = GetBaseScale();
            Vector2 enlargedScale = baseScale * multiplier;
            
            // CRÍTICO: Si hay un pulse activo, detenerlo primero
            bool hadPulse = _squashStretchTween != null && _squashStretchTween.IsValid();
            StopStretchAnimation();
            
            // CRÍTICO: Guardar el tamaño agrandado esperado para que el pulse lo use correctamente
            _enlargedScale = enlargedScale;
            
            var tween = CreateTween();
            tween.TweenProperty(this, "scale", enlargedScale, duration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);
            
            _isEnlargedByDialog = true;
            
            // CRÍTICO: Iniciar el pulse después de agrandar si se solicita (por defecto siempre)
            // Esto asegura que el pulse siempre se active cuando el personaje está hablando
            // CRÍTICO: Usar CallDeferred para asegurar que el tween termine completamente antes de iniciar el pulse
            if (startPulseAfterEnlarge)
            {
                tween.TweenCallback(Callable.From(() => {
                    // CRÍTICO: Asegurar que el Scale esté en el tamaño agrandado antes de iniciar el pulse
                    // Esto evita que el pulse capture un tamaño incorrecto
                    Scale = enlargedScale;
                    
                    // CRÍTICO: Usar CallDeferred para asegurar que el Scale esté completamente establecido
                    // antes de iniciar el pulse. Esto evita que el pulse capture un tamaño intermedio
                    CallDeferred(MethodName.StartPulseAfterEnlarge);
                }));
            }
        }
        
        /// <summary>
        /// Restaura el tamaño base del personaje
        /// </summary>
        /// <param name="duration">Duración de la animación (por defecto 0.3 segundos)</param>
        public void RestoreBaseScale(float duration = 0.3f)
        {
            if (!_isEnlargedByDialog) return; // No está agrandado
            
            Vector2 baseScale = GetBaseScale();
            
            // CRÍTICO: Limpiar el tamaño agrandado guardado
            _enlargedScale = Vector2.Zero;
            
            var tween = CreateTween();
            tween.TweenProperty(this, "scale", baseScale, duration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Cubic);
            
            _isEnlargedByDialog = false;
        }
        
        /// <summary>
        /// Inicia el pulse después de que el agrandamiento termine completamente
        /// Se llama desde CallDeferred para asegurar que el Scale esté completamente establecido
        /// </summary>
        private void StartPulseAfterEnlarge()
        {
            // CRÍTICO: Verificar que el personaje aún está agrandado
            if (!_isEnlargedByDialog || _enlargedScale == Vector2.Zero)
            {
                return;
            }
            
            // CRÍTICO: Asegurar que el Scale esté en el tamaño agrandado antes de iniciar el pulse
            // Esto evita que el pulse capture un tamaño incorrecto
            if (Scale != _enlargedScale)
            {
                Scale = _enlargedScale;
            }
            
            // CRÍTICO: Iniciar el pulse ahora que el tamaño está completamente establecido
            PlayPulseAnimation();
        }
        
        /// <summary>
        /// Establece el tamaño del personaje como porcentaje del viewport
        /// 100 = altura completa del viewport
        /// El porcentaje se aplica directamente al viewport, sin considerar el tamaño de la imagen
        /// El ancho se calcula proporcionalmente para mantener la relación de aspecto de la textura
        /// </summary>
        /// <param name="percent">Porcentaje de 0 a 100 (0 = invisible, 100 = altura completa del viewport)</param>
        public void SetSize(float percent)
        {
            if (_sprite == null || _sprite.Texture == null)
            {
                GD.PrintErr($"[{CharacterName}] ❌ No se puede establecer tamaño: sprite o textura no disponible");
                return;
            }
            
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            
            // CRÍTICO: Calcular la altura objetivo directamente del viewport (sin considerar tamaño de imagen)
            // Si percent = 70 y viewport = 100px, entonces targetHeight = 70px (directamente)
            // percent ya es el porcentaje (70 = 70%), no necesita dividir por 100
            float targetHeight = (viewportSize.Y * percent) / 100.0f;
            
            // Obtener el tamaño original de la textura solo para calcular la proporción del ancho
            var textureSize = _sprite.Texture.GetSize();
            
            if (textureSize.Y <= 0)
            {
                GD.PrintErr($"[{CharacterName}] ❌ No se puede establecer tamaño: altura de textura inválida ({textureSize.Y})");
                return;
            }
            
            // CRÍTICO: Calcular el ancho proporcionalmente basado en la relación de aspecto de la textura
            // Mantener la proporción: ancho = (ancho_textura / alto_textura) * alto_objetivo
            float aspectRatio = textureSize.X / textureSize.Y;
            float targetWidth = targetHeight * aspectRatio;
            
            // Si el porcentaje es 0, el tamaño será 0
            if (percent <= 0.0f)
            {
                targetWidth = 0.0f;
                targetHeight = 0.0f;
            }
            
            // CRÍTICO: Ajustar el CustomMinimumSize y Size del TextureRect directamente
            _sprite.CustomMinimumSize = new Vector2(targetWidth, targetHeight);
            _sprite.Size = new Vector2(targetWidth, targetHeight);
            _sprite.Position = new Vector2(targetWidth * -0.5f, targetHeight * -0.5f);
            
            GD.Print($"[{CharacterName}] ✅ SetSize: {percent}% -> targetSize: ({targetWidth}, {targetHeight})px (viewport: {viewportSize}, aspectRatio: {aspectRatio})");
        }
        
        public override void _Ready()
        {
            // Asegurar que el personaje no se vea afectado por la pausa
            ProcessMode = Node.ProcessModeEnum.Always;
            _sprite = new TextureRect();
            _sprite.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
            _sprite.StretchMode = TextureRect.StretchModeEnum.Scale;
            _sprite.Visible = false;
            
            // Esperar un frame para asegurar que el viewport esté listo antes de configurar el sprite
            CallDeferred(MethodName.SetupSpriteDeferred);
            
            GD.Print($"{CharacterName} inicializado");
        }
        
        /// <summary>
        /// Configura el sprite de forma diferida para asegurar que el viewport esté listo
        /// </summary>
        private void SetupSpriteDeferred()
        {
            SetupSprite();
            
            // Establecer posición por defecto: esquina inferior izquierda
            SetDefaultPosition();
            
            StartIdleAnimation();
        }
        
        /// <summary>
        /// Establece la posición por defecto del personaje (esquina inferior izquierda)
        /// </summary>
        private void SetDefaultPosition()
        {
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            
            // Obtener el tamaño del sprite escalado para asegurar que quepa en pantalla
            Vector2 spriteSize = Vector2.Zero;
            if (_sprite != null && _sprite.Texture != null)
            {
                spriteSize = _sprite.Texture.GetSize() * Scale;
            }
            
            // Posición por defecto: esquina inferior izquierda
            // 5% desde la izquierda, 95% desde arriba (5% desde abajo)
            float absoluteX = viewportSize.X * 0.05f;
            float absoluteY = viewportSize.Y * 0.95f;
            
            // Asegurar que el sprite quepa completamente en pantalla
            absoluteX = Mathf.Max(spriteSize.X * 0.5f, absoluteX); // No salirse por la izquierda
            absoluteY = Mathf.Min(viewportSize.Y - spriteSize.Y * 0.5f, absoluteY); // No salirse por abajo
            absoluteY = Mathf.Max(spriteSize.Y * 0.5f, absoluteY); // No salirse por arriba
            
            Position = new Vector2(absoluteX, absoluteY);
            _basePosition = Position;
            
            GD.Print($"[{CharacterName}] Posición por defecto establecida: {Position}");
        }
        
        /// <summary>
        /// Configura el sprite del personaje
        /// Debe ser implementado por las clases derivadas para cargar la textura específica
        /// </summary>
        protected abstract void SetupSprite();
        
        /// <summary>
        /// Cambia la emoción del personaje
        /// </summary>
        /// <param name="emotion">Nueva emoción</param>
        public virtual void ChangeEmotion(Emotion emotion)
        {
            CurrentEmotion = emotion;
            UpdateEmotionVisual();
            GD.Print($"{CharacterName} cambió de emoción a: {emotion}");
        }
        
        /// <summary>
        /// Actualiza la apariencia visual según la emoción
        /// Puede ser sobrescrito por clases derivadas para personalizar los colores
        /// </summary>
        protected virtual void UpdateEmotionVisual()
        {
            if (_sprite == null) return;
            
            // Aplicar efectos visuales según la emoción usando modulación de color
            switch (CurrentEmotion)
            {
                case Emotion.Happy:
                    _sprite.Modulate = new Color(1.1f, 1.1f, 0.9f, 1f); // Más cálido y brillante
                    break;
                case Emotion.Sad:
                    _sprite.Modulate = new Color(0.8f, 0.8f, 1.0f, 1f); // Más frío
                    break;
                case Emotion.Surprised:
                    _sprite.Modulate = new Color(1.2f, 1.1f, 1.0f, 1f); // Más brillante
                    break;
                case Emotion.Angry:
                    _sprite.Modulate = new Color(1.1f, 0.9f, 0.9f, 1f); // Más rojizo
                    break;
                case Emotion.Confused:
                    _sprite.Modulate = new Color(0.9f, 0.9f, 1.1f, 1f); // Ligeramente azulado
                    break;
                case Emotion.Neutral:
                default:
                    _sprite.Modulate = Colors.White;
                    break;
            }
        }
        
        /// <summary>
        /// Hace que el personaje aparezca con animación
        /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
        /// </summary>
        /// <param name="duration">Duración de la animación</param>
        public virtual void Appear(float duration = 1.0f)
        {
            // CRÍTICO: Limpiar todos los tweens activos antes de hacer la animación
            // Esto evita que animaciones de hablar o pulse interfieran con la animación de aparecer
            CleanupTweens();
            
            // CRÍTICO: Asegurar que el nodo sea visible ANTES de hacer la animación
            Visible = true;
            
            if (_sprite != null && IsInstanceValid(_sprite))
            {
                // Asegurar que el sprite también sea visible
                _sprite.Visible = true;
                
                // Iniciar desde transparente
                _sprite.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                
                // CRÍTICO: Obtener el scale objetivo correctamente
                // Si _baseScale está establecido, usarlo; si no, usar el Scale actual o un valor por defecto
                Vector2 targetScale;
                if (_baseScale != Vector2.Zero && _baseScale.X > 0.1f && _baseScale.Y > 0.1f)
                {
                    // Usar el scale base si está establecido
                    targetScale = _baseScale;
                }
                else
                {
                    // Si no hay baseScale, usar el Scale actual o un valor por defecto
                    targetScale = Scale;
                    if (targetScale == Vector2.Zero || (targetScale.X <= 0.1f && targetScale.Y <= 0.1f))
                    {
                        targetScale = new Vector2(1.0f, 1.0f);
                    }
                    // Guardar como baseScale para futuras animaciones
                    _baseScale = targetScale;
                }
                
                // Iniciar desde un scale más pequeño para el efecto de aparecer
                Vector2 startScale = targetScale * 0.5f;
                Scale = startScale;
                
                // Crear tween para la animación de aparecer
                var tween = CreateTween();
                tween.Parallel();
                tween.TweenProperty(_sprite, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), duration);
                tween.TweenProperty(this, "scale", targetScale, duration);
                tween.SetEase(Tween.EaseType.Out);
                tween.SetTrans(Tween.TransitionType.Back);
                
                tween.TweenCallback(Callable.From(() => {
                    // Asegurar que el scale final sea el correcto
                    Scale = targetScale;
                    _baseScale = targetScale;
                    // Iniciar animación idle después de aparecer
                    StartIdleAnimation();
                    GD.Print($"[{CharacterName}] {CharacterName} ha aparecido completamente - Scale final: {Scale}");
                }));
            }
            else
            {
                // Si no hay sprite, simplemente hacer visible
                GD.Print($"[{CharacterName}] Appear llamado pero no hay sprite disponible");
            }
        }
        
        /// <summary>
        /// Inicia animación de movimiento continuo (flotación suave)
        /// </summary>
        protected virtual void StartIdleAnimation()
        {
            GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation llamado");
            GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation - Position actual: {Position}, _basePosition: {_basePosition}");
            GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation - Estado: _movementTween válido: {(_movementTween != null && _movementTween.IsValid())}, _idleTween válido: {(_idleTween != null && _idleTween.IsValid())}");
            
            // CRÍTICO: Detener cualquier tween de movimiento activo antes de iniciar idle
            if (_movementTween != null && _movementTween.IsValid())
            {
                GD.Print($"[{CharacterName}] 🛑 StartIdleAnimation - Matando _movementTween");
                _movementTween.Kill();
                _movementTween = null;
            }
            
            if (_idleTween != null && _idleTween.IsValid())
            {
                GD.Print($"[{CharacterName}] 🛑 StartIdleAnimation - Matando _idleTween anterior");
                _idleTween.Kill();
            }
            
            // CRÍTICO: Si _basePosition ya está establecido (por ejemplo, después de un movimiento),
            // usarlo directamente. Si no, usar Position actual.
            if (_basePosition == Vector2.Zero)
            {
                GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation - _basePosition es Zero, estableciendo a Position: {Position}");
            _basePosition = Position;
            }
            else
            {
                GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation - _basePosition ya establecido: {_basePosition}, NO sobrescribiendo");
            }
            // Si _basePosition ya está establecido, no sobrescribirlo con Position
            // Esto evita que se capture una posición incorrecta después de un movimiento
            
            if (_baseScale == Vector2.Zero)
            {
                _baseScale = Scale;
            }
            
            GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation - Creando _idleTween desde _basePosition: {_basePosition}");
            _idleTween = CreateTween();
            _idleTween.SetLoops();
            
            // CRÍTICO: Usar _basePosition directamente, no actualizarlo desde Position
            // Esto asegura que la animación idle use la posición correcta establecida después del movimiento
            
            _idleTween.TweenProperty(this, "position", _basePosition + new Vector2(0, -10), 1.5f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            _idleTween.TweenProperty(this, "position", _basePosition + new Vector2(0, 10), 1.5f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            
            GD.Print($"[{CharacterName}] 🎭 StartIdleAnimation completado - _idleTween creado");
        }
        
        /// <summary>
        /// Animación de hablar (usa Pulse para efecto squash and stretch)
        /// </summary>
        public virtual void PlaySpeakingAnimation()
        {
            GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation llamado");
            GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - Position actual: {Position}, _basePosition: {_basePosition}");
            GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - Estado: _movementTween válido: {(_movementTween != null && _movementTween.IsValid())}, _idleTween válido: {(_idleTween != null && _idleTween.IsValid())}");
            
            // CRÍTICO: Detener idle antes de iniciar speaking
            if (_idleTween != null && _idleTween.IsValid())
            {
                GD.Print($"[{CharacterName}] 🛑 PlaySpeakingAnimation - Matando _idleTween");
                _idleTween.Kill();
            }
            
            // CRÍTICO: NO sobrescribir _basePosition si ya está establecido (después de un movimiento)
            // Si hay un movimiento en progreso, NO capturar _basePosition todavía
            // Esperar a que el movimiento termine para usar la posición final correcta
            if (_movementTween == null || !_movementTween.IsValid())
            {
                GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - NO hay movimiento activo");
                // No hay movimiento activo, proceder normalmente
                // Solo actualizar _basePosition si no está establecido
                if (_basePosition == Vector2.Zero)
                {
                    GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - _basePosition es Zero, estableciendo a Position: {Position}");
            _basePosition = Position;
                }
                else
                {
                    GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - _basePosition ya establecido: {_basePosition}, NO sobrescribiendo");
                }
                // Si _basePosition ya está establecido, NO sobrescribirlo
                // Esto asegura que la animación speaking use la posición correcta establecida después del movimiento
            }
            else
            {
                GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - HAY movimiento activo, NO actualizando _basePosition todavía");
            }
            // Si hay movimiento activo, NO actualizar _basePosition todavía
            // El callback del movimiento se encargará de establecer _basePosition correctamente
            
            // CRÍTICO: Iniciar el pulse solo si el personaje NO está agrandado
            // Si está agrandado, el pulse se iniciará después de que termine el agrandamiento en EnlargeScale()
            // Esto evita conflictos entre el tween de agrandamiento y el pulse
            if (!_isEnlargedByDialog)
            {
                PlayPulseAnimation();
            }
            // Si está agrandado, EnlargeScale() se encargará de iniciar el pulse después del tween
            
            // CRÍTICO: Solo crear tween de posición speaking si NO hay un movimiento en progreso
            // Si hay un movimiento, marcar que se debe iniciar speaking después
            if (_movementTween == null || !_movementTween.IsValid())
            {
                GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - NO hay movimiento activo, iniciando StartSpeakingPositionAnimation inmediatamente");
                // No hay movimiento activo, iniciar speaking inmediatamente
                StartSpeakingPositionAnimation();
            }
            else
            {
                GD.Print($"[{CharacterName}] 💬 PlaySpeakingAnimation - HAY movimiento activo, marcando _shouldStartSpeakingAfterMovement = true");
                // Hay movimiento activo, marcar que se debe iniciar speaking cuando termine
                _shouldStartSpeakingAfterMovement = true;
            }
        }
        
        /// <summary>
        /// Inicia la animación de posición para speaking (separado para evitar duplicación)
        /// </summary>
        private void StartSpeakingPositionAnimation()
        {
            GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation llamado");
            GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation - Position actual: {Position}, _basePosition: {_basePosition}");
            
            // CRÍTICO: Matar cualquier tween de speaking anterior si existe
            if (_speakingPositionTween != null && _speakingPositionTween.IsValid())
            {
                GD.Print($"[{CharacterName}] 🛑 StartSpeakingPositionAnimation - Matando _speakingPositionTween anterior");
                _speakingPositionTween.Kill();
                _speakingPositionTween = null;
            }
            
            // CRÍTICO: NO establecer Position = _basePosition aquí
            // Position ya debería estar en _basePosition después del movimiento
            // Establecerlo aquí causaría un salto visual
            
            // CRÍTICO: Asegurar que _basePosition esté actualizado a la posición actual
            // Esto evita que la animación comience desde una posición incorrecta
            // Pero solo si _basePosition no está establecido o está en Vector2.Zero
            if (_basePosition == Vector2.Zero)
            {
                GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation - _basePosition es Zero, estableciendo a Position: {Position}");
                _basePosition = Position;
            }
            else
            {
                // Si _basePosition ya está establecido, verificar si Position está cerca
                // Si están muy lejos, actualizar _basePosition a Position actual
                float distance = Position.DistanceTo(_basePosition);
                GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation - Distancia entre Position y _basePosition: {distance}");
                if (distance > 10.0f) // Si están a más de 10px de distancia, actualizar
                {
                    GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation - Distancia > 10px, actualizando _basePosition de {_basePosition} a {Position}");
                    _basePosition = Position;
                }
                else
                {
                    GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation - Distancia <= 10px, NO actualizando _basePosition");
                }
            }
            
            GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation - Creando _speakingPositionTween desde _basePosition: {_basePosition}");
            _speakingPositionTween = CreateTween();
            _speakingPositionTween.SetLoops();
            
            // CRÍTICO: Usar _basePosition directamente para la animación
            // Esto asegura que la animación comience desde la posición correcta establecida después del movimiento
            _speakingPositionTween.TweenProperty(this, "position", _basePosition + new Vector2(0, -15), 0.8f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            _speakingPositionTween.TweenProperty(this, "position", _basePosition + new Vector2(0, 5), 0.8f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            
            GD.Print($"[{CharacterName}] 🗣️ StartSpeakingPositionAnimation completado - _speakingPositionTween creado");
        }
        
        /// <summary>
        /// Detiene la animación de hablar y vuelve a la animación idle
        /// </summary>
        public virtual void StopSpeakingAnimation()
        {
            if (_idleTween != null && _idleTween.IsValid())
            {
                _idleTween.Kill();
            }
            StopStretchAnimation();
            StartIdleAnimation();
        }
        
        /// <summary>
        /// Pulse (estiramiento + compresión alternada) - Pequeña oscilación (stretch → squash → normal)
        /// </summary>
        /// <param name="pulseDuration">Duración total del pulse</param>
        protected virtual void PlayPulseAnimation(float pulseDuration = 5.5f)
        {
            StopStretchAnimation();
            
            // CRÍTICO: Determinar el tamaño base para el pulse
            // Si el personaje está agrandado, SIEMPRE usar el tamaño agrandado guardado
            // Si no está agrandado, usar el Scale actual
            Vector2 currentScale;
            
            if (_isEnlargedByDialog && _enlargedScale != Vector2.Zero)
            {
                // CRÍTICO: Si está agrandado, usar SIEMPRE el tamaño agrandado guardado
                // Esto asegura que el pulse oscile alrededor del tamaño agrandado correcto
                currentScale = _enlargedScale;
                
                // CRÍTICO: Asegurar que el Scale esté en el tamaño agrandado antes de iniciar el pulse
                // Esto evita que el pulse capture un tamaño intermedio
                if (Scale != _enlargedScale)
                {
                    Scale = _enlargedScale;
                }
            }
            else
            {
                // Si no está agrandado, usar el Scale actual
                currentScale = Scale;
            }
            
            // Calcular las variaciones del pulse sobre el tamaño base determinado
            // Las variaciones son pequeñas (5%) para que se vean naturales
            Vector2 stretchScale = new Vector2(currentScale.X * 0.95f, currentScale.Y * 1.00f);
            Vector2 squashScale = new Vector2(currentScale.X * 1.00f, currentScale.Y * 0.95f);
            
            _squashStretchTween = CreateTween();
            _squashStretchTween.SetLoops();
            _squashStretchTween.TweenProperty(this, "scale", stretchScale, pulseDuration * 0.33f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            _squashStretchTween.TweenProperty(this, "scale", squashScale, pulseDuration * 0.33f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            _squashStretchTween.TweenProperty(this, "scale", currentScale, pulseDuration * 0.34f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
        }
        
        /// <summary>
        /// Detiene todas las animaciones de squash and stretch activas
        /// </summary>
        protected virtual void StopStretchAnimation()
        {
            if (_squashStretchTween != null && _squashStretchTween.IsValid())
            {
                _squashStretchTween.Kill();
                _squashStretchTween = null;
            }
            
            if (_baseScale != Vector2.Zero)
            {
                Scale = _baseScale;
            }
        }
        
        /// <summary>
        /// Limpia todos los tweens activos
        /// </summary>
        public virtual void CleanupTweens()
        {
            if (_idleTween != null && _idleTween.IsValid())
            {
                _idleTween.Kill();
                _idleTween = null;
            }
            
            if (_movementTween != null && _movementTween.IsValid())
            {
                _movementTween.Kill();
                _movementTween = null;
            }
            
            StopStretchAnimation();
        }
        
        /// <summary>
        /// Hace que el personaje desaparezca con animación
        /// </summary>
        /// <param name="duration">Duración de la animación</param>
        public virtual void Disappear(float duration = 1.0f)
        {
            CleanupTweens();
            
            var tween = CreateTween();
            tween.Parallel();
            tween.TweenProperty(this, "modulate", new Color(1.0f, 1.0f, 1.0f, 0.0f), duration);
            tween.TweenProperty(this, "scale", new Vector2(0.5f, 0.5f), duration);
            tween.SetEase(Tween.EaseType.In);
            tween.SetTrans(Tween.TransitionType.Quart);
            
            tween.TweenCallback(Callable.From(() => {
                if (IsInstanceValid(this))
                {
                    QueueFree();
                }
            }));
        }
        
        /// <summary>
        /// Oculta el personaje sin eliminarlo (para poder reaparecer después)
        /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
        /// </summary>
        /// <param name="duration">Duración de la animación de ocultamiento</param>
        public virtual void Hide(float duration = 0.5f)
        {
            CleanupTweens();
            StopStretchAnimation();

                var tween = CreateTween();
                tween.Parallel();
            tween.TweenProperty(this, "modulate", new Color(1.0f, 1.0f, 1.0f, 0.0f), duration);
                tween.SetEase(Tween.EaseType.In);
                tween.SetTrans(Tween.TransitionType.Quart);
                tween.TweenCallback(Callable.From(() => {
                    if (IsInstanceValid(this))
                    {
                    _sprite.Visible = false;
                    }
                }));
            }
        
        /// <summary>
        /// Verifica si el personaje está visible (sprite visible y alpha > 0.1)
        /// </summary>
        /// <returns>True si el personaje está visible</returns>
        public virtual bool IsCharacterVisible()
        {
            if (_sprite == null)
            {
                return false;
            }
            
            // Verificar si el sprite está visible y el alpha es suficientemente alto
            return _sprite.Visible && Modulate.A > 0.1f;
        }
        
        public virtual void ShowCharacter(float duration = 0.5f)
        {
            if (_sprite == null)
            {
                GD.PrintErr($"[Character] ShowCharacter: _sprite es null para {CharacterId}");
                return;
            }
            
            // CRÍTICO: Cancelar cualquier animación de ocultamiento en progreso
            // Esto asegura que si Hide() estaba en progreso, se cancele inmediatamente
            CleanupTweens();
            
            _sprite.Visible = true;
            var tween = CreateTween();
            tween.Parallel();
            tween.TweenProperty(this, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), duration);
            tween.SetEase(Tween.EaseType.Out);
            tween.SetTrans(Tween.TransitionType.Quart);
        }
        
        /// <summary>
        /// 1. Squash (compresión vertical) - El personaje se aplasta ligeramente (menor altura, más ancho)
        /// </summary>
        /// <param name="squashDuration">Duración del squash</param>
        /// <param name="recoveryDuration">Duración de recuperación</param>
        public virtual void PlaySquashAnimation(float squashDuration = 0.2f, float recoveryDuration = 0.25f)
        {
            StopStretchAnimation();
            
            _baseScale = Scale;
            Vector2 squashScale = new Vector2(_baseScale.X * 1.15f, _baseScale.Y * 0.85f);
            
            _squashStretchTween = CreateTween();
            _squashStretchTween.TweenProperty(this, "scale", squashScale, squashDuration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Sine);
            _squashStretchTween.TweenProperty(this, "scale", _baseScale, recoveryDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Sine);
        }
        
        /// <summary>
        /// 2. Stretch (estiramiento vertical) - El personaje se alarga ligeramente (mayor altura, más estrecho)
        /// </summary>
        /// <param name="stretchDuration">Duración del stretch</param>
        /// <param name="recoveryDuration">Duración de recuperación</param>
        public virtual void PlayStretchAnimation(float stretchDuration = 0.15f, float recoveryDuration = 0.15f)
        {
            StopStretchAnimation();
            
            _baseScale = Scale;
            Vector2 stretchScale = new Vector2(_baseScale.X * 0.85f, _baseScale.Y * 1.15f);
            
            _squashStretchTween = CreateTween();
            _squashStretchTween.TweenProperty(this, "scale", stretchScale, stretchDuration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Back);
            _squashStretchTween.TweenProperty(this, "scale", _baseScale, recoveryDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);
        }
        
        /// <summary>
        /// 4. Recoil (retroceso con squash leve) - Ligera compresión seguida de desplazamiento hacia atrás
        /// </summary>
        /// <param name="squashDuration">Duración del squash</param>
        /// <param name="recoveryDuration">Duración de recuperación</param>
        public virtual void PlayRecoilAnimation(float squashDuration = 0.25f, float recoveryDuration = 0.2f)
        {
            StopStretchAnimation();
            
            _basePosition = Position;
            _baseScale = Scale;
            Vector2 squashScale = new Vector2(_baseScale.X * 1.1f, _baseScale.Y * 0.9f);
            Vector2 recoilPosition = _basePosition + new Vector2(-20, 0);
            
            _squashStretchTween = CreateTween();
            _squashStretchTween.Parallel();
            _squashStretchTween.TweenProperty(this, "scale", squashScale, squashDuration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Quart);
            _squashStretchTween.TweenProperty(this, "position", recoilPosition, squashDuration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Quart);
            
            _squashStretchTween.TweenProperty(this, "scale", _baseScale, recoveryDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);
            _squashStretchTween.TweenProperty(this, "position", _basePosition, recoveryDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);
        }
        
        /// <summary>
        /// 5. Pop-in (stretch inicial al aparecer) - Efecto de estiramiento rápido al entrar en escena
        /// </summary>
        /// <param name="stretchDuration">Duración del stretch</param>
        /// <param name="squashDuration">Duración del squash</param>
        /// <param name="recoveryDuration">Duración de recuperación</param>
        public virtual void PlayPopInAnimation(float stretchDuration = 0.1f, float squashDuration = 0.15f, float recoveryDuration = 0.1f)
        {
            StopStretchAnimation();
            
            _baseScale = Scale;
            Vector2 stretchScale = new Vector2(_baseScale.X * 0.8f, _baseScale.Y * 1.2f);
            Vector2 squashScale = new Vector2(_baseScale.X * 1.1f, _baseScale.Y * 0.9f);
            
            _squashStretchTween = CreateTween();
            _squashStretchTween.TweenProperty(this, "scale", stretchScale, stretchDuration)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Back);
            _squashStretchTween.TweenProperty(this, "scale", squashScale, squashDuration)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Elastic);
            _squashStretchTween.TweenProperty(this, "scale", _baseScale, recoveryDuration)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Back);
        }
        
        /// <summary>
        /// 6. Heart Bounce (squash rítmico sincronizado) - Pequeña oscilación acompasada (como un pulso cardíaco)
        /// </summary>
        /// <param name="cycleDuration">Duración por ciclo</param>
        public virtual void PlayHeartBounceAnimation(float cycleDuration = 0.9f)
        {
            StopStretchAnimation();
            
            _baseScale = Scale;
            Vector2 squashScale = new Vector2(_baseScale.X * 1.05f, _baseScale.Y * 0.95f);
            
            _squashStretchTween = CreateTween();
            _squashStretchTween.SetLoops();
            _squashStretchTween.TweenProperty(this, "scale", squashScale, cycleDuration * 0.5f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            _squashStretchTween.TweenProperty(this, "scale", _baseScale, cycleDuration * 0.5f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
        }
        
        /// <summary>
        /// Mueve el personaje al borde izquierdo, totalmente abajo pero sin salirse de la pantalla
        /// </summary>
        /// <param name="isEffect">Si es true, hace el desplazamiento suave. Si es false, instantáneo. Por defecto true</param>
        public virtual void LeftBorder(bool isEffect = true)
        {
            GD.Print($"[{CharacterName}] 🚀 LeftBorder llamado - isEffect: {isEffect}, Position actual: {Position}, _basePosition: {_basePosition}");
            MoveToBorderPosition(0.05f, isEffect); // 5% desde la izquierda
        }
        
        /// <summary>
        /// Mueve el personaje al borde derecho, totalmente abajo pero sin salirse de la pantalla
        /// </summary>
        /// <param name="isEffect">Si es true, hace el desplazamiento suave. Si es false, instantáneo. Por defecto true</param>
        public virtual void RightBorder(bool isEffect = true)
        {
            GD.Print($"[{CharacterName}] 🚀 RightBorder llamado - isEffect: {isEffect}, Position actual: {Position}, _basePosition: {_basePosition}");
            MoveToBorderPosition(0.95f, isEffect); // 95% desde la izquierda (casi al borde derecho)
        }
        
        /// <summary>
        /// Mueve el personaje al centro, totalmente abajo pero sin salirse de la pantalla
        /// </summary>
        /// <param name="isEffect">Si es true, hace el desplazamiento suave. Si es false, instantáneo. Por defecto true</param>
        public virtual void Center(bool isEffect = true)
        {
            GD.Print($"[{CharacterName}] 🚀 Center llamado - isEffect: {isEffect}, Position actual: {Position}, _basePosition: {_basePosition}");
            MoveToBorderPosition(0.5f, isEffect); // 50% desde la izquierda (centro)
        }
        
        /// <summary>
        /// Mueve el personaje a una posición personalizada basada en porcentajes del viewport
        /// </summary>
        /// <param name="xPercent">Porcentaje horizontal (0.0 a 1.0, donde 0.0 es izquierda y 1.0 es derecha)</param>
        /// <param name="yPercent">Porcentaje vertical (0.0 a 1.0, donde 0.0 es arriba y 1.0 es abajo)</param>
        /// <param name="isEffect">Si es true, hace el desplazamiento suave. Si es false, instantáneo. Por defecto true</param>
        public virtual void CustomPercentPosition(float xPercent, float yPercent, bool isEffect = true)
        {
            GD.Print($"[{CharacterName}] 🚀 CustomPercentPosition llamado - xPercent: {xPercent}, yPercent: {yPercent}, isEffect: {isEffect}, Position actual: {Position}, _basePosition: {_basePosition}");
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            
            // Obtener el tamaño del sprite escalado para asegurar que quepa en pantalla
            Vector2 spriteSize = Vector2.Zero;
            if (_sprite != null && _sprite.Texture != null)
            {
                spriteSize = _sprite.Texture.GetSize() * Scale;
            }
            
            // Calcular posición absoluta desde porcentajes
            float absoluteX = viewportSize.X * Mathf.Clamp(xPercent, 0.0f, 1.0f);
            float absoluteY = viewportSize.Y * Mathf.Clamp(yPercent, 0.0f, 1.0f);
            
            // Asegurar que el sprite quepa completamente en pantalla
            absoluteX = Mathf.Max(spriteSize.X * 0.5f, absoluteX); // No salirse por la izquierda
            absoluteX = Mathf.Min(viewportSize.X - spriteSize.X * 0.5f, absoluteX); // No salirse por la derecha
            absoluteY = Mathf.Min(viewportSize.Y - spriteSize.Y * 0.5f, absoluteY); // No salirse por abajo
            absoluteY = Mathf.Max(spriteSize.Y * 0.5f, absoluteY); // No salirse por arriba
            
            Vector2 targetPosition = new Vector2(absoluteX, absoluteY);
            
            GD.Print($"[{CharacterName}] 📍 CustomPercentPosition - Posición objetivo calculada: {targetPosition}, desde Position: {Position}");
            
            if (isEffect)
            {
                GD.Print($"[{CharacterName}] ⚙️ CustomPercentPosition - Iniciando movimiento suave");
                GD.Print($"[{CharacterName}] ⚙️ CustomPercentPosition - Estado antes: _idleTween válido: {(_idleTween != null && _idleTween.IsValid())}, _movementTween válido: {(_movementTween != null && _movementTween.IsValid())}, _speakingPositionTween válido: {(_speakingPositionTween != null && _speakingPositionTween.IsValid())}");
                
                // CRÍTICO: Detener animaciones de idle/speaking antes de mover para evitar conflictos
                if (_idleTween != null && _idleTween.IsValid())
                {
                    GD.Print($"[{CharacterName}] 🛑 CustomPercentPosition - Matando _idleTween");
                    _idleTween.Kill();
                    _idleTween = null;
                }
                
                // CRÍTICO: Detener animación de speaking position antes de mover
                if (_speakingPositionTween != null && _speakingPositionTween.IsValid())
                {
                    GD.Print($"[{CharacterName}] 🛑 CustomPercentPosition - Matando _speakingPositionTween");
                    _speakingPositionTween.Kill();
                    _speakingPositionTween = null;
                }
                
                // Movimiento suave
                if (_movementTween != null && _movementTween.IsValid())
                {
                    GD.Print($"[{CharacterName}] 🛑 CustomPercentPosition - Matando _movementTween anterior");
                    _movementTween.Kill();
                }
                
                GD.Print($"[{CharacterName}] 🎬 CustomPercentPosition - Creando nuevo _movementTween desde {Position} hacia {targetPosition}");
                _movementTween = CreateTween();
                _movementTween.TweenProperty(this, "position", targetPosition, 1.2f)
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Cubic);
                
                _movementTween.TweenCallback(Callable.From(() => {
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: Movimiento terminado");
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: Position ANTES de actualizar: {Position}");
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: targetPosition: {targetPosition}");
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: _basePosition ANTES de actualizar: {_basePosition}");
                    
                    // CRÍTICO: Usar targetPosition directamente, no Position, para evitar diferencias por animaciones
                    Position = targetPosition;
                    _basePosition = targetPosition;
                    
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: Position DESPUÉS de actualizar: {Position}");
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: _basePosition DESPUÉS de actualizar: {_basePosition}");
                    
                    // CRÍTICO: Limpiar el tween de movimiento
                    _movementTween = null;
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: _movementTween limpiado (null)");
                    
                    // CRÍTICO: Si se debe iniciar speaking después del movimiento, hacerlo ahora con CallDeferred
                    // Esto asegura que Position esté completamente establecido antes de iniciar speaking
                    if (_shouldStartSpeakingAfterMovement)
                    {
                        GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: _shouldStartSpeakingAfterMovement = true, iniciando StartSpeakingPositionAnimation con CallDeferred");
                        _shouldStartSpeakingAfterMovement = false;
                        CallDeferred(MethodName.StartSpeakingPositionAnimation);
                    }
                    else
                    {
                        GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK: _shouldStartSpeakingAfterMovement = false, NO iniciando speaking");
                    }
                    
                    GD.Print($"[{CharacterName}] ✅ CustomPercentPosition - CALLBACK completado - Movido a posición personalizada: {targetPosition} (x: {xPercent * 100}%, y: {yPercent * 100}%)");
                }));
            }
            else
            {
                // Movimiento instantáneo
                GD.Print($"[{CharacterName}] ⚡ CustomPercentPosition - Movimiento instantáneo");
                GD.Print($"[{CharacterName}] ⚡ CustomPercentPosition - Position ANTES: {Position}, targetPosition: {targetPosition}");
                Position = targetPosition;
                _basePosition = Position;
                GD.Print($"[{CharacterName}] ⚡ CustomPercentPosition - Position DESPUÉS: {Position}, _basePosition: {_basePosition}");
                GD.Print($"[{CharacterName}] ⚡ Movido instantáneamente a posición personalizada: {targetPosition} (x: {xPercent * 100}%, y: {yPercent * 100}%)");
            }
        }
        
        /// <summary>
        /// Método auxiliar para mover el personaje a una posición de borde (izquierda, derecha o centro)
        /// </summary>
        /// <param name="xPercent">Porcentaje horizontal (0.05 para izquierda, 0.5 para centro, 0.95 para derecha)</param>
        /// <param name="isEffect">Si es true, hace el desplazamiento suave. Si es false, instantáneo</param>
        private void MoveToBorderPosition(float xPercent, bool isEffect)
        {
            GD.Print($"[{CharacterName}] 🔧 MoveToBorderPosition llamado - xPercent: {xPercent}, isEffect: {isEffect}, Position actual: {Position}, _basePosition: {_basePosition}");
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            
            // Obtener el tamaño del sprite escalado para asegurar que quepa en pantalla
            Vector2 spriteSize = Vector2.Zero;
            if (_sprite != null && _sprite.Texture != null)
            {
                spriteSize = _sprite.Texture.GetSize() * Scale;
            }
            
            // Calcular posición: xPercent horizontalmente, 95% desde arriba (5% desde abajo)
            float absoluteX = viewportSize.X * xPercent;
            float absoluteY = viewportSize.Y * 0.95f; // Totalmente abajo
            
            // Asegurar que el sprite quepa completamente en pantalla
            absoluteX = Mathf.Max(spriteSize.X * 0.5f, absoluteX); // No salirse por la izquierda
            absoluteX = Mathf.Min(viewportSize.X - spriteSize.X * 0.5f, absoluteX); // No salirse por la derecha
            absoluteY = Mathf.Min(viewportSize.Y - spriteSize.Y * 0.5f, absoluteY); // No salirse por abajo
            absoluteY = Mathf.Max(spriteSize.Y * 0.5f, absoluteY); // No salirse por arriba
            
            Vector2 targetPosition = new Vector2(absoluteX, absoluteY);
            
            GD.Print($"[{CharacterName}] 📍 MoveToBorderPosition - Posición objetivo calculada: {targetPosition}, desde Position: {Position}");
            
            if (isEffect)
            {
                GD.Print($"[{CharacterName}] ⚙️ MoveToBorderPosition - Iniciando movimiento suave");
                GD.Print($"[{CharacterName}] ⚙️ MoveToBorderPosition - Estado antes: _idleTween válido: {(_idleTween != null && _idleTween.IsValid())}, _movementTween válido: {(_movementTween != null && _movementTween.IsValid())}, _speakingPositionTween válido: {(_speakingPositionTween != null && _speakingPositionTween.IsValid())}");
                
                // CRÍTICO: Detener animaciones de idle/speaking antes de mover para evitar conflictos
                if (_idleTween != null && _idleTween.IsValid())
                {
                    GD.Print($"[{CharacterName}] 🛑 MoveToBorderPosition - Matando _idleTween");
                    _idleTween.Kill();
                    _idleTween = null;
                }
                
                // CRÍTICO: Detener animación de speaking position antes de mover
                if (_speakingPositionTween != null && _speakingPositionTween.IsValid())
                {
                    GD.Print($"[{CharacterName}] 🛑 MoveToBorderPosition - Matando _speakingPositionTween");
                    _speakingPositionTween.Kill();
                    _speakingPositionTween = null;
                }
                
                // Movimiento suave
                if (_movementTween != null && _movementTween.IsValid())
                {
                    GD.Print($"[{CharacterName}] 🛑 MoveToBorderPosition - Matando _movementTween anterior");
                    _movementTween.Kill();
                }
                
                GD.Print($"[{CharacterName}] 🎬 MoveToBorderPosition - Creando nuevo _movementTween desde {Position} hacia {targetPosition}");
                _movementTween = CreateTween();
                _movementTween.TweenProperty(this, "position", targetPosition, 1.2f)
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Cubic);
                
                _movementTween.TweenCallback(Callable.From(() => {
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: Movimiento terminado");
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: Position ANTES de actualizar: {Position}");
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: targetPosition: {targetPosition}");
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: _basePosition ANTES de actualizar: {_basePosition}");
                    
                    // CRÍTICO: Usar targetPosition directamente, no Position, para evitar diferencias por animaciones
                    Position = targetPosition;
                    _basePosition = targetPosition;
                    
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: Position DESPUÉS de actualizar: {Position}");
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: _basePosition DESPUÉS de actualizar: {_basePosition}");
                    
                    // CRÍTICO: Limpiar el tween de movimiento
                    _movementTween = null;
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: _movementTween limpiado (null)");
                    
                    // CRÍTICO: Si se debe iniciar speaking después del movimiento, hacerlo ahora con CallDeferred
                    // Esto asegura que Position esté completamente establecido antes de iniciar speaking
                    if (_shouldStartSpeakingAfterMovement)
                    {
                        GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: _shouldStartSpeakingAfterMovement = true, iniciando StartSpeakingPositionAnimation con CallDeferred");
                        _shouldStartSpeakingAfterMovement = false;
                        CallDeferred(MethodName.StartSpeakingPositionAnimation);
                    }
                    else
                    {
                        GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK: _shouldStartSpeakingAfterMovement = false, NO iniciando speaking");
                    }
                    
                    GD.Print($"[{CharacterName}] ✅ MoveToBorderPosition - CALLBACK completado - Movido a borde: {targetPosition}");
                }));
            }
            else
            {
                // Movimiento instantáneo
                GD.Print($"[{CharacterName}] ⚡ MoveToBorderPosition - Movimiento instantáneo");
                GD.Print($"[{CharacterName}] ⚡ MoveToBorderPosition - Position ANTES: {Position}, targetPosition: {targetPosition}");
                Position = targetPosition;
                _basePosition = Position;
                GD.Print($"[{CharacterName}] ⚡ MoveToBorderPosition - Position DESPUÉS: {Position}, _basePosition: {_basePosition}");
                GD.Print($"[{CharacterName}] ⚡ Movido instantáneamente a borde: {targetPosition}");
            }
        }
    }
}

