using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprendizdemago.Package.RPGMapNavigation
{
    /// <summary>
    /// Representa un mapa con fondo y nodos de navegación
    /// </summary>
    public class Map
    {
        /// <summary>
        /// Nombre único del mapa
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Ruta de la imagen de fondo del mapa
        /// </summary>
        public string BackgroundPath { get; private set; }
        
        /// <summary>
        /// Textura del fondo del mapa
        /// </summary>
        public Texture2D BackgroundTexture { get; private set; }
        
        /// <summary>
        /// Diccionario de nodos por nombre
        /// </summary>
        private Dictionary<string, MapNode> _nodes = new Dictionary<string, MapNode>();
        
        /// <summary>
        /// Lista de nodos en orden de creación
        /// </summary>
        private List<MapNode> _nodeOrder = new List<MapNode>();
        
        /// <summary>
        /// Constructor del mapa
        /// </summary>
        /// <param name="name">Nombre del mapa</param>
        /// <param name="backgroundPath">Ruta de la imagen de fondo</param>
        public Map(string name, string backgroundPath)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del mapa no puede estar vacío", nameof(name));
            }
            
            Name = name;
            BackgroundPath = backgroundPath;
            
            // Cargar la textura de fondo
            LoadBackgroundTexture();
            
            GD.Print($"🗺️ Mapa creado: {Name}");
        }
        
        /// <summary>
        /// Carga la textura de fondo del mapa
        /// </summary>
        private void LoadBackgroundTexture()
        {
            if (!string.IsNullOrEmpty(BackgroundPath))
            {
                BackgroundTexture = GD.Load<Texture2D>(BackgroundPath);
                if (BackgroundTexture != null)
                {
                    GD.Print($"✅ Fondo cargado para {Name}: {BackgroundPath}");
                }
                else
                {
                    GD.PrintErr($"❌ No se pudo cargar el fondo: {BackgroundPath}");
                }
            }
        }
        
        /// <summary>
        /// Agrega un nodo al mapa
        /// </summary>
        /// <param name="node">Nodo a agregar</param>
        public void AddNode(MapNode node)
        {
            if (node == null || string.IsNullOrEmpty(node.Name))
            {
                GD.PrintErr("❌ No se puede agregar nodo: datos inválidos");
                return;
            }
            
            if (_nodes.ContainsKey(node.Name))
            {
                GD.PrintErr($"❌ Ya existe un nodo con el nombre: {node.Name}");
                return;
            }
            
            _nodes[node.Name] = node;
            _nodeOrder.Add(node);
            
            GD.Print($"✅ Nodo agregado a {Name}: {node.Name}");
        }
        
        /// <summary>
        /// Obtiene un nodo por nombre
        /// </summary>
        /// <param name="nodeName">Nombre del nodo</param>
        /// <returns>Nodo encontrado o null</returns>
        public MapNode GetNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                GD.PrintErr("❌ Nombre de nodo inválido");
                return null;
            }
            
            if (!_nodes.ContainsKey(nodeName))
            {
                GD.PrintErr($"❌ No se encontró el nodo: {nodeName}");
                return null;
            }
            
            return _nodes[nodeName];
        }
        
        /// <summary>
        /// Obtiene todos los nodos del mapa
        /// </summary>
        /// <returns>Lista de nodos</returns>
        public List<MapNode> GetAllNodes()
        {
            return new List<MapNode>(_nodes.Values);
        }
        
        /// <summary>
        /// Obtiene todos los nodos en orden de creación
        /// </summary>
        /// <returns>Lista de nodos en orden</returns>
        public List<MapNode> GetNodesInOrder()
        {
            return new List<MapNode>(_nodeOrder);
        }
        
        /// <summary>
        /// Obtiene el primer nodo del mapa
        /// </summary>
        /// <returns>Primer nodo o null</returns>
        public MapNode GetFirstNode()
        {
            return _nodeOrder.FirstOrDefault();
        }
        
        /// <summary>
        /// Obtiene el último nodo del mapa
        /// </summary>
        /// <returns>Último nodo o null</returns>
        public MapNode GetLastNode()
        {
            return _nodeOrder.LastOrDefault();
        }
        
        /// <summary>
        /// Conecta dos nodos en secuencia
        /// </summary>
        /// <param name="fromNodeName">Nombre del nodo origen</param>
        /// <param name="toNodeName">Nombre del nodo destino</param>
        public void ConnectNodes(string fromNodeName, string toNodeName)
        {
            var fromNode = GetNode(fromNodeName);
            var toNode = GetNode(toNodeName);
            
            if (fromNode == null || toNode == null)
            {
                GD.PrintErr("❌ No se pueden conectar nodos: uno o ambos nodos no existen");
                return;
            }
            
            fromNode.SetNextNode(toNode);
            toNode.SetPreviousNode(fromNode);
            
            GD.Print($"🔗 Nodos conectados: {fromNodeName} → {toNodeName}");
        }
        
        /// <summary>
        /// Desconecta un nodo de sus conexiones
        /// </summary>
        /// <param name="nodeName">Nombre del nodo</param>
        public void DisconnectNode(string nodeName)
        {
            var node = GetNode(nodeName);
            if (node == null) return;
            
            if (node.PreviousNode != null)
            {
                node.PreviousNode.SetNextNode(null);
            }
            
            if (node.NextNode != null)
            {
                node.NextNode.SetPreviousNode(null);
            }
            
            node.SetPreviousNode(null);
            node.SetNextNode(null);
            
            GD.Print($"🔌 Nodo desconectado: {nodeName}");
        }
        
        /// <summary>
        /// Elimina un nodo del mapa
        /// </summary>
        /// <param name="nodeName">Nombre del nodo</param>
        public void RemoveNode(string nodeName)
        {
            var node = GetNode(nodeName);
            if (node == null) return;
            
            // Desconectar el nodo primero
            DisconnectNode(nodeName);
            
            // Eliminar del diccionario y lista
            _nodes.Remove(nodeName);
            _nodeOrder.Remove(node);
            
            GD.Print($"🗑️ Nodo eliminado de {Name}: {nodeName}");
        }
        
        /// <summary>
        /// Obtiene información del mapa
        /// </summary>
        /// <returns>String con información del mapa</returns>
        public string GetInfo()
        {
            var info = $"=== MAPA: {Name} ===\n";
            info += $"Fondo: {BackgroundPath ?? "Ninguno"}\n";
            info += $"Nodos: {_nodes.Count}\n";
            
            if (_nodes.Count > 0)
            {
                info += "\n--- NODOS ---\n";
                foreach (var node in _nodeOrder)
                {
                    info += $"{node.Name}";
                    if (node.PreviousNode != null)
                        info += $" ← {node.PreviousNode.Name}";
                    if (node.NextNode != null)
                        info += $" → {node.NextNode.Name}";
                    info += "\n";
                }
            }
            
            return info;
        }
        
        /// <summary>
        /// Limpia todos los nodos del mapa
        /// </summary>
        public void ClearNodes()
        {
            _nodes.Clear();
            _nodeOrder.Clear();
            GD.Print($"🗑️ Todos los nodos eliminados de {Name}");
        }
    }
}
