using Godot;
using Package.Background;
using SlimeKingdomChronicles.Core.UI;
using TheLastInterview.Interview.Models;
using TheLastInterview.Interview.Managers;

namespace TheLastInterview.Scenes
{
    /// <summary>
    /// Escena que muestra el ending del juego
    /// </summary>
    public partial class EndingScene : Control
    {
        private Label _titleLabel;
        private RichTextLabel _descriptionLabel;
        private Button _restartButton;
        private Button _exitButton;
        private SceneBackground _background;
        private Ending _currentEnding;

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

            // Obtener ending (por ahora, usar uno de prueba)
            // TODO: Pasar ending desde InterviewScene
            var stateManager = new StateManager();
            var endingManager = new EndingManager();
            _currentEnding = endingManager.DetermineEnding(stateManager.GameState);

            // Crear UI
            CreateUI();
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
        /// Crea la UI del ending
        /// </summary>
        private void CreateUI()
        {
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);

            // Contenedor principal centrado
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            mainContainer.CustomMinimumSize = new Vector2(800, 600);
            AddChild(mainContainer);

            // Título del ending
            _titleLabel = new Label();
            _titleLabel.Name = "TitleLabel";
            _titleLabel.Text = _currentEnding?.Title ?? "Fin";
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _titleLabel.VerticalAlignment = VerticalAlignment.Center;
            float titleFontSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.5f;
            _titleLabel.AddThemeFontSizeOverride("font_size", (int)titleFontSize);
            _titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.8f, 0.0f, 1.0f));
            mainContainer.AddChild(_titleLabel);

            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 50);
            mainContainer.AddChild(spacer);

            // Descripción del ending
            _descriptionLabel = new RichTextLabel();
            _descriptionLabel.Name = "DescriptionLabel";
            _descriptionLabel.Text = _currentEnding?.Description ?? "Fin del juego.";
            _descriptionLabel.BbcodeEnabled = true;
            _descriptionLabel.FitContent = true;
            float descFontSize = FontManager.GetScaledSize(TextType.Body);
            _descriptionLabel.AddThemeFontSizeOverride("normal_font_size", (int)descFontSize);
            _descriptionLabel.AddThemeColorOverride("default_color", new Color(1.0f, 1.0f, 1.0f, 1.0f));
            _descriptionLabel.CustomMinimumSize = new Vector2(700, 300);
            mainContainer.AddChild(_descriptionLabel);

            // Espaciador
            var spacer2 = new Control();
            spacer2.CustomMinimumSize = new Vector2(0, 50);
            mainContainer.AddChild(spacer2);

            // Botón Volver a empezar
            _restartButton = new Button();
            _restartButton.Name = "RestartButton";
            _restartButton.Text = "Volver a empezar";
            _restartButton.CustomMinimumSize = new Vector2(300, 60);
            float buttonFontSize = FontManager.GetScaledSize(TextType.Body);
            _restartButton.AddThemeFontSizeOverride("font_size", (int)buttonFontSize);
            _restartButton.Pressed += OnRestartButtonPressed;
            mainContainer.AddChild(_restartButton);

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
        /// Se llama cuando se presiona el botón Volver a empezar
        /// </summary>
        private void OnRestartButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://src/Scenes/InterviewScene.tscn");
        }

        /// <summary>
        /// Se llama cuando se presiona el botón Salir
        /// </summary>
        private void OnExitButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://src/Scenes/MainMenu.tscn");
        }
    }
}

