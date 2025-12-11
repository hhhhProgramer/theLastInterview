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
        
        protected override void CreateUI()
        {
            // Panel de fondo (más alto para que quepa el texto del resultado)
            var panel = new Panel();
            panel.Name = "MinigamePanel";
            panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            panel.CustomMinimumSize = new Vector2(700, 550);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -350;
            panel.OffsetRight = 350;
            panel.OffsetTop = -275;
            panel.OffsetBottom = 275;
            
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
            titleLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            titleLabel.OffsetTop = 20;
            titleLabel.OffsetBottom = 60;
            titleLabel.OffsetLeft = 20;
            titleLabel.OffsetRight = -20;
            panel.AddChild(titleLabel);
            
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
            _instructionLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _instructionLabel.OffsetTop = 70;
            _instructionLabel.OffsetBottom = 130;
            _instructionLabel.OffsetLeft = 30;
            _instructionLabel.OffsetRight = -30;
            panel.AddChild(_instructionLabel);
            
            // Barra de estrés
            _stressBar = new ProgressBar();
            _stressBar.Name = "StressBar";
            _stressBar.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _stressBar.CustomMinimumSize = new Vector2(550, 50);
            _stressBar.OffsetLeft = -275;
            _stressBar.OffsetRight = 275;
            _stressBar.OffsetTop = -30;
            _stressBar.OffsetBottom = 20;
            _stressBar.MinValue = 0;
            _stressBar.MaxValue = 100;
            _stressBar.Value = 0;
            panel.AddChild(_stressBar);
            
            // Botón relájate (ajustado para dejar espacio al resultado)
            _relaxButton = new Button();
            _relaxButton.Name = "RelaxButton";
            _relaxButton.Text = "Relájate";
            _relaxButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _relaxButton.CustomMinimumSize = new Vector2(200, 50);
            _relaxButton.OffsetLeft = -100;
            _relaxButton.OffsetRight = 100;
            _relaxButton.OffsetTop = 30;
            _relaxButton.OffsetBottom = 80;
            _relaxButton.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _relaxButton.Pressed += OnRelaxPressed;
            panel.AddChild(_relaxButton);
            
            // Label de resultado (más grande y visible)
            _resultLabel = new Label();
            _resultLabel.Name = "ResultLabel";
            _resultLabel.Text = "";
            _resultLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _resultLabel.VerticalAlignment = VerticalAlignment.Center;
            _resultLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _resultLabel.ClipContents = true;
            // Tamaño de fuente más grande para mejor legibilidad
            float resultSize = FontManager.GetScaledSize(TextType.Body) * 1.1f;
            _resultLabel.AddThemeFontSizeOverride("font_size", (int)resultSize);
            _resultLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            // Área más grande para el texto
            _resultLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _resultLabel.OffsetTop = 160;
            _resultLabel.OffsetBottom = 280;
            _resultLabel.OffsetLeft = 40;
            _resultLabel.OffsetRight = -40;
            panel.AddChild(_resultLabel);
            
            // Botón continuar (oculto inicialmente)
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -30;
            _continueButton.OffsetTop = -90;
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _continueButton.Pressed += OnContinuePressed;
            panel.AddChild(_continueButton);
            
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
            
            // La barra sube sola sin control
            _stressBar.Value = Mathf.Min(_stressBar.Value + _random.Next(3, 8), 100);
            
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
            };
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

