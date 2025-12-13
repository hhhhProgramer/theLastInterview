using Godot;
using SlimeKingdomChronicles.Core.UI;
using System;

namespace TheLastInterview.Interview.Minigames
{
    /// <summary>
    /// Minijuego: Escribir un Reporte con Teclas Bloqueadas
    /// </summary>
    public partial class TypeReportMinigame : BaseMinigame
    {
        private Label _instructionLabel;
        private LineEdit _inputField;
        private Label _errorLabel;
        private ProgressBar _progressBar;
        private Button _continueButton;
        private System.Random _random = new System.Random();
        
        private string _targetText = "INFORME COMPLETO";
        private int _charactersTyped = 0;
        private int _nextBlockAt = 0;
        private bool _isBlocked = false;
        private string _blockedChar = "";
        
        public TypeReportMinigame(Node parent) : base(parent)
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
            float panelHeightPercent = viewportSize.Y < 1000 ? 0.85f : 0.55f;
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
            
            // Título
            var titleLabel = new Label();
            titleLabel.Text = "ESCRIBIR REPORTE";
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float titleSize = FontManager.GetScaledSize(TextType.Subtitle) * 1.3f; // 30% más grande
            titleLabel.AddThemeFontSizeOverride("font_size", (int)titleSize);
            titleLabel.AddThemeColorOverride("font_color", new Color(0.2f, 0.8f, 1.0f, 1.0f));
            mainContainer.AddChild(titleLabel);
            
            // Instrucción
            _instructionLabel = new Label();
            _instructionLabel.Text = $"Escribe: {_targetText}";
            _instructionLabel.HorizontalAlignment = HorizontalAlignment.Center;
            float bodySize = FontManager.GetScaledSize(TextType.Body) * 1.2f; // 20% más grande
            _instructionLabel.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _instructionLabel.AddThemeColorOverride("font_color", new Color(0.9f, 0.9f, 0.9f, 1.0f));
            mainContainer.AddChild(_instructionLabel);
            
            // Mensaje de error
            _errorLabel = new Label();
            _errorLabel.Text = "";
            _errorLabel.HorizontalAlignment = HorizontalAlignment.Center;
            _errorLabel.AddThemeFontSizeOverride("font_size", (int)bodySize); // Usar tamaño completo
            _errorLabel.AddThemeColorOverride("font_color", new Color(1.0f, 0.3f, 0.3f, 1.0f));
            mainContainer.AddChild(_errorLabel);
            
            // Campo de entrada (tamaño responsive)
            _inputField = new LineEdit();
            _inputField.PlaceholderText = "Escribe aquí...";
            float inputHeight = viewportSize.Y * 0.035f;
            _inputField.CustomMinimumSize = new Vector2(0, inputHeight);
            _inputField.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _inputField.TextChanged += OnTextChanged;
            mainContainer.AddChild(_inputField);
            
            // Barra de progreso (tamaño responsive)
            _progressBar = new ProgressBar();
            float progressBarHeight = viewportSize.Y * 0.025f;
            _progressBar.CustomMinimumSize = new Vector2(0, progressBarHeight);
            _progressBar.MinValue = 0;
            _progressBar.MaxValue = _targetText.Length;
            _progressBar.Value = 0;
            mainContainer.AddChild(_progressBar);
            
            // Espaciador
            var spacer = new Control();
            float spacerHeight = viewportSize.Y * 0.015f;
            spacer.CustomMinimumSize = new Vector2(0, spacerHeight);
            spacer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            mainContainer.AddChild(spacer);
            
            // Botón continuar (tamaño responsive)
            _continueButton = new Button();
            _continueButton.Text = "Continuar";
            Vector2 continueButtonSize = GetResponsiveSize(0.30f, 0.035f);
            _continueButton.CustomMinimumSize = continueButtonSize;
            _continueButton.Visible = false;
            _continueButton.AddThemeFontSizeOverride("font_size", (int)bodySize);
            _continueButton.Pressed += OnContinuePressed;
            mainContainer.AddChild(_continueButton);
            
            // Calcular próximo bloqueo
            _nextBlockAt = _random.Next(3, 6);
            
            // Enfocar el campo de entrada
            _inputField.GrabFocus();
        }
        
        private void OnTextChanged(string newText)
        {
            // Si está bloqueado, no permitir escribir más
            if (_isBlocked && newText.Length > _charactersTyped)
            {
                _inputField.Text = _inputField.Text.Substring(0, _charactersTyped);
                return;
            }
            
            // Si se desbloqueó, continuar
            if (_isBlocked && newText.Length == _charactersTyped)
            {
                _isBlocked = false;
                _errorLabel.Text = "";
            }
            
            // Actualizar progreso
            int correctChars = 0;
            for (int i = 0; i < Mathf.Min(newText.Length, _targetText.Length); i++)
            {
                if (newText[i] == _targetText[i])
                {
                    correctChars++;
                }
            }
            
            _charactersTyped = correctChars;
            _progressBar.Value = correctChars;
            
            // Verificar si se completó
            if (newText.ToUpper() == _targetText)
            {
                _inputField.Editable = false;
                _errorLabel.Text = "¡Reporte completado!";
                _errorLabel.AddThemeColorOverride("font_color", new Color(0.3f, 1.0f, 0.3f, 1.0f));
                GetTree().CreateTimer(1.5f).Timeout += () => {
                    _continueButton.Visible = true;
                    StartButtonBlink(_continueButton); // Hacer parpadear el botón continuar
                };
                return;
            }
            
            // Bloquear cada X caracteres
            if (correctChars >= _nextBlockAt && !_isBlocked)
            {
                _isBlocked = true;
                _blockedChar = _targetText[correctChars].ToString();
                _errorLabel.Text = $"Tecla '{_blockedChar}' no detectada. Intente fuerte.";
                
                // Calcular próximo bloqueo
                _nextBlockAt = correctChars + _random.Next(3, 6);
                
                // Desbloquear después de 1 segundo
                GetTree().CreateTimer(1.0f).Timeout += () => {
                    if (_isBlocked)
                    {
                        _isBlocked = false;
                        _errorLabel.Text = "";
                    }
                };
            }
        }
        
        private void OnContinuePressed()
        {
            FinishMinigame();
        }
    }
}

