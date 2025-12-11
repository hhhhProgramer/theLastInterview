using System.Collections.Generic;
using System.Linq;
using TheLastInterview.Interview.Models;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager que determina y gestiona los endings del juego
    /// </summary>
    public class EndingManager
    {
        private List<Ending> _endings;

        /// <summary>
        /// Constructor
        /// </summary>
        public EndingManager()
        {
            LoadEndings();
        }

        /// <summary>
        /// Carga todos los endings del juego
        /// </summary>
        private void LoadEndings()
        {
            _endings = new List<Ending>();

            // Ending 1: Contratado por Error
            _endings.Add(new Ending(
                "hired_by_mistake",
                "Contratado por Error",
                "Después de revisar tus respuestas completamente absurdas, el comité decidió que eres 'perfecto para nuestra cultura corporativa'. Has sido contratado. Felicidades, supongo.",
                new EndingCondition
                {
                    MinTotalPoints = 70,
                    MaxTotalPoints = 150,
                    PredominantAnswerType = AnswerType.AbsurdCoherent
                }
            ));

            // Ending 2: Despedido Antes de Empezar
            _endings.Add(new Ending(
                "fired_before_start",
                "Despedido Antes de Empezar",
                "Una de tus respuestas hizo que el entrevistador renunciara en el acto. Has sido despedido antes de ser contratado. Logro desbloqueado.",
                new EndingCondition
                {
                    MinTotalPoints = 70,
                    MaxTotalPoints = 150,
                    PredominantAnswerType = AnswerType.Aggressive
                }
            ));

            // Ending 3: Contratado como Jefe
            _endings.Add(new Ending(
                "hired_as_boss",
                "Contratado como Jefe",
                "El entrevistador se rindió y te nombró su jefe. Ahora eres el nuevo entrevistador. El ciclo continúa.",
                new EndingCondition
                {
                    MinTotalPoints = 50,
                    MaxTotalPoints = 90,
                    RequiredState = InterviewState.Chaos,
                    PredominantAnswerType = AnswerType.AbsurdExtreme
                }
            ));

            // Ending 4: HR Simulation Found Corrupted
            _endings.Add(new Ending(
                "hr_simulation_corrupted",
                "HR Simulation Found Corrupted",
                "El sistema detectó una corrupción en la simulación de HR. Has sido contratado como el nuevo entrevistador. Bienvenido al loop infinito.",
                new EndingCondition
                {
                    MinTotalPoints = 0,
                    MaxTotalPoints = 100,
                    RequiredAnswerIds = new List<string> { "Me convertiría en tu programador", "Soy un bot, pero bien programado" }
                }
            ));

            // Ending 5: Final Zen
            _endings.Add(new Ending(
                "zen_ending",
                "Final Zen",
                "Tus respuestas tan relajadas hicieron que crean que eres un gurú corporativo. Has sido contratado como consultor de bienestar. Ironías del destino.",
                new EndingCondition
                {
                    MinTotalPoints = 0,
                    MaxTotalPoints = 40,
                    PredominantAnswerType = AnswerType.Zen
                }
            ));

            // Ending 6: Expulsado Violentamente
            _endings.Add(new Ending(
                "violently_expelled",
                "Expulsado Violentamente",
                "Tu última respuesta fue tan extrema que el entrevistador llamó a seguridad. Has sido expulsado del edificio. Al menos fue memorable.",
                new EndingCondition
                {
                    MinTotalPoints = 70,
                    MaxTotalPoints = 150,
                    PredominantAnswerType = AnswerType.Sociopathic
                }
            ));

            // Ending 7: Normal (Genérico)
            _endings.Add(new Ending(
                "normal_rejection",
                "Rechazado",
                "Después de una entrevista... interesante, el comité decidió que no eres el candidato adecuado. Gracias por tu tiempo.",
                new EndingCondition
                {
                    MinTotalPoints = 0,
                    MaxTotalPoints = 40,
                    RequiredState = InterviewState.Normal
                }
            ));
        }

        /// <summary>
        /// Determina el ending basado en el estado del juego
        /// </summary>
        public Ending DetermineEnding(GameState gameState)
        {
            // Ordenar endings por prioridad (más específicos primero)
            var sortedEndings = _endings.OrderByDescending(e => GetEndingPriority(e));

            foreach (var ending in sortedEndings)
            {
                if (CheckEndingCondition(ending.Condition, gameState))
                {
                    return ending;
                }
            }

            // Fallback: ending normal
            return _endings.FirstOrDefault(e => e.Id == "normal_rejection") ?? _endings[0];
        }

        /// <summary>
        /// Calcula la prioridad de un ending (más específico = mayor prioridad)
        /// </summary>
        private int GetEndingPriority(Ending ending)
        {
            int priority = 0;
            var condition = ending.Condition;

            if (condition.RequiredAnswerIds != null && condition.RequiredAnswerIds.Count > 0)
                priority += 100; // Respuestas específicas son muy prioritarias

            if (condition.PredominantAnswerType.HasValue)
                priority += 50; // Tipo de respuesta predominante

            if (condition.RequiredState.HasValue)
                priority += 30; // Estado específico

            if (condition.MinTotalPoints.HasValue || condition.MaxTotalPoints.HasValue)
                priority += 20; // Rango de puntos

            return priority;
        }

        /// <summary>
        /// Verifica si se cumple la condición de un ending
        /// </summary>
        private bool CheckEndingCondition(EndingCondition condition, GameState gameState)
        {
            // Verificar puntos totales
            if (condition.MinTotalPoints.HasValue && gameState.TotalPoints < condition.MinTotalPoints.Value)
                return false;

            if (condition.MaxTotalPoints.HasValue && gameState.TotalPoints > condition.MaxTotalPoints.Value)
                return false;

            // Verificar puntos Normal
            if (condition.MinNormalPoints.HasValue && gameState.NormalPoints < condition.MinNormalPoints.Value)
                return false;

            if (condition.MaxNormalPoints.HasValue && gameState.NormalPoints > condition.MaxNormalPoints.Value)
                return false;

            // Verificar puntos Caos
            if (condition.MinChaosPoints.HasValue && gameState.ChaosPoints < condition.MinChaosPoints.Value)
                return false;

            if (condition.MaxChaosPoints.HasValue && gameState.ChaosPoints > condition.MaxChaosPoints.Value)
                return false;

            // Verificar estado
            if (condition.RequiredState.HasValue && gameState.CurrentState != condition.RequiredState.Value)
                return false;

            // Verificar respuestas específicas en el historial
            if (condition.RequiredAnswerIds != null && condition.RequiredAnswerIds.Count > 0)
            {
                bool hasAllAnswers = condition.RequiredAnswerIds.All(id => 
                    gameState.AnswerHistory.Any(answer => answer.Contains(id)));
                if (!hasAllAnswers)
                    return false;
            }

            // Verificar tipo de respuesta predominante
            if (condition.PredominantAnswerType.HasValue)
            {
                if (gameState.AnswerTypeHistory == null || gameState.AnswerTypeHistory.Count == 0)
                    return false;
                
                // Contar cuántas respuestas de cada tipo hay
                var typeCounts = new System.Collections.Generic.Dictionary<AnswerType, int>();
                foreach (var type in gameState.AnswerTypeHistory)
                {
                    if (!typeCounts.ContainsKey(type))
                        typeCounts[type] = 0;
                    typeCounts[type]++;
                }
                
                // Verificar si el tipo requerido es el más común (o al menos el 40% de las respuestas)
                var requiredType = condition.PredominantAnswerType.Value;
                if (!typeCounts.ContainsKey(requiredType))
                    return false;
                
                int requiredCount = typeCounts[requiredType];
                int totalAnswers = gameState.AnswerTypeHistory.Count;
                
                // El tipo debe ser al menos el 40% de las respuestas para ser "predominante"
                if (requiredCount < totalAnswers * 0.4f)
                    return false;
                
                // Además, debe ser el tipo más común o estar muy cerca
                int maxCount = typeCounts.Values.Max();
                if (requiredCount < maxCount * 0.8f)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Obtiene un ending por ID
        /// </summary>
        public Ending GetEndingById(string id)
        {
            return _endings.FirstOrDefault(e => e.Id == id);
        }
    }
}

