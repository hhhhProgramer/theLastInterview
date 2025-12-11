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
    }
}

