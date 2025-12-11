using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprendizdemago.Package.RPGMapNavigation
{
    /// <summary>
    /// Manager principal para la navegación de mapas RPG
    /// Maneja múltiples mapas y sus nodos de conexión
    /// Singleton persistente disponible en todas las escenas
    /// </summary>
    public partial class MapNavigationManager : Node
    {
        /// <summary>
        /// Instancia singleton del MapNavigationManager
        /// </summary>
        public static MapNavigationManager Instance { get; private set; }
        
        /// <summary>
        /// Diccionario de mapas por nombre
        /// </summary>
        private Dictionary<string, Map> _maps = new Dictionary<string, Map>();
        
        /// <summary>
        /// Mapa actualmente activo
        /// </summary>
        public Map CurrentMap { get; private set; }
        
        /// <summary>
        /// Nodo actualmente seleccionado
        /// </summary>
        public MapNode CurrentNode { get; private set; }
        
        /// <summary>
        /// Índice del mapa actual en la lista de mapas
        /// </summary>
        public int CurrentMapIndex { get; private set; } = 0;
        
        /// <summary>
        /// Indica si el sistema está inicializado
        /// </summary>
        public bool IsInitialized { get; private set; } = false;
        
        /// <summary>
        /// Indica si se están creando líneas de conexión para evitar duplicados
        /// </summary>
        private bool _isCreatingConnectionLines = false;
        
        /// <summary>
        /// Evento que se dispara cuando se cambia de mapa
        /// </summary>
        public event Action<Map> OnMapChanged;
        
        /// <summary>
        /// Evento que se dispara cuando se selecciona un nodo
        /// </summary>
        public event Action<MapNode> OnNodeSelected;
        
        /// <summary>
        /// Evento que se dispara cuando se navega a un nodo
        /// </summary>
        public event Action<MapNode> OnNodeNavigated;
        
        /// <summary>
        /// Evento que se dispara cuando se necesita crear una línea de conexión
        /// </summary>
        public event Action<MapNode, MapNode, int> OnConnectionLineRequested;
        
        /// <summary>
        /// Evento que se dispara cuando se necesita limpiar las líneas de conexión
        /// </summary>
        public event Action OnConnectionLinesClearRequested;
        
        /// <summary>
        /// Evento que se dispara cuando se necesita crear efecto neon para una línea
        /// </summary>
        public event Action<Line2D, Vector2, Vector2> OnNeonEffectRequested;
        
        /// <summary>
        /// Evento que se dispara cuando se necesita crear animación de parpadeo
        /// </summary>
        public event Action<Line2D, Line2D, Line2D> OnFlickerEffectRequested;
        
        /// <summary>
        /// Constructor del manager
        /// </summary>
        public MapNavigationManager()
        {
            Name = "MapNavigationManager";
            
            // Implementar patrón singleton
            if (Instance == null)
            {
                Instance = this;
                // Hacer persistente para que sobreviva cambios de escena
                ProcessMode = ProcessModeEnum.Always;
            }
            else
            {
                // Si ya existe una instancia, eliminar esta
                QueueFree();
                return;
            }
        }
        
        /// <summary>
        /// Inicializa el sistema de navegación de mapas
        /// </summary>
        public override void _Ready()
        {
            // Solo inicializar si somos la instancia singleton
            if (Instance == this)
            {
                GD.Print("🗺️ MapNavigationManager singleton inicializado");
                IsInitialized = true;
            }
        }
        
        /// <summary>
        /// Agrega un nuevo mapa al sistema
        /// </summary>
        /// <param name="map">Mapa a agregar</param>
        public void AddMap(Map map)
        {
            if (map == null || string.IsNullOrEmpty(map.Name))
            {
                GD.PrintErr("❌ No se puede agregar mapa: nombre inválido");
                return;
            }
            
            if (_maps.ContainsKey(map.Name))
            {
                GD.PrintErr($"❌ Ya existe un mapa con el nombre: {map.Name}");
                return;
            }
            
            _maps[map.Name] = map;
            GD.Print($"✅ Mapa agregado: {map.Name}");
            
            // Si es el primer mapa, establecerlo como actual
            if (CurrentMap == null)
            {
                SetCurrentMap(map.Name);
            }
        }
        
        /// <summary>
        /// Obtiene un mapa por nombre
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        /// <returns>Mapa encontrado o null</returns>
        public Map GetMap(string mapName)
        {
            if (string.IsNullOrEmpty(mapName))
            {
                GD.PrintErr("❌ Nombre de mapa inválido");
                return null;
            }
            
            if (!_maps.ContainsKey(mapName))
            {
                GD.PrintErr($"❌ No se encontró el mapa: {mapName}");
                return null;
            }
            
            return _maps[mapName];
        }
        
        /// <summary>
        /// Establece el mapa actual
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        public void SetCurrentMap(string mapName)
        {
            var map = GetMap(mapName);
            if (map == null) return;
            
            CurrentMap = map;
            CurrentNode = null; // Resetear nodo actual
            
            GD.Print($"🗺️ Mapa actual cambiado a: {mapName}");
            OnMapChanged?.Invoke(map);
            
            // Solo solicitar líneas de conexión si el mapa tiene nodos
            if (map.GetAllNodes().Count > 0)
            {
                RequestConnectionLines();
            }
        }
        
        /// <summary>
        /// Selecciona un nodo en el mapa actual
        /// </summary>
        /// <param name="nodeName">Nombre del nodo</param>
        public void SelectNode(string nodeName)
        {
            if (CurrentMap == null)
            {
                GD.PrintErr("❌ No hay mapa actual seleccionado");
                return;
            }
            
            var node = CurrentMap.GetNode(nodeName);
            if (node == null) return;
            
            CurrentNode = node;
            GD.Print($"🎯 Nodo seleccionado: {nodeName}");
            OnNodeSelected?.Invoke(node);
        }
        
        /// <summary>
        /// Navega al siguiente nodo desde el nodo actual
        /// </summary>
        public void NavigateToNext()
        {
            if (CurrentNode?.NextNode == null)
            {
                GD.PrintErr("❌ No hay siguiente nodo disponible");
                return;
            }
            
            NavigateToNode(CurrentNode.NextNode);
        }
        
        /// <summary>
        /// Navega al nodo anterior desde el nodo actual
        /// </summary>
        public void NavigateToPrevious()
        {
            if (CurrentNode?.PreviousNode == null)
            {
                GD.PrintErr("❌ No hay nodo anterior disponible");
                return;
            }
            
            NavigateToNode(CurrentNode.PreviousNode);
        }
        
        /// <summary>
        /// Navega a un nodo específico
        /// </summary>
        /// <param name="node">Nodo destino</param>
        private void NavigateToNode(MapNode node)
        {
            if (node == null)
            {
                GD.PrintErr("❌ Nodo destino inválido");
                return;
            }
            
            CurrentNode = node;
            GD.Print($"🚀 Navegando a nodo: {node.Name}");
            OnNodeNavigated?.Invoke(node);
        }
        
        /// <summary>
        /// Obtiene todos los mapas disponibles
        /// </summary>
        /// <returns>Lista de nombres de mapas</returns>
        public List<string> GetAvailableMaps()
        {
            return _maps.Keys.ToList();
        }
        
        /// <summary>
        /// Obtiene todos los nodos del mapa actual
        /// </summary>
        /// <returns>Lista de nodos</returns>
        public List<MapNode> GetCurrentMapNodes()
        {
            return CurrentMap?.GetAllNodes() ?? new List<MapNode>();
        }
        
        /// <summary>
        /// Obtiene información del estado actual del sistema
        /// </summary>
        /// <returns>String con información del estado</returns>
        public string GetStatus()
        {
            var status = "=== ESTADO DE NAVEGACIÓN DE MAPAS ===\n";
            status += $"Mapas disponibles: {_maps.Count}\n";
            status += $"Mapa actual: {CurrentMap?.Name ?? "Ninguno"}\n";
            status += $"Nodo actual: {CurrentNode?.Name ?? "Ninguno"}\n";
            
            if (CurrentMap != null)
            {
                status += $"Nodos en mapa actual: {CurrentMap.GetAllNodes().Count}\n";
            }
            
            return status;
        }
        
        /// <summary>
        /// Limpia todos los mapas del sistema
        /// </summary>
        public void ClearAllMaps()
        {
            _maps.Clear();
            CurrentMap = null;
            CurrentNode = null;
            GD.Print("🗺️ Todos los mapas han sido eliminados");
        }
        
        /// <summary>
        /// Solicita la creación de líneas de conexión para el mapa actual
        /// </summary>
        public void RequestConnectionLines()
        {
            if (CurrentMap == null) return;
            
            // Evitar solicitudes duplicadas
            if (_isCreatingConnectionLines) return;
            _isCreatingConnectionLines = true;
            
            // Limpiar líneas existentes primero
            OnConnectionLinesClearRequested?.Invoke();
            
            // Crear líneas para cada conexión
            var nodes = CurrentMap.GetNodesInOrder();
            int connectionIndex = 0;
            
            // ⚠️ CRÍTICO: Crear líneas para TODOS los nodos consecutivos en orden
            // Crear líneas basándose en el orden de la lista, no en las conexiones NextNode
            // Esto asegura que todas las líneas se creen, incluyendo la última (9-10)
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                var currentNode = nodes[i];
                var nextNode = nodes[i + 1];
                
                // Solicitar línea de conexión para todos los nodos consecutivos
                OnConnectionLineRequested?.Invoke(currentNode, nextNode, connectionIndex);
                connectionIndex++;
            }
            
            GD.Print($"🔗 Se solicitaron {connectionIndex} líneas de conexión para {nodes.Count} nodos (debería ser {nodes.Count - 1})");
            _isCreatingConnectionLines = false;
        }
        
        /// <summary>
        /// Actualiza las líneas de conexión cuando cambia el mapa
        /// </summary>
        public void UpdateConnectionLines()
        {
            RequestConnectionLines();
        }
        
        /// <summary>
        /// Solicita la creación de efecto neon para una línea
        /// </summary>
        public void RequestNeonEffect(Line2D mainLine, Vector2 fromPoint, Vector2 toPoint)
        {
            OnNeonEffectRequested?.Invoke(mainLine, fromPoint, toPoint);
        }
        
        /// <summary>
        /// Solicita la creación de animación de parpadeo
        /// </summary>
        public void RequestFlickerEffect(Line2D mainLine, Line2D glowLine, Line2D innerLine)
        {
            OnFlickerEffectRequested?.Invoke(mainLine, glowLine, innerLine);
        }
        
        /// <summary>
        /// Navega al mapa anterior
        /// </summary>
        public void NavigateToPreviousMap()
        {
            if (CurrentMapIndex > 0)
            {
                CurrentMapIndex--;
                var mapNames = _maps.Keys.ToList();
                if (CurrentMapIndex < mapNames.Count)
                {
                    SetCurrentMap(mapNames[CurrentMapIndex]);
                    GD.Print($"🗺️ Navegando al mapa anterior: {mapNames[CurrentMapIndex]}");
                }
            }
        }
        
        /// <summary>
        /// Navega al mapa siguiente
        /// </summary>
        public void NavigateToNextMap()
        {
            var mapNames = _maps.Keys.ToList();
            if (CurrentMapIndex < mapNames.Count - 1)
            {
                CurrentMapIndex++;
                SetCurrentMap(mapNames[CurrentMapIndex]);
                GD.Print($"🗺️ Navegando al mapa siguiente: {mapNames[CurrentMapIndex]}");
            }
        }
        
        /// <summary>
        /// Verifica si se puede navegar al mapa anterior
        /// </summary>
        public bool CanNavigateToPreviousMap()
        {
            return CurrentMapIndex > 0;
        }
        
        /// <summary>
        /// Verifica si se puede navegar al mapa siguiente
        /// </summary>
        public bool CanNavigateToNextMap()
        {
            var mapNames = _maps.Keys.ToList();
            return CurrentMapIndex < mapNames.Count - 1;
        }
        
        /// <summary>
        /// Obtiene el nombre del mapa actual
        /// </summary>
        public string GetCurrentMapName()
        {
            return CurrentMap?.Name ?? "Sin mapa";
        }
        
        /// <summary>
        /// Obtiene el índice del mapa actual (1-based)
        /// </summary>
        public int GetCurrentMapNumber()
        {
            return CurrentMapIndex + 1;
        }
        
        /// <summary>
        /// Obtiene el total de mapas disponibles
        /// </summary>
        public int GetTotalMaps()
        {
            return _maps.Count;
        }
        
        /// <summary>
        /// Desbloquea el siguiente nodo después de completar uno específico
        /// Maneja el caso de pasar al siguiente mapa si es el último nodo del mapa actual
        /// </summary>
        /// <param name="completedNodeName">Nombre del nodo que se completó</param>
        /// <param name="mapName">Nombre del mapa que contiene el nodo</param>
        /// <returns>True si se desbloqueó el siguiente nodo, False si no hay siguiente</returns>
        public bool UnlockNextNode(string completedNodeName, string mapName)
        {
            // Obtener el mapa específico
            var map = GetMap(mapName);
            if (map == null)
            {
                GD.PrintErr($"❌ No se encontró el mapa: {mapName}");
                return false;
            }
            
            // Obtener el nodo completado
            var completedNode = map.GetNode(completedNodeName);
            if (completedNode == null)
            {
                GD.PrintErr($"❌ No se encontró el nodo: {completedNodeName} en el mapa: {mapName}");
                return false;
            }
            
            // Marcar el nodo como completado
            completedNode.Complete();
            GD.Print($"✅ Nodo {completedNodeName} marcado como completado");
            
            // Desbloquear el siguiente nodo si existe en el mismo mapa
            if (completedNode.NextNode != null)
            {
                completedNode.NextNode.Unlock();
                GD.Print($"🔓 Siguiente nodo desbloqueado: {completedNode.NextNode.Name} en el mismo mapa");
                return true;
            }
            else
            {
                // No hay siguiente nodo en el mismo mapa, intentar desbloquear el primer nodo del siguiente mapa
                GD.Print($"ℹ️ No hay siguiente nodo en {mapName}, buscando siguiente mapa...");
                
                // Obtener el índice del mapa actual
                int currentMapIndex = GetMapIndex(mapName);
                if (currentMapIndex < 0)
                {
                    GD.PrintErr($"❌ No se encontró el índice del mapa: {mapName}");
                    return false;
                }
                
                // Verificar si hay un siguiente mapa
                int nextMapIndex = currentMapIndex + 1;
                if (nextMapIndex < _maps.Count)
                {
                    // Obtener el siguiente mapa
                    var nextMap = _maps.Values.ElementAt(nextMapIndex);
                    if (nextMap != null)
                    {
                        // Obtener el primer nodo del siguiente mapa
                        var firstNode = nextMap.GetFirstNode();
                        if (firstNode != null)
                        {
                            firstNode.Unlock();
                            GD.Print($"🔓 Primer nodo del siguiente mapa desbloqueado: {firstNode.Name} en {nextMap.Name}");
                            return true;
                        }
                        else
                        {
                            GD.PrintErr($"❌ No se encontró el primer nodo del siguiente mapa: {nextMap.Name}");
                            return false;
                        }
                    }
                    else
                    {
                        GD.PrintErr($"❌ El siguiente mapa en índice {nextMapIndex} es null");
                        return false;
                    }
                }
                else
                {
                    GD.Print($"ℹ️ No hay siguiente mapa después de {mapName} (último mapa)");
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Obtiene el índice del mapa por su nombre
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        /// <returns>Índice del mapa o -1 si no se encuentra</returns>
        private int GetMapIndex(string mapName)
        {
            int index = 0;
            foreach (var map in _maps.Values)
            {
                if (map.Name == mapName)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
