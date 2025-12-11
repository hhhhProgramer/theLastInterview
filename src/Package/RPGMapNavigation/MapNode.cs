using Godot;
using System;
using System.Collections.Generic;

namespace Aprendizdemago.Package.RPGMapNavigation
{
    /// <summary>
    /// Representa un nodo de navegación en un mapa
    /// </summary>
    public class MapNode
    {
        /// <summary>
        /// Nombre único del nodo
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// Posición del nodo en el mapa
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// Ruta de la imagen del nodo
        /// </summary>
        public string ImagePath { get; private set; }
        
        /// <summary>
        /// Textura del nodo
        /// </summary>
        public Texture2D NodeTexture { get; private set; }
        
        /// <summary>
        /// Ruta de la imagen de fondo del nodo
        /// </summary>
        public string BackgroundImagePath { get; set; } = "";
        
        /// <summary>
        /// Textura de fondo del nodo (cargada desde BackgroundImagePath)
        /// </summary>
        public Texture2D BackgroundTexture { get; set; }
        
        /// <summary>
        /// Nodo siguiente en la secuencia
        /// </summary>
        public MapNode NextNode { get; private set; }
        
        /// <summary>
        /// Nodo anterior en la secuencia
        /// </summary>
        public MapNode PreviousNode { get; private set; }
        
        /// <summary>
        /// Indica si el nodo está desbloqueado
        /// </summary>
        public bool IsUnlocked { get; set; } = false;
        
        /// <summary>
        /// Indica si el nodo está completado
        /// </summary>
        public bool IsCompleted { get; set; } = false;
        
        /// <summary>
        /// Datos adicionales del nodo
        /// </summary>
        public Dictionary<string, object> Data { get; private set; } = new Dictionary<string, object>();
        
        /// <summary>
        /// Constructor del nodo
        /// </summary>
        /// <param name="name">Nombre del nodo</param>
        /// <param name="position">Posición en el mapa</param>
        /// <param name="imagePath">Ruta de la imagen del nodo</param>
        public MapNode(string name, Vector2 position, string imagePath = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("El nombre del nodo no puede estar vacío", nameof(name));
            }
            
            Name = name;
            Position = position;
            ImagePath = imagePath;
            
            // Cargar la textura del nodo
            LoadNodeTexture();
            
            GD.Print($"🎯 Nodo creado: {Name} en posición {Position}");
        }
        
        /// <summary>
        /// Carga la textura del nodo
        /// </summary>
        private void LoadNodeTexture()
        {
            if (!string.IsNullOrEmpty(ImagePath))
            {
                NodeTexture = GD.Load<Texture2D>(ImagePath);
                if (NodeTexture != null)
                {
                    GD.Print($"✅ Imagen cargada para nodo {Name}: {ImagePath}");
                }
                else
                {
                    GD.PrintErr($"❌ No se pudo cargar la imagen del nodo: {ImagePath}");
                }
            }
        }
        
        /// <summary>
        /// Establece el nodo siguiente
        /// </summary>
        /// <param name="nextNode">Nodo siguiente</param>
        public void SetNextNode(MapNode nextNode)
        {
            NextNode = nextNode;
            GD.Print($"🔗 {Name} → {nextNode?.Name ?? "null"}");
        }
        
        /// <summary>
        /// Establece el nodo anterior
        /// </summary>
        /// <param name="previousNode">Nodo anterior</param>
        public void SetPreviousNode(MapNode previousNode)
        {
            PreviousNode = previousNode;
            GD.Print($"🔗 {previousNode?.Name ?? "null"} ← {Name}");
        }
        
        /// <summary>
        /// Desbloquea el nodo
        /// </summary>
        public void Unlock()
        {
            IsUnlocked = true;
            GD.Print($"🔓 Nodo desbloqueado: {Name}");
        }
        
        /// <summary>
        /// Bloquea el nodo
        /// </summary>
        public void Lock()
        {
            IsUnlocked = false;
            GD.Print($"🔒 Nodo bloqueado: {Name}");
        }
        
        /// <summary>
        /// Marca el nodo como completado
        /// </summary>
        public void Complete()
        {
            IsCompleted = true;
            GD.Print($"✅ Nodo completado: {Name}");
        }
        
