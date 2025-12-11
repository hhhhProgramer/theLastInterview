using Core.Package.UI;
using Godot;
using System;

namespace Package.UI
{
    /// <summary>
    /// Manager independiente para crear popups usando GenericCard
    /// Permite configurar callbacks (OnConfirm, OnCancel, etc.) como en JavaScript
    /// Completamente independiente - solo hacer new y configurar
    /// </summary>
    public class PopupManager
    {
        /// <summary>
        /// Callback que se ejecuta cuando se confirma el popup
        /// </summary>
        public Action OnConfirm { get; set; }
        
        /// <summary>
        /// Callback que se ejecuta cuando se cancela el popup
        /// </summary>
        public Action OnCancel { get; set; }
        
        /// <summary>
        /// Callback que se ejecuta cuando se cierra el popup
        /// </summary>
        public Action OnClose { get; set; }
        
        /// <summary>
        /// Callback que se ejecuta cuando se muestra el popup
        /// </summary>
        public Action OnShow { get; set; }
        
        /// <summary>
        /// Callback que se ejecuta cuando se oculta el popup
        /// </summary>
        public Action OnHide { get; set; }
        
        /// <summary>
        /// Instancia del GenericCard usado para el popup
        /// </summary>
        private GenericCard _popupCard;
        
        /// <summary>
        /// CanvasLayer para mostrar el popup por encima de todo
        /// </summary>
        private CanvasLayer _canvasLayer;
        
        /// <summary>
        /// Nodo padre donde se mostrará el popup
        /// </summary>
        private Node _parent;
        
        /// <summary>
        /// Indica si el popup está visible
        /// </summary>
        public bool IsVisible => _popupCard != null && _popupCard.IsInsideTree() && _popupCard.Visible;
        
        /// <summary>
        /// Constructor del PopupManager
        /// </summary>
        public PopupManager()
        {
            // Inicialización vacía - todo se configura con métodos
        }
        
        /// <summary>
        /// Configura el fondo del popup
        /// </summary>
        /// <param name="backgroundImagePath">Ruta de la imagen de fondo</param>
        /// <returns>Esta instancia para encadenar métodos (fluent interface)</returns>
        public PopupManager SetBackground(string backgroundImagePath)
        {
            // Se aplicará cuando se cree el card
            _backgroundImagePath = backgroundImagePath;
            return this;
        }
        
        /// <summary>
        /// Configura los márgenes externos del popup
        /// </summary>
        /// <param name="margin">Margen para todos los lados</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetMargin(int margin)
        {
            _margin = margin;
            return this;
        }
        
        /// <summary>
        /// Configura los márgenes externos del popup individualmente
        /// </summary>
        /// <param name="top">Margen superior</param>
        /// <param name="right">Margen derecho</param>
        /// <param name="bottom">Margen inferior</param>
        /// <param name="left">Margen izquierdo</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetMargin(int top, int right, int bottom, int left)
        {
            _marginTop = top;
            _marginRight = right;
            _marginBottom = bottom;
            _marginLeft = left;
            return this;
        }
        
        /// <summary>
        /// Configura el padding interno del popup
        /// </summary>
        /// <param name="padding">Padding para todos los lados</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetPadding(int padding)
        {
            _padding = padding;
            return this;
        }
        
        /// <summary>
        /// Configura el padding interno del popup individualmente
        /// </summary>
        /// <param name="top">Padding superior</param>
        /// <param name="right">Padding derecho</param>
        /// <param name="bottom">Padding inferior</param>
        /// <param name="left">Padding izquierdo</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetPadding(int top, int right, int bottom, int left)
        {
            _paddingTop = top;
            _paddingRight = right;
            _paddingBottom = bottom;
            _paddingLeft = left;
            return this;
        }
        
        /// <summary>
        /// Configura el tamaño del popup
        /// </summary>
        /// <param name="size">Tamaño del popup</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetSize(Vector2 size)
        {
            _size = size;
            return this;
        }
        
        /// <summary>
        /// Configura la posición del popup
        /// </summary>
        /// <param name="position">Posición del popup</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetPosition(Vector2 position)
        {
            _position = position;
            return this;
        }
        
        /// <summary>
        /// Configura el título del popup
        /// </summary>
        /// <param name="title">Texto del título</param>
        /// <param name="fontSize">Tamaño de fuente (opcional)</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetTitle(string title, float fontSize = 48.0f)
        {
            _title = title;
            _titleFontSize = fontSize;
            return this;
        }
        
