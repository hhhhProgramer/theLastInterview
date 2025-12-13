using Godot;
using SlimeKingdomChronicles.Core.UI;
using System.Collections.Generic;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Eliminar Correos Spam - scroll automático
    /// </summary>
    public partial class DeleteSpamMinigame : BaseMinigame
    {
        private ScrollContainer _scrollContainer;
        private VBoxContainer _emailsContainer;
        private Label _instructionLabel;
        private Label _scoreLabel;
        private Button _continueButton;
        private Timer _scrollTimer;
        private System.Random _random = new System.Random();
        
        private List<Button> _emailButtons;
        private int _deletedCount = 0;
        private int _targetCount = 5;
        private bool _gameActive = false;
        
        private string[] _urgentEmails = {
            "URGENTE: Reunión",
            "URGENTE: Pago pendiente",
            "URGENTE: Revisar documento",
            "URGENTE: Respuesta requerida",
            "URGENTE: Fecha límite",
            "URGENTE: Confirmación necesaria"
        };
        
        private string[] _spamEmails = {
            "Publicita tu negocio",
            "Oferta especial",
            "Gana dinero fácil",
            "Promoción exclusiva",
            "Descuento increíble",
            "Oferta limitada"
        };
        
        public DeleteSpamMinigame(Node parent) : base(parent)
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
            
            // Contenedor principal (márgenes responsive)
            var mainContainer = new VBoxContainer();
            mainContainer.Name = "MainContainer";
            mainContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            float margin = GetResponsiveMargin(0.015f);
            mainContainer.OffsetLeft = margin;
            mainContainer.OffsetRight = -margin;
            mainContainer.OffsetTop = margin * 0.7f;
            mainContainer.OffsetBottom = -margin * 0.7f;
            float separation = GetResponsiveMargin(0.01f);
            mainContainer.AddThemeConstantOverride("separation", (int)separation);
            panel.AddChild(mainContainer);
            
            // Título (más grande)
            var titleLabel = new Label();
            titleLabel.Text = "ELIMINAR CORREOS SPAM";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.3f; // 30% más grande
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.5f, 0.2f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Contador (más grande)
            _scoreLabel = new Label();
            _scoreLabel.Text = $"Eliminados: {_deletedCount} / {_targetCount}";
            _scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float bodySize = FontManager.GetScaledSize(TextType.Body) * 1.2f; // 20% más grande
            _scoreLabel.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _scoreLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_scoreLabel);
            
            // Instrucción (más grande)
            _instructionLabel = new Label();
            _instructionLabel.Text = "Haz clic en los correos URGENTE para eliminarlos";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)(bodySize * 0.95f)); // Usar bodySize ya aumentado
            _instructionLabel.AddThemeColorOverride("font_color", new Color(0.8f, 0.8f, 1.0f, 1.0f));
            mainContainer.AddChild(_instructionLabel);
            
            // Scroll container con emails
            _scrollContainer = new ScrollContainer();
            _scrollContainer.CustomMinimumSize = new Vector2(0, 300);
            mainContainer.AddChild(_scrollContainer);
            
            _emailsContainer = new VBoxContainer();
            _emailsContainer.AddThemeConstantOverride("separation", 5);
            _scrollContainer.AddChild(_emailsContainer);
            
            _emailButtons = new List<Button>();
            
            // Crear emails iniciales
            for (int i = 0; i < 20; i++)
            {
                CreateEmail(i < 10);
            }
            
            // Espaciador
            var spacer = new Control();
            spacer.CustomMinimumSize = new Vector2(0, 10);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            mainContainer.AddChild(spacer);
            
            // Botón continuar
            _continueButton = new Button();
            _continueButton.Text = "Continuar";
            _continueButton.CustomMinimumSize = new Vector2(200, 50);
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
            
            // Iniciar scroll automático
            _gameActive = true;
            StartAutoScroll();
        }
        
        private void CreateEmail(bool isUrgent)
        {
            var emailButton = new Button();
            string emailText = isUrgent 
                ? _urgentEmails[_random.Next(_urgentEmails.Length)]
                : _spamEmails[_random.Next(_spamEmails.Length)];
            
            emailButton.Text = $"📧 {emailText}";
            // Obtener viewportSize para calcular altura responsive
            var viewportSize = GetViewportSize();
            float emailButtonHeight = viewportSize.Y * 0.028f;
            emailButton.CustomMinimumSize = new Vector2(0, emailButtonHeight);
            // Aumentar tamaño de fuente para mejor legibilidad (usar Body completo en lugar de 0.9f)
            emailButton.AddThemeFontSizeOverride("font_size", (int)(FontManager.GetScaledSize(TextType.Body) * 1.2f));
            
            // Estilo diferente para urgentes
            if (isUrgent)
            {
                var style = new StyleBoxFlat();
                style.BgColor = new Color(0.3f, 0.1f, 0.1f, 1.0f);
                style.BorderColor = new Color(1.0f, 0.3f, 0.3f, 1.0f);
                style.BorderWidthLeft = 2;
                style.BorderWidthTop = 2;
                style.BorderWidthRight = 2;
                style.BorderWidthBottom = 2;
                emailButton.AddThemeStyleboxOverride("normal", style);
                emailButton.AddThemeColorOverride("font_color", new Color(1.0f, 0.8f, 0.8f, 1.0f));
            }
            
            emailButton.Pressed += () => OnEmailClicked(emailButton, isUrgent);
            _emailButtons.Add(emailButton);
            _emailsContainer.AddChild(emailButton);
        }
        
        private void StartAutoScroll()
        {
            _scrollTimer = new Timer();
            _scrollTimer.WaitTime = 0.016f; // ~60 FPS
            _scrollTimer.Timeout += OnScrollTimer;
            _scrollTimer.Autostart = true;
            AddChild(_scrollTimer);
        }
        
        private void OnScrollTimer()
        {
            if (!_gameActive) return;
            
            // Mover scroll hacia arriba (1 pixel por frame)
            _scrollContainer.ScrollVertical = Mathf.Max(0, _scrollContainer.ScrollVertical - 1);
            
            // Agregar nuevos emails ocasionalmente
            if (_random.Next(0, 30) == 0)
            {
                CreateEmail(_random.Next(0, 2) == 0);
            }
        }
        
        private void OnEmailClicked(Button emailButton, bool isUrgent)
        {
            if (!_gameActive || !emailButton.Visible) return;
            
            if (isUrgent)
            {
                _deletedCount++;
                _scoreLabel.Text = $"Eliminados: {_deletedCount} / {_targetCount}";
                emailButton.Visible = false;
                
                if (_deletedCount >= _targetCount)
                {
                    _gameActive = false;
                    _scrollTimer.Stop();
                    _instructionLabel.Text = "¡Completado! Has eliminado 5 correos urgentes.";
                    _instructionLabel.AddThemeColorOverride("font_color", new Color(0.3f, 1.0f, 0.3f, 1.0f));
                    GetTree().CreateTimer(1.5f).Timeout += () => {
                        _continueButton.Visible = true;
                        StartButtonBlink(_continueButton); // Hacer parpadear el botón continuar
                    };
                }
            }
            else
            {
                _instructionLabel.Text = "Ese no es un correo URGENTE. Elimina solo los urgentes.";
                GetTree().CreateTimer(2.0f).Timeout += () => {
                    _instructionLabel.Text = "Haz clic en los correos URGENTE para eliminarlos";
                };
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

