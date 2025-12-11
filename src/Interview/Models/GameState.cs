namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Estado actual del juego
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Puntos acumulados de Normal (profesional)
        /// </summary>
        public int NormalPoints { get; set; } = 0;

        /// <summary>
        /// Puntos acumulados de Caos (absurdo/agresivo)
        /// </summary>
        public int ChaosPoints { get; set; } = 0;

        /// <summary>
        /// Estado actual de la entrevista
        /// </summary>
        public InterviewState CurrentState { get; set; } = InterviewState.Normal;

        /// <summary>
        /// Número de preguntas respondidas
        /// </summary>
        public int QuestionsAnswered { get; set; } = 0;

        /// <summary>
        /// IDs de preguntas ya respondidas
        /// </summary>
        public System.Collections.Generic.HashSet<string> AnsweredQuestionIds { get; set; } = new System.Collections.Generic.HashSet<string>();

        /// <summary>
        /// Historial de respuestas (para endings especiales)
        /// </summary>
        public System.Collections.Generic.List<string> AnswerHistory { get; set; } = new System.Collections.Generic.List<string>();

        /// <summary>
        /// Calcula el total de puntos
        /// </summary>
        public int TotalPoints => NormalPoints + ChaosPoints;

        /// <summary>
        /// Reinicia el estado del juego
        /// </summary>
        public void Reset()
        {
            NormalPoints = 0;
            ChaosPoints = 0;
            CurrentState = InterviewState.Normal;
            QuestionsAnswered = 0;
            AnsweredQuestionIds.Clear();
            AnswerHistory.Clear();
        }
    }

    /// <summary>
    /// Estados posibles de la entrevista
    /// </summary>
    public enum InterviewState
    {
        Normal,                 // 0-30 puntos: Todo relativamente profesional
        Tense,                  // 31-60 puntos: Entrevistador empieza a sospechar
        Chaos,                  // 61-90 puntos: Entrevistador pierde la cordura
        HiredByMistake,         // 91-100 puntos, respuestas absurdas pero "encajan"
        ViolentlyExpelled       // 91-100 puntos, respuestas muy agresivas
    }
}

