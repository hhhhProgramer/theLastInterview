using System;
using Godot;

namespace Package.Background
{
    /// <summary>
    /// Componente para agregar un background de pantalla completa a cualquier escena
    /// Usa TextureRect con FullRect para ajustarse automáticamente al tamaño del viewport
    /// Siguiendo las mejores prácticas SOLID, KISS, SRP, DRY
    /// </summary>
    public partial class SceneBackground : Control
    {
        /// <summary>
        /// TextureRect que muestra el background
        /// </summary>
        private TextureRect _backgroundTextureRect;
        
        /// <summary>
        /// ColorRect de fallback (fondo sólido detrás de la imagen)
        /// </summary>
        private ColorRect _solidBackground;
        
        /// <summary>
        /// Ruta de la imagen de background
        /// </summary>
        private string _backgroundImagePath;
        
        /// <summary>
        /// Color de fallback si no se carga la imagen
        /// </summary>
        private Color _fallbackColor = new Color(0.1f, 0.1f, 0.1f, 1.0f);
        
        /// <summary>
        /// Indica si el background está configurado
        /// </summary>
        public bool IsConfigured { get; private set; } = false;
        
        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public SceneBackground()
        {
            Name = "SceneBackground";
        }
        
        public override void _Ready()
        {
            base._Ready();
            
            // Configurar Control raíz para que ocupe toda la pantalla
            SetupRootControl();
            
            // Si ya se configuró el background antes de _Ready, aplicarlo
            if (!string.IsNullOrEmpty(_backgroundImagePath))
            {
                SetBackground(_backgroundImagePath);
            }
        }
        
        /// <summary>
        /// Configura el Control raíz para que ocupe toda la pantalla
        /// </summary>
        private void SetupRootControl()
        {
            // Usar FullRect para que ocupe toda la pantalla
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            
            // CRÍTICO: No bloquear clicks - permitir que pasen a través
            MouseFilter = Control.MouseFilterEnum.Ignore;
            
            // Asegurar que esté detrás de todo
            ZIndex = -10; // Muy bajo para estar detrás de todo
        }
        
        /// <summary>
        /// Establece el background de la escena
        /// </summary>
        /// <param name="imagePath">Ruta de la imagen de background</param>
        /// <param name="fallbackColor">Color de fallback si no se carga la imagen (opcional)</param>
        public void SetBackground(string imagePath, Color? fallbackColor = null)
        {
            _backgroundImagePath = imagePath;
            
            if (fallbackColor.HasValue)
            {
                _fallbackColor = fallbackColor.Value;
            }
            
            // Si aún no está en el árbol, guardar la ruta y aplicar en _Ready
            if (!IsInsideTree())
            {
                return;
            }
            
            // Aplicar background inmediatamente
            ApplyBackground();
        }
        
        /// <summary>
        /// Aplica el background configurado
        /// </summary>
        private void ApplyBackground()
        {
            // Crear fondo sólido de fallback primero (detrás de la imagen)
            CreateSolidBackground();
            
            // Crear TextureRect para la imagen
            CreateBackgroundImage();
            
            IsConfigured = true;
        }
        
        /// <summary>
        /// Crea el fondo sólido de fallback
        /// </summary>
        private void CreateSolidBackground()
        {
            // Si ya existe, no recrearlo
            if (_solidBackground != null && IsInstanceValid(_solidBackground))
            {
                return;
            }
            
            _solidBackground = new ColorRect();
            _solidBackground.Name = "SolidBackground";
            _solidBackground.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _solidBackground.Color = _fallbackColor;
            _solidBackground.ZIndex = -2; // Detrás del background de imagen
            _solidBackground.MouseFilter = Control.MouseFilterEnum.Ignore; // Permitir clicks a través
            
            AddChild(_solidBackground);
            MoveChild(_solidBackground, 0); // Mover al principio
        }
        
