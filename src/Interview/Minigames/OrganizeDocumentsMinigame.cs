using Godot;
using SlimeKingdomChronicles.Core.UI;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Ordena documentos - selecciona el más urgente
    /// </summary>
    public partial class OrganizeDocumentsMinigame : BaseMinigame
    {
        private List<Button> _documentButtons;
        private Label _feedbackLabel;
        private Label _commentLabel;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        private int _documentsPlaced = 0;
        private HashSet<int> _placedDocuments = new HashSet<int>();
        
        private string[] _documentNames = {
            "Solicitud de descanso emocional",
            "Queja del pasante por existir",
            "Denuncia del ventilador"
        };
        
        private string[] _comments = {
            "Interesante elección. Eres un genio del archivo.",
            "Perfecto. Esa es exactamente la prioridad correcta... creo.",
            "Bien hecho. Tu instinto organizacional es impresionante.",
            "Aprobado. Aunque técnicamente nada de esto tiene sentido.",
            "Perfecto. Has demostrado dominio total del caos.",
            "Eso no es urgente. ¿Estás seguro de que sabes leer?",
            "Incorrecto. Ese documento debería ser... ¿dónde?",
            "Mal. Muy mal. Pero seguimos adelante porque sí.",
            "Eso está mal, pero me da igual. Siguiente.",
            "No, eso no es correcto. Pero tampoco importa realmente.",
            "Excelente. Tu criterio es... cuestionable.",
            "Bien. Aunque personalmente lo habría puesto en otra categoría inexistente.",
            "¡Genial! Ese documento definitivamente no debería estar ahí... o sí?",
            "Perfecto. Has elegido el documento más... documentado.",
            "Interesante. Ese documento tiene exactamente 0% de urgencia. Aprobado.",
            "Excelente. Tu capacidad de organización es... existente.",
            "Bien hecho. Ese documento es tan urgente como mi deseo de estar aquí.",
            "Perfecto. Has demostrado que puedes hacer clic. Habilidad impresionante."
        };
        
        public OrganizeDocumentsMinigame(Node parent) : base(parent)
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
            float panelWidthPercent = viewportSize.X < 1000 ? 0.90f : 0.65f;
            float panelHeightPercent = viewportSize.Y < 1000 ? 0.85f : 0.55f;
            Vector2 panelSize = GetResponsiveSize(panelWidthPercent, panelHeightPercent);
            
            // Panel de fondo (compacto)
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
            
            // Contenedor principal con VBoxContainer (márgenes responsive)
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            float margin = GetResponsiveMargin(0.015f);
            mainContainer.OffsetLeft = margin;
            mainContainer.OffsetRight = -margin;
            mainContainer.OffsetTop = margin * 0.7f;
            mainContainer.OffsetBottom = -margin * 0.7f;
            float separation = GetResponsiveMargin(0.008f);
            mainContainer.AddThemeConstantOverride("separation", (int)separation);
            panel.AddChild(mainContainer);
            
            // Título
            var titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Text = "ORGANIZA LOS DOCUMENTOS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.3f; // 30% más grande
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.2f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Instrucción simple
            var instructionLabel = new Label();
            instructionLabel.Name = "InstructionLabel";
            instructionLabel.Text = "Selecciona el más urgente:";
            instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            instructionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            instructionLabel.ClipContents = true;
            float instructionSize = FontManager.GetScaledSize(TextType.Body) * 1.2f; // 20% más grande
            instructionLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            instructionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(instructionLabel);
            
            // Contenedor para documentos (botones simples)
            var documentsContainer = new VBoxContainer();
            documentsContainer.Name = "DocumentsContainer";
            documentsContainer.AddThemeConstantOverride("separation", 10);
            mainContainer.AddChild(documentsContainer);
            
            _documentButtons = new List<Button>();
            for (int i = 0; i < _documentNames.Length; i++)
            {
                int index = i;
                var button = new Button();
                button.Text = $"📄 {_documentNames[i]}";
                float buttonHeight = viewportSize.Y * 0.035f;
                button.CustomMinimumSize = new Vector2(0, buttonHeight);
                button.AddThemeFontSizeOverride("font_size", (int)instructionSize); // Usar tamaño completo
                button.Pressed += () => OnDocumentClicked(index);
                _documentButtons.Add(button);
                documentsContainer.AddChild(button);
            }
            
            // Feedback label (resultado)
            _feedbackLabel = new Label();
            _feedbackLabel.Name = "FeedbackLabel";
            _feedbackLabel.Text = "";
            _feedbackLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _feedbackLabel.VerticalAlignment = VerticalAlignment.Center;
            _feedbackLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _feedbackLabel.ClipContents = true;
            _feedbackLabel.CustomMinimumSize = new Vector2(0, 50);
            _feedbackLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _feedbackLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            mainContainer.AddChild(_feedbackLabel);
            
            // Comentario label (con fuente más pequeña)
            _commentLabel = new Label();
            _commentLabel.Name = "CommentLabel";
            _commentLabel.Text = "";
            _commentLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _commentLabel.VerticalAlignment = VerticalAlignment.Center;
            _commentLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _commentLabel.ClipContents = true;
            _commentLabel.CustomMinimumSize = new Vector2(0, 60);
            float smallSize = FontManager.GetScaledSize(TextType.Small);
            _commentLabel.AddThemeFontSizeOverride("font_size", (int)smallSize);
            _commentLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            _commentLabel.Visible = false;
            mainContainer.AddChild(_commentLabel);
            
            // Espaciador
            var spacer = new Control();
            spacer.Name = "Spacer";
            spacer.CustomMinimumSize = new Vector2(0, 10);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            mainContainer.AddChild(spacer);
            
            // Botón continuar
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.CustomMinimumSize = new Vector2(200, 50);
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
        }
        
        private void OnDocumentClicked(int documentIndex)
        {
            if (documentIndex < 0 || documentIndex >= _documentNames.Length) return;
            if (_placedDocuments.Contains(documentIndex)) return; // Ya fue seleccionado
            
            _documentsPlaced++;
            _placedDocuments.Add(documentIndex);
            string selectedDocument = _documentNames[documentIndex];
            
            // Efecto visual: el botón desaparece con animación
            var button = _documentButtons[documentIndex];
            var tween = CreateTween();
            tween.TweenProperty(button, "modulate", new Color(1, 1, 1, 0), 0.3f);
            tween.TweenCallback(Callable.From(() => button.Visible = false));
            
            // Mostrar resultado con efecto
            _feedbackLabel.Text = $"Seleccionaste: \"{selectedDocument}\"";
            _feedbackLabel.Visible = true;
            _feedbackLabel.Modulate = new Color(1, 1, 1, 0);
            var fadeTween = CreateTween();
            fadeTween.TweenProperty(_feedbackLabel, "modulate", new Color(1, 1, 1, 1), 0.3f);
            
            // Comentario aleatorio (siempre visible hasta seleccionar otro)
            string comment = _comments[_random.Next(_comments.Length)];
            _commentLabel.Text = $"Entrevistador: \"{comment}\"";
            _commentLabel.Visible = true;
            _commentLabel.Modulate = new Color(1, 1, 1, 0);
            var commentTween = CreateTween();
            commentTween.TweenProperty(_commentLabel, "modulate", new Color(1, 1, 1, 1), 0.3f);
            
            // Efecto de pulso en el comentario
            var pulseTween = CreateTween();
            pulseTween.SetLoops(2);
            pulseTween.TweenProperty(_commentLabel, "scale", new Vector2(1.05f, 1.05f), 0.15f);
            pulseTween.TweenProperty(_commentLabel, "scale", Vector2.One, 0.15f);
            
            // Si todos los documentos están seleccionados, mostrar botón continuar
            if (_documentsPlaced >= _documentNames.Length)
            {
                GetTree().CreateTimer(1.0f).Timeout += () => {
                    _continueButton.Visible = true;
                    StartButtonBlink(_continueButton); // Hacer parpadear el botón continuar
                };
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}
