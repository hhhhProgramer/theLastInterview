using Godot;
using TheLastInterview.Interview.Models;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager que maneja los rumores de la oficina
    /// </summary>
    public class OfficeRumorManager
    {
        private static System.Random _random = new System.Random();
        private static List<OfficeRumor> _rumors = new List<OfficeRumor>();

        static OfficeRumorManager()
        {
            InitializeRumors();
        }

        /// <summary>
        /// Inicializa la lista de rumores disponibles
        /// </summary>
        private static void InitializeRumors()
        {
            _rumors = new List<OfficeRumor>
            {
                new OfficeRumor("rumor_01", "Dicen que el entrevistador despidió a alguien por pestañear demasiado."),
                new OfficeRumor("rumor_02", "Se rumora que hoy están de malas porque perdió en el fantasy."),
                new OfficeRumor("rumor_03", "Cuentan que la empresa usa sillas diferentes para saber quién miente."),
                new OfficeRumor("rumor_04", "Se dice que el último candidato desapareció después de la pregunta 5."),
                new OfficeRumor("rumor_05", "Rumor: El entrevistador tiene un gemelo idéntico que también entrevista."),
                new OfficeRumor("rumor_06", "Cuentan que aquí las plantas mueren de aburrimiento, no de falta de agua."),
                new OfficeRumor("rumor_07", "Se rumora que el aire acondicionado está programado para crear incomodidad."),
                new OfficeRumor("rumor_08", "Dicen que el monitor CRT muestra mensajes del futuro."),
                new OfficeRumor("rumor_09", "Rumor: El entrevistador anterior renunció porque las preguntas eran demasiado normales."),
                new OfficeRumor("rumor_10", "Se dice que la oficina tiene más cámaras que una prisión de máxima seguridad.")
            };
        }

        /// <summary>
        /// Obtiene un rumor aleatorio para la partida
        /// </summary>
        public static OfficeRumor GetRandomRumor()
        {
            if (_rumors.Count == 0)
            {
                return null;
            }
            return _rumors[_random.Next(_rumors.Count)];
        }
    }
}

