using Godot;
using Package.UI;
using Package.Background;
using TheLastInterview.Interview.Models;
using TheLastInterview.Interview.Minigames;
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
        private BaseMinigame _currentMinigame;
        private System.Random _minigameRandom = new System.Random();
        private System.Random _eventRandom = new System.Random();
        private HashSet<MinigameManager.MinigameType> _usedMinigames = new HashSet<MinigameManager.MinigameType>();
        private int _questionsShown = 0; // Contador de preguntas mostradas
        private const int MIN_QUESTIONS_BEFORE_MINIGAME = 3; // Mínimo de preguntas antes de permitir minijuegos
        private const float EVENT_PROBABILITY = 0.18f; // 18% de probabilidad de evento meta-oficina
        private const float INTERRUPTION_PROBABILITY = 0.15f; // 15% de probabilidad de interrupción
        private bool _rumorShown = false; // Flag para mostrar el rumor solo una vez

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

            // Conectar señal de DialogFinished para detectar cuando termina un diálogo
            if (DialogSystem.Instance != null)
            {
                DialogSystem.Instance.DialogFinished += OnDialogFinished;
            }

            // Iniciar la entrevista
            StartInterview();
        }

        /// <summary>
        /// Flag para indicar que estamos esperando que termine una reacción
        /// </summary>
        private bool _waitingForReactionToFinish = false;
        
        /// <summary>
        /// Flag para indicar que estamos esperando que termine una consecuencia visual
        /// </summary>
        private bool _waitingForVisualConsequence = false;
        
        /// <summary>
        /// Última respuesta seleccionada (para mostrar consecuencias visuales)
        /// </summary>
        private Answer _lastSelectedAnswer = null;
        
        /// <summary>
        /// Se llama cuando termina un diálogo
        /// </summary>
        private void OnDialogFinished()
        {
            // Si estamos esperando que termine una reacción, mostrar consecuencia visual o continuar
            if (_waitingForReactionToFinish)
            {
                _waitingForReactionToFinish = false;
                
                // Verificar si hay consecuencia visual que mostrar
                if (_lastSelectedAnswer != null && !string.IsNullOrEmpty(_lastSelectedAnswer.VisualConsequenceText))
                {
                    _waitingForVisualConsequence = true;
                    ShowVisualConsequence(_lastSelectedAnswer.VisualConsequenceText);
                    _lastSelectedAnswer = null; // Limpiar después de usar
                    return;
                }
                
                _lastSelectedAnswer = null; // Limpiar
                GD.Print("[InterviewManager] Reacción terminada, continuando con siguiente pregunta");
                ShowNextQuestion();
            }
            // Si estamos esperando que termine una consecuencia visual, continuar
            else if (_waitingForVisualConsequence)
            {
                _waitingForVisualConsequence = false;
                GD.Print("[InterviewManager] Consecuencia visual terminada, continuando con siguiente pregunta");
                ShowNextQuestion();
            }
        }
        

        /// <summary>
        /// Configura el background de la oficina
        /// Siguiendo el patrón de HallwaysScene para consistencia
        /// </summary>
        private void SetupBackground()
        {
            // Crear CanvasLayer para background (detrás de todo)
            var backgroundCanvasLayer = new CanvasLayer();
            backgroundCanvasLayer.Name = "BackgroundCanvasLayer";
            backgroundCanvasLayer.Layer = -1; // Detrás de todo
            AddChild(backgroundCanvasLayer);
            
            // Crear Control contenedor dentro del CanvasLayer
            var backgroundContainer = new Control();
            backgroundContainer.Name = "BackgroundContainer";
            backgroundContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            
            // CRÍTICO: No bloquear clicks - permitir que pasen a través
            backgroundContainer.MouseFilter = Control.MouseFilterEnum.Ignore;
            
            // Obtener tamaño del viewport y establecer tamaño mínimo
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            backgroundContainer.CustomMinimumSize = viewportSize;
            
            backgroundCanvasLayer.AddChild(backgroundContainer);
            
            // Crear SceneBackground usando package
            _background = new SceneBackground();
            // Usar el fondo de oficina
            _background.SetBackground("res://src/Image/Background/backgroun_lobby.png", new Color(0.1f, 0.1f, 0.1f, 1.0f));
            backgroundContainer.AddChild(_background);
            _background.ChangeBackgroundWithFade("res://src/Image/Background/backgroun_office.png", 1.0f, () => {
                GD.Print("[InterviewManager] ✅ Background de oficina configurado");
            });
            
            GD.Print("[InterviewManager] ✅ Background de oficina configurado");
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
            _usedMinigames.Clear(); // Limpiar minijuegos usados al iniciar nueva partida
            _questionsShown = 0; // Resetear contador de preguntas
            _rumorShown = false; // Resetear flag de rumor
            
            // Seleccionar rumor aleatorio para esta partida
            var rumor = OfficeRumorManager.GetRandomRumor();
            if (rumor != null)
            {
                _stateManager.GameState.ActiveRumor = rumor;
            }
            
            // Mostrar el rumor al inicio
            ShowRumorIfAvailable();
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

            // Verificar si mostrar un evento meta-oficina aleatorio (18% de probabilidad)
            if (_eventRandom.NextDouble() < EVENT_PROBABILITY)
            {
                ShowRandomOfficeEvent();
                return; // El evento mostrará la pregunta después
            }

            // Solo permitir minijuegos si ya se han mostrado al menos 3 preguntas
            bool canShowMinigame = _questionsShown >= MIN_QUESTIONS_BEFORE_MINIGAME;
            
            // 30% de probabilidad de mostrar un minijuego antes de la pregunta (solo si se cumplen las condiciones)
            if (canShowMinigame && _minigameRandom.Next(0, 10) < 3)
            {
                ShowRandomMinigame();
            }
            else
            {
                // Verificar si mostrar una interrupción incómoda (15% de probabilidad)
                if (_eventRandom.NextDouble() < INTERRUPTION_PROBABILITY)
                {
                    ShowRandomInterruption();
                    return; // La interrupción mostrará la pregunta después
                }
                
                // Convertir pregunta a DialogEntry y mostrar directamente
                ShowQuestionAsDialog(_currentQuestion);
            }
        }
        
        /// <summary>
        /// Muestra un minijuego aleatorio que no haya sido usado antes en esta partida
        /// Si todos los minijuegos ya se usaron, muestra la pregunta directamente sin minijuego
        /// </summary>
        private void ShowRandomMinigame()
        {
            // Obtener todos los tipos de minijuegos disponibles
            var allMinigameTypes = new List<MinigameManager.MinigameType>
            {
                MinigameManager.MinigameType.LieDetector,
                MinigameManager.MinigameType.TypeName,
                MinigameManager.MinigameType.OrganizeDocuments,
                MinigameManager.MinigameType.TechnicalTest,
                MinigameManager.MinigameType.StayCalm
            };
            
            // Filtrar solo los que no se han usado
            var availableMinigames = new List<MinigameManager.MinigameType>();
            foreach (var type in allMinigameTypes)
            {
                if (!_usedMinigames.Contains(type))
                {
                    availableMinigames.Add(type);
                }
            }
            
            // Si todos los minijuegos ya se usaron, mostrar la pregunta directamente sin minijuego
            if (availableMinigames.Count == 0)
            {
                GD.Print("[InterviewManager] Todos los minijuegos ya se usaron en esta partida, mostrando pregunta directamente");
                ShowQuestionAsDialog(_currentQuestion);
                return;
            }
            
            // Seleccionar uno aleatorio de los disponibles
            var minigameType = availableMinigames[_minigameRandom.Next(availableMinigames.Count)];
            
            // Marcar como usado
            _usedMinigames.Add(minigameType);
            
            _currentMinigame = MinigameManager.CreateMinigame(minigameType, this);
            _currentMinigame.OnMinigameFinished += OnMinigameFinished;
            
            // Crear CanvasLayer para el minijuego (por encima del diálogo)
            var minigameLayer = new CanvasLayer();
            minigameLayer.Name = "MinigameLayer";
            minigameLayer.Layer = 3000; // Por encima del DialogBox (que está en 1000)
            AddChild(minigameLayer);
            
            // Agregar el minijuego al layer
            minigameLayer.AddChild(_currentMinigame);
            
            // Mostrar el minijuego
            _currentMinigame.ShowMinigame();
        }
        
        /// <summary>
        /// Se llama cuando termina un minijuego
        /// </summary>
        private void OnMinigameFinished()
        {
            if (_currentMinigame != null && IsInstanceValid(_currentMinigame))
            {
                _currentMinigame.OnMinigameFinished -= OnMinigameFinished;
                
                // Limpiar el CanvasLayer del minijuego
                var minigameLayer = GetNodeOrNull<CanvasLayer>("MinigameLayer");
                if (minigameLayer != null)
                {
                    minigameLayer.QueueFree();
                }
                
                _currentMinigame = null;
            }
            
            // Continuar con la pregunta después del minijuego
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

            // Incrementar contador de preguntas mostradas
            _questionsShown++;

            // Crear opciones de diálogo desde las respuestas
            var dialogOptions = new List<DialogOption>();
            for (int i = 0; i < question.Answers.Count; i++)
            {
                var answer = question.Answers[i];
                int answerIndex = i; // Capturar índice para el closure

                // Crear opción de diálogo
                var dialogOption = new DialogOption(
                    answer.Text,
                    () => {
                        // Aplicar respuesta inmediatamente cuando se selecciona
                        ProcessAnswer(answerIndex);
                        
                        // Si hay reacción, mostrarla y esperar a que termine
                        if (!string.IsNullOrEmpty(answer.ReactionText))
                        {
                            _waitingForReactionToFinish = true;
                            ShowInterviewerReaction(answer.ReactionText);
                        }
                        else if (!string.IsNullOrEmpty(answer.VisualConsequenceText))
                        {
                            // Si no hay reacción pero hay consecuencia visual, mostrarla
                            _waitingForVisualConsequence = true;
                            ShowVisualConsequence(answer.VisualConsequenceText);
                        }
                        else
                        {
                            // Si no hay reacción ni consecuencia, continuar inmediatamente con siguiente pregunta
                            ShowNextQuestion();
                        }
                    },
                    null // No usar NextDialogs - manejamos el flujo manualmente
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
        /// Procesa una respuesta seleccionada (aplica puntos y marca como respondida)
        /// </summary>
        private void ProcessAnswer(int answerIndex)
        {
            if (_currentQuestion == null || answerIndex < 0 || answerIndex >= _currentQuestion.Answers.Count)
            {
                GD.PrintErr("[InterviewManager] Índice de respuesta inválido");
                return;
            }

            var selectedAnswer = _currentQuestion.Answers[answerIndex];
            _lastSelectedAnswer = selectedAnswer; // Guardar para mostrar consecuencias visuales

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

            // La señal DialogFinished se conectará automáticamente y continuará con la siguiente pregunta
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
        /// Muestra un evento meta-oficina aleatorio
        /// </summary>
        private void ShowRandomOfficeEvent()
        {
            var officeEvent = OfficeEventManager.GetRandomEvent();
            if (officeEvent == null || DialogSystem.Instance == null)
            {
                ShowQuestionAsDialog(_currentQuestion);
                return;
            }

            var eventEntry = new DialogEntry(
                officeEvent.Text,
                "Oficina",
                null,
                null,
                null,
                null
            );

            // Después del evento, mostrar la pregunta
            var dialogList = new List<DialogEntry> { eventEntry };
            DialogSystem.Instance.StartDialog(dialogList);
            
            // Conectar para mostrar la pregunta después del evento
            DialogSystem.Instance.DialogFinished += OnOfficeEventFinished;
        }

        /// <summary>
        /// Se llama cuando termina un evento meta-oficina
        /// </summary>
        private void OnOfficeEventFinished()
        {
            DialogSystem.Instance.DialogFinished -= OnOfficeEventFinished;
            ShowQuestionAsDialog(_currentQuestion);
        }

        /// <summary>
        /// Muestra una interrupción incómoda aleatoria
        /// </summary>
        private void ShowRandomInterruption()
        {
            var interruption = OfficeInterruptionManager.GetRandomInterruption();
            if (interruption == null || DialogSystem.Instance == null)
            {
                ShowQuestionAsDialog(_currentQuestion);
                return;
            }

            var interruptionEntry = new DialogEntry(
                interruption.Text,
                interruption.Source,
                null,
                null,
                null,
                null
            );

            // Después de la interrupción, mostrar la pregunta
            var dialogList = new List<DialogEntry> { interruptionEntry };
            DialogSystem.Instance.StartDialog(dialogList);
            
            // Conectar para mostrar la pregunta después de la interrupción
            DialogSystem.Instance.DialogFinished += OnInterruptionFinished;
        }

        /// <summary>
        /// Se llama cuando termina una interrupción
        /// </summary>
        private void OnInterruptionFinished()
        {
            DialogSystem.Instance.DialogFinished -= OnInterruptionFinished;
            ShowQuestionAsDialog(_currentQuestion);
        }

        /// <summary>
        /// Muestra el rumor de la oficina al inicio de la partida
        /// </summary>
        private void ShowRumorIfAvailable()
        {
            if (_rumorShown || _stateManager.GameState.ActiveRumor == null || DialogSystem.Instance == null)
            {
                // Si ya se mostró o no hay rumor, continuar con la primera pregunta
                if (!_rumorShown)
                {
                    ShowNextQuestion();
                }
                return;
            }

            _rumorShown = true;

            var rumorEntry = new DialogEntry(
                _stateManager.GameState.ActiveRumor.Text,
                "Rumor",
                null,
                null,
                null,
                null
            );

            // Después del rumor, mostrar la primera pregunta
            var dialogList = new List<DialogEntry> { rumorEntry };
            DialogSystem.Instance.StartDialog(dialogList);
            
            // Conectar para mostrar la primera pregunta después del rumor
            DialogSystem.Instance.DialogFinished += OnRumorFinished;
        }

        /// <summary>
        /// Se llama cuando termina el rumor
        /// </summary>
        private void OnRumorFinished()
        {
            DialogSystem.Instance.DialogFinished -= OnRumorFinished;
            ShowNextQuestion();
        }

        /// <summary>
        /// Muestra una consecuencia visual mínima (solo texto descriptivo)
        /// </summary>
        private void ShowVisualConsequence(string consequenceText)
        {
            if (string.IsNullOrEmpty(consequenceText) || DialogSystem.Instance == null)
            {
                ShowNextQuestion();
                return;
            }

            var consequenceEntry = new DialogEntry(
                consequenceText,
                "Sistema",
                null,
                null,
                null,
                null
            );

            // Después de la consecuencia, continuar con la siguiente pregunta
            var dialogList = new List<DialogEntry> { consequenceEntry };
            DialogSystem.Instance.StartDialog(dialogList);
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

