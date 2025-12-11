using Core.Services;
using Godot;
using Package.Core.Enums;
using System.Collections.Generic;

namespace Package.UI
{
	/// <summary>
	/// Componente de diálogo que cubre la pantalla inferior con fondo y texto
	/// Se usa para mostrar diálogos del slime y otros personajes
	/// </summary>
	public partial class DialogBox : Control
	{
		private Panel _dialogPanel;
		private Panel _characterNamePanel;
		private Label _characterNameLabel;
		private RichTextLabel _dialogText;
		private Label _dialogTextShadow; // Label de sombra detrás del texto para efecto visual
		private Control _continueIndicator; // Indicador parpadeante en esquina inferior izquierda
		private Timer _textTimer;
		private Timer _blinkTimer; // Timer para el efecto parpadeante
		private string _fullText;
		private int _currentCharIndex;
		private bool _isTyping;

		/// <summary>
		/// Overlay de pantalla completa para las opciones de respuesta
		/// </summary>
		private Control _optionsOverlay;

		/// <summary>
		/// Panel oscuro de fondo para las opciones
		/// </summary>
		private Panel _optionsBackgroundPanel;

		/// <summary>
		/// Contenedor para las opciones de respuesta (centrado)
		/// </summary>
		private VBoxContainer _optionsContainer;

		/// <summary>
		/// Lista de botones de opciones actuales
		/// </summary>
		private List<Button> _optionButtons;
		
		/// <summary>
		/// Texto duplicado del diálogo para animar en el panel de opciones
		/// </summary>
		private RichTextLabel _duplicatedDialogText;
		
		/// <summary>
		/// Panel que contiene el texto duplicado
		/// </summary>
		private Panel _duplicatedTextPanel;
		
		/// <summary>
		/// Callback a ejecutar cuando termina la animación del texto duplicado
		/// </summary>
		private System.Action _onTextAnimationEnd;

		/// <summary>
		/// Evento que se dispara cuando el diálogo termina completamente
		/// </summary>
		[Signal]
		public delegate void DialogFinishedEventHandler();

		/// <summary>
		/// Evento que se dispara cuando se presiona el botón continuar
		/// </summary>
		[Signal]
		public delegate void ContinuePressedEventHandler();

		/// <summary>
		/// Evento que se dispara cuando se necesita cambiar la emoción del slime
		/// </summary>
		[Signal]
		public delegate void SlimeChangeEmotionEventHandler(int emotion);
		
		/// <summary>
		/// Evento que se dispara cuando la animación del texto duplicado termina y el temporizador puede iniciarse
		/// </summary>
		[Signal]
		public delegate void TimedDecisionReadyToStartEventHandler(float timeLimit);
		
		/// <summary>
		/// Evento que se dispara cuando termina la animación del texto duplicado
		/// Se puede usar para reproducir sonidos o ejecutar otras acciones
		/// </summary>
		[Signal]
		public delegate void DuplicatedTextAnimationFinishedEventHandler();

		public override void _Ready()
		{
			GD.Print("DialogBox._Ready() iniciando...");

			// Configurar el DialogBox principal para cubrir toda la pantalla
			SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

			CreateDialogPanel();
			CreateCharacterNamePanel();
			CreateDialogText();
			CreateOptionsContainer();
			CreateContinueIndicator();
			CreateTextTimer();
			CreateBlinkTimer();
			SetupConnections();

			GD.Print("DialogBox._Ready() completado. Visible: " + Visible);
			GD.Print("DialogBox size: " + Size);
		}

		/// <summary>
		/// Crea el panel principal del diálogo que cubre la pantalla inferior
		/// CRÍTICO: La altura del panel debe ser suficiente para contener 3 líneas de texto con márgenes
		/// </summary>
		private void CreateDialogPanel()
		{
			_dialogPanel = new Panel();
			_dialogPanel.Name = "DialogPanel";

			// Obtener el tamaño de la pantalla
			var viewportSize = GetViewportRect().Size;

			// Detectar si es teléfono (pantalla pequeña)
			var screenSize = DisplayServer.ScreenGetSize();
			bool isPhone = screenSize.X < 1000 || screenSize.Y < 1000;

			// CRÍTICO: El cuadro de diálogo debe ocupar máximo 40% de la altura del viewport (o más en teléfonos)
			// Calcular altura como porcentaje del viewport (responsivo)
			// En teléfonos, usar 50% para hacer el panel más grande
			float maxHeightPercent = isPhone ? 0.50f : 0.40f; // 50% en teléfonos, 40% en pantallas grandes
			float maxDialogHeight = viewportSize.Y * maxHeightPercent;
			
			// Calcular la altura mínima necesaria para el panel basada en el contenido
			// Hacer los tamaños responsivos al viewport
			// Tamaño de fuente base: 2.5% de la altura del viewport (responsivo)
			// En teléfonos, aumentar el porcentaje base para letras más grandes
			float baseFontSizePercent = isPhone ? 0.035f : 0.025f; // 3.5% en teléfonos, 2.5% en pantallas grandes
			float baseFontSize = viewportSize.Y * baseFontSizePercent;
			float fontSize = baseFontSize * 1.2f; // 20% más grande para diálogos
			int lineSeparation = (int)(baseFontSize * 0.05f); // 5% del tamaño de fuente
			const int MAX_LINES = 3;
			float extraMargin = baseFontSize * 0.4f; // 40% del tamaño de fuente
			float textHeight = (fontSize * MAX_LINES) + (lineSeparation * (MAX_LINES - 1)) + extraMargin;
			// En teléfonos, aumentar los márgenes para más espacio
			float marginMultiplier = isPhone ? 1.3f : 1.0f;
			float topMargin = baseFontSize * 1.4f * marginMultiplier; // Responsivo al viewport
			float bottomMargin = baseFontSize * 2.4f * marginMultiplier; // Responsivo al viewport
			float calculatedHeight = textHeight + topMargin + bottomMargin; // Altura calculada por contenido
			
			// Usar el menor entre la altura calculada y el máximo del 40%
			float dialogHeight = Mathf.Min(calculatedHeight, maxDialogHeight);

			// IMPORTANTE: Asegurar una altura mínima razonable (pero no exceder el máximo)
			// En teléfonos, usar un mínimo más grande
			float minHeight = isPhone ? 280.0f : 200.0f; // Mínimo 280px en teléfonos, 200px en pantallas grandes
			if (dialogHeight < minHeight && minHeight <= maxDialogHeight)
			{
				dialogHeight = minHeight;
			}

			// Configurar el panel para cubrir toda la pantalla inferior
			_dialogPanel.SetAnchorsAndOffsetsPreset(LayoutPreset.BottomWide);
			_dialogPanel.OffsetTop = -dialogHeight; // Altura calculada desde la parte inferior
			_dialogPanel.OffsetBottom = 0; // Desde la parte inferior

			// Cargar y aplicar la imagen de fondo del panel general
			Texture2D panelTexture = GD.Load<Texture2D>("res://src/Image/Gemini_Generated_Image_11b5vz11b5vz11b5.png");
			if (panelTexture != null)
			{
				// Usar StyleBoxTexture para aplicar la imagen de fondo
				StyleBoxTexture styleBox = new StyleBoxTexture();
				styleBox.Texture = panelTexture;
				styleBox.TextureMarginLeft = 0.0f;
				styleBox.TextureMarginRight = 0.0f;
				styleBox.TextureMarginTop = 0.0f;
				styleBox.TextureMarginBottom = 0.0f;
				_dialogPanel.Modulate = new Color(1, 1, 1, 0.9f);

				_dialogPanel.AddThemeStyleboxOverride("panel", styleBox);
			}
			else
			{
				// Fallback a StyleBoxFlat si no se encuentra la imagen
				GD.PrintErr("No se pudo cargar panel-general.png, usando fondo por defecto");
				StyleBoxFlat styleBox = new StyleBoxFlat();
				styleBox.BgColor = new Color(0, 0, 0, 0.8f);
				styleBox.CornerRadiusTopLeft = 20;
				styleBox.CornerRadiusTopRight = 20;
				_dialogPanel.AddThemeStyleboxOverride("panel", styleBox);
			}

			AddChild(_dialogPanel);

			GD.Print("DialogPanel creado - Viewport size: " + viewportSize + ", Dialog height: " + dialogHeight);
		}

		/// <summary>
		/// Crea el panel elegante en la parte superior para mostrar el nombre del personaje
		/// Panel adaptativo con diseño moderno siguiendo mejores prácticas de UX/UI para novelas visuales
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// </summary>
		private void CreateCharacterNamePanel()
		{
			_characterNamePanel = new Panel();
			_characterNamePanel.Name = "CharacterNamePanel";

			// Posicionar en la parte superior izquierda del panel de diálogo
			// Usar anclas para posicionar desde la esquina superior izquierda
			_characterNamePanel.AnchorLeft = 0.0f;
			_characterNamePanel.AnchorTop = 0.0f;
			_characterNamePanel.AnchorRight = 0.0f;
			_characterNamePanel.AnchorBottom = 0.0f;
			
			// Posición inicial (se ajustará dinámicamente según el texto)
			// AJUSTADO: Panel más arriba y se adaptará al ancho del texto
			_characterNamePanel.OffsetLeft = 50; // 120px desde la izquierda (mismo margen que el texto)
			_characterNamePanel.OffsetTop = -15; // 10px desde arriba (más cerca del borde superior)
			_characterNamePanel.OffsetRight = 120; // Se ajustará dinámicamente según el texto
			_characterNamePanel.OffsetBottom = FontManager.GetScaledSize(FontManager.TextType.Large) + 10; // Altura del panel: 60px (más alto para mejor presencia)

			// IMPORTANTE: El panel NO debe expandirse horizontalmente
			// Solo ocupará el espacio necesario del texto
			_characterNamePanel.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;

			// MEJORADO: Aplicar estilo moderno y elegante al panel
			// Diseño inspirado en mejores prácticas de novelas visuales modernas
			var namePanelStyle = new StyleBoxFlat();
			
			// Fondo oscuro elegante con ligera transparencia para profundidad
			namePanelStyle.BgColor = new Color(0.08f, 0.08f, 0.12f, 0.98f); // Fondo más oscuro y elegante
			
			// Bordes redondeados más pronunciados para diseño moderno
			namePanelStyle.CornerRadiusTopLeft = 12;
			namePanelStyle.CornerRadiusTopRight = 12;
			namePanelStyle.CornerRadiusBottomLeft = 12;
			namePanelStyle.CornerRadiusBottomRight = 12;
			
			// Borde elegante con color más vibrante y visible
			namePanelStyle.BorderColor = new Color(0.6f, 0.65f, 0.75f, 1.0f); // Borde más brillante y elegante
			namePanelStyle.BorderWidthLeft = 3;
			namePanelStyle.BorderWidthTop = 3;
			namePanelStyle.BorderWidthRight = 3;
			namePanelStyle.BorderWidthBottom = 3;
			
			_characterNamePanel.AddThemeStyleboxOverride("panel", namePanelStyle);

			// Crear el label para el nombre
			_characterNameLabel = new Label();
			_characterNameLabel.Name = "CharacterNameLabel";
			
			// El label debe ocupar todo el panel pero con márgenes internos más generosos
			_characterNameLabel.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			_characterNameLabel.OffsetLeft = 24; // Margen interno izquierdo más generoso
			_characterNameLabel.OffsetTop = 0; // Margen interno superior
			_characterNameLabel.OffsetRight = -24; // Margen interno derecho más generoso
			_characterNameLabel.OffsetBottom = -12; // Margen interno inferior

			// MEJORADO: Aplicar estilo tipográfico distintivo y elegante
			// Tamaño más grande para mejor presencia y legibilidad
			float baseSize = FontManager.GetScaledSize(FontManager.TextType.Subtitle);
			float nameSize = baseSize * 1.3f; // 30% más grande para destacar
			_characterNameLabel.AddThemeFontSizeOverride("font_size", (int)nameSize);
			
			// Color más vibrante y distintivo para el nombre
			// Blanco brillante con ligera calidez para elegancia
			_characterNameLabel.AddThemeColorOverride("font_color", new Color(1.0f, 1.0f, 0.98f, 1.0f));
			
			// MEJORADO: Sombra más pronunciada para mejor legibilidad y profundidad
			_characterNameLabel.AddThemeColorOverride("font_shadow_color", new Color(0.0f, 0.0f, 0.0f, 0.85f));
			_characterNameLabel.AddThemeConstantOverride("shadow_offset_x", 3);
			_characterNameLabel.AddThemeConstantOverride("shadow_offset_y", 3);
			
			// MEJORADO: Outline más visible para mejor contraste y elegancia
			_characterNameLabel.AddThemeColorOverride("font_outline_color", new Color(0.2f, 0.2f, 0.25f, 0.9f));
			_characterNameLabel.AddThemeConstantOverride("outline_size", 2);
			
			_characterNameLabel.HorizontalAlignment = HorizontalAlignment.Left;
			_characterNameLabel.VerticalAlignment = VerticalAlignment.Center;
			
			// IMPORTANTE: El label NO debe expandirse, solo ocupar el espacio del texto
			_characterNameLabel.SizeFlagsHorizontal = Control.SizeFlags.ShrinkBegin;

			// Inicialmente oculto (se mostrará con animación cuando haya nombre)
			_characterNamePanel.Visible = false;
			_characterNamePanel.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f); // Iniciar invisible para animación
			_characterNameLabel.Text = "";

