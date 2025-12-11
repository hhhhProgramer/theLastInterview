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
                new OfficeEvent("event_01", "Un empleado pasa corriendo con un extintor. Nadie comenta nada."),
                new OfficeEvent("event_02", "Se escucha a alguien gritar: '¡Deja mi lonche!' y luego un golpe seco."),
                new OfficeEvent("event_03", "Un gerente aparece, te mira como si te conociera, se va sin decir nada."),
                new OfficeEvent("event_04", "El entrevistador recibe un mensaje, sonríe demasiado y finge que nada pasó."),
                new OfficeEvent("event_05", "Una planta de oficina se cae sola. Todos la ignoran."),
                new OfficeEvent("event_06", "Se escucha un 'ping' del sistema. Nadie sabe de dónde vino."),
                new OfficeEvent("event_07", "Alguien pasa con una cafetera llena. El olor a café quemado llena el aire."),
                new OfficeEvent("event_08", "Un teléfono suena en otra oficina. Nadie lo contesta. Sigue sonando."),
                new OfficeEvent("event_09", "El aire acondicionado hace un ruido extraño. Todos lo ignoran."),
                new OfficeEvent("event_10", "Se escucha un 'click' de una pluma. Alguien está escribiendo muy fuerte.")
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

