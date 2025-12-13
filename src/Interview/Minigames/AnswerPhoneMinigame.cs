using Godot;
using SlimeKingdomChronicles.Core.UI;
using System;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Responder el Teléfono Correcto - 3 paneles que parpadean
    /// </summary>
    public partial class AnswerPhoneMinigame : BaseMinigame
    {
        private Button[] _phoneButtons;
        private Label _instructionLabel;
        private Button _continueButton;
        private Timer _blinkTimer;
        private System.Random _random = new System.Random();
        
        private int _currentRingingPhone = 0;
        private int _changeCount = 0;
        private int _maxChanges = 3;
        private bool _gameActive = false;
        private bool _phoneAnswered = false;
        
        public AnswerPhoneMinigame(Node parent) : base(parent)
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
            panel.CustomMinimumSize = new Vector2(800, 500);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -400;
            panel.OffsetRight = 400;
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
            titleLabel.Text = "RESPONDER EL TELÉFONO";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.3f; // 30% más grande
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.2f, 0.8f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Instrucción
            _instructionLabel = new Label();
            _instructionLabel.Text = "Responde el teléfono que está sonando (el que parpadea)";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float bodySize = FontManager.GetScaledSize(TextType.Body) * 1.2f; // 20% más grande
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _instructionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_instructionLabel);
            
            // Contenedor de teléfonos
            var phonesContainer = new HBoxContainer();
            phonesContainer.AddThemeConstantOverride("separation", 20);
            mainContainer.AddChild(phonesContainer);
            
            _phoneButtons = new Button[3];
            for (int i = 0; i < 3; i++)
            {
                int index = i;
                var phoneButton = new Button();
                phoneButton.Text = $"📞 Teléfono {(char)('A' + i)}";
                Vector2 phoneButtonSize = GetResponsiveSize(0.28f, 0.10f);
                phoneButton.CustomMinimumSize = phoneButtonSize;
                phoneButton.AddThemeFontSizeOverride("font_size", (int)(bodySize * 1.1f)); // 10% más grande
                phoneButton.Pressed += () => OnPhoneClicked(index);
                _phoneButtons[i] = phoneButton;
                phonesContainer.AddChild(phoneButton);
            }
            
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
            _currentRingingPhone = _random.Next(0, 3);
            StartBlinkTimer();
        }
        
        private void StartBlinkTimer()
        {
            _blinkTimer = new Timer();
            _blinkTimer.WaitTime = 1.0f;
            _blinkTimer.Timeout += OnBlinkTimer;
            _blinkTimer.Autostart = true;
            AddChild(_blinkTimer);
            UpdatePhoneDisplay();
        }
        
        private void OnBlinkTimer()
        {
            if (!_gameActive || _phoneAnswered) return;
            
            // Cambiar teléfono si no se ha alcanzado el máximo de cambios
            if (_changeCount < _maxChanges)
            {
                // 70% de probabilidad de cambiar
                if (_random.Next(0, 10) < 7)
                {
                    int newPhone = _random.Next(0, 3);
                    while (newPhone == _currentRingingPhone)
                    {
                        newPhone = _random.Next(0, 3);
                    }
                    _currentRingingPhone = newPhone;
                    _changeCount++;
                    UpdatePhoneDisplay();
                }
            }
            else
            {
                // Después de 3 cambios, quedarse fijo por 1 segundo
                _blinkTimer.WaitTime = 1.0f;
            }
        }
        
        private void UpdatePhoneDisplay()
        {
            for (int i = 0; i < _phoneButtons.Length; i++)
            {
                if (i == _currentRingingPhone)
                {
                    // Teléfono sonando - parpadeo
                    var style = new StyleBoxFlat();
                    style.BgColor = new Color(0.3f, 0.8f, 0.3f, 1.0f);
                    style.BorderColor = new Color(0.5f, 1.0f, 0.5f, 1.0f);
                    style.BorderWidthLeft = 4;
                    style.BorderWidthTop = 4;
                    style.BorderWidthRight = 4;
                    style.BorderWidthBottom = 4;
                    _phoneButtons[i].AddThemeStyleboxOverride("normal", style);
                    _phoneButtons[i].Text = $"📞 RING\nTeléfono {(char)('A' + i)}";
                }
                else
                {
                    // Teléfono normal
                    var style = new StyleBoxFlat();
                    style.BgColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
                    style.BorderColor = new Color(0.4f, 0.4f, 0.4f, 1.0f);
                    style.BorderWidthLeft = 2;
                    style.BorderWidthTop = 2;
                    style.BorderWidthRight = 2;
                    style.BorderWidthBottom = 2;
                    _phoneButtons[i].AddThemeStyleboxOverride("normal", style);
                    _phoneButtons[i].Text = $"📞 Teléfono {(char)('A' + i)}";
                }
            }
        }
        
        private void OnPhoneClicked(int phoneIndex)
        {
            if (!_gameActive || _phoneAnswered) return;
            
            // 40% de probabilidad de que NO cambie, 60% de que cambie aleatoriamente
            bool shouldChange = _random.Next(0, 10) < 6; // 60% de probabilidad
            
            if (shouldChange)
            {
                // Cambiar a otro teléfono aleatorio
                int newPhone = _random.Next(0, 3);
                while (newPhone == _currentRingingPhone)
                {
                    newPhone = _random.Next(0, 3);
                }
                _currentRingingPhone = newPhone;
                _changeCount++;
                UpdatePhoneDisplay();
                
                // Mostrar mensaje de que cambió
                _instructionLabel.Text = $"El teléfono cambió a {(char)('A' + _currentRingingPhone)}";
                _instructionLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.8f, 0.2f, 1.0f));
                
                // Después de un momento, verificar si el click fue correcto
                GetTree().CreateTimer(0.3f).Timeout += () => {
                    CheckPhoneAnswer(phoneIndex);
                };
            }
            else
            {
                // No cambió, verificar respuesta inmediatamente
                CheckPhoneAnswer(phoneIndex);
            }
        }
        
        private void CheckPhoneAnswer(int phoneIndex)
        {
            if (_phoneAnswered) return;
            
            _phoneAnswered = true;
            _blinkTimer.Stop();
            
            if (phoneIndex == _currentRingingPhone)
            {
                _instructionLabel.Text = $"¡Correcto! Respondiste el Teléfono {(char)('A' + phoneIndex)}";
                _instructionLabel.AddThemeColorOverride("font_color", new Color(0.3f, 1.0f, 0.3f, 1.0f));
            }
            else
            {
                _instructionLabel.Text = $"Incorrecto. El teléfono correcto era {(char)('A' + _currentRingingPhone)}";
                _instructionLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.3f, 0.3f, 1.0f));
            }
            
            // Deshabilitar todos los botones
            foreach (var button in _phoneButtons)
            {
                button.Disabled = true;
            }
            
            GetTree().CreateTimer(1.5f).Timeout += () => {
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

