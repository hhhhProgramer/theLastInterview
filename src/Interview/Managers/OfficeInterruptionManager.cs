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
                new OfficeInterruption("interrupt_01", "Error. El entrevistador ha dejado de funcionar.", "Sistema"),
                new OfficeInterruption("interrupt_02", "Advertencia: tu CV contiene demasiadas mentiras.", "Pop-up"),
                new OfficeInterruption("interrupt_03", "Mamá del entrevistador: ¿sí le preguntaste si toma mucho?", "Llamada"),
                new OfficeInterruption("interrupt_04", "Sistema: El entrevistador necesita actualizarse. ¿Desea continuar?", "Sistema"),
                new OfficeInterruption("interrupt_05", "Alerta: Se detectó una sonrisa genuina. Procediendo con protocolo anti-felicidad.", "Sistema"),
                new OfficeInterruption("interrupt_06", "Notificación: El último candidato aún está esperando en la sala. Desde ayer.", "Sistema"),
                new OfficeInterruption("interrupt_07", "Recordatorio: La entrevista debería haber terminado hace 3 horas.", "Calendario"),
                new OfficeInterruption("interrupt_08", "Mensaje: '¿Ya terminaste? Tengo hambre.' - Entrevistador anterior", "Mensaje"),
                new OfficeInterruption("interrupt_09", "Alerta de seguridad: Se detectaron demasiadas respuestas coherentes.", "Sistema"),
                new OfficeInterruption("interrupt_10", "Actualización: El sistema de evaluación ha sido reemplazado por una moneda.", "Sistema")
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

