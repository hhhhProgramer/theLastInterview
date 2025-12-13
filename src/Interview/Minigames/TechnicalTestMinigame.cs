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
        
        private string[] _questions = {
            "¿Cuánto es 2 + desesperación?",
            "¿Cuál es la velocidad de la luz en un viernes a las 5pm?",
            "¿Cuántas personas se necesitan para cambiar una bombilla?",
            "¿Qué es más importante: el trabajo o el café?",
            "Si un árbol cae en un bosque sin nadie, ¿hace ruido?",
            "¿Cuál es el sentido de la vida, el universo y todo lo demás?",
            "¿Por qué los lunes existen?",
            "¿Cuántas veces puedes doblar un papel antes de que se rompa?",
            "¿Qué vino primero: el huevo o la gallina?",
            "¿Cuánto tiempo tarda en hervir el agua en Marte?",
            "¿Cuál es la mejor forma de organizar un escritorio?",
            "¿Por qué los viernes se sienten más largos que los lunes?"
        };
        
        private string _question = "";
        
        private string[] _options = {
            "4",
            "No sé",
            "Mi salario",
            "¿Estoy contratado?",
            "42",
            "Depende",
            "Sí",
            "No",
            "Tal vez",
            "Error 404"
        };
        
        private string[] _responses = {
            "Interesante enfoque matemático-corporativo.",
            "Tu lógica me asusta un poco... continúa.",
            "Correcto. La respuesta es siempre 'mi salario'.",
            "Esa es una pregunta válida. Pero no la respondí yo.",
            "Matemáticamente incorrecto, pero emocionalmente preciso.",
            "Aceptado. Has demostrado pensamiento lateral.",
            "Error 404: Lógica no encontrada. Pero aprobado.",
            "¡Genial! Esa respuesta tiene exactamente 0% de sentido. Perfecto.",
            "Interesante. Tu capacidad de razonamiento es... existente.",
            "Correcto. Aunque técnicamente incorrecto. Pero correcto.",
            "Excelente. Has demostrado que puedes leer. Habilidad impresionante.",
            "Perfecto. Esa respuesta es tan válida como mi deseo de estar aquí."
        };
        
        public TechnicalTestMinigame(Node parent) : base(parent)
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
            titleLabel.Text = "PRUEBA TÉCNICA";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.3f; // 30% más grande
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.2f, 0.8f, 1.0f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Seleccionar pregunta aleatoria
            _question = _questions[_random.Next(_questions.Length)];
            
            // Pregunta
            _questionLabel = new Label();
            _questionLabel.Name = "QuestionLabel";
            _questionLabel.Text = $"Demuestra tu competencia técnica. Resuelve este problema:\n\n{_question}";
            _questionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _questionLabel.VerticalAlignment = VerticalAlignment.Center;
            _questionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _questionLabel.ClipContents = true;
            float questionSize = FontManager.GetScaledSize(TextType.Body) * 1.2f; // 20% más grande
            _questionLabel.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _questionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_questionLabel);
            
            // Opciones (seleccionar 4 aleatorias)
            _optionButtons = new List<Button>();
            var optionsContainer = new VBoxContainer();
            optionsContainer.Name = "OptionsContainer";
            optionsContainer.AddThemeConstantOverride("separation", 10);
            mainContainer.AddChild(optionsContainer);
            
            var selectedOptions = new List<string>();
            while (selectedOptions.Count < 4)
            {
                string option = _options[_random.Next(_options.Length)];
                if (!selectedOptions.Contains(option))
                {
                    selectedOptions.Add(option);
                }
            }
            
            for (int i = 0; i < selectedOptions.Count; i++)
            {
                int index = i;
                var button = new Button();
                button.Text = selectedOptions[i];
                button.AddThemeFontSizeOverride("font_size", (int)(questionSize * 1.1f)); // 10% más grande
                button.Pressed += () => OnOptionSelected(index);
                _optionButtons.Add(button);
                optionsContainer.AddChild(button);
            }
            
            // Label de resultado
            _resultLabel = new Label();
            _resultLabel.Name = "ResultLabel";
            _resultLabel.Text = "";
            _resultLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _resultLabel.VerticalAlignment = VerticalAlignment.Center;
            _resultLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _resultLabel.ClipContents = true;
            _resultLabel.CustomMinimumSize = new Vector2(0, 100);
            _resultLabel.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _resultLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            mainContainer.AddChild(_resultLabel);
            
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
            _continueButton.AddThemeFontSizeOverride("font_size", (int)questionSize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
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
                StartButtonBlink(_continueButton); // Hacer parpadear el botón continuar
            };
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}
