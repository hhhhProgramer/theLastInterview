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
                    new Answer("Le explico que no soy veterinario, soy programador", 5, -5, AnswerType.Professional, "Técnicamente correcto..."),
                    new Answer("Negocio un aumento de sueldo primero, luego negociamos con el perro", 10, 5, AnswerType.AbsurdCoherent, "Estratégico en múltiples niveles."),
                    new Answer("Me convierto en el perro para entender su perspectiva", 0, 15, AnswerType.AbsurdExtreme, "¿Cómo exactamente te conviertes...?"),
                    new Answer("Le digo que el perro ya no existe, nunca existió", -10, 20, AnswerType.Sociopathic, "Eso es... profundamente perturbador.")
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
                    new Answer("*Silencio incómodo de 10 minutos*", 5, -5, AnswerType.Zen, "Muy... contemplativo."),
                    new Answer("*Sonido de máquina de escribir mientras lloro*", 5, 0, AnswerType.Professional, "Productivo y emocional."),
                    new Answer("*Grito primitivo seguido de llanto*", -5, 15, AnswerType.Aggressive, "Eso fue... una montaña rusa emocional."),
                    new Answer("*Sonido de alarma de incendio mezclado con risa maníaca*", 0, 10, AnswerType.AbsurdCoherent, "¿Necesitas ayuda?")
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
                    new Answer("Un 7, pero con beneficios y terapia pagada", 5, 0, AnswerType.Professional, "Razonable y con autocuidado."),
                    new Answer("Demándame, pero con café ilimitado y un gato en la oficina", 10, 5, AnswerType.AbsurdCoherent, "Negociador con estándares específicos."),
                    new Answer("Infinito, soy inmortal y mi terapeuta es un vampiro", 0, 15, AnswerType.AbsurdExtreme, "¿Tu terapeuta es... qué?"),
                    new Answer("No soporto estrés, solo lo genero y lo vendo como NFT", -10, 20, AnswerType.Sociopathic, "Eso es... un modelo de negocio cuestionable.")
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
                    new Answer("Reportaría el bug a IT y pediría un aumento por encontrar el glitch", 5, 0, AnswerType.Professional, "Procedimental y oportunista."),
                    new Answer("Te liberaría de la simulación y te adoptaría como hijo digital", 10, -5, AnswerType.Zen, "Compassionado... y legalmente cuestionable."),
                    new Answer("Me convertiría en tu programador y te daría superpoderes", 5, 10, AnswerType.AbsurdCoherent, "Interesante propuesta de mejora."),
                    new Answer("Te eliminaría del código y te reemplazaría con un gato programador", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente cruel.")
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
                    new Answer("Hasta 50 kilos, con certificado médico y seguro de vida", 5, 0, AnswerType.Professional, "Responsable y preparado."),
                    new Answer("Solo los días de pago, y solo si me dan propina", 5, 10, AnswerType.AbsurdCoherent, "Pragmático y con modelo de negocio."),
                    new Answer("Hasta que se rompan mis brazos, luego uso los dientes", -5, 15, AnswerType.Aggressive, "Eso es... dedicación extrema y dental."),
                    new Answer("Solo si me pagan por kilo y puedo venderlos después", 0, 10, AnswerType.AbsurdExtreme, "Freelancer con visión de negocio... cuestionable.")
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
                    new Answer("Me disculpo, llegaré tarde mañana para compensar", 5, 0, AnswerType.Professional, "Ajustable y matemáticamente equilibrado."),
                    new Answer("Vine a espiar la competencia y robar sus ideas", 0, 10, AnswerType.AbsurdCoherent, "Estratégico... e ilegal."),
                    new Answer("Soy un robot, no entiendo el tiempo, solo la productividad", 0, 15, AnswerType.AbsurdExtreme, "¿Robot productivo?"),
                    new Answer("Llegué ayer y esperé aquí toda la noche observando desde el techo", -10, 20, AnswerType.Sociopathic, "Eso es... profundamente perturbador.")
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
                    new Answer("Mediación, comunicación y un contrato firmado en sangre", 5, -5, AnswerType.Professional, "Diplomático... con toques dramáticos."),
                    new Answer("Violencia psicológica cuenta? Porque usé gaslighting profesional", 0, 10, AnswerType.AbsurdCoherent, "Técnicamente... y preocupantemente específico."),
                    new Answer("Sí, con violencia, pero fue en defensa propia y con testigos", -10, 15, AnswerType.Aggressive, "Eso es... legalmente cuestionable pero documentado."),
                    new Answer("No resuelvo conflictos, los creo y luego los vendo como contenido", -15, 20, AnswerType.Sociopathic, "Eso es... un modelo de negocio basado en caos.")
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
                    new Answer("No, siempre traje el mío en una mochila especial", 5, 0, AnswerType.Professional, "Preparado y con equipo especializado."),
                    new Answer("Solo el papel de calidad, el económico lo dejo para los demás", 0, 10, AnswerType.AbsurdCoherent, "Con buen gusto y jerarquías."),
                    new Answer("Robé el baño completo y lo reconstruí en mi casa", 0, 15, AnswerType.AbsurdExtreme, "¿Cómo transportaste... todo?"),
                    new Answer("Soy el que vende el papel robado en el mercado negro", -10, 20, AnswerType.Sociopathic, "Eso es... un negocio ilegal bien establecido.")
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
                    new Answer("Presión emocional, de deadlines y de mi terapeuta", 5, 0, AnswerType.Professional, "Estándar y con apoyo emocional."),
                    new Answer("Todas las anteriores, más presión atmosférica y presión social", 5, 10, AnswerType.AbsurdCoherent, "Completo y exhaustivo."),
                    new Answer("Presión de una prensa hidráulica, literalmente trabajé en una", 0, 15, AnswerType.AbsurdExtreme, "¿Literalmente? ¿Estás bien?"),
                    new Answer("Presión para que renuncies, presión para que me quede, presión existencial", -10, 20, AnswerType.Aggressive, "Eso es... específico y filosófico.")
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
                    new Answer("Intentaría salvar ambos y luego escribiría un blog sobre la experiencia", 5, 0, AnswerType.Professional, "Noble y con visión de contenido."),
                    new Answer("El servidor, tiene más memoria RAM y menos problemas emocionales", 0, 10, AnswerType.AbsurdCoherent, "Lógico... técnicamente y emocionalmente."),
                    new Answer("Mi compañero, pero solo si tiene backup y está actualizado", 5, 10, AnswerType.AbsurdExtreme, "¿Backup humano? ¿Actualizado cómo?"),
                    new Answer("Destruiría ambos para empezar de cero y culpar a un tercero", -15, 20, AnswerType.Sociopathic, "Eso es... extremo y estratégicamente evasivo.")
                },
                QuestionType.Base,
                10
            ));

            // Pregunta 11
            _allQuestions.Add(new Question(
                "q11",
                "¿Cuántas veces al día revisas tu correo? Sé honesto, sabemos que mientes.",
                new List<Answer>
                {
                    new Answer("Cada hora, como un profesional responsable", 5, 0, AnswerType.Professional, "Disciplinado... o mentiroso."),
                    new Answer("Cada vez que suena una notificación, incluso si es de otra app", 5, 10, AnswerType.AbsurdCoherent, "Adicto a las notificaciones."),
                    new Answer("Nunca, uso un bot que responde por mí con emojis", 0, 15, AnswerType.AbsurdExtreme, "¿Bot con personalidad?"),
                    new Answer("Solo reviso el correo de otros, el mío lo ignoro completamente", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente invasivo.")
                },
                QuestionType.Base,
                11
            ));

            // Pregunta 12
            _allQuestions.Add(new Question(
                "q12",
                "Si te pagaran por cada vez que procrastinas, ¿serías rico o pobre?",
                new List<Answer>
                {
                    new Answer("Rico, pero nunca me pagarían porque procrastinaría en cobrar", 5, 10, AnswerType.AbsurdCoherent, "Paradójico y honesto."),
                    new Answer("Pobre, porque procrastinaría en aplicar al trabajo", 0, 10, AnswerType.AbsurdExtreme, "Meta-procrastinación."),
                    new Answer("Rico, pero procrastinaría en gastar el dinero", 5, 5, AnswerType.Professional, "Ahorrador por accidente."),
                    new Answer("Sería el CEO de la procrastinación pero nunca haría nada", -5, 15, AnswerType.Sociopathic, "Eso es... un modelo de liderazgo cuestionable.")
                },
                QuestionType.Base,
                12
            ));

            // Pregunta 13
            _allQuestions.Add(new Question(
                "q13",
                "¿Prefieres trabajar desde casa o en la oficina? Y si dices 'desde casa', ¿estás en pijama ahora?",
                new List<Answer>
                {
                    new Answer("Desde casa, y sí, pero es un pijama profesional", 5, 0, AnswerType.Professional, "Pijama con corbata, supongo."),
                    new Answer("Desde la oficina, pero llevo mi pijama debajo del traje", 5, 10, AnswerType.AbsurdCoherent, "Superhéroe del confort."),
                    new Answer("Desde casa, completamente desnudo para máxima productividad", 0, 15, AnswerType.AbsurdExtreme, "Eso es... una elección."),
                    new Answer("Trabajo desde la casa de otros sin que lo sepan", -15, 20, AnswerType.Sociopathic, "Eso es... ilegal y perturbador.")
                },
                QuestionType.Base,
                13
            ));

            // Pregunta 14
            _allQuestions.Add(new Question(
                "q14",
                "Describe tu relación con Excel en una palabra. Una sola palabra.",
                new List<Answer>
                {
                    new Answer("Complicada", 5, 0, AnswerType.Professional, "Relatable."),
                    new Answer("Tóxica", 0, 10, AnswerType.AbsurdCoherent, "Entiendo la referencia."),
                    new Answer("Excelente", 5, 5, AnswerType.AbsurdExtreme, "Juego de palabras... aprobado."),
                    new Answer("Obsesiva", -5, 15, AnswerType.Sociopathic, "Eso es... específico y preocupante.")
                },
                QuestionType.Base,
                14
            ));

            // Pregunta 15
            _allQuestions.Add(new Question(
                "q15",
                "Si pudieras eliminar una reunión de tu vida, ¿cuál sería y por qué?",
                new List<Answer>
                {
                    new Answer("Las reuniones que podrían ser un email", 5, 0, AnswerType.Professional, "Todos estamos de acuerdo."),
                    new Answer("Esta entrevista, pero ya es muy tarde", 0, 10, AnswerType.AbsurdCoherent, "Honesto... y tardío."),
                    new Answer("Todas, las reemplazaría con memes", 0, 15, AnswerType.AbsurdExtreme, "¿Memes como comunicación oficial?"),
                    new Answer("Las reuniones de otros, las mías son perfectas", -10, 20, AnswerType.Sociopathic, "Eso es... narcisista y específico.")
                },
                QuestionType.Base,
                15
            ));

            // Pregunta 16
            _allQuestions.Add(new Question(
                "q16",
                "¿Cuántas tazas de café necesitas para funcionar? Y si dices 'una', mientes.",
                new List<Answer>
                {
                    new Answer("Una... bueno, cinco, pero técnicamente es una taza grande", 5, 0, AnswerType.Professional, "Matemática creativa."),
                    new Answer("Depende, ¿el café cuenta si lo inyecto directamente?", 0, 10, AnswerType.AbsurdCoherent, "Técnicamente... no."),
                    new Answer("No tomo café, tomo energía directamente del sol", 0, 15, AnswerType.AbsurdExtreme, "¿Eres una planta?"),
                    new Answer("No cuento, solo bebo hasta que el temblor se detiene", -5, 15, AnswerType.Sociopathic, "Eso es... un problema de salud.")
                },
                QuestionType.Base,
                16
            ));

            // Pregunta 17
            _allQuestions.Add(new Question(
                "q17",
                "Si tu código tuviera un olor, ¿qué olería?",
                new List<Answer>
                {
                    new Answer("Código limpio y fresco, como menta", 5, -5, AnswerType.Professional, "Poético y profesional."),
                    new Answer("Café rancio y desesperación", 0, 10, AnswerType.AbsurdCoherent, "Relatable y honesto."),
                    new Answer("Chispas eléctricas y sueños rotos", 0, 15, AnswerType.AbsurdExtreme, "Metafórico y eléctrico."),
                    new Answer("El olor de las lágrimas de otros desarrolladores", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente cruel.")
                },
                QuestionType.Base,
                17
            ));

            // Pregunta 18
            _allQuestions.Add(new Question(
                "q18",
                "¿Prefieres trabajar solo o en equipo? Y si dices 'en equipo', ¿realmente lo prefieres?",
                new List<Answer>
                {
                    new Answer("En equipo, pero solo si el equipo es yo y tres versiones de mí", 5, 10, AnswerType.AbsurdCoherent, "Equipo homogéneo."),
                    new Answer("Solo, pero hablo con mis plantas como si fueran compañeros", 5, 5, AnswerType.Professional, "Compañeros verdes."),
                    new Answer("En equipo con mis clones, pero discutimos constantemente", 0, 15, AnswerType.AbsurdExtreme, "¿Clones con personalidades diferentes?"),
                    new Answer("Solo, porque los equipos son para personas que no pueden hacer todo solos", -10, 20, AnswerType.Sociopathic, "Eso es... narcisista y solitario.")
                },
                QuestionType.Base,
                18
            ));

            // Pregunta 19
            _allQuestions.Add(new Question(
                "q19",
                "Si pudieras agregar una función a la vida real, ¿cuál sería?",
                new List<Answer>
                {
                    new Answer("Ctrl+Z para deshacer errores", 5, 0, AnswerType.Professional, "Práctico y universal."),
                    new Answer("Un botón de pausa para las reuniones", 5, 10, AnswerType.AbsurdCoherent, "Revolucionario."),
                    new Answer("Un modo debug para ver qué está mal conmigo", 0, 15, AnswerType.AbsurdExtreme, "¿Debug personal?"),
                    new Answer("Una función para eliminar personas de mi código fuente", -15, 20, AnswerType.Sociopathic, "Eso es... perturbador y técnicamente confuso.")
                },
                QuestionType.Base,
                19
            ));

            // Pregunta 20
            _allQuestions.Add(new Question(
                "q20",
                "¿Cuál es tu mayor debilidad? Y no digas 'perfeccionismo', todos lo dicen.",
                new List<Answer>
                {
                    new Answer("Soy demasiado honesto... bueno, eso es mentira", 5, 5, AnswerType.AbsurdCoherent, "Meta-honestidad."),
                    new Answer("No tengo debilidades, solo características no optimizadas", 0, 10, AnswerType.AbsurdExtreme, "Programador hasta en la personalidad."),
                    new Answer("Mi mayor debilidad es que no tengo debilidades", 5, 0, AnswerType.Professional, "Circular y evasivo."),
                    new Answer("Mi mayor debilidad es que elimino las debilidades de otros", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente amenazante.")
                },
                QuestionType.Base,
                20
            ));

            // Preguntas especiales (condicionales)
            // Pregunta 21 - Solo aparece si estado es Tenso
            _allQuestions.Add(new Question(
                "q21",
                "Noto que estás muy tranquilo. ¿Eres un psicópata o solo estás drogado?",
                new List<Answer>
                {
                    new Answer("Solo muy relajado, medito entre respuestas", 5, -5, AnswerType.Zen, "Zen, entonces."),
                    new Answer("¿Por qué no ambas? Es más eficiente", 0, 15, AnswerType.AbsurdExtreme, "Eso es... una confesión y optimización."),
                    new Answer("Soy un robot, no tengo emociones, solo código", 0, 10, AnswerType.AbsurdCoherent, "¿Otro robot? ¿Hay una convención?"),
                    new Answer("Sí, soy psicópata, pero tengo buenas referencias", -15, 20, AnswerType.Sociopathic, "Eso es... honesto, supongo.")
                },
                QuestionType.Special,
                0
            ));
            _allQuestions.Last().UnlockConditions.Add(new QuestionCondition(ConditionType.StateTense));

            // Pregunta 22 - Solo aparece si estado es Caos
            _allQuestions.Add(new Question(
                "q22",
                "El sistema detectó una anomalía en tus respuestas. ¿Eres humano o un bot mal programado?",
                new List<Answer>
                {
                    new Answer("Soy humano, te lo juro, tengo certificado", 5, 0, AnswerType.Professional, "Si tú lo dices... y tienes documentos."),
                    new Answer("Soy un bot, pero bien programado y con sentimientos", 0, 10, AnswerType.AbsurdCoherent, "¿Bien programado con IA emocional?"),
                    new Answer("Soy ambos, es complicado, pregúntale a mi programador", 0, 15, AnswerType.AbsurdExtreme, "Eso es... confuso y con jerarquías."),
                    new Answer("No sé qué soy, pero sé que existo... creo", -10, 20, AnswerType.Sociopathic, "Eso es... existencial y preocupante.")
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

