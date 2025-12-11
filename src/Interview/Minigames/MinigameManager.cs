using Godot;
using System;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Manager que controla los minijuegos tontos entre preguntas
    /// </summary>
    public class MinigameManager
    {
        private static System.Random _random = new System.Random();
        
        /// <summary>
        /// Tipos de minijuegos disponibles
        /// </summary>
        public enum MinigameType
        {
            LieDetector,        // Detector de mentiras descompuesto
            TypeName,           // Escribe tu nombre con teclado aleatorio
            OrganizeDocuments,  // Ordena documentos (comentarios aleatorios)
            TechnicalTest,      // Prueba técnica falsa
            StayCalm            // Mantén la calma (barra de estrés)
        }
        
        /// <summary>
        /// Obtiene un minijuego aleatorio
        /// </summary>
        public static MinigameType GetRandomMinigame()
        {
            var values = Enum.GetValues(typeof(MinigameType));
            return (MinigameType)values.GetValue(_random.Next(values.Length));
        }
        
        /// <summary>
        /// Crea una instancia del minijuego especificado
        /// </summary>
        public static BaseMinigame CreateMinigame(MinigameType type, Node parent)
        {
            return type switch
            {
                MinigameType.LieDetector => new LieDetectorMinigame(parent),
                MinigameType.TypeName => new TypeNameMinigame(parent),
                MinigameType.OrganizeDocuments => new OrganizeDocumentsMinigame(parent),
                MinigameType.TechnicalTest => new TechnicalTestMinigame(parent),
                MinigameType.StayCalm => new StayCalmMinigame(parent),
                _ => throw new ArgumentException($"Tipo de minijuego desconocido: {type}")
            };
        }
    }
}