			_characterNamePanel.AddChild(_characterNameLabel);
			_dialogPanel.AddChild(_characterNamePanel);
		}

		/// <summary>
		/// Crea el texto del diálogo con efecto de escritura y mejor espaciado
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// </summary>
		private void CreateDialogText()
		{
			// IMPORTANTE: Usar LayoutMode = 0 (Position mode) para control directo del tamaño
			// Esto es más confiable que usar anclas con offsets para RichTextLabel con BBCode
			// Similar al enfoque usado en PanelWithBackground.cs
			_dialogText = new RichTextLabel();
			_dialogText.Name = "DialogText";
			_dialogText.LayoutMode = 0; // Position mode para control directo

			// IMPORTANTE: Establecer posición y tamaño directamente basado en el panel
			// Esto asegura que el RichTextLabel respete los límites del contenedor
			// Los márgenes se calculan: 120px desde la izquierda, 30px desde la derecha
			// AJUSTADO: 80px desde arriba (10px margen superior + 60px altura panel nombre + 10px espacio reducido)
			_dialogText.Position = new Vector2(120, 80); // Padding desde la esquina superior izquierda (ajustado para el panel de nombre con espacio reducido)

			// IMPORTANTE: Establecer un tamaño inicial razonable basado en el viewport
			// Esto se actualizará dinámicamente en ShowDialog() basado en el tamaño de fuente y espaciado
			var viewportSize = GetViewportRect().Size;
			var initialWidth = viewportSize.X - 120 - 30; // Ancho del viewport menos márgenes

			// CRÍTICO: Calcular altura inicial para 3 líneas basándose en el tamaño de fuente
			float fontSize = FontManager.GetScaledSize(FontManager.TextType.Large) * 1.5f; // 34 * 1.5 = 51
			int lineSeparation = 4; // Aumentado para mejor espaciado
			const int MAX_LINES = 3;
			const float EXTRA_MARGIN = 55.0f; // Aumentado para asegurar que quepan 3 líneas
			var initialHeight = (fontSize * MAX_LINES) + (lineSeparation * (MAX_LINES - 1)) + EXTRA_MARGIN;

			_dialogText.Size = new Vector2(initialWidth > 0 ? initialWidth : 1000, initialHeight > 0 ? initialHeight : 200);

			_dialogText.BbcodeEnabled = true;
			_dialogText.FitContent = false; // No expandir para ajustar contenido
			_dialogText.ScrollActive = false; // Desactivar scroll
			_dialogText.ClipContents = false; 
			_dialogText.AutowrapMode = TextServer.AutowrapMode.WordSmart; // IMPORTANTE: Ajustar texto automáticamente

			// IMPORTANTE: NO usar SizeFlags con Position mode, ya que el tamaño se controla directamente
			// _dialogText.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill; // NO usar con Position mode
			// _dialogText.SizeFlagsVertical = Control.SizeFlags.ExpandFill; // NO usar con Position mode

			// IMPORTANTE: NO usar márgenes internos de texto (text_margin_*)
			// Los márgenes ya están manejados por la posición y el tamaño
			_dialogText.AddThemeConstantOverride("text_margin_left", 0);
			_dialogText.AddThemeConstantOverride("text_margin_right", 0);
			_dialogText.AddThemeConstantOverride("text_margin_top", 0);
			_dialogText.AddThemeConstantOverride("text_margin_bottom", 0);

			// Aplicar estilo de texto usando FontManager
			ApplyDialogTextStyle(_dialogText);

			// MEJORADO: Crear Label de sombra detrás del texto para efecto visual mejorado
			// Esto asegura que el texto tenga profundidad y contraste incluso si RichTextLabel no soporta sombras nativas
			CreateDialogTextShadow();

			// Agregar RichTextLabel directamente al panel
			_dialogPanel.AddChild(_dialogText);
		}

		/// <summary>
		/// Crea un Label de sombra detrás del texto del diálogo para efecto visual mejorado
		/// Esto proporciona profundidad y contraste al texto
		/// </summary>
		private void CreateDialogTextShadow()
		{
			_dialogTextShadow = new Label();
			_dialogTextShadow.Name = "DialogTextShadow";
			_dialogTextShadow.LayoutMode = 0; // Position mode, igual que el texto principal
			
			// Posicionar ligeramente desplazado para efecto de sombra (3px de offset para mejor visibilidad)
			var viewportSize = GetViewportRect().Size;
			_dialogTextShadow.Position = new Vector2(123, 83); // 3px de offset para sombra más visible (ajustado para nueva posición del texto)
			_dialogTextShadow.Size = _dialogText.Size; // Mismo tamaño que el texto principal
			
			// Configurar estilo de sombra - usar el mismo tamaño de fuente que el texto principal
			float baseFontSize = viewportSize.Y * 0.025f;
			float scaledSize = baseFontSize * 1.2f;
			_dialogTextShadow.AddThemeFontSizeOverride("font_size", (int)scaledSize);
			
			// Sombra oscura más visible para mejor contraste
			_dialogTextShadow.AddThemeColorOverride("font_color", new Color(0.0f, 0.0f, 0.0f, 0.6f));

			// Agregar outline adicional a la sombra para más profundidad
			_dialogTextShadow.AddThemeColorOverride("font_outline_color", new Color(0.0f, 0.0f, 0.0f, 0.4f));
			_dialogTextShadow.AddThemeConstantOverride("outline_size", 1);
			
			_dialogTextShadow.AutowrapMode = TextServer.AutowrapMode.WordSmart;
			_dialogTextShadow.ClipContents = false;
			_dialogTextShadow.Visible = false; // Se mostrará cuando haya texto
			_dialogTextShadow.MouseFilter = Control.MouseFilterEnum.Ignore; // No interceptar clics
			
			// Agregar detrás del texto principal (menor ZIndex)
			_dialogPanel.AddChild(_dialogTextShadow);
			_dialogTextShadow.ZIndex = _dialogText.ZIndex - 1; // Detrás del texto principal
		}

		/// <summary>
		/// Crea el indicador parpadeante en la esquina inferior derecha
		/// Aparece cuando el texto termina de escribirse para indicar que se puede continuar
		/// </summary>
		private void CreateContinueIndicator()
		{
			_continueIndicator = new Control();
			_continueIndicator.Name = "ContinueIndicator";
			
			// Posicionar en esquina inferior derecha
			_continueIndicator.SetAnchorsAndOffsetsPreset(LayoutPreset.BottomRight);
			_continueIndicator.OffsetLeft = -30 - 16; // 30px desde la derecha + ancho del punto
			_continueIndicator.OffsetTop = -50; // 50px desde la parte inferior
			_continueIndicator.OffsetRight = -30; // 30px desde la derecha
			_continueIndicator.OffsetBottom = -30; // 30px desde la parte inferior
			
			// Crear un punto circular usando un Panel con estilo
			var point = new Panel();
			point.Name = "BlinkPoint";
			point.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);

			// Estilo del punto principal - círculo blanco
			var pointStyle = new StyleBoxFlat();
			pointStyle.BgColor = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Blanco
			pointStyle.CornerRadiusTopLeft = 8; // Radio grande para hacerlo circular
			pointStyle.CornerRadiusTopRight = 8;
			pointStyle.CornerRadiusBottomLeft = 8;
			pointStyle.CornerRadiusBottomRight = 8;
			point.AddThemeStyleboxOverride("panel", pointStyle);
			
			// Tamaño del indicador (punto circular)
			_continueIndicator.CustomMinimumSize = new Vector2(16, 16);
			_continueIndicator.Size = new Vector2(16, 16);
			
			_continueIndicator.AddChild(point);
			_continueIndicator.Visible = false; // Inicialmente oculto
			_continueIndicator.MouseFilter = Control.MouseFilterEnum.Ignore; // No interceptar clics

