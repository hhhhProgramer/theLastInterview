using Godot;

namespace SlimeKingdomChronicles.Core.UI
{
	/// <summary>
	/// Componente reutilizable que crea un panel con fondo de panel.png
	/// y texto estilizado usando FontManager
	/// </summary>
	public partial class PanelWithBackground : Control
	{
		private TextureRect _backgroundPanel;
		private RichTextLabel _contentLabel;
		private Label _titleLabel;
		
		/// <summary>
		/// Inicializa el panel con fondo y configuración básica
		/// </summary>
		/// <param name="title">Título del panel</param>
		/// <param name="size">Tamaño del panel</param>
		public void Initialize(string title, Vector2 size)
		{
			// Configurar tamaño del panel
			Size = size;
			
			// Crear fondo con panel.png
			CreateBackground();
			
			// Crear título
			CreateTitle(title);
			
			// Crear RichTextLabel para el contenido
			CreateContentLabel();
		}
		
		/// <summary>
		/// Crea el fondo del panel usando panel.png (basado en FishingLogPanel TextureRect)
		/// </summary>
		private void CreateBackground()
		{
			_backgroundPanel = new TextureRect();
			_backgroundPanel.Name = "TextureRect";
			_backgroundPanel.LayoutMode = 0; // Position mode
			
			// Configurar offsets basados en el .tscn (ajustados para el tamaño del panel)
			_backgroundPanel.OffsetLeft = -31.0f;
			_backgroundPanel.OffsetTop = -91.0f;
			_backgroundPanel.OffsetRight = Size.X + 31.0f;
			_backgroundPanel.OffsetBottom = Size.Y + 91.0f;
			
			// Cargar la textura del panel
			var panelTexture = GD.Load<Texture2D>("res://src/Images/panel.png");
			if (panelTexture != null)
			{
				_backgroundPanel.Texture = panelTexture;
				_backgroundPanel.ExpandMode = TextureRect.ExpandModeEnum.FitWidthProportional;
				_backgroundPanel.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
			}
			else
			{
				GD.PrintErr("No se pudo cargar panel.png");
				// Fallback a color sólido si no se encuentra la textura
				_backgroundPanel.Modulate = new Color(0.2f, 0.3f, 0.2f, 0.8f);
			}
			
			AddChild(_backgroundPanel);
		}
		
		/// <summary>
		/// Crea el RichTextLabel para el contenido (basado en FishingLogLabel)
		/// </summary>
		private void CreateContentLabel()
		{
			_contentLabel = new RichTextLabel();
			_contentLabel.Name = "ContentLabel";
			_contentLabel.LayoutMode = 0; // Position mode para control directo
			_contentLabel.Position = new Vector2(60, 60); // Padding desde la esquina superior izquierda
			_contentLabel.Size = new Vector2(Size.X - 40, Size.Y - 60); // Tamaño del panel menos padding
			_contentLabel.BbcodeEnabled = true;
			_contentLabel.FitContent = false; // No expandir para ajustar contenido
			_contentLabel.ScrollActive = false; // Desactivar scroll
			_contentLabel.ClipContents = true; // Cortar contenido que se salga
			_contentLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart; // Wrap inteligente
			
			// Aplicar estilo de texto pequeño usando FontManager
			ApplyRichTextStyle(_contentLabel);
			
			AddChild(_contentLabel);
		}
		
		/// <summary>
		/// Crea el título del panel
		/// </summary>
		/// <param name="title">Texto del título</param>
		private void CreateTitle(string title)
		{
			_titleLabel = new Label();
			_titleLabel.Name = "TitleLabel";
			_titleLabel.Text = title;
			_titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
			_titleLabel.VerticalAlignment = VerticalAlignment.Center;
			_titleLabel.LayoutMode = 1; // Anchors mode
			_titleLabel.SetAnchorsAndOffsetsPreset(LayoutPreset.TopWide);
			_titleLabel.OffsetTop = 10.0f;
			_titleLabel.OffsetBottom = 40.0f;
			
			// Aplicar estilo de subtítulo usando FontManager
			FontManager.ApplySubtitleStyle(_titleLabel);
			
			AddChild(_titleLabel);
		}
		
		
		/// <summary>
		/// Agrega una línea de información al panel usando RichTextLabel
		/// </summary>
		/// <param name="label">Etiqueta de la información</param>
		/// <param name="value">Valor de la información</param>
		/// <param name="textType">Tipo de texto a usar</param>
		public void AddInfoLine(string label, string value, TextType textType = TextType.Body)
		{
			if (_contentLabel == null) return;
			
			// Si el label está vacío, solo mostrar el value
			string lineText;
			if (string.IsNullOrEmpty(label))
			{
				lineText = value;
			}
			else
			{
				lineText = $"{label}: {value}";
			}
			
			// Agregar la línea al RichTextLabel
			string currentText = _contentLabel.Text;
			if (!string.IsNullOrEmpty(currentText))
			{
				_contentLabel.Text = $"{lineText}\n{currentText}";
			}
			else
			{
				_contentLabel.Text = lineText;
			}
		}
		
		/// <summary>
		/// Actualiza una línea de información existente
		/// </summary>
		/// <param name="index">Índice de la línea a actualizar</param>
		/// <param name="label">Nueva etiqueta</param>
		/// <param name="value">Nuevo valor</param>
		public void UpdateInfoLine(int index, string label, string value)
		{
			if (_contentLabel == null) return;
			
			// Para RichTextLabel, necesitamos reconstruir todo el contenido
			// Esto es una limitación del diseño actual, pero evita crear subpaneles
			string currentText = _contentLabel.Text;
			string[] lines = currentText.Split('\n');
			
			// Si el índice es válido, actualizar esa línea
			if (index >= 0 && index < lines.Length)
			{
				string lineText = string.IsNullOrEmpty(label) ? value : $"{label}: {value}";
				lines[index] = lineText;
				_contentLabel.Text = string.Join("\n", lines);
			}
			else
			{
				// Si el índice no es válido, agregar nueva línea
				AddInfoLine(label, value);
			}
		}
		
		/// <summary>
		/// Limpia todas las líneas de información
		/// </summary>
		public void ClearInfoLines()
		{
			if (_contentLabel != null)
			{
				_contentLabel.Text = "";
			}
		}
		
		/// <summary>
		/// Actualiza el título del panel
		/// </summary>
		/// <param name="newTitle">Nuevo título</param>
		public void UpdateTitle(string newTitle)
		{
			if (_titleLabel != null)
			{
				_titleLabel.Text = newTitle;
			}
		}
		
		/// <summary>
		/// Aplica estilo de texto pequeño al RichTextLabel
		/// </summary>
		/// <param name="richTextLabel">RichTextLabel al que aplicar el estilo</param>
		private void ApplyRichTextStyle(RichTextLabel richTextLabel)
		{
			if (richTextLabel == null) return;
			
			// Aplicar tamaño de fuente muy pequeño
			float scaledSize = FontManager.GetScaledSize(TextType.Tiny);
			richTextLabel.AddThemeFontSizeOverride("normal_font_size", (int)scaledSize);
			
			// Aplicar color gris
			richTextLabel.AddThemeColorOverride("default_color", new Color(0.7f, 0.7f, 0.7f, 1.0f));
		}
	}
}
