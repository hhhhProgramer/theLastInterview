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

            // ========== PREGUNTAS META - ROMPEN LA 4TA PARED ==========
            
            // Pregunta: ¿Por qué estás leyendo esta pregunta tan seriamente?
            _allQuestions.Add(new Question(
                "q_meta_01",
                "¿Por qué estás leyendo esta pregunta tan seriamente?",
                new List<Answer>
                {
                    new Answer("Porque tomo todo en serio, incluso los juegos", 5, 0, AnswerType.Professional, "Consciente y reflexivo."),
                    new Answer("Porque no sé si esto es parte del juego o no", 0, 10, AnswerType.AbsurdCoherent, "¿Meta-confusión?"),
                    new Answer("Porque me diste miedo y ahora leo todo cuidadosamente", 0, 15, AnswerType.AbsurdExtreme, "¿Paranoia meta?"),
                    new Answer("Porque sospecho que me estás evaluando a través de esto", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente paranoico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te diste cuenta de que tu mouse tardó en moverse antes de responder? Yo sí.
            _allQuestions.Add(new Question(
                "q_meta_02",
                "¿Te diste cuenta de que tu mouse tardó en moverse antes de responder? Yo sí.",
                new List<Answer>
                {
                    new Answer("Sí, estaba pensando en la respuesta", 5, 0, AnswerType.Professional, "Reflexivo y cuidadoso."),
                    new Answer("No, pero ahora me siento observado", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia justificada?"),
                    new Answer("Mi mouse tiene vida propia, no es mi culpa", 0, 15, AnswerType.AbsurdExtreme, "¿Mouse autónomo?"),
                    new Answer("Estaba esperando a que me dieras una pista", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente dependiente.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Estás respondiendo rápido porque sabes que esto es un juego, o porque eres así en la vida real?
            _allQuestions.Add(new Question(
                "q_meta_03",
                "¿Estás respondiendo rápido porque sabes que esto es un juego, o porque eres así en la vida real?",
                new List<Answer>
                {
                    new Answer("Soy así en la vida real, tomo decisiones rápidas", 5, 0, AnswerType.Professional, "Decisivo y confiado."),
                    new Answer("Es un juego, así que no importa tanto", 0, 10, AnswerType.AbsurdCoherent, "¿Desapego lúdico?"),
                    new Answer("No sé, ¿cuál es la diferencia?", 0, 15, AnswerType.AbsurdExtreme, "¿Filosofía existencial?"),
                    new Answer("Respondo rápido para que no me juzgues más", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente evasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Estás eligiendo la opción más graciosa o la que te hace sentir menos culpable?
            _allQuestions.Add(new Question(
                "q_meta_04",
                "¿Estás eligiendo la opción más graciosa o la que te hace sentir menos culpable?",
                new List<Answer>
                {
                    new Answer("La que mejor representa mi respuesta honesta", 5, 0, AnswerType.Professional, "Honesto y auténtico."),
                    new Answer("La más graciosa, obviamente", 0, 10, AnswerType.AbsurdCoherent, "¿Prioridad al humor?"),
                    new Answer("Ambas, dependiendo de mi estado de ánimo", 0, 15, AnswerType.AbsurdExtreme, "¿Estrategia adaptativa?"),
                    new Answer("La que creo que quieres que elija", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente complaciente.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te gustaría que este juego dejara de juzgarte tanto?
            _allQuestions.Add(new Question(
                "q_meta_05",
                "¿Te gustaría que este juego dejara de juzgarte tanto?",
                new List<Answer>
                {
                    new Answer("No, el juicio es parte de la experiencia", 5, 0, AnswerType.Professional, "Aceptación y madurez."),
                    new Answer("Sí, pero sé que no va a pasar", 0, 10, AnswerType.AbsurdCoherent, "¿Resignación realista?"),
                    new Answer("Me gusta que me juzgue, es estimulante", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo lúdico?"),
                    new Answer("No me juzga, yo me juzgo a mí mismo", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente introspectivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Eres consciente de que el entrevistador solo existe porque presionaste "Start"?
            _allQuestions.Add(new Question(
                "q_meta_06",
                "¿Eres consciente de que el entrevistador solo existe porque presionaste \"Start\"?",
                new List<Answer>
                {
                    new Answer("Sí, es parte de la mecánica del juego", 5, 0, AnswerType.Professional, "Consciente del medio."),
                    new Answer("No, ahora me siento responsable de su existencia", 0, 10, AnswerType.AbsurdCoherent, "¿Carga existencial?"),
                    new Answer("Él existe porque yo existo, es simbiótico", 0, 15, AnswerType.AbsurdExtreme, "¿Filosofía simbiótica?"),
                    new Answer("Si dejo de jugar, ¿deja de existir? Eso es perturbador", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente existencial.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te sientes observado incluso cuando nadie juega?
            _allQuestions.Add(new Question(
                "q_meta_07",
                "¿Te sientes observado incluso cuando nadie juega?",
                new List<Answer>
                {
                    new Answer("No, es solo un juego", 5, 0, AnswerType.Professional, "Lúcido y racional."),
                    new Answer("Sí, pero solo un poco", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia leve?"),
                    new Answer("Siempre me siento observado, juego o no", 0, 15, AnswerType.AbsurdExtreme, "¿Paranoia constante?"),
                    new Answer("El juego me observa incluso cuando está cerrado", -10, 20, AnswerType.Sociopathic, "Eso es... específicamente paranoico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Hiciste clic por convicción o sólo porque no había otra opción decente?
            _allQuestions.Add(new Question(
                "q_meta_08",
                "¿Hiciste clic por convicción o sólo porque no había otra opción decente?",
                new List<Answer>
                {
                    new Answer("Por convicción, elegí la mejor opción", 5, 0, AnswerType.Professional, "Decisivo y confiado."),
                    new Answer("Porque era la menos mala de las opciones", 0, 10, AnswerType.AbsurdCoherent, "¿Pragmatismo forzado?"),
                    new Answer("Hice clic al azar y esperé lo mejor", 0, 15, AnswerType.AbsurdExtreme, "¿Estrategia aleatoria?"),
                    new Answer("Hice clic porque todas las opciones eran igual de malas", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente resignado.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Por qué sigues leyendo todo si podrías saltar directo al final?
            _allQuestions.Add(new Question(
                "q_meta_09",
                "¿Por qué sigues leyendo todo si podrías saltar directo al final?",
                new List<Answer>
                {
                    new Answer("Porque valoro la experiencia completa", 5, 0, AnswerType.Professional, "Apreciativo y paciente."),
                    new Answer("Porque tengo miedo de perderme algo importante", 0, 10, AnswerType.AbsurdCoherent, "¿FOMO lúdico?"),
                    new Answer("Porque no sé cómo saltar al final", 0, 15, AnswerType.AbsurdExtreme, "¿Ignorancia técnica?"),
                    new Answer("Porque el juego me obliga a leer todo", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente fatalista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Estás pensando lo que yo creo que estás pensando?
            _allQuestions.Add(new Question(
                "q_meta_10",
                "¿Estás pensando lo que yo creo que estás pensando?",
                new List<Answer>
                {
                    new Answer("Probablemente no, pero es interesante intentar adivinarlo", 5, 0, AnswerType.Professional, "Filosófico y abierto."),
                    new Answer("Sí, y me da miedo que lo sepas", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia justificada?"),
                    new Answer("No sé qué estás pensando que estoy pensando", 0, 15, AnswerType.AbsurdExtreme, "¿Meta-confusión?"),
                    new Answer("Estoy pensando que estás pensando que estoy pensando", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente recursivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // ========== PREGUNTAS QUE TE HABLAN DIRECTAMENTE A TI (JUGADOR) ==========
            
            // Pregunta: ¿Por qué sigues jugando esto en vez de hacer algo productivo?
            _allQuestions.Add(new Question(
                "q_meta_player_01",
                "¿Por qué sigues jugando esto en vez de hacer algo productivo?",
                new List<Answer>
                {
                    new Answer("Porque el entretenimiento también es válido", 5, 0, AnswerType.Professional, "Balanceado y consciente."),
                    new Answer("Porque esto es más interesante que ser productivo", 0, 10, AnswerType.AbsurdCoherent, "¿Prioridades cuestionables?"),
                    new Answer("Porque la productividad es una ilusión", 0, 15, AnswerType.AbsurdExtreme, "¿Filosofía anti-productividad?"),
                    new Answer("Porque estoy procrastinando y lo sé", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente honesto.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: Dime la verdad: ¿te estás riendo o solo moviendo el mouse como zombie?
            _allQuestions.Add(new Question(
                "q_meta_player_02",
                "Dime la verdad: ¿te estás riendo o solo moviendo el mouse como zombie?",
                new List<Answer>
                {
                    new Answer("Me estoy riendo, el juego es gracioso", 5, 0, AnswerType.Professional, "Apreciativo y honesto."),
                    new Answer("Un poco de ambos, la verdad", 0, 10, AnswerType.AbsurdCoherent, "¿Híbrido lúdico?"),
                    new Answer("Solo muevo el mouse, ya no siento nada", 0, 15, AnswerType.AbsurdExtreme, "¿Desensibilización lúdica?"),
                    new Answer("Soy un zombie que se ríe, es complicado", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente contradictorio.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿En qué momento decidiste que esta sería una buena idea para tu tarde?
            _allQuestions.Add(new Question(
                "q_meta_player_03",
                "¿En qué momento decidiste que esta sería una buena idea para tu tarde?",
                new List<Answer>
                {
                    new Answer("Cuando vi el concepto del juego", 5, 0, AnswerType.Professional, "Decisivo y curioso."),
                    new Answer("Cuando me aburrí de todo lo demás", 0, 10, AnswerType.AbsurdCoherent, "¿Último recurso?"),
                    new Answer("Nunca lo decidí, solo pasó", 0, 15, AnswerType.AbsurdExtreme, "¿Fatalismo lúdico?"),
                    new Answer("Cuando me di cuenta de que ya era muy tarde para arrepentirme", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente resignado.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Cuántos juegos de entrevista vas a jugar antes de aceptar que necesitas un descanso?
            _allQuestions.Add(new Question(
                "q_meta_player_04",
                "¿Cuántos juegos de entrevista vas a jugar antes de aceptar que necesitas un descanso?",
                new List<Answer>
                {
                    new Answer("Este es el primero y último", 5, 0, AnswerType.Professional, "Consciente y moderado."),
                    new Answer("Hasta que me canse, probablemente muchos", 0, 10, AnswerType.AbsurdCoherent, "¿Persistencia cuestionable?"),
                    new Answer("Infinitos, nunca descansaré", 0, 15, AnswerType.AbsurdExtreme, "¿Adicción lúdica?"),
                    new Answer("Hasta que el juego me diga que pare", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente dependiente.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te das cuenta de que tus elecciones no solo afectan al personaje… te afectan a ti?
            _allQuestions.Add(new Question(
                "q_meta_player_05",
                "¿Te das cuenta de que tus elecciones no solo afectan al personaje… te afectan a ti?",
                new List<Answer>
                {
                    new Answer("Sí, los juegos pueden ser reflexivos", 5, 0, AnswerType.Professional, "Consciente y reflexivo."),
                    new Answer("No, pero ahora me da miedo", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia justificada?"),
                    new Answer("Sí, y me gusta que me afecten", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo emocional?"),
                    new Answer("No me afectan, soy inmune a los juegos", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente negacionista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Estás presionando las opciones al azar? No mientas. Te estoy viendo.
            _allQuestions.Add(new Question(
                "q_meta_player_06",
                "¿Estás presionando las opciones al azar? No mientas. Te estoy viendo.",
                new List<Answer>
                {
                    new Answer("No, leo y elijo cuidadosamente", 5, 0, AnswerType.Professional, "Cuidadoso y deliberado."),
                    new Answer("A veces, cuando me aburro", 0, 10, AnswerType.AbsurdCoherent, "¿Honestidad parcial?"),
                    new Answer("Sí, pero solo para ver qué pasa", 0, 15, AnswerType.AbsurdExtreme, "¿Experimentación caótica?"),
                    new Answer("Sí, y me da igual si lo sabes", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente desafiante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Cuándo fue la última vez que guardaste algo importante?
            _allQuestions.Add(new Question(
                "q_meta_player_07",
                "¿Cuándo fue la última vez que guardaste algo importante?",
                new List<Answer>
                {
                    new Answer("Hace poco, soy cuidadoso con mis archivos", 5, 0, AnswerType.Professional, "Organizado y responsable."),
                    new Answer("No recuerdo, probablemente hace mucho", 0, 10, AnswerType.AbsurdCoherent, "¿Memoria cuestionable?"),
                    new Answer("Nunca, confío en la nube", 0, 15, AnswerType.AbsurdExtreme, "¿Fe tecnológica?"),
                    new Answer("No guardo nada, vivo peligrosamente", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente temerario.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿De verdad pensaste que habría una respuesta correcta aquí?
            _allQuestions.Add(new Question(
                "q_meta_player_08",
                "¿De verdad pensaste que habría una respuesta correcta aquí?",
                new List<Answer>
                {
                    new Answer("No, pero busco la mejor opción posible", 5, 0, AnswerType.Professional, "Realista y optimista."),
                    new Answer("Sí, y me decepcioné cuando no la encontré", 0, 10, AnswerType.AbsurdCoherent, "¿Expectativas irrealistas?"),
                    new Answer("No sé, sigo buscando", 0, 15, AnswerType.AbsurdExtreme, "¿Búsqueda infinita?"),
                    new Answer("Todas son incorrectas, así que todas son correctas", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente paradójico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Sabes que esta pregunta fue escrita solo para incomodarte a ti?
            _allQuestions.Add(new Question(
                "q_meta_player_09",
                "¿Sabes que esta pregunta fue escrita solo para incomodarte a ti?",
                new List<Answer>
                {
                    new Answer("Sí, y funciona", 5, 0, AnswerType.Professional, "Consciente y honesto."),
                    new Answer("No, pero ahora me siento incómodo", 0, 10, AnswerType.AbsurdCoherent, "¿Incomodidad retroactiva?"),
                    new Answer("Sí, pero me gusta sentirme incómodo", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo emocional?"),
                    new Answer("No me incomoda, soy inmune", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente resistente.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Crees que el juego te está evaluando? Porque sí lo está.
            _allQuestions.Add(new Question(
                "q_meta_player_10",
                "¿Crees que el juego te está evaluando? Porque sí lo está.",
                new List<Answer>
                {
                    new Answer("Sí, es parte de la mecánica del juego", 5, 0, AnswerType.Professional, "Consciente del sistema."),
                    new Answer("No, pero ahora me da miedo", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia justificada?"),
                    new Answer("Sí, y me gusta ser evaluado", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo evaluativo?"),
                    new Answer("No me importa, evaluar es subjetivo", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente desapegado.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // ========== PREGUNTAS QUE ADMITEN QUE SON UN JUEGO ==========
            
            // Pregunta: Si este juego tuviera presupuesto, ¿qué crees que verías en pantalla?
            _allQuestions.Add(new Question(
                "q_meta_game_01",
                "Si este juego tuviera presupuesto, ¿qué crees que verías en pantalla?",
                new List<Answer>
                {
                    new Answer("Animaciones y gráficos más elaborados", 5, 0, AnswerType.Professional, "Realista y visual."),
                    new Answer("Más preguntas absurdas como estas", 0, 10, AnswerType.AbsurdCoherent, "¿Prioridad al contenido?"),
                    new Answer("Un entrevistador 3D con expresiones faciales", 0, 15, AnswerType.AbsurdExtreme, "¿Realismo extremo?"),
                    new Answer("Nada, el presupuesto se gastaría en café", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente realista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te molesta que este juego no tenga IA pero sí preguntas profundas?
            _allQuestions.Add(new Question(
                "q_meta_game_02",
                "¿Te molesta que este juego no tenga IA pero sí preguntas profundas?",
                new List<Answer>
                {
                    new Answer("No, las preguntas escritas son más consistentes", 5, 0, AnswerType.Professional, "Apreciativo del diseño."),
                    new Answer("Un poco, pero me divierte de todas formas", 0, 10, AnswerType.AbsurdCoherent, "¿Aceptación condicional?"),
                    new Answer("Sí, quiero que el juego piense por sí mismo", 0, 15, AnswerType.AbsurdExtreme, "¿IA sentiente?"),
                    new Answer("No me molesta, me molesta que me lo recuerdes", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente contradictorio.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te gustaría que agregara más gráficos o ya te acostumbraste a mi pobreza visual?
            _allQuestions.Add(new Question(
                "q_meta_game_03",
                "¿Te gustaría que agregara más gráficos o ya te acostumbraste a mi pobreza visual?",
                new List<Answer>
                {
                    new Answer("El estilo actual es suficiente y funcional", 5, 0, AnswerType.Professional, "Apreciativo del estilo."),
                    new Answer("Más gráficos serían geniales, pero no es necesario", 0, 10, AnswerType.AbsurdCoherent, "¿Mejora opcional?"),
                    new Answer("Ya me acostumbré, no cambies nada", 0, 15, AnswerType.AbsurdExtreme, "¿Resistencia al cambio?"),
                    new Answer("Me gusta la pobreza visual, es auténtica", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente nostálgico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Qué te hace seguir aquí aunque no haya animaciones llamativas?
            _allQuestions.Add(new Question(
                "q_meta_game_04",
                "¿Qué te hace seguir aquí aunque no haya animaciones llamativas?",
                new List<Answer>
                {
                    new Answer("El contenido y las preguntas interesantes", 5, 0, AnswerType.Professional, "Valora el contenido."),
                    new Answer("La curiosidad de ver qué pregunta viene", 0, 10, AnswerType.AbsurdCoherent, "¿Curiosidad adictiva?"),
                    new Answer("No sé, pero sigo aquí", 0, 15, AnswerType.AbsurdExtreme, "¿Inercia lúdica?"),
                    new Answer("Me siento atrapado y no puedo salir", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente prisionero.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Crees que este juego necesita mecánicas nuevas o ya te resignaste?
            _allQuestions.Add(new Question(
                "q_meta_game_05",
                "¿Crees que este juego necesita mecánicas nuevas o ya te resignaste?",
                new List<Answer>
                {
                    new Answer("Las mecánicas actuales son suficientes", 5, 0, AnswerType.Professional, "Satisfecho con el diseño."),
                    new Answer("Más mecánicas serían geniales", 0, 10, AnswerType.AbsurdCoherent, "¿Deseo de expansión?"),
                    new Answer("Ya me resigné, pero sigo jugando", 0, 15, AnswerType.AbsurdExtreme, "¿Resignación activa?"),
                    new Answer("No necesito mecánicas, solo más preguntas como esta", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente meta.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Sientes que las opciones importan aunque yo sé que no?
            _allQuestions.Add(new Question(
                "q_meta_game_06",
                "¿Sientes que las opciones importan aunque yo sé que no?",
                new List<Answer>
                {
                    new Answer("Sí, cada elección afecta la experiencia", 5, 0, AnswerType.Professional, "Valora las decisiones."),
                    new Answer("A veces, pero sé que es ilusorio", 0, 10, AnswerType.AbsurdCoherent, "¿Ilusión consciente?"),
                    new Answer("No, pero finjo que sí para disfrutar más", 0, 15, AnswerType.AbsurdExtreme, "¿Autoengaño lúdico?"),
                    new Answer("Nada importa, todo es aleatorio", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente nihilista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Si pudieras reescribir esta pregunta, la harías mejor o peor?
            _allQuestions.Add(new Question(
                "q_meta_game_07",
                "¿Si pudieras reescribir esta pregunta, la harías mejor o peor?",
                new List<Answer>
                {
                    new Answer("Mejor, pero respeto la original", 5, 0, AnswerType.Professional, "Respetuoso y constructivo."),
                    new Answer("Peor, definitivamente peor", 0, 10, AnswerType.AbsurdCoherent, "¿Modestia excesiva?"),
                    new Answer("Igual, porque no sé escribir preguntas", 0, 15, AnswerType.AbsurdExtreme, "¿Auto-duda creativa?"),
                    new Answer("La reescribiría para que sea sobre mí", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente narcisista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Cuánto tardaste en darte cuenta de que tu final depende de un random?
            _allQuestions.Add(new Question(
                "q_meta_game_08",
                "¿Cuánto tardaste en darte cuenta de que tu final depende de un random?",
                new List<Answer>
                {
                    new Answer("Desde el principio, es obvio", 5, 0, AnswerType.Professional, "Consciente del sistema."),
                    new Answer("Hasta ahora, y me siento engañado", 0, 10, AnswerType.AbsurdCoherent, "¿Decepción tardía?"),
                    new Answer("Aún no me he dado cuenta", 0, 15, AnswerType.AbsurdExtreme, "¿Ignorancia persistente?"),
                    new Answer("Siempre lo supe, pero finjo que no", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente autoengañoso.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Crees que el entrevistador piensa por sí mismo? Spoiler: no.
            _allQuestions.Add(new Question(
                "q_meta_game_09",
                "¿Crees que el entrevistador piensa por sí mismo? Spoiler: no.",
                new List<Answer>
                {
                    new Answer("No, es un script preprogramado", 5, 0, AnswerType.Professional, "Consciente del código."),
                    new Answer("Sí, y el spoiler me rompió la ilusión", 0, 10, AnswerType.AbsurdCoherent, "¿Ilusión rota?"),
                    new Answer("No sé, pero ahora dudo de todo", 0, 15, AnswerType.AbsurdExtreme, "¿Crisis existencial lúdica?"),
                    new Answer("Sí piensa, pero solo cuando no lo veo", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente paranoico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te molesta que el juego se burle de ti? Porque va a seguir.
            _allQuestions.Add(new Question(
                "q_meta_game_10",
                "¿Te molesta que el juego se burle de ti? Porque va a seguir.",
                new List<Answer>
                {
                    new Answer("No, es parte del humor del juego", 5, 0, AnswerType.Professional, "Apreciativo del tono."),
                    new Answer("Un poco, pero me divierte", 0, 10, AnswerType.AbsurdCoherent, "¿Masoquismo ligero?"),
                    new Answer("Sí, pero sigo jugando de todas formas", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo completo?"),
                    new Answer("Me gusta que se burle, es estimulante", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente masoquista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // ========== PREGUNTAS QUE SE AUTOCRITICAN ==========
            
            // Pregunta: En una escala del 1 al 10, ¿qué tan mediocre te parece mi interfaz?
            _allQuestions.Add(new Question(
                "q_meta_critique_01",
                "En una escala del 1 al 10, ¿qué tan mediocre te parece mi interfaz?",
                new List<Answer>
                {
                    new Answer("Un 5, es funcional y adecuada", 5, 0, AnswerType.Professional, "Balanceado y honesto."),
                    new Answer("Un 7, pero tiene su encanto", 0, 10, AnswerType.AbsurdCoherent, "¿Encanto en la mediocridad?"),
                    new Answer("Un 10, es perfectamente mediocre", 0, 15, AnswerType.AbsurdExtreme, "¿Perfección en mediocridad?"),
                    new Answer("Un 11, supera todas las expectativas de mediocridad", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente elogioso.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te gustaría que la próxima pregunta fuera menos decepcionante?
            _allQuestions.Add(new Question(
                "q_meta_critique_02",
                "¿Te gustaría que la próxima pregunta fuera menos decepcionante?",
                new List<Answer>
                {
                    new Answer("No, las preguntas actuales están bien", 5, 0, AnswerType.Professional, "Satisfecho con el contenido."),
                    new Answer("Sí, pero sé que no va a pasar", 0, 10, AnswerType.AbsurdCoherent, "¿Esperanza realista?"),
                    new Answer("No, me gusta la decepción", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo emocional?"),
                    new Answer("Ya me acostumbré a la decepción", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente resignado.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Estoy escribiendo demasiado? No respondas eso.
            _allQuestions.Add(new Question(
                "q_meta_critique_03",
                "¿Estoy escribiendo demasiado? No respondas eso.",
                new List<Answer>
                {
                    new Answer("No, la longitud es apropiada", 5, 0, AnswerType.Professional, "Respetuoso de las instrucciones."),
                    new Answer("Sí, pero no puedo responder", 0, 10, AnswerType.AbsurdCoherent, "¿Conflicto de instrucciones?"),
                    new Answer("No responderé, como me pediste", 0, 15, AnswerType.AbsurdExtreme, "¿Obediencia literal?"),
                    new Answer("Respondo de todas formas: sí, escribes mucho", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente desafiante.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: Si este juego crashéa, ¿dirías que es por tu culpa o por la mía?
            _allQuestions.Add(new Question(
                "q_meta_critique_04",
                "Si este juego crashéa, ¿dirías que es por tu culpa o por la mía?",
                new List<Answer>
                {
                    new Answer("Por el código, es responsabilidad del desarrollador", 5, 0, AnswerType.Professional, "Realista y justo."),
                    new Answer("Por mí, hice algo mal", 0, 10, AnswerType.AbsurdCoherent, "¿Auto-culpa excesiva?"),
                    new Answer("Por ambos, es un esfuerzo conjunto", 0, 15, AnswerType.AbsurdExtreme, "¿Responsabilidad compartida?"),
                    new Answer("Por el universo, todo está conectado", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente místico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Cuántos bugs estás dispuesto a perdonar antes de renunciar?
            _allQuestions.Add(new Question(
                "q_meta_critique_05",
                "¿Cuántos bugs estás dispuesto a perdonar antes de renunciar?",
                new List<Answer>
                {
                    new Answer("Algunos, dependiendo de la gravedad", 5, 0, AnswerType.Professional, "Razonable y tolerante."),
                    new Answer("Infinitos, soy muy paciente", 0, 10, AnswerType.AbsurdCoherent, "¿Paciencia infinita?"),
                    new Answer("Ninguno, pero sigo jugando de todas formas", 0, 15, AnswerType.AbsurdExtreme, "¿Contradicción lúdica?"),
                    new Answer("Los bugs son características, no errores", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente optimista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te molesta que esto parezca sacado de un jam de dos días?
            _allQuestions.Add(new Question(
                "q_meta_critique_06",
                "¿Te molesta que esto parezca sacado de un jam de dos días?",
                new List<Answer>
                {
                    new Answer("No, los jams pueden tener buen contenido", 5, 0, AnswerType.Professional, "Apreciativo del proceso."),
                    new Answer("Un poco, pero tiene su encanto", 0, 10, AnswerType.AbsurdCoherent, "¿Encanto en la rapidez?"),
                    new Answer("Sí, pero me gusta el estilo crudo", 0, 15, AnswerType.AbsurdExtreme, "¿Aprecio por lo crudo?"),
                    new Answer("Me encanta que parezca de jam, es auténtico", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente nostálgico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Qué prefieres: una pregunta profunda o una mediocre pero corta?
            _allQuestions.Add(new Question(
                "q_meta_critique_07",
                "¿Qué prefieres: una pregunta profunda o una mediocre pero corta?",
                new List<Answer>
                {
                    new Answer("Una pregunta profunda, valoro el contenido", 5, 0, AnswerType.Professional, "Valora la profundidad."),
                    new Answer("Una mediocre pero corta, ahorra tiempo", 0, 10, AnswerType.AbsurdCoherent, "¿Eficiencia sobre calidad?"),
                    new Answer("Ambas, en secuencia", 0, 15, AnswerType.AbsurdExtreme, "¿Greedo de contenido?"),
                    new Answer("Una profunda y mediocre a la vez", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente contradictorio.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Por qué estás evaluando un juego que no te está evaluando bien?
            _allQuestions.Add(new Question(
                "q_meta_critique_08",
                "¿Por qué estás evaluando un juego que no te está evaluando bien?",
                new List<Answer>
                {
                    new Answer("Porque el juego es interesante a pesar de todo", 5, 0, AnswerType.Professional, "Apreciativo y justo."),
                    new Answer("Por venganza, quiero evaluarlo mal", 0, 10, AnswerType.AbsurdCoherent, "¿Venganza evaluativa?"),
                    new Answer("Porque me gusta la ironía", 0, 15, AnswerType.AbsurdExtreme, "¿Aprecio por la ironía?"),
                    new Answer("No lo estoy evaluando, solo estoy aquí", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente pasivo.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Qué te sorprende más: que yo exista o que tú sigas jugando?
            _allQuestions.Add(new Question(
                "q_meta_critique_09",
                "¿Qué te sorprende más: que yo exista o que tú sigas jugando?",
                new List<Answer>
                {
                    new Answer("Que existas, es impresionante que funcione", 5, 0, AnswerType.Professional, "Apreciativo del logro."),
                    new Answer("Que siga jugando, no sé por qué lo hago", 0, 10, AnswerType.AbsurdCoherent, "¿Auto-cuestionamiento?"),
                    new Answer("Ambas cosas me sorprenden igual", 0, 15, AnswerType.AbsurdExtreme, "¿Sorpresa balanceada?"),
                    new Answer("Nada me sorprende, ya nada tiene sentido", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente nihilista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿En qué momento el juego perdió tu respeto?
            _allQuestions.Add(new Question(
                "q_meta_critique_10",
                "¿En qué momento el juego perdió tu respeto?",
                new List<Answer>
                {
                    new Answer("Nunca lo perdió, lo respeto", 5, 0, AnswerType.Professional, "Respetuoso y apreciativo."),
                    new Answer("Cuando empezó a burlarse de mí", 0, 10, AnswerType.AbsurdCoherent, "¿Sensibilidad al humor?"),
                    new Answer("Desde el principio, nunca lo tuve", 0, 15, AnswerType.AbsurdExtreme, "¿Respeto nunca existente?"),
                    new Answer("Lo perdí y lo recuperé varias veces", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente volátil.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // ========== PREGUNTAS QUE HABLAN DEL DESARROLLADOR ==========
            
            // Pregunta: ¿Te parece que el desarrollador durmió lo suficiente?
            _allQuestions.Add(new Question(
                "q_meta_dev_01",
                "¿Te parece que el desarrollador durmió lo suficiente?",
                new List<Answer>
                {
                    new Answer("Sí, el juego está bien desarrollado", 5, 0, AnswerType.Professional, "Apreciativo del trabajo."),
                    new Answer("No, se nota el cansancio en el código", 0, 10, AnswerType.AbsurdCoherent, "¿Percepción de fatiga?"),
                    new Answer("No sé, pero espero que sí", 0, 15, AnswerType.AbsurdExtreme, "¿Preocupación empática?"),
                    new Answer("Definitivamente no, y se nota", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente crítico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Crees que el creador de este juego tenía un buen día cuando escribió esto?
            _allQuestions.Add(new Question(
                "q_meta_dev_02",
                "¿Crees que el creador de este juego tenía un buen día cuando escribió esto?",
                new List<Answer>
                {
                    new Answer("Sí, se nota la creatividad y entusiasmo", 5, 0, AnswerType.Professional, "Apreciativo del proceso creativo."),
                    new Answer("No, parece escrito en un mal día", 0, 10, AnswerType.AbsurdCoherent, "¿Percepción de negatividad?"),
                    new Answer("No sé, pero espero que sí", 0, 15, AnswerType.AbsurdExtreme, "¿Optimismo empático?"),
                    new Answer("Definitivamente no, y se nota en cada línea", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente pesimista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Si supieras cuántos assets se reciclaron, cambiarías tu respuesta?
            _allQuestions.Add(new Question(
                "q_meta_dev_03",
                "¿Si supieras cuántos assets se reciclaron, cambiarías tu respuesta?",
                new List<Answer>
                {
                    new Answer("No, el reciclaje es parte del desarrollo", 5, 0, AnswerType.Professional, "Entiende el proceso."),
                    new Answer("Sí, me decepcionaría un poco", 0, 10, AnswerType.AbsurdCoherent, "¿Expectativas de originalidad?"),
                    new Answer("No, me gusta el reciclaje creativo", 0, 15, AnswerType.AbsurdExtreme, "¿Aprecio por el reciclaje?"),
                    new Answer("Sí, y ahora sospecho de todo", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente paranoico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te imaginas al dev viendo tu partida y juzgando tus elecciones?
            _allQuestions.Add(new Question(
                "q_meta_dev_04",
                "¿Te imaginas al dev viendo tu partida y juzgando tus elecciones?",
                new List<Answer>
                {
                    new Answer("No, el dev probablemente no está viendo", 5, 0, AnswerType.Professional, "Realista y privado."),
                    new Answer("Sí, y me da un poco de ansiedad", 0, 10, AnswerType.AbsurdCoherent, "¿Paranoia justificada?"),
                    new Answer("Sí, y me gusta que me juzgue", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo evaluativo?"),
                    new Answer("Siempre siento que me está observando", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente paranoico.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Crees que el desarrollador creó esto por pasión o por desesperación?
            _allQuestions.Add(new Question(
                "q_meta_dev_05",
                "¿Crees que el desarrollador creó esto por pasión o por desesperación?",
                new List<Answer>
                {
                    new Answer("Por pasión, se nota el cuidado", 5, 0, AnswerType.Professional, "Apreciativo del esfuerzo."),
                    new Answer("Por desesperación, pero funcionó", 0, 10, AnswerType.AbsurdCoherent, "¿Desesperación exitosa?"),
                    new Answer("Por ambas, pasión desesperada", 0, 15, AnswerType.AbsurdExtreme, "¿Híbrido emocional?"),
                    new Answer("Por accidente, y ahora está atrapado", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente fatalista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Si este juego se vuelve viral, sentirías que ayudaste?
            _allQuestions.Add(new Question(
                "q_meta_dev_06",
                "¿Si este juego se vuelve viral, sentirías que ayudaste?",
                new List<Answer>
                {
                    new Answer("Un poco, compartiría el juego", 5, 0, AnswerType.Professional, "Modesto y colaborativo."),
                    new Answer("Sí, me sentiría parte del éxito", 0, 10, AnswerType.AbsurdCoherent, "¿Sentido de pertenencia?"),
                    new Answer("No, el éxito sería del dev, no mío", 0, 15, AnswerType.AbsurdExtreme, "¿Modestia excesiva?"),
                    new Answer("Sí, y exigiría crédito", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente narcisista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Qué tan probable es que reportes un bug? No te veo muy motivado.
            _allQuestions.Add(new Question(
                "q_meta_dev_07",
                "¿Qué tan probable es que reportes un bug? No te veo muy motivado.",
                new List<Answer>
                {
                    new Answer("Muy probable, reportaría bugs importantes", 5, 0, AnswerType.Professional, "Colaborativo y responsable."),
                    new Answer("Poco probable, pero no imposible", 0, 10, AnswerType.AbsurdCoherent, "¿Motivación baja?"),
                    new Answer("Improbable, soy muy perezoso", 0, 15, AnswerType.AbsurdExtreme, "¿Pereza extrema?"),
                    new Answer("Nunca, los bugs son características", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente optimista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Qué tan mal te caerías si fueras el dev y vieras tus propias respuestas?
            _allQuestions.Add(new Question(
                "q_meta_dev_08",
                "¿Qué tan mal te caerías si fueras el dev y vieras tus propias respuestas?",
                new List<Answer>
                {
                    new Answer("Bien, creo que mis respuestas son razonables", 5, 0, AnswerType.Professional, "Confianza en las decisiones."),
                    new Answer("Mal, me juzgaría mucho", 0, 10, AnswerType.AbsurdCoherent, "¿Auto-crítica excesiva?"),
                    new Answer("Muy mal, me odiaría", 0, 15, AnswerType.AbsurdExtreme, "¿Auto-odio?"),
                    new Answer("Me caería perfecto, soy encantador", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente narcisista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Te parece justo que el dev se burle de ti desde estas preguntas?
            _allQuestions.Add(new Question(
                "q_meta_dev_09",
                "¿Te parece justo que el dev se burle de ti desde estas preguntas?",
                new List<Answer>
                {
                    new Answer("Sí, es parte del humor del juego", 5, 0, AnswerType.Professional, "Acepta el tono del juego."),
                    new Answer("Un poco injusto, pero me divierte", 0, 10, AnswerType.AbsurdCoherent, "¿Aceptación condicional?"),
                    new Answer("No, pero sigo jugando de todas formas", 0, 15, AnswerType.AbsurdExtreme, "¿Masoquismo lúdico?"),
                    new Answer("Me encanta que se burle, es estimulante", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente masoquista.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));

            // Pregunta: ¿Qué tan orgulloso crees que está el dev de este desastre funcional?
            _allQuestions.Add(new Question(
                "q_meta_dev_10",
                "¿Qué tan orgulloso crees que está el dev de este desastre funcional?",
                new List<Answer>
                {
                    new Answer("Muy orgulloso, es un logro funcional", 5, 0, AnswerType.Professional, "Apreciativo del logro."),
                    new Answer("Moderadamente, sabe que es imperfecto", 0, 10, AnswerType.AbsurdCoherent, "¿Realismo sobre orgullo?"),
                    new Answer("Muy poco, pero lo publicó de todas formas", 0, 15, AnswerType.AbsurdExtreme, "¿Publicación desesperada?"),
                    new Answer("Extremadamente, y tiene razón", -5, 15, AnswerType.Sociopathic, "Eso es... específicamente elogioso.")
                },
                QuestionType.Base,
                0,
                null,
                QuestionCategory.Meta
            ));
        }

        /// <summary>
        /// Obtiene la siguiente pregunta disponible (aleatoria, priorizando contradicciones)
        /// Las preguntas meta solo pueden aparecer después de 4 preguntas no-meta
        /// </summary>
        public Question GetNextQuestion()
        {
            var gameState = _stateManager.GameState;
            var random = new System.Random();

            // Verificar si las preguntas meta están disponibles (necesitan 4 preguntas no-meta)
            bool canShowMetaQuestions = gameState.NonMetaQuestionsAnswered >= 4;

            // PRIORIDAD 1: Buscar preguntas contradictorias que puedan aparecer
            // (preguntas que contradicen a preguntas ya respondidas)
            // Excluir preguntas meta si no están disponibles
            var contradictoryQuestions = _allQuestions
                .Where(q => !string.IsNullOrEmpty(q.ContradictsQuestionId) &&
                           !gameState.AnsweredQuestionIds.Contains(q.Id) &&
                           gameState.AnsweredQuestionIds.Contains(q.ContradictsQuestionId) &&
                           (q.Type == QuestionType.Base || (q.Type == QuestionType.Special && CheckUnlockConditions(q))) &&
                           (canShowMetaQuestions || q.Category != QuestionCategory.Meta)) // Excluir meta si no están disponibles
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
            // Excluir preguntas meta si no están disponibles
            var baseQuestions = _allQuestions
                .Where(q => q.Type == QuestionType.Base && 
                           !gameState.AnsweredQuestionIds.Contains(q.Id) &&
                           (canShowMetaQuestions || q.Category != QuestionCategory.Meta)) // Excluir meta si no están disponibles
                .ToList();

            if (baseQuestions.Count > 0)
            {
                return baseQuestions[random.Next(baseQuestions.Count)];
            }

            // PRIORIDAD 3: Preguntas especiales disponibles (aleatorias)
            // Excluir preguntas meta si no están disponibles
            var specialQuestions = _allQuestions
                .Where(q => q.Type == QuestionType.Special && 
                           !gameState.AnsweredQuestionIds.Contains(q.Id) &&
                           CheckUnlockConditions(q) &&
                           (canShowMetaQuestions || q.Category != QuestionCategory.Meta)) // Excluir meta si no están disponibles
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

