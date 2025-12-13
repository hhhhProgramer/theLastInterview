using Godot;
using SlimeKingdomChronicles.Core.UI;
using System;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Archivar Archivos Incorrectos - el destino cambia cada 0.5s
    /// </summary>
    public partial class ArchiveFilesMinigame : BaseMinigame
    {
        private Label _fileLabel;
        private Button _trayAButton;
        private Button _trayBButton;
        private Label _scoreLabel;
        private Button _continueButton;
        private Timer _changeTimer;
        private System.Random _random = new System.Random();
        
        private int _correctCount = 0;
        private int _targetCount = 5;
        private string _currentDestination = "A";
        private bool _gameActive = false;
        
        public ArchiveFilesMinigame(Node parent) : base(parent)
        {
        }
        
        public override void ShowMinigame()
        {
            Visible = true;
            CreateUI();
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
            
            // Contenedor principal
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            mainContainer.OffsetLeft = 30;
            mainContainer.OffsetRight = -30;
            mainContainer.OffsetTop = 20;
            mainContainer.OffsetBottom = -20;
            mainContainer.AddThemeConstantOverride("separation", 20);
            panel.AddChild(mainContainer);
            
            // Título
            var titleLabel = new Label();
            titleLabel.Text = "ARCHIVAR ARCHIVOS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.3f; // 30% más grande
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.9f, 0.0f, 1.0f)); // Amarillo brillante
            mainContainer.AddChild(titleLabel);
            
            // Contador
            _scoreLabel = new Label();
            _scoreLabel.Text = $"Correctos: {_correctCount} / {_targetCount}";
            _scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float bodySize = FontManager.GetScaledSize(TextType.Body) * 1.2f; // 20% más grande
            _scoreLabel.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _scoreLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_scoreLabel);
            
            // Archivo actual
            _fileLabel = new Label();
            _fileLabel.Text = "archivo_23.pdf → Enviar a: A";
            _fileLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _fileLabel.VerticalAlignment = VerticalAlignment.Center;
            _fileLabel.CustomMinimumSize = new Vector2(0, 100);
            _fileLabel.AddThemeFontSizeOverride("font_size", (int)(bodySize * 1.4f)); // Aumentado de 1.2f a 1.4f
            _fileLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.9f, 0.6f, 1.0f));
            mainContainer.AddChild(_fileLabel);
            
            // Botones de bandejas
            var buttonsContainer = new HBoxContainer();
            buttonsContainer.AddThemeConstantOverride("separation", 30);
            mainContainer.AddChild(buttonsContainer);
            
            _trayAButton = new Button();
            _trayAButton.Text = "BANDEJA A";
            Vector2 trayButtonSize = GetResponsiveSize(0.35f, 0.055f);
            _trayAButton.CustomMinimumSize = trayButtonSize;
            _trayAButton.AddThemeFontSizeOverride("font_size", (int)(bodySize * 1.1f)); // 10% más grande
            _trayAButton.Pressed += () => OnTraySelected("A");
            buttonsContainer.AddChild(_trayAButton);
            
            _trayBButton = new Button();
            _trayBButton.Text = "BANDEJA B";
            _trayBButton.CustomMinimumSize = trayButtonSize;
            _trayBButton.AddThemeFontSizeOverride("font_size", (int)(bodySize * 1.1f)); // 10% más grande
            _trayBButton.Pressed += () => OnTraySelected("B");
            buttonsContainer.AddChild(_trayBButton);
            
            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 20);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            mainContainer.AddChild(spacer);
            
            // Botón continuar
            _continueButton = new Button();
            _continueButton.Text = "Continuar";
            _continueButton.CustomMinimumSize = new Vector2(200, 50);
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
            
            // Iniciar el juego
            _gameActive = true;
            StartChangeTimer();
        }
        
        private void StartChangeTimer()
        {
            _changeTimer = new Timer();
            _changeTimer.WaitTime = 0.5f + (float)_random.NextDouble() * 0.5f; // Entre 0.5 y 1.0 segundos
            _changeTimer.Timeout += ChangeDestination;
            _changeTimer.Autostart = true;
            AddChild(_changeTimer);
        }
        
        private void ChangeDestination()
        {
            if (!_gameActive) return;
            
            // Cambiar entre A y B
            _currentDestination = _currentDestination == "A" ? "B" : "A";
            _fileLabel.Text = $"archivo_{_random.Next(10, 99)}.pdf → Enviar a: {_currentDestination}";
            
            // Reiniciar timer con tiempo aleatorio
            _changeTimer.WaitTime = 0.5f + (float)_random.NextDouble() * 0.5f;
        }
        
        private void OnTraySelected(string selectedTray)
        {
            if (!_gameActive) return;
            
            if (selectedTray == _currentDestination)
            {
                _correctCount++;
                _scoreLabel.Text = $"Correctos: {_correctCount} / {_targetCount}";
                
                if (_correctCount >= _targetCount)
                {
                    _gameActive = false;
                    _changeTimer.Stop();
                    
                    // Ocultar botones
                    _trayAButton.Visible = false;
                    _trayBButton.Visible = false;
                    
                    // Mostrar mensaje de completado con mejor formato
                    _fileLabel.Text = "¡Completado! Has archivado 5 archivos correctamente.";
                    _fileLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                    _fileLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.9f, 0.0f, 1.0f)); // Amarillo brillante
                    _fileLabel.AddThemeFontSizeOverride("font_size", (int)(FontManager.GetScaledSize(TextType.Body) * 1.1f));
                    
                    // Centrar el mensaje mejor
                    _fileLabel.HorizontalAlignment = HorizontalAlignment.Center;
                    _fileLabel.VerticalAlignment = VerticalAlignment.Center;
                    
                    GetTree().CreateTimer(1.5f).Timeout += () => {
                        _continueButton.Visible = true;
                        StartButtonBlink(_continueButton); // Hacer parpadear el botón continuar
                    };
                }
                else
                {
                    // Cambiar a nuevo archivo
                    _currentDestination = _random.Next(0, 2) == 0 ? "A" : "B";
                    _fileLabel.Text = $"archivo_{_random.Next(10, 99)}.pdf → Enviar a: {_currentDestination}";
                }
            }
            else
            {
                // Incorrecto, pero continuar
                _fileLabel.Text = $"Incorrecto. El destino cambió a: {_currentDestination}";
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

