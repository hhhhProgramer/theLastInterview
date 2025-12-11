using System;
using Godot;
using SlimeKingdomChronicles.Core.UI;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Detector de mentiras descompuesto - siempre da resultados aleatorios y cómicos
    /// </summary>
    public partial class LieDetectorMinigame : BaseMinigame
    {
        private ProgressBar _detectorBar;
        private Label _statusLabel;
        private Label _statementLabel;
        private Button _confirmButton;
        private Button _continueButton;
        private Timer _barTimer;
        private System.Random _random = new System.Random();
        private bool _hasConfirmed = false;
        
        private string[] _statements = {
            "Soy muy trabajador y puntual.",
            "Me encanta trabajar en equipo.",
            "Siempre cumplo con los plazos.",
            "Tengo mucha experiencia en este campo.",
            "Estoy muy motivado para este puesto."
        };
        
        private string[] _results = {
            "MENTIROSO detectado. Nivel de honestidad: 23%",
            "Aprobado absurdamente. Honestidad: 156% (¿cómo?)",
            "Evaluando... Error 404: Honestidad no encontrada",
            "Resultado: INCONCLUSOPS. El detector se confundió.",
            "Sistema dice: 'Probablemente humano, pero no estoy seguro.'",
            "Análisis completo: Eres 73% agua, 27% dudas existenciales.",
            "El detector detectó... ¡otro detector! Error recursivo."
        };
        
        public LieDetectorMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo
            var panel = new Panel();
            panel.Name = "MinigamePanel";
            panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            panel.CustomMinimumSize = new Vector2(700, 600);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -350;
            panel.OffsetRight = 350;
            panel.OffsetTop = -300;
            panel.OffsetBottom = 300;
            
            var styleBox = new StyleBoxFlat();
            styleBox.BgColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            styleBox.BorderColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            styleBox.BorderWidthLeft = 3;
            styleBox.BorderWidthTop = 3;
            styleBox.BorderWidthRight = 3;
            styleBox.BorderWidthBottom = 3;
            styleBox.CornerRadiusTopLeft = 10;
            styleBox.CornerRadiusTopRight = 10;
            styleBox.CornerRadiusBottomLeft = 10;
            styleBox.CornerRadiusBottomRight = 10;
            panel.AddThemeStyleboxOverride("panel", styleBox);
            AddChild(panel);
            
            // Contenedor principal con VBoxContainer
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            mainContainer.OffsetLeft = 30;
            mainContainer.OffsetRight = -30;
            mainContainer.OffsetTop = 20;
            mainContainer.OffsetBottom = -20;
            mainContainer.AddThemeConstantOverride("separation", 15);
            panel.AddChild(mainContainer);
            
            // Título
            var titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Text = "DETECTOR DE MENTIRAS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.2f, 0.2f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Afirmación del jugador
            _statementLabel = new Label();
            _statementLabel.Name = "StatementLabel";
            _statementLabel.Text = $"Afirmación: \"{_statements[_random.Next(_statements.Length)]}\"";
            _statementLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _statementLabel.VerticalAlignment = VerticalAlignment.Center;
            _statementLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _statementLabel.ClipContents = true;
            float statementSize = FontManager.GetScaledSize(TextType.Body);
            _statementLabel.AddThemeFontSizeOverride("font_size", (int)statementSize);
            _statementLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_statementLabel);
            
            // Contenedor para la barra (centrado)
            var barContainer = new CenterContainer();
            barContainer.Name = "BarContainer";
            barContainer.CustomMinimumSize = new Vector2(0, 60);
            mainContainer.AddChild(barContainer);
            
            // Barra del detector
            _detectorBar = new ProgressBar();
            _detectorBar.Name = "DetectorBar";
            _detectorBar.CustomMinimumSize = new Vector2(550, 50);
            _detectorBar.MinValue = 0;
            _detectorBar.MaxValue = 100;
            _detectorBar.Value = 0;
            barContainer.AddChild(_detectorBar);
            
            // Label de estado
            _statusLabel = new Label();
            _statusLabel.Name = "StatusLabel";
            _statusLabel.Text = "Presiona el botón para confirmar tu honestidad...";
            _statusLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _statusLabel.VerticalAlignment = VerticalAlignment.Center;
            _statusLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _statusLabel.ClipContents = true;
            _statusLabel.CustomMinimumSize = new Vector2(0, 100);
            _statusLabel.AddThemeFontSizeOverride("font_size", (int)statementSize);
            mainContainer.AddChild(_statusLabel);
            
            // Contenedor para botón confirmar (centrado)
            var buttonContainer = new CenterContainer();
            buttonContainer.Name = "ButtonContainer";
            buttonContainer.CustomMinimumSize = new Vector2(0, 60);
            mainContainer.AddChild(buttonContainer);
            
            // Botón confirmar honestidad
            _confirmButton = new Button();
            _confirmButton.Name = "ConfirmButton";
            _confirmButton.Text = "Confirmar Honestidad";
            _confirmButton.CustomMinimumSize = new Vector2(250, 50);
            _confirmButton.AddThemeFontSizeOverride("font_size", (int)statementSize);
            _confirmButton.Pressed += OnConfirmPressed;
            buttonContainer.AddChild(_confirmButton);
            
            // Espaciador
            var spacer = new Control();
            spacer.Name = "Spacer";
            spacer.CustomMinimumSize = new Vector2(0, 20);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            mainContainer.AddChild(spacer);
            
            // Botón continuar (oculto inicialmente)
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.CustomMinimumSize = new Vector2(200, 50);
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)statementSize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
            
            // Iniciar animación de la barra
            StartBarAnimation();
        }
        
        private void StartBarAnimation()
        {
            _barTimer = new Timer();
            _barTimer.WaitTime = 0.1f;
            _barTimer.Timeout += UpdateBar;
            _barTimer.Autostart = true;
            AddChild(_barTimer);
        }
        
        private void UpdateBar()
        {
            if (_hasConfirmed) return;
            
            // La barra sube y baja aleatoriamente
            float currentValue = (float)_detectorBar.Value;
            
            if (_random.Next(0, 2) == 0)
            {
                _detectorBar.Value = Mathf.Clamp(currentValue + _random.Next(-15, 20), 0, 100);
            }
            else
            {
                _detectorBar.Value = Mathf.Clamp(currentValue + _random.Next(-20, 15), 0, 100);
            }
        }
        
        private void OnConfirmPressed()
        {
            if (_hasConfirmed) return;
            
            _hasConfirmed = true;
            _confirmButton.Visible = false;
            _barTimer.Stop();
            
            // Resultado aleatorio y cómico
            string result = _results[_random.Next(_results.Length)];
            _statusLabel.Text = result;
            _statusLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.8f, 0.2f, 1.0f));
            
            // La barra se queda en un valor aleatorio
            _detectorBar.Value = _random.Next(0, 101);
            
            // Mostrar botón continuar después de un momento
            GetTree().CreateTimer(1.5f).Timeout += () => {
                _continueButton.Visible = true;
            };
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}
