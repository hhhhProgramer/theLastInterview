using System.Collections.Generic;

namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Modelo que representa una pregunta del entrevistador
    /// </summary>
    public class Question
    {
        /// <summary>
        /// ID único de la pregunta
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Texto de la pregunta
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Lista de respuestas disponibles para esta pregunta
        /// </summary>
        public List<Answer> Answers { get; set; }

        /// <summary>
        /// Tipo de pregunta
        /// </summary>
        public QuestionType Type { get; set; }

        /// <summary>
        /// Condiciones para desbloquear esta pregunta (opcional)
        /// </summary>
        public List<QuestionCondition> UnlockConditions { get; set; }

        /// <summary>
        /// Orden de aparición (para preguntas base)
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Question(string id, string text, List<Answer> answers, QuestionType type = QuestionType.Base, int order = 0)
        {
            Id = id;
            Text = text;
            Answers = answers ?? new List<Answer>();
            Type = type;
            UnlockConditions = new List<QuestionCondition>();
            Order = order;
        }
    }

    /// <summary>
    /// Tipos de pregunta
    /// </summary>
    public enum QuestionType
    {
        Base,           // Pregunta base que siempre aparece
        Special,        // Pregunta especial condicional
        Secret          // Pregunta secreta con condiciones específicas
    }

    /// <summary>
    /// Condición para desbloquear una pregunta
    /// </summary>
    public class QuestionCondition
    {
        /// <summary>
        /// Tipo de condición
        /// </summary>
        public ConditionType Type { get; set; }

        /// <summary>
        /// Valor requerido para la condición
        /// </summary>
        public int RequiredValue { get; set; }

        /// <summary>
        /// ID de pregunta relacionada (para condiciones de respuesta)
        /// </summary>
        public string RelatedQuestionId { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionCondition(ConditionType type, int requiredValue = 0, string relatedQuestionId = null)
        {
            Type = type;
            RequiredValue = requiredValue;
            RelatedQuestionId = relatedQuestionId;
        }
    }

    /// <summary>
    /// Tipos de condición
    /// </summary>
    public enum ConditionType
    {
        MinNormalPoints,    // Mínimo de puntos Normal
        MinChaosPoints,     // Mínimo de puntos Caos
        MaxNormalPoints,    // Máximo de puntos Normal
        MaxChaosPoints,     // Máximo de puntos Caos
        StateTense,         // Estado debe ser Tenso
        StateChaos,         // Estado debe ser Caos
        SpecificAnswer      // Respuesta específica a otra pregunta
    }
}