        /// <summary>
        /// Configura el mensaje del popup
        /// </summary>
        /// <param name="message">Texto del mensaje</param>
        /// <param name="fontSize">Tamaño de fuente (opcional)</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetMessage(string message, float fontSize = 28.0f)
        {
            _message = message;
            _messageFontSize = fontSize;
            return this;
        }
        
        /// <summary>
        /// Configura el texto del botón de confirmar
        /// </summary>
        /// <param name="text">Texto del botón</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetConfirmButtonText(string text)
        {
            _confirmButtonText = text;
            return this;
        }
        
        /// <summary>
        /// Configura el texto del botón de cancelar
        /// </summary>
        /// <param name="text">Texto del botón</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager SetCancelButtonText(string text)
        {
            _cancelButtonText = text;
            return this;
        }
        
        /// <summary>
        /// Muestra el popup en el nodo padre especificado
        /// </summary>
        /// <param name="parent">Nodo padre donde se mostrará el popup</param>
        /// <returns>Esta instancia para encadenar métodos</returns>
        public PopupManager Show(Node parent)
        {
            _parent = parent;
            
            // Crear CanvasLayer para mostrar por encima de todo
            _canvasLayer = new CanvasLayer();
            _canvasLayer.Name = "PopupCanvasLayer";
            _canvasLayer.Layer = 100; // Layer muy alto
            parent.AddChild(_canvasLayer);
            
            // Crear GenericCard con configuración
            _popupCard = new GenericCard(_backgroundImagePath ?? "res://src/Image/Gemini_Generated_Image_11b5vz11b5vz11b5.png");
            _popupCard.Name = "PopupCard";
            
            // Configurar márgenes
            if (_margin.HasValue)
            {
                _popupCard.SetMargin(_margin.Value);
            }
            else
            {
                _popupCard.SetMargin(_marginTop, _marginRight, _marginBottom, _marginLeft);
            }
            
            // Configurar padding
            if (_padding.HasValue)
            {
                _popupCard.SetPadding(_padding.Value);
            }
            else
            {
                _popupCard.SetPadding(_paddingTop, _paddingRight, _paddingBottom, _paddingLeft);
            }
            
            // Configurar tamaño y posición
            if (_size.HasValue)
            {
                _popupCard.CustomMinimumSize = _size.Value;
                _popupCard.Size = _size.Value;
            }
            
            if (_position.HasValue)
            {
                _popupCard.Position = _position.Value;
            }
            // El centrado se hará cuando el card esté listo
            
            // Agregar al CanvasLayer
            _canvasLayer.AddChild(_popupCard);
            
            // Configurar contenido y centrar cuando esté listo
            _popupCard.ExecuteWhenReady(() => {
                SetupContent();
                // Centrar después de configurar contenido si no hay posición específica
                if (!_position.HasValue)
                {
                    CenterPopup();
                }
            });
            
            // Ejecutar callback OnShow
            OnShow?.Invoke();
            
            return this;
        }
        
