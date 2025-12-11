using Godot;

namespace SlimeKingdomChronicles.Core.UI
{
    /// <summary>
    /// Manager centralizado para todos los estilos de fuente del juego
    /// Permite escalar y desescalar fácilmente todos los textos
    /// </summary>
    public static class FontManager
    {
        // Escala base para todos los textos (1.0 = tamaño normal)
        private static float _globalScale = 1.0f;
        
        // Tamaños base de fuente (sin escala)
        private const float TITLE_SIZE = 56.0f;
        private const float SUBTITLE_SIZE = 36.0f;
        private const float BODY_SIZE = 28.0f;
        private const float SMALL_SIZE = 24.0f;
        private const float TINY_SIZE = 18.0f;
        
        // Colores temáticos del juego
        private static readonly Color GOLD_COLOR = new Color(1.0f, 0.8f, 0.0f, 1.0f);
        private static readonly Color BLUE_COLOR = new Color(0.2f, 0.8f, 1.0f, 1.0f);
        private static readonly Color PINK_COLOR = new Color(1.0f, 0.6f, 0.8f, 1.0f);
        private static readonly Color WHITE_COLOR = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        private static readonly Color GRAY_COLOR = new Color(0.7f, 0.7f, 0.7f, 1.0f);
        
        // Colores de sombra
        private static readonly Color GOLD_SHADOW = new Color(0.5f, 0.3f, 0.0f, 0.8f);
        private static readonly Color BLUE_SHADOW = new Color(0.0f, 0.4f, 0.6f, 0.8f);
        private static readonly Color PINK_SHADOW = new Color(0.8f, 0.3f, 0.5f, 0.8f);
        private static readonly Color WHITE_SHADOW = new Color(0.3f, 0.3f, 0.3f, 0.8f);
        
        // Colores de outline
        private static readonly Color GOLD_OUTLINE = new Color(1.0f, 1.0f, 0.0f, 0.8f);
        private static readonly Color BLUE_OUTLINE = new Color(0.0f, 0.6f, 1.0f, 0.6f);
        private static readonly Color PINK_OUTLINE = new Color(1.0f, 0.8f, 0.9f, 0.6f);
        private static readonly Color WHITE_OUTLINE = new Color(0.8f, 0.8f, 0.8f, 0.6f);
        
        /// <summary>
        /// Establece la escala global para todos los textos
        /// </summary>
        /// <param name="scale">Escala (1.0 = normal, 0.5 = mitad, 2.0 = doble)</param>
        public static void SetGlobalScale(float scale)
        {
            _globalScale = Mathf.Clamp(scale, 0.1f, 5.0f);
        }
        
        /// <summary>
        /// Obtiene la escala global actual
        /// </summary>
        /// <returns>Escala global actual</returns>
        public static float GetGlobalScale()
        {
            return _globalScale;
        }
        
        /// <summary>
        /// Aplica el estilo de título principal (celebración de pesca)
        /// </summary>
        /// <param name="label">Label al que aplicar el estilo</param>
        public static void ApplyTitleStyle(Label label)
        {
            if (label == null) return;
            
            // Tamaño escalado
            float scaledSize = TITLE_SIZE * _globalScale;
            label.AddThemeFontSizeOverride("font_size", (int)scaledSize);
            
            // Color principal
            label.AddThemeColorOverride("font_color", GOLD_COLOR);
            
            // Sombra
            label.AddThemeColorOverride("font_shadow_color", GOLD_SHADOW);
            label.AddThemeConstantOverride("shadow_offset_x", (int)(3 * _globalScale));
            label.AddThemeConstantOverride("shadow_offset_y", (int)(3 * _globalScale));
            
            // Outline
            label.AddThemeColorOverride("font_outline_color", GOLD_OUTLINE);
            label.AddThemeConstantOverride("outline_size", (int)(2 * _globalScale));
        }
        
        /// <summary>
        /// Aplica el estilo de subtítulo (motivación)
        /// </summary>
        /// <param name="label">Label al que aplicar el estilo</param>
        public static void ApplySubtitleStyle(Label label)
        {
            if (label == null) return;
            
            // Tamaño escalado
            float scaledSize = SUBTITLE_SIZE * _globalScale;
            label.AddThemeFontSizeOverride("font_size", (int)scaledSize);
            
            // Color principal
            label.AddThemeColorOverride("font_color", BLUE_COLOR);
            
            // Sombra
            label.AddThemeColorOverride("font_shadow_color", BLUE_SHADOW);
            label.AddThemeConstantOverride("shadow_offset_x", (int)(2 * _globalScale));
            label.AddThemeConstantOverride("shadow_offset_y", (int)(2 * _globalScale));
            
            // Outline
            label.AddThemeColorOverride("font_outline_color", BLUE_OUTLINE);
            label.AddThemeConstantOverride("outline_size", (int)(1 * _globalScale));
        }
        
