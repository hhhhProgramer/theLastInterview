using System;
using Godot;
using SlimeKingdomChronicles.Core.UI;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Detector de mentiras que nunca acierta
    /// </summary>
    public partial class LieDetectorMinigame : BaseMinigame
    {
        private ProgressBar _detectorBar;
        private Label _statusLabel;
        private Label _questionLabel;
        private Button _continueButton;
        private Timer _fakeTimer;
        private bool _isDetecting = false;
        
        public LieDetectorMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo (más grande para que quepan los textos)
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
            
            // Pregunta del detector (contexto)
            _questionLabel = new Label();
            _questionLabel.Name = "QuestionLabel";
            _questionLabel.Text = "Pregunta: ¿Estás mintiendo en esta entrevista?\n(El detector siempre falla, es solo para molestarte)";
            _questionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _questionLabel.VerticalAlignment = VerticalAlignment.Center;
            _questionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _questionLabel.ClipContents = true;
            float questionSize = FontManager.GetScaledSize(TextType.Body);
            _questionLabel.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _questionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            _questionLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _questionLabel.OffsetTop = 70;
            _questionLabel.OffsetBottom = 130;
            _questionLabel.OffsetLeft = 30;
            _questionLabel.OffsetRight = -30;
            panel.AddChild(_questionLabel);
            
            // Barra del detector
            _detectorBar = new ProgressBar();
            _detectorBar.Name = "DetectorBar";
            _detectorBar.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _detectorBar.CustomMinimumSize = new Vector2(550, 50);
            _detectorBar.OffsetLeft = -275;
            _detectorBar.OffsetRight = 275;
            _detectorBar.OffsetTop = -30;
            _detectorBar.OffsetBottom = 20;
            _detectorBar.MinValue = 0;
            _detectorBar.MaxValue = 100;
            _detectorBar.Value = 0;
            panel.AddChild(_detectorBar);
            
            // Label de estado
            _statusLabel = new Label();
            _statusLabel.Name = "StatusLabel";
            _statusLabel.Text = "Analizando tu respuesta...";
            _statusLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _statusLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _statusLabel.ClipContents = true;
            float statusSize = FontManager.GetScaledSize(TextType.Body);
            _statusLabel.AddThemeFontSizeOverride("font_size", (int)statusSize);
            _statusLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _statusLabel.OffsetTop = 50;
            _statusLabel.OffsetBottom = 100;
            _statusLabel.OffsetLeft = 30;
            _statusLabel.OffsetRight = -30;
            panel.AddChild(_statusLabel);
            
            // Botón continuar (oculto inicialmente)
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -30;
            _continueButton.OffsetTop = -90;
            _continueButton.Visible = false;
            float buttonSize = FontManager.GetScaledSize(TextType.Body);
            _continueButton.AddThemeFontSizeOverride("font_size", (int)buttonSize);
            _continueButton.Pressed += OnContinuePressed;
            panel.AddChild(_continueButton);
            
            // Iniciar detección falsa
            StartFakeDetection();
        }
        
        private void StartFakeDetection()
        {
            _isDetecting = true;
            _fakeTimer = new Timer();
            _fakeTimer.WaitTime = 0.1f;
            _fakeTimer.Timeout += UpdateFakeBar;
            _fakeTimer.Autostart = true;
            AddChild(_fakeTimer);
        }
        
        private void UpdateFakeBar()
        {
            if (!_isDetecting) return;
            
            // La barra sube y baja aleatoriamente, nunca llega al 100%
            var random = new System.Random();
            float currentValue = (float)_detectorBar.Value;
            
            // Movimiento errático que nunca completa
            if (currentValue < 30)
            {
                _detectorBar.Value = currentValue + random.Next(5, 15);
            }
            else if (currentValue < 70)
            {
                // Va hacia arriba pero luego baja
                if (random.Next(0, 2) == 0)
                {
                    _detectorBar.Value = currentValue + random.Next(1, 10);
                }
                else
                {
                    _detectorBar.Value = Math.Max(0, currentValue - random.Next(5, 15));
                }
            }
            else
            {
                // Casi llega pero siempre falla
                if (currentValue > 85)
                {
                    _detectorBar.Value = Math.Max(0, currentValue - random.Next(10, 20));
                    _statusLabel.Text = "Error en el sistema... Reintentando...";
                }
                else
                {
                    _detectorBar.Value = currentValue + random.Next(1, 5);
                }
            }
            
            // Después de 3 segundos, siempre falla de forma cómica
            if (_fakeTimer.TimeLeft < 2.0f && _fakeTimer.TimeLeft > 0)
            {
                _detectorBar.Value = random.Next(0, 30);
                _statusLabel.Text = "Resultado: INCONCLUSOPS... ERROR DEL SISTEMA\n(Como siempre, el detector no sirve para nada)";
                _isDetecting = false;
                _fakeTimer.Stop();
                _continueButton.Visible = true;
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

