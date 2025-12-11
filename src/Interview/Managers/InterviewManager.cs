using Godot;
using Package.UI;
using Package.Background;
using TheLastInterview.Interview.Models;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Managers
{
    /// <summary>
    /// Manager principal que controla el flujo completo de la entrevista
    /// </summary>
    public partial class InterviewManager : Node
    {
        private StateManager _stateManager;
        private QuestionSystem _questionSystem;
        private EndingManager _endingManager;
        private Question _currentQuestion;
        private SceneBackground _background;
        private Control _interviewerVisual;

        /// <summary>
        /// Señal que se emite cuando la entrevista termina
        /// </summary>
        [Signal]
        public delegate void InterviewFinishedEventHandler(string endingId);

        /// <summary>
        /// Señal que se emite cuando cambia el estado de la entrevista
        /// </summary>
        [Signal]
        public delegate void InterviewStateChangedEventHandler(InterviewState newState);

        public override void _Ready()
        {
            base._Ready();
            
            // Inicializar managers
            _stateManager = new StateManager();
            _questionSystem = new QuestionSystem(_stateManager);
            _endingManager = new EndingManager();

            // Configurar background
            SetupBackground();

            // Configurar entrevistador visual
            SetupInterviewer();

            // Iniciar la entrevista
            StartInterview();
        }

        /// <summary>
        /// Configura el background de la oficina
        /// </summary>
        private void SetupBackground()
        {
            var canvasLayer = new CanvasLayer();
            canvasLayer.Layer = -1; // Detrás de todo
            AddChild(canvasLayer);

            _background = new SceneBackground();
            _background.SetBackground("res://src/Image/Background/backgroun_office.png", new Color(0.1f, 0.1f, 0.1f, 1.0f));
            canvasLayer.AddChild(_background);
        }

        /// <summary>
        /// Configura el visual del entrevistador (silueta con ojos)
        /// </summary>
        private void SetupInterviewer()
        {
            var canvasLayer = new CanvasLayer();
            canvasLayer.Layer = 0;
            AddChild(canvasLayer);

            _interviewerVisual = new Control();
            _interviewerVisual.Name = "InterviewerVisual";
            _interviewerVisual.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            
            // TODO: Agregar sprite del entrevistador (silueta con ojos)
            // Por ahora, solo creamos el contenedor
            
            canvasLayer.AddChild(_interviewerVisual);
        }

        /// <summary>
        /// Inicia la entrevista
        /// </summary>
        public void StartInterview()
        {
            _stateManager.Reset();
            ShowNextQuestion();
        }

        /// <summary>
        /// Muestra la siguiente pregunta
        /// </summary>
        private void ShowNextQuestion()
        {
            _currentQuestion = _questionSystem.GetNextQuestion();

            if (_currentQuestion == null)
            {
                // No hay más preguntas, terminar entrevista
                EndInterview();
                return;
            }

            // Convertir pregunta a DialogEntry y mostrar
            ShowQuestionAsDialog(_currentQuestion);
        }

        /// <summary>
        /// Muestra una pregunta usando el DialogSystem
        /// </summary>
        private void ShowQuestionAsDialog(Question question)
        {
            if (DialogSystem.Instance == null)
            {
                GD.PrintErr("[InterviewManager] DialogSystem.Instance es null");
                return;
            }

            // Crear opciones de diálogo desde las respuestas
            var dialogOptions = new List<DialogOption>();
            for (int i = 0; i < question.Answers.Count; i++)
            {
                var answer = question.Answers[i];
                int answerIndex = i; // Capturar índice para el closure

                var dialogOption = new DialogOption(
                    answer.Text,
                    () => OnAnswerSelected(answerIndex),
                    null
                );
                dialogOptions.Add(dialogOption);
            }

            // Crear entrada de diálogo
            var dialogEntry = new DialogEntry(
                question.Text,
                "Entrevistador", // CharacterId
                null, // Emotion
                null, // Position
                dialogOptions,
                null  // OnShow
            );
            // Asegurar que solo use opciones normales (no verdad/mentira ni tiempo limitado)
            dialogEntry.IsTruthLieDecision = false;
            dialogEntry.IsTimedDecision = false;

            // Mostrar diálogo
            var dialogList = new List<DialogEntry> { dialogEntry };
            DialogSystem.Instance.StartDialog(dialogList);
        }

        /// <summary>
        /// Se llama cuando el jugador selecciona una respuesta
        /// </summary>
        private void OnAnswerSelected(int answerIndex)
        {
            if (_currentQuestion == null || answerIndex < 0 || answerIndex >= _currentQuestion.Answers.Count)
            {
                GD.PrintErr("[InterviewManager] Índice de respuesta inválido");
                return;
            }

            var selectedAnswer = _currentQuestion.Answers[answerIndex];

            // Aplicar respuesta al estado
            _stateManager.ApplyAnswer(selectedAnswer);
            _stateManager.MarkQuestionAnswered(_currentQuestion.Id);

            // Verificar cambio de estado
            var previousState = _stateManager.GameState.CurrentState;
            var newState = _stateManager.GameState.CurrentState;
            if (previousState != newState)
            {
                EmitSignal(SignalName.InterviewStateChanged, (int)newState);
                OnStateChanged(newState);
            }

            // Mostrar reacción del entrevistador si hay
            if (!string.IsNullOrEmpty(selectedAnswer.ReactionText))
            {
                ShowInterviewerReaction(selectedAnswer.ReactionText);
            }

            // Esperar un momento y mostrar siguiente pregunta
            GetTree().CreateTimer(1.5f).Timeout += () => ShowNextQuestion();
        }

        /// <summary>
        /// Muestra la reacción del entrevistador
        /// </summary>
        private void ShowInterviewerReaction(string reactionText)
        {
            if (DialogSystem.Instance == null) return;

            var reactionEntry = new DialogEntry(
                reactionText,
                "Entrevistador",
                null,
                null,
                null,
                null
            );

            var dialogList = new List<DialogEntry> { reactionEntry };
            DialogSystem.Instance.StartDialog(dialogList);
        }

        /// <summary>
        /// Se llama cuando cambia el estado de la entrevista
        /// </summary>
        private void OnStateChanged(InterviewState newState)
        {
            // Efectos visuales según el estado
            switch (newState)
            {
                case InterviewState.Tense:
                    // Efectos de tensión (parpadeo del monitor, etc.)
                    break;
                case InterviewState.Chaos:
                    // Efectos de caos (shake, efectos visuales)
                    ApplyChaosEffects();
                    break;
            }
        }

        /// <summary>
        /// Aplica efectos visuales de caos
        /// </summary>
        private void ApplyChaosEffects()
        {
            // Shake de cámara
            var camera = GetViewport().GetCamera2D();
            if (camera != null)
            {
                // TODO: Implementar shake de cámara
            }

            // Efecto de pulso en el background
            if (_background != null)
            {
                _background.StartPulseEffect(0.1f, 0.6f, 0.05f);
            }
        }

        /// <summary>
        /// Termina la entrevista y determina el ending
        /// </summary>
        private void EndInterview()
        {
            var ending = _endingManager.DetermineEnding(_stateManager.GameState);
            EmitSignal(SignalName.InterviewFinished, ending.Id);
        }
    }
}