        /// <summary>
        /// Crea el TextureRect para la imagen de background
        /// </summary>
        private void CreateBackgroundImage()
        {
            // Si ya existe, actualizar la textura
            if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
            {
                LoadBackgroundTexture();
                return;
            }
            
            _backgroundTextureRect = new TextureRect();
            _backgroundTextureRect.Name = "BackgroundTextureRect";
            _backgroundTextureRect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            
            // Configuración para que la imagen cubra toda la pantalla siempre (como IntroScene)
            // FitWidthProportional: ajusta el ancho al viewport manteniendo proporción
            // KeepAspectCovered: cubre toda el área, recortando si es necesario
            _backgroundTextureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional;
            _backgroundTextureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
            
            // Establecer tamaño mínimo usando constantes
            
            // Asegurar que esté detrás de todo (pero encima del fondo sólido)
            _backgroundTextureRect.ZIndex = -1;
            _backgroundTextureRect.MouseFilter = Control.MouseFilterEnum.Ignore; // Permitir clicks a través
            
            // Cargar textura
            LoadBackgroundTexture();
            
            // Agregar y mover después del fondo sólido
            AddChild(_backgroundTextureRect);
            if (_solidBackground != null)
            {
                MoveChild(_backgroundTextureRect, 1); // Después del fondo sólido
            }
            else
            {
                MoveChild(_backgroundTextureRect, 0);
            }
        }
        
        /// <summary>
        /// Carga la textura del background
        /// </summary>
        private void LoadBackgroundTexture()
        {
            if (string.IsNullOrEmpty(_backgroundImagePath))
            {
                GD.PrintErr("[SceneBackground] No se especificó ruta de imagen de background");
                return;
            }
            
            var texture = GD.Load<Texture2D>(_backgroundImagePath);
            if (texture != null)
            {
                _backgroundTextureRect.Texture = texture;
                GD.Print($"[SceneBackground] Background cargado: {_backgroundImagePath}");
            }
            else
            {
                GD.PrintErr($"[SceneBackground] No se pudo cargar el background: {_backgroundImagePath}");
                // El fondo sólido de fallback se mostrará
            }
        }
        
        /// <summary>
        /// Cambia el background dinámicamente
        /// </summary>
        /// <param name="imagePath">Nueva ruta de la imagen</param>
        public void ChangeBackground(string imagePath)
        {
            _backgroundImagePath = imagePath;
            
            if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
            {
                LoadBackgroundTexture();
            }
            else
            {
                CreateBackgroundImage();
            }
        }
        
        /// <summary>
        /// Establece el color de fallback
        /// </summary>
        /// <param name="color">Color de fallback</param>
        public void SetFallbackColor(Color color)
        {
            _fallbackColor = color;
            
            if (_solidBackground != null && IsInstanceValid(_solidBackground))
            {
                _solidBackground.Color = color;
            }
        }
        
        /// <summary>
        /// Limpia el background (oculta la imagen, muestra solo el fondo sólido)
        /// </summary>
        public void ClearBackground()
        {
            if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
            {
                _backgroundTextureRect.Texture = null;
                _backgroundImagePath = null;
            }
        }
        
        /// <summary>
        /// TextureRect temporal para el fade (nuevo background)
        /// </summary>
        private TextureRect _fadeTextureRect;
        
        /// <summary>
        /// Tween para el fade
        /// </summary>
        private Tween _fadeTween;
        
        /// <summary>
        /// Tween para el efecto de pulso
        /// </summary>
        private Tween _pulseTween;
        
        /// <summary>
        /// Indica si el efecto de pulso está activo
        /// </summary>
        private bool _isPulsing = false;
        
        /// <summary>
        /// Escala base del background (sin pulso)
        /// </summary>
        private Vector2 _baseScale = Vector2.One;
        
