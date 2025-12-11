using Godot;
using Package.Audio;

namespace Core.Package.UI
{
    /// <summary>
    /// Card genérica reutilizable para popups y paneles
    /// Utiliza MarginContainer como nodo raíz para manejar márgenes de forma nativa
    /// Estructura: MarginContainer (raíz, margin) -> Background -> MarginContainer (padding) -> VBoxContainer (contenido)
    /// Similar a CSS: margin (externo) y padding (interno)
    /// </summary>
    public partial class GenericCard : MarginContainer
    {
        private TextureRect _backgroundTextureRect;
        private MarginContainer _paddingContainer;
        private VBoxContainer _vboxContainer;
        private string _backgroundImagePath;
        private bool _isInitialized = false;
        private System.Collections.Generic.List<PendingAction> _pendingActions = new();
        
        // Constantes para márgenes y padding por defecto
        private int _marginTop = 0; // Margen superior externo
        private int _marginRight = 0; // Margen derecho externo
        private int _marginBottom = 0; // Margen inferior externo
        private int _marginLeft = 0; // Margen izquierdo externo
        
        private int _paddingTop = 50; // Padding superior interno
        private int _paddingRight = 50; // Padding derecho interno
        private int _paddingBottom = 50; // Padding inferior interno
        private int _paddingLeft = 50; // Padding izquierdo interno
        
        private const int CONTENT_SEPARATION = 20; // Separación entre elementos en VBoxContainer
        
        private class PendingAction
        {
            public System.Action Action { get; set; }
        }
        
        public GenericCard(string backgroundImagePath = "res://src/Image/Gemini_Generated_Image_11b5vz11b5vz11b5.png")
        {
            _backgroundImagePath = backgroundImagePath;
            MouseFilter = Control.MouseFilterEnum.Stop;
            ZIndex = 100; // Por encima de todo por defecto
        }
        
        public override void _Ready()
        {
            base._Ready();
            InitializeCard();
        }
        
        /// <summary>
        /// Inicializa la card de forma segura, se puede llamar múltiples veces
        /// </summary>
        private void InitializeCard()
        {
            if (_isInitialized)
                return;
                
            SetupCard();
            _isInitialized = true;
            
            // Ejecutar acciones pendientes usando CallDeferred para asegurar que todo esté listo
            if (_pendingActions.Count > 0)
            {
                CallDeferred(MethodName.ExecutePendingActions);
            }
        }
        
        /// <summary>
        /// Ejecuta las acciones pendientes después de que la card esté completamente inicializada
        /// </summary>
        private void ExecutePendingActions()
        {
            // Usar CallDeferred para asegurar que todo esté completamente listo
            CallDeferred(MethodName.ExecutePendingActionsInternal);
        }
        
        /// <summary>
        /// Ejecuta las acciones pendientes internamente
        /// </summary>
        private void ExecutePendingActionsInternal()
        {
            while (_pendingActions.Count > 0)
            {
                var pendingAction = _pendingActions[0];
                _pendingActions.RemoveAt(0);
                pendingAction.Action?.Invoke();
            }
        }
        
