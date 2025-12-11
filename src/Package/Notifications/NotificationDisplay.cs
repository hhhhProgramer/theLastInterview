using Godot;

namespace Aprendizdemago.Package.Notifications
{
    /// <summary>
    /// Componente visual individual de una notificación
    /// Se crea dinámicamente y se muestra en la pantalla
    /// </summary>
    public partial class NotificationDisplay : Control
    {
        private Label _label;
        private ColorRect _background;
        private NotificationMessage _message;
        private Tween _tween;
        
        public override void _Ready()
        {
            base._Ready();
            CreateVisual();
            PlayAnimation();
        }
        
        public void Initialize(NotificationMessage message)
        {
            _message = message;
        }
        
        private void CreateVisual()
        {
            if (_message == null)
                return;
            
            // Fondo solo si no es transparente
            if (_message.BackgroundColor.A > 0.01f)
            {
                _background = new ColorRect();
                _background.Color = GetStyleColor();
                _background.Size = Size;
                _background.MouseFilter = Control.MouseFilterEnum.Ignore;
                AddChild(_background);
            }
            
            // Label con el texto
            _label = new Label();
            _label.Text = _message.Text;
            _label.AddThemeFontSizeOverride("font_size", _message.FontSize);
            _label.AddThemeColorOverride("font_color", _message.TextColor);
            _label.HorizontalAlignment = HorizontalAlignment.Center;
            _label.VerticalAlignment = VerticalAlignment.Center;
            _label.Size = Size;
            _label.Position = Vector2.Zero;
            _label.MouseFilter = Control.MouseFilterEnum.Ignore;
            
            // Agregar sombras al texto (outline)
            _label.AddThemeConstantOverride("outline_size", 4);
            _label.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
            
            AddChild(_label);
            
            // Ajustar tamaño del fondo según el texto
            // Usar un tamaño mínimo razonable
            var font = _label.GetThemeDefaultFont();
            Vector2 textSize;
            if (font != null)
            {
                textSize = font.GetStringSize(_message.Text, HorizontalAlignment.Left, -1, _message.FontSize);
            }
            else
            {
                // Fallback: estimar tamaño basado en caracteres
                textSize = new Vector2(_message.Text.Length * _message.FontSize * 0.6f, _message.FontSize * 1.5f);
            }
            
            Size = new Vector2(textSize.X + 20, textSize.Y + 10);
            if (_background != null)
            {
                _background.Size = Size;
            }
            
            // Posición inicial según la animación
            SetInitialPosition();
        }
        
        private Color GetStyleColor()
        {
            return _message.Style switch
            {
                ENotificationStyle.Warning => new Color(0.8f, 0.6f, 0.0f, 0.9f),
                ENotificationStyle.Error => new Color(0.8f, 0.2f, 0.2f, 0.9f),
                ENotificationStyle.Success => new Color(0.2f, 0.8f, 0.2f, 0.9f),
                ENotificationStyle.Info => new Color(0.2f, 0.6f, 0.8f, 0.9f),
                _ => _message.BackgroundColor
            };
        }
        
        private void SetInitialPosition()
        {
            Position = _message.Position;
            
            // Ajustar posición según la animación
            switch (_message.Animation)
            {
                case ENotificationAnimation.SlideFromTop:
                    Position = new Vector2(_message.Position.X, -Size.Y - 10);
                    break;
                case ENotificationAnimation.SlideFromBottom:
                    var screenHeight = GetViewport().GetVisibleRect().Size.Y;
                    Position = new Vector2(_message.Position.X, screenHeight + 10);
                    break;
                case ENotificationAnimation.Pop:
                case ENotificationAnimation.Bounce:
                    Modulate = new Color(1, 1, 1, 0);
                    Scale = Vector2.Zero;
                    break;
                case ENotificationAnimation.DamageArc:
                    // Para daño, empieza visible pero transparente, luego aparece rápido
                    Modulate = new Color(1, 1, 1, 0);
                    Scale = new Vector2(0.5f, 0.5f); // Empieza más pequeño
                    break;
                default: // FadeInOut
                    Modulate = new Color(1, 1, 1, 0);
                    break;
            }
        }
        
