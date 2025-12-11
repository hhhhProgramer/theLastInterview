using Godot;
using System;
using System.Collections.Generic;
using Aprendizdemago.Package.RPGMapNavigation;

namespace Aprendizdemago.Package.RPGMapNavigation
{
    /// <summary>
    /// Factory para crear mapas y nodos de forma fácil
    /// </summary>
    public static class MapNavigationFactory
    {
        /// <summary>
        /// Crea un nuevo mapa
        /// </summary>
        /// <param name="name">Nombre del mapa</param>
        /// <param name="backgroundPath">Ruta de la imagen de fondo</param>
        /// <returns>Nuevo mapa</returns>
        public static Map CreateMap(string name, string backgroundPath)
        {
            return new Map(name, backgroundPath);
        }
        
        /// <summary>
        /// Crea un nuevo nodo
        /// </summary>
        /// <param name="name">Nombre del nodo</param>
        /// <param name="position">Posición en el mapa</param>
        /// <param name="imagePath">Ruta de la imagen del nodo (opcional)</param>
        /// <returns>Nuevo nodo</returns>
        public static MapNode CreateNode(string name, Vector2 position, string imagePath = null)
        {
            return new MapNode(name, position, imagePath);
        }
        
        /// <summary>
        /// Crea un mapa con nodos conectados en línea
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        /// <param name="backgroundPath">Ruta de la imagen de fondo</param>
        /// <param name="nodeCount">Número de nodos a crear</param>
        /// <param name="startPosition">Posición inicial</param>
        /// <param name="spacing">Espaciado entre nodos</param>
        /// <returns>Mapa con nodos conectados</returns>
        public static Map CreateLinearMap(string mapName, string backgroundPath, int nodeCount, 
            Vector2 startPosition, Vector2 spacing)
        {
            var map = CreateMap(mapName, backgroundPath);
            
            for (int i = 0; i < nodeCount; i++)
            {
                var nodeName = $"Nodo_{i + 1}";
                var nodePosition = startPosition + spacing * i;
                var node = CreateNode(nodeName, nodePosition);
                
                map.AddNode(node);
                
                // Conectar con el nodo anterior si existe
                if (i > 0)
                {
                    var previousNode = map.GetNode($"Nodo_{i}");
                    map.ConnectNodes(previousNode.Name, node.Name);
                }
            }
            
            return map;
        }
        
        /// <summary>
        /// Crea un mapa con nodos en forma de grilla
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        /// <param name="backgroundPath">Ruta de la imagen de fondo</param>
        /// <param name="rows">Número de filas</param>
        /// <param name="cols">Número de columnas</param>
        /// <param name="startPosition">Posición inicial</param>
        /// <param name="spacing">Espaciado entre nodos</param>
        /// <returns>Mapa con nodos en grilla</returns>
        public static Map CreateGridMap(string mapName, string backgroundPath, int rows, int cols,
            Vector2 startPosition, Vector2 spacing)
        {
            var map = CreateMap(mapName, backgroundPath);
            
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var nodeName = $"Nodo_{row}_{col}";
                    var nodePosition = startPosition + new Vector2(col * spacing.X, row * spacing.Y);
                    var node = CreateNode(nodeName, nodePosition);
                    
                    map.AddNode(node);
                    
                    // Conectar con nodos adyacentes
                    if (col > 0) // Conectar con nodo izquierdo
                    {
                        var leftNode = map.GetNode($"Nodo_{row}_{col - 1}");
                        map.ConnectNodes(leftNode.Name, node.Name);
                    }
                    
                    if (row > 0) // Conectar con nodo superior
                    {
                        var topNode = map.GetNode($"Nodo_{row - 1}_{col}");
                        map.ConnectNodes(topNode.Name, node.Name);
                    }
                }
            }
            
            return map;
        }
        
        /// <summary>
        /// Crea un mapa con nodos en forma de árbol
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        /// <param name="backgroundPath">Ruta de la imagen de fondo</param>
        /// <param name="levels">Número de niveles del árbol</param>
        /// <param name="startPosition">Posición inicial</param>
        /// <param name="spacing">Espaciado entre nodos</param>
        /// <returns>Mapa con nodos en forma de árbol</returns>
        public static Map CreateTreeMap(string mapName, string backgroundPath, int levels,
            Vector2 startPosition, Vector2 spacing)
        {
            var map = CreateMap(mapName, backgroundPath);
            var nodeCounter = 1;
            
            // Crear nodo raíz
            var rootNode = CreateNode($"Nodo_{nodeCounter++}", startPosition);
            map.AddNode(rootNode);
            
            var currentLevelNodes = new List<MapNode> { rootNode };
            
            for (int level = 1; level < levels; level++)
            {
                var nextLevelNodes = new List<MapNode>();
                
                foreach (var parentNode in currentLevelNodes)
                {
                    // Crear 2 nodos hijos por cada nodo padre
                    for (int child = 0; child < 2; child++)
                    {
                        var childPosition = parentNode.Position + 
                            new Vector2((child - 0.5f) * spacing.X, spacing.Y);
                        var childNode = CreateNode($"Nodo_{nodeCounter++}", childPosition);
                        
                        map.AddNode(childNode);
                        map.ConnectNodes(parentNode.Name, childNode.Name);
                        
                        nextLevelNodes.Add(childNode);
                    }
                }
                
                currentLevelNodes = nextLevelNodes;
            }
            
            return map;
        }
        
        /// <summary>
        /// Crea un mapa con nodos en forma de círculo
        /// </summary>
        /// <param name="mapName">Nombre del mapa</param>
        /// <param name="backgroundPath">Ruta de la imagen de fondo</param>
        /// <param name="nodeCount">Número de nodos</param>
        /// <param name="centerPosition">Posición central</param>
        /// <param name="radius">Radio del círculo</param>
        /// <returns>Mapa con nodos en círculo</returns>
        public static Map CreateCircularMap(string mapName, string backgroundPath, int nodeCount,
            Vector2 centerPosition, float radius)
        {
            var map = CreateMap(mapName, backgroundPath);
            
            for (int i = 0; i < nodeCount; i++)
            {
                var angle = (2.0f * Mathf.Pi * i) / nodeCount;
                var nodePosition = centerPosition + new Vector2(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius
                );
                
                var nodeName = $"Nodo_{i + 1}";
                var node = CreateNode(nodeName, nodePosition);
                
                map.AddNode(node);
                
                // Conectar con el siguiente nodo en el círculo
                var nextIndex = (i + 1) % nodeCount;
                var nextNodeName = $"Nodo_{nextIndex + 1}";
                map.ConnectNodes(nodeName, nextNodeName);
            }
            
            return map;
        }
    }
}
