namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Modelo que representa una respuesta del jugador
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Texto de la respuesta que se mostrará al jugador
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Puntos que suma a Normal (profesional)
        /// </summary>
        public int NormalPoints { get; set; }

        /// <summary>
        /// Puntos que suma a Caos (absurdo/agresivo)
        /// </summary>
        public int ChaosPoints { get; set; }

        /// <summary>
        /// Tipo de respuesta para determinar efectos especiales
        /// </summary>
        public AnswerType Type { get; set; }

        /// <summary>
        /// Texto de reacción del entrevistador después de esta respuesta
        /// </summary>
        public string ReactionText { get; set; }

        /// <summary>
        /// Texto descriptivo de consecuencia visual mínima (solo texto, sin gráficos)
        /// </summary>
        public string VisualConsequenceText { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Answer(string text, int normalPoints, int chaosPoints, AnswerType type, string reactionText = "", string visualConsequenceText = "")
        {
            Text = text;
            NormalPoints = normalPoints;
            ChaosPoints = chaosPoints;
            Type = type;
            ReactionText = reactionText;
            VisualConsequenceText = visualConsequenceText;
        }
    }

    /// <summary>
    /// Tipos de respuesta disponibles
    /// </summary>
    public enum AnswerType
    {
        Professional,      // Respuesta profesional
        AbsurdCoherent,    // Absurda pero coherente
        AbsurdExtreme,     // Absurda extrema
        Aggressive,        // Agresiva/extrema
        Zen,               // Zen/relajada
        Sociopathic        // Sociopática
    }
}

