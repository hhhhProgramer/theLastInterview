using Godot;
using TheLastInterview.Interview.Models;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager que maneja las interrupciones incómodas durante la entrevista
    /// </summary>
    public class OfficeInterruptionManager
    {
        private static System.Random _random = new System.Random();
        private static List<OfficeInterruption> _interruptions = new List<OfficeInterruption>();

        static OfficeInterruptionManager()
        {
            InitializeInterruptions();
        }

        /// <summary>
        /// Inicializa la lista de interrupciones disponibles
        /// </summary>
        private static void InitializeInterruptions()
        {
            _interruptions = new List<OfficeInterruption>
            {
                // Pop-ups simulados
                new OfficeInterruption("interrupt_01", "Pop-up: \"Advertencia: tu seguridad laboral está en riesgo.\"", "Pop-up"),
                new OfficeInterruption("interrupt_02", "Pop-up: \"Fallo crítico: honestidad detectada.\"", "Pop-up"),
                new OfficeInterruption("interrupt_03", "Pop-up: \"Sistema del entrevistador: requiere actualización emocional.\"", "Pop-up"),
                new OfficeInterruption("interrupt_04", "Pop-up: \"Su respuesta fue enviada a Recursos Humanos para análisis forense.\"", "Pop-up"),
                new OfficeInterruption("interrupt_05", "Advertencia: tu CV contiene demasiadas mentiras.", "Pop-up"),
                
                // Interrupciones del entrevistador
                new OfficeInterruption("interrupt_06", "Perdón, ¿puedes repetir? Estaba revisando memes.", "Entrevistador"),
                new OfficeInterruption("interrupt_07", "¿Puedes hablar más lento? Soy pésimo fingiendo interés.", "Entrevistador"),
                new OfficeInterruption("interrupt_08", "Esa respuesta… ¿es en serio?", "Entrevistador"),
                new OfficeInterruption("interrupt_09", "No entiendo tu CV. Aunque para ser sincero, tampoco leí el de los demás.", "Entrevistador"),
                new OfficeInterruption("interrupt_10", "Voy a fingir que no escuché eso. Por el bien de ambos.", "Entrevistador"),
                
                // Interrupciones del entorno
                new OfficeInterruption("interrupt_11", "Tu silla hace un ruido extraño. Todo el mundo mira.", "Entorno"),
                new OfficeInterruption("interrupt_12", "Suena el teléfono. Nadie lo contesta.", "Entorno"),
                new OfficeInterruption("interrupt_13", "El entrevistador estornuda; su computadora crashea al mismo tiempo.", "Entorno"),
                new OfficeInterruption("interrupt_14", "La puerta se abre sola… probablemente el aire.", "Entorno"),
                new OfficeInterruption("interrupt_15", "La luz se apaga y vuelve. No pregunten.", "Entorno"),
                
                // Interrupciones del sistema
                new OfficeInterruption("interrupt_16", "Error. El entrevistador ha dejado de funcionar.", "Sistema"),
                new OfficeInterruption("interrupt_17", "Sistema: El entrevistador necesita actualizarse. ¿Desea continuar?", "Sistema"),
                new OfficeInterruption("interrupt_18", "Alerta: Se detectó una sonrisa genuina. Procediendo con protocolo anti-felicidad.", "Sistema"),
                new OfficeInterruption("interrupt_19", "Mamá del entrevistador: ¿sí le preguntaste si toma mucho?", "Llamada")
            };
        }

        /// <summary>
        /// Obtiene una interrupción aleatoria
        /// </summary>
        public static OfficeInterruption GetRandomInterruption()
        {
            if (_interruptions.Count == 0)
            {
                return null;
            }
            return _interruptions[_random.Next(_interruptions.Count)];
        }
    }
}

