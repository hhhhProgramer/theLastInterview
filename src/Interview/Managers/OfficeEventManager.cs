using Godot;
using TheLastInterview.Interview.Models;
using System.Collections.Generic;
using System.Linq;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager que maneja los eventos "Meta-Oficina" aleatorios
    /// </summary>
    public class OfficeEventManager
    {
        private static System.Random _random = new System.Random();
        private static List<OfficeEvent> _events = new List<OfficeEvent>();

        static OfficeEventManager()
        {
            InitializeEvents();
        }

        /// <summary>
        /// Inicializa la lista de eventos disponibles
        /// </summary>
        private static void InitializeEvents()
        {
            _events = new List<OfficeEvent>
            {
                new OfficeEvent("event_01", "Un empleado pasa corriendo con un extintor. No se detiene."),
                new OfficeEvent("event_02", "Se escucha un grito distante: \"¡Eso NO es mi tupper!\"."),
                new OfficeEvent("event_03", "Una impresora empieza a imprimir hojas en blanco frenéticamente."),
                new OfficeEvent("event_04", "El foco sobre tu cabeza parpadea… luego se queda prendido de forma sospechosa."),
                new OfficeEvent("event_05", "Un gato aparece en la puerta, mira a todos como juez divino, luego se va."),
                new OfficeEvent("event_06", "El entrevistador recibe un mensaje, sonríe de forma incómoda."),
                new OfficeEvent("event_07", "Alguien tose exageradamente en otra sala. Nadie comenta nada."),
                new OfficeEvent("event_08", "Un gerente asoma la cabeza, pregunta \"¿todo bien?\" sin intención real de saber la respuesta."),
                new OfficeEvent("event_09", "Se escucha algo que parece una explosión leve. Ojalá haya sido un microondas."),
                new OfficeEvent("event_10", "Una silla chirría como si gritara por ayuda… aunque nadie la está usando."),
                new OfficeEvent("event_11", "Un empleado pasa corriendo con un extintor. Nadie comenta nada."),
                new OfficeEvent("event_12", "Se escucha a alguien gritar: '¡Deja mi lonche!' y luego un golpe seco."),
                new OfficeEvent("event_13", "Un gerente aparece, te mira como si te conociera, se va sin decir nada."),
                new OfficeEvent("event_14", "El entrevistador recibe un mensaje, sonríe demasiado y finge que nada pasó."),
                new OfficeEvent("event_15", "Una planta de oficina se cae sola. Todos la ignoran.")
            };
        }

        /// <summary>
        /// Obtiene un evento aleatorio
        /// </summary>
        public static OfficeEvent GetRandomEvent()
        {
            if (_events.Count == 0)
            {
                return null;
            }
            return _events[_random.Next(_events.Count)];
        }
    }
}

