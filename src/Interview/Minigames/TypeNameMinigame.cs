using Godot;
using SlimeKingdomChronicles.Core.UI;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Escribe tu nombre con teclado que deliberadamente falla
    /// </summary>
    public partial class TypeNameMinigame : BaseMinigame
    {
        private LineEdit _nameInput;
        private Label _instructionLabel;
        private Label _resultLabel;
        private Button _continueButton;
        private string _targetName = "";
        private System.Random _random = new System.Random();
        
        public TypeNameMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo (más grande para que quepan los textos)
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
            _instructionLabel.Text = "Por favor, escribe tu nombre en el campo de abajo:\n(El teclado está roto y cambiará letras aleatoriamente)";
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
            
            // Campo de texto (con comportamiento que falla)
            _nameInput = new LineEdit();
            _nameInput.Name = "NameInput";
            _nameInput.PlaceholderText = "Escribe tu nombre aquí...";
            _nameInput.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _nameInput.CustomMinimumSize = new Vector2(450, 50);
            _nameInput.OffsetLeft = -225;
            _nameInput.OffsetRight = 225;
            _nameInput.OffsetTop = -10;
            _nameInput.OffsetBottom = 40;
            _nameInput.TextChanged += OnTextChanged;
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
            _resultLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.3f, 0.3f, 1.0f));
            _resultLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            _resultLabel.OffsetTop = 60;
            _resultLabel.OffsetBottom = 120;
            _resultLabel.OffsetLeft = 30;
            _resultLabel.OffsetRight = -30;
            panel.AddChild(_resultLabel);
            
            // Botón continuar
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar de todas formas";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -30;
            _continueButton.OffsetTop = -90;
            float buttonSize = FontManager.GetScaledSize(TextType.Body);
            _continueButton.AddThemeFontSizeOverride("font_size", (int)buttonSize);
            _continueButton.Pressed += OnContinuePressed;
            panel.AddChild(_continueButton);
            
            // Focus en el input
            _nameInput.CallDeferred(Control.MethodName.GrabFocus);
        }
        
        private void OnTextChanged(string newText)
        {
            if (string.IsNullOrEmpty(newText)) return;
            
            // El teclado "falla" cambiando caracteres aleatoriamente
            if (_random.Next(0, 3) == 0) // 33% de probabilidad de fallar
            {
                // Cambiar un carácter aleatorio
                var chars = newText.ToCharArray();
                int index = _random.Next(0, chars.Length);
                
                // Reemplazar con carácter aleatorio incorrecto
                char[] wrongChars = { 'X', 'Z', 'Q', '7', '@', '#', '&' };
                chars[index] = wrongChars[_random.Next(wrongChars.Length)];
                
                // Desconectar temporalmente el evento para evitar loop infinito
                _nameInput.TextChanged -= OnTextChanged;
                _nameInput.Text = new string(chars);
                _nameInput.TextChanged += OnTextChanged;
                _resultLabel.Text = "¡El teclado cambió una letra!\n(Está roto, no puedes escribir correctamente)";
                
                // Mover el cursor al final
                _nameInput.CaretColumn = _nameInput.Text.Length;
            }
            else if (newText.Length > 3)
            {
                // Después de escribir algo, siempre muestra error
                _resultLabel.Text = "Error: El sistema no reconoce ese nombre.\n(El teclado sigue fallando, es inútil intentar)";
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

