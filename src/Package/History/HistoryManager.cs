using Godot;

namespace SlimeKingdomChronicles.src.Core.History
{
    /// <summary>
    /// Manager que controla la reproducción de capítulos de historia con animaciones de texto e imágenes
    /// </summary>
    public partial class HistoryManager : Node
    {
        [Signal]
        public delegate void ChapterStartedEventHandler(string chapterName);
        
        [Signal]
        public delegate void SceneChangedEventHandler(string title, string imagePath);
        
        [Signal]
        public delegate void TextProgressEventHandler(string currentText);
        
        [Signal]
        public delegate void ChapterCompletedEventHandler();
        
        [Signal]
        public delegate void HistoryCompletedEventHandler();
        
        private IChapter _currentChapter;
        private int _currentSceneIndex;
        private ChapterScene _currentScene;
        private Timer _textTimer;
        private string _displayedText = string.Empty;
        private int _textCharacterIndex = 0;
        private bool _isPlaying = false;
        private bool _isTextComplete = false;
        
        /// <summary>
        /// Capítulo actual que se está reproduciendo
        /// </summary>
        public IChapter CurrentChapter => _currentChapter;
        
        /// <summary>
        /// Índice de la escena actual
        /// </summary>
        public int CurrentSceneIndex => _currentSceneIndex;
        
        /// <summary>
        /// Indica si la historia se está reproduciendo actualmente
        /// </summary>
        public bool IsPlaying => _isPlaying;
        
        /// <summary>
        /// Indica si el texto de la escena actual se ha completado
        /// </summary>
        public bool IsTextComplete => _isTextComplete;
        
        /// <summary>
        /// Texto actualmente mostrado en pantalla
        /// </summary>
        public string DisplayedText => _displayedText;
        
        public override void _Ready()
        {
            SetupTimers();
        }
        
        /// <summary>
        /// Inicia la reproducción de un capítulo específico
        /// </summary>
        /// <param name="chapter">Capítulo a reproducir</param>
        public void PlayChapter(IChapter chapter)
        {
            if (_isPlaying)
            {
                StopHistory();
            }
            
            _currentChapter = chapter;
            _currentSceneIndex = 0;
            _isPlaying = true;
            _isTextComplete = false;
            
            LoadCurrentScene();
            EmitSignal(SignalName.ChapterStarted, chapter.ChapterName);
        }
        
        /// <summary>
        /// Continúa con la siguiente escena del capítulo actual
        /// </summary>
        public void NextScene()
        {
            if (_currentChapter == null || !_isPlaying)
                return;
                
            if (!_currentChapter.HasNextScene(_currentSceneIndex))
            {
                CompleteChapter();
                return;
            }
            
            _currentSceneIndex++;
            LoadCurrentScene();
        }
        
        /// <summary>
        /// Avanza automáticamente al siguiente texto o escena
        /// </summary>
        public void Advance()
        {
            if (!_isPlaying)
                return;
                
            if (!_isTextComplete)
            {
                CompleteCurrentText();
            }
            else
            {
                NextScene();
            }
        }
        
        /// <summary>
        /// Detiene la reproducción de la historia
        /// </summary>
        public void StopHistory()
        {
            _isPlaying = false;
            _isTextComplete = false;
            _textTimer?.Stop();
            _displayedText = string.Empty;
            _textCharacterIndex = 0;
        }
        
        /// <summary>
        /// Reinicia el capítulo actual desde el principio
        /// </summary>
        public void RestartChapter()
        {
            if (_currentChapter == null)
                return;
                
            PlayChapter(_currentChapter);
        }
        
        /// <summary>
        /// Configura los timers necesarios para la animación de texto
        /// </summary>
        private void SetupTimers()
        {
            _textTimer = new Timer();
            _textTimer.OneShot = false;
            _textTimer.Timeout += OnTextTimerTimeout;
            AddChild(_textTimer);
        }
        
        /// <summary>
        /// Carga la escena actual y comienza su reproducción
        /// </summary>
        private void LoadCurrentScene()
        {
            if (_currentChapter == null)
                return;
                
            _currentScene = _currentChapter.GetScene(_currentSceneIndex);
            if (_currentScene == null)
            {
                CompleteChapter();
                return;
            }
            
            _displayedText = string.Empty;
            _textCharacterIndex = 0;
            _isTextComplete = false;
            
            EmitSignal(SignalName.SceneChanged, _currentScene.Title, _currentScene.ImagePath);
            StartTextAnimation();
        }
        
        /// <summary>
        /// Inicia la animación de escritura del texto
        /// </summary>
        private void StartTextAnimation()
        {
            if (_currentScene == null)
                return;
                
            _textTimer?.Stop();
            _textTimer?.Start(1.0f / _currentScene.TextSpeed);
        }
        
        /// <summary>
        /// Maneja el evento de timeout del timer de texto
        /// </summary>
        private void OnTextTimerTimeout()
        {
            if (_currentScene == null || _isTextComplete)
                return;
                
            if (_textCharacterIndex < _currentScene.StoryText.Length)
            {
                _displayedText += _currentScene.StoryText[_textCharacterIndex];
                _textCharacterIndex++;
                EmitSignal(SignalName.TextProgress, _displayedText);
            }
            else
            {
                CompleteCurrentText();
            }
        }
        
        /// <summary>
        /// Completa el texto actual y espera a que el usuario presione siguiente
        /// </summary>
        private void CompleteCurrentText()
        {
            if (_currentScene == null)
                return;
                
            _textTimer?.Stop();
            _isTextComplete = true;
            _displayedText = _currentScene.StoryText;
            EmitSignal(SignalName.TextProgress, _displayedText);
            
            // No cambiar automáticamente, esperar a que el usuario presione siguiente
        }
        
        
        /// <summary>
        /// Completa el capítulo actual y emite las señales correspondientes
        /// </summary>
        private void CompleteChapter()
        {
            _isPlaying = false;
            EmitSignal(SignalName.ChapterCompleted);
            
            // Por ahora solo tenemos un capítulo, cuando haya más se puede implementar lógica adicional
            EmitSignal(SignalName.HistoryCompleted);
        }
    }
}