			_dialogPanel.AddChild(_continueIndicator);
		}

		/// <summary>
		/// Crea el overlay de pantalla completa para las opciones de respuesta
		/// </summary>
		private void CreateOptionsContainer()
		{
			// Crear overlay de pantalla completa
			_optionsOverlay = new Control();
			_optionsOverlay.Name = "OptionsOverlay";
			_optionsOverlay.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			_optionsOverlay.MouseFilter = Control.MouseFilterEnum.Stop; // Bloquear clics a través
			_optionsOverlay.Visible = false;
			_optionsOverlay.ZIndex = 2000; // Por encima del diálogo (que está en 1000)

			// Crear panel oscuro de fondo
			_optionsBackgroundPanel = new Panel();
			_optionsBackgroundPanel.Name = "OptionsBackgroundPanel";
			_optionsBackgroundPanel.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			_optionsBackgroundPanel.MouseFilter = Control.MouseFilterEnum.Ignore; // Permitir clics a través

			// Aplicar estilo oscuro semi-transparente
			var backgroundStyle = new StyleBoxFlat();
			backgroundStyle.BgColor = new Color(0, 0, 0, 0.8f); // Negro con 80% de opacidad
			_optionsBackgroundPanel.AddThemeStyleboxOverride("panel", backgroundStyle);
			_optionsOverlay.AddChild(_optionsBackgroundPanel);

			// Crear contenedor centrado para las opciones (debe estar después del panel de fondo para estar por encima)
			var centerContainer = new CenterContainer();
			centerContainer.Name = "OptionsCenterContainer";
			centerContainer.SetAnchorsAndOffsetsPreset(LayoutPreset.FullRect);
			centerContainer.MouseFilter = Control.MouseFilterEnum.Stop; // Bloquear clics en el contenedor
			centerContainer.Visible = true; // Asegurar que sea visible
			_optionsOverlay.AddChild(centerContainer);

			_optionsContainer = new VBoxContainer();
			_optionsContainer.Name = "OptionsContainer";
			_optionsContainer.CustomMinimumSize = new Vector2(500, 300);
			_optionsContainer.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;
			_optionsContainer.SizeFlagsVertical = Control.SizeFlags.ShrinkCenter;
			_optionsContainer.AddThemeConstantOverride("separation", 15);
			_optionsContainer.Alignment = BoxContainer.AlignmentMode.Center;
			_optionsContainer.Visible = true; // Asegurar que sea visible
			centerContainer.AddChild(_optionsContainer);

			_optionButtons = new List<Button>();

			// Agregar el overlay como hijo del DialogBox (no del panel de diálogo)
			AddChild(_optionsOverlay);
		}

		/// <summary>
		/// Crea el timer para el efecto parpadeante del indicador
		/// </summary>
		private void CreateBlinkTimer()
		{
			_blinkTimer = new Timer();
			_blinkTimer.Name = "BlinkTimer";
			_blinkTimer.WaitTime = 0.5f; // Parpadear cada 0.5 segundos
			_blinkTimer.OneShot = false;
			_blinkTimer.Timeout += OnBlinkTimerTimeout;

			AddChild(_blinkTimer);
		}

		/// <summary>
		/// Crea el timer para el efecto de escritura
		/// </summary>
		private void CreateTextTimer()
		{
			_textTimer = new Timer();
			_textTimer.Name = "TextTimer";
			_textTimer.WaitTime = 0.03f; // Velocidad de escritura
			_textTimer.OneShot = false;
			_textTimer.Timeout += OnTextTimerTimeout;

			AddChild(_textTimer);
		}

		/// <summary>
		/// Configura las conexiones de señales
		/// </summary>
		private void SetupConnections()
		{
			// Inicialmente oculto
			Visible = false;
		}

		/// <summary>
		/// Aplica estilo de texto al RichTextLabel del diálogo
		/// Mejora el estilo con sombras y outlines para que el texto se vea más bonito y elegante
		/// </summary>
		/// <param name="richTextLabel">RichTextLabel al que aplicar el estilo</param>
		private void ApplyDialogTextStyle(RichTextLabel richTextLabel)
		{
			if (richTextLabel == null) return;

			// Aplicar tamaño de fuente responsivo al viewport
			// Detectar si es teléfono (pantalla pequeña)
			var screenSize = DisplayServer.ScreenGetSize();
			bool isPhone = screenSize.X < 1000 || screenSize.Y < 1000;
			
			// Tamaño de fuente: 2.5% de la altura del viewport (responsivo, más grande)
			// En teléfonos, aumentar el porcentaje base para letras más grandes
			var viewportSize = GetViewportRect().Size;
			float baseFontSizePercent = isPhone ? 0.035f : 0.025f; // 3.5% en teléfonos, 2.5% en pantallas grandes
			float baseFontSize = viewportSize.Y * baseFontSizePercent;
			float scaledSize = baseFontSize * 1.2f; // 20% más grande para diálogos
			richTextLabel.AddThemeFontSizeOverride("normal_font_size", (int)scaledSize);
			
			// CRÍTICO: Configurar el tamaño de fuente para texto en cursiva (italics)
			// Si no se configura, el texto en [I] usará el tamaño por defecto del tema, que puede ser más pequeño
			richTextLabel.AddThemeFontSizeOverride("italics_font_size", (int)(scaledSize));
			
			// También configurar tamaño para texto en negrita por si acaso
			richTextLabel.AddThemeFontSizeOverride("bold_font_size", (int)scaledSize);

			// Aplicar color blanco elegante con ligera calidez para mejor legibilidad
			richTextLabel.AddThemeColorOverride("default_color", new Color(0.98f, 0.98f, 0.95f, 1.0f));

			// MEJORADO: Agregar sombra elegante para dar profundidad al texto
			// CRÍTICO: En RichTextLabel, las propiedades de sombra pueden requerir configuración adicional
			// Intentar múltiples formas de aplicar sombra para compatibilidad
			richTextLabel.AddThemeColorOverride("font_shadow_color", new Color(0.1f, 0.1f, 0.1f, 0.8f));
			richTextLabel.AddThemeConstantOverride("shadow_offset_x", 3);
			richTextLabel.AddThemeConstantOverride("shadow_offset_y", 3);
			// También intentar sin prefijo "font_" por si acaso
			richTextLabel.AddThemeColorOverride("shadow_color", new Color(0.1f, 0.1f, 0.1f, 0.8f));

			// MEJORADO: Agregar outline sutil para mejor contraste y elegancia
			// CRÍTICO: En RichTextLabel, las propiedades de outline pueden requerir configuración adicional
			// Intentar múltiples formas de aplicar outline para compatibilidad
			richTextLabel.AddThemeColorOverride("font_outline_color", new Color(0.2f, 0.2f, 0.2f, 0.9f));
			richTextLabel.AddThemeConstantOverride("outline_size", 2);
			// También intentar sin prefijo "font_" por si acaso
			richTextLabel.AddThemeColorOverride("outline_color", new Color(0.2f, 0.2f, 0.2f, 0.9f));

			// Configurar espaciado de líneas para mejor legibilidad
			richTextLabel.AddThemeConstantOverride("line_separation", 4); // Aumentado para mejor respiración
			richTextLabel.AddThemeConstantOverride("paragraph_separation", 6); // Aumentado para mejor separación

			// IMPORTANTE: NO usar márgenes internos de texto (text_margin_*)
			// Los márgenes ya están manejados por los offsets del RichTextLabel en CreateDialogText()
			// Usar márgenes internos adicionales puede causar que el texto se salga del contenedor
			richTextLabel.AddThemeConstantOverride("text_margin_left", 0);
			richTextLabel.AddThemeConstantOverride("text_margin_right", 0);
			richTextLabel.AddThemeConstantOverride("text_margin_top", 0);
			richTextLabel.AddThemeConstantOverride("text_margin_bottom", 0);

			// IMPORTANTE: Asegurar que el texto se ajuste correctamente
			// Esto fuerza al RichTextLabel a respetar los límites del contenedor
			richTextLabel.CustomMinimumSize = Vector2.Zero; // Permitir que se ajuste al contenedor
		}

		/// <summary>
		/// Muestra el diálogo con el texto especificado
		/// </summary>
		/// <param name="text">Texto a mostrar en el diálogo</param>
		/// <param name="characterName">Nombre del personaje que habla (opcional)</param>
		public void ShowDialog(string text, string characterName = null)
		{
			GD.Print("DialogBox.ShowDialog llamado con texto: " + text.Substring(0, Mathf.Min(50, text.Length)) + "...");

			// Mostrar u ocultar el panel de nombre del personaje con animación elegante
			if (!string.IsNullOrEmpty(characterName))
			{
				_characterNameLabel.Text = characterName;
				
				// IMPORTANTE: Ajustar el tamaño del panel según el texto del nombre
				// Calcular el ancho necesario basado en el texto
				CallDeferred(nameof(UpdateCharacterNamePanelSize), characterName);
				
				// MEJORADO: Mostrar con animación fade-in elegante
				_characterNamePanel.Visible = true;
				AnimateNamePanelAppearance();
			}
			else
			{
				// Ocultar con animación fade-out
				AnimateNamePanelDisappearance();
			}

			_fullText = text;
			_currentCharIndex = 0;
			_isTyping = true;

			// IMPORTANTE: Asegurar que el RichTextLabel respete los límites del contenedor
			// Forzar actualización del tamaño ANTES de establecer el texto
			if (_dialogText != null && IsInstanceValid(_dialogText))
			{
				// IMPORTANTE: Establecer el tamaño ANTES de establecer el texto
				// Esto es crítico para que el AutowrapMode funcione correctamente
				ForceUpdateTextSize();

				// Asegurar que el AutowrapMode esté activo
				_dialogText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
				_dialogText.ClipContents = false; // Recortar contenido que se salga

				// IMPORTANTE: Con LayoutMode = 0, NO usar SizeFlags
				// El tamaño se controla directamente con Size
			}

			// Configurar el texto completo en el RichTextLabel con BBCode habilitado
			_dialogText.BbcodeEnabled = true;
			_dialogText.Text = text;
			_dialogText.VisibleCharacters = 0;

			// MEJORADO: Actualizar el texto de sombra (sin BBCode para simplicidad)
			// Remover tags BBCode básicos para el label de sombra
			if (_dialogTextShadow != null)
			{
				string shadowText = text;
				// Remover tags BBCode comunes de forma más completa
				shadowText = shadowText.Replace("[B]", "").Replace("[/B]", "");
				shadowText = shadowText.Replace("[I]", "").Replace("[/I]", "");
				shadowText = shadowText.Replace("[U]", "").Replace("[/U]", "");
				shadowText = shadowText.Replace("[S]", "").Replace("[/S]", "");
				shadowText = shadowText.Replace("[CODE]", "").Replace("[/CODE]", "");
				// Remover tags de color (pueden tener parámetros)
				shadowText = System.Text.RegularExpressions.Regex.Replace(shadowText, @"\[COLOR=.*?\]", "");
				shadowText = shadowText.Replace("[/COLOR]", "");
				// Remover cualquier otro tag BBCode (incluyendo tags con parámetros)
				shadowText = System.Text.RegularExpressions.Regex.Replace(shadowText, @"\[.*?\]", "");
				_dialogTextShadow.Text = shadowText;
				_dialogTextShadow.Visible = true;
				_dialogTextShadow.VisibleCharacters = 0; // Iniciar con 0 caracteres visibles
			}

			// IMPORTANTE: Forzar recálculo del texto después de establecer el tamaño
			// Esto asegura que el AutowrapMode funcione correctamente
			CallDeferred(nameof(ForceTextRecalculation));

			// Ocultar el indicador al iniciar nuevo diálogo
			if (_continueIndicator != null)
			{
				_continueIndicator.Visible = false;
				_blinkTimer.Stop();
			}
			_optionsContainer.Visible = false;
			Visible = true;

			// Asegurar que esté en la parte superior
			ZIndex = 1000;

			// Forzar actualización del tamaño
			var viewportSize = GetViewportRect().Size;
			GD.Print("Viewport size: " + viewportSize);
			GD.Print("DialogBox visible: " + Visible);
			GD.Print("DialogBox position: " + Position);
			GD.Print("DialogBox size: " + Size);
			GD.Print("DialogBox parent: " + (GetParent()?.Name ?? "null"));
			GD.Print("DialogPanel size: " + (_dialogPanel?.Size ?? Vector2.Zero));

			_textTimer.Start();
		}

		/// <summary>
		/// Timer visual para decisiones en tiempo
		/// </summary>
		private ProgressBar _timeBar;
		private Label _timeLabel;
		private Timer _timeBarTimer;
		
		/// <summary>
		/// Tiempo límite pendiente para iniciar después de la animación del texto duplicado
		/// </summary>
		private float _pendingTimeLimit = 0f;

		/// <summary>
		/// Muestra las opciones de respuesta al jugador en un overlay de pantalla completa
		/// </summary>
		/// <param name="options">Lista de opciones a mostrar</param>
		/// <param name="onOptionSelected">Callback que se ejecuta cuando se selecciona una opción (recibe el índice)</param>
		/// <param name="isTimed">Indica si es una decisión en tiempo</param>
		/// <param name="timeLimit">Tiempo límite en segundos (solo si isTimed es true)</param>
		/// <param name="isTruthLie">Indica si es una decisión de verdad/mentira</param>
		public void ShowOptions(List<DialogOption> options, System.Action<int> onOptionSelected, bool isTimed = false, float timeLimit = 0f, bool isTruthLie = false)
		{
			if (options == null || options.Count == 0)
			{
				GD.PrintErr("No hay opciones para mostrar");
				return;
			}

			// Ocultar indicador si está visible
			if (_continueIndicator != null)
			{
				_continueIndicator.Visible = false;
				_blinkTimer.Stop();
			}

			// Limpiar opciones anteriores
			ClearOptions();
			
			// MEJORADO: Ocultar el dialog box original cuando aparecen las opciones
			// Esto evita redundancia visual y mejora la claridad
			if (_dialogPanel != null && IsInstanceValid(_dialogPanel))
			{
				_dialogPanel.Visible = false;
			}
			
			// MEJORADO: Duplicar el texto del diálogo y agregarlo al overlay de opciones
			// Esto permite animar el texto sin tocar la lógica del textbox principal
			DuplicateDialogTextForAnimation();

			// Crear botones para cada opción
			for (int i = 0; i < options.Count; i++)
			{
				var option = options[i];
				var button = CreateOptionButton(option.Text, i, onOptionSelected);
				_optionButtons.Add(button);
				_optionsContainer.AddChild(button);
			}

			// Mostrar el overlay de pantalla completa y todos sus contenedores
			_optionsOverlay.Visible = true;
			if (_optionsBackgroundPanel != null)
			{
				_optionsBackgroundPanel.Visible = true;
			}
			var centerContainer = _optionsOverlay.GetNodeOrNull<CenterContainer>("OptionsCenterContainer");
			if (centerContainer != null)
			{
				centerContainer.Visible = true;
			}
			if (_optionsContainer != null)
			{
				_optionsContainer.Visible = true;
			}
			
			// Si es una decisión en tiempo, preparar timer (pero NO iniciarlo hasta que termine la animación)
			if (isTimed && timeLimit > 0f)
			{
				_pendingTimeLimit = timeLimit; // Guardar para iniciar después de la animación
				ShowTimedDecisionUI(timeLimit, startTimer: false); // Preparar UI pero NO iniciar timer
			}
			else
			{
				HideTimedDecisionUI();
			}
			
			// Si es verdad/mentira, aplicar estilos especiales
			if (isTruthLie)
			{
				ApplyTruthLieStyles();
			}
			
			GD.Print($"✅ Mostrando {options.Count} opciones de respuesta en overlay de pantalla completa (Timed: {isTimed}, TruthLie: {isTruthLie})");
			
			// MEJORADO: Animar el texto duplicado hacia arriba después de mostrar las opciones
			CallDeferred(nameof(AnimateDuplicatedTextUp));
		}
		
		/// <summary>
		/// Duplica el texto del diálogo y lo agrega al overlay de opciones en la misma posición
		/// Esto permite animar el texto sin tocar la lógica del textbox principal
		/// </summary>
		private void DuplicateDialogTextForAnimation()
		{
			if (_dialogText == null || !IsInstanceValid(_dialogText) || string.IsNullOrEmpty(_fullText))
			{
				GD.Print("[DialogBox] No hay texto para duplicar");
				return;
			}
			
			// Limpiar texto duplicado anterior si existe
			if (_duplicatedTextPanel != null && IsInstanceValid(_duplicatedTextPanel))
			{
				_duplicatedTextPanel.QueueFree();
				_duplicatedTextPanel = null;
				_duplicatedDialogText = null;
			}
			else if (_duplicatedDialogText != null && IsInstanceValid(_duplicatedDialogText))
			{
				_duplicatedDialogText.QueueFree();
				_duplicatedDialogText = null;
			}
			
			// Crear RichTextLabel duplicado
			_duplicatedDialogText = new RichTextLabel();
			_duplicatedDialogText.Name = "DuplicatedDialogText";
			_duplicatedDialogText.LayoutMode = 0; // Position mode, igual que el original
			
			// MEJORADO: Calcular posición inicial usando GetGlobalRect
			// El overlay está en FullRect, así que su posición relativa al viewport es (0, 0)
			// Necesitamos la posición del texto original en coordenadas del viewport
			
			// Obtener el rectángulo global del texto original (en coordenadas del viewport)
			var textGlobalRect = _dialogText.GetGlobalRect();
			
			// Obtener el rectángulo global del overlay
			var overlayGlobalRect = _optionsOverlay.GetGlobalRect();
			
			// CRÍTICO: Calcular posición relativa al overlay
			// Si el overlay está en FullRect, overlayGlobalRect debería empezar en (0, 0)
			// Por lo tanto, la posición relativa es simplemente textGlobalRect.Position - overlayGlobalRect.Position
			Vector2 relativePos = textGlobalRect.Position - overlayGlobalRect.Position;
			
			// Guardar esta posición para usarla más adelante en el posicionamiento del panel
			// Por ahora, establecer una posición temporal para el texto (se ajustará dentro del panel)
			_duplicatedDialogText.Position = Vector2.Zero; // Se ajustará dentro del panel
			
			GD.Print($"[DialogBox] Texto original - Rect global: {textGlobalRect}");
			GD.Print($"[DialogBox] Overlay - Rect global: {overlayGlobalRect}");
			GD.Print($"[DialogBox] Posición relativa del texto original: {relativePos}");
			
			// Copiar tamaño del texto original
			_duplicatedDialogText.Size = _dialogText.Size;
			
			// Copiar todas las propiedades del texto original
			_duplicatedDialogText.BbcodeEnabled = true;
			_duplicatedDialogText.Text = _dialogText.Text; // Texto completo con BBCode
			_duplicatedDialogText.VisibleCharacters = _dialogText.VisibleCharacters; // Mismo número de caracteres visibles
			_duplicatedDialogText.AutowrapMode = _dialogText.AutowrapMode;
			
			// Copiar todos los estilos del texto original
			// Copiar tamaño de fuente
			int fontSize = _dialogText.GetThemeFontSize("normal_font_size");
			if (fontSize > 0)
			{
				_duplicatedDialogText.AddThemeFontSizeOverride("normal_font_size", fontSize);
			}
			
			// Copiar colores
			var defaultColor = _dialogText.GetThemeColor("default_color");
			_duplicatedDialogText.AddThemeColorOverride("default_color", defaultColor);
			
			var shadowColor = _dialogText.GetThemeColor("font_shadow_color");
			if (shadowColor.A > 0)
			{
				_duplicatedDialogText.AddThemeColorOverride("font_shadow_color", shadowColor);
				_duplicatedDialogText.AddThemeConstantOverride("shadow_offset_x", _dialogText.GetThemeConstant("shadow_offset_x"));
				_duplicatedDialogText.AddThemeConstantOverride("shadow_offset_y", _dialogText.GetThemeConstant("shadow_offset_y"));
			}
			
			var outlineColor = _dialogText.GetThemeColor("font_outline_color");
			if (outlineColor.A > 0)
			{
				_duplicatedDialogText.AddThemeColorOverride("font_outline_color", outlineColor);
				_duplicatedDialogText.AddThemeConstantOverride("outline_size", _dialogText.GetThemeConstant("outline_size"));
			}
			
			// Copiar separación de líneas
			_duplicatedDialogText.AddThemeConstantOverride("line_separation", _dialogText.GetThemeConstant("line_separation"));
			_duplicatedDialogText.AddThemeConstantOverride("paragraph_separation", _dialogText.GetThemeConstant("paragraph_separation"));
			
			// MEJORADO: Crear un panel de fondo para el texto duplicado para mejor integración visual
			_duplicatedTextPanel = new Panel();
			_duplicatedTextPanel.Name = "DuplicatedTextPanel";
			_duplicatedTextPanel.LayoutMode = 0; // Position mode
			
			// Calcular tamaño del panel (más ancho que el texto para mejor presentación)
			var viewportSize = GetViewportRect().Size;
			float panelWidth = viewportSize.X * 0.95f; // 70% del ancho del viewport
			float panelHeight = _duplicatedDialogText.Size.Y + 40f; // Altura del texto + márgenes
			
			// MEJORADO: Posicionar el panel inicialmente en la posición del texto original
			// Luego se animará hacia arriba
			// Usar la posición relativa calculada anteriormente (relativePos)
			// Posición inicial: donde está el texto original (ajustada para el panel)
			float panelX = relativePos.X - 20f; // Ajustar para centrar mejor
			float panelY = relativePos.Y - 20f; // Ajustar para centrar mejor
			
			// Posición final: centrado horizontalmente, arriba de las opciones
			float finalPanelX = (viewportSize.X - panelWidth) / 2f;
			float finalPanelY = viewportSize.Y * 0.00f; // 15% desde arriba
			
			_duplicatedTextPanel.Position = new Vector2(panelX, panelY);
			_duplicatedTextPanel.Size = new Vector2(panelWidth, panelHeight);
			
			// Aplicar estilo al panel (fondo semi-transparente elegante)
			var panelStyle = new StyleBoxFlat();
			panelStyle.BgColor = new Color(0.1f, 0.1f, 0.15f, 0.85f); // Fondo oscuro semi-transparente
			panelStyle.CornerRadiusTopLeft = 12;
			panelStyle.CornerRadiusTopRight = 12;
			panelStyle.CornerRadiusBottomLeft = 12;
			panelStyle.CornerRadiusBottomRight = 12;
			panelStyle.BorderColor = new Color(0.5f, 0.5f, 0.6f, 0.8f);
			panelStyle.BorderWidthLeft = 2;
			panelStyle.BorderWidthTop = 2;
			panelStyle.BorderWidthRight = 2;
			panelStyle.BorderWidthBottom = 2;
			_duplicatedTextPanel.AddThemeStyleboxOverride("panel", panelStyle);
			
			// Ajustar posición del texto dentro del panel (centrado con márgenes)
			_duplicatedDialogText.Position = new Vector2(20, 20); // Márgenes internos
			_duplicatedDialogText.Size = new Vector2(panelWidth - 40, panelHeight - 40); // Ajustar tamaño
			_duplicatedDialogText.HorizontalAlignment = HorizontalAlignment.Center; // Centrar texto
			
			// Agregar texto al panel
			_duplicatedTextPanel.AddChild(_duplicatedDialogText);
			
			// Agregar panel al overlay (por encima del fondo pero debajo de las opciones)
			_duplicatedTextPanel.ZIndex = 100; // Por encima del fondo pero debajo de las opciones
			_duplicatedTextPanel.MouseFilter = Control.MouseFilterEnum.Ignore; // No interceptar clics
			
			// IMPORTANTE: Iniciar invisible para luego hacer fade-in
			_duplicatedTextPanel.Modulate = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			
			_optionsOverlay.AddChild(_duplicatedTextPanel);
			
			GD.Print($"[DialogBox] Panel de texto duplicado creado en posición: {_duplicatedTextPanel.Position}, Tamaño: {_duplicatedTextPanel.Size}");
		}
		
		/// <summary>
		/// Anima el panel del texto duplicado: primero aparece (fade-in) y luego sube gradualmente
		/// El panel empieza en la posición del texto original y se mueve hacia arriba
		/// CRÍTICO: Cuando termine la animación, inicia el temporizador si es una decisión con tiempo
		/// </summary>
		private void AnimateDuplicatedTextUp()
		{
			if (_duplicatedTextPanel == null || !IsInstanceValid(_duplicatedTextPanel))
			{
				return;
			}
			
			// Obtener el tamaño del viewport para calcular la posición final
			var viewportSize = GetViewportRect().Size;
			
			// Calcular posición final: centrado horizontalmente, arriba de las opciones
			float panelWidth = _duplicatedTextPanel.Size.X;
			float finalPanelX = (viewportSize.X - panelWidth) / 2f;
			float finalPanelY = viewportSize.Y * 0.07f; // 15% desde arriba
			
			float currentY = _duplicatedTextPanel.Position.Y;
			
			// Crear tween secuencial: primero fade-in, luego movimiento
			var tween = CreateTween();
			tween.SetParallel(false); // Secuencial para controlar el orden
			
			// Paso 1: Fade-in del panel (aparecer)
			tween.TweenProperty(_duplicatedTextPanel, "modulate:a", 1.0f, 0.3f)
				.SetEase(Tween.EaseType.Out)
				.SetTrans(Tween.TransitionType.Quad);
			
			// Paso 2: Mover hacia arriba y centrar (después del fade-in)
			// Animar tanto X como Y para centrar y subir
			tween.TweenProperty(_duplicatedTextPanel, "position:x", finalPanelX, 1.2f)
				.SetEase(Tween.EaseType.Out)
				.SetTrans(Tween.TransitionType.Quad);
			
			tween.TweenProperty(_duplicatedTextPanel, "position:y", finalPanelY, 1.2f)
				.SetEase(Tween.EaseType.Out)
				.SetTrans(Tween.TransitionType.Quad);
			
			// CRÍTICO: Cuando termine la animación, iniciar el temporizador si es necesario
			tween.TweenCallback(Callable.From(OnDuplicatedTextAnimationFinished));
			
			GD.Print($"[DialogBox] Animando panel de texto duplicado: fade-in y luego subir desde Y={currentY} hasta Y={finalPanelY}...");
		}
		
		/// <summary>
		/// Se llama cuando termina la animación del texto duplicado
		/// Inicia el temporizador si es una decisión con tiempo
		/// </summary>
		private void OnDuplicatedTextAnimationFinished()
		{
			GD.Print("[DialogBox] ✅ Animación del texto duplicado terminada - iniciando temporizador si es necesario");
			
			// Emitir señal para que el código externo pueda manejar eventos (como reproducir sonidos)
			EmitSignal(SignalName.DuplicatedTextAnimationFinished);
			
			// Iniciar el temporizador si está preparado pero no iniciado
			if (_pendingTimeLimit > 0f)
			{
				StartTimedDecisionTimer(_pendingTimeLimit);
				
				// CRÍTICO: Notificar a DialogSystem que puede iniciar su timer
				EmitSignal(SignalName.TimedDecisionReadyToStart, _pendingTimeLimit);
				
				_pendingTimeLimit = 0f; // Limpiar
			}
		}
		
		/// <summary>
		/// Muestra la UI de decisión en tiempo (barra de tiempo)
		/// </summary>
		/// <param name="timeLimit">Tiempo límite en segundos</param>
		/// <param name="startTimer">Si es true, inicia el timer inmediatamente. Si es false, solo prepara la UI.</param>
		private void ShowTimedDecisionUI(float timeLimit, bool startTimer = true)
		{
			// Crear contenedor principal para el temporizador en la parte inferior si no existe
			Control timeContainer = null;
			if (_optionsOverlay != null)
			{
				timeContainer = _optionsOverlay.GetNodeOrNull<Control>("TimeContainer");
				if (timeContainer == null)
				{
					timeContainer = new Control();
					timeContainer.Name = "TimeContainer";
					
					// CRÍTICO: Posicionar en la parte inferior usando anclas
					timeContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
					
					// Configurar márgenes desde abajo
					var viewportSize = GetViewportRect().Size;
					float bottomMargin = viewportSize.Y * 0.08f; // 8% desde abajo
					timeContainer.OffsetBottom = -bottomMargin;
					timeContainer.OffsetTop = -bottomMargin - 120f; // Altura del contenedor
					
					// Centrar horizontalmente
					float containerWidth = viewportSize.X * 0.5f; // 50% del ancho
					timeContainer.OffsetLeft = (viewportSize.X - containerWidth) / 2f;
					timeContainer.OffsetRight = -(viewportSize.X - containerWidth) / 2f;
					
					_optionsOverlay.AddChild(timeContainer);
				}
			}
			
			// Crear barra de tiempo mejorada si no existe
			if (_timeBar == null)
			{
				_timeBar = new ProgressBar();
				_timeBar.Name = "TimeBar";
				_timeBar.MinValue = 0f;
				_timeBar.MaxValue = timeLimit;
				_timeBar.Value = timeLimit;
				_timeBar.CustomMinimumSize = new Vector2(500, 8); // Más ancha y delgada para efecto elegante
				_timeBar.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
				
				// Estilo de fondo con efecto de brillo sutil
				var bgStyle = new StyleBoxFlat();
				bgStyle.BgColor = new Color(0.15f, 0.15f, 0.15f, 0.85f); // Fondo oscuro semi-transparente
				bgStyle.BorderColor = new Color(0.4f, 0.4f, 0.4f, 0.6f);
				bgStyle.BorderWidthLeft = 1;
				bgStyle.BorderWidthTop = 1;
				bgStyle.BorderWidthRight = 1;
				bgStyle.BorderWidthBottom = 1;
				bgStyle.CornerRadiusTopLeft = 4;
				bgStyle.CornerRadiusTopRight = 4;
				bgStyle.CornerRadiusBottomLeft = 4;
				bgStyle.CornerRadiusBottomRight = 4;
				_timeBar.AddThemeStyleboxOverride("background", bgStyle);
				
				// Estilo de relleno con gradiente (se actualizará dinámicamente)
				var fillStyle = new StyleBoxFlat();
				fillStyle.BgColor = new Color(0.2f, 0.6f, 1.0f, 1.0f); // Azul inicial (calma)
				fillStyle.CornerRadiusTopLeft = 4;
				fillStyle.CornerRadiusTopRight = 4;
				fillStyle.CornerRadiusBottomLeft = 4;
				fillStyle.CornerRadiusBottomRight = 4;
				_timeBar.AddThemeStyleboxOverride("fill", fillStyle);
				
				// Posicionar la barra en el contenedor
				_timeBar.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.BottomWide);
				_timeBar.OffsetBottom = -10f;
				_timeBar.OffsetTop = -18f;
				
				if (timeContainer != null)
				{
					timeContainer.AddChild(_timeBar);
				}
			}
			
			// Crear label de tiempo mejorado con efectos visuales
			if (_timeLabel == null)
			{
				_timeLabel = new Label();
				_timeLabel.Name = "TimeLabel";
				_timeLabel.Text = $"{timeLimit:F1}s";
				_timeLabel.HorizontalAlignment = HorizontalAlignment.Center;
				_timeLabel.VerticalAlignment = VerticalAlignment.Center;
				
				// Estilo de fuente más grande y elegante
				float fontSize = FontManager.GetScaledSize(FontManager.TextType.Large) * 1.2f;
				_timeLabel.AddThemeFontSizeOverride("font_size", (int)fontSize);
				_timeLabel.AddThemeColorOverride("font_color", new Color(0.95f, 0.95f, 1.0f, 1.0f));
				
				// Agregar sombra y outline para mejor legibilidad
				_timeLabel.AddThemeColorOverride("font_shadow_color", new Color(0.0f, 0.0f, 0.0f, 0.9f));
				_timeLabel.AddThemeConstantOverride("shadow_offset_x", 2);
				_timeLabel.AddThemeConstantOverride("shadow_offset_y", 2);
				_timeLabel.AddThemeColorOverride("font_outline_color", new Color(0.1f, 0.1f, 0.2f, 0.8f));
				_timeLabel.AddThemeConstantOverride("outline_size", 1);
				
				// Posicionar el label encima de la barra
				_timeLabel.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopWide);
				_timeLabel.OffsetBottom = -35f; // Justo encima de la barra
				_timeLabel.OffsetTop = -55f;
				
				if (timeContainer != null)
				{
					timeContainer.AddChild(_timeLabel);
				}
			}
			
			// Crear efecto de pulso/brillo (ColorRect animado)
			if (timeContainer != null)
			{
				var pulseEffect = timeContainer.GetNodeOrNull<ColorRect>("PulseEffect");
				if (pulseEffect == null)
				{
					pulseEffect = new ColorRect();
					pulseEffect.Name = "PulseEffect";
					pulseEffect.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
					pulseEffect.Color = new Color(1.0f, 1.0f, 1.0f, 0.0f); // Inicialmente transparente
					pulseEffect.MouseFilter = Control.MouseFilterEnum.Ignore;
					timeContainer.AddChild(pulseEffect);
					timeContainer.MoveChild(pulseEffect, 0); // Al fondo
				}
			}
			
			// Configurar timer para actualizar barra
			if (_timeBarTimer == null)
			{
				_timeBarTimer = new Timer();
				_timeBarTimer.WaitTime = 0.05f; // Actualizar cada 0.05 segundos para animaciones más suaves
				_timeBarTimer.Timeout += UpdateTimeBar;
				AddChild(_timeBarTimer);
			}
			
			_timeBar.MaxValue = timeLimit;
			_timeBar.Value = timeLimit;
			_timeBar.Visible = true;
			if (_timeLabel != null) _timeLabel.Visible = true;
			
			// Animación de entrada elegante
			if (timeContainer != null)
			{
				timeContainer.Modulate = new Color(1, 1, 1, 0);
				var fadeInTween = CreateTween();
				fadeInTween.TweenProperty(timeContainer, "modulate:a", 1.0f, 0.3f)
					.SetEase(Tween.EaseType.Out)
					.SetTrans(Tween.TransitionType.Cubic);
			}
			
			// CRÍTICO: Solo iniciar el timer si se solicita explícitamente
			if (startTimer)
			{
				_timeBarTimer.Start();
			}
		}
		
		/// <summary>
		/// Inicia el temporizador de decisión después de que termine la animación del texto duplicado
		/// </summary>
		private void StartTimedDecisionTimer(float timeLimit)
		{
			GD.Print($"[DialogBox] ✅ Iniciando temporizador de decisión: {timeLimit} segundos");
			
			// Asegurar que la UI esté visible
			if (_timeBar != null)
			{
				_timeBar.Visible = true;
				_timeBar.MaxValue = timeLimit;
				_timeBar.Value = timeLimit;
			}
			if (_timeLabel != null)
			{
				_timeLabel.Visible = true;
				_timeLabel.Text = $"{timeLimit:F1}s";
			}
			
			// Iniciar el timer de actualización visual
			if (_timeBarTimer != null)
			{
				_timeBarTimer.Start();
			}
			
			// Notificar a DialogSystem que inicie su timer también
			// Esto se hace a través de una señal o llamando directamente
			// Por ahora, DialogSystem ya tiene su timer configurado, solo necesita iniciarlo
		}
		
		/// <summary>
		/// Actualiza la barra de tiempo con efectos visuales innovadores
		/// Incluye cambios de color suaves, efectos de pulso y animaciones
		/// </summary>
		private void UpdateTimeBar()
		{
			if (_timeBar == null) return;
			
			_timeBar.Value -= 0.05f; // Actualizar con el nuevo intervalo
			
			float timePercent = (float)(_timeBar.Value / _timeBar.MaxValue);
			
			// Actualizar label de tiempo
			if (_timeLabel != null)
			{
				_timeLabel.Text = $"{Mathf.Max(0f, _timeBar.Value):F1}s";
				
				// Cambio de color suave según tiempo restante con transiciones elegantes
				Color labelColor;
				if (timePercent <= 0.25f) // Menos del 25% - Rojo intenso con pulso
				{
					// Efecto de pulso rojo
					float pulse = 0.8f + 0.2f * Mathf.Sin(Engine.GetProcessFrames() * 0.3f);
					labelColor = new Color(1.0f, 0.2f * pulse, 0.2f * pulse, 1.0f);
				}
				else if (timePercent <= 0.5f) // Menos del 50% - Naranja/Amarillo
				{
					// Transición suave de amarillo a naranja
					float t = (0.5f - timePercent) / 0.25f; // 0 a 1
					labelColor = new Color(1.0f, 0.5f + 0.5f * t, 0.2f * t, 1.0f);
				}
				else if (timePercent <= 0.75f) // Menos del 75% - Amarillo suave
				{
					labelColor = new Color(1.0f, 0.9f, 0.5f, 1.0f);
				}
				else // Más del 75% - Azul/Blanco (calma)
				{
					labelColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
				}
				
				// Aplicar color con animación suave
				if (_timeLabel.Modulate != labelColor)
				{
					var colorTween = CreateTween();
					colorTween.TweenProperty(_timeLabel, "modulate", labelColor, 0.2f)
						.SetEase(Tween.EaseType.Out)
						.SetTrans(Tween.TransitionType.Cubic);
				}
			}
			
			// Actualizar color de la barra con gradiente suave
			var fillStyle = _timeBar.GetThemeStylebox("fill") as StyleBoxFlat;
			if (fillStyle != null)
			{
				Color barColor;
				if (timePercent <= 0.25f) // Rojo intenso con efecto de "sangre"
				{
					float pulse = 0.7f + 0.3f * Mathf.Sin(Engine.GetProcessFrames() * 0.4f);
					barColor = new Color(1.0f, 0.1f * pulse, 0.1f * pulse, 1.0f);
				}
				else if (timePercent <= 0.5f) // Naranja
				{
					float t = (0.5f - timePercent) / 0.25f;
					barColor = new Color(1.0f, 0.4f + 0.3f * t, 0.1f, 1.0f);
				}
				else if (timePercent <= 0.75f) // Amarillo
				{
					barColor = new Color(1.0f, 0.8f, 0.3f, 1.0f);
				}
				else // Azul (calma inicial)
				{
					barColor = new Color(0.2f, 0.6f, 1.0f, 1.0f);
				}
				
				fillStyle.BgColor = barColor;
				_timeBar.AddThemeStyleboxOverride("fill", fillStyle);
			}
			
			// Efecto de pulso cuando el tiempo está en amarillo o menos (comienza en 75%)
			if (timePercent <= 0.30f && _optionsOverlay != null)
			{
				var timeContainer = _optionsOverlay.GetNodeOrNull<Control>("TimeContainer");
				if (timeContainer != null)
				{
					var pulseEffect = timeContainer.GetNodeOrNull<ColorRect>("PulseEffect");
					if (pulseEffect != null)
					{
						// Pulso rojo que aumenta en intensidad a medida que el tiempo se agota
						// Más sutil en amarillo (75%), más intenso en rojo (0%)
						float intensity = 1.0f - (timePercent / 0.75f); // 0 a 1 desde amarillo hasta rojo
						float baseAlpha = 0.08f + (0.12f * intensity); // De 0.08 a 0.20
						float pulseAlpha = baseAlpha * Mathf.Abs(Mathf.Sin(Engine.GetProcessFrames() * 0.5f));
						
						// Color que va de naranja suave (amarillo) a rojo intenso
						float redIntensity = 0.3f + (0.7f * intensity); // De 0.3 a 1.0
						pulseEffect.Color = new Color(1.0f, 0.1f * redIntensity, 0.1f * redIntensity, pulseAlpha);
					}
				}
			}
			
			// Efecto de "glitch" o distorsión cuando queda muy poco tiempo
			if (timePercent <= 0.1f && _timeLabel != null)
			{
				// Efecto de "shake" sutil
				float shake = (0.1f - timePercent) * 2f; // Intensidad aumenta
				float offsetX = (GD.Randf() - 0.5f) * shake * 3f;
				float offsetY = (GD.Randf() - 0.5f) * shake * 3f;
				_timeLabel.Position = new Vector2(offsetX, offsetY);
			}
			else if (_timeLabel != null)
			{
				// Restaurar posición normal
				_timeLabel.Position = Vector2.Zero;
			}
			
			if (_timeBar.Value <= 0f)
			{
				_timeBarTimer?.Stop();
				
				// Efecto final de desvanecimiento
				if (_optionsOverlay != null)
				{
					var timeContainer = _optionsOverlay.GetNodeOrNull<Control>("TimeContainer");
					if (timeContainer != null)
					{
						var fadeOutTween = CreateTween();
						fadeOutTween.TweenProperty(timeContainer, "modulate:a", 0.0f, 0.2f)
							.SetEase(Tween.EaseType.In)
							.SetTrans(Tween.TransitionType.Cubic);
					}
				}
			}
		}
		
		/// <summary>
		/// Oculta la UI de decisión en tiempo
		/// </summary>
		/// <summary>
		/// Oculta la UI de decisión en tiempo con animación elegante
		/// </summary>
		public void HideTimedDecisionUI()
		{
			if (_timeBarTimer != null) _timeBarTimer.Stop();
			
			// Ocultar con animación de fade-out si existe el contenedor
			if (_optionsOverlay != null)
			{
				var timeContainer = _optionsOverlay.GetNodeOrNull<Control>("TimeContainer");
				if (timeContainer != null && timeContainer.Visible)
				{
					var fadeOutTween = CreateTween();
					fadeOutTween.TweenProperty(timeContainer, "modulate:a", 0.0f, 0.2f)
						.SetEase(Tween.EaseType.In)
						.SetTrans(Tween.TransitionType.Cubic);
					fadeOutTween.TweenCallback(Callable.From(() => {
						if (timeContainer != null && IsInstanceValid(timeContainer))
						{
							timeContainer.Visible = false;
						}
					}));
				}
				else
				{
					// Fallback: ocultar directamente si no hay contenedor
					if (_timeBar != null) _timeBar.Visible = false;
					if (_timeLabel != null) _timeLabel.Visible = false;
				}
			}
			else
			{
				// Fallback: ocultar directamente
				if (_timeBar != null) _timeBar.Visible = false;
				if (_timeLabel != null) _timeLabel.Visible = false;
			}
			
			// Restaurar posición del label si tenía shake
			if (_timeLabel != null)
			{
				_timeLabel.Position = Vector2.Zero;
			}
		}
		
		/// <summary>
		/// Aplica estilos especiales para opciones de verdad/mentira
		/// Estilo más sutil y elegante para novela visual
		/// IMPORTANTE: Usa la misma imagen de fondo que los botones normales
		/// </summary>
		private void ApplyTruthLieStyles()
		{
			// Cargar textura de fondo
			var buttonTexture = GD.Load<Texture2D>(BUTTON_OPTION_BACKGROUND_PATH);
			
			// Aplicar estilos sutiles y elegantes a los botones
			// Similar a botones normales pero con modulación de color para distinguir
			for (int i = 0; i < _optionButtons.Count; i++)
			{
				var button = _optionButtons[i];
				
				if (buttonTexture != null)
				{
					// Estilo base con textura
					var style = new StyleBoxTexture();
					style.Texture = buttonTexture;
					style.ContentMarginLeft = 10;
					style.ContentMarginRight = 10;
					style.ContentMarginTop = 10;
					style.ContentMarginBottom = 10;
					button.AddThemeStyleboxOverride("normal", style);
					
					// Hover - misma textura
					var hoverStyle = new StyleBoxTexture();
					hoverStyle.Texture = buttonTexture;
					hoverStyle.ContentMarginLeft = 10;
					hoverStyle.ContentMarginRight = 10;
					hoverStyle.ContentMarginTop = 10;
					hoverStyle.ContentMarginBottom = 10;
					button.AddThemeStyleboxOverride("hover", hoverStyle);
					
					// Aplicar modulación de color al botón para distinguir verdad/mentira
					if (i == 0) // Primera opción = Verdad
					{
						// Modulación verde/azul sutil
						button.Modulate = new Color(0.9f, 1.0f, 0.95f, 1.0f);
					}
					else // Segunda opción = Mentira
					{
						// Modulación rojo/rosa sutil
						button.Modulate = new Color(1.0f, 0.9f, 0.95f, 1.0f);
					}
				}
				else
				{
					// Fallback: StyleBoxFlat si no se carga la imagen
					var style = new StyleBoxFlat();
					style.BgColor = new Color(0.15f, 0.15f, 0.15f, 0.95f); // Fondo oscuro elegante
					
					// Bordes sutiles para distinguir
					if (i == 0) // Primera opción = Verdad
					{
						// Borde sutil verde/azul claro
						style.BorderColor = new Color(0.3f, 0.6f, 0.5f, 0.8f);
						style.BorderWidthLeft = 3;
						style.BorderWidthTop = 3;
						style.BorderWidthRight = 3;
						style.BorderWidthBottom = 3;
					}
					else // Segunda opción = Mentira
					{
						// Borde sutil rojo/rosa claro
						style.BorderColor = new Color(0.6f, 0.3f, 0.4f, 0.8f);
						style.BorderWidthLeft = 3;
						style.BorderWidthTop = 3;
						style.BorderWidthRight = 3;
						style.BorderWidthBottom = 3;
					}
					
					style.CornerRadiusTopLeft = 10;
					style.CornerRadiusTopRight = 10;
					style.CornerRadiusBottomLeft = 10;
					style.CornerRadiusBottomRight = 10;
					button.AddThemeStyleboxOverride("normal", style);
					
					// Hover elegante - fondo ligeramente más claro con borde más brillante
					var hoverStyle = new StyleBoxFlat();
					hoverStyle.BgColor = new Color(0.25f, 0.25f, 0.25f, 0.95f);
					hoverStyle.BorderColor = style.BorderColor.Lightened(0.3f);
					hoverStyle.BorderWidthLeft = 3;
					hoverStyle.BorderWidthTop = 3;
					hoverStyle.BorderWidthRight = 3;
					hoverStyle.BorderWidthBottom = 3;
					hoverStyle.CornerRadiusTopLeft = 10;
					hoverStyle.CornerRadiusTopRight = 10;
					hoverStyle.CornerRadiusBottomLeft = 10;
					hoverStyle.CornerRadiusBottomRight = 10;
					button.AddThemeStyleboxOverride("hover", hoverStyle);
				}
				
				// Asegurar que el texto sea blanco elegante y legible con sombras
				button.AddThemeColorOverride("font_color", new Color(0.98f, 0.98f, 0.95f, 1.0f));
				button.AddThemeColorOverride("font_hover_color", new Color(1.0f, 1.0f, 0.9f, 1.0f)); // Ligeramente más cálido en hover
				
				// Agregar sombra para mejor legibilidad
				button.AddThemeColorOverride("font_shadow_color", new Color(0.1f, 0.1f, 0.1f, 0.7f));
				button.AddThemeConstantOverride("shadow_offset_x", 2);
				button.AddThemeConstantOverride("shadow_offset_y", 2);
				
				// Agregar outline sutil para mejor contraste
				button.AddThemeColorOverride("font_outline_color", new Color(0.15f, 0.15f, 0.15f, 0.8f));
				button.AddThemeConstantOverride("outline_size", 1);
			}
		}

		/// <summary>
		/// Ruta de la imagen de fondo para botones de opciones
		/// </summary>
		private const string BUTTON_OPTION_BACKGROUND_PATH = "res://src/Image/Gui/button_option.png";

		/// <summary>
		/// Crea un botón para una opción de respuesta
		/// IMPORTANTE: Usa StyleBoxTexture con la imagen button_option.png como fondo
		/// </summary>
		private Button CreateOptionButton(string text, int index, System.Action<int> onOptionSelected)
		{
			var button = new Button();
			button.Name = $"OptionButton_{index}";
			button.Text = text;
			button.CustomMinimumSize = new Vector2(450, 60);
			button.SizeFlagsHorizontal = Control.SizeFlags.ShrinkCenter;

			// Aplicar estilo al botón con mejor tipografía
			float scaledSize = FontManager.GetScaledSize(FontManager.TextType.Large);
			button.AddThemeFontSizeOverride("font_size", (int)scaledSize);
			
			// Color blanco elegante
			button.AddThemeColorOverride("font_color", new Color(0.98f, 0.98f, 0.95f, 1.0f));
			button.AddThemeColorOverride("font_hover_color", new Color(1.0f, 1.0f, 0.9f, 1.0f)); // Ligeramente más cálido en hover
			
			// MEJORADO: Agregar sombra más visible para mejor legibilidad (similar al texto del diálogo)
			button.AddThemeColorOverride("font_shadow_color", new Color(0.0f, 0.0f, 0.0f, 0.85f));
			button.AddThemeConstantOverride("shadow_offset_x", 3);
			button.AddThemeConstantOverride("shadow_offset_y", 3);
			
			// MEJORADO: Agregar outline más visible para mejor contraste (similar al texto del diálogo)
			button.AddThemeColorOverride("font_outline_color", new Color(0.2f, 0.2f, 0.2f, 0.9f));
			button.AddThemeConstantOverride("outline_size", 2);

			// CRÍTICO: Aplicar fondo con imagen usando StyleBoxTexture
			var buttonTexture = GD.Load<Texture2D>(BUTTON_OPTION_BACKGROUND_PATH);
			if (buttonTexture != null)
			{
				// Estilo normal con textura - MÁRGENES AUMENTADOS para dar más espacio al texto
				var buttonStyle = new StyleBoxTexture();
				buttonStyle.Texture = buttonTexture;
				buttonStyle.ContentMarginLeft = 25; // Aumentado de 10 a 25
				buttonStyle.ContentMarginRight = 25; // Aumentado de 10 a 25
				buttonStyle.ContentMarginTop = 20; // Aumentado de 10 a 20
				buttonStyle.ContentMarginBottom = 20; // Aumentado de 10 a 20
				button.AddThemeStyleboxOverride("normal", buttonStyle);
				
				// Hover effect - usar la misma textura pero con modulación más brillante
				var hoverStyle = new StyleBoxTexture();
				hoverStyle.Texture = buttonTexture;
				hoverStyle.ContentMarginLeft = 25; // Aumentado de 10 a 25
				hoverStyle.ContentMarginRight = 25; // Aumentado de 10 a 25
				hoverStyle.ContentMarginTop = 20; // Aumentado de 10 a 20
				hoverStyle.ContentMarginBottom = 20; // Aumentado de 10 a 20
				button.AddThemeStyleboxOverride("hover", hoverStyle);
				
				// Aplicar modulación más brillante en hover usando el modulate del botón
				// Nota: Esto se manejará con un efecto visual adicional si es necesario
			}
			else
			{
				// Fallback: StyleBoxFlat si no se carga la imagen
				GD.PrintErr($"[DialogBox] No se pudo cargar la imagen de fondo del botón: {BUTTON_OPTION_BACKGROUND_PATH}");
			var buttonStyle = new StyleBoxFlat();
			buttonStyle.BgColor = new Color(0.2f, 0.2f, 0.2f, 0.9f);
			buttonStyle.ContentMarginLeft = 25; // Aumentado para dar más espacio al texto
			buttonStyle.ContentMarginRight = 25;
			buttonStyle.ContentMarginTop = 20;
			buttonStyle.ContentMarginBottom = 20;
			buttonStyle.CornerRadiusTopLeft = 8;
			buttonStyle.CornerRadiusTopRight = 8;
			buttonStyle.CornerRadiusBottomLeft = 8;
			buttonStyle.CornerRadiusBottomRight = 8;
			button.AddThemeStyleboxOverride("normal", buttonStyle);

			var hoverStyle = new StyleBoxFlat();
			hoverStyle.BgColor = new Color(0.3f, 0.3f, 0.3f, 0.9f);
			hoverStyle.ContentMarginLeft = 25; // Aumentado para dar más espacio al texto
			hoverStyle.ContentMarginRight = 25;
			hoverStyle.ContentMarginTop = 20;
			hoverStyle.ContentMarginBottom = 20;
			hoverStyle.CornerRadiusTopLeft = 8;
			hoverStyle.CornerRadiusTopRight = 8;
			hoverStyle.CornerRadiusBottomLeft = 8;
			hoverStyle.CornerRadiusBottomRight = 8;
			button.AddThemeStyleboxOverride("hover", hoverStyle);
			}

			// Conectar evento
			button.Pressed += () =>
			{
				onOptionSelected?.Invoke(index);
			};

			return button;
		}

		/// <summary>
		/// Actualiza el tamaño del panel del nombre del personaje según el texto
		/// Hace que el panel sea adaptativo y solo ocupe el espacio necesario
		/// </summary>
		/// <param name="characterName">Nombre del personaje</param>
		private void UpdateCharacterNamePanelSize(string characterName)
		{
			if (_characterNameLabel == null || _characterNamePanel == null || string.IsNullOrEmpty(characterName))
			{
				return;
			}

			// MEJORADO: Usar el tamaño real del texto renderizado para cálculo más preciso
			// Obtener el tamaño de fuente del label
			int fontSize = _characterNameLabel.GetThemeFontSize("font_size");
			
			// Calcular el ancho real del texto usando el Font del label
			// Primero intentar obtener el font del tema
			var font = _characterNameLabel.GetThemeFont("font");
			float textWidth = 0f;
			
			if (font != null)
			{
				// Usar el font para calcular el ancho real del texto
				textWidth = font.GetStringSize(characterName, HorizontalAlignment.Left, -1, fontSize).X;
			}
			else
			{
				// Fallback: usar aproximación mejorada
				// Considerar que algunos caracteres son más anchos que otros
				float averageCharWidth = fontSize * 0.65f; // Ajustado para mejor aproximación
				textWidth = characterName.Length * averageCharWidth;
			}
			
			// Agregar márgenes internos (24px izquierda + 24px derecha = 48px)
			float panelWidth = textWidth + 48f;
			
			// Establecer un ancho mínimo razonable
			float minWidth = 80f; // Reducido para permitir nombres más cortos
			if (panelWidth < minWidth)
			{
				panelWidth = minWidth;
			}
			
			// Establecer un ancho máximo (no más del 50% del ancho del viewport)
			var viewportSize = GetViewportRect().Size;
			float maxWidth = viewportSize.X * 0.5f;
			if (panelWidth > maxWidth)
			{
				panelWidth = maxWidth;
			}
			
			// Actualizar el tamaño del panel
			// Mantener la posición izquierda y superior, ajustar solo el ancho
			_characterNamePanel.OffsetRight = _characterNamePanel.OffsetLeft + panelWidth;
			
			GD.Print($"[DialogBox] Panel de nombre ajustado: ancho = {panelWidth}px (texto: {textWidth}px) para '{characterName}'");
		}

		/// <summary>
		/// Anima la aparición del panel de nombre con un fade-in elegante
		/// Sigue mejores prácticas de UX/UI para novelas visuales
		/// </summary>
		private void AnimateNamePanelAppearance()
		{
			if (_characterNamePanel == null || !IsInstanceValid(_characterNamePanel)) return;
			
			// Crear tween para animación fade-in suave
			var tween = CreateTween();
			tween.SetParallel(true); // Permitir animaciones paralelas
			
			// Fade-in del panel (de transparente a opaco)
			tween.TweenProperty(_characterNamePanel, "modulate:a", 1.0f, 0.3f)
				.SetEase(Tween.EaseType.Out)
				.SetTrans(Tween.TransitionType.Cubic);
			
			// Opcional: Ligero movimiento desde arriba para efecto elegante
			var initialY = _characterNamePanel.Position.Y;
			_characterNamePanel.Position = new Vector2(_characterNamePanel.Position.X, initialY - 10);
			tween.TweenProperty(_characterNamePanel, "position:y", initialY, 0.3f)
				.SetEase(Tween.EaseType.Out)
				.SetTrans(Tween.TransitionType.Cubic);
		}

		/// <summary>
		/// Anima la desaparición del panel de nombre con un fade-out elegante
		/// </summary>
		private void AnimateNamePanelDisappearance()
		{
			if (_characterNamePanel == null || !IsInstanceValid(_characterNamePanel)) return;
			
			// Crear tween para animación fade-out suave
			var tween = CreateTween();
			
			// Fade-out del panel (de opaco a transparente)
			tween.TweenProperty(_characterNamePanel, "modulate:a", 0.0f, 0.2f)
				.SetEase(Tween.EaseType.In)
				.SetTrans(Tween.TransitionType.Cubic);
			
			// Ocultar después de la animación
			tween.TweenCallback(Callable.From(() => {
				if (_characterNamePanel != null && IsInstanceValid(_characterNamePanel))
				{
					_characterNamePanel.Visible = false;
					_characterNameLabel.Text = "";
				}
			}));
		}

		/// <summary>
		/// Limpia todas las opciones actuales
		/// </summary>
		private void ClearOptions()
		{
			foreach (var button in _optionButtons)
			{
				if (button != null && IsInstanceValid(button))
				{
					button.QueueFree();
				}
			}
			_optionButtons.Clear();
			
			// MEJORADO: Eliminar el texto duplicado y su panel si existen
			if (_duplicatedTextPanel != null && IsInstanceValid(_duplicatedTextPanel))
			{
				_duplicatedTextPanel.QueueFree();
				_duplicatedTextPanel = null;
				_duplicatedDialogText = null;
			}
			else if (_duplicatedDialogText != null && IsInstanceValid(_duplicatedDialogText))
			{
				_duplicatedDialogText.QueueFree();
				_duplicatedDialogText = null;
			}
			
			// CRÍTICO: Ocultar todos los elementos del overlay
			if (_optionsOverlay != null)
			{
			_optionsOverlay.Visible = false;
			}
			if (_optionsBackgroundPanel != null)
			{
				_optionsBackgroundPanel.Visible = false;
			}
			var centerContainer = _optionsOverlay?.GetNodeOrNull<CenterContainer>("OptionsCenterContainer");
			if (centerContainer != null)
			{
				centerContainer.Visible = false;
			}
			if (_optionsContainer != null)
			{
				_optionsContainer.Visible = false;
			}
			
			// MEJORADO: Restaurar visibilidad del dialog box original
			EnsureDialogPanelVisible();
		}

		/// <summary>
		/// Asegura que el panel del diálogo esté visible
		/// Útil después de ocultar opciones o cuando se inicia un nuevo diálogo
		/// </summary>
		public void EnsureDialogPanelVisible()
		{
			if (_dialogPanel != null && IsInstanceValid(_dialogPanel))
			{
				_dialogPanel.Visible = true;
				GD.Print("[DialogBox] Panel del diálogo restaurado a visible");
			}
		}
		
		/// <summary>
		/// Oculta las opciones
		/// </summary>
		public void HideOptions()
		{
			ClearOptions();
		}

		/// <summary>
		/// Fuerza la actualización del tamaño del RichTextLabel para asegurar que respete los límites del contenedor
		/// IMPORTANTE: Con LayoutMode = 0 (Position mode), el tamaño se controla directamente
		/// CRÍTICO: La altura debe calcularse basándose en el tamaño de fuente y espaciado de líneas para 3 líneas completas
		/// </summary>
		private void ForceUpdateTextSize()
		{
			if (_dialogText == null || !IsInstanceValid(_dialogText)) return;

			// IMPORTANTE: Usar el tamaño del viewport, no del panel
			// El panel puede ser más grande que el viewport, causando que el texto se salga
			var viewportSize = GetViewportRect().Size;

			// IMPORTANTE: Usar el ancho del viewport, no del panel
			// Esto asegura que el texto no se salga del viewport
			float viewportWidth = viewportSize.X;

			// Calcular el ancho disponible considerando los márgenes
			// Márgenes: 120px izquierda, 30px derecha
			float availableWidth = viewportWidth - 120 - 30; // Ancho del viewport menos márgenes

			// IMPORTANTE: Asegurar que el ancho sea positivo
			if (availableWidth <= 0) availableWidth = 100; // Fallback mínimo

			// IMPORTANTE: Asegurar que el ancho no exceda el viewport
			// Esto es crítico para evitar que el texto se salga de la pantalla
			if (availableWidth > viewportWidth - 120 - 30)
			{
				availableWidth = viewportWidth - 120 - 30;
			}

			// CRÍTICO: Calcular la altura mínima necesaria para 3 líneas completas
			// Hacer los tamaños responsivos al viewport
			// Detectar si es teléfono (pantalla pequeña)
			var screenSize = DisplayServer.ScreenGetSize();
			bool isPhone = screenSize.X < 1000 || screenSize.Y < 1000;
			
			// Tamaño de fuente base: 2.5% de la altura del viewport (responsivo)
			// En teléfonos, aumentar el porcentaje base para letras más grandes
			float baseFontSizePercent = isPhone ? 0.035f : 0.025f; // 3.5% en teléfonos, 2.5% en pantallas grandes
			float baseFontSize = viewportSize.Y * baseFontSizePercent;
			float fontSize = baseFontSize * 1.2f; // 20% más grande para diálogos
			int lineSeparation = Mathf.Max(4, (int)(baseFontSize * 0.08f)); // 8% del tamaño de fuente, mínimo 4px
			const int MAX_LINES = 3; // Máximo de líneas por página
			float extraMargin = baseFontSize * 0.8f; // 80% del tamaño de fuente (aumentado para asegurar 3 líneas)

			// Calcular altura para 3 líneas:
			// - Altura de cada línea = fontSize
			// - Espaciado entre líneas = lineSeparation (hay 2 espacios entre 3 líneas)
			// - Margen adicional para evitar cortes = extraMargin (aumentado)
			float minHeightFor3Lines = (fontSize * MAX_LINES) + (lineSeparation * (MAX_LINES - 1)) + extraMargin;

			// IMPORTANTE: Usar la altura calculada, no la altura del panel
			// Esto garantiza que siempre haya espacio para 3 líneas completas
			float availableHeight = minHeightFor3Lines;

			// IMPORTANTE: Asegurar que la altura sea positiva
			if (availableHeight <= 0) availableHeight = 200; // Fallback mínimo

			// IMPORTANTE: Con LayoutMode = 0, establecer el tamaño directamente
			// Esto fuerza al RichTextLabel a respetar los límites del contenedor
			var newSize = new Vector2(availableWidth, availableHeight);

			// IMPORTANTE: Siempre establecer el tamaño, incluso si es similar
			// Esto asegura que el RichTextLabel tenga el tamaño correcto antes de renderizar
			_dialogText.Size = newSize;
			
			// MEJORADO: Actualizar también el tamaño de la sombra
			if (_dialogTextShadow != null)
			{
				_dialogTextShadow.Size = newSize;
			}

			GD.Print($"[DialogBox] Forzando tamaño del RichTextLabel a {newSize} (Viewport: {viewportSize})");
			GD.Print($"[DialogBox] RichTextLabel Position: {_dialogText.Position}, Size: {_dialogText.Size}");
			GD.Print($"[DialogBox] Ancho disponible: {availableWidth}, Alto mínimo para 3 líneas: {availableHeight}");
			GD.Print($"[DialogBox] FontSize: {fontSize}, LineSeparation: {lineSeparation}, Cálculo: ({fontSize} * {MAX_LINES}) + ({lineSeparation} * {MAX_LINES - 1}) + {extraMargin} = {minHeightFor3Lines}");
		}

		/// <summary>
		/// Fuerza el recálculo del texto después de establecer el tamaño
		/// Esto asegura que el AutowrapMode funcione correctamente
		/// </summary>
		private void ForceTextRecalculation()
		{
			if (_dialogText == null || !IsInstanceValid(_dialogText)) return;

			// IMPORTANTE: Forzar recálculo del texto estableciendo el texto nuevamente
			// Esto asegura que el AutowrapMode funcione correctamente con el nuevo tamaño
			var currentText = _dialogText.Text;
			if (!string.IsNullOrEmpty(currentText))
			{
				// Forzar recálculo estableciendo el texto vacío y luego el texto completo
				_dialogText.Text = "";
				_dialogText.Text = currentText;

				// IMPORTANTE: Asegurar que el AutowrapMode esté activo
				_dialogText.AutowrapMode = TextServer.AutowrapMode.WordSmart;

				GD.Print($"[DialogBox] Texto recalculado, Size: {_dialogText.Size}");
			}
		}

		/// <summary>
		/// Oculta el diálogo
		/// </summary>
		public void HideDialog()
		{
			Visible = false;
			_textTimer.Stop();
			_isTyping = false;
			// Ocultar el indicador
			if (_continueIndicator != null)
			{
				_continueIndicator.Visible = false;
				_blinkTimer.Stop();
			}
		}

		/// <summary>
		/// Cambia la emoción del slime durante el diálogo
		/// </summary>
		/// <param name="emotion">Nueva emoción del slime</param>
		public void ChangeSlimeEmotion(Emotion emotion)
		{
			EmitSignal(SignalName.SlimeChangeEmotion, (int)emotion);
			GD.Print("DialogBox solicitó cambio de emoción del slime a: " + emotion);
		}

		/// <summary>
		/// Maneja el evento del timer para el efecto de escritura
		/// </summary>
		private void OnTextTimerTimeout()
		{
			if (!_isTyping || _currentCharIndex >= _fullText.Length)
			{
				_textTimer.Stop();
				_isTyping = false;
				// Mostrar el indicador parpadeante cuando termine el texto
				if (_continueIndicator != null)
				{
					_continueIndicator.Visible = true;
					_continueIndicator.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Asegurar opacidad completa al iniciar
					_blinkTimer.Start();
				}
				return;
			}

			// Usar visible_characters para el efecto de escritura
			_dialogText.VisibleCharacters = _currentCharIndex;
			
			// MEJORADO: Actualizar también la sombra para que coincida con el efecto de escritura
			if (_dialogTextShadow != null && _dialogTextShadow.Visible)
			{
				_dialogTextShadow.VisibleCharacters = _currentCharIndex;
			}
			
			_currentCharIndex++;
		}
		
		/// <summary>
		/// Maneja el evento del timer para el efecto parpadeante
		/// </summary>
		private void OnBlinkTimerTimeout()
		{
			if (_continueIndicator != null && _continueIndicator.Visible)
			{
				// Alternar la opacidad para crear efecto parpadeante suave
				var currentAlpha = _continueIndicator.Modulate.A;
				// Alternar entre opacidad completa y baja para efecto parpadeante visible
				_continueIndicator.Modulate = new Color(1.0f, 1.0f, 1.0f, currentAlpha > 0.5f ? 0.2f : 1.0f);
			}
		}


		/// <summary>
		/// Maneja el evento de continuar el diálogo (llamado desde input o click)
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// </summary>
		private void OnContinuePressed()
		{
			// CRÍTICO: No permitir avanzar si hay opciones visibles
			if (HasOptionsVisible())
			{
				GD.Print("[DialogBox] No se puede avanzar: hay opciones visibles esperando selección");
				return;
			}

			if (_isTyping)
			{
				// Si está escribiendo, mostrar todo el texto inmediatamente
				SkipTyping();
			}
			else
			{
				// Si ya terminó de escribir, ocultar el indicador y avanzar
				if (_continueIndicator != null)
				{
					_continueIndicator.Visible = false;
					_blinkTimer.Stop();
				}
				// Cerrar el diálogo y avanzar
				EmitSignal(SignalName.ContinuePressed);
				EmitSignal(SignalName.DialogFinished);
			}
		}

		/// <summary>
		/// Verifica si el texto está siendo escrito actualmente
		/// </summary>
		/// <returns>True si está escribiendo</returns>
		public bool IsTyping()
		{
			return _isTyping;
		}

		/// <summary>
		/// Verifica si hay opciones de respuesta visibles
		/// </summary>
		/// <returns>True si hay opciones visibles</returns>
		public bool HasOptionsVisible()
		{
			return _optionsOverlay != null && IsInstanceValid(_optionsOverlay) && _optionsOverlay.Visible;
		}

		/// <summary>
		/// Termina inmediatamente el efecto de escritura y muestra todo el texto
		/// </summary>
		public void SkipTyping()
		{
			if (_isTyping)
			{
				_dialogText.VisibleCharacters = -1; // Mostrar todo el texto
				
				// MEJORADO: Actualizar también la sombra
				if (_dialogTextShadow != null && _dialogTextShadow.Visible)
				{
					_dialogTextShadow.VisibleCharacters = -1;
				}
				
				_textTimer.Stop();
				_isTyping = false;
				// Mostrar el indicador parpadeante cuando termine el texto
				if (_continueIndicator != null)
				{
					_continueIndicator.Visible = true;
					_continueIndicator.Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f); // Asegurar opacidad completa al iniciar
					_blinkTimer.Start();
				}
			}
		}

		/// <summary>
		/// Maneja el input del mouse para avanzar el diálogo con click
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// </summary>
		public override void _Input(InputEvent @event)
		{
			// Solo procesar si el diálogo está visible
			if (!Visible) return;

			// Solo procesar clicks si no hay opciones visibles
			if (HasOptionsVisible())
			{
				return; // No permitir avanzar si hay opciones visibles
			}

			// Detectar click del mouse
			if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
			{
				// Solo procesar click izquierdo
				if (mouseEvent.ButtonIndex == MouseButton.Left)
				{
					// Verificar que el click esté dentro del área del diálogo
					if (_dialogPanel != null && IsInstanceValid(_dialogPanel))
					{
						// Obtener posición global del mouse
						var globalMousePos = mouseEvent.GlobalPosition;
						// Obtener posición global del panel
						var panelGlobalPos = _dialogPanel.GlobalPosition;
						var panelSize = _dialogPanel.Size;

						// Verificar si el click está dentro del panel
						if (globalMousePos.X >= panelGlobalPos.X &&
							globalMousePos.X <= panelGlobalPos.X + panelSize.X &&
							globalMousePos.Y >= panelGlobalPos.Y &&
							globalMousePos.Y <= panelGlobalPos.Y + panelSize.Y)
						{
							// Avanzar el diálogo
							OnContinuePressed();
							GetViewport().SetInputAsHandled();
						}
					}
				}
			}
		}
	}
}
