using Godot;
using System;
using System.Collections.Generic;

namespace Aprendizdemago.Package.RPGMapNavigation
{
    /// <summary>
    /// Factory para crear líneas de conexión entre nodos de mapa
    /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
    /// </summary>
    public static class ConnectionLineFactory
    {
        /// <summary>
        /// Información de posición de un nodo visual para crear líneas
        /// </summary>
        public class NodeVisualInfo
        {
            public MapNode Node { get; set; }
            public Vector2 Position { get; set; }
            public Vector2 Size { get; set; }
        }
        
        /// <summary>
        /// Crea una línea de conexión entre dos nodos
        /// </summary>
        /// <param name="fromNodeInfo">Información visual del nodo origen</param>
        /// <param name="toNodeInfo">Información visual del nodo destino</param>
        /// <param name="connectionIndex">Índice de la conexión</param>
        /// <returns>Línea de conexión creada</returns>
        public static Line2D CreateConnectionLine(NodeVisualInfo fromNodeInfo, NodeVisualInfo toNodeInfo, int connectionIndex)
        {
            if (fromNodeInfo == null || toNodeInfo == null)
            {
                GD.PrintErr("❌ No se puede crear línea: información de nodos inválida");
                return null;
            }
            
            // Calcular puntos de conexión (centros de los nodos)
            Vector2 fromPoint = CalculateNodeCenter(fromNodeInfo.Position, fromNodeInfo.Size);
            Vector2 toPoint = CalculateNodeCenter(toNodeInfo.Position, toNodeInfo.Size);
            
            // Crear línea
            var line = new Line2D();
            line.Name = $"ConnectionLine_{connectionIndex}";
            
            // Configurar la línea con efecto neon
            line.Width = 8;
            line.DefaultColor = new Color(0.0f, 1.0f, 1.0f, 1.0f); // Cian brillante
            
            line.AddPoint(fromPoint);
            line.AddPoint(toPoint);
            
            GD.Print($"✅ Línea de conexión creada entre {fromNodeInfo.Node.Name} y {toNodeInfo.Node.Name} (de {fromPoint} a {toPoint})");
            
            return line;
        }
        
        /// <summary>
        /// Crea todas las líneas de conexión para un mapa
        /// </summary>
        /// <param name="map">Mapa del cual crear las líneas</param>
        /// <param name="nodeVisualInfos">Diccionario de información visual de nodos por nombre</param>
        /// <returns>Lista de líneas creadas</returns>
        public static List<Line2D> CreateAllConnectionLines(Map map, Dictionary<string, NodeVisualInfo> nodeVisualInfos)
        {
            var lines = new List<Line2D>();
            
            if (map == null || nodeVisualInfos == null)
            {
                GD.PrintErr("❌ No se pueden crear líneas: mapa o información visual inválida");
                return lines;
            }
            
            var nodes = map.GetNodesInOrder();
            int connectionIndex = 0;
            
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                var currentNode = nodes[i];
                var nextNode = nodes[i + 1];
                
                // Verificar que la conexión existe
                if (currentNode.NextNode == nextNode)
                {
                    // Obtener información visual de los nodos
                    if (nodeVisualInfos.TryGetValue(currentNode.Name, out var fromInfo) &&
                        nodeVisualInfos.TryGetValue(nextNode.Name, out var toInfo))
                    {
                        var line = CreateConnectionLine(fromInfo, toInfo, connectionIndex);
                        if (line != null)
                        {
                            lines.Add(line);
                            connectionIndex++;
                        }
                    }
                    else
                    {
                        GD.PrintErr($"❌ No se encontró información visual para nodos: {currentNode.Name} o {nextNode.Name}");
                    }
                }
            }
            
            GD.Print($"🔗 Se crearon {lines.Count} líneas de conexión para el mapa {map.Name}");
            return lines;
        }
        
        /// <summary>
        /// Calcula el centro de un nodo basado en su posición y tamaño
        /// </summary>
        /// <param name="position">Posición del nodo</param>
        /// <param name="size">Tamaño del nodo</param>
        /// <returns>Posición del centro del nodo</returns>
        private static Vector2 CalculateNodeCenter(Vector2 position, Vector2 size)
        {
            return position + new Vector2(size.X * 0.5f, size.Y * 0.5f);
        }
    }
}

