using Core.Services;
using Godot;
using System;
using System.Collections.Generic;

namespace Aprendizdemago.Package.Selectors
{
    /// <summary>
    /// Clase Card refactorizada con imagen de fondo que cubre toda la carta
    /// El contenido aparece al hacer hover (similar a CSS)
    /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
    /// CRÍTICO: Usa anclas para posicionamiento responsive
    /// </summary>
    public partial class Card : Control
    {
        private TextureRect _backgroundImage;
        private Label _titleLabel;
        private VBoxContainer _hoverContentContainer;
        private ColorRect _hoverOverlay;
        private MarginContainer _hoverMarginContainer;
        private bool _isHovered = false;
        private string _pendingBackgroundPath;
        private Color? _pendingBackgroundColor;
        private List<(string text, FontManager.TextType textType, Color? color)> _pendingTexts = new List<(string, FontManager.TextType, Color?)>();
        
        public float Width { get; set; }
        public float Height { get; set; }
        
        /// <summary>
        /// Constructor de Card
        /// </summary>
        public Card()
        {
            // CRÍTICO: Configurar anclas ANTES de agregar al árbol
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopLeft);
            
            CustomMinimumSize = new Vector2(350, 500);
            Width = 350;
            Height = 500;
            
            // Configurar mouse filter para detectar hover
            MouseFilter = Control.MouseFilterEnum.Stop;
            
            // Configurar ZIndex
            ZIndex = 1;
            
            // Asegurar que sea visible
            Visible = true;
        }
        
        public override void _Ready()
        {
            base._Ready();
            SetupCard();
        }
        
        /// <summary>
        /// Configura la estructura de la card
        /// Estructura: Control (raíz) -> TextureRect (fondo) -> Label (título) -> ColorRect (overlay hover) -> VBoxContainer (contenido hover)
        /// </summary>
        private void SetupCard()
        {
            // CRÍTICO: Background image que cubre toda la carta (como CSS background-image)
            // ZIndex más bajo para que esté detrás de todo
            _backgroundImage = new TextureRect();
            _backgroundImage.Name = "BackgroundImage";
            _backgroundImage.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _backgroundImage.ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional;
            _backgroundImage.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
            _backgroundImage.MouseFilter = Control.MouseFilterEnum.Ignore;
            _backgroundImage.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _backgroundImage.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            _backgroundImage.ZIndex = 0; // Más bajo
            _backgroundImage.Visible = true;
            AddChild(_backgroundImage);
            MoveChild(_backgroundImage, 0); // Asegurar que esté al fondo
            
            // Título siempre visible sobre la imagen (parte inferior)
            // ZIndex alto para que esté siempre visible
            _titleLabel = new Label();
            _titleLabel.Name = "TitleLabel";
            _titleLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
            _titleLabel.OffsetBottom = -10;
            _titleLabel.OffsetLeft = 10;
            _titleLabel.OffsetRight = -10;
            _titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _titleLabel.VerticalAlignment = VerticalAlignment.Bottom;
            _titleLabel.AddThemeFontSizeOverride("font_size", (int)FontManager.GetScaledSize(FontManager.TextType.Subtitle));
            _titleLabel.AddThemeColorOverride("font_color", Colors.White);
            _titleLabel.AddThemeConstantOverride("outline_size", 4);
            _titleLabel.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
            _titleLabel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _titleLabel.ClipContents = true;
            _titleLabel.ZIndex = 2; // Alto para estar siempre visible
            _titleLabel.Visible = true;
            AddChild(_titleLabel);
            
            // Overlay oscuro que aparece al hacer hover
            // ZIndex medio para estar sobre la imagen pero debajo del contenido
            _hoverOverlay = new ColorRect();
            _hoverOverlay.Name = "HoverOverlay";
            _hoverOverlay.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _hoverOverlay.Color = new Color(0, 0, 0, 0.7f);
            _hoverOverlay.Visible = false;
            _hoverOverlay.MouseFilter = Control.MouseFilterEnum.Pass; // Permitir clics a través del overlay
            _hoverOverlay.ZIndex = 1; // Medio
            AddChild(_hoverOverlay);
            
            // Contenedor de contenido que aparece al hacer hover
            // Usar MarginContainer para agregar padding interno
            // ZIndex alto para estar sobre el overlay
            _hoverMarginContainer = new MarginContainer();
            _hoverMarginContainer.Name = "HoverMarginContainer";
            _hoverMarginContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _hoverMarginContainer.AddThemeConstantOverride("margin_left", 20);
            _hoverMarginContainer.AddThemeConstantOverride("margin_right", 20);
            _hoverMarginContainer.AddThemeConstantOverride("margin_top", 20);
            _hoverMarginContainer.AddThemeConstantOverride("margin_bottom", 20);
            _hoverMarginContainer.Visible = false;
            _hoverMarginContainer.ZIndex = 3; // Más alto para estar sobre todo
            _hoverMarginContainer.MouseFilter = Control.MouseFilterEnum.Pass; // Permitir que los hijos reciban clics
            AddChild(_hoverMarginContainer);
            
            _hoverContentContainer = new VBoxContainer();
            _hoverContentContainer.Name = "HoverContentContainer";
            _hoverContentContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _hoverContentContainer.AddThemeConstantOverride("separation", 10);
            _hoverContentContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _hoverContentContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            _hoverContentContainer.Visible = true; // Siempre visible dentro del contenedor
            _hoverContentContainer.ZIndex = 4; // Asegurar que esté sobre el overlay
            _hoverContentContainer.MouseFilter = Control.MouseFilterEnum.Pass; // Permitir que los hijos reciban clics
            _hoverMarginContainer.AddChild(_hoverContentContainer);
            
            // Aplicar background pendiente si existe
            if (!string.IsNullOrEmpty(_pendingBackgroundPath))
            {
                SetBackground(_pendingBackgroundPath);
                _pendingBackgroundPath = null;
            }
            else if (_pendingBackgroundColor.HasValue)
            {
                SetBackgroundFlat(_pendingBackgroundColor.Value);
                _pendingBackgroundColor = null;
            }
            
            // Aplicar título pendiente si existe
            ApplyPendingTitle();
            
            // Aplicar textos pendientes si existen
            ApplyPendingTexts();
            
            // Conectar señales de mouse para hover
            MouseEntered += OnMouseEntered;
            MouseExited += OnMouseExited;
        }
        