        private void PlayAnimation()
        {
            if (_message == null)
                return;
            
            _tween = CreateTween();
            _tween.SetParallel(true);
            
            // Animación de entrada
            switch (_message.Animation)
            {
                case ENotificationAnimation.FadeInOut:
                    _tween.TweenProperty(this, "modulate:a", 1.0f, 0.3f);
                    break;
                    
                case ENotificationAnimation.SlideFromTop:
                    _tween.TweenProperty(this, "position:y", _message.Position.Y, 0.3f);
                    _tween.TweenProperty(this, "modulate:a", 1.0f, 0.3f);
                    break;
                    
                case ENotificationAnimation.SlideFromBottom:
                    _tween.TweenProperty(this, "position:y", _message.Position.Y, 0.3f);
                    _tween.TweenProperty(this, "modulate:a", 1.0f, 0.3f);
                    break;
                    
                case ENotificationAnimation.Pop:
                    _tween.TweenProperty(this, "modulate:a", 1.0f, 0.2f);
                    _tween.TweenProperty(this, "scale", Vector2.One, 0.2f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
                    break;
                    
                case ENotificationAnimation.Bounce:
                    _tween.TweenProperty(this, "modulate:a", 1.0f, 0.3f);
                    _tween.TweenProperty(this, "scale", Vector2.One, 0.3f).SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Bounce);
                    break;
                    
                case ENotificationAnimation.DamageArc:
                    // Para daño: aparecer rápidamente, luego arco hacia arriba
                    _tween.TweenProperty(this, "modulate:a", 1.0f, 0.1f);
                    _tween.TweenProperty(this, "scale", Vector2.One, 0.1f);
                    break;
            }
            
            // Mantener visible durante la duración
            // El delay se maneja antes de iniciar la animación de salida
            _tween.SetParallel(false);
            
            // Para DamageArc, el movimiento en arco va durante toda la duración
            if (_message.Animation == ENotificationAnimation.DamageArc)
            {
                // Movimiento en arco: sube y se mueve ligeramente hacia la derecha
                var arcEndY = _message.Position.Y - 40.0f; // Sube 40 píxeles
                var arcEndX = _message.Position.X + 15.0f; // Se mueve ligeramente a la derecha
                var arcEnd = new Vector2(arcEndX, arcEndY);
                
                // Animación de arco usando una curva suave (cubic ease out)
                var arcDuration = _message.Duration * 0.8f; // 80% del tiempo para el arco
                
                // Movimiento en arco (simulado con ease out cubic que da sensación de arco)
                _tween.TweenProperty(this, "position", arcEnd, arcDuration)
                    .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
                
                // Último 20% para desvanecerse
                _tween.TweenInterval(_message.Duration * 0.2f);
            }
            else
            {
                _tween.TweenInterval(_message.Duration);
            }
            
            // Animación de salida
            switch (_message.Animation)
            {
                case ENotificationAnimation.FadeInOut:
                case ENotificationAnimation.SlideFromTop:
                case ENotificationAnimation.SlideFromBottom:
                    _tween.TweenProperty(this, "modulate:a", 0.0f, 0.3f);
                    break;
                    
                case ENotificationAnimation.Pop:
                case ENotificationAnimation.Bounce:
                    _tween.TweenProperty(this, "modulate:a", 0.0f, 0.2f);
                    _tween.Parallel().TweenProperty(this, "scale", Vector2.Zero, 0.2f).SetEase(Tween.EaseType.In);
                    break;
                    
                case ENotificationAnimation.DamageArc:
                    // Desvanecerse suavemente mientras sigue en el arco
                    _tween.TweenProperty(this, "modulate:a", 0.0f, 0.3f);
                    break;
            }
            
            // Eliminar y completar callback
            _tween.TweenCallback(Callable.From(() => {
                _message?.OnComplete?.Invoke();
                QueueFree();
            }));
        }
    }
}