        /// <summary>
        /// Marca el nodo como no completado
        /// </summary>
        public void Uncomplete()
        {
            IsCompleted = false;
            GD.Print($"❌ Nodo marcado como no completado: {Name}");
        }
        
        /// <summary>
        /// Agrega datos adicionales al nodo
        /// </summary>
        /// <param name="key">Clave del dato</param>
        /// <param name="value">Valor del dato</param>
        public void SetData(string key, object value)
        {
            Data[key] = value;
        }
        
        /// <summary>
        /// Obtiene datos adicionales del nodo
        /// </summary>
        /// <param name="key">Clave del dato</param>
        /// <returns>Valor del dato o null</returns>
        public object GetData(string key)
        {
            return Data.ContainsKey(key) ? Data[key] : null;
        }
        
        /// <summary>
        /// Carga la textura de fondo desde BackgroundImagePath
        /// </summary>
        public void LoadBackgroundTexture()
        {
            if (!string.IsNullOrEmpty(BackgroundImagePath))
            {
                BackgroundTexture = GD.Load<Texture2D>(BackgroundImagePath);
                if (BackgroundTexture == null)
                {
                    GD.PrintErr($"❌ No se pudo cargar la imagen de fondo: {BackgroundImagePath}");
                }
                else
                {
                    GD.Print($"✅ Imagen de fondo cargada: {BackgroundImagePath}");
                }
            }
        }
        
        /// <summary>
        /// Establece la imagen de fondo del nodo
        /// </summary>
        /// <param name="imagePath">Ruta de la imagen de fondo</param>
        public void SetBackgroundImage(string imagePath)
        {
            BackgroundImagePath = imagePath;
            LoadBackgroundTexture();
        }
        
        /// <summary>
        /// Obtiene datos adicionales del nodo con tipo específico
        /// </summary>
        /// <typeparam name="T">Tipo del dato</typeparam>
        /// <param name="key">Clave del dato</param>
        /// <returns>Valor del dato o valor por defecto</returns>
        public T GetData<T>(string key)
        {
            var value = GetData(key);
            if (value is T typedValue)
            {
                return typedValue;
            }
            return default(T);
        }
        
        /// <summary>
        /// Verifica si el nodo puede ser navegado
        /// </summary>
        /// <returns>True si puede ser navegado</returns>
        public bool CanNavigate()
        {
            return IsUnlocked;
        }
        
        /// <summary>
        /// Obtiene el estado visual del nodo
        /// </summary>
        /// <returns>Estado visual del nodo</returns>
        public NodeVisualState GetVisualState()
        {
            if (IsCompleted)
                return NodeVisualState.Completed;
            else if (IsUnlocked)
                return NodeVisualState.Unlocked;
            else
                return NodeVisualState.Locked;
        }
        
        /// <summary>
        /// Obtiene información del nodo
        /// </summary>
        /// <returns>String con información del nodo</returns>
        public string GetInfo()
        {
            var info = $"=== NODO: {Name} ===\n";
            info += $"Posición: {Position}\n";
            info += $"Imagen: {ImagePath ?? "Ninguna"}\n";
            info += $"Estado: {GetVisualState()}\n";
            info += $"Desbloqueado: {IsUnlocked}\n";
            info += $"Completado: {IsCompleted}\n";
            info += $"Puede navegar: {CanNavigate()}\n";
            
            if (PreviousNode != null)
                info += $"Anterior: {PreviousNode.Name}\n";
            if (NextNode != null)
                info += $"Siguiente: {NextNode.Name}\n";
            
            if (Data.Count > 0)
            {
                info += "\n--- DATOS ADICIONALES ---\n";
                foreach (var kvp in Data)
                {
                    info += $"{kvp.Key}: {kvp.Value}\n";
                }
            }
            
            return info;
        }
    }
    
    /// <summary>
    /// Estados visuales posibles para un nodo
    /// </summary>
    public enum NodeVisualState
    {
        /// <summary>
        /// Nodo bloqueado (no se puede acceder)
        /// </summary>
        Locked,
        
        /// <summary>
        /// Nodo desbloqueado (se puede acceder)
        /// </summary>
        Unlocked,
        
        /// <summary>
        /// Nodo completado (ya se completó)
        /// </summary>
        Completed
    }
}
