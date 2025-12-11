using Godot;
using Package.Audio;
using Package.Background;
using SlimeKingdomChronicles.Core.UI;

namespace TheLastInterview.Scenes
{
    /// <summary>
    /// Escena del menú principal
    /// </summary>
    public partial class MainMenu : Control
    {
        private Button _startButton;
        private Button _minigamesButton;
        private Button _exitButton;
        private Label _titleLabel;
        private Label _subtitleLabel;
        private SceneBackground _background;

        public override void _Ready()
        {
            base._Ready();

            // Configurar Control raíz
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            CustomMinimumSize = viewportSize;

            // Configurar background
            SetupBackground();

            // Crear UI
            CreateUI();
            
            // Reproducir música de fondo al iniciar
            PlayBackgroundMusic();
        }
        
        /// <summary>
        /// Reproduce la música de fondo del menú principal
        /// </summary>
        private void PlayBackgroundMusic()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMusic(MusicTrack.WeHaveTime, fadeIn: true);
                GD.Print("[MainMenu] Reproduciendo música: WeHaveTime");
            }
            else
            {
                GD.PrintErr("[MainMenu] AudioManager.Instance es null - no se puede reproducir música");
            }
        }

        /// <summary>
        /// Configura el background
        /// </summary>
        private void SetupBackground()
        {
            _background = new SceneBackground();
            _background.SetBackground("res://src/Image/Background/backgroun_office.png", new Color(0.05f, 0.05f, 0.05f, 1.0f));
            AddChild(_background);
            MoveChild(_background, 0);
        }

        /// <summary>
        /// Crea la UI del menú
        /// </summary>
        private void CreateUI()
        {
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);

            // Contenedor principal centrado usando CenterContainer para centrado perfecto
            var centerContainer = new CenterContainer();
            centerContainer.Name = "CenterContainer";
            centerContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            AddChild(centerContainer);
            
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.CustomMinimumSize = new Vector2(600, 400);
            mainContainer.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            mainContainer.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            centerContainer.AddChild(mainContainer);

            // Título
            _titleLabel = new Label();
            _titleLabel.Name = "TitleLabel";
            _titleLabel.Text = "The Last Job Interview Simulator";
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _titleLabel.VerticalAlignment = VerticalAlignment.Center;
            float titleFontSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.5f;
            _titleLabel.AddThemeFontSizeOverride("font_size", (int)titleFontSize);
            _titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.8f, 0.0f, 1.0f));
            mainContainer.AddChild(_titleLabel);

            // Subtítulo
            _subtitleLabel = new Label();
            _subtitleLabel.Name = "SubtitleLabel";
            _subtitleLabel.Text = "Sobrevive la entrevista... o no";
            _subtitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float subtitleFontSize = FontManager.GetScaledSize(TextType.Subtitle);
            _subtitleLabel.AddThemeFontSizeOverride("font_size", (int)subtitleFontSize);
            _subtitleLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.8f, 1.0f));
            mainContainer.AddChild(_subtitleLabel);

            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 50);
            mainContainer.AddChild(spacer);

            // Botón Iniciar
            _startButton = new Button();
            _startButton.Name = "StartButton";
            _startButton.Text = "Iniciar Entrevista";
            _startButton.CustomMinimumSize = new Vector2(300, 60);
            float buttonFontSize = FontManager.GetScaledSize(TextType.Body);
            _startButton.AddThemeFontSizeOverride("font_size", (int)buttonFontSize);
            _startButton.Pressed += OnStartButtonPressed;
            mainContainer.AddChild(_startButton);

            // Espaciador pequeño
            var spacer2 = new Control();
            spacer2.CustomMinimumSize = new Vector2(0, 20);
            mainContainer.AddChild(spacer2);

            // Botón Minijuegos
            _minigamesButton = new Button();
            _minigamesButton.Name = "MinigamesButton";
            _minigamesButton.Text = "Minijuegos";
            _minigamesButton.CustomMinimumSize = new Vector2(300, 60);
            _minigamesButton.AddThemeFontSizeOverride("font_size", (int)buttonFontSize);
            _minigamesButton.Pressed += OnMinigamesButtonPressed;
            mainContainer.AddChild(_minigamesButton);

            // Espaciador pequeño
            var spacer3 = new Control();
            spacer3.CustomMinimumSize = new Vector2(0, 20);
            mainContainer.AddChild(spacer3);

            // Botón Salir
            _exitButton = new Button();
            _exitButton.Name = "ExitButton";
            _exitButton.Text = "Salir";
            _exitButton.CustomMinimumSize = new Vector2(300, 60);
            _exitButton.AddThemeFontSizeOverride("font_size", (int)buttonFontSize);
            _exitButton.Pressed += OnExitButtonPressed;
            mainContainer.AddChild(_exitButton);
        }

        /// <summary>
        /// Se llama cuando se presiona el botón Iniciar
        /// </summary>
        private void OnStartButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://src/Scenes/InterviewScene.tscn");
        }

        /// <summary>
        /// Se llama cuando se presiona el botón Minijuegos
        /// </summary>
        private void OnMinigamesButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://src/Scenes/MinigamesMenu.tscn");
        }

        /// <summary>
        /// Se llama cuando se presiona el botón Salir
        /// </summary>
        private void OnExitButtonPressed()
        {
            GetTree().Quit();
        }
    }
}