        /// <summary>
        /// Configura la estructura básica de la card
        /// Estructura: MarginContainer (raíz, margin) -> Background -> MarginContainer (padding) -> VBoxContainer (contenido)
        /// </summary>
        private void SetupCard()
        {
            // Evitar crear múltiples veces
            if (_vboxContainer != null)
                return;
            
            // Configurar márgenes externos del MarginContainer raíz (margin en CSS)
            AddThemeConstantOverride("margin_left", _marginLeft);
            AddThemeConstantOverride("margin_right", _marginRight);
            AddThemeConstantOverride("margin_top", _marginTop);
            AddThemeConstantOverride("margin_bottom", _marginBottom);
            
            // Background - TextureRect que cubre todo el MarginContainer raíz
            // Se agrega primero para que quede detrás del contenido
            _backgroundTextureRect = new TextureRect();
            _backgroundTextureRect.Name = "BackgroundTextureRect";
            _backgroundTextureRect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            // ExpandMode: FitWidthProportional se adapta tanto en alto como en ancho
            _backgroundTextureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional;
            // StretchMode: KeepAspectCovered cubre todo el área manteniendo aspecto
            _backgroundTextureRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered;
            _backgroundTextureRect.MouseFilter = Control.MouseFilterEnum.Ignore;
            // SizeFlags para que se expanda y llene todo el espacio
            _backgroundTextureRect.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _backgroundTextureRect.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            
            // Cargar textura de fondo
            var backgroundTexture = GD.Load<Texture2D>(_backgroundImagePath);
            if (backgroundTexture != null)
            {
                _backgroundTextureRect.Texture = backgroundTexture;
            }
            
            // Agregar background primero (índice 0) para que quede detrás
            AddChild(_backgroundTextureRect);
            MoveChild(_backgroundTextureRect, 0);
            
            // MarginContainer interno para padding (padding en CSS)
            _paddingContainer = new MarginContainer();
            _paddingContainer.Name = "PaddingContainer";
            _paddingContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _paddingContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _paddingContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            // Configurar padding (márgenes internos)
            _paddingContainer.AddThemeConstantOverride("margin_left", _paddingLeft);
            _paddingContainer.AddThemeConstantOverride("margin_right", _paddingRight);
            _paddingContainer.AddThemeConstantOverride("margin_top", _paddingTop);
            _paddingContainer.AddThemeConstantOverride("margin_bottom", _paddingBottom);
            AddChild(_paddingContainer);
            
            // VBoxContainer para el contenido - dentro del padding container
            _vboxContainer = new VBoxContainer();
            _vboxContainer.Name = "VBoxContainer";
            _vboxContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _vboxContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _vboxContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            _vboxContainer.AddThemeConstantOverride("separation", CONTENT_SEPARATION);
            _paddingContainer.AddChild(_vboxContainer);
        }
        
