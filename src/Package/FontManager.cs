using Godot;

namespace Core.Services
{
    /// <summary>
    /// Manager de fuentes (stub para compatibilidad con DialogBox y NavigationCardCreator)
    /// </summary>
    public static class FontManager
    {
        public enum TextType
        {
            Tiny = 0,
            Small = 1,
            Medium = 2,
            Large = 3,
            Subtitle = 4,
            Body = 5
        }
        
        /// <summary>
        /// Obtiene el tamaño escalado de fuente según el tipo
        /// Aumenta el tamaño en pantallas pequeñas (teléfonos) para mejor legibilidad
        /// </summary>
        public static float GetScaledSize(TextType textType)
        {
            // Detectar si es teléfono (pantalla pequeña)
            // Usar DisplayServer para obtener el tamaño de la pantalla
            var screenSize = DisplayServer.ScreenGetSize();
            bool isPhone = screenSize.X > 1920 || screenSize.Y > 1080;

            // Multiplicador para teléfonos (aumentar tamaño en 40%)
            float phoneMultiplier = isPhone ? 1.4f : 1.0f;
            
            return textType switch
            {
                TextType.Tiny => 20.0f * phoneMultiplier,
                TextType.Small => 24.0f * phoneMultiplier,
                TextType.Medium => 28.0f * phoneMultiplier,
                TextType.Large => 34.0f * phoneMultiplier,
                TextType.Subtitle => 30.0f * phoneMultiplier,
                TextType.Body => 26.0f * phoneMultiplier,
                _ => 28.0f * phoneMultiplier
            };
        }
        
        /// <summary>
        /// Aplica estilo pequeño a un label (stub para compatibilidad)
        /// </summary>
        public static void ApplyTinyStyle(Label label)
        {
            if (label != null)
            {
                label.AddThemeFontSizeOverride("font_size", 12);
            }
        }
    }
}

