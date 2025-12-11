using Godot;
using TheLastInterview.Interview.Models;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager que maneja los estados ocultos de la entrevista
    /// </summary>
    public class StateManager
    {
        private GameState _gameState;

        /// <summary>
        /// Estado actual del juego
        /// </summary>
        public GameState GameState => _gameState;

        /// <summary>
        /// Constructor
        /// </summary>
        public StateManager()
        {
            _gameState = new GameState();
        }

        /// <summary>
        /// Aplica una respuesta y actualiza el estado
        /// </summary>
        public void ApplyAnswer(Answer answer)
        {
            _gameState.NormalPoints += answer.NormalPoints;
            _gameState.ChaosPoints += answer.ChaosPoints;
            _gameState.QuestionsAnswered++;
            _gameState.AnswerHistory.Add(answer.Text);
            _gameState.AnswerTypeHistory.Add(answer.Type);

            // Actualizar estado basado en puntos totales
            UpdateInterviewState();
        }

        /// <summary>
        /// Actualiza el estado de la entrevista basado en los puntos
        /// </summary>
        private void UpdateInterviewState()
        {
            int totalPoints = _gameState.TotalPoints;

            if (totalPoints >= 91)
            {
                // Estados especiales basados en tipo de respuestas predominantes
                if (IsPredominantType(AnswerType.AbsurdCoherent) || IsPredominantType(AnswerType.AbsurdExtreme))
                {
                    _gameState.CurrentState = InterviewState.HiredByMistake;
                }
                else if (IsPredominantType(AnswerType.Aggressive) || IsPredominantType(AnswerType.Sociopathic))
                {
                    _gameState.CurrentState = InterviewState.ViolentlyExpelled;
                }
                else
                {
                    _gameState.CurrentState = InterviewState.Chaos;
                }
            }
            else if (totalPoints >= 61)
            {
                _gameState.CurrentState = InterviewState.Chaos;
            }
            else if (totalPoints >= 31)
            {
                _gameState.CurrentState = InterviewState.Tense;
            }
            else
            {
                _gameState.CurrentState = InterviewState.Normal;
            }
        }

        /// <summary>
        /// Verifica si un tipo de respuesta es predominante
        /// </summary>
        private bool IsPredominantType(AnswerType type)
        {
            // Contar respuestas del tipo en el historial
            // Por ahora, simplificado: verificar si hay más respuestas de este tipo
            // TODO: Implementar lógica más sofisticada si es necesario
            return false; // Placeholder
        }

        /// <summary>
        /// Marca una pregunta como respondida
        /// </summary>
        public void MarkQuestionAnswered(string questionId)
        {
            _gameState.AnsweredQuestionIds.Add(questionId);
        }

        /// <summary>
        /// Marca una pregunta como respondida y actualiza el contador de preguntas no-meta
        /// </summary>
        public void MarkQuestionAnswered(Question question)
        {
            if (question == null) return;
            
            _gameState.AnsweredQuestionIds.Add(question.Id);
            
            // Si es una pregunta meta, reiniciar el contador
            if (question.Category == QuestionCategory.Meta)
            {
                _gameState.NonMetaQuestionsAnswered = 0;
            }
            else
            {
                // Si no es meta, incrementar el contador
                _gameState.NonMetaQuestionsAnswered++;
            }
        }

        /// <summary>
        /// Verifica si una pregunta ya fue respondida
        /// </summary>
        public bool IsQuestionAnswered(string questionId)
        {
            return _gameState.AnsweredQuestionIds.Contains(questionId);
        }

        /// <summary>
        /// Reinicia el estado del juego
        /// </summary>
        public void Reset()
        {
            _gameState.Reset();
        }
    }
}

