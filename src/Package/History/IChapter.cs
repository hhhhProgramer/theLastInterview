using Godot;

namespace SlimeKingdomChronicles.src.Core.History
{
    /// <summary>
    /// Interfaz que define la estructura de un capítulo de historia
    /// </summary>
    public interface IChapter
    {
        /// <summary>
        /// Nombre del capítulo
        /// </summary>
        string ChapterName { get; }
        
        /// <summary>
        /// Número total de escenas en el capítulo
        /// </summary>
        int TotalScenes { get; }
        
        /// <summary>
        /// Obtiene la información de una escena específica
        /// </summary>
        /// <param name="sceneIndex">Índice de la escena (0-based)</param>
        /// <returns>Información de la escena o null si el índice es inválido</returns>
        ChapterScene GetScene(int sceneIndex);
        
        /// <summary>
        /// Verifica si el capítulo tiene más escenas después del índice dado
        /// </summary>
        /// <param name="currentSceneIndex">Índice de la escena actual</param>
        /// <returns>True si hay más escenas, False si es la última</returns>
        bool HasNextScene(int currentSceneIndex);
    }
}
