using Godot;
using TheLastInterview.Interview.Managers;
using TheLastInterview.Interview.Models;

namespace TheLastInterview.Scenes
{
    /// <summary>
    /// Escena principal de la entrevista
    /// </summary>
    public partial class InterviewScene : Control
    {
        private InterviewManager _interviewManager;

        public override void _Ready()
        {
            base._Ready();

            // Configurar Control raíz
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            CustomMinimumSize = viewportSize;

            // Crear InterviewManager
            _interviewManager = new InterviewManager();
            _interviewManager.Name = "InterviewManager";
            AddChild(_interviewManager);

            // Conectar señales
            _interviewManager.InterviewFinished += OnInterviewFinished;
            _interviewManager.InterviewStateChanged += OnInterviewStateChanged;
        }

        /// <summary>
        /// Se llama cuando la entrevista termina
        /// </summary>
        private void OnInterviewFinished(string endingId)
        {
            // Cambiar a escena de ending
            GetTree().ChangeSceneToFile("res://src/Scenes/EndingScene.tscn");
        }

        /// <summary>
        /// Se llama cuando cambia el estado de la entrevista
        /// </summary>
        private void OnInterviewStateChanged(InterviewState newState)
        {
            GD.Print($"[InterviewScene] Estado cambiado a: {newState}");
        }
    }
}

