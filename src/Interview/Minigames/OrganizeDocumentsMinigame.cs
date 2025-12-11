using Godot;
using SlimeKingdomChronicles.Core.UI;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Ordena documentos - da igual dónde los sueltes, siempre comentarios aleatorios
    /// </summary>
    public partial class OrganizeDocumentsMinigame : BaseMinigame
    {
        private List<Control> _documents;
        private List<Control> _slots;
        private Label _instructionLabel;
        private Label _feedbackLabel;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        private int _documentsPlaced = 0;
        private string _selectedDocument = null;
        
        private string[] _slotLabels = { "Urgente", "No urgente", "Totalmente urgente" };
        
        private string[] _documentNames = {
            "Solicitud de descanso emocional",
            "Queja del pasante por existir",
            "Denuncia del ventilador"
        };
        
        private string[] _positiveFeedback = {
            "Excelente organización. Eres un genio del archivo.",
            "Perfecto. Esa es exactamente la carpeta correcta... creo.",
            "Bien hecho. Tu instinto organizacional es impresionante.",
            "Aprobado. Aunque técnicamente nada de esto tiene sentido.",
            "Perfecto. Has demostrado dominio total del caos."
        };
        
        private string[] _negativeFeedback = {
            "Eso no va ahí. ¿Estás seguro de que sabes leer?",
            "Incorrecto. Ese documento pertenece a... ¿dónde?",
            "Mal. Muy mal. Pero seguimos adelante porque sí.",
            "Eso está mal, pero me da igual. Siguiente.",
            "No, eso no es correcto. Pero tampoco importa realmente."
        };
        
        public OrganizeDocumentsMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo (más alto para evitar sobreposiciones)
            var panel = new Panel();
            panel.Name = "MinigamePanel";
            panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            panel.CustomMinimumSize = new Vector2(900, 650);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -450;
            panel.OffsetRight = 450;
            panel.OffsetTop = -325;
            panel.OffsetBottom = 325;
            
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
            
            // Contenedor superior con VBoxContainer para textos
            var topContainer = new VBoxContainer();
            topContainer.Name = "TopContainer";
            topContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            topContainer.OffsetTop = 15;
            topContainer.OffsetBottom = 230;
            topContainer.OffsetLeft = 20;
            topContainer.OffsetRight = -20;
            topContainer.AddThemeConstantOverride("separation", 10);
            panel.AddChild(topContainer);
            
            // Título
            var titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Text = "ORGANIZA LOS DOCUMENTOS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.2f, 1.0f));
            topContainer.AddChild(titleLabel);
            
            // Instrucción (más clara y compacta)
            _instructionLabel = new Label();
            _instructionLabel.Name = "InstructionLabel";
            _instructionLabel.Text = "1. Click en documento (izquierda) → 2. Click en carpeta (derecha)";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _instructionLabel.VerticalAlignment = VerticalAlignment.Center;
            _instructionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _instructionLabel.ClipContents = true;
            float instructionSize = FontManager.GetScaledSize(TextType.Body);
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _instructionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            topContainer.AddChild(_instructionLabel);
            
            // Feedback label (mucho más grande y visible)
            _feedbackLabel = new Label();
            _feedbackLabel.Name = "FeedbackLabel";
            _feedbackLabel.Text = "Selecciona un documento para comenzar...";
            _feedbackLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _feedbackLabel.VerticalAlignment = VerticalAlignment.Center;
            _feedbackLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _feedbackLabel.ClipContents = true;
            _feedbackLabel.CustomMinimumSize = new Vector2(0, 120);
            _feedbackLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _feedbackLabel.AddThemeColorOverride("font_color", new Color(0.8f, 1.0f, 0.6f, 1.0f));
            topContainer.AddChild(_feedbackLabel);
            
            _documents = new List<Control>();
            _slots = new List<Control>();
            
            // Crear documentos (lado izquierdo) - movidos más abajo para no sobreponerse con textos
            for (int i = 0; i < _documentNames.Length; i++)
            {
                var doc = CreateDocument(_documentNames[i], new Vector2(100, 250 + i * 90));
                _documents.Add(doc);
                panel.AddChild(doc);
            }
            
            // Crear slots (lado derecho) - movidos más abajo
            for (int i = 0; i < _slotLabels.Length; i++)
            {
                var slot = CreateSlot(_slotLabels[i], new Vector2(550, 250 + i * 90));
                _slots.Add(slot);
                panel.AddChild(slot);
            }
            
            // Botón continuar (más espacio desde arriba)
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Continuar";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -20;
            _continueButton.OffsetTop = -80;
            _continueButton.Visible = false;
            float buttonSize = FontManager.GetScaledSize(TextType.Body);
            _continueButton.AddThemeFontSizeOverride("font_size", (int)buttonSize);
            _continueButton.Pressed += OnContinuePressed;
            panel.AddChild(_continueButton);
        }
        
        private Control CreateDocument(string name, Vector2 position)
        {
            var doc = new Panel();
            doc.Name = $"Document_{name}";
            doc.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopLeft);
            doc.Position = position;
            doc.CustomMinimumSize = new Vector2(200, 70);
            doc.Size = new Vector2(200, 70);
            
            var docStyle = new StyleBoxFlat();
            docStyle.BgColor = new Color(0.9f, 0.9f, 0.7f, 1.0f);
            docStyle.BorderColor = new Color(0.5f, 0.5f, 0.3f, 1.0f);
            docStyle.BorderWidthLeft = 2;
            docStyle.BorderWidthTop = 2;
            docStyle.BorderWidthRight = 2;
            docStyle.BorderWidthBottom = 2;
            doc.AddThemeStyleboxOverride("panel", docStyle);
            
            var label = new Label();
            label.Text = name;
            label.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            label.ClipContents = true;
            float labelSize = FontManager.GetScaledSize(TextType.Small);
            label.AddThemeFontSizeOverride("font_size", (int)labelSize);
            doc.AddChild(label);
            
            // Hacer clickeable - seleccionar documento
            doc.GuiInput += (InputEvent @event) => {
                if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    OnDocumentClicked(name, doc);
                }
            };
            
            return doc;
        }
        
        private Control CreateSlot(string label, Vector2 position)
        {
            var slot = new Panel();
            slot.Name = $"Slot_{label}";
            slot.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopLeft);
            slot.Position = position;
            slot.CustomMinimumSize = new Vector2(200, 70);
            slot.Size = new Vector2(200, 70);
            
            var slotStyle = new StyleBoxFlat();
            slotStyle.BgColor = new Color(0.2f, 0.2f, 0.4f, 0.5f);
            slotStyle.BorderColor = new Color(0.4f, 0.4f, 0.6f, 1.0f);
            slotStyle.BorderWidthLeft = 2;
            slotStyle.BorderWidthTop = 2;
            slotStyle.BorderWidthRight = 2;
            slotStyle.BorderWidthBottom = 2;
            slot.AddThemeStyleboxOverride("panel", slotStyle);
            
            var slotLabel = new Label();
            slotLabel.Text = label;
            slotLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            slotLabel.HorizontalAlignment = HorizontalAlignment.Center;
            slotLabel.VerticalAlignment = VerticalAlignment.Center;
            slotLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            slotLabel.ClipContents = true;
            float labelSize = FontManager.GetScaledSize(TextType.Small);
            slotLabel.AddThemeFontSizeOverride("font_size", (int)labelSize);
            slot.AddChild(slotLabel);
            
            // Hacer clickeable - colocar documento en carpeta
            slot.GuiInput += (InputEvent @event) => {
                if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    OnSlotClicked(label, slot);
                }
            };
            
            return slot;
        }
        
        private void OnDocumentClicked(string documentName, Control doc)
        {
            // Seleccionar documento (resaltar visualmente)
            _selectedDocument = documentName;
            
            // Resaltar el documento seleccionado
            foreach (var d in _documents)
            {
                var style = d.GetThemeStylebox("panel") as StyleBoxFlat;
                if (style != null)
                {
                    if (d == doc)
                    {
                        style.BorderColor = new Color(1.0f, 1.0f, 0.0f, 1.0f); // Amarillo para seleccionado
                        style.BorderWidthLeft = 4;
                        style.BorderWidthTop = 4;
                        style.BorderWidthRight = 4;
                        style.BorderWidthBottom = 4;
                    }
                    else
                    {
                        style.BorderColor = new Color(0.5f, 0.5f, 0.3f, 1.0f); // Normal
                        style.BorderWidthLeft = 2;
                        style.BorderWidthTop = 2;
                        style.BorderWidthRight = 2;
                        style.BorderWidthBottom = 2;
                    }
                }
            }
            
            _feedbackLabel.Text = $"✓ Documento seleccionado:\n\"{documentName}\"\n\n→ Ahora haz click en una carpeta (derecha)";
        }
        
        private void OnSlotClicked(string slotName, Control slot)
        {
            if (string.IsNullOrEmpty(_selectedDocument))
            {
                _feedbackLabel.Text = "⚠ Primero selecciona un documento\nhaciendo click en él (lado izquierdo)";
                return;
            }
            
            _documentsPlaced++;
            
            // Comentario aleatorio (da igual dónde lo pongas)
            bool isPositive = _random.Next(0, 2) == 0;
            string feedback = isPositive 
                ? _positiveFeedback[_random.Next(_positiveFeedback.Length)]
                : _negativeFeedback[_random.Next(_negativeFeedback.Length)];
            
            _feedbackLabel.Text = $"✓ Colocado: \"{_selectedDocument}\"\n→ En carpeta: \"{slotName}\"\n\n💬 Entrevistador:\n\"{feedback}\"";
            
            // Deseleccionar documento
            _selectedDocument = null;
            foreach (var d in _documents)
            {
                var style = d.GetThemeStylebox("panel") as StyleBoxFlat;
                if (style != null)
                {
                    style.BorderColor = new Color(0.5f, 0.5f, 0.3f, 1.0f);
                    style.BorderWidthLeft = 2;
                    style.BorderWidthTop = 2;
                    style.BorderWidthRight = 2;
                    style.BorderWidthBottom = 2;
                }
            }
            
            // Después de colocar todos los documentos, mostrar botón continuar
            if (_documentsPlaced >= _documentNames.Length)
            {
                GetTree().CreateTimer(2.0f).Timeout += () => {
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