        /// <summary>
        /// Configura el contenido del popup
        /// </summary>
        private void SetupContent()
        {
            if (_popupCard == null) return;
            
            var contentContainer = _popupCard.GetContentContainer();
            if (contentContainer == null) return;
            
            // Agregar título si existe
            if (!string.IsNullOrEmpty(_title))
            {
                _popupCard.AddLabel(_title, _titleFontSize, HorizontalAlignment.Center);
            }
            
            // Agregar mensaje si existe
            if (!string.IsNullOrEmpty(_message))
            {
                _popupCard.AddLabel(_message, _messageFontSize, HorizontalAlignment.Center);
            }
            
            // Crear contenedor de botones manualmente para tener control
            var buttonsContainer = new HBoxContainer();
            buttonsContainer.Name = "ButtonsContainer";
            buttonsContainer.AddThemeConstantOverride("separation", 20);
            buttonsContainer.Alignment = BoxContainer.AlignmentMode.Center;
            contentContainer.AddChild(buttonsContainer);
            
            // Botón de cancelar (si existe callback o texto)
            if (OnCancel != null || !string.IsNullOrEmpty(_cancelButtonText))
            {
                var cancelButton = new Button();
                cancelButton.Name = "CancelButton";
                cancelButton.Text = _cancelButtonText ?? "Cancelar";
                cancelButton.AddThemeFontSizeOverride("font_size", 32);
                cancelButton.AddThemeColorOverride("font_color", Colors.White);
                cancelButton.AddThemeConstantOverride("outline_size", 4);
                cancelButton.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
                
                // Configurar sonido de selección automático
                Package.Audio.AudioManager.SetupButtonSelectSound(cancelButton);
                
                cancelButton.Pressed += () => {
                    OnCancel?.Invoke();
                    Hide();
                };
                
                buttonsContainer.AddChild(cancelButton);
            }
            
            // Botón de confirmar (si existe callback o texto)
            if (OnConfirm != null || !string.IsNullOrEmpty(_confirmButtonText))
            {
                var confirmButton = new Button();
                confirmButton.Name = "ConfirmButton";
                confirmButton.Text = _confirmButtonText ?? "Confirmar";
                confirmButton.AddThemeFontSizeOverride("font_size", 32);
                confirmButton.AddThemeColorOverride("font_color", Colors.White);
                confirmButton.AddThemeConstantOverride("outline_size", 4);
                confirmButton.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
                
                // Configurar sonido de selección automático
                Package.Audio.AudioManager.SetupButtonSelectSound(confirmButton);
                
                confirmButton.Pressed += () => {
                    OnConfirm?.Invoke();
                    Hide();
                };
                
                buttonsContainer.AddChild(confirmButton);
            }
            
            // Botón de cerrar (si no hay otros botones)
            if (OnCancel == null && OnConfirm == null && string.IsNullOrEmpty(_cancelButtonText) && string.IsNullOrEmpty(_confirmButtonText))
            {
                var closeButton = new Button();
                closeButton.Name = "CloseButton";
                closeButton.Text = "Cerrar";
                closeButton.AddThemeFontSizeOverride("font_size", 32);
                closeButton.AddThemeColorOverride("font_color", Colors.White);
                closeButton.AddThemeConstantOverride("outline_size", 4);
                closeButton.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.8f));
                
                // Configurar sonido de selección automático
                Package.Audio.AudioManager.SetupButtonSelectSound(closeButton);
                
                closeButton.Pressed += () => {
                    OnClose?.Invoke();
                    Hide();
                };
                
                buttonsContainer.AddChild(closeButton);
            }
        }
        
        /// <summary>
        /// Centra el popup en la pantalla usando viewport real dinámicamente
        /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
        /// </summary>
        private void CenterPopup()
        {
            if (_popupCard == null || !_popupCard.IsInsideTree()) return;
            
            // ⚠️ CRÍTICO: Obtener viewport real dinámicamente
            var viewport = _popupCard.GetViewport();
            var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(1920, 1080);
            // Obtener tamaño real del card (puede ser CustomMinimumSize o Size)
            var cardSize = _size.HasValue ? _size.Value : _popupCard.Size;
            if (cardSize.X <= 0 || cardSize.Y <= 0)
            {
                cardSize = _popupCard.CustomMinimumSize;
            }
            
            // Calcular centro como porcentaje del viewport
            var centerX = (viewportSize.X - cardSize.X) * 0.5f;
            var centerY = (viewportSize.Y - cardSize.Y) * 0.5f;
            
            _popupCard.Position = new Vector2(centerX, centerY);
        }
        
        /// <summary>
        /// Oculta el popup
        /// </summary>
        public void Hide()
        {
            if (_popupCard != null && _popupCard.IsInsideTree())
            {
                OnHide?.Invoke();
                _popupCard.Close();
                _popupCard = null;
            }
            
            if (_canvasLayer != null && _canvasLayer.IsInsideTree())
            {
                _canvasLayer.QueueFree();
                _canvasLayer = null;
            }
        }
        
        // Campos privados para configuración
        private string _backgroundImagePath;
        private int? _margin;
        private int _marginTop = 50;
        private int _marginRight = 50;
        private int _marginBottom = 50;
        private int _marginLeft = 50;
        private int? _padding;
        private int _paddingTop = 50;
        private int _paddingRight = 50;
        private int _paddingBottom = 50;
        private int _paddingLeft = 50;
        private Vector2? _size = new Vector2(600, 400);
        private Vector2? _position;
        private string _title;
        private float _titleFontSize = 48.0f;
        private string _message;
        private float _messageFontSize = 28.0f;
        private string _confirmButtonText = "Confirmar";
        private string _cancelButtonText = "Cancelar";
    }
}

