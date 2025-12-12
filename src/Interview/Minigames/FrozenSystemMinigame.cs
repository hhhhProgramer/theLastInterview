using Godot;
using SlimeKingdomChronicles.Core.UI;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Sistema Congelado - Reinicia los Paneles
    /// </summary>
    public partial class FrozenSystemMinigame : BaseMinigame
    {
        private List<Panel> _popupPanels;
        private Label _instructionLabel;
        private Button _restartButton;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        
        private int _closeCount = 0;
        private bool _restartButtonVisible = false;
        private bool _gameActive = false;
        
        public FrozenSystemMinigame(Node parent) : base(parent)
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
            mainContainer.OffsetLeft = 20;
            mainContainer.OffsetRight = -20;
            mainContainer.OffsetTop = 20;
            mainContainer.OffsetBottom = -20;
            mainContainer.AddThemeConstantOverride("separation", 15);
            panel.AddChild(mainContainer);
            
            // Título
            var titleLabel = new Label();
            titleLabel.Text = "SISTEMA CONGELADO";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle);
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.3f, 0.3f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Instrucción
            _instructionLabel = new Label();
            _instructionLabel.Text = "Cierra los paneles emergentes...";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float bodySize = FontManager.GetScaledSize(TextType.Body);
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _instructionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_instructionLabel);
            
            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 20);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            mainContainer.AddChild(spacer);
            
            // Botón reiniciar (oculto inicialmente, en la parte inferior)
            _restartButton = new Button();
            _restartButton.Text = "Reiniciar todo";
            _restartButton.CustomMinimumSize = new Vector2(200, 50);
            _restartButton.Visible = false;
            _restartButton.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _restartButton.AddThemeColorOverride("font_color", new Color(1.0f, 0.2f, 0.2f, 1.0f));
            _restartButton.Pressed += OnRestartPressed;
            mainContainer.AddChild(_restartButton);
            
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
            _popupPanels = new List<Panel>();
            CreateInitialPopup();
        }
        
        private void CreateInitialPopup()
        {
            CreatePopup();
        }
        
        private void CreatePopup()
        {
            // Obtener tamaño del viewport para calcular posiciones
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            
            var popup = new Panel();
            popup.Name = $"Popup_{_popupPanels.Count}";
            popup.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.Center);
            Vector2 popupSize = GetResponsiveSize(0.35f, 0.14f);
            popup.CustomMinimumSize = popupSize;
            popup.AnchorLeft = 0.5f;
            popup.AnchorTop = 0.5f;
            popup.AnchorRight = 0.5f;
            popup.AnchorBottom = 0.5f;
            
            // Posición aleatoria en todo el espacio horizontal
            // Calcular el rango máximo horizontal (mitad del viewport menos la mitad del ancho del popup)
            float maxHorizontalOffset = (viewportSize.X * 0.5f) - 200; // 200 es la mitad del ancho del popup
            int offsetX = _random.Next(-(int)maxHorizontalOffset, (int)maxHorizontalOffset);
            
            // Posición vertical: evitar la parte inferior donde está el botón de reiniciar
            int offsetY = _random.Next(-200, 50); // Máximo 50 hacia abajo para no tapar el botón
            
            popup.OffsetLeft = -200 + offsetX;
            popup.OffsetRight = 200 + offsetX;
            popup.OffsetTop = -100 + offsetY;
            popup.OffsetBottom = 100 + offsetY;
            
            // Cargar imagen de fondo del botón
            const string BUTTON_IMAGE_PATH = "res://src/Image/Gui/button_option.png";
            var buttonTexture = GD.Load<Texture2D>(BUTTON_IMAGE_PATH);
            
            if (buttonTexture != null)
            {
                // Usar StyleBoxTexture con la imagen de fondo
                var styleBox = new StyleBoxTexture();
                styleBox.Texture = buttonTexture;
                styleBox.ContentMarginLeft = 20;
                styleBox.ContentMarginRight = 20;
                styleBox.ContentMarginTop = 20;
                styleBox.ContentMarginBottom = 20;
                popup.AddThemeStyleboxOverride("panel", styleBox);
            }
            else
            {
                // Fallback: StyleBoxFlat si no se carga la imagen
                var styleBox = new StyleBoxFlat();
                styleBox.BgColor = new Color(0.2f, 0.2f, 0.2f, 0.98f);
                styleBox.BorderColor = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                styleBox.BorderWidthLeft = 3;
                styleBox.BorderWidthTop = 3;
                styleBox.BorderWidthRight = 3;
                styleBox.BorderWidthBottom = 3;
                styleBox.CornerRadiusTopLeft = 8;
                styleBox.CornerRadiusTopRight = 8;
                styleBox.CornerRadiusBottomLeft = 8;
                styleBox.CornerRadiusBottomRight = 8;
                popup.AddThemeStyleboxOverride("panel", styleBox);
            }
            
            AddChild(popup);
            
            // Contenedor del popup
            var container = new VBoxContainer();
            container.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            container.OffsetLeft = 15;
            container.OffsetRight = -15;
            container.OffsetTop = 40; // Aumentado de 15 a 40 para bajar el texto
            container.OffsetBottom = -15;
            container.AddThemeConstantOverride("separation", 10);
            popup.AddChild(container);
            
            // Mensaje del popup
            var messageLabel = new Label();
            if (_closeCount >= 5)
            {
                messageLabel.Text = "Error duplicado – multiplicando ventanas...";
            }
            else
            {
                messageLabel.Text = "Panel del sistema\n(Error de sistema)";
            }
            messageLabel.HorizontalAlignment = HorizontalAlignment.Center;
            messageLabel.VerticalAlignment = VerticalAlignment.Center;
            messageLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            float bodySize = FontManager.GetScaledSize(TextType.Body);
            messageLabel.AddThemeFontSizeOverride("font_size", (int)(bodySize * 0.9f));
            messageLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            container.AddChild(messageLabel);
            
            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 10);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            container.AddChild(spacer);
            
            // Botón cerrar
            var closeButton = new Button();
            closeButton.Text = "X Cerrar";
            closeButton.CustomMinimumSize = new Vector2(150, 40);
            closeButton.AddThemeFontSizeOverride("font_size", (int)(bodySize * 0.9f));
            closeButton.Pressed += () => OnClosePopup(popup);
            container.AddChild(closeButton);
            
            _popupPanels.Add(popup);
        }
        
        private void OnClosePopup(Panel popup)
        {
            if (!_gameActive) return;
            
            _closeCount++;
            popup.QueueFree();
            _popupPanels.Remove(popup);
            
            // Mostrar mensaje después de 5 cierres
            if (_closeCount >= 5 && !_restartButtonVisible)
            {
                _instructionLabel.Text = "Error: Los paneles se están multiplicando.\nBusca el botón 'Reiniciar todo' abajo.";
                _restartButton.Visible = true;
                _restartButtonVisible = true;
                StartButtonBlink(_restartButton); // Hacer parpadear el botón de reiniciar
            }
            
            // Crear 2 nuevos popups (duplicación exponencial)
            if (_popupPanels.Count < 15) // Limitar a 15 popups máximo para evitar saturación
            {
                CreatePopup(); // Primer popup
                CreatePopup(); // Segundo popup (duplicación)
            }
        }
        
        private void OnRestartPressed()
        {
            if (!_gameActive) return;
            
            _gameActive = false;
            
            // Cerrar todos los popups
            foreach (var popup in _popupPanels)
            {
                popup.QueueFree();
            }
            _popupPanels.Clear();
            
            _instructionLabel.Text = "Sistema reiniciado correctamente.";
            _instructionLabel.AddThemeColorOverride("font_color", new Color(0.3f, 1.0f, 0.3f, 1.0f));
            _restartButton.Visible = false;
            
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

