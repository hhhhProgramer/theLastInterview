using Godot;

namespace SlimeKingdomChronicles.Core.UI
{
    /// <summary>
    /// Ejemplo de uso del FontManager para diferentes tipos de texto
    /// Este archivo es solo para referencia y puede ser eliminado
    /// </summary>
    public static class FontManagerExample
    {
        /// <summary>
        /// Ejemplo de cómo usar FontManager en diferentes escenas
        /// </summary>
        public static void ExampleUsage()
        {
            // Crear un label
            var label = new Label();
            
            // Aplicar diferentes estilos según el tipo de texto
            FontManager.ApplyTitleStyle(label);        // Para títulos principales
            FontManager.ApplySubtitleStyle(label);     // Para subtítulos
            FontManager.ApplyBodyStyle(label);         // Para texto normal
            FontManager.ApplySmallStyle(label);        // Para texto pequeño
            FontManager.ApplyTinyStyle(label);         // Para texto muy pequeño
            FontManager.ApplyEncouragementStyle(label); // Para mensajes de aliento
            
            // Cambiar la escala global (afecta a todos los textos)
            FontManager.SetGlobalScale(1.2f);  // 20% más grande
            FontManager.SetGlobalScale(0.8f);  // 20% más pequeño
            FontManager.SetGlobalScale(1.0f);  // Tamaño normal
            
            // Obtener tamaños escalados
            float titleSize = FontManager.GetScaledSize(TextType.Title);
            float subtitleSize = FontManager.GetScaledSize(TextType.Subtitle);
            float bodySize = FontManager.GetScaledSize(TextType.Body);
            
            // Obtener tamaños base (sin escala)
            float baseTitleSize = FontManager.GetBaseSize(TextType.Title);
            float baseSubtitleSize = FontManager.GetBaseSize(TextType.Subtitle);
        }
        
        /// <summary>
        /// Ejemplo de cómo crear una etiqueta con estilo específico
        /// </summary>
        /// <param name="text">Texto a mostrar</param>
        /// <param name="textType">Tipo de texto</param>
        /// <returns>Label configurado</returns>
        public static Label CreateStyledLabel(string text, TextType textType)
        {
            var label = new Label();
            label.Text = text;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            
            // Aplicar estilo según el tipo
            switch (textType)
            {
                case TextType.Title:
                    FontManager.ApplyTitleStyle(label);
                    break;
                case TextType.Subtitle:
                    FontManager.ApplySubtitleStyle(label);
                    break;
                case TextType.Body:
                    FontManager.ApplyBodyStyle(label);
                    break;
                case TextType.Small:
                    FontManager.ApplySmallStyle(label);
                    break;
                case TextType.Tiny:
                    FontManager.ApplyTinyStyle(label);
                    break;
            }
            
            // Calcular tamaño basado en el texto escalado
            float scaledSize = FontManager.GetScaledSize(textType);
            label.Size = new Vector2(scaledSize * 8, scaledSize * 2);
            
            return label;
        }
        
        /// <summary>
        /// Ejemplo de cómo configurar diferentes escalas para diferentes resoluciones
        /// </summary>
        public static void ConfigureForResolution()
        {
            // Obtener resolución de pantalla
            var screenSize = DisplayServer.ScreenGetSize();
            
            // Configurar escala basada en resolución
            if (screenSize.X < 1280)
            {
                FontManager.SetGlobalScale(0.8f);  // Pantallas pequeñas
            }
            else if (screenSize.X > 1920)
            {
                FontManager.SetGlobalScale(1.2f);  // Pantallas grandes
            }
            else
            {
                FontManager.SetGlobalScale(1.0f);  // Resolución normal
            }
        }
    }
}
