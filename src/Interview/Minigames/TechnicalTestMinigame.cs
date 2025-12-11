using Godot;
using SlimeKingdomChronicles.Core.UI;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Prueba técnica falsa con preguntas absurdas
    /// </summary>
    public partial class TechnicalTestMinigame : BaseMinigame
    {
        private Label _questionLabel;
        private List<Button> _optionButtons;
        private Label _resultLabel;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        private bool _hasAnswered = false;
        
        private string _question = "¿Cuánto es 2 + desesperación?";
        
        private string[] _options = {
            "4",
            "No sé",
            "Mi salario",
            "¿Estoy contratado?"
        };
        
        private string[] _responses = {
            "Interesante enfoque matemático-corporativo.",
            "Tu lógica me asusta un poco... continúa.",
            "Correcto. La respuesta es siempre 'mi salario'.",
            "Esa es una pregunta válida. Pero no la respondí yo.",
            "Matemáticamente incorrecto, pero emocionalmente preciso.",
            "Aceptado. Has demostrado pensamiento lateral.",
            "Error 404: Lógica no encontrada. Pero aprobado."
        };
        
        public TechnicalTestMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo (más alto para que quepa todo sin sobreponerse)
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
            
            // Título
            var titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Text = "PRUEBA TÉCNICA";
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
            
            // Pregunta (más espacio)
            _questionLabel = new Label();
            _questionLabel.Name = "QuestionLabel";
            _questionLabel.Text = $"Demuestra tu competencia técnica. Resuelve este problema:\n\n{_question}";
            _questionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _questionLabel.VerticalAlignment = VerticalAlignment.Center;
            _questionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _questionLabel.ClipContents = true;
            float questionSize = FontManager.GetScaledSize(TextType.Body);
            _questionLabel.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _questionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            _questionLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _questionLabel.OffsetTop = 70;
            _questionLabel.OffsetBottom = 160;
            _questionLabel.OffsetLeft = 30;
            _questionLabel.OffsetRight = -30;
            panel.AddChild(_questionLabel);
            
            // Opciones (movidas más abajo para no sobreponerse con la pregunta)
            _optionButtons = new List<Button>();
            var optionsContainer = new VBoxContainer();
            optionsContainer.Name = "OptionsContainer";
            optionsContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            optionsContainer.CustomMinimumSize = new Vector2(400, 180);
            optionsContainer.OffsetLeft = 150;
            optionsContainer.OffsetRight = -150;
            optionsContainer.OffsetTop = 170;
            optionsContainer.OffsetBottom = 350;
            optionsContainer.AddThemeConstantOverride("separation", 10);
            panel.AddChild(optionsContainer);
            
            for (int i = 0; i < _options.Length; i++)
            {
                int index = i;
                var button = new Button();
                button.Text = _options[i];
                button.AddThemeFontSizeOverride("font_size", (int)questionSize);
                button.Pressed += () => OnOptionSelected(index);
                _optionButtons.Add(button);
                optionsContainer.AddChild(button);
            }
            
            // Label de resultado (más espacio desde el botón continuar)
            _resultLabel = new Label();
            _resultLabel.Name = "ResultLabel";
            _resultLabel.Text = "";
            _resultLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _resultLabel.VerticalAlignment = VerticalAlignment.Center;
            _resultLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _resultLabel.ClipContents = true;
            _resultLabel.CustomMinimumSize = new Vector2(500, 100);
            _resultLabel.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _resultLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            // Posicionado más arriba para dejar espacio al botón continuar
            _resultLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _resultLabel.OffsetTop = 360;
            _resultLabel.OffsetBottom = 460;
            _resultLabel.OffsetLeft = 100;
            _resultLabel.OffsetRight = -100;
            panel.AddChild(_resultLabel);
            
            // Botón continuar (más espacio desde el ResultLabel)
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -30;
            _continueButton.OffsetTop = -90;
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _continueButton.Pressed += OnContinuePressed;
            panel.AddChild(_continueButton);
        }
        
        private void OnOptionSelected(int optionIndex)
        {
            if (_hasAnswered) return;
            
            _hasAnswered = true;
            
            // Ocultar opciones
            foreach (var button in _optionButtons)
            {
                button.Visible = false;
            }
            
            // Respuesta aleatoria y cómica (no hay respuesta correcta)
            string response = _responses[_random.Next(_responses.Length)];
            _resultLabel.Text = $"Entrevistador: \"{response}\"";
            
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

