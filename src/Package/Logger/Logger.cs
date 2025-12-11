using Godot;
using System.Collections.Generic;

namespace Aprendizdemago.Package.Logger
{
    /// <summary>
    /// Logger centralizado para el juego
    /// Permite controlar logs con prefijo, delay por frames, y activación/desactivación
    /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Indica si el logger está activo globalmente
        /// </summary>
        public static bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Prefijo por defecto para los logs
        /// </summary>
        public static string DefaultPrefix { get; set; } = "[LOG]";
        
        /// <summary>
        /// Diccionario para rastrear logs con delay por prefijo
        /// </summary>
        private static Dictionary<string, LogEntry> _logEntries = new Dictionary<string, LogEntry>();
        
        /// <summary>
        /// Diccionario para rastrear qué prefijos están activos/desactivados
        /// Si un prefijo no está en el diccionario, está activo por defecto
        /// </summary>
        private static Dictionary<string, bool> _prefixStates = new Dictionary<string, bool>();
        
        /// <summary>
        /// Contador de frames global
        /// </summary>
        private static long _frameCount = 0;
        
        /// <summary>
        /// Clase interna para rastrear entradas de log con delay
        /// </summary>
        private class LogEntry
        {
            public string Message { get; set; }
            public long LastFrame { get; set; }
            public int DelayFrames { get; set; }
            public int Count { get; set; }
        }
        
        /// <summary>
        /// Actualiza el contador de frames (debe llamarse cada frame)
        /// </summary>
        public static void UpdateFrame()
        {
            _frameCount++;
        }
        
        /// <summary>
        /// Activa o desactiva logs para un prefijo específico
        /// </summary>
        /// <param name="prefix">Prefijo a activar/desactivar</param>
        /// <param name="active">true para activar, false para desactivar</param>
        public static void SetPrefixActive(string prefix, bool active)
        {
            if (string.IsNullOrEmpty(prefix))
                return;
            
            _prefixStates[prefix] = active;
        }
        
        /// <summary>
        /// Verifica si un prefijo está activo
        /// </summary>
        /// <param name="prefix">Prefijo a verificar</param>
        /// <returns>true si está activo, false si está desactivado. Si no existe en el diccionario, retorna true (activo por defecto)</returns>
        public static bool IsPrefixActive(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return true;
            
            // Si no está en el diccionario, está activo por defecto
            if (!_prefixStates.ContainsKey(prefix))
                return true;
            
            return _prefixStates[prefix];
        }
        
        /// <summary>
        /// Activa todos los prefijos
        /// </summary>
        public static void ActivateAllPrefixes()
        {
            _prefixStates.Clear();
        }
        
        /// <summary>
        /// Desactiva todos los prefijos
        /// </summary>
        public static void DeactivateAllPrefixes()
        {
            // Obtener todos los prefijos únicos que se han usado
            var allPrefixes = new HashSet<string>();
            foreach (var key in _logEntries.Keys)
            {
                // Extraer el prefijo de la clave (formato: "prefix_message")
                var parts = key.Split('_', 2);
                if (parts.Length > 0 && !string.IsNullOrEmpty(parts[0]))
                {
                    allPrefixes.Add(parts[0]);
                }
            }
            
            // Desactivar todos
            foreach (var prefix in allPrefixes)
            {
                _prefixStates[prefix] = false;
            }
        }
        
        /// <summary>
        /// Imprime un log con prefijo y delay opcional
        /// </summary>
        /// <param name="message">Mensaje a imprimir</param>
        /// <param name="prefix">Prefijo del log (opcional, usa DefaultPrefix si es null)</param>
        /// <param name="delayFrames">Delay en frames antes de imprimir (0 = inmediato)</param>
        public static void Print(string message, string prefix = null, int delayFrames = 0)
        {
            // Verificar estado global primero
            if (!IsActive)
                return;
            
            var logPrefix = prefix ?? DefaultPrefix;
            
            // Verificar si el prefijo específico está activo
            if (!IsPrefixActive(logPrefix))
                return;
            
            var logKey = $"{logPrefix}_{message}";
            
            // Si no hay delay, imprimir inmediatamente
            if (delayFrames <= 0)
            {
                GD.Print($"{logPrefix} {message}");
                return;
            }
            
            // Si hay delay, verificar si ya pasó el tiempo necesario
            if (_logEntries.ContainsKey(logKey))
            {
                var entry = _logEntries[logKey];
                entry.Count++;
                
                // Si ya pasaron los frames necesarios desde el último log
                if (_frameCount - entry.LastFrame >= delayFrames)
                {
                    // Si hay múltiples mensajes acumulados, mostrar el conteo
                    if (entry.Count > 1)
                    {
                        GD.Print($"{logPrefix} {message} (x{entry.Count})");
                    }
                    else
                    {
                        GD.Print($"{logPrefix} {message}");
                    }
                    
                    entry.LastFrame = _frameCount;
                    entry.Count = 0;
                }
            }
            else
            {
                // Primera vez que se registra este log
                var entry = new LogEntry
                {
                    Message = message,
                    LastFrame = _frameCount,
                    DelayFrames = delayFrames,
                    Count = 1
                };
                _logEntries[logKey] = entry;
                
                // Imprimir inmediatamente la primera vez
                GD.Print($"{logPrefix} {message}");
            }
        }
        
        /// <summary>
        /// Imprime un error
        /// </summary>
        /// <param name="message">Mensaje de error</param>
        /// <param name="prefix">Prefijo del log (opcional, usa DefaultPrefix si es null)</param>
        public static void PrintErr(string message, string prefix = null)
        {
            // Verificar estado global primero
            if (!IsActive)
                return;
            
            var logPrefix = prefix ?? DefaultPrefix;
            
            // Verificar si el prefijo específico está activo
            if (!IsPrefixActive(logPrefix))
                return;
            
            GD.PrintErr($"{logPrefix} {message}");
        }
        
        /// <summary>
        /// Imprime una advertencia
        /// </summary>
        /// <param name="message">Mensaje de advertencia</param>
        /// <param name="prefix">Prefijo del log (opcional, usa DefaultPrefix si es null)</param>
        public static void PrintWarn(string message, string prefix = null)
        {
            // Verificar estado global primero
            if (!IsActive)
                return;
            
            var logPrefix = prefix ?? DefaultPrefix;
            
            // Verificar si el prefijo específico está activo
            if (!IsPrefixActive(logPrefix))
                return;
            
            GD.Print($"{logPrefix} [WARN] {message}");
        }
        
        /// <summary>
        /// Limpia todas las entradas de log con delay
        /// </summary>
        public static void ClearDelayedLogs()
        {
            _logEntries.Clear();
        }
        
        /// <summary>
        /// Resetea el contador de frames
        /// </summary>
        public static void ResetFrameCount()
        {
            _frameCount = 0;
        }
    }
}

