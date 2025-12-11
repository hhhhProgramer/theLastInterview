namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Modelo que representa un ending del juego
    /// </summary>
    public class Ending
    {
        /// <summary>
        /// ID único del ending
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Título del ending
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Descripción del ending
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Condiciones para obtener este ending
        /// </summary>
        public EndingCondition Condition { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Ending(string id, string title, string description, EndingCondition condition)
        {
            Id = id;
            Title = title;
            Description = description;
            Condition = condition;
        }
    }

    /// <summary>
    /// Condición para obtener un ending
    /// </summary>
    public class EndingCondition
    {
        /// <summary>
        /// Rango mínimo de puntos totales
        /// </summary>
        public int? MinTotalPoints { get; set; }

        /// <summary>
        /// Rango máximo de puntos totales
        /// </summary>
        public int? MaxTotalPoints { get; set; }

        /// <summary>
        /// Rango mínimo de puntos Normal
        /// </summary>
        public int? MinNormalPoints { get; set; }

        /// <summary>
        /// Rango máximo de puntos Normal
        /// </summary>
        public int? MaxNormalPoints { get; set; }

        /// <summary>
        /// Rango mínimo de puntos Caos
        /// </summary>
        public int? MinChaosPoints { get; set; }

        /// <summary>
        /// Rango máximo de puntos Caos
        /// </summary>
        public int? MaxChaosPoints { get; set; }

        /// <summary>
        /// Estado requerido (si es null, cualquier estado)
        /// </summary>
        public InterviewState? RequiredState { get; set; }

        /// <summary>
        /// Tipo de respuesta predominante requerido
        /// </summary>
        public AnswerType? PredominantAnswerType { get; set; }

        /// <summary>
        /// IDs de respuestas específicas que deben estar en el historial
        /// </summary>
        public System.Collections.Generic.List<string> RequiredAnswerIds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public EndingCondition()
        {
            RequiredAnswerIds = new System.Collections.Generic.List<string>();
        }
    }
}

