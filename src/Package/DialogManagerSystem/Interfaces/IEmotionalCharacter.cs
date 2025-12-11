
using Package.Core.Enums;

namespace Package.Core.Interfaces
{
    /// <summary>
    /// Interfaz para personajes que pueden cambiar de emoción y hacer animaciones
    /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
    /// </summary>
    public interface IEmotionalCharacter
    {
        string CharacterId { get; }
        void ChangeEmotion(Emotion emotion);

        /// <summary>
        /// Inicia la animación de hablar (pulse)
        /// </summary>
        void PlaySpeakingAnimation();

        /// <summary>
        /// Detiene la animación de hablar y vuelve a idle
        /// </summary>
        void StopSpeakingAnimation();

        /// <summary>
        /// Oculta el personaje sin eliminarlo (para poder reaparecer después)
        /// </summary>
        /// <param name="duration">Duración de la animación de ocultamiento (opcional)</param>
        void Hide(float duration = 0.5f);
        
        /// <summary>
        /// Muestra el personaje
        /// </summary>
        void ShowCharacter(float duration = 0.5f);
    }
}

