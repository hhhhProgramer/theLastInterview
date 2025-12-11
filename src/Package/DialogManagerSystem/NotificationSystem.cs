using Godot;

namespace SlimeKingdomChronicles.Core.UI
{
    /// <summary>
    /// Sistema simple de notificaciones para mostrar mensajes temporales al jugador
    /// </summary>
    public partial class NotificationSystem : Node
    {
        private static NotificationSystem _instance;
        public static NotificationSystem Instance => _instance;
        
        private Label _notificationLabel;
        private Tween _notificationTween;
        
        public override void _Ready()
        {
            if (_instance == null)
            {
                _instance = this;
                SetupNotificationSystem();
                GD.Print("NotificationSystem inicializado");
            }
            else
            {
                QueueFree();
            }
        }
        
        /// <summary>
        /// Configura el sistema de notificaciones
        /// </summary>
        private void SetupNotificationSystem()
        {
            // Crear el label de notificación
            _notificationLabel = new Label();
            _notificationLabel.Name = "NotificationLabel";
            _notificationLabel.Text = "";
            _notificationLabel.Visible = false;
            _notificationLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _notificationLabel.VerticalAlignment = VerticalAlignment.Center;
            
            // Configurar el estilo del label
            var styleBox = new StyleBoxFlat();
            styleBox.BgColor = new Color(0, 0, 0, 0.8f);
            styleBox.CornerRadiusTopLeft = 10;
            styleBox.CornerRadiusTopRight = 10;
            styleBox.CornerRadiusBottomLeft = 10;
            styleBox.CornerRadiusBottomRight = 10;
            styleBox.ContentMarginLeft = 20;
            styleBox.ContentMarginRight = 20;
            styleBox.ContentMarginTop = 10;
            styleBox.ContentMarginBottom = 10;
            
            _notificationLabel.AddThemeStyleboxOverride("normal", styleBox);
            _notificationLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1, 1));
            _notificationLabel.AddThemeFontSizeOverride("font_size", 18);
            
            // Agregar el label a la escena
            AddChild(_notificationLabel);
            
            // Configurar anclas para centrar en pantalla
            _notificationLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _notificationLabel.OffsetTop = -50;
            _notificationLabel.OffsetBottom = 50;
            _notificationLabel.OffsetLeft = -200;
            _notificationLabel.OffsetRight = 200;
            
            // Asegurar que esté en la capa superior
            _notificationLabel.ZIndex = 1000;
            
            GD.Print("NotificationSystem configurado correctamente");
        }
        
        /// <summary>
        /// Muestra una notificación temporal al jugador
        /// </summary>
        /// <param name="message">Mensaje a mostrar</param>
        /// <param name="duration">Duración en segundos (por defecto 3 segundos)</param>
        /// <param name="color">Color del texto (por defecto blanco)</param>
        public void ShowNotification(string message, float duration = 3.0f, Color? color = null)
        {
            if (_notificationLabel == null) 
            {
                GD.PrintErr("NotificationLabel es null!");
                return;
            }
            
            GD.Print($"Mostrando notificación: {message}");
            
            // Detener animación anterior si existe
            _notificationTween?.Kill();
            
            // Configurar el mensaje
            _notificationLabel.Text = message;
            _notificationLabel.Visible = true;
            _notificationLabel.Modulate = new Color(1, 1, 1, 1);
            _notificationLabel.Scale = new Vector2(1.0f, 1.0f);
            
            if (color.HasValue)
            {
                _notificationLabel.AddThemeColorOverride("font_color", color.Value);
            }
            else
            {
                _notificationLabel.AddThemeColorOverride("font_color", new Color(1, 1, 1, 1));
            }
            
            // Crear animación simple
            _notificationTween = CreateTween();
            
            // Esperar y luego desaparecer
            _notificationTween.TweenInterval(duration);
            _notificationTween.TweenProperty(_notificationLabel, "modulate", new Color(1, 1, 1, 0), 0.5f);
            _notificationTween.TweenCallback(Callable.From(() => {
                _notificationLabel.Visible = false;
                GD.Print("Notificación ocultada");
            }));
        }
        
        /// <summary>
        /// Muestra una notificación de error (texto rojo)
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="duration">Duración en segundos</param>
        public void ShowError(string message, float duration = 3.0f)
        {
            ShowNotification(message, duration, new Color(1, 0.3f, 0.3f, 1));
        }
        
        /// <summary>
        /// Muestra una notificación de éxito (texto verde)
        /// </summary>
        /// <param name="message">Mensaje de éxito</param>
        /// <param name="duration">Duración en segundos</param>
        public void ShowSuccess(string message, float duration = 3.0f)
        {
            ShowNotification(message, duration, new Color(0.3f, 1, 0.3f, 1));
        }
        
        /// <summary>
        /// Muestra una notificación de información (texto azul)
        /// </summary>
        /// <param name="message">Mensaje informativo</param>
        /// <param name="duration">Duración en segundos</param>
        public void ShowInfo(string message, float duration = 3.0f)
        {
            ShowNotification(message, duration, new Color(0.3f, 0.7f, 1, 1));
        }
    }
}
