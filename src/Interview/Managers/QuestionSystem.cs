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
                1,
                null,
                QuestionCategory.Absurd
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

            // ========== NUEVAS PREGUNTAS POR CATEGORÍA ==========
            
            // ========== A) PREGUNTAS DE PERSONALIDAD ==========
            
            // Pregunta: ¿Cuál es tu mayor defecto y por qué sigue vivo?
            _allQuestions.Add(new Question(
                "q_personality_01",
                "¿Cuál es tu mayor defecto y por qué sigue vivo?",
                new List<Answer>
                {
                    new Answer("Mi mayor defecto es que soy demasiado perfeccionista... espera, eso es mentira", 5, 5, AnswerType.AbsurdCoherent, "Meta-defecto."),
                    new Answer("No tengo defectos, solo características no documentadas", 0, 10, AnswerType.AbsurdExtreme, "¿Como un bug no reportado?"),
                    new Answer("Mi mayor defecto es que no reconozco mis defectos", 5, 0, AnswerType.Professional, "Circular y honesto."),
                    new Answer("Mi mayor defecto es que elimino los defectos de otros", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente amenazante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Prefieres trabajar bajo presión o presionar a otros?
            _allQuestions.Add(new Question(
                "q_personality_02",
                "¿Prefieres trabajar bajo presión o presionar a otros?",
                new List<Answer>
                {
                    new Answer("Bajo presión, me motiva el desafío", 5, 0, AnswerType.Professional, "Estándar y profesional."),
                    new Answer("Presionar a otros, es más eficiente", 0, 10, AnswerType.AbsurdCoherent, "Eficiencia cuestionable."),
                    new Answer("Ambas, creo presión y luego trabajo bajo ella", 0, 15, AnswerType.AbsurdExtreme, "¿Auto-presión?"),
                    new Answer("Presionar a otros hasta que colapsen, luego trabajo sobre sus ruinas", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente cruel.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Te consideras alguien estable o solo estás fingiendo?
            _allQuestions.Add(new Question(
                "q_personality_03",
                "¿Te consideras alguien estable o solo estás fingiendo?",
                new List<Answer>
                {
                    new Answer("Estable, con momentos de inestabilidad controlada", 5, 0, AnswerType.Professional, "Estable... con asteriscos."),
                    new Answer("Fingiendo, pero muy bien, nadie lo nota", 0, 10, AnswerType.AbsurdCoherent, "Actor profesional."),
                    new Answer("No sé, depende del día y de la fase lunar", 0, 15, AnswerType.AbsurdExtreme, "¿Influencia lunar?"),
                    new Answer("Soy estable en mi inestabilidad, es mi constante", -5, 15, AnswerType.Sociopathic, "Eso es... filosófico y preocupante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Qué haces cuando cometes un error? (No, "culpar a los demás" no cuenta… ¿o sí?)
            _allQuestions.Add(new Question(
                "q_personality_04",
                "¿Qué haces cuando cometes un error? (No, \"culpar a los demás\" no cuenta… ¿o sí?)",
                new List<Answer>
                {
                    new Answer("Reconozco el error, lo documento y busco soluciones", 5, 0, AnswerType.Professional, "Procedimental y responsable."),
                    new Answer("Culpo a los demás, pero de forma creativa y documentada", 0, 10, AnswerType.AbsurdCoherent, "Culpabilidad con estilo."),
                    new Answer("Me convierto en el error, así no puedo cometerlo de nuevo", 0, 15, AnswerType.AbsurdExtreme, "¿Metamorfosis de error?"),
                    new Answer("Elimino la evidencia del error y culpo a un tercero inexistente", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Qué tan seguido dudas de ti mismo? ¿Solo hoy o siempre?
            _allQuestions.Add(new Question(
                "q_personality_05",
                "¿Qué tan seguido dudas de ti mismo? ¿Solo hoy o siempre?",
                new List<Answer>
                {
                    new Answer("Ocasionalmente, es saludable cuestionarse", 5, 0, AnswerType.Professional, "Autoconsciente y saludable."),
                    new Answer("Siempre, pero dudo incluso de mis dudas", 0, 10, AnswerType.AbsurdCoherent, "Meta-duda."),
                    new Answer("Nunca, estoy 100% seguro de que dudo constantemente", 0, 15, AnswerType.AbsurdExtreme, "Paradójico y confuso."),
                    new Answer("No dudo de mí, solo de la existencia de otros", -10, 20, AnswerType.Sociopathic, "Eso es... solipsista y perturbador.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: Describe tu ética de trabajo usando una sola mentira.
            _allQuestions.Add(new Question(
                "q_personality_06",
                "Describe tu ética de trabajo usando una sola mentira.",
                new List<Answer>
                {
                    new Answer("Soy honesto, trabajador y nunca miento... espera", 5, 5, AnswerType.AbsurdCoherent, "Meta-mentira."),
                    new Answer("Mi ética es perfecta, como esta respuesta", 0, 10, AnswerType.AbsurdExtreme, "¿Mentira perfecta?"),
                    new Answer("Trabajo duro, pero esta descripción es falsa", 5, 0, AnswerType.Professional, "Paradójico y honesto."),
                    new Answer("No tengo ética, pero eso es mentira... o no", -5, 15, AnswerType.Sociopathic, "Eso es... confuso y circular.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Qué tan bien manejas el estrés del que tú mismo eres responsable?
            _allQuestions.Add(new Question(
                "q_personality_07",
                "¿Qué tan bien manejas el estrés del que tú mismo eres responsable?",
                new List<Answer>
                {
                    new Answer("Bien, reconozco cuando lo genero y lo manejo", 5, 0, AnswerType.Professional, "Autoconsciente y responsable."),
                    new Answer("Perfectamente, porque lo creé, así que lo controlo", 0, 10, AnswerType.AbsurdCoherent, "Creador y controlador."),
                    new Answer("Mal, pero sigo creándolo porque es adictivo", 0, 15, AnswerType.AbsurdExtreme, "¿Adicción al estrés?"),
                    new Answer("Lo manejo creando más estrés para otros", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente tóxico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Qué harías si tu jefe te ignora? Además de llorar.
            _allQuestions.Add(new Question(
                "q_personality_08",
                "¿Qué harías si tu jefe te ignora? Además de llorar.",
                new List<Answer>
                {
                    new Answer("Buscaría comunicación directa y profesional", 5, 0, AnswerType.Professional, "Proactivo y profesional."),
                    new Answer("Me convertiría en su sombra hasta que me note", 0, 10, AnswerType.AbsurdCoherent, "Persistencia sombría."),
                    new Answer("Crearía un culto en su honor para llamar su atención", 0, 15, AnswerType.AbsurdExtreme, "¿Culto corporativo?"),
                    new Answer("Lo ignoraría de vuelta, pero de forma más efectiva", -5, 15, AnswerType.Sociopathic, "Eso es... competitivo y pasivo-agresivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: ¿Qué te motiva más: el dinero o huir del fracaso?
            _allQuestions.Add(new Question(
                "q_personality_09",
                "¿Qué te motiva más: el dinero o huir del fracaso?",
                new List<Answer>
                {
                    new Answer("El dinero, pero con propósito y valores", 5, 0, AnswerType.Professional, "Práctico y con valores."),
                    new Answer("Huir del fracaso, pero cobrando mientras huyo", 0, 10, AnswerType.AbsurdCoherent, "Eficiencia en la huida."),
                    new Answer("Ambos, pero el fracaso me persigue más rápido", 0, 15, AnswerType.AbsurdExtreme, "¿Persecución existencial?"),
                    new Answer("Ninguno, me motiva ver fracasar a otros", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente cruel.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // Pregunta: Si fueras un meme, ¿qué meme serías y por qué?
            _allQuestions.Add(new Question(
                "q_personality_10",
                "Si fueras un meme, ¿qué meme serías y por qué?",
                new List<Answer>
                {
                    new Answer("El de 'This is fine' porque manejo bien el caos", 5, 5, AnswerType.AbsurdCoherent, "Relatable y honesto."),
                    new Answer("El de 'Distracted boyfriend' pero con código", 0, 10, AnswerType.AbsurdExtreme, "¿Código como distracción?"),
                    new Answer("El de éxito, pero irónicamente", 5, 0, AnswerType.Professional, "Meta y autocrítico."),
                    new Answer("El de 'I have no idea what I'm doing' pero aplicado a la vida", -5, 15, AnswerType.Sociopathic, "Eso es... honesto y preocupante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Personality
            ));

            // ========== B) PREGUNTAS DE EXPERIENCIA ==========
            
            // Pregunta: ¿Cuál fue la cosa más innecesaria que aprendiste en tu empleo anterior?
            _allQuestions.Add(new Question(
                "q_experience_01",
                "¿Cuál fue la cosa más innecesaria que aprendiste en tu empleo anterior?",
                new List<Answer>
                {
                    new Answer("A usar un sistema obsoleto que nadie usa", 5, 0, AnswerType.Professional, "Relatable y técnico."),
                    new Answer("A fingir que escucho en las reuniones", 0, 10, AnswerType.AbsurdCoherent, "Habilidad social cuestionable."),
                    new Answer("A comunicarme solo con memes corporativos", 0, 15, AnswerType.AbsurdExtreme, "¿Lenguaje de memes?"),
                    new Answer("A culpar a otros sin dejar evidencia", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Cuántos correos sin leer son aceptables? (No digas cero, nadie te cree.)
            _allQuestions.Add(new Question(
                "q_experience_02",
                "¿Cuántos correos sin leer son aceptables? (No digas cero, nadie te cree.)",
                new List<Answer>
                {
                    new Answer("Menos de 50, mantengo un límite razonable", 5, 0, AnswerType.Professional, "Organizado... relativamente."),
                    new Answer("999+, es un logro personal", 0, 10, AnswerType.AbsurdCoherent, "¿Logro de procrastinación?"),
                    new Answer("No cuento, solo marco todos como leídos", 0, 15, AnswerType.AbsurdExtreme, "¿Fingir que leíste todo?"),
                    new Answer("Infinitos, los correos no existen si no los abro", -5, 15, AnswerType.Sociopathic, "Eso es... solipsista y evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Estás familiarizado con sistemas que nadie usa desde 2007?
            _allQuestions.Add(new Question(
                "q_experience_03",
                "¿Estás familiarizado con sistemas que nadie usa desde 2007?",
                new List<Answer>
                {
                    new Answer("Sí, trabajé con sistemas legacy", 5, 0, AnswerType.Professional, "Experiencia en legacy."),
                    new Answer("Sí, los uso por nostalgia y terquedad", 0, 10, AnswerType.AbsurdCoherent, "Nostálgico y terco."),
                    new Answer("Sí, los creé en 2007 y aún los mantengo", 0, 15, AnswerType.AbsurdExtreme, "¿Creador y mantenedor eterno?"),
                    new Answer("Sí, los uso para torturar a nuevos empleados", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente cruel.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: Describe un proyecto en el que trabajaste… pero que jamás debió existir.
            _allQuestions.Add(new Question(
                "q_experience_04",
                "Describe un proyecto en el que trabajaste… pero que jamás debió existir.",
                new List<Answer>
                {
                    new Answer("Un sistema de gestión que nadie usó", 5, 0, AnswerType.Professional, "Relatable y común."),
                    new Answer("Una app para rastrear cuántas veces alguien respira", 0, 10, AnswerType.AbsurdCoherent, "¿App de respiración?"),
                    new Answer("Un proyecto para automatizar el aburrimiento", 0, 15, AnswerType.AbsurdExtreme, "¿Automatización de aburrimiento?"),
                    new Answer("Un sistema para eliminar proyectos innecesarios... que nunca se usó", -5, 15, AnswerType.Sociopathic, "Eso es... irónico y circular.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Qué tan cómodo te sientes con tareas que no están en tu descripción de puesto?
            _allQuestions.Add(new Question(
                "q_experience_05",
                "¿Qué tan cómodo te sientes con tareas que no están en tu descripción de puesto?",
                new List<Answer>
                {
                    new Answer("Cómodo, soy flexible y adaptable", 5, 0, AnswerType.Professional, "Flexible y profesional."),
                    new Answer("Muy cómodo, es mi zona de confort fuera de mi zona", 0, 10, AnswerType.AbsurdCoherent, "¿Zona de confort expandida?"),
                    new Answer("Tan cómodo que hago tareas que no existen", 0, 15, AnswerType.AbsurdExtreme, "¿Tareas imaginarias?"),
                    new Answer("Cómodo, especialmente si son tareas de otros", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente invasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Cuál ha sido tu mayor logro que nadie jamás te reconoció?
            _allQuestions.Add(new Question(
                "q_experience_06",
                "¿Cuál ha sido tu mayor logro que nadie jamás te reconoció?",
                new List<Answer>
                {
                    new Answer("Implementar una solución que ahorró tiempo pero nadie notó", 5, 0, AnswerType.Professional, "Modesto y efectivo."),
                    new Answer("Sobrevivir a una reunión de 3 horas sin dormirme", 0, 10, AnswerType.AbsurdCoherent, "¿Logro de resistencia?"),
                    new Answer("Crear un sistema que funciona tan bien que nadie sabe que existe", 0, 15, AnswerType.AbsurdExtreme, "¿Sistema invisible?"),
                    new Answer("Eliminar un bug que nadie sabía que existía", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Cuántas horas extras crees que puedes soportar antes de colapsar?
            _allQuestions.Add(new Question(
                "q_experience_07",
                "¿Cuántas horas extras crees que puedes soportar antes de colapsar?",
                new List<Answer>
                {
                    new Answer("Hasta 10 horas, con descansos razonables", 5, 0, AnswerType.Professional, "Razonable y con límites."),
                    new Answer("Hasta el colapso, luego me reinicio", 0, 10, AnswerType.AbsurdCoherent, "¿Sistema con reinicio?"),
                    new Answer("Infinitas, ya colapsé y sigo funcionando", 0, 15, AnswerType.AbsurdExtreme, "¿Funcionamiento post-colapso?"),
                    new Answer("Hasta que otros colapsen primero", -10, 20, AnswerType.Sociopathic, "Eso es... competitivo y tóxico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Has tenido un jefe peor que yo? Cuidado con tu respuesta.
            _allQuestions.Add(new Question(
                "q_experience_08",
                "¿Has tenido un jefe peor que yo? Cuidado con tu respuesta.",
                new List<Answer>
                {
                    new Answer("No, todos han sido diferentes y valiosos", 5, 0, AnswerType.Professional, "Diplomático y evasivo."),
                    new Answer("Sí, pero no puedo decirlo porque está escuchando", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia justificada?"),
                    new Answer("No lo sé, aún no termino de evaluarte", 0, 15, AnswerType.AbsurdExtreme, "¿Evaluación en curso?"),
                    new Answer("Sí, pero lo eliminé de mi memoria", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Qué te hace mejor candidato que la gente que sí estudió?
            _allQuestions.Add(new Question(
                "q_experience_09",
                "¿Qué te hace mejor candidato que la gente que sí estudió?",
                new List<Answer>
                {
                    new Answer("Mi experiencia práctica y aprendizaje autodidacta", 5, 0, AnswerType.Professional, "Confianza y experiencia."),
                    new Answer("No tengo límites impuestos por la educación formal", 0, 10, AnswerType.AbsurdCoherent, "¿Creatividad sin límites?"),
                    new Answer("Estudié en la universidad de la vida... y YouTube", 0, 15, AnswerType.AbsurdExtreme, "¿Educación digital?"),
                    new Answer("No estudié, así que no tengo malos hábitos que desaprender", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente optimista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // Pregunta: ¿Qué tanto dependía tu antiguo trabajo de Google?
            _allQuestions.Add(new Question(
                "q_experience_10",
                "¿Qué tanto dependía tu antiguo trabajo de Google?",
                new List<Answer>
                {
                    new Answer("Moderadamente, usábamos herramientas de Google", 5, 0, AnswerType.Professional, "Estándar y común."),
                    new Answer("Completamente, si Google caía, caíamos todos", 0, 10, AnswerType.AbsurdCoherent, "¿Dependencia total?"),
                    new Answer("Tanto que contratamos a Google como empleado", 0, 15, AnswerType.AbsurdExtreme, "¿Google como empleado?"),
                    new Answer("Tanto que Google nos despidió por usar demasiado", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente absurdo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Experience
            ));

            // ========== C) PREGUNTAS ÉTICAS ==========
            
            // Pregunta: Si vieras un paquete de galletas en la oficina sin nombre, ¿lo tomas o solo piensas en tomarlo?
            _allQuestions.Add(new Question(
                "q_ethics_01",
                "Si vieras un paquete de galletas en la oficina sin nombre, ¿lo tomas o solo piensas en tomarlo?",
                new List<Answer>
                {
                    new Answer("Preguntaría primero si alguien las dejó", 5, 0, AnswerType.Professional, "Respetuoso y considerado."),
                    new Answer("Lo pienso mucho, luego lo tomo de todas formas", 0, 10, AnswerType.AbsurdCoherent, "¿Pensamiento justificativo?"),
                    new Answer("Solo lo pienso, pero muy intensamente", 0, 15, AnswerType.AbsurdExtreme, "¿Pensamiento intenso?"),
                    new Answer("Lo tomo y dejo una nota diciendo 'gracias'", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente pasivo-agresivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Robarías ideas de un compañero si él no las usa bien?
            _allQuestions.Add(new Question(
                "q_ethics_02",
                "¿Robarías ideas de un compañero si él no las usa bien?",
                new List<Answer>
                {
                    new Answer("No, colaboraría con él para mejorarlas", 5, 0, AnswerType.Professional, "Colaborativo y ético."),
                    new Answer("Sí, pero le daría crédito... en mi mente", 0, 10, AnswerType.AbsurdCoherent, "¿Crédito mental?"),
                    new Answer("Sí, pero las mejoraría tanto que serían mías", 0, 15, AnswerType.AbsurdExtreme, "¿Mejora como apropiación?"),
                    new Answer("Sí, y luego le vendería las ideas mejoradas", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente oportunista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Qué tan malo es mentir en tu CV si nadie lo revisa?
            _allQuestions.Add(new Question(
                "q_ethics_03",
                "¿Qué tan malo es mentir en tu CV si nadie lo revisa?",
                new List<Answer>
                {
                    new Answer("Muy malo, la honestidad es fundamental", 5, 0, AnswerType.Professional, "Ético y honesto."),
                    new Answer("Depende, ¿mentiras pequeñas o grandes?", 0, 10, AnswerType.AbsurdCoherent, "¿Escala de mentiras?"),
                    new Answer("No es malo si nadie lo sabe... ¿verdad?", 0, 15, AnswerType.AbsurdExtreme, "¿Filosofía de mentiras?"),
                    new Answer("No es mentir si crees tus propias mentiras", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente autoengañoso.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Dirías que eres honesto? ¿Seguro? ¿Muy seguro?
            _allQuestions.Add(new Question(
                "q_ethics_04",
                "¿Dirías que eres honesto? ¿Seguro? ¿Muy seguro?",
                new List<Answer>
                {
                    new Answer("Sí, la honestidad es importante para mí", 5, 0, AnswerType.Professional, "Confianza y valores."),
                    new Answer("Sí... creo... ¿eso cuenta como honesto?", 0, 10, AnswerType.AbsurdCoherent, "¿Honestidad dudosa?"),
                    new Answer("Sí, pero solo cuando es conveniente", 0, 15, AnswerType.AbsurdExtreme, "¿Honestidad selectiva?"),
                    new Answer("Sí, soy honesto sobre ser deshonesto", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente paradójico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Pedirías perdón aunque no te arrepientas?
            _allQuestions.Add(new Question(
                "q_ethics_05",
                "¿Pedirías perdón aunque no te arrepientas?",
                new List<Answer>
                {
                    new Answer("No, solo pediría perdón si realmente me arrepiento", 5, 0, AnswerType.Professional, "Sincero y auténtico."),
                    new Answer("Sí, es más eficiente que el conflicto", 0, 10, AnswerType.AbsurdCoherent, "¿Eficiencia sobre sinceridad?"),
                    new Answer("Sí, pero con un guiño para que sepan que no es real", 0, 15, AnswerType.AbsurdExtreme, "¿Perdón irónico?"),
                    new Answer("Sí, y luego haría lo mismo de nuevo", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente cíclico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Prefieres quedar bien o tener la razón? No puedes elegir "ambos".
            _allQuestions.Add(new Question(
                "q_ethics_06",
                "¿Prefieres quedar bien o tener la razón? No puedes elegir \"ambos\".",
                new List<Answer>
                {
                    new Answer("Tener la razón, pero de forma diplomática", 5, 0, AnswerType.Professional, "Equilibrado y profesional."),
                    new Answer("Quedar bien, la razón es sobrevalorada", 0, 10, AnswerType.AbsurdCoherent, "¿Pragmatismo social?"),
                    new Answer("Tener la razón, incluso si significa quedar mal", 0, 15, AnswerType.AbsurdExtreme, "¿Principios sobre relaciones?"),
                    new Answer("Quedar bien mientras tengo la razón en secreto", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente estratégico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Has descubierto fallas en la empresa antes de entrar aquí?
            _allQuestions.Add(new Question(
                "q_ethics_07",
                "¿Has descubierto fallas en la empresa antes de entrar aquí?",
                new List<Answer>
                {
                    new Answer("No, investigué pero no encontré problemas graves", 5, 0, AnswerType.Professional, "Investigador y honesto."),
                    new Answer("Sí, pero las guardé como material de negociación", 0, 10, AnswerType.AbsurdCoherent, "¿Leverage ético?"),
                    new Answer("Sí, las encontré hackeando sus sistemas... de forma ética", 0, 15, AnswerType.AbsurdExtreme, "¿Hacking ético?"),
                    new Answer("Sí, y las usaré si no me contratan", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente amenazante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: ¿Qué harías si tu compañero siempre llega tarde?
            _allQuestions.Add(new Question(
                "q_ethics_08",
                "¿Qué harías si tu compañero siempre llega tarde?",
                new List<Answer>
                {
                    new Answer("Hablaría con él directamente sobre el problema", 5, 0, AnswerType.Professional, "Comunicativo y directo."),
                    new Answer("Llegaría más tarde que él para establecer dominancia", 0, 10, AnswerType.AbsurdCoherent, "¿Competencia de tardanza?"),
                    new Answer("Documentaría cada vez que llega tarde con fotos", 0, 15, AnswerType.AbsurdExtreme, "¿Evidencia fotográfica?"),
                    new Answer("Llegaría antes y cambiaría todos los relojes", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente manipulador.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: Si el café se acaba, ¿a quién culparías primero?
            _allQuestions.Add(new Question(
                "q_ethics_09",
                "Si el café se acaba, ¿a quién culparías primero?",
                new List<Answer>
                {
                    new Answer("A nadie, buscaría una solución", 5, 0, AnswerType.Professional, "Proactivo y solucionador."),
                    new Answer("Al último que lo usó, obviamente", 0, 10, AnswerType.AbsurdCoherent, "¿Lógica de culpabilidad?"),
                    new Answer("A mí mismo, por no haberlo previsto", 0, 15, AnswerType.AbsurdExtreme, "¿Auto-culpa preventiva?"),
                    new Answer("A un tercero inexistente para evitar conflicto", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // Pregunta: En una escala del 1 al 10, ¿qué tan peligroso es darte poder?
            _allQuestions.Add(new Question(
                "q_ethics_10",
                "En una escala del 1 al 10, ¿qué tan peligroso es darte poder?",
                new List<Answer>
                {
                    new Answer("Un 3, uso el poder responsablemente", 5, 0, AnswerType.Professional, "Modesto y responsable."),
                    new Answer("Un 7, pero de forma creativa y divertida", 0, 10, AnswerType.AbsurdCoherent, "¿Poder creativo?"),
                    new Answer("Un 11, pero solo los fines de semana", 0, 15, AnswerType.AbsurdExtreme, "¿Poder de fin de semana?"),
                    new Answer("Un 10, pero prometo usarlo bien... creo", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente preocupante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Ethics
            ));

            // ========== D) PREGUNTAS ABSURDAS/SURREALISTAS ==========
            
            // Pregunta: ¿Cuántos patos necesitas para perder tu compostura?
            _allQuestions.Add(new Question(
                "q_absurd_01",
                "¿Cuántos patos necesitas para perder tu compostura?",
                new List<Answer>
                {
                    new Answer("Muchos, tengo alta tolerancia a patos", 5, 5, AnswerType.AbsurdCoherent, "¿Tolerancia a patos?"),
                    new Answer("Uno, si ese pato sabe programar", 0, 10, AnswerType.AbsurdExtreme, "¿Pato programador?"),
                    new Answer("Ninguno, ya perdí la compostura hace años", 0, 15, AnswerType.AbsurdExtreme, "¿Compostura perdida?"),
                    new Answer("Depende, ¿los patos están organizados?", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente estratégico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: Define tu estilo laboral usando sabores de helado.
            _allQuestions.Add(new Question(
                "q_absurd_02",
                "Define tu estilo laboral usando sabores de helado.",
                new List<Answer>
                {
                    new Answer("Vainilla, clásico y confiable", 5, 0, AnswerType.Professional, "Tradicional y estable."),
                    new Answer("Chocolate con chispas, impredecible pero delicioso", 0, 10, AnswerType.AbsurdCoherent, "¿Impredecibilidad sabrosa?"),
                    new Answer("Helado de pescado, nadie lo entiende pero existe", 0, 15, AnswerType.AbsurdExtreme, "¿Existencia cuestionable?"),
                    new Answer("Todos los sabores mezclados, caos organizado", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente caótico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: ¿Qué opinas de trabajar al lado de un compañero imaginario?
            _allQuestions.Add(new Question(
                "q_absurd_03",
                "¿Qué opinas de trabajar al lado de un compañero imaginario?",
                new List<Answer>
                {
                    new Answer("No lo recomendaría, prefiero compañeros reales", 5, 0, AnswerType.Professional, "Práctico y realista."),
                    new Answer("Genial, no habla en las reuniones", 0, 10, AnswerType.AbsurdCoherent, "¿Compañero silencioso?"),
                    new Answer("Perfecto, ya tengo uno y funciona bien", 0, 15, AnswerType.AbsurdExtreme, "¿Compañero existente?"),
                    new Answer("Mejor que mis compañeros reales", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente crítico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: ¿Cuál es tu villano favorito y por qué se parece a tu ex jefe?
            _allQuestions.Add(new Question(
                "q_absurd_04",
                "¿Cuál es tu villano favorito y por qué se parece a tu ex jefe?",
                new List<Answer>
                {
                    new Answer("No tengo villanos favoritos relacionados con jefes", 5, 0, AnswerType.Professional, "Diplomático y evasivo."),
                    new Answer("El Joker, ambos tienen planes complicados", 0, 10, AnswerType.AbsurdCoherent, "¿Planes complicados?"),
                    new Answer("Darth Vader, ambos tienen problemas de respiración", 0, 15, AnswerType.AbsurdExtreme, "¿Problemas respiratorios?"),
                    new Answer("Todos, porque todos se parecen a mi ex jefe", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente traumático.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: Es lunes. ¿Vives o sobrevives?
            _allQuestions.Add(new Question(
                "q_absurd_05",
                "Es lunes. ¿Vives o sobrevives?",
                new List<Answer>
                {
                    new Answer("Vivo, los lunes son como cualquier otro día", 5, 0, AnswerType.Professional, "Positivo y equilibrado."),
                    new Answer("Sobrevivo, pero con estilo", 0, 10, AnswerType.AbsurdCoherent, "¿Supervivencia estilizada?"),
                    new Answer("Ni vivo ni sobrevivo, existo en un limbo", 0, 15, AnswerType.AbsurdExtreme, "¿Existencia limbo?"),
                    new Answer("Sobrevivo para que otros no puedan", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente competitivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: Si el éxito fuera un animal, ¿qué animal te mordería?
            _allQuestions.Add(new Question(
                "q_absurd_06",
                "Si el éxito fuera un animal, ¿qué animal te mordería?",
                new List<Answer>
                {
                    new Answer("Un perro amigable, accesible y leal", 5, 0, AnswerType.Professional, "Positivo y accesible."),
                    new Answer("Un gato, porque es impredecible", 0, 10, AnswerType.AbsurdCoherent, "¿Impredecibilidad felina?"),
                    new Answer("Un pulpo, porque tiene muchos brazos para alcanzarme", 0, 15, AnswerType.AbsurdExtreme, "¿Pulpo multi-brazo?"),
                    new Answer("Un tiburón, porque ya me mordió", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente traumático.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: ¿Qué harías si descubres que la oficina es una simulación?
            _allQuestions.Add(new Question(
                "q_absurd_07",
                "¿Qué harías si descubres que la oficina es una simulación?",
                new List<Answer>
                {
                    new Answer("Reportaría el bug y buscaría una solución", 5, 0, AnswerType.Professional, "Procedimental y lógico."),
                    new Answer("Explotaría los bugs para mi beneficio", 0, 10, AnswerType.AbsurdCoherent, "¿Explotación de bugs?"),
                    new Answer("Me convertiría en el administrador de la simulación", 0, 15, AnswerType.AbsurdExtreme, "¿Admin de simulación?"),
                    new Answer("Eliminaría la simulación y a todos en ella", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente destructivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: ¿Qué harías si el entrevistador fuera un clon? (No confirmo nada.)
            _allQuestions.Add(new Question(
                "q_absurd_08",
                "¿Qué harías si el entrevistador fuera un clon? (No confirmo nada.)",
                new List<Answer>
                {
                    new Answer("Trataría al clon con el mismo respeto", 5, 0, AnswerType.Professional, "Respetuoso e inclusivo."),
                    new Answer("Le preguntaría cuál es el original", 0, 10, AnswerType.AbsurdCoherent, "¿Identificación de original?"),
                    new Answer("Me convertiría en su clon para entenderlo mejor", 0, 15, AnswerType.AbsurdExtreme, "¿Clon de clon?"),
                    new Answer("Eliminaría al clon y me haría pasar por el original", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente suplantador.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: Si tu autoestima fuera un programa, ¿ya habría crasheado?
            _allQuestions.Add(new Question(
                "q_absurd_09",
                "Si tu autoestima fuera un programa, ¿ya habría crasheado?",
                new List<Answer>
                {
                    new Answer("No, está bien optimizado y estable", 5, 0, AnswerType.Professional, "Confianza y estabilidad."),
                    new Answer("Sí, pero se reinicia automáticamente", 0, 10, AnswerType.AbsurdCoherent, "¿Auto-reinicio?"),
                    new Answer("Sí, y está en un loop infinito de errores", 0, 15, AnswerType.AbsurdExtreme, "¿Loop de errores?"),
                    new Answer("Sí, y eliminé el código fuente", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente irreversible.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // Pregunta: ¿Cómo reaccionas cuando el silencio se vuelve demasiado largo… como ahora?
            _allQuestions.Add(new Question(
                "q_absurd_10",
                "¿Cómo reaccionas cuando el silencio se vuelve demasiado largo… como ahora?",
                new List<Answer>
                {
                    new Answer("Me siento cómodo con el silencio", 5, 0, AnswerType.Professional, "Tranquilo y cómodo."),
                    new Answer("Empiezo a hablar conmigo mismo", 0, 10, AnswerType.AbsurdCoherent, "¿Auto-conversación?"),
                    new Answer("El silencio me habla y le respondo", 0, 15, AnswerType.AbsurdExtreme, "¿Conversación con silencio?"),
                    new Answer("Lleno el silencio con pensamientos destructivos", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente preocupante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Absurd
            ));

            // ========== PREGUNTAS CONTRADICTORIAS ==========
            // Estas preguntas contradicen a otras preguntas anteriores
            
            // Pregunta contradictoria: ¿Por qué deberías contratar a alguien tan seguro de sí mismo?
            // (Contradice respuestas sobre dudas personales)
            _allQuestions.Add(new Question(
                "q_contradict_01",
                "¿Por qué deberíamos contratar a alguien tan seguro de sí mismo?",
                new List<Answer>
                {
                    new Answer("Porque la confianza genera resultados", 5, 0, AnswerType.Professional, "Confianza y resultados."),
                    new Answer("Porque la seguridad es contagiosa", 0, 10, AnswerType.AbsurdCoherent, "¿Seguridad contagiosa?"),
                    new Answer("Porque si no estoy seguro, ¿quién lo estará?", 0, 15, AnswerType.AbsurdExtreme, "¿Seguridad absoluta?"),
                    new Answer("Porque la seguridad es mi única constante", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente inamovible.")
                },
                QuestionType.Base,
                0,
                "q_personality_05", // Contradice la pregunta sobre dudas
                QuestionCategory.Personality
            ));

            // Pregunta contradictoria: ¿Por qué pareces tan poco seguro de ti mismo?
            // (Contradice respuestas sobre seguridad)
            _allQuestions.Add(new Question(
                "q_contradict_02",
                "¿Por qué pareces tan poco seguro de ti mismo?",
                new List<Answer>
                {
                    new Answer("Soy realista sobre mis capacidades", 5, 0, AnswerType.Professional, "Modestia y realismo."),
                    new Answer("La inseguridad es mi zona de confort", 0, 10, AnswerType.AbsurdCoherent, "¿Zona de confort insegura?"),
                    new Answer("Soy tan inseguro que dudo de mi inseguridad", 0, 15, AnswerType.AbsurdExtreme, "¿Meta-inseguridad?"),
                    new Answer("La inseguridad es un superpoder disfrazado", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente optimista.")
                },
                QuestionType.Base,
                0,
                "q_contradict_01", // Contradice la pregunta sobre seguridad
                QuestionCategory.Personality
            ));

            // Pregunta contradictoria: Define tu mayor fortaleza.
            // (Contradice la pregunta sobre defectos)
            _allQuestions.Add(new Question(
                "q_contradict_03",
                "Define tu mayor fortaleza.",
                new List<Answer>
                {
                    new Answer("Mi capacidad de adaptación y aprendizaje", 5, 0, AnswerType.Professional, "Flexible y aprendedor."),
                    new Answer("Mi habilidad para convertir debilidades en fortalezas", 0, 10, AnswerType.AbsurdCoherent, "¿Transformación de debilidades?"),
                    new Answer("Mi fortaleza es que no tengo debilidades", 0, 15, AnswerType.AbsurdExtreme, "¿Fortaleza absoluta?"),
                    new Answer("Mi mayor fortaleza es eliminar las fortalezas de otros", -15, 20, AnswerType.Sociopathic, "Eso es... específicamente competitivo.")
                },
                QuestionType.Base,
                0,
                "q_personality_01", // Contradice la pregunta sobre defectos
                QuestionCategory.Personality
            ));

            // Pregunta contradictoria: Define por qué esa fortaleza es en realidad un problema.
            // (Contradice la pregunta sobre fortalezas)
            _allQuestions.Add(new Question(
                "q_contradict_04",
                "Define por qué esa fortaleza es en realidad un problema.",
                new List<Answer>
                {
                    new Answer("Puede volverse obsesión si no se controla", 5, 0, AnswerType.Professional, "Autoconsciente y balanceado."),
                    new Answer("Porque las fortalezas extremas son debilidades disfrazadas", 0, 10, AnswerType.AbsurdCoherent, "¿Filosofía de fortalezas?"),
                    new Answer("Porque no tengo otras características, solo esa", 0, 15, AnswerType.AbsurdExtreme, "¿Unidimensionalidad?"),
                    new Answer("Porque uso mi fortaleza para debilitar a otros", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente tóxico.")
                },
                QuestionType.Base,
                0,
                "q_contradict_03", // Contradice la pregunta sobre fortalezas
                QuestionCategory.Personality
            ));

            // Pregunta contradictoria: ¿Eres una persona proactiva?
            // (Contradice respuestas sobre reactividad)
            _allQuestions.Add(new Question(
                "q_contradict_05",
                "¿Eres una persona proactiva?",
                new List<Answer>
                {
                    new Answer("Sí, anticipo problemas y busco soluciones", 5, 0, AnswerType.Professional, "Proactivo y anticipador."),
                    new Answer("Sí, pero solo cuando es necesario", 0, 10, AnswerType.AbsurdCoherent, "¿Proactividad selectiva?"),
                    new Answer("Sí, proactivamente evito ser proactivo", 0, 15, AnswerType.AbsurdExtreme, "¿Meta-proactividad?"),
                    new Answer("Sí, proactivamente creo problemas para resolverlos", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente circular.")
                },
                QuestionType.Base,
                0,
                null, // No contradice una pregunta específica, pero puede generar contradicción
                QuestionCategory.Personality
            ));

            // Pregunta contradictoria: ¿Por qué eres tan reactivo en lugar de proactivo?
            // (Contradice la pregunta sobre proactividad)
            _allQuestions.Add(new Question(
                "q_contradict_06",
                "¿Por qué eres tan reactivo en lugar de proactivo?",
                new List<Answer>
                {
                    new Answer("Prefiero analizar antes de actuar", 5, 0, AnswerType.Professional, "Analítico y cuidadoso."),
                    new Answer("La reactividad es más eficiente que la proactividad", 0, 10, AnswerType.AbsurdCoherent, "¿Eficiencia reactiva?"),
                    new Answer("Soy tan reactivo que reacciono a mi propia proactividad", 0, 15, AnswerType.AbsurdExtreme, "¿Reactividad meta?"),
                    new Answer("Reacciono mejor cuando otros actúan primero", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente oportunista.")
                },
                QuestionType.Base,
                0,
                "q_contradict_05", // Contradice la pregunta sobre proactividad
                QuestionCategory.Personality
            ));
        }

        /// <summary>
        /// Obtiene la siguiente pregunta disponible (aleatoria, priorizando contradicciones)
        /// </summary>
        public Question GetNextQuestion()
        {
            var gameState = _stateManager.GameState;
            var random = new System.Random();

            // PRIORIDAD 1: Buscar preguntas contradictorias que puedan aparecer
            // (preguntas que contradicen a preguntas ya respondidas)
            var contradictoryQuestions = _allQuestions
                .Where(q => !string.IsNullOrEmpty(q.ContradictsQuestionId) &&
                           !gameState.AnsweredQuestionIds.Contains(q.Id) &&
                           gameState.AnsweredQuestionIds.Contains(q.ContradictsQuestionId) &&
                           (q.Type == QuestionType.Base || (q.Type == QuestionType.Special && CheckUnlockConditions(q))))
                .ToList();

            if (contradictoryQuestions.Count > 0)
            {
                // 70% de probabilidad de mostrar una pregunta contradictoria si hay disponibles
                if (random.Next(100) < 70)
                {
                    return contradictoryQuestions[random.Next(contradictoryQuestions.Count)];
                }
            }

            // PRIORIDAD 2: Preguntas base no respondidas (aleatorias)
            var baseQuestions = _allQuestions
                .Where(q => q.Type == QuestionType.Base && !gameState.AnsweredQuestionIds.Contains(q.Id))
                .ToList();

            if (baseQuestions.Count > 0)
            {
                return baseQuestions[random.Next(baseQuestions.Count)];
            }

            // PRIORIDAD 3: Preguntas especiales disponibles (aleatorias)
            var specialQuestions = _allQuestions
                .Where(q => q.Type == QuestionType.Special && 
                           !gameState.AnsweredQuestionIds.Contains(q.Id) &&
                           CheckUnlockConditions(q))
                .ToList();

            if (specialQuestions.Count > 0)
            {
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