        /// <summary>
        /// Maneja cuando el mouse entra en la carta
        /// </summary>
        private void OnMouseEntered()
        {
            _isHovered = true;
            ShowHoverContent();
        }
        
        /// <summary>
        /// Maneja cuando el mouse sale de la carta
        /// </summary>
        private void OnMouseExited()
        {
            _isHovered = false;
            HideHoverContent();
        }
        
        /// <summary>
        /// Muestra el contenido al hacer hover
        /// </summary>
        private void ShowHoverContent()
        {
            _hoverOverlay.Visible = true;
            if (_hoverMarginContainer != null)
            {
                _hoverMarginContainer.Visible = true;
                // Asegurar que el contenedor de contenido sea visible
                if (_hoverContentContainer != null)
                {
                    _hoverContentContainer.Visible = true;
                }
            }
        }
        
        /// <summary>
        /// Oculta el contenido al salir del hover
        /// </summary>
        private void HideHoverContent()
        {
            _hoverOverlay.Visible = false;
            if (_hoverMarginContainer != null)
            {
                _hoverMarginContainer.Visible = false;
            }
        }
        
        /// <summary>
        /// Configura la imagen de fondo que cubre toda la carta
        /// </summary>
        /// <param name="imagePath">Ruta de la imagen de fondo</param>
        public void SetBackground(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                GD.PrintErr("[Card] Ruta de imagen de fondo vacía");
                return;
            }
            
            // Si aún no está inicializado, guardar para aplicar después
            if (_backgroundImage == null)
            {
                _pendingBackgroundPath = imagePath;
                GD.Print($"[Card] Background pendiente (aún no inicializado): {imagePath}");
                return;
            }
            
