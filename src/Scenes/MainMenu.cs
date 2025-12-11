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
            _background.SetBackground("res://src/Image/Background/backgroun_lobby.png", new Color(0.05f, 0.05f, 0.05f, 1.0f));
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
            mainContainer.CustomMinimumSize = new Vector2(700, 500);
            mainContainer.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            mainContainer.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            centerContainer.AddChild(mainContainer);

            // Título con efectos mejorados
            _titleLabel = new Label();
            _titleLabel.Name = "TitleLabel";
            _titleLabel.Text = "The Last Job Interview Simulator";
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _titleLabel.VerticalAlignment = VerticalAlignment.Center;
            float titleFontSize = FontManager.GetScaledSize(TextType.Subtitle) * 2.0f; // Más grande
            _titleLabel.AddThemeFontSizeOverride("font_size", (int)titleFontSize);
            
            // Color dorado brillante con gradiente
            _titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.85f, 0.3f, 1.0f));
            
            // Sombra pronunciada para efecto 3D
            _titleLabel.AddThemeColorOverride("font_shadow_color", new Color(0.0f, 0.0f, 0.0f, 0.9f));
            _titleLabel.AddThemeConstantOverride("shadow_offset_x", 4);
            _titleLabel.AddThemeConstantOverride("shadow_offset_y", 4);
            
            // Outline para mejor legibilidad
            _titleLabel.AddThemeColorOverride("font_outline_color", new Color(0.2f, 0.15f, 0.1f, 0.95f));
            _titleLabel.AddThemeConstantOverride("outline_size", 3);
            
            mainContainer.AddChild(_titleLabel);
            
            // Animación de pulso para el título
            StartTitlePulseAnimation();

            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 30);
            mainContainer.AddChild(spacer);

            // Subtítulo con efectos mejorados
            _subtitleLabel = new Label();
            _subtitleLabel.Name = "SubtitleLabel";
            _subtitleLabel.Text = "Sobrevive la entrevista... o no";
            _subtitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float subtitleFontSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.2f; // Más grande
            _subtitleLabel.AddThemeFontSizeOverride("font_size", (int)subtitleFontSize);
            
            // Color plateado elegante
            _subtitleLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.85f, 1.0f));
            
            // Sombra sutil
            _subtitleLabel.AddThemeColorOverride("font_shadow_color", new Color(0.0f, 0.0f, 0.0f, 0.7f));
            _subtitleLabel.AddThemeConstantOverride("shadow_offset_x", 2);
            _subtitleLabel.AddThemeConstantOverride("shadow_offset_y", 2);
            
            // Outline sutil
            _subtitleLabel.AddThemeColorOverride("font_outline_color", new Color(0.15f, 0.15f, 0.15f, 0.8f));
            _subtitleLabel.AddThemeConstantOverride("outline_size", 2);
            
            mainContainer.AddChild(_subtitleLabel);

            // Espaciador más grande
            var spacerLarge = new Control();
            spacerLarge.CustomMinimumSize = new Vector2(0, 60);
            mainContainer.AddChild(spacerLarge);

            // Botón Iniciar con imagen y efectos
            _startButton = CreateStyledButton("Iniciar Entrevista", new Vector2(400, 70));
            _startButton.Pressed += OnStartButtonPressed;
            mainContainer.AddChild(_startButton);

            // Espaciador entre botones
            var spacer2 = new Control();
            spacer2.CustomMinimumSize = new Vector2(0, 25);
            mainContainer.AddChild(spacer2);

            // Botón Minijuegos con imagen y efectos
            _minigamesButton = CreateStyledButton("Minijuegos", new Vector2(400, 70));
            _minigamesButton.Pressed += OnMinigamesButtonPressed;
            mainContainer.AddChild(_minigamesButton);

            // Espaciador entre botones
            var spacer3 = new Control();
            spacer3.CustomMinimumSize = new Vector2(0, 25);
            mainContainer.AddChild(spacer3);

            // Botón Salir con imagen y efectos
            _exitButton = CreateStyledButton("Salir", new Vector2(400, 70));
            _exitButton.Pressed += OnExitButtonPressed;
            mainContainer.AddChild(_exitButton);
        }

        /// <summary>
        /// Crea un botón estilizado con imagen de fondo y efectos visuales
        /// </summary>
        private Button CreateStyledButton(string text, Vector2 size)
        {
            var button = new Button();
            button.Name = $"Button_{text}";
            button.Text = text;
            button.CustomMinimumSize = size;
            button.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            
            // Fuente más grande y elegante
            float buttonFontSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.1f;
            button.AddThemeFontSizeOverride("font_size", (int)buttonFontSize);
            
            // Color de texto blanco brillante
            button.AddThemeColorOverride("font_color", new Color(0.98f, 0.98f, 0.95f, 1.0f));
            button.AddThemeColorOverride("font_hover_color", new Color(1.0f, 1.0f, 0.9f, 1.0f));
            button.AddThemeColorOverride("font_pressed_color", new Color(0.9f, 0.9f, 0.85f, 1.0f));
            
            // Sombra pronunciada para efecto 3D
            button.AddThemeColorOverride("font_shadow_color", new Color(0.0f, 0.0f, 0.0f, 0.85f));
            button.AddThemeConstantOverride("shadow_offset_x", 3);
            button.AddThemeConstantOverride("shadow_offset_y", 3);
            
            // Outline para mejor legibilidad
            button.AddThemeColorOverride("font_outline_color", new Color(0.2f, 0.2f, 0.2f, 0.9f));
            button.AddThemeConstantOverride("outline_size", 2);
            
            // Cargar imagen de fondo del botón
            const string BUTTON_IMAGE_PATH = "res://src/Image/Gui/button_option.png";
            var buttonTexture = GD.Load<Texture2D>(BUTTON_IMAGE_PATH);
            
            if (buttonTexture != null)
            {
                // Estilo normal con textura
                var normalStyle = new StyleBoxTexture();
                normalStyle.Texture = buttonTexture;
                normalStyle.ContentMarginLeft = 30;
                normalStyle.ContentMarginRight = 30;
                normalStyle.ContentMarginTop = 20;
                normalStyle.ContentMarginBottom = 20;
                button.AddThemeStyleboxOverride("normal", normalStyle);
                
                // Estilo hover - más brillante
                var hoverStyle = new StyleBoxTexture();
                hoverStyle.Texture = buttonTexture;
                hoverStyle.ContentMarginLeft = 30;
                hoverStyle.ContentMarginRight = 30;
                hoverStyle.ContentMarginTop = 20;
                hoverStyle.ContentMarginBottom = 20;
                button.AddThemeStyleboxOverride("hover", hoverStyle);
                
                // Estilo pressed - más oscuro
                var pressedStyle = new StyleBoxTexture();
                pressedStyle.Texture = buttonTexture;
                pressedStyle.ContentMarginLeft = 30;
                pressedStyle.ContentMarginRight = 30;
                pressedStyle.ContentMarginTop = 20;
                pressedStyle.ContentMarginBottom = 20;
                button.AddThemeStyleboxOverride("pressed", pressedStyle);
                
                // Aplicar modulación de color para efectos hover/pressed
                button.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                
                // Conectar señales para efectos visuales
                button.MouseEntered += () => OnButtonHoverEnter(button);
                button.MouseExited += () => OnButtonHoverExit(button);
            }
            else
            {
                // Fallback: StyleBoxFlat elegante si no se carga la imagen
                var normalStyle = new StyleBoxFlat();
                normalStyle.BgColor = new Color(0.2f, 0.2f, 0.2f, 0.95f);
                normalStyle.BorderColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                normalStyle.BorderWidthLeft = 3;
                normalStyle.BorderWidthTop = 3;
                normalStyle.BorderWidthRight = 3;
                normalStyle.BorderWidthBottom = 3;
                normalStyle.CornerRadiusTopLeft = 12;
                normalStyle.CornerRadiusTopRight = 12;
                normalStyle.CornerRadiusBottomLeft = 12;
                normalStyle.CornerRadiusBottomRight = 12;
                normalStyle.ContentMarginLeft = 30;
                normalStyle.ContentMarginRight = 30;
                normalStyle.ContentMarginTop = 20;
                normalStyle.ContentMarginBottom = 20;
                button.AddThemeStyleboxOverride("normal", normalStyle);
                
                // Hover - más brillante
                var hoverStyle = new StyleBoxFlat();
                hoverStyle.BgColor = new Color(0.3f, 0.3f, 0.3f, 0.95f);
                hoverStyle.BorderColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
                hoverStyle.BorderWidthLeft = 3;
                hoverStyle.BorderWidthTop = 3;
                hoverStyle.BorderWidthRight = 3;
                hoverStyle.BorderWidthBottom = 3;
                hoverStyle.CornerRadiusTopLeft = 12;
                hoverStyle.CornerRadiusTopRight = 12;
                hoverStyle.CornerRadiusBottomLeft = 12;
                hoverStyle.CornerRadiusBottomRight = 12;
                hoverStyle.ContentMarginLeft = 30;
                hoverStyle.ContentMarginRight = 30;
                hoverStyle.ContentMarginTop = 20;
                hoverStyle.ContentMarginBottom = 20;
                button.AddThemeStyleboxOverride("hover", hoverStyle);
                
                // Pressed - más oscuro
                var pressedStyle = new StyleBoxFlat();
                pressedStyle.BgColor = new Color(0.15f, 0.15f, 0.15f, 0.95f);
                pressedStyle.BorderColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);
                pressedStyle.BorderWidthLeft = 3;
                pressedStyle.BorderWidthTop = 3;
                pressedStyle.BorderWidthRight = 3;
                pressedStyle.BorderWidthBottom = 3;
                pressedStyle.CornerRadiusTopLeft = 12;
                pressedStyle.CornerRadiusTopRight = 12;
                pressedStyle.CornerRadiusBottomLeft = 12;
                pressedStyle.CornerRadiusBottomRight = 12;
                pressedStyle.ContentMarginLeft = 30;
                pressedStyle.ContentMarginRight = 30;
                pressedStyle.ContentMarginTop = 20;
                pressedStyle.ContentMarginBottom = 20;
                button.AddThemeStyleboxOverride("pressed", pressedStyle);
            }
            
            return button;
        }

        /// <summary>
        /// Efecto visual cuando el mouse entra en un botón
        /// </summary>
        private void OnButtonHoverEnter(Button button)
        {
            // Animación de escala y brillo
            var tween = CreateTween();
            tween.TweenProperty(button, "scale", new Vector2(1.05f, 1.05f), 0.2f);
            tween.Parallel().TweenProperty(button, "modulate", new Color(1.1f, 1.1f, 1.0f, 1.0f), 0.2f);
        }

        /// <summary>
        /// Efecto visual cuando el mouse sale de un botón
        /// </summary>
        private void OnButtonHoverExit(Button button)
        {
            // Volver a escala y color normal
            var tween = CreateTween();
            tween.TweenProperty(button, "scale", Vector2.One, 0.2f);
            tween.Parallel().TweenProperty(button, "modulate", new Color(1.0f, 1.0f, 1.0f, 1.0f), 0.2f);
        }

        /// <summary>
        /// Animación de pulso para el título
        /// </summary>
        private void StartTitlePulseAnimation()
        {
            if (_titleLabel == null) return;
            
            var tween = CreateTween();
            tween.SetLoops();
            tween.TweenProperty(_titleLabel, "modulate", new Color(1.0f, 0.9f, 0.4f, 1.0f), 2.0f);
            tween.TweenProperty(_titleLabel, "modulate", new Color(1.0f, 0.85f, 0.3f, 1.0f), 2.0f);
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