        /// <summary>
        /// Cambia el background con un efecto de fade
        /// </summary>
        /// <param name="newImagePath">Ruta de la nueva imagen de background</param>
        /// <param name="fadeDuration">Duración del fade en segundos (por defecto 1.0)</param>
        /// <param name="onFadeFinish">Callback que se ejecuta cuando termina el fade (opcional)</param>
        public void ChangeBackgroundWithFade(string newImagePath, float fadeDuration = 1.0f, System.Action onFadeFinish = null)
        {
            if (string.IsNullOrEmpty(newImagePath))
            {
                GD.PrintErr("[SceneBackground] No se especificó ruta de imagen para ChangeBackgroundWithFade");
                onFadeFinish?.Invoke();
                return;
            }
            
            // Si no hay background actual, cambiar directamente sin fade
            if (_backgroundTextureRect == null || !IsInstanceValid(_backgroundTextureRect))
            {
                SetBackground(newImagePath);
                // Si hay cuadro negro, quitarlo
                FadeOutBlackIfActive();
                onFadeFinish?.Invoke();
                return;
            }
            
            // Cargar la nueva textura
            var newTexture = GD.Load<Texture2D>(newImagePath);
            if (newTexture == null)
            {
                GD.PrintErr($"[SceneBackground] No se pudo cargar la nueva imagen: {newImagePath}");
                onFadeFinish?.Invoke();
                return;
            }
            
            // Cancelar fade anterior si existe
            if (_fadeTween != null && _fadeTween.IsValid())
            {
                _fadeTween.Kill();
            }
            
            // Crear TextureRect temporal para el nuevo background (encima del actual)
            if (_fadeTextureRect == null || !IsInstanceValid(_fadeTextureRect))
            {
                _fadeTextureRect = new TextureRect();
                _fadeTextureRect.Name = "FadeTextureRect";
                _fadeTextureRect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
                _fadeTextureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional;
                _fadeTextureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
                _fadeTextureRect.ZIndex = 0; // Encima del background actual
                _fadeTextureRect.MouseFilter = Control.MouseFilterEnum.Ignore;
                AddChild(_fadeTextureRect);
            }
            
            // Verificar si hay cuadro negro activo que necesite ser removido
            bool hasBlackOverlay = false;
            if (_solidBackground != null && IsInstanceValid(_solidBackground))
            {
                var currentColor = _solidBackground.Color;
                // Verificar si el cuadro negro está activo (negro con alpha > 0 y ZIndex alto)
                if (currentColor.A > 0.01f && currentColor.R < 0.1f && currentColor.G < 0.1f && currentColor.B < 0.1f && _solidBackground.ZIndex >= 1)
                {
                    hasBlackOverlay = true;
                    GD.Print("[SceneBackground] Detectado cuadro negro activo, se hará fade out simultáneo");
                }
            }
            
            // Configurar el nuevo background con color original (sin modulación)
            _fadeTextureRect.Texture = newTexture;
            _fadeTextureRect.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f); // Iniciar transparente
            
            // Crear tween para el fade
            _fadeTween = CreateTween();
            
            // Hacer fade in del nuevo background (color original, alpha de 0 a 1)
            _fadeTween.TweenProperty(_fadeTextureRect, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), fadeDuration);
            
            // Si hay cuadro negro, hacer fade out simultáneo
            if (hasBlackOverlay && _solidBackground != null && IsInstanceValid(_solidBackground))
            {
                // Hacer fade out del cuadro negro al mismo tiempo
                var currentBlackColor = _solidBackground.Color;
                _fadeTween.Parallel().TweenProperty(_solidBackground, "color", 
                    new Color(currentBlackColor.R, currentBlackColor.G, currentBlackColor.B, 0.0f), fadeDuration);
            }
            
            _fadeTween.SetEase(Tween.EaseType.InOut);
            _fadeTween.SetTrans(Tween.TransitionType.Sine);
            
