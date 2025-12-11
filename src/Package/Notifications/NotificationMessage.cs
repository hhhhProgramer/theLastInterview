using Godot;

namespace Aprendizdemago.Package.Notifications
{
    /// <summary>
    /// Configuración de un mensaje de notificación
    /// Permite personalizar todo el aspecto y comportamiento del mensaje
    /// </summary>
    public class NotificationMessage
    {
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public int FontSize { get; set; } = 24;
        public Color TextColor { get; set; } = Colors.White;
        public Color BackgroundColor { get; set; } = new Color(0.2f, 0.2f, 0.2f, 0.9f);
        public float Duration { get; set; } = 3.0f; // Segundos
        public ENotificationStyle Style { get; set; } = ENotificationStyle.Normal;
        public ENotificationAnimation Animation { get; set; } = ENotificationAnimation.FadeInOut;
        public System.Action OnComplete { get; set; }
        
        public NotificationMessage(string text)
        {
            Text = text;
        }
    }
    
    /// <summary>
    /// Estilos predefinidos para las notificaciones
    /// </summary>
    public enum ENotificationStyle
    {
        Normal,     // Estilo estándar
        Warning,    // Amarillo/naranja
        Error,      // Rojo
        Success,    // Verde
        Info        // Azul/cyan
    }
    
    /// <summary>
    /// Tipos de animación para las notificaciones
    /// </summary>
    public enum ENotificationAnimation
    {
        FadeInOut,      // Aparecer y desaparecer suavemente
        SlideFromTop,   // Deslizar desde arriba
        SlideFromBottom,// Deslizar desde abajo
        Pop,            // Efecto de pop/zoom
        Bounce,         // Rebote
        DamageArc       // Movimiento en arco para daño (sube y se desvanece)
    }
}

