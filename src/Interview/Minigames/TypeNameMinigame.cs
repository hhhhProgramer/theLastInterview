using Godot;
using SlimeKingdomChronicles.Core.UI;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Escribe tu nombre pero cada tecla introduce una letra aleatoria
    /// </summary>
    public partial class TypeNameMinigame : BaseMinigame
    {
        private LineEdit _nameInput;
        private Label _instructionLabel;
        private Label _resultLabel;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        private int _letterCount = 0;
        private bool _completed = false;
        
        private string[] _interruptions = {
            "Perfecto. Buena ortografía. Siguiente pregunta.",
            "¿Ese es tu nombre? Suena muy corporativo.",
            "Perfecto... pero contratamos solo a gente sin vocales.",
            "Interesante. Tu nombre tiene demasiadas consonantes para nuestro estándar.",
            "Aceptado. Aunque suena como un nombre de bot.",
            "Bien, pero preferimos nombres que rimen con 'productividad'.",
            "Excelente. Tu nombre contiene exactamente 5 letras. Coincidencia perfecta."
        };
        
        public TypeNameMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo
            var panel = new Panel();
            panel.Name = "MinigamePanel";
            panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            panel.CustomMinimumSize = new Vector2(700, 450);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -350;
            panel.OffsetRight = 350;
            panel.OffsetTop = -225;
            panel.OffsetBottom = 225;
            
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
            titleLabel.Text = "ESCRIBE TU NOMBRE";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.2f, 0.8f, 1.0f, 1.0f));
            titleLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            titleLabel.OffsetTop = 20;
            titleLabel.OffsetBottom = 60;
            titleLabel.OffsetLeft = 20;
            titleLabel.OffsetRight = -20;
            panel.AddChild(titleLabel);
            
            // Instrucción
            _instructionLabel = new Label();
            _instructionLabel.Name = "InstructionLabel";
            _instructionLabel.Text = "Escribe tu nombre (cada tecla introduce una letra aleatoria):";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _instructionLabel.VerticalAlignment = VerticalAlignment.Center;
            _instructionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _instructionLabel.ClipContents = true;
            float instructionSize = FontManager.GetScaledSize(TextType.Body);
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _instructionLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _instructionLabel.OffsetTop = 70;
            _instructionLabel.OffsetBottom = 120;
            _instructionLabel.OffsetLeft = 30;
            _instructionLabel.OffsetRight = -30;
            panel.AddChild(_instructionLabel);
            
            // Campo de texto
            _nameInput = new LineEdit();
            _nameInput.Name = "NameInput";
            _nameInput.PlaceholderText = "Escribe aquí...";
            _nameInput.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _nameInput.CustomMinimumSize = new Vector2(450, 50);
            _nameInput.OffsetLeft = -225;
            _nameInput.OffsetRight = 225;
            _nameInput.OffsetTop = -20;
            _nameInput.OffsetBottom = 30;
            _nameInput.TextChanged += OnTextChanged;
            _nameInput.TextSubmitted += OnTextSubmitted;
            float inputSize = FontManager.GetScaledSize(TextType.Body);
            _nameInput.AddThemeFontSizeOverride("font_size", (int)inputSize);
            panel.AddChild(_nameInput);
            
            // Label de resultado
            _resultLabel = new Label();
            _resultLabel.Name = "ResultLabel";
            _resultLabel.Text = "";
            _resultLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _resultLabel.VerticalAlignment = VerticalAlignment.Center;
            _resultLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _resultLabel.ClipContents = true;
            _resultLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _resultLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            _resultLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _resultLabel.OffsetTop = 50;
            _resultLabel.OffsetBottom = 110;
            _resultLabel.OffsetLeft = 30;
            _resultLabel.OffsetRight = -30;
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
            
            // Focus en el input
            _nameInput.CallDeferred(Control.MethodName.GrabFocus);
        }
        
        private void OnTextChanged(string newText)
        {
            if (_completed) return;
            
            // Cada vez que escribes, se reemplaza con una letra aleatoria
            if (!string.IsNullOrEmpty(newText))
            {
                char randomChar = (char)_random.Next('A', 'Z' + 1);
                
                // Desconectar temporalmente para evitar loop
                _nameInput.TextChanged -= OnTextChanged;
                _nameInput.Text = randomChar.ToString();
                _nameInput.CaretColumn = 1;
                _nameInput.TextChanged += OnTextChanged;
                
                _letterCount++;
                
                // Si completas 5 letras, el entrevistador interrumpe
                if (_letterCount >= 5)
                {
                    _completed = true;
                    _nameInput.Editable = false;
                    string interruption = _interruptions[_random.Next(_interruptions.Length)];
                    _resultLabel.Text = $"Entrevistador: \"{interruption}\"";
                    
                    GetTree().CreateTimer(2.0f).Timeout += () => {
                        _continueButton.Visible = true;
                    };
                }
            }
        }
        
        private void OnTextSubmitted(string text)
        {
            // Si presionan Enter antes de las 5 letras, también avanza
            if (!_completed && _letterCount > 0)
            {
                _completed = true;
                _nameInput.Editable = false;
                string interruption = _interruptions[_random.Next(_interruptions.Length)];
                _resultLabel.Text = $"Entrevistador: \"{interruption}\"";
                
                GetTree().CreateTimer(1.5f).Timeout += () => {
                    _continueButton.Visible = true;
                };
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}