        /// <summary>
        /// Asegura que la card esté inicializada antes de agregar contenido
        /// </summary>
        private void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                // Si estamos en el árbol, inicializar inmediatamente
                if (IsInsideTree())
                {
                    InitializeCard();
                }
                else
                {
                    // Si no estamos en el árbol, esperar a _Ready()
                    // Las acciones se ejecutarán cuando se inicialice
                }
            }
        }
        
        /// <summary>
        /// Obtiene el VBoxContainer para agregar contenido
        /// </summary>
        public VBoxContainer GetContentContainer()
        {
            EnsureInitialized();
            return _vboxContainer;
        }
        
        /// <summary>
        /// Establece los márgenes externos de la card (margin en CSS)
        /// Si solo se proporciona un valor, se aplica a todos los lados
        /// </summary>
        public void SetMargin(int margin)
        {
            SetMargin(margin, margin, margin, margin);
        }
        
        /// <summary>
        /// Establece los márgenes externos de la card individualmente (margin en CSS)
        /// </summary>
        public void SetMargin(int top, int right, int bottom, int left)
        {
            _marginTop = top;
            _marginRight = right;
            _marginBottom = bottom;
            _marginLeft = left;
            
            // Actualizar márgenes del MarginContainer raíz
            AddThemeConstantOverride("margin_left", _marginLeft);
            AddThemeConstantOverride("margin_right", _marginRight);
            AddThemeConstantOverride("margin_top", _marginTop);
            AddThemeConstantOverride("margin_bottom", _marginBottom);
        }
        
        /// <summary>
        /// Establece el padding interno de la card (padding en CSS)
        /// Si solo se proporciona un valor, se aplica a todos los lados
        /// </summary>
        public void SetPadding(int padding)
        {
            SetPadding(padding, padding, padding, padding);
        }
        
        /// <summary>
        /// Establece el padding interno de la card individualmente (padding en CSS)
        /// </summary>
        public void SetPadding(int top, int right, int bottom, int left)
        {
            _paddingTop = top;
            _paddingRight = right;
            _paddingBottom = bottom;
            _paddingLeft = left;
            
            // Actualizar padding del MarginContainer interno
            if (_paddingContainer != null)
            {
                _paddingContainer.AddThemeConstantOverride("margin_left", _paddingLeft);
                _paddingContainer.AddThemeConstantOverride("margin_right", _paddingRight);
                _paddingContainer.AddThemeConstantOverride("margin_top", _paddingTop);
                _paddingContainer.AddThemeConstantOverride("margin_bottom", _paddingBottom);
            }
        }
        
        /// <summary>
        /// Ejecuta una acción después de que la card esté completamente inicializada
        /// Si ya está inicializada, la ejecuta inmediatamente
        /// </summary>
        public void ExecuteWhenReady(System.Action action)
        {
            if (_isInitialized && IsInsideTree() && _vboxContainer != null)
            {
                // Ya está lista, ejecutar inmediatamente
                action?.Invoke();
            }
            else
            {
                // Encolar para ejecutar cuando esté lista
                _pendingActions.Add(new PendingAction
                {
                    Action = action
                });
            }
        }
        
        /// <summary>
        /// Agrega un Label al contenido
        /// </summary>
        public Label AddLabel(string text, float fontSize, HorizontalAlignment alignment = HorizontalAlignment.Center)
        {
            if (_isInitialized && IsInsideTree() && _vboxContainer != null)
            {
                return AddLabelInternal(text, fontSize, alignment);
            }
            else
            {
                // Si no está inicializada, encolar la acción para ejecutarla cuando esté lista
                var capturedText = text;
                var capturedFontSize = fontSize;
                var capturedAlignment = alignment;
                _pendingActions.Add(new PendingAction
                {
                    Action = () => {
                        AddLabelInternal(capturedText, capturedFontSize, capturedAlignment);
                    }
                });
                // Retornar null ya que el label se creará cuando se ejecute la acción
                return null;
            }
        }
        
        private Label AddLabelInternal(string text, float fontSize, HorizontalAlignment alignment)
        {
            if (_vboxContainer == null)
                return null;
                
            var label = new Label();
            label.Text = text;
            label.HorizontalAlignment = alignment;
            label.AddThemeFontSizeOverride("font_size", (int)fontSize);
            label.AddThemeColorOverride("font_color", Colors.White);
            label.AddThemeConstantOverride("outline_size", 4);
            label.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
            _vboxContainer.AddChild(label);
            return label;
        }
        
        /// <summary>
        /// Agrega un HBoxContainer al contenido
        /// </summary>
        public HBoxContainer AddHBoxContainer(int separation = 10, BoxContainer.AlignmentMode alignment = BoxContainer.AlignmentMode.Center)
        {
            if (_isInitialized && IsInsideTree() && _vboxContainer != null)
            {
                return AddHBoxContainerInternal(separation, alignment);
            }
            else
            {
                // Si no está inicializada, encolar la acción para ejecutarla cuando esté lista
                var capturedSeparation = separation;
                var capturedAlignment = alignment;
                HBoxContainer result = null;
                _pendingActions.Add(new PendingAction
                {
                    Action = () => {
                        result = AddHBoxContainerInternal(capturedSeparation, capturedAlignment);
                    }
                });
                // Retornar null ya que el hbox se creará cuando se ejecute la acción
                return null;
            }
        }
        
        private HBoxContainer AddHBoxContainerInternal(int separation, BoxContainer.AlignmentMode alignment)
        {
            if (_vboxContainer == null)
                return null;
                
            var hbox = new HBoxContainer();
            hbox.AddThemeConstantOverride("separation", separation);
            hbox.Alignment = alignment;
            _vboxContainer.AddChild(hbox);
            return hbox;
        }
        
        /// <summary>
        /// Agrega un botón al contenido
        /// </summary>
        public Button AddButton(string text, float fontSize, System.Action onPressed = null)
        {
            if (_isInitialized && IsInsideTree() && _vboxContainer != null)
            {
                return AddButtonInternal(text, fontSize, onPressed);
            }
            else
            {
                // Si no está inicializada, encolar la acción para ejecutarla cuando esté lista
                var capturedText = text;
                var capturedFontSize = fontSize;
                var capturedOnPressed = onPressed;
                _pendingActions.Add(new PendingAction
                {
                    Action = () => {
                        AddButtonInternal(capturedText, capturedFontSize, capturedOnPressed);
                    }
                });
                // Retornar null ya que el botón se creará cuando se ejecute la acción
                return null;
            }
        }
        
        private Button AddButtonInternal(string text, float fontSize, System.Action onPressed)
        {
            if (_vboxContainer == null)
                return null;
                
            var button = new Button();
            button.Text = text;
            button.AddThemeFontSizeOverride("font_size", (int)fontSize);
            button.AddThemeColorOverride("font_color", Colors.White);
            button.AddThemeConstantOverride("outline_size", 4);
            button.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
            
            // Configurar sonido de selección automático
            AudioManager.SetupButtonSelectSound(button);
            
            if (onPressed != null)
            {
                button.Pressed += onPressed;
            }
            _vboxContainer.AddChild(button);
            return button;
        }
        
        /// <summary>
        /// Crea y agrega la card al nodo padre especificado
        /// Permite controlar dónde se crea el card y evitar que tome el tamaño del contenedor padre
        /// </summary>
        /// <param name="parent">Nodo padre donde se agregará el card</param>
        /// <param name="position">Posición del card (opcional, Vector2.Zero por defecto)</param>
        /// <param name="size">Tamaño del card (opcional, Vector2.Zero por defecto)</param>
        /// <returns>La instancia del GenericCard creada</returns>
        public static GenericCard Create(Node parent, Vector2? position = null, Vector2? size = null)
        {
            var card = new GenericCard();
            
            // Configurar tamaño si se proporciona
            if (size.HasValue)
            {
                card.CustomMinimumSize = size.Value;
                card.Size = size.Value;
            }
            
            // Configurar posición si se proporciona
            if (position.HasValue)
            {
                card.Position = position.Value;
            }
            
            // Agregar al nodo padre
            parent.AddChild(card);
            
            // Asegurar que esté visible
            card.Visible = true;
            
            // La inicialización se hará automáticamente en _Ready()
            return card;
        }
        
        /// <summary>
        /// Muestra la card en un CanvasLayer con layer alto para estar por encima de todo
        /// Todo se maneja internamente, solo necesita ser llamada una vez
        /// </summary>
        public void ShowOnTop(Node parent, Vector2 position, Vector2 size)
        {
            // Verificar si ya está mostrada para evitar duplicados
            if (IsInsideTree() && GetParent() is CanvasLayer)
            {
                // Ya está mostrada, solo actualizar posición y tamaño
                Position = position;
                Size = size;
                CustomMinimumSize = size;
                return;
            }
            
            // Crear CanvasLayer con layer alto
            var canvasLayer = new CanvasLayer();
            canvasLayer.Name = "GenericCardCanvasLayer";
            canvasLayer.Layer = 100; // Layer muy alto para estar por encima de todo
            parent.AddChild(canvasLayer);
            
            // Configurar tamaño y posición
            CustomMinimumSize = size;
            Size = size;
            Position = position;
            
            // Agregar la card al CanvasLayer
            canvasLayer.AddChild(this);
            
            // Asegurar que esté visible
            Visible = true;
            
            // La inicialización se hará automáticamente en _Ready()
        }
        
        /// <summary>
        /// Cierra la card y elimina su CanvasLayer
        /// </summary>
        public void Close()
        {
            var parent = GetParent();
            if (parent is CanvasLayer canvasLayer)
            {
                QueueFree();
                canvasLayer.QueueFree();
            }
            else
            {
                QueueFree();
            }
        }
    }
}
