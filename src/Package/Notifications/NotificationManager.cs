using Godot;
using System.Collections.Generic;
using System.Linq;

namespace Aprendizdemago.Package.Notifications
{
    /// <summary>
    /// Manager centralizado para mostrar notificaciones en la interfaz
    /// Permite mostrar mensajes personalizables en diferentes partes de la pantalla
    /// Usando principios SOLID, KISS, SRP, DRY
    /// </summary>
    public partial class NotificationManager : CanvasLayer
    {
        private static NotificationManager _instance;
        public static NotificationManager Instance => _instance;
        
        private Control _container;
        private List<NotificationDisplay> _activeNotifications;
        
        public override void _Ready()
        {
            base._Ready();
            
            if (_instance != null && _instance != this)
            {
                QueueFree();
                return;
            }
            
            _instance = this;
            _activeNotifications = new List<NotificationDisplay>();
            
            // Crear contenedor para las notificaciones
            _container = new Control();
            _container.Name = "NotificationContainer";
            _container.MouseFilter = Control.MouseFilterEnum.Ignore;
            _container.Size = GetViewport().GetVisibleRect().Size;
            AddChild(_container);
        }
        
        public override void _ExitTree()
        {
            base._ExitTree();
            if (_instance == this)
            {
                _instance = null;
            }
        }
        
        /// <summary>
        /// Muestra una notificación con la configuración proporcionada
        /// </summary>
        public void ShowNotification(NotificationMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.Text))
                return;
            
            var display = new NotificationDisplay();
            display.Initialize(message);
            display.Name = $"Notification_{System.Guid.NewGuid()}";
            display.MouseFilter = Control.MouseFilterEnum.Ignore;
            
            _container.AddChild(display);
            _activeNotifications.Add(display);
            
            // Limpiar cuando se elimine
            display.TreeExiting += () => _activeNotifications.Remove(display);
        }
        
        /// <summary>
        /// Muestra una notificación rápida con valores por defecto
        /// </summary>
        public void ShowQuickNotification(string text, ENotificationStyle style = ENotificationStyle.Normal)
        {
            var viewport = GetViewport().GetVisibleRect();
            var message = new NotificationMessage(text)
            {
                Position = new Vector2(viewport.Size.X * 0.5f, 100),
                Style = style,
                Duration = 2.0f,
                Animation = ENotificationAnimation.FadeInOut
            };
            
            ShowNotification(message);
        }
        
        /// <summary>
        /// Muestra una notificación en la parte superior de la pantalla
        /// </summary>
        public void ShowTopNotification(string text, ENotificationStyle style = ENotificationStyle.Info, float duration = 3.0f)
        {
            var viewport = GetViewport().GetVisibleRect();
            var message = new NotificationMessage(text)
            {
                Position = new Vector2(viewport.Size.X * 0.5f, 50),
                Style = style,
                Duration = duration,
                Animation = ENotificationAnimation.SlideFromTop,
                FontSize = 28
            };
            
            ShowNotification(message);
        }
        
        /// <summary>
        /// Muestra una notificación de daño en una posición específica (por ejemplo, sobre un enemigo)
        /// Con efecto de arco, sin fondo, texto amarillo y más grande
        /// </summary>
        public void ShowDamageNotification(string text, Vector2 worldPosition, bool isCritical = false)
        {
            // Convertir posición del mundo a posición de la pantalla
            var screenPos = worldPosition;
            
            var message = new NotificationMessage(text)
            {
                Position = screenPos,
                BackgroundColor = Colors.Transparent, // Sin fondo
                Duration = 1.2f,
                Animation = ENotificationAnimation.DamageArc, // Animación especial de arco
                FontSize = isCritical ? 48 : 40, // Más grande (aumentado de 36/28)
                TextColor = isCritical ? Colors.Orange : Colors.Yellow, // Amarillo en lugar de rojo
                Style = ENotificationStyle.Normal // Para evitar colores de fondo
            };
            
            ShowNotification(message);
        }
        
        /// <summary>
        /// Muestra una notificación personalizada completamente
        /// </summary>
        public void ShowCustomNotification(string text, Vector2 position, int fontSize, 
            ENotificationStyle style, ENotificationAnimation animation, float duration = 3.0f)
        {
            var message = new NotificationMessage(text)
            {
                Position = position,
                FontSize = fontSize,
                Style = style,
                Animation = animation,
                Duration = duration
            };
            
            ShowNotification(message);
        }
        
        /// <summary>
        /// Muestra una notificación de cambios de stats (buffs/debuffs) en una posición específica
        /// Muestra múltiples cambios de stats con colores diferentes (verde para buffs, rojo para debuffs)
        /// </summary>
        public void ShowStatChangeNotification(Dictionary<string, int> statChanges, Vector2 worldPosition, float duration = 2.0f)
        {
            if (statChanges == null || statChanges.Count == 0)
                return;
            
            // Crear texto con todos los cambios
            var changeTexts = new List<string>();
            foreach (var change in statChanges)
            {
                string statName = change.Key;
                int changeValue = change.Value;
                
                if (changeValue == 0)
                    continue;
                
                string sign = changeValue > 0 ? "+" : "";
                string text = $"{sign}{changeValue} {statName}";
                changeTexts.Add(text);
            }
            
            if (changeTexts.Count == 0)
                return;
            
            // Combinar todos los cambios en un solo texto con saltos de línea
            string combinedText = string.Join("\n", changeTexts);
            
            // Determinar color basado en si hay más buffs o debuffs
            bool hasBuffs = statChanges.Values.Any(v => v > 0);
            bool hasDebuffs = statChanges.Values.Any(v => v < 0);
            
            Color textColor = Colors.White;
            if (hasBuffs && !hasDebuffs)
                textColor = Colors.LightGreen;
            else if (hasDebuffs && !hasBuffs)
                textColor = Colors.LightCoral;
            else
                textColor = Colors.Yellow; // Mixto
            
            var message = new NotificationMessage(combinedText)
            {
                Position = worldPosition,
                BackgroundColor = Colors.Transparent, // Sin fondo
                Duration = duration,
                Animation = ENotificationAnimation.DamageArc, // Animación de arco (sube y se desvanece)
                FontSize = 40, // Más grande (aumentado de 28)
                TextColor = textColor,
                Style = ENotificationStyle.Normal
            };
            
            ShowNotification(message);
        }
        
        /// <summary>
        /// Limpia todas las notificaciones activas
        /// </summary>
        public void ClearAllNotifications()
        {
            foreach (var notification in _activeNotifications.ToArray())
            {
                notification.QueueFree();
            }
            _activeNotifications.Clear();
        }
    }
}