        /// <summary>
        /// Aplica el estilo de texto de aliento
        /// </summary>
        /// <param name="label">Label al que aplicar el estilo</param>
        public static void ApplyEncouragementStyle(Label label)
        {
            if (label == null) return;
            
            // Tamaño escalado
            float scaledSize = BODY_SIZE * _globalScale;
            label.AddThemeFontSizeOverride("font_size", (int)scaledSize);
            
            // Color principal
            label.AddThemeColorOverride("font_color", PINK_COLOR);
            
            // Sombra
            label.AddThemeColorOverride("font_shadow_color", PINK_SHADOW);
            label.AddThemeConstantOverride("shadow_offset_x", (int)(2 * _globalScale));
            label.AddThemeConstantOverride("shadow_offset_y", (int)(2 * _globalScale));
            
            // Outline
            label.AddThemeColorOverride("font_outline_color", PINK_OUTLINE);
            label.AddThemeConstantOverride("outline_size", (int)(1 * _globalScale));
        }
        
        /// <summary>
        /// Aplica el estilo de texto normal
        /// </summary>
        /// <param name="label">Label al que aplicar el estilo</param>
        public static void ApplyBodyStyle(Label label)
        {
            if (label == null) return;
            
            // Tamaño escalado
            float scaledSize = BODY_SIZE * _globalScale;
            label.AddThemeFontSizeOverride("font_size", (int)scaledSize);
            
            // Color principal
            label.AddThemeColorOverride("font_color", WHITE_COLOR);
            
            // Sombra
            label.AddThemeColorOverride("font_shadow_color", WHITE_SHADOW);
            label.AddThemeConstantOverride("shadow_offset_x", (int)(1 * _globalScale));
            label.AddThemeConstantOverride("shadow_offset_y", (int)(1 * _globalScale));
            
            // Outline
            label.AddThemeColorOverride("font_outline_color", WHITE_OUTLINE);
            label.AddThemeConstantOverride("outline_size", (int)(1 * _globalScale));
        }
        
        /// <summary>
        /// Aplica el estilo de texto pequeño
        /// </summary>
        /// <param name="label">Label al que aplicar el estilo</param>
        public static void ApplySmallStyle(Label label)
        {
            if (label == null) return;
            
            // Tamaño escalado
            float scaledSize = SMALL_SIZE * _globalScale;
            label.AddThemeFontSizeOverride("font_size", (int)scaledSize);
            
            // Color principal
            label.AddThemeColorOverride("font_color", GRAY_COLOR);
            
            // Sombra
            label.AddThemeColorOverride("font_shadow_color", WHITE_SHADOW);
            label.AddThemeConstantOverride("shadow_offset_x", (int)(1 * _globalScale));
            label.AddThemeConstantOverride("shadow_offset_y", (int)(1 * _globalScale));
        }
        
        /// <summary>
        /// Aplica el estilo de texto muy pequeño
        /// </summary>
        /// <param name="label">Label al que aplicar el estilo</param>
        public static void ApplyTinyStyle(Label label)
        {
            if (label == null) return;
            
            // Tamaño escalado
            float scaledSize = TINY_SIZE * _globalScale;
            label.AddThemeFontSizeOverride("font_size", (int)scaledSize);
            
            // Color principal
            label.AddThemeColorOverride("font_color", GRAY_COLOR);
        }
        
        /// <summary>
        /// Obtiene el tamaño escalado para un tipo de texto específico
        /// </summary>
        /// <param name="textType">Tipo de texto</param>
        /// <returns>Tamaño escalado</returns>
        public static float GetScaledSize(TextType textType)
        {
            return textType switch
            {
                TextType.Title => TITLE_SIZE * _globalScale,
                TextType.Subtitle => SUBTITLE_SIZE * _globalScale,
                TextType.Body => BODY_SIZE * _globalScale,
                TextType.Small => SMALL_SIZE * _globalScale,
                TextType.Tiny => TINY_SIZE * _globalScale,
                _ => BODY_SIZE * _globalScale
            };
        }
        
        /// <summary>
        /// Obtiene el tamaño base para un tipo de texto específico
        /// </summary>
        /// <param name="textType">Tipo de texto</param>
        /// <returns>Tamaño base</returns>
        public static float GetBaseSize(TextType textType)
        {
            return textType switch
            {
                TextType.Title => TITLE_SIZE,
                TextType.Subtitle => SUBTITLE_SIZE,
                TextType.Body => BODY_SIZE,
                TextType.Small => SMALL_SIZE,
                TextType.Tiny => TINY_SIZE,
                _ => BODY_SIZE
            };
        }
    }
    
    /// <summary>
    /// Tipos de texto disponibles en el juego
    /// </summary>
    public enum TextType
    {
        Title,      // Títulos principales (celebración)
        Subtitle,   // Subtítulos (motivación)
        Body,       // Texto normal
        Small,      // Texto pequeño
        Tiny        // Texto muy pequeño
    }
}
