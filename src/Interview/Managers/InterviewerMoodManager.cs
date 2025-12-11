using Godot;
using Package.Core.Enums;
using TheLastInterview.Interview.Models;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager que maneja el estado emocional (humor) del entrevistador
    /// </summary>
    public class InterviewerMoodManager
    {
        /// <summary>
        /// Estados emocionales del entrevistador
        /// </summary>
        public enum InterviewerMood
        {
            Neutral,   // Estado neutral (actual)
            Happy,      // Feliz - habla bonito
            Angry      // Molesto - habla feo
        }

        /// <summary>
        /// Determina el estado emocional del entrevistador basado en el estado del juego
        /// </summary>
        public static InterviewerMood DetermineMood(GameState gameState)
        {
            // Basado en el estado de la entrevista y los puntos
            switch (gameState.CurrentState)
            {
                case InterviewState.Normal:
                    // Si tiene más puntos Normal que Caos, está feliz
                    if (gameState.NormalPoints > gameState.ChaosPoints + 10)
                    {
                        return InterviewerMood.Happy;
                    }
                    // Si tiene más puntos Caos, está molesto
                    else if (gameState.ChaosPoints > gameState.NormalPoints + 10)
                    {
                        return InterviewerMood.Angry;
                    }
                    // Por defecto, neutral
                    return InterviewerMood.Neutral;

                case InterviewState.Tense:
                    // En estado tenso, generalmente está molesto
                    return InterviewerMood.Angry;

                case InterviewState.Chaos:
                    // En caos, definitivamente está molesto
                    return InterviewerMood.Angry;

                default:
                    return InterviewerMood.Neutral;
            }
        }

        /// <summary>
        /// Obtiene la emoción correspondiente al estado emocional
        /// </summary>
        public static Emotion? GetEmotion(InterviewerMood mood)
        {
            return mood switch
            {
                InterviewerMood.Happy => Emotion.Happy,
                InterviewerMood.Angry => Emotion.Angry,
                InterviewerMood.Neutral => Emotion.Neutral,
                _ => Emotion.Neutral
            };
        }

        /// <summary>
        /// Modifica un texto según el estado emocional del entrevistador
        /// </summary>
        public static string ModifyTextByMood(string originalText, InterviewerMood mood)
        {
            if (string.IsNullOrEmpty(originalText))
                return originalText;

            return mood switch
            {
                InterviewerMood.Happy => MakeTextHappy(originalText),
                InterviewerMood.Angry => MakeTextAngry(originalText),
                InterviewerMood.Neutral => originalText, // Sin cambios
                _ => originalText
            };
        }

        /// <summary>
        /// Hace que un texto suene más feliz/amigable
        /// </summary>
        private static string MakeTextHappy(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Reemplazar palabras negativas con positivas
            text = text.Replace("preocupante", "interesante");
            text = text.Replace("perturbador", "único");
            text = text.Replace("cuestionable", "diferente");
            text = text.Replace("específicamente", "definitivamente");
            text = text.Replace("Eso es...", "¡Eso es");
            text = text.Replace("Técnicamente correcto", "¡Perfecto! Técnicamente correcto");
            text = text.Replace("Técnicamente", "¡Excelente! Técnicamente");
            
            // Agregar prefijos positivos si no tiene uno
            if (!text.StartsWith("¡") && !text.Contains("Excelente") && !text.Contains("Genial") && !text.Contains("Perfecto"))
            {
                if (text.Length < 60) // Solo para textos cortos
                {
                    if (text.StartsWith("Técnicamente") || text.StartsWith("Eso es"))
                    {
                        // Ya se reemplazó arriba
                    }
                    else
                    {
                        text = "¡Muy bien! " + text;
                    }
                }
            }

            // Cambiar puntos suspensivos por exclamaciones
            text = text.Replace("...", "!");
            text = text.Replace("..", "!");
            
            // Asegurar que termine con signo positivo
            if (text.EndsWith(".") && !text.EndsWith("..."))
            {
                text = text.TrimEnd('.') + "!";
            }
            else if (!text.EndsWith("!") && !text.EndsWith("?") && !text.EndsWith("..."))
            {
                text = text.TrimEnd('.') + "!";
            }

            return text;
        }

        /// <summary>
        /// Hace que un texto suene más molesto/agresivo
        /// </summary>
        private static string MakeTextAngry(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            // Reemplazar palabras positivas con negativas o neutras
            text = text.Replace("¡Excelente!", "Bien");
            text = text.Replace("Excelente", "Aceptable");
            text = text.Replace("¡Perfecto!", "Bien");
            text = text.Replace("Perfecto", "Aceptable");
            text = text.Replace("Genial", "Aceptable");
            text = text.Replace("interesante", "preocupante");
            text = text.Replace("único", "perturbador");
            text = text.Replace("diferente", "cuestionable");
            text = text.Replace("Técnicamente correcto", "Técnicamente... aceptable");
            text = text.Replace("¡Muy bien!", "Mmm...");
            text = text.Replace("Muy bien", "Bien");
            
            // Agregar prefijos de duda/irritación
            if (!text.StartsWith("¿") && !text.StartsWith("Mmm") && !text.Contains("..."))
            {
                if (text.Length < 60) // Solo para textos cortos
                {
                    if (!text.StartsWith("Técnicamente") && !text.StartsWith("Eso es"))
                    {
                        text = "Mmm... " + text;
                    }
                }
            }

            // Cambiar exclamaciones por puntos suspensivos o puntos
            text = text.Replace("!", ".");
            
            // Agregar puntos suspensivos para mostrar duda/irritación
            if (!text.Contains("...") && text.Length < 70)
            {
                if (text.EndsWith(".") && !text.EndsWith("..."))
                {
                    text = text.TrimEnd('.') + "...";
                }
                else if (!text.EndsWith(".") && !text.EndsWith("?") && !text.EndsWith("..."))
                {
                    text = text + "...";
                }
            }

            return text;
        }
    }
}

