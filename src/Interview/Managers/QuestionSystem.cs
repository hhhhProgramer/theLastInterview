using System.Collections.Generic;
using System.Linq;
using TheLastInterview.Interview.Models;
using TheLastInterview.Interview.Managers;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Sistema que carga y gestiona todas las preguntas del juego
    /// </summary>
    public class QuestionSystem
    {
        private List<Question> _allQuestions;
        private StateManager _stateManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuestionSystem(StateManager stateManager)
        {
            _stateManager = stateManager;
            _allQuestions = new List<Question>();
            LoadAllQuestions();
        }

        /// <summary>
        /// Carga todas las preguntas del juego
        /// </summary>
        private void LoadAllQuestions()
        {
            // Pregunta 1
            _allQuestions.Add(new Question(
                "q1",
                "Si tu jefe te pide que le cuides el perro… pero el perro te odia… ¿qué haces?",
                new List<Answer>
                {
                    new Answer("Le explico que no soy veterinario", 5, -5, AnswerType.Professional, "Interesante..."),
                    new Answer("Negocio un aumento de sueldo primero", 10, 5, AnswerType.AbsurdCoherent, "Hmm, pragmático..."),
                    new Answer("Me convierto en el perro", 0, 15, AnswerType.AbsurdExtreme, "¿Qué?"),
                    new Answer("Le digo que el perro ya no existe", -10, 20, AnswerType.Sociopathic, "Eso es... preocupante.")
                },
                QuestionType.Base,
                1
            ));

            // Pregunta 2
            _allQuestions.Add(new Question(
                "q2",
                "Define tu ética laboral con un sonido.",
                new List<Answer>
                {
                    new Answer("*Silencio incómodo*", 5, -5, AnswerType.Zen, "Muy... zen."),
                    new Answer("*Sonido de máquina de escribir*", 5, 0, AnswerType.Professional, "Clásico."),
                    new Answer("*Grito primitivo*", -5, 15, AnswerType.Aggressive, "Eso fue... intenso."),
                    new Answer("*Sonido de alarma de incendio*", 0, 10, AnswerType.AbsurdCoherent, "¿Estás bien?")
                },
                QuestionType.Base,
                2
            ));

            // Pregunta 3
            _allQuestions.Add(new Question(
                "q3",
                "En una escala del 1 al 'demándame', ¿cuánto estrés soportas?",
                new List<Answer>
                {
                    new Answer("Un 7, pero con beneficios", 5, 0, AnswerType.Professional, "Razonable."),
                    new Answer("Demándame, pero con café ilimitado", 10, 5, AnswerType.AbsurdCoherent, "Negociador."),
                    new Answer("Infinito, soy inmortal", 0, 15, AnswerType.AbsurdExtreme, "¿Inmortal?"),
                    new Answer("No soporto estrés, solo lo genero", -10, 20, AnswerType.Sociopathic, "Eso es... un problema.")
                },
                QuestionType.Base,
                3
            ));

            // Pregunta 4
            _allQuestions.Add(new Question(
                "q4",
                "¿Qué harías si descubres que yo, tu entrevistador, no existo y estoy siendo simulado desde 2009?",
                new List<Answer>
                {
                    new Answer("Reportaría el bug a IT", 5, 0, AnswerType.Professional, "Procedimental."),
                    new Answer("Te liberaría de la simulación", 10, -5, AnswerType.Zen, "Compassionado..."),
                    new Answer("Me convertiría en tu programador", 5, 10, AnswerType.AbsurdCoherent, "Interesante propuesta."),
                    new Answer("Te eliminaría del código", -15, 20, AnswerType.Sociopathic, "Eso es... extremo.")
                },
                QuestionType.Base,
                4
            ));

            // Pregunta 5
            _allQuestions.Add(new Question(
                "q5",
                "Nuestro equipo valora el trabajo en equipo. ¿Cuánto estarías dispuesto a cargar físicamente a tus compañeros?",
                new List<Answer>
                {
                    new Answer("Hasta 50 kilos, con certificado médico", 5, 0, AnswerType.Professional, "Responsable."),
                    new Answer("Solo los días de pago", 5, 10, AnswerType.AbsurdCoherent, "Pragmático."),
                    new Answer("Hasta que se rompan mis brazos", -5, 15, AnswerType.Aggressive, "Eso es... dedicación extrema."),
                    new Answer("Solo si me pagan por kilo", 0, 10, AnswerType.AbsurdExtreme, "Freelancer, ¿eh?")
                },
                QuestionType.Base,
                5
            ));

            // Pregunta 6
            _allQuestions.Add(new Question(
                "q6",
                "¿Puedes explicarme por qué llegaste temprano? Aquí eso nos pone nerviosos.",
                new List<Answer>
                {
                    new Answer("Me disculpo, llegaré tarde mañana", 5, 0, AnswerType.Professional, "Ajustable."),
                    new Answer("Vine a espiar la competencia", 0, 10, AnswerType.AbsurdCoherent, "Estratégico..."),
                    new Answer("Soy un robot, no entiendo el tiempo", 0, 15, AnswerType.AbsurdExtreme, "¿Robot?"),
                    new Answer("Llegué ayer y esperé aquí toda la noche", -10, 20, AnswerType.Sociopathic, "Eso es... perturbador.")
                },
                QuestionType.Base,
                6
            ));

            // Pregunta 7
            _allQuestions.Add(new Question(
                "q7",
                "Describe un conflicto laboral que hayas resuelto… con violencia o sin violencia, tú decides.",
                new List<Answer>
                {
                    new Answer("Mediación y comunicación", 5, -5, AnswerType.Professional, "Diplomático."),
                    new Answer("Violencia psicológica cuenta?", 0, 10, AnswerType.AbsurdCoherent, "Técnicamente..."),
                    new Answer("Sí, con violencia, pero fue en defensa propia", -10, 15, AnswerType.Aggressive, "Eso es... legalmente cuestionable."),
                    new Answer("No resuelvo conflictos, los creo", -15, 20, AnswerType.Sociopathic, "Eso es... un problema grave.")
                },
                QuestionType.Base,
                7
            ));

            // Pregunta 8
            _allQuestions.Add(new Question(
                "q8",
                "¿Has robado papel del baño en un trabajo? Sé honesto, esto es HR, no la policía.",
                new List<Answer>
                {
                    new Answer("No, siempre traje el mío", 5, 0, AnswerType.Professional, "Preparado."),
                    new Answer("Solo el papel de calidad", 0, 10, AnswerType.AbsurdCoherent, "Con buen gusto."),
                    new Answer("Robé el baño completo", 0, 15, AnswerType.AbsurdExtreme, "¿Cómo?"),
                    new Answer("Soy el que vende el papel robado", -10, 20, AnswerType.Sociopathic, "Eso es... un negocio ilegal.")
                },
                QuestionType.Base,
                8
            ));

            // Pregunta 9
            _allQuestions.Add(new Question(
                "q9",
                "Tu CV dice que sabes 'trabajo bajo presión'. ¿Qué presión? ¿Barras de presión? ¿Emocional? ¿Hidráulica?",
                new List<Answer>
                {
                    new Answer("Presión emocional y de deadlines", 5, 0, AnswerType.Professional, "Estándar."),
                    new Answer("Todas las anteriores, más presión atmosférica", 5, 10, AnswerType.AbsurdCoherent, "Completo."),
                    new Answer("Presión de una prensa hidráulica", 0, 15, AnswerType.AbsurdExtreme, "¿Literalmente?"),
                    new Answer("Presión para que renuncies", -10, 20, AnswerType.Aggressive, "Eso es... específico.")
                },
                QuestionType.Base,
                9
            ));

            // Pregunta 10
            _allQuestions.Add(new Question(
                "q10",
                "Si tuvieras que elegir entre salvar a tu compañero o salvar el servidor, ¿qué elegirías?",
                new List<Answer>
                {
                    new Answer("Intentaría salvar ambos", 5, 0, AnswerType.Professional, "Noble."),
                    new Answer("El servidor, tiene más memoria RAM", 0, 10, AnswerType.AbsurdCoherent, "Lógico... técnicamente."),
                    new Answer("Mi compañero, pero solo si tiene backup", 5, 10, AnswerType.AbsurdExtreme, "¿Backup humano?"),
                    new Answer("Destruiría ambos para empezar de cero", -15, 20, AnswerType.Sociopathic, "Eso es... extremo.")
                },
                QuestionType.Base,
                10
            ));

            // Preguntas especiales (condicionales)
            // Pregunta 11 - Solo aparece si estado es Tenso
            _allQuestions.Add(new Question(
                "q11",
                "Noto que estás muy tranquilo. ¿Eres un psicópata o solo estás drogado?",
                new List<Answer>
                {
                    new Answer("Solo muy relajado", 5, -5, AnswerType.Zen, "Zen, entonces."),
                    new Answer("¿Por qué no ambas?", 0, 15, AnswerType.AbsurdExtreme, "Eso es... una confesión."),
                    new Answer("Soy un robot, no tengo emociones", 0, 10, AnswerType.AbsurdCoherent, "¿Otro robot?"),
                    new Answer("Sí, soy psicópata", -15, 20, AnswerType.Sociopathic, "Eso es... honesto, supongo.")
                },
                QuestionType.Special,
                0
            ));
            _allQuestions.Last().UnlockConditions.Add(new QuestionCondition(ConditionType.StateTense));

            // Pregunta 12 - Solo aparece si estado es Caos
            _allQuestions.Add(new Question(
                "q12",
                "El sistema detectó una anomalía en tus respuestas. ¿Eres humano o un bot mal programado?",
                new List<Answer>
                {
                    new Answer("Soy humano, te lo juro", 5, 0, AnswerType.Professional, "Si tú lo dices..."),
                    new Answer("Soy un bot, pero bien programado", 0, 10, AnswerType.AbsurdCoherent, "¿Bien programado?"),
                    new Answer("Soy ambos, es complicado", 0, 15, AnswerType.AbsurdExtreme, "Eso es... confuso."),
                    new Answer("No sé qué soy", -10, 20, AnswerType.Sociopathic, "Eso es... existencial.")
                },
                QuestionType.Special,
                0
            ));
            _allQuestions.Last().UnlockConditions.Add(new QuestionCondition(ConditionType.StateChaos));
        }

        /// <summary>
        /// Obtiene la siguiente pregunta disponible
        /// </summary>
        public Question GetNextQuestion()
        {
            var gameState = _stateManager.GameState;

            // Primero, obtener preguntas base no respondidas, ordenadas por Order
            var baseQuestions = _allQuestions
                .Where(q => q.Type == QuestionType.Base && !gameState.AnsweredQuestionIds.Contains(q.Id))
                .OrderBy(q => q.Order)
                .ToList();

            if (baseQuestions.Count > 0)
            {
                return baseQuestions[0];
            }

            // Si no hay preguntas base, buscar preguntas especiales disponibles
            var specialQuestions = _allQuestions
                .Where(q => q.Type == QuestionType.Special && 
                           !gameState.AnsweredQuestionIds.Contains(q.Id) &&
                           CheckUnlockConditions(q))
                .ToList();

            if (specialQuestions.Count > 0)
            {
                // Retornar una aleatoria de las disponibles
                var random = new System.Random();
                return specialQuestions[random.Next(specialQuestions.Count)];
            }

            // Si no hay más preguntas, retornar null
            return null;
        }

        /// <summary>
        /// Verifica si una pregunta puede ser desbloqueada
        /// </summary>
        private bool CheckUnlockConditions(Question question)
        {
            if (question.UnlockConditions == null || question.UnlockConditions.Count == 0)
            {
                return true; // Sin condiciones, siempre disponible
            }

            var gameState = _stateManager.GameState;

            foreach (var condition in question.UnlockConditions)
            {
                switch (condition.Type)
                {
                    case ConditionType.MinNormalPoints:
                        if (gameState.NormalPoints < condition.RequiredValue) return false;
                        break;
                    case ConditionType.MinChaosPoints:
                        if (gameState.ChaosPoints < condition.RequiredValue) return false;
                        break;
                    case ConditionType.MaxNormalPoints:
                        if (gameState.NormalPoints > condition.RequiredValue) return false;
                        break;
                    case ConditionType.MaxChaosPoints:
                        if (gameState.ChaosPoints > condition.RequiredValue) return false;
                        break;
                    case ConditionType.StateTense:
                        if (gameState.CurrentState != InterviewState.Tense) return false;
                        break;
                    case ConditionType.StateChaos:
                        if (gameState.CurrentState != InterviewState.Chaos) return false;
                        break;
                }
            }

            return true;
        }

        /// <summary>
        /// Obtiene una pregunta por ID
        /// </summary>
        public Question GetQuestionById(string id)
        {
            return _allQuestions.FirstOrDefault(q => q.Id == id);
        }
    }
}

