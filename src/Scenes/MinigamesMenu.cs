using Godot;
using Package.Background;
using SlimeKingdomChronicles.Core.UI;
using TheLastInterview.Interview.Minigames;
using System.Collections.Generic;

namespace TheLastInterview.Scenes
{
    /// <summary>
    /// Escena del menú de selección de minijuegos
    /// </summary>
    public partial class MinigamesMenu : Control
    {
        private Container _buttonsContainer; // Cambiado a Container para soportar GridContainer
        private Button _backButton;
        private SceneBackground _background;
        private BaseMinigame _currentMinigame;
        private CanvasLayer _minigameLayer;
        
        public override void _Ready()
        {
            base._Ready();
            
            // Configurar Control raíz
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            var viewport = GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440);
            CustomMinimumSize = viewportSize;
            
            // Configurar background
            SetupBackground();
            
            // Crear UI
            CreateUI();
        }
        
        /// <summary>
        /// Configura el background
        /// </summary>
        private void SetupBackground()
        {
            _background = new SceneBackground();
            _background.SetBackground("res://src/Image/Background/backgroun_office.png", new Color(0.05f, 0.05f, 0.05f, 1.0f));
            AddChild(_background);
            MoveChild(_background, 0);
        }
        
        /// <summary>
        /// Crea la UI del menú de minijuegos
        /// </summary>
        private void CreateUI()
        {
            // Contenedor principal centrado
            var centerContainer = new CenterContainer();
            centerContainer.Name = "CenterContainer";
            centerContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            AddChild(centerContainer);
            
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.CustomMinimumSize = new Vector2(600, 500);
            mainContainer.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
            mainContainer.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
            mainContainer.AddThemeConstantOverride("separation", 20);
            centerContainer.AddChild(mainContainer);
            
            // Título
            var titleLabel = new Label();
            titleLabel.Name = "TitleLabel";
            titleLabel.Text = "MINIJUEGOS";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float titleFontSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.5f;
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleFontSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.8f, 0.0f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Subtítulo
            var subtitleLabel = new Label();
            subtitleLabel.Name = "SubtitleLabel";
            subtitleLabel.Text = "Selecciona un minijuego para jugar";
            subtitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float subtitleFontSize = FontManager.GetScaledSize(TextType.Body);
            subtitleLabel.AddThemeFontSizeOverride("font_size", (int)subtitleFontSize);
            subtitleLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 0.8f, 1.0f));
            mainContainer.AddChild(subtitleLabel);
            
            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 30);
            mainContainer.AddChild(spacer);
            
            // Contenedor de botones de minijuegos (Grid para que quepan todos)
            var gridContainer = new GridContainer();
            gridContainer.Name = "ButtonsContainer";
            gridContainer.Columns = 2; // 2 columnas para que quepan mejor
            gridContainer.AddThemeConstantOverride("h_separation", 20);
            gridContainer.AddThemeConstantOverride("v_separation", 15);
            mainContainer.AddChild(gridContainer);
            
            _buttonsContainer = gridContainer; // Mantener referencia para compatibilidad
            
            // Crear botones para cada minijuego
            CreateMinigameButtons();
            
            // Espaciador
            var spacer2 = new Control();
            spacer2.CustomMinimumSize = new Vector2(0, 30);
            mainContainer.AddChild(spacer2);
            
            // Botón volver al menú
            _backButton = new Button();
            _backButton.Name = "BackButton";
            _backButton.Text = "Volver al Menú Principal";
            _backButton.CustomMinimumSize = new Vector2(300, 60);
            _backButton.AddThemeFontSizeOverride("font_size", (int)subtitleFontSize);
            _backButton.Pressed += OnBackButtonPressed;
            mainContainer.AddChild(_backButton);
        }
        
        /// <summary>
        /// Crea los botones para cada minijuego con iconos
        /// </summary>
        private void CreateMinigameButtons()
        {
            var minigameData = new Dictionary<MinigameManager.MinigameType, (string name, string icon)>
            {
                { MinigameManager.MinigameType.LieDetector, ("Detector de Mentiras", "🔍") },
                { MinigameManager.MinigameType.TypeName, ("Escribe tu Nombre", "⌨️") },
                { MinigameManager.MinigameType.OrganizeDocuments, ("Organiza Documentos", "📁") },
                { MinigameManager.MinigameType.TechnicalTest, ("Prueba Técnica", "💻") },
                { MinigameManager.MinigameType.StayCalm, ("Mantén la Calma", "🧘") },
                { MinigameManager.MinigameType.ArchiveFiles, ("Archivar Archivos", "📂") },
                { MinigameManager.MinigameType.TypeReport, ("Escribir Reporte", "📝") },
                { MinigameManager.MinigameType.AnswerPhone, ("Responder Teléfono", "📞") },
                { MinigameManager.MinigameType.DeleteSpam, ("Eliminar Spam", "🗑️") },
                { MinigameManager.MinigameType.FrozenSystem, ("Sistema Congelado", "❄️") }
            };
            
            float buttonFontSize = FontManager.GetScaledSize(TextType.Body);
            
            foreach (var kvp in minigameData)
            {
                // Crear botón con icono y texto
                var button = new Button();
                button.Text = $"{kvp.Value.icon} {kvp.Value.name}"; // Icono + texto directamente en el botón
                button.CustomMinimumSize = new Vector2(300, 60); // Más compacto para que quepan 2 por línea
                button.AddThemeFontSizeOverride("font_size", (int)buttonFontSize);
                
                // Estilo mejorado para los botones
                var styleBox = new StyleBoxFlat();
                styleBox.BgColor = new Color(0.2f, 0.2f, 0.2f, 0.95f);
                styleBox.BorderColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                styleBox.BorderWidthLeft = 2;
                styleBox.BorderWidthTop = 2;
                styleBox.BorderWidthRight = 2;
                styleBox.BorderWidthBottom = 2;
                styleBox.CornerRadiusTopLeft = 8;
                styleBox.CornerRadiusTopRight = 8;
                styleBox.CornerRadiusBottomLeft = 8;
                styleBox.CornerRadiusBottomRight = 8;
                button.AddThemeStyleboxOverride("normal", styleBox);
                
                // Hover style
                var hoverStyle = new StyleBoxFlat();
                hoverStyle.BgColor = new Color(0.3f, 0.3f, 0.3f, 0.95f);
                hoverStyle.BorderColor = new Color(0.7f, 0.7f, 0.7f, 1.0f);
                hoverStyle.BorderWidthLeft = 2;
                hoverStyle.BorderWidthTop = 2;
                hoverStyle.BorderWidthRight = 2;
                hoverStyle.BorderWidthBottom = 2;
                hoverStyle.CornerRadiusTopLeft = 8;
                hoverStyle.CornerRadiusTopRight = 8;
                hoverStyle.CornerRadiusBottomLeft = 8;
                hoverStyle.CornerRadiusBottomRight = 8;
                button.AddThemeStyleboxOverride("hover", hoverStyle);
                
                MinigameManager.MinigameType minigameType = kvp.Key;
                button.Pressed += () => OnMinigameSelected(minigameType);
                
                _buttonsContainer.AddChild(button);
            }
        }
        
        /// <summary>
        /// Se llama cuando se selecciona un minijuego
        /// </summary>
        private void OnMinigameSelected(MinigameManager.MinigameType minigameType)
        {
            // Ocultar el menú
            var centerContainer = GetNode<CenterContainer>("CenterContainer");
            centerContainer.Visible = false;
            
            // Crear CanvasLayer para el minijuego
            _minigameLayer = new CanvasLayer();
            _minigameLayer.Name = "MinigameLayer";
            _minigameLayer.Layer = 3000;
            AddChild(_minigameLayer);
            
            // Crear y mostrar el minijuego
            _currentMinigame = MinigameManager.CreateMinigame(minigameType, this);
            _currentMinigame.OnMinigameFinished += OnMinigameFinished;
            _minigameLayer.AddChild(_currentMinigame);
            
            // Mostrar el minijuego
            _currentMinigame.ShowMinigame();
        }
        
        /// <summary>
        /// Se llama cuando termina un minijuego
        /// </summary>
        private void OnMinigameFinished()
        {
            if (_currentMinigame != null && IsInstanceValid(_currentMinigame))
            {
                _currentMinigame.OnMinigameFinished -= OnMinigameFinished;
                
                // Limpiar el CanvasLayer del minijuego
                if (_minigameLayer != null)
                {
                    _minigameLayer.QueueFree();
                    _minigameLayer = null;
                }
                
                _currentMinigame = null;
            }
            
            // Mostrar el menú nuevamente
            var centerContainer = GetNode<CenterContainer>("CenterContainer");
            centerContainer.Visible = true;
        }
        
        /// <summary>
        /// Se llama cuando se presiona el botón Volver
        /// </summary>
        private void OnBackButtonPressed()
        {
            GetTree().ChangeSceneToFile("res://src/Scenes/MainMenu.tscn");
        }
    }
}

