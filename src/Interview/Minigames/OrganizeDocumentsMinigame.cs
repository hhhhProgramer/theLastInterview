using Godot;
using SlimeKingdomChronicles.Core.UI;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Acomoda documentos donde nada encaja
    /// </summary>
    public partial class OrganizeDocumentsMinigame : BaseMinigame
    {
        private List<Control> _documents;
        private List<Control> _slots;
        private Label _instructionLabel;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        
        public OrganizeDocumentsMinigame(Node parent) : base(parent)
        {
        }
        
        protected override void CreateUI()
        {
            // Panel de fondo (más grande para que quepan los textos)
            var panel = new Panel();
            panel.Name = "MinigamePanel";
            panel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            panel.CustomMinimumSize = new Vector2(900, 550);
            panel.AnchorLeft = 0.5f;
            panel.AnchorTop = 0.5f;
            panel.AnchorRight = 0.5f;
            panel.AnchorBottom = 0.5f;
            panel.OffsetLeft = -450;
            panel.OffsetRight = 450;
            panel.OffsetTop = -275;
            panel.OffsetBottom = 275;
            
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
            titleLabel.Text = "ORGANIZA LOS DOCUMENTOS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            titleLabel.ClipContents = true;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.2f, 1.0f));
            titleLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            titleLabel.OffsetTop = 15;
            titleLabel.OffsetBottom = 55;
            titleLabel.OffsetLeft = 20;
            titleLabel.OffsetRight = -20;
            panel.AddChild(titleLabel);
            
            // Instrucción
            _instructionLabel = new Label();
            _instructionLabel.Name = "InstructionLabel";
            _instructionLabel.Text = "Haz click en los documentos para intentar organizarlos.\n(Los tamaños no coinciden, nada encaja)";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _instructionLabel.VerticalAlignment = VerticalAlignment.Center;
            _instructionLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _instructionLabel.ClipContents = true;
            float instructionSize = FontManager.GetScaledSize(TextType.Body);
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)instructionSize);
            _instructionLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
            _instructionLabel.OffsetTop = 65;
            _instructionLabel.OffsetBottom = 115;
            _instructionLabel.OffsetLeft = 30;
            _instructionLabel.OffsetRight = -30;
            panel.AddChild(_instructionLabel);
            
            _documents = new List<Control>();
            _slots = new List<Control>();
            
            // Crear documentos (lado izquierdo)
            string[] documentNames = { "CV.pdf", "Contrato.docx", "Factura.xlsx", "Foto.jpg" };
            for (int i = 0; i < documentNames.Length; i++)
            {
                var doc = CreateDocument(documentNames[i], new Vector2(100, 150 + i * 80));
                _documents.Add(doc);
                panel.AddChild(doc);
            }
            
            // Crear slots (lado derecho) - pero con tamaños que no encajan
            string[] slotLabels = { "IMPORTANTE", "URGENTE", "ARCHIVAR", "BASURA" };
            for (int i = 0; i < slotLabels.Length; i++)
            {
                var slot = CreateSlot(slotLabels[i], new Vector2(500, 150 + i * 80));
                _slots.Add(slot);
                panel.AddChild(slot);
            }
            
            // Botón continuar
            _continueButton = new Button();
            _continueButton.Name = "ContinueButton";
            _continueButton.Text = "Nada encaja, continuar de todas formas";
            _continueButton.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _continueButton.OffsetBottom = -20;
            _continueButton.OffsetTop = -80;
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
            doc.CustomMinimumSize = new Vector2(120, 60);
            doc.Size = new Vector2(120, 60);
            
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
            
            // Hacer que se pueda arrastrar (pero nunca encaja)
            doc.GuiInput += (InputEvent @event) => {
                if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
                {
                    _instructionLabel.Text = $"Intentando colocar {name}... No encaja en ningún lado.";
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
            // Tamaños diferentes para que nada encaje
            var sizes = new Vector2[] { 
                new Vector2(150, 50),  // Más pequeño
                new Vector2(180, 70), // Más grande
                new Vector2(140, 55), // Mediano pero diferente
                new Vector2(160, 65)  // Otro tamaño
            };
            int index = _slots.Count;
            slot.CustomMinimumSize = sizes[index % sizes.Length];
            slot.Size = sizes[index % sizes.Length];
            
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
            
            return slot;
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

