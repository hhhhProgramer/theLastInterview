using Godot;

namespace SlimeKingdomChronicles.src.Core.History
{
    /// <summary>
    /// Representa una escena individual dentro de un capítulo de historia
    /// </summary>
    public class ChapterScene
    {
        /// <summary>
        /// Título de la escena
        /// </summary>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// Texto de la historia que se mostrará progresivamente
        /// </summary>
        public string StoryText { get; set; } = string.Empty;
        
        /// <summary>
        /// Ruta de la imagen que se mostrará en esta escena
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;
        
        /// <summary>
        /// Velocidad de escritura del texto en caracteres por segundo
        /// </summary>
        public float TextSpeed { get; set; } = 30.0f;
        
        /// <summary>
        /// Tiempo de pausa antes de pasar a la siguiente escena (en segundos)
        /// </summary>
        public float PauseAfterText { get; set; } = 2.0f;
    }
}