            var texture = GD.Load<Texture2D>(imagePath);
            if (texture != null)
            {
                _backgroundImage.Texture = texture;
                _backgroundImage.Visible = true;
                GD.Print($"[Card] Background configurado: {imagePath}");
            }
            else
            {
                GD.PrintErr($"[Card] No se pudo cargar la imagen de fondo: {imagePath}");
            }
        }
        
        /// <summary>
        /// Configura el background usando un color sólido
        /// </summary>
        /// <param name="color">Color del fondo</param>
        public void SetBackgroundFlat(Color color)
        {
            // Si aún no está inicializado, guardar para aplicar después
            if (_backgroundImage == null)
            {
                _pendingBackgroundColor = color;
                GD.Print($"[Card] Background plano pendiente (aún no inicializado): {color}");
                return;
            }
            
            // Crear una textura de color sólido
            var image = Image.CreateEmpty(1, 1, false, Image.Format.Rgba8);
            image.Fill(color);
            var texture = ImageTexture.CreateFromImage(image);
            _backgroundImage.Texture = texture;
            _backgroundImage.Visible = true;
            
            GD.Print($"[Card] Background plano configurado: {color}");
        }
        
        /// <summary>
        /// Agrega texto a la tarjeta (aparece al hacer hover)
        /// </summary>
        public void AddText(string text, FontManager.TextType textType, Color? textColor = null)
        {
            // Si aún no está inicializado, guardar para aplicar después
            if (_hoverContentContainer == null)
            {
                _pendingTexts.Add((text, textType, textColor));
                GD.Print($"[Card] Texto pendiente (aún no inicializado): {text.Substring(0, Math.Min(30, text.Length))}...");
                return;
            }
            
            var label = new Label();
            label.Name = $"Label_{_hoverContentContainer.GetChildCount()}";
            label.Text = text;
            label.AddThemeFontSizeOverride("font_size", (int)FontManager.GetScaledSize(textType));
            label.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            label.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            label.ClipContents = true;
            label.AddThemeColorOverride("font_color", textColor ?? Colors.White);
            label.AddThemeConstantOverride("outline_size", 3);
            label.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
            label.Visible = true;
            label.ZIndex = 5; // Más alto para estar sobre el overlay y el contenedor
            
            _hoverContentContainer.AddChild(label);
            GD.Print($"[Card] Texto agregado al hover: {text.Substring(0, Math.Min(30, text.Length))}...");
        }
        
        /// <summary>
        /// Aplica los textos pendientes después de la inicialización
        /// </summary>
        private void ApplyPendingTexts()
        {
            if (_pendingTexts.Count > 0 && _hoverContentContainer != null)
            {
                foreach (var (text, textType, color) in _pendingTexts)
                {
                    AddText(text, textType, color);
                }
                _pendingTexts.Clear();
            }
        }
        
        private string _pendingTitle;
        
        /// <summary>
        /// Establece el título de la carta (siempre visible)
        /// </summary>
        /// <param name="title">Texto del título</param>
        public void SetTitle(string title)
        {
            if (_titleLabel != null)
            {
                _titleLabel.Text = title;
                _titleLabel.Visible = true;
            }
            else
            {
                // Si aún no está inicializado, guardar para aplicar después
                _pendingTitle = title;
            }
        }
        
        /// <summary>
        /// Aplica el título pendiente después de la inicialización
        /// </summary>
        private void ApplyPendingTitle()
        {
            if (!string.IsNullOrEmpty(_pendingTitle) && _titleLabel != null)
            {
                _titleLabel.Text = _pendingTitle;
                _titleLabel.Visible = true;
                _pendingTitle = null;
            }
        }
        
        /// <summary>
        /// Agrega un botón a la tarjeta (aparece al hacer hover)
        /// </summary>
        public void AddButton(Button button, Vector2 position)
        {
            if (button == null)
            {
                GD.PrintErr("[Card] Botón es null");
                return;
            }
            
            if (_hoverContentContainer == null)
            {
                GD.PrintErr("[Card] HoverContentContainer no inicializado");
                return;
            }
            
            // CRÍTICO: Asegurar que el botón pueda recibir clics
            button.MouseFilter = Control.MouseFilterEnum.Stop; // El botón debe recibir clics
            button.Visible = true;
            
            // Si la posición es Vector2.Zero, centrar el botón
            if (position == Vector2.Zero)
            {
                var centerContainer = new CenterContainer();
                centerContainer.Name = "ButtonCenterContainer";
                centerContainer.CustomMinimumSize = new Vector2(0, 80);
                centerContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                centerContainer.SizeFlagsVertical = Control.SizeFlags.ShrinkEnd;
                centerContainer.Visible = true;
                centerContainer.MouseFilter = Control.MouseFilterEnum.Pass; // Permitir que el botón reciba clics
                centerContainer.AddChild(button);
                _hoverContentContainer.AddChild(centerContainer);
            }
            else
            {
                // CRÍTICO: Configurar anclas para el botón
                button.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopLeft);
                button.OffsetLeft = position.X;
                button.OffsetTop = position.Y;
                
                // Agregar al contenedor de hover
                _hoverContentContainer.AddChild(button);
            }
        }
        
        /// <summary>
        /// Agrega una imagen a la tarjeta (aparece al hacer hover)
        /// Este método ahora se usa para el background, pero se mantiene para compatibilidad
        /// </summary>
        public void AddImage(string imagePath, Vector2 size, TextureRect.StretchModeEnum stretchMode, TextureRect.ExpandModeEnum expandMode)
        {
            // Si se llama AddImage, configurar como background
            SetBackground(imagePath);
        }
        
        /// <summary>
        /// Actualiza la altura de la tarjeta
        /// </summary>
        public void UpdateHeight(float height = 0f)
        {
            if (height > 0)
            {
                Height = height;
                CustomMinimumSize = new Vector2(CustomMinimumSize.X, height);
            }
            else
            {
                // Calcular altura automáticamente basada en el contenido
                UpdateHeight();
            }
        }
        
        /// <summary>
        /// Actualiza la altura de la tarjeta sin parámetros (calcula automáticamente)
        /// </summary>
        public void UpdateHeight()
        {
            // La altura se mantiene fija basada en CustomMinimumSize
            // El contenido se ajusta dentro de la carta
        }
    }
}
