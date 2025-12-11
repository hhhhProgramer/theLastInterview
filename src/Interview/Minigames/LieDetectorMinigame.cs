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
            panel.CustomMinimumSize = new Vector2(700, 500);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -350;
            panel.OffsetRight = 350;
            panel.OffsetTop = -250;
            panel.OffsetBottom = 250;
            
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
            titleLabel.Text = "DETECTOR DE MENTIRAS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.2f, 0.2f, 1.0f));
            titleLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            titleLabel.OffsetTop = 20;
            titleLabel.OffsetBottom = 60;
            titleLabel.OffsetLeft = 20;
            titleLabel.OffsetRight = -20;
            panel.AddChild(titleLabel);
            
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
            _statementLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _statementLabel.OffsetTop = 70;
            _statementLabel.OffsetBottom = 130;
            _statementLabel.OffsetLeft = 30;
            _statementLabel.OffsetRight = -30;
            panel.AddChild(_statementLabel);
            
            // Barra del detector
            _detectorBar = new ProgressBar();
            _detectorBar.Name = "DetectorBar";
            _detectorBar.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _detectorBar.CustomMinimumSize = new Vector2(550, 50);
            _detectorBar.OffsetLeft = -275;
            _detectorBar.OffsetRight = 275;
            _detectorBar.OffsetTop = -50;
            _detectorBar.OffsetBottom = 0;
            _detectorBar.MinValue = 0;
            _detectorBar.MaxValue = 100;
            _detectorBar.Value = 0;
            panel.AddChild(_detectorBar);
            
            // Label de estado
            _statusLabel = new Label();
            _statusLabel.Name = "StatusLabel";
            _statusLabel.Text = "Presiona el botón para confirmar tu honestidad...";
            _statusLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _statusLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _statusLabel.ClipContents = true;
            _statusLabel.AddThemeFontSizeOverride("font_size", (int)statementSize);
            _statusLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _statusLabel.OffsetTop = 30;
            _statusLabel.OffsetBottom = 80;
            _statusLabel.OffsetLeft = 30;
            _statusLabel.OffsetRight = -30;
            panel.AddChild(_statusLabel);
            
            // Botón confirmar honestidad
            _confirmButton = new Button();
            _confirmButton.Name = "ConfirmButton";
            _confirmButton.Text = "Confirmar Honestidad";
            _confirmButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _confirmButton.CustomMinimumSize = new Vector2(250, 50);
            _confirmButton.OffsetLeft = -125;
            _confirmButton.OffsetRight = 125;
            _confirmButton.OffsetTop = 60;
            _confirmButton.OffsetBottom = 110;
            _confirmButton.AddThemeFontSizeOverride("font_size", (int)statementSize);
            _confirmButton.Pressed += OnConfirmPressed;
            panel.AddChild(_confirmButton);
            
            // Botón continuar (oculto inicialmente)
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -30;
            _continueButton.OffsetTop = -90;
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)statementSize);
            _continueButton.Pressed += OnContinuePressed;
            panel.AddChild(_continueButton);
            
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
