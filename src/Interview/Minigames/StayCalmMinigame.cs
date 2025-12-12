using Godot;
using SlimeKingdomChronicles.Core.UI;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Mantén la calma - barra de estrés que sube sola
    /// </summary>
    public partial class StayCalmMinigame : BaseMinigame
    {
        private ProgressBar _stressBar;
        private Label _instructionLabel;
        private Label _resultLabel;
        private Button _relaxButton;
        private Button _continueButton;
        private Timer _stressTimer;
        private System.Random _random = new System.Random();
        private bool _hasPressedRelax = false;
        private int _pressCount = 0;
        
        private string[] _results = {
            "La barra explota → Entrevistador: 'Amo la pasión.'",
            "La barra se congela → 'Ah, eres demasiado frío para esta empresa.'",
            "La barra se derrite → 'Clara señal de liderazgo.'",
            "La barra desaparece → 'Interesante. Has alcanzado el nirvana corporativo.'",
            "La barra se multiplica → 'Excelente. Múltiples niveles de estrés demuestran versatilidad.'",
            "La barra se vuelve azul → 'El estrés azul es el más productivo. Aprobado.'",
            "La barra hace un loop → 'Has creado un ciclo infinito de productividad. Contratado.'"
        };
        
        public StayCalmMinigame(Node parent) : base(parent)
        {
        }
        
        public override void ShowMinigame()
        {
            Visible = true;
            CreateUI();
        }
        
        protected override void CreateUI()
        {
            // Obtener tamaño del viewport para hacer el panel responsive
            var viewportSize = GetViewportSize();
            
            // Calcular tamaño del panel como porcentaje del viewport (optimizado para HD 1920x1080)
            float panelWidthPercent = viewportSize.X < 1000 ? 0.90f : 0.70f;
            float panelHeightPercent = viewportSize.Y < 1000 ? 0.85f : 0.60f;
            Vector2 panelSize = GetResponsiveSize(panelWidthPercent, panelHeightPercent);
            
            // Panel de fondo
            var panel = new Panel();
            panel.Name = "MinigamePanel";
            panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            panel.CustomMinimumSize = panelSize;
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -panelSize.X * 0.5f;
            panel.OffsetRight = panelSize.X * 0.5f;
            panel.OffsetTop = -panelSize.Y * 0.5f;
            panel.OffsetBottom = panelSize.Y * 0.5f;
            
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
            
            // Contenedor principal con VBoxContainer para organizar verticalmente
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
            titleLabel.Text = "MANTÉN LA CALMA";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.5f, 0.2f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Instrucción
            _instructionLabel = new Label();
            _instructionLabel.Name = "InstructionLabel";
            _instructionLabel.Text = "Tu nivel de estrés está subiendo sin control.\nPresiona el botón para relajarte:";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _instructionLabel.VerticalAlignment = VerticalAlignment.Center;
            _instructionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _instructionLabel.ClipContents = true;
            float instructionSize = FontManager.GetScaledSize(TextType.Body);
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            mainContainer.AddChild(_instructionLabel);
            
            // Contenedor para la barra de estrés (centrado)
            var barContainer = new CenterContainer();
            barContainer.Name = "BarContainer";
            barContainer.CustomMinimumSize = new Vector2(0, 60);
            mainContainer.AddChild(barContainer);
            
            // Barra de estrés
            _stressBar = new ProgressBar();
            _stressBar.Name = "StressBar";
            _stressBar.CustomMinimumSize = new Vector2(550, 50);
            _stressBar.MinValue = 0;
            _stressBar.MaxValue = 100;
            _stressBar.Value = 0;
            barContainer.AddChild(_stressBar);
            
            // Contenedor para el botón relájate (centrado)
            var buttonContainer = new CenterContainer();
            buttonContainer.Name = "ButtonContainer";
            buttonContainer.CustomMinimumSize = new Vector2(0, 60);
            mainContainer.AddChild(buttonContainer);
            
            // Botón relájate
            _relaxButton = new Button();
            _relaxButton.Name = "RelaxButton";
            _relaxButton.Text = "Relájate";
            _relaxButton.CustomMinimumSize = new Vector2(200, 50);
            _relaxButton.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _relaxButton.Pressed += OnRelaxPressed;
            buttonContainer.AddChild(_relaxButton);
            
            // Label de resultado (con tamaño mínimo para que sea visible)
            _resultLabel = new Label();
            _resultLabel.Name = "ResultLabel";
            _resultLabel.Text = "";
            _resultLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _resultLabel.VerticalAlignment = VerticalAlignment.Center;
            _resultLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _resultLabel.ClipContents = true;
            _resultLabel.CustomMinimumSize = new Vector2(0, 120);
            float resultSize = FontManager.GetScaledSize(TextType.Body) * 1.1f;
            _resultLabel.AddThemeFontSizeOverride("font_size", (int)resultSize);
            _resultLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            mainContainer.AddChild(_resultLabel);
            
            // Espaciador para empujar el botón continuar hacia abajo
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
            _continueButton.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
            
            // Iniciar aumento de estrés
            StartStressIncrease();
        }
        
        private void StartStressIncrease()
        {
            _stressTimer = new Timer();
            _stressTimer.WaitTime = 0.2f;
            _stressTimer.Timeout += IncreaseStress;
            _stressTimer.Autostart = true;
            AddChild(_stressTimer);
        }
        
        private void IncreaseStress()
        {
            if (_hasPressedRelax) return;
            
            // La barra sube sola sin control (más rápido y caótico)
            int increase = _random.Next(4, 12);
            _stressBar.Value = Mathf.Min(_stressBar.Value + increase, 100);
            
            // Cambiar color de la barra según el nivel de estrés
            if (_stressBar.Value < 30)
            {
                _stressBar.Modulate = new Color(0.3f, 1.0f, 0.3f, 1.0f); // Verde (calmado)
            }
            else if (_stressBar.Value < 60)
            {
                _stressBar.Modulate = new Color(1.0f, 1.0f, 0.3f, 1.0f); // Amarillo (tenso)
            }
            else if (_stressBar.Value < 90)
            {
                _stressBar.Modulate = new Color(1.0f, 0.6f, 0.2f, 1.0f); // Naranja (estresado)
            }
            else
            {
                _stressBar.Modulate = new Color(1.0f, 0.2f, 0.2f, 1.0f); // Rojo (crítico)
            }
            
            // Efecto visual: la barra pulsa cuando está alta
            if (_stressBar.Value > 70)
            {
                var pulseTween = CreateTween();
                pulseTween.TweenProperty(_stressBar, "scale", new Vector2(1.02f, 1.0f), 0.1f);
                pulseTween.TweenProperty(_stressBar, "scale", Vector2.One, 0.1f);
            }
            
            // Si llega al máximo, automáticamente muestra resultado
            if (_stressBar.Value >= 100)
            {
                ShowRandomResult();
            }
        }
        
        private void OnRelaxPressed()
        {
            if (_hasPressedRelax) return;
            
            _pressCount++;
            _hasPressedRelax = true;
            _stressTimer.Stop();
            
            // Resultado aleatorio y cómico
            ShowRandomResult();
        }
        
        private void ShowRandomResult()
        {
            string result = _results[_random.Next(_results.Length)];
            _resultLabel.Text = result;
            _relaxButton.Visible = false;
            
            // Efecto visual aleatorio en la barra
            int effect = _random.Next(0, 3);
            switch (effect)
            {
                case 0: // Explota
                    _stressBar.Value = 100;
                    break;
                case 1: // Se congela
                    _stressBar.Value = _random.Next(30, 70);
                    break;
                case 2: // Se derrite
                    _stressBar.Value = _random.Next(0, 30);
                    break;
            }
            
            GetTree().CreateTimer(2.0f).Timeout += () => {
                _continueButton.Visible = true;
                StartButtonBlink(_continueButton); // Hacer parpadear el botón continuar
            };
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

