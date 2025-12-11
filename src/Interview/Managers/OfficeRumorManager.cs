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
                new OfficeRumor("rumor_01", "Dicen que el entrevistador ya despidió a alguien hoy. Antes del desayuno."),
                new OfficeRumor("rumor_02", "Se rumora que las entrevistas de la tarde tienen más probabilidades de fracaso."),
                new OfficeRumor("rumor_03", "Alguien vio una lista negra con nombres tachados. O quizá era la lista del súper."),
                new OfficeRumor("rumor_04", "Cuentan que Recursos Humanos funciona como secta. No está confirmado."),
                new OfficeRumor("rumor_05", "Hay quien dice que la empresa evalúa a la gente según cómo toma asiento."),
                new OfficeRumor("rumor_06", "Se dice que el café está adulterado con ansiedad extra."),
                new OfficeRumor("rumor_07", "Un empleado afirmó haber visto a un clon del entrevistador."),
                new OfficeRumor("rumor_08", "Circula el rumor de que nadie ha visto al CEO en años… pero manda correos a diario."),
                new OfficeRumor("rumor_09", "Se habla de un \"cuarto prohibido\" lleno de CVs rechazados."),
                new OfficeRumor("rumor_10", "Se comenta que hoy los candidatos están cayendo como moscas."),
                new OfficeRumor("rumor_11", "Dicen que el entrevistador despidió a alguien por pestañear demasiado."),
                new OfficeRumor("rumor_12", "Se rumora que hoy están de malas porque perdió en el fantasy."),
                new OfficeRumor("rumor_13", "Cuentan que la empresa usa sillas diferentes para saber quién miente.")
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

