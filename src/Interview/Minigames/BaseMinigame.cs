using Godot;
using System;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Clase base para todos los minijuegos tontos
    /// </summary>
    public partial class BaseMinigame : Control
    {
        /// <summary>
        /// Evento que se dispara cuando el minijuego termina (siempre falla)
        /// </summary>
        public event Action OnMinigameFinished;
        
        protected Node _parent;
        
        public BaseMinigame(Node parent)
        {
            _parent = parent;
            Name = GetType().Name;
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            MouseFilter = Control.MouseFilterEnum.Stop;
            Visible = false;
        }
        
        /// <summary>
        /// Muestra el minijuego
        /// </summary>
        public virtual void ShowMinigame()
        {
            Visible = true;
            CreateUI();
        }
        
        /// <summary>
        /// Crea la UI del minijuego (implementar en clases derivadas)
        /// </summary>
        protected virtual void CreateUI()
        {
        }
        
        /// <summary>
        /// Termina el minijuego (siempre falla de forma cómica)
        /// </summary>
        protected virtual void FinishMinigame()
        {
            Visible = false;
            OnMinigameFinished?.Invoke();
        }
        
        /// <summary>
        /// Hace que un botón parpadee continuamente
        /// </summary>
        protected void StartButtonBlink(Button button)
        {
            if (button == null || !IsInstanceValid(button)) return;
            
            var blinkTimer = new Timer();
            blinkTimer.WaitTime = 0.5f; // Parpadea cada 0.5 segundos
            blinkTimer.Timeout += () => {
                if (button != null && IsInstanceValid(button) && button.Visible)
                {
                    // Alternar opacidad entre 1.0 y 0.5
                    var currentAlpha = button.Modulate.A;
                    button.Modulate = new Color(1.0f, 1.0f, 1.0f, currentAlpha > 0.75f ? 0.5f : 1.0f);
                }
            };
            blinkTimer.Autostart = true;
            AddChild(blinkTimer);
        }
        
        /// <summary>
        /// Obtiene el tamaño del viewport de forma segura
        /// </summary>
        protected Vector2 GetViewportSize()
        {
            var viewport = GetViewport();
            return viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
        }
        
        /// <summary>
        /// Calcula un tamaño responsive basado en porcentajes del viewport
        /// </summary>
        protected Vector2 GetResponsiveSize(float widthPercent, float heightPercent)
        {
            var viewportSize = GetViewportSize();
            return new Vector2(viewportSize.X * widthPercent, viewportSize.Y * heightPercent);
        }
        
        /// <summary>
        /// Calcula un margen responsive basado en porcentaje del viewport
        /// </summary>
        protected float GetResponsiveMargin(float percent)
        {
            var viewportSize = GetViewportSize();
            return Mathf.Max(viewportSize.X, viewportSize.Y) * percent;
        }
    }
}