            // Cuando termine el fade, reemplazar el background actual y limpiar
            _fadeTween.TweenCallback(Callable.From(() => {
                // Asegurarse de que el cuadro negro esté completamente transparente y con ZIndex bajo
                if (hasBlackOverlay && _solidBackground != null && IsInstanceValid(_solidBackground))
                {
                    _solidBackground.Color = new Color(_fallbackColor.R, _fallbackColor.G, _fallbackColor.B, 0.0f);
                    _solidBackground.ZIndex = -2; // Restaurar ZIndex original
                    GD.Print("[SceneBackground] Cuadro negro removido y ZIndex restaurado");
                }
                FinishBackgroundFade(newImagePath, newTexture, onFadeFinish);
            }));
        }
        
        /// <summary>
        /// Finaliza el cambio de background después del fade
        /// </summary>
        private void FinishBackgroundFade(string newImagePath, Texture2D newTexture, System.Action onFadeFinish)
        {
            // CRÍTICO: El nuevo background ya está completamente visible en _fadeTextureRect (alpha = 1.0)
            // En lugar de actualizar el background actual y eliminar el temporal,
            // convertimos el temporal en el nuevo background actual y eliminamos el viejo
            // Esto es más seguro y evita problemas de visibilidad
            
            if (_fadeTextureRect != null && IsInstanceValid(_fadeTextureRect))
            {
                // El fadeTextureRect ya tiene la nueva textura y está completamente visible
                // Lo convertimos en el nuevo background actual
                
                // Eliminar el background viejo
                if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
                {
                    _backgroundTextureRect.QueueFree();
                }
                
                // El fadeTextureRect se convierte en el nuevo background actual
                _backgroundTextureRect = _fadeTextureRect;
                _backgroundTextureRect.Name = "BackgroundTextureRect";
                _backgroundTextureRect.ZIndex = -1; // Restaurar el ZIndex correcto
                
                // Actualizar la ruta de la imagen
                _backgroundImagePath = newImagePath;
                
                // Limpiar la referencia temporal
                _fadeTextureRect = null;
                
                GD.Print($"[SceneBackground] Background reemplazado completamente: {newImagePath}, Modulate: {_backgroundTextureRect.Modulate}, Visible: {_backgroundTextureRect.Visible}, Texture: {(_backgroundTextureRect.Texture != null ? "OK" : "NULL")}");
            }
            else
            {
                GD.PrintErr("[SceneBackground] ❌ _fadeTextureRect no es válido al finalizar el fade");
                
                // Fallback: intentar actualizar el background actual
                if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
                {
                    _backgroundImagePath = newImagePath;
                    _backgroundTextureRect.Texture = newTexture;
                    _backgroundTextureRect.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    _backgroundTextureRect.Visible = true;
                }
            }
            
            // Verificar que el background actual esté visible después de la limpieza
            if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
            {
                var isVisible = _backgroundTextureRect.Visible;
                var modulate = _backgroundTextureRect.Modulate;
                var hasTexture = _backgroundTextureRect.Texture != null;
                
                GD.Print($"[SceneBackground] Verificación final - Visible: {isVisible}, Modulate: {modulate}, Texture: {(hasTexture ? "OK" : "NULL")}");
                
                // Si algo está mal, intentar corregirlo
                if (!isVisible || modulate.A < 0.9f || !hasTexture)
                {
                    GD.PrintErr($"[SceneBackground] ⚠️ Background no está correctamente configurado, corrigiendo...");
                    _backgroundTextureRect.Visible = true;
                    _backgroundTextureRect.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    if (!hasTexture && newTexture != null)
                    {
                        _backgroundTextureRect.Texture = newTexture;
                    }
                }
            }
            
            GD.Print($"[SceneBackground] Background cambiado con fade completado: {newImagePath}");
            
            // Ejecutar callback
            onFadeFinish?.Invoke();
        }
        
        /// <summary>
        /// Obtiene el tamaño actual del viewport
        /// </summary>
        /// <returns>Tamaño del viewport</returns>
        private Vector2 GetViewportSize()
        {
            var viewport = GetViewport();
            if (viewport != null)
            {
                var visibleRect = viewport.GetVisibleRect();
                if (visibleRect.Size != Vector2.Zero)
                {
                    return visibleRect.Size;
                }
            }
            
            return new Vector2(2560, 1440); // 16:9
        }

        /// <summary>
        /// Hace un fade a negro sobre el fondo actual usando el SolidBackground existente.
        /// </summary>
        /// <param name="duration">Duración del fade en segundos</param>
        /// <param name="onFinish">Acción a ejecutar al finalizar el fade</param>
        public void fadeToBlack(float duration, Action onFinish)
        {
            // Asegurar que el SolidBackground existe
            if (_solidBackground == null || !IsInstanceValid(_solidBackground))
            {
                CreateSolidBackground();
            }
            
            // Esto es necesario para asegurarse de que _solidBackground esté disponible y sea válido antes de intentar hacer el fade.
            // Si _solidBackground aún es nulo o inválido después de intentar crearlo, significa que no se puede continuar con el fade,
            // así que informamos del error y llamamos a onFinish para no dejar el flujo atascado (por ejemplo, si hay un callback que espera continuación).
            if (_solidBackground == null || !IsInstanceValid(_solidBackground))
            {
                GD.PrintErr("[SceneBackground] No se pudo crear o acceder al SolidBackground para fade a negro");
                onFinish?.Invoke(); // Llamamos onFinish para continuar el flujo del juego aunque no se haya ejecutado el fade visual.
                return;
            }
            
            // Guardar el color original del SolidBackground
            Color originalColor = _solidBackground.Color;
            
            // CRÍTICO: Cambiar ZIndex a 1 para estar encima del background
            // Establecer ZAsRelative = false para que el ZIndex sea absoluto
            _solidBackground.ZAsRelative = false;
            _solidBackground.ZIndex = 1;
            
            // También mover el nodo al final para asegurar que esté encima visualmente
            MoveChild(_solidBackground, GetChildCount() - 1); // Mover al final del árbol
            
            // Iniciar con el color actual pero transparente
            _solidBackground.Color = new Color(originalColor.R, originalColor.G, originalColor.B, 0.0f);
            
            GD.Print($"[SceneBackground] SolidBackground ZIndex cambiado a {_solidBackground.ZIndex} (ZAsRelative={_solidBackground.ZAsRelative}), Color inicial: {_solidBackground.Color}");
            
            // Crear tween para hacer fade in del SolidBackground a negro opaco
            var tween = GetTree().CreateTween();
            tween.TweenProperty(_solidBackground, "color", new Color(0.0f, 0.0f, 0.0f, 1.0f), duration);
            tween.SetEase(Tween.EaseType.InOut);
            tween.SetTrans(Tween.TransitionType.Sine);
            
            if (onFinish != null)
            {
                tween.TweenCallback(Callable.From(() => { onFinish?.Invoke(); }));
            }
            
            GD.Print($"[SceneBackground] Fade a negro iniciado (duración: {duration}s) - SolidBackground ZIndex={_solidBackground.ZIndex}");
        }
        
        /// <summary>
        /// Hace fade out del cuadro negro si está activo
        /// </summary>
        private void FadeOutBlackIfActive()
        {
            if (_solidBackground != null && IsInstanceValid(_solidBackground))
            {
                var currentColor = _solidBackground.Color;
                // Verificar si el cuadro negro está activo
                if (currentColor.A > 0.01f && currentColor.R < 0.1f && currentColor.G < 0.1f && currentColor.B < 0.1f && _solidBackground.ZIndex >= 1)
                {
                    var tween = GetTree().CreateTween();
                    tween.TweenProperty(_solidBackground, "color", 
                        new Color(_fallbackColor.R, _fallbackColor.G, _fallbackColor.B, 0.0f), 1.0f);
                    tween.TweenCallback(Callable.From(() => {
                        _solidBackground.ZIndex = -2; // Restaurar ZIndex original
                        GD.Print("[SceneBackground] Cuadro negro removido");
                    }));
                }
            }
        }
        
        /// <summary>
        /// Inicia un efecto de pulso en el background (como en películas de terror/suspenso)
        /// Crea un efecto de "heartbeat" que hace que el fondo pulse y se distorsione ligeramente
        /// </summary>
        /// <param name="intensity">Intensidad del pulso (0.0 a 1.0, por defecto 0.05 = 5%)</param>
        /// <param name="pulseSpeed">Velocidad del pulso en segundos por ciclo (por defecto 0.8s, más rápido = más tensión)</param>
        /// <param name="distortionAmount">Cantidad de distorsión (0.0 a 1.0, por defecto 0.02 = 2%)</param>
        public void StartPulseEffect(float intensity = 0.05f, float pulseSpeed = 0.8f, float distortionAmount = 0.02f)
        {
            if (_isPulsing)
            {
                StopPulseEffect();
            }
            
            if (_backgroundTextureRect == null || !IsInstanceValid(_backgroundTextureRect))
            {
                GD.PrintErr("[SceneBackground] No hay background para aplicar efecto de pulso");
                return;
            }
            
            _isPulsing = true;
            
            // CRÍTICO: Configurar el PivotOffset al centro para que el pulso se origine desde el centro
            // Obtener el tamaño del TextureRect
            var textureRectSize = _backgroundTextureRect.Size;
            if (textureRectSize == Vector2.Zero)
            {
                // Si el tamaño es cero, usar el tamaño del viewport
                var viewport = GetViewport();
                if (viewport != null)
                {
                    textureRectSize = viewport.GetVisibleRect().Size;
                }
                else
                {
                    textureRectSize = new Vector2(2560, 1440); // Tamaño por defecto
                }
            }
            
            // Configurar PivotOffset al centro del TextureRect
            _backgroundTextureRect.PivotOffset = textureRectSize / 2.0f;
            
            // Guardar la escala base
            _baseScale = _backgroundTextureRect.Scale;
            if (_baseScale == Vector2.Zero)
            {
                _baseScale = Vector2.One;
            }
            
            // Calcular escalas para el pulso
            // El pulso hace que el fondo se expanda y contraiga ligeramente desde el centro
            Vector2 expandedScale = _baseScale * (1.0f + intensity);
            Vector2 contractedScale = _baseScale * (1.0f - intensity * 0.5f); // Contracción más sutil
            
            // Calcular modulación de color para efecto de distorsión visual
            Color normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Color distortedColor = new Color(1.0f + distortionAmount, 1.0f - distortionAmount * 0.5f, 1.0f - distortionAmount * 0.5f, 1.0f);
            
            // Crear tween con loop infinito
            _pulseTween = CreateTween();
            _pulseTween.SetLoops(); // Loop infinito
            
            // Fase 1: Expansión rápida (como un latido)
            _pulseTween.TweenProperty(_backgroundTextureRect, "scale", expandedScale, pulseSpeed * 0.2f)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Quad);
            
            // Fase 2: Contracción rápida
            _pulseTween.Parallel().TweenProperty(_backgroundTextureRect, "modulate", distortedColor, pulseSpeed * 0.2f)
                .SetEase(Tween.EaseType.In)
                .SetTrans(Tween.TransitionType.Quad);
            
            _pulseTween.TweenProperty(_backgroundTextureRect, "scale", contractedScale, pulseSpeed * 0.3f)
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Sine);
            
            // Fase 3: Recuperación a normal
            _pulseTween.TweenProperty(_backgroundTextureRect, "scale", _baseScale, pulseSpeed * 0.5f)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Elastic);
            
            _pulseTween.Parallel().TweenProperty(_backgroundTextureRect, "modulate", normalColor, pulseSpeed * 0.5f)
                .SetEase(Tween.EaseType.Out)
                .SetTrans(Tween.TransitionType.Sine);
            
            GD.Print($"[SceneBackground] Efecto de pulso iniciado (intensidad: {intensity}, velocidad: {pulseSpeed}s)");
        }
        
        /// <summary>
        /// Detiene el efecto de pulso y restaura el background a su estado normal
        /// </summary>
        public void StopPulseEffect()
        {
            if (!_isPulsing)
            {
                return;
            }
            
            _isPulsing = false;
            
            // Detener el tween si existe
            if (_pulseTween != null && _pulseTween.IsValid())
            {
                _pulseTween.Kill();
                _pulseTween = null;
            }
            
            // Restaurar escala y color normales con animación suave
            if (_backgroundTextureRect != null && IsInstanceValid(_backgroundTextureRect))
            {
                var restoreTween = CreateTween();
                restoreTween.TweenProperty(_backgroundTextureRect, "scale", _baseScale, 0.3f)
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Sine);
                restoreTween.Parallel().TweenProperty(_backgroundTextureRect, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.3f)
                    .SetEase(Tween.EaseType.Out)
                    .SetTrans(Tween.TransitionType.Sine);
                
                // Restaurar PivotOffset al finalizar (volver a esquina superior izquierda)
                restoreTween.TweenCallback(Callable.From(() => {
                    _backgroundTextureRect.PivotOffset = Vector2.Zero;
                }));
            }
            
            GD.Print("[SceneBackground] Efecto de pulso detenido");
        }
        
        /// <summary>
        /// Verifica si el efecto de pulso está activo
        /// </summary>
        /// <returns>True si el pulso está activo, false en caso contrario</returns>
        public bool IsPulsing()
        {
            return _isPulsing;
        }
    }
}

