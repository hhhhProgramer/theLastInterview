using Core.Services;
using Godot;
using Package.Characters;
using Package.Core.Enums;
using Package.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Package.UI
{
	/// <summary>
	/// Sistema de diálogos complejo que maneja múltiples personajes y sus emociones
	/// Permite control granular de emociones por personaje durante el diálogo
	/// Funciona como autoload en cualquier escena con tamaños fijos
	/// </summary>
	public partial class DialogSystem : CanvasLayer
	{
		private DialogBox _dialogBox;
		private Dictionary<string, IEmotionalCharacter> _characters;
		private List<DialogEntry> _currentDialog;
		private int _currentDialogIndex;
		private bool _isDialogActive;

		/// <summary>
		/// Diálogos ramificados actuales (si hay opciones seleccionadas)
		/// </summary>
		private List<DialogEntry> _branchDialogs;

		/// <summary>
		/// Diálogo original antes de entrar en una rama
		/// </summary>
		private List<DialogEntry> _originalDialog;

		/// <summary>
		/// Índice original antes de entrar en una rama
		/// </summary>
		private int _originalDialogIndex;

		/// <summary>
		/// Opciones pendientes de mostrar (para CallDeferred)
		/// </summary>
		private List<DialogOption> _pendingOptions;
		
		/// <summary>
		/// Entrada de diálogo pendiente con decisión de verdad/mentira
		/// </summary>
		private DialogEntry _pendingTruthLieEntry;
		
		/// <summary>
		/// Entrada de diálogo pendiente con decisión en tiempo
		/// </summary>
		private DialogEntry _pendingTimedEntry;

		/// <summary>
		/// Opciones actualmente mostradas (para validación al seleccionar)
		/// CRÍTICO: Se usa para validar que el índice de opción seleccionado sea válido
		/// </summary>
		private List<DialogOption> _currentShownOptions;

		/// <summary>
		/// Indica si ya se está procesando una selección de opción
		/// CRÍTICO: Evita múltiples selecciones simultáneas
		/// </summary>
		private bool _isProcessingOption = false;

		/// <summary>
		/// ID del personaje que está hablando actualmente
		/// </summary>
		private string _currentSpeakingCharacterId = null;

		/// <summary>
		/// Tecla configurable para adelantar el diálogo (por defecto Z)
		/// </summary>
		public Key SkipKey { get; set; } = Key.Z;

		/// <summary>
		/// Configura la tecla para adelantar el diálogo
		/// </summary>
		/// <param name="key">Nueva tecla para adelantar</param>
		public void SetSkipKey(Key key)
		{
			SkipKey = key;

			// NOTA: El hint de "Presiona Z para continuar" fue eliminado
			// Los usuarios pueden hacer click o presionar la tecla configurada para avanzar

			GD.Print($"Tecla de skip configurada a: {key}");
		}

		/// <summary>
		/// Tamaños base relativos de los personajes (porcentaje del viewport)
		/// </summary>
		private Dictionary<string, float> _characterBaseSizes;

		/// <summary>
		/// Diccionario de emociones anteriores de los personajes para evitar transiciones innecesarias
		/// </summary>
		private Dictionary<string, Emotion> _previousEmotions;

		/// <summary>
		/// Instancia singleton del DialogSystem (para autoload)
		/// </summary>
		public static DialogSystem Instance { get; private set; }

		/// <summary>
		/// Constructor del DialogSystem
		/// </summary>
		public DialogSystem()
		{
			// Establecer la instancia singleton
			Instance = this;

			// Inicializar variables para evitar NullReferenceException
			_characters = new Dictionary<string, IEmotionalCharacter>();
			_currentDialog = new List<DialogEntry>();
			_currentDialogIndex = 0;
			_isDialogActive = false;
			_characterBaseSizes = new Dictionary<string, float>();
			_previousEmotions = new Dictionary<string, Emotion>();
		}

		/// <summary>
		/// Evento que se dispara cuando el diálogo termina completamente
		/// </summary>
		[Signal]
		public delegate void DialogFinishedEventHandler();

		/// <summary>
		/// Evento que se dispara cuando el diálogo comienza
		/// </summary>
		[Signal]
		public delegate void DialogStartedEventHandler();

		/// <summary>
		/// Evento que se dispara cuando se presiona el botón continuar
		/// </summary>
		[Signal]
		public delegate void ContinuePressedEventHandler();

		/// <summary>
		/// Evento que se dispara cuando se necesita cambiar la emoción de un personaje específico
		/// </summary>
		[Signal]
		public delegate void CharacterEmotionChangeEventHandler(string characterId, int emotion);

		/// <summary>
		/// Recarga los diálogos para la escena actual
		/// </summary>
		public void ReloadDialogs()
		{
			// Limpiar diálogos anteriores
			EndDialog();

			// Limpiar personajes registrados
			_characters.Clear();
			_characterBaseSizes.Clear();
			_previousEmotions.Clear();

			// Recrear DialogBox si es necesario
			if (_dialogBox == null || !IsInstanceValid(_dialogBox))
			{
				CreateDialogBox();
				GD.Print("DialogBox recreado para nueva escena");
			}

		}

		/// <summary>
		/// Reinicia completamente el DialogSystem a su estado inicial
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// CRÍTICO: Limpia TODAS las variables persistentes que deben reiniciarse al reiniciar el nivel
		/// </summary>
		public void Reset()
		{
			GD.Print("[DialogSystem] Reiniciando completamente...");

			// Limpiar diálogos activos
			EndDialog();

			// Limpiar todos los registros
			_characters.Clear();
			_characterBaseSizes.Clear();
			_previousEmotions.Clear();
			_currentDialog.Clear();
			_currentDialogIndex = 0;
			_isDialogActive = false;

			// CRÍTICO: Limpiar variables de diálogos ramificados que causan "Índice de opción inválido"
			_branchDialogs = null;
			_originalDialog = null;
			_originalDialogIndex = 0;
			_pendingOptions = null;
			_currentShownOptions = null;
			_isProcessingOption = false;

			// Limpiar personaje que habla actualmente
			_currentSpeakingCharacterId = null;

			// Ocultar DialogBox si existe
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				// ⚠️ CRÍTICO: Ocultar opciones ANTES de ocultar el diálogo
				// Esto asegura que el panel de opciones siempre quede invisible cuando se resetea el sistema
				_dialogBox.HideOptions();
				_dialogBox.HideDialog();
			}

			// Ocultar DialogControl si existe
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl != null && IsInstanceValid(dialogControl))
			{
				dialogControl.Visible = false;
			}

			GD.Print("[DialogSystem] ✓ Reset completo finalizado - Todas las variables persistentes limpiadas");
		}

		public override void _Ready()
		{
			// Configurar el CanvasLayer con capa alta
			Layer = 10; // Capa muy alta para estar por encima de todo

			// Configurar el DialogSystem con tamaños fijos
			ConfigureFixedSize();
			SetupDialogSystem();
			
			// Conectar al evento de cambio de tamaño del viewport
			GetViewport().SizeChanged += OnViewportSizeChanged;
		}
		
		/// <summary>
		/// Se llama cuando cambia el tamaño del viewport
		/// Actualiza tamaños y posiciones de todos los personajes
		/// IMPORTANTE: Solo actualiza posiciones si son significativamente diferentes para no interrumpir animaciones
		/// </summary>
		private void OnViewportSizeChanged()
		{
			GD.Print("[DialogSystem] Tamaño del viewport cambió, actualizando personajes...");
			UpdateAllCharactersSizeAndPosition();
		}
		
		/// <summary>
		/// Actualiza el tamaño de todos los personajes registrados
		/// IMPORTANTE: NO modifica tamaños - solo Character puede modificar su tamaño
		/// Este método está vacío porque DialogSystem no debe modificar tamaños
		/// </summary>
		private void UpdateAllCharactersSizeAndPosition()
		{
			// CRÍTICO: NO modificar tamaños aquí
			// Solo Character puede modificar su propio tamaño
			GD.Print("[DialogSystem] UpdateAllCharactersSizeAndPosition llamado (sin modificar tamaños - solo Character puede hacerlo)");
		}
		
		/// <summary>
		/// Actualiza el tamaño de un personaje basado en el viewport actual
		/// IMPORTANTE: NO modifica el tamaño - solo Character puede modificar su tamaño
		/// Este método está vacío porque DialogSystem no debe modificar tamaños
		/// </summary>
		private void UpdateCharacterSize(string characterId, IEmotionalCharacter character)
		{
			// CRÍTICO: NO modificar el tamaño aquí
			// Solo Character puede modificar su propio tamaño
			// El DialogSystem solo registra personajes, no modifica sus propiedades
			GD.Print($"[DialogSystem] UpdateCharacterSize llamado para {characterId} (sin modificar tamaño - solo Character puede hacerlo)");
		}
		

		/// <summary>
		/// Configura el DialogSystem con tamaños fijos que nunca cambian
		/// </summary>
		private void ConfigureFixedSize()
		{
			// Obtener o crear el Control hijo para configurar
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl == null)
			{
				// Crear DialogControl si no existe
				dialogControl = new Control();
				dialogControl.Name = "DialogControl";
				AddChild(dialogControl);
				GD.Print("DialogControl creado dinámicamente");
			}

			// Configurar anclas para cubrir toda la pantalla
			dialogControl.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);

			// Usar CallDeferred para establecer el tamaño después de _ready()
			CallDeferred(nameof(SetFixedSize));

			// Inicialmente oculto hasta que haya un diálogo activo
			dialogControl.Visible = false;
		}

		/// <summary>
		/// Establece el tamaño fijo del DialogSystem de forma diferida
		/// </summary>
		private void SetFixedSize()
		{
			// Obtener el Control hijo
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl != null)
			{
				// Establecer tamaño fijo basado en la resolución del viewport
				var viewportSize = GetViewport().GetVisibleRect().Size;
				dialogControl.CustomMinimumSize = viewportSize;
				dialogControl.Size = viewportSize;
			}
		}

		/// <summary>
		/// Configura el sistema de diálogos
		/// </summary>
		private void SetupDialogSystem()
		{
			CreateDialogBox();
			SetupConnections();

		}

		/// <summary>
		/// Crea el DialogBox principal con configuración fija
		/// </summary>
		private void CreateDialogBox()
		{
			_dialogBox = new DialogBox();
			_dialogBox.Name = "DialogBox";
			_dialogBox.DialogFinished += OnDialogFinished;
			_dialogBox.ContinuePressed += OnContinuePressed;

			// Configurar el DialogBox con tamaños fijos
			_dialogBox.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
			_dialogBox.ZIndex = 1000; // ZIndex alto para estar encima de todo (preguntas, UI, etc.)
			_dialogBox.ZAsRelative = false;
			_dialogBox.Visible = true; // Siempre visible cuando se crea

			// Agregar al Control hijo del CanvasLayer
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl == null)
			{
				// Crear DialogControl si no existe
				dialogControl = new Control();
				dialogControl.Name = "DialogControl";
				dialogControl.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
				AddChild(dialogControl);
			}
			dialogControl.AddChild(_dialogBox);

			// Usar CallDeferred para establecer el tamaño después de que se agregue al árbol
			CallDeferred(nameof(SetDialogBoxSize));

			GD.Print("DialogBox creado con configuración diferida");
		}

		/// <summary>
		/// Establece el tamaño del DialogBox de forma diferida
		/// </summary>
		private void SetDialogBoxSize()
		{
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				var viewportSize = GetViewport().GetVisibleRect().Size;
				_dialogBox.CustomMinimumSize = viewportSize;
				_dialogBox.Size = viewportSize;

				GD.Print($"DialogBox tamaño establecido: {_dialogBox.Size}");
			}
		}

		/// <summary>
		/// Configura las conexiones de señales
		/// </summary>
		private void SetupConnections()
		{
			// El DialogSystem se mantiene visible para funcionar correctamente
			// Solo se oculta cuando no hay diálogo activo
		}

		/// <summary>
		/// Verifica si un personaje ya está registrado
		/// </summary>
		/// <param name="characterId">ID del personaje</param>
		/// <returns>True si está registrado</returns>
		public bool IsCharacterRegistered(string characterId)
		{
			return _characters.ContainsKey(characterId);
		}

		/// <summary>
		/// Obtiene un personaje registrado por su ID
		/// </summary>
		/// <param name="characterId">ID del personaje</param>
		/// <returns>El personaje si está registrado, null si no</returns>
		public IEmotionalCharacter GetCharacter(string characterId)
		{
			if (_characters.ContainsKey(characterId))
			{
				var character = _characters[characterId];
				// Verificar que el personaje sea válido
				if (character != null && character is GodotObject godotObject && GodotObject.IsInstanceValid(godotObject))
				{
					return character;
			}
			else
			{
					// Si el personaje ya no es válido, desregistrarlo
					_characters.Remove(characterId);
					GD.Print($"[DialogSystem] Personaje {characterId} ya no es válido, desregistrado");
					return null;
				}
			}
			return null;
		}

		/// <summary>
		/// Registra un personaje emocional en el sistema
		/// IMPORTANTE: NO modifica el tamaño del personaje - solo Character puede modificar su tamaño
		/// </summary>
		/// <param name="character">Personaje que implementa IEmotionalCharacter</param>
		public void RegisterCharacter(Character character)
		{
			if (character == null) return;

			// CRÍTICO: No registrar duplicados
			if (_characters.ContainsKey(character.CharacterId))
			{
				GD.Print($"[DialogSystem] Personaje {character.CharacterId} ya está registrado, ignorando registro duplicado");
				return;
			}

			_characters[character.CharacterId] = character;

			// CRÍTICO: NO modificar el tamaño del personaje aquí
			// Solo Character puede modificar su propio tamaño
			// El DialogSystem solo registra, no modifica
			var mainScene = GetTree().CurrentScene;
			mainScene?.AddChild(character);
			GD.Print($"[DialogSystem] Personaje registrado: {character.CharacterId} (sin modificar tamaño)");
		}
		/// <summary>
		/// Desregistra un personaje del sistema
		/// </summary>
		/// <param name="characterId">ID del personaje a desregistrar</param>
		public void UnregisterCharacter(string characterId)
		{
			if (_characters.ContainsKey(characterId))
			{
				_characters.Remove(characterId);
				GD.Print("Personaje desregistrado: " + characterId);
			}
		}

		/// <summary>
		/// Inicia un diálogo complejo con múltiples entradas
		/// IMPORTANTE: Divide automáticamente el texto en múltiples diálogos si excede 3 líneas
		/// </summary>
		/// <param name="dialogEntries">Lista de entradas del diálogo</param>
		public void StartDialog(List<DialogEntry> dialogEntries)
		{
			if (dialogEntries == null || dialogEntries.Count == 0) return;

			// IMPORTANTE: Procesar y dividir diálogos largos automáticamente
			// Esto asegura que ningún diálogo exceda 3 líneas
			var processedDialogs = ProcessAndSplitDialogs(dialogEntries);

			_currentDialog = processedDialogs;
			_currentDialogIndex = 0;
			_isDialogActive = true;
			_branchDialogs = null; // Limpiar ramas anteriores
			_originalDialog = null; // Limpiar diálogo original
			_originalDialogIndex = 0; // Limpiar índice original
			_pendingOptions = null; // Limpiar opciones pendientes

			// Hacer visible el DialogSystem solo cuando hay diálogo activo
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl != null)
			{
				dialogControl.Visible = true;
			}

			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.Visible = true;
				// CRÍTICO: Asegurar que el panel del diálogo esté visible al iniciar un nuevo diálogo
				// Esto es necesario porque puede estar oculto después de mostrar opciones
				_dialogBox.EnsureDialogPanelVisible();
			}

			// Emitir señal de que el diálogo comenzó
			EmitSignal(SignalName.DialogStarted);

			ShowCurrentDialogEntry();
		}

		/// <summary>
		/// Procesa y divide diálogos largos en múltiples diálogos más cortos
		/// Cada diálogo tendrá un máximo de 3 líneas, similar a Pokémon
		/// </summary>
		/// <param name="dialogEntries">Lista de entradas de diálogo originales</param>
		/// <returns>Lista de entradas de diálogo procesadas y divididas</returns>
		private List<DialogEntry> ProcessAndSplitDialogs(List<DialogEntry> dialogEntries)
		{
			var processedDialogs = new List<DialogEntry>();

			foreach (var entry in dialogEntries)
			{
				// IMPORTANTE: Si el diálogo tiene opciones, no dividirlo (las opciones deben estar en un solo diálogo)
				if (entry.Options != null && entry.Options.Count > 0)
				{
					processedDialogs.Add(entry);
					continue;
				}

				// IMPORTANTE: Dividir el texto si es demasiado largo
				var splitDialogs = SplitDialogText(entry);
				processedDialogs.AddRange(splitDialogs);
			}

			GD.Print($"[DialogSystem] Diálogos procesados: {dialogEntries.Count} originales -> {processedDialogs.Count} procesados");
			return processedDialogs;
		}

		/// <summary>
		/// Divide un diálogo largo en múltiples diálogos más cortos
		/// Cada diálogo tendrá un máximo de 3 líneas
		/// </summary>
		/// <param name="entry">Entrada de diálogo original</param>
		/// <returns>Lista de entradas de diálogo divididas</returns>
		private List<DialogEntry> SplitDialogText(DialogEntry entry)
		{
			var splitDialogs = new List<DialogEntry>();

			// IMPORTANTE: Calcular el ancho disponible y el número máximo de caracteres por línea
			var viewport = GetViewport();
			var viewportSize = viewport?.GetVisibleRect().Size ?? new Vector2(2560, 1440); // Fallback si no hay viewport
			float availableWidth = viewportSize.X - 120 - 30; // Ancho del viewport menos márgenes
			float fontSize = FontManager.GetScaledSize(FontManager.TextType.Large) * 1.5f;

			// IMPORTANTE: Calcular caracteres por línea de forma más precisa y generosa
			// Usar un promedio de 0.35 * fontSize por carácter (más realista para fuentes proporcionales)
			// Esto permite que más texto quepa en cada línea sin cortarse
			int charsPerLine = (int)(availableWidth / (fontSize * 0.35f));

			// IMPORTANTE: Asegurar que haya al menos un mínimo razonable de caracteres por línea
			if (charsPerLine < 50) charsPerLine = 50; // Mínimo más generoso

			// IMPORTANTE: Máximo 3 líneas por diálogo, pero con un límite mínimo más generoso
			// Permitir hasta 250 caracteres por diálogo para textos largos normales
			int maxCharsPerDialog = Mathf.Max(charsPerLine * 3, 250);

			// IMPORTANTE: Si el texto es corto o moderado, no dividirlo
			// Solo dividir si realmente excede el límite de manera significativa
			if (entry.Text.Length <= maxCharsPerDialog)
			{
				splitDialogs.Add(entry);
				return splitDialogs;
			}

			// IMPORTANTE: Dividir el texto preservando BBCode y asegurando que no se corte
			var textParts = SplitTextWithBBCode(entry.Text, maxCharsPerDialog, charsPerLine);

			// IMPORTANTE: Crear un diálogo para cada parte
			for (int i = 0; i < textParts.Count; i++)
			{
				var dialogEntry = new DialogEntry(
					textParts[i],
					entry.CharacterId,
					entry.Emotion,
					entry.Position,
					null // Las opciones solo van en el último diálogo si es necesario
				);
				splitDialogs.Add(dialogEntry);
			}

			GD.Print($"[DialogSystem] Diálogo dividido en {splitDialogs.Count} partes (max {maxCharsPerDialog} caracteres por diálogo, {charsPerLine} por línea)");
			return splitDialogs;
		}

		/// <summary>
		/// Divide el texto preservando las etiquetas BBCode
		/// IMPORTANTE: NO agrega saltos de línea (\n) - deja que RichTextLabel con AutowrapMode maneje el ajuste automáticamente
		/// Divide el texto en múltiples diálogos basándose solo en la longitud, no en líneas manuales
		/// </summary>
		/// <param name="text">Texto a dividir</param>
		/// <param name="maxCharsPerPart">Número máximo de caracteres por parte (3 líneas estimadas)</param>
		/// <param name="charsPerLine">Número de caracteres por línea (no se usa para agregar \n, solo para estimar)</param>
		/// <returns>Lista de partes del texto dividido</returns>
		private List<string> SplitTextWithBBCode(string text, int maxCharsPerPart, int charsPerLine)
		{
			var parts = new List<string>();

			if (string.IsNullOrEmpty(text))
			{
				return parts;
			}

			// IMPORTANTE: Si el texto es corto, no dividirlo
			if (text.Length <= maxCharsPerPart)
			{
				parts.Add(text);
				return parts;
			}

			// IMPORTANTE: Dividir el texto en palabras preservando BBCode
			// NO agregar \n manualmente - dejar que RichTextLabel con AutowrapMode maneje el ajuste
			var currentPart = new StringBuilder();
			var currentLength = 0; // Longitud total incluyendo BBCode
			var openTags = new Stack<string>(); // Pila para etiquetas BBCode abiertas
			var currentWord = new StringBuilder(); // Palabra actual

			int i = 0;
			while (i < text.Length)
			{
				// IMPORTANTE: Detectar etiquetas BBCode
				if (text[i] == '[' && i + 1 < text.Length)
				{
					// Buscar el cierre de la etiqueta
					int tagEnd = text.IndexOf(']', i);
					if (tagEnd > 0)
					{
						string tag = text.Substring(i, tagEnd - i + 1);

						// IMPORTANTE: Manejar etiquetas de apertura y cierre
						if (tag.StartsWith("[/"))
						{
							// Etiqueta de cierre
							if (openTags.Count > 0)
							{
								openTags.Pop();
							}
							currentWord.Append(tag);
							i = tagEnd + 1;
							continue;
						}
						else if (tag.StartsWith("[") && !tag.StartsWith("[/"))
						{
							// Etiqueta de apertura
							openTags.Push(tag);
							currentWord.Append(tag);
							i = tagEnd + 1;
							continue;
						}
					}
				}

				// IMPORTANTE: Agregar el carácter actual a la palabra
				currentWord.Append(text[i]);

				// IMPORTANTE: Si encontramos un espacio o fin de texto, procesar la palabra
				if (char.IsWhiteSpace(text[i]) || i == text.Length - 1)
				{
					string word = currentWord.ToString();

					// IMPORTANTE: Verificar si agregar esta palabra excedería el límite de caracteres
					// Si es así, crear una nueva parte (sin agregar \n)
					if (currentLength > 0 && currentLength + word.Length > maxCharsPerPart)
					{
						// IMPORTANTE: Guardar las etiquetas abiertas antes de cerrarlas
						var tagsToReopen = new List<string>(openTags);

						// IMPORTANTE: Cerrar todas las etiquetas BBCode abiertas
						while (openTags.Count > 0)
						{
							string openTag = openTags.Pop();
							string tagName = ExtractTagName(openTag);
							if (!string.IsNullOrEmpty(tagName))
							{
								currentPart.Append($"[/{tagName}]");
							}
						}

						parts.Add(currentPart.ToString().Trim());
						currentPart.Clear();
						currentLength = 0;

						// IMPORTANTE: Reabrir las etiquetas en la siguiente parte (en orden inverso)
						for (int j = tagsToReopen.Count - 1; j >= 0; j--)
						{
							var tag = tagsToReopen[j];
							openTags.Push(tag);
							currentPart.Append(tag);
						}
					}

					// IMPORTANTE: Agregar la palabra a la parte actual (sin \n)
					currentPart.Append(word);
					currentLength += word.Length; // Longitud total incluyendo BBCode
					currentWord.Clear();
				}

				i++;
			}

			// IMPORTANTE: Agregar la última parte si hay contenido
			if (currentPart.Length > 0)
			{
				parts.Add(currentPart.ToString().Trim());
			}

			return parts;
		}

		/// <summary>
		/// Calcula la longitud del texto sin contar las etiquetas BBCode
		/// </summary>
		/// <param name="text">Texto con BBCode</param>
		/// <returns>Longitud del texto sin BBCode</returns>
		private int GetTextLengthWithoutBBCode(string text)
		{
			if (string.IsNullOrEmpty(text))
			{
				return 0;
			}

			int length = 0;
			int i = 0;

			while (i < text.Length)
			{
				// IMPORTANTE: Detectar etiquetas BBCode
				if (text[i] == '[' && i + 1 < text.Length)
				{
					int tagEnd = text.IndexOf(']', i);
					if (tagEnd > 0)
					{
						// IMPORTANTE: Saltar la etiqueta BBCode completa
						i = tagEnd + 1;
						continue;
					}
				}

				// IMPORTANTE: Contar el carácter actual
				length++;
				i++;
			}

			return length;
		}

		/// <summary>
		/// Extrae el nombre de una etiqueta BBCode
		/// </summary>
		/// <param name="tag">Etiqueta BBCode (ej: [color=yellow])</param>
		/// <returns>Nombre de la etiqueta (ej: color)</returns>
		private string ExtractTagName(string tag)
		{
			if (string.IsNullOrEmpty(tag) || !tag.StartsWith("[") || !tag.EndsWith("]"))
			{
				return string.Empty;
			}

			// IMPORTANTE: Extraer el nombre de la etiqueta
			string tagContent = tag.Substring(1, tag.Length - 2); // Remover [ y ]
			int equalsIndex = tagContent.IndexOf('=');
			if (equalsIndex > 0)
			{
				return tagContent.Substring(0, equalsIndex);
			}

			return tagContent;
		}

		/// <summary>
		/// Muestra la entrada actual del diálogo
		/// </summary>
		private void ShowCurrentDialogEntry()
		{
			if (_currentDialogIndex >= _currentDialog.Count)
			{
				EndDialog();
				return;
			}

			var currentEntry = _currentDialog[_currentDialogIndex];
			string newSpeakingCharacterId = currentEntry.CharacterId;

			// IMPORTANTE: Restaurar tamaño del personaje anterior si cambió el que habla
			if (!string.IsNullOrEmpty(_currentSpeakingCharacterId) &&
				_currentSpeakingCharacterId != newSpeakingCharacterId &&
				_characters.ContainsKey(_currentSpeakingCharacterId))
			{
				var previousCharacter = _characters[_currentSpeakingCharacterId];
				if (previousCharacter != null && GodotObject.IsInstanceValid(previousCharacter as GodotObject))
				{
					// Restaurar tamaño usando método público de Character
					if (previousCharacter is Character characterObj)
					{
						characterObj.RestoreBaseScale();
					}
					previousCharacter.StopSpeakingAnimation();
				}
			}

			// IMPORTANTE: Activar animación de hablar y aumentar tamaño si hay un personaje hablando
			// Usar la interfaz IEmotionalCharacter para que funcione con todos los personajes
			if (!string.IsNullOrEmpty(newSpeakingCharacterId) && _characters.ContainsKey(newSpeakingCharacterId))
			{
				var character = _characters[newSpeakingCharacterId];

				// IMPORTANTE: Usar la interfaz en lugar de verificar tipos específicos
				// Esto permite que cualquier personaje que implemente IEmotionalCharacter pueda hacer animaciones
				if (character != null && GodotObject.IsInstanceValid(character as GodotObject))
				{
					// CRÍTICO: Mostrar el personaje si está oculto (para diálogos consecutivos y cambios de escena)
					// Esto asegura que personajes que fueron ocultados se muestren automáticamente cuando hablan
					if (character is Character characterObj)
					{
						// Mostrar el personaje si está oculto (verificar sprite y alpha)
						if (!characterObj.IsCharacterVisible())
						{
							characterObj.ShowCharacter(0.3f);
							GD.Print($"✅ Personaje {newSpeakingCharacterId} mostrado automáticamente al hablar (estaba oculto - alpha: {characterObj.Modulate.A})");
						}
						characterObj.EnlargeScale();
					}
					character.PlaySpeakingAnimation();
					_currentSpeakingCharacterId = newSpeakingCharacterId;
					GD.Print($"✅ Personaje {newSpeakingCharacterId} agrandado y animación de hablar activada");
				}
			}
			else
			{
				// Si no hay personaje hablando, limpiar el anterior
				_currentSpeakingCharacterId = null;
			}

			// Cambiar emoción del personaje si está especificada
			if (!string.IsNullOrEmpty(currentEntry.CharacterId) && currentEntry.Emotion.HasValue)
			{
				ChangeCharacterEmotion(currentEntry.CharacterId, currentEntry.Emotion.Value);
			}

			// IMPORTANTE: El posicionamiento de personajes ahora se maneja directamente en Character.cs
			// DialogSystem ya no controla las posiciones de los personajes

			// IMPORTANTE: Ejecutar callback OnShow si existe (antes de mostrar el texto)
			if (currentEntry.OnShow != null)
			{
				currentEntry.OnShow();
			}

			// Asegurar visibilidad (los tamaños ya están fijos)
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl != null)
			{
				dialogControl.Visible = true;
			}

			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.Visible = true;
				// CRÍTICO: Asegurar que el panel del diálogo esté visible después de ocultar opciones
				// Esto es necesario porque cuando se muestran opciones, el panel se oculta
				_dialogBox.EnsureDialogPanelVisible();
			}

			// CRÍTICO: Obtener el nombre del personaje si existe
			// REGLA: Cuando un personaje habla (tiene CharacterId), SIEMPRE se muestra su nombre
			// REGLA: Cuando el protagonista habla (IsProtagonistSpeech), se muestra "Tu"
			string characterName = null;
			if (!string.IsNullOrEmpty(currentEntry.CharacterId) && _characters.ContainsKey(currentEntry.CharacterId))
			{
				var character = _characters[currentEntry.CharacterId];
				// Verificar si el personaje es un Character (que tiene CharacterName)
				if (character is Character characterWithName)
				{
					characterName = characterWithName.CharacterName;
				}
			}
			else if (currentEntry.IsProtagonistSpeech)
			{
				// REGLA: Si el protagonista habla (no es pensamiento), mostrar el nombre personalizado o "Tu" por defecto
				characterName = !string.IsNullOrEmpty(currentEntry.ProtagonistName) ? currentEntry.ProtagonistName : "Tu";
			}

			// REGLA: Si es un pensamiento interno, envolver el texto en itálica usando BBCode
			string displayText = currentEntry.Text;
			if (currentEntry.IsInternalThought)
			{
				displayText = $"[i]{currentEntry.Text}[/i]";
			}

			// Mostrar el texto del diálogo con el nombre del personaje
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.ShowDialog(displayText, characterName);

				// CRÍTICO: Verificar si es una decisión de verdad/mentira
				if (currentEntry.IsTruthLieDecision)
				{
					// Guardar referencia a la entrada para mostrar opciones después
					_pendingTruthLieEntry = currentEntry;
					// Esperar a que termine de escribir y luego mostrar opciones de verdad/mentira
					GetTree().CreateTimer(0.1f).Timeout += CheckAndShowTruthLieOptions;
				}
				// CRÍTICO: Verificar si es una decisión en tiempo
				else if (currentEntry.IsTimedDecision && currentEntry.Options != null && currentEntry.Options.Count > 0)
				{
					// Guardar opciones pendientes y esperar a que termine de escribir
					_pendingOptions = currentEntry.Options;
					_pendingTimedEntry = currentEntry;
					// Usar CreateTimer para esperar a que termine de escribir
					GetTree().CreateTimer(0.1f).Timeout += CheckAndShowTimedOptions;
				}
				// IMPORTANTE: Si hay opciones normales, mostrarlas cuando termine de escribir
				else if (currentEntry.Options != null && currentEntry.Options.Count > 0)
				{
					GD.Print($"[DialogSystem] ✅ Opciones detectadas: {currentEntry.Options.Count} opciones");
					// Guardar opciones pendientes y esperar a que termine de escribir
					_pendingOptions = currentEntry.Options;
					// Usar CreateTimer para esperar a que termine de escribir
					GetTree().CreateTimer(0.1f).Timeout += CheckAndShowOptions;
				}
				else
				{
					GD.Print($"[DialogSystem] ⚠️ No hay opciones: Options={currentEntry.Options?.Count ?? 0}, IsTruthLie={currentEntry.IsTruthLieDecision}, IsTimed={currentEntry.IsTimedDecision}");
				}
			}

			GD.Print($"Mostrando entrada {_currentDialogIndex + 1}/{_currentDialog.Count} del diálogo");
		}

		/// <summary>
		/// Ejecuta el callback OnEnd del diálogo actual si existe
		/// Se llama cuando el diálogo termina (después de que termine de escribir o cuando se avance)
		/// </summary>
		private void ExecuteDialogEndCallback()
		{
			if (_currentDialogIndex < _currentDialog.Count)
			{
				var currentEntry = _currentDialog[_currentDialogIndex];
				if (currentEntry.OnEnd != null)
				{
					currentEntry.OnEnd();
				}
			}
		}

		/// <summary>
		/// Verifica si el texto terminó de escribirse y muestra las opciones
		/// </summary>
		private void CheckAndShowOptions()
		{
			GD.Print($"[DialogSystem] CheckAndShowOptions llamado - _pendingOptions: {(_pendingOptions != null ? _pendingOptions.Count.ToString() : "null")}");
			
			if (_pendingOptions == null || _pendingOptions.Count == 0)
			{
				GD.Print("[DialogSystem] ⚠️ No hay opciones pendientes para mostrar");
				return;
			}

			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				// Asegurar que el texto haya terminado de escribirse
				if (_dialogBox.IsTyping())
				{
					GD.Print("[DialogSystem] ⏳ Texto aún escribiéndose, esperando...");
					// Esperar un poco más
					GetTree().CreateTimer(0.1f).Timeout += CheckAndShowOptions;
					return;
				}

				// Mostrar opciones
				var options = _pendingOptions;
				_pendingOptions = null; // Limpiar pendientes

				// CRÍTICO: Guardar las opciones mostradas para validación
				_currentShownOptions = new List<DialogOption>(options);

				GD.Print($"[DialogSystem] ✅ Mostrando {options.Count} opciones en DialogBox");
				_dialogBox.ShowOptions(options, OnOptionSelected);
			}
			else
			{
				GD.PrintErr("[DialogSystem] ❌ DialogBox no disponible o inválido");
			}
		}
		
		/// <summary>
		/// Verifica si el texto terminó de escribirse y muestra las opciones de verdad/mentira
		/// </summary>
		private void CheckAndShowTruthLieOptions()
		{
			if (_pendingTruthLieEntry == null)
			{
				return;
			}

			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				// Asegurar que el texto haya terminado de escribirse
				if (_dialogBox.IsTyping())
				{
					// Esperar un poco más
					GetTree().CreateTimer(0.1f).Timeout += CheckAndShowTruthLieOptions;
					return;
				}

				// Mostrar opciones de verdad/mentira
				var entry = _pendingTruthLieEntry;
				_pendingTruthLieEntry = null; // Limpiar pendiente
				ShowTruthLieOptions(entry);
			}
		}
		
		/// <summary>
		/// Verifica si el texto terminó de escribirse y muestra las opciones con tiempo
		/// </summary>
		private void CheckAndShowTimedOptions()
		{
			if (_pendingTimedEntry == null)
			{
				return;
			}

			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				// Asegurar que el texto haya terminado de escribirse
				if (_dialogBox.IsTyping())
				{
					// Esperar un poco más
					GetTree().CreateTimer(0.1f).Timeout += CheckAndShowTimedOptions;
					return;
				}

				// Mostrar opciones con tiempo
				var entry = _pendingTimedEntry;
				_pendingTimedEntry = null; // Limpiar pendiente
				ShowTimedOptions(entry);
			}
		}

		/// <summary>
		/// Se llama cuando el jugador selecciona una opción
		/// CRÍTICO: Valida contra las opciones mostradas, no contra las del diálogo actual
		/// </summary>
		private void OnOptionSelected(int optionIndex)
		{
			// CRÍTICO: Evitar múltiples selecciones simultáneas
			if (_isProcessingOption)
			{
				GD.Print("[DialogSystem] Ya se está procesando una selección de opción, ignorando...");
				return;
			}

			// CRÍTICO: Validar contra las opciones mostradas, no contra las del diálogo
			// Esto evita el error "Índice de opción inválido" cuando las opciones cambian
			if (_currentShownOptions == null || _currentShownOptions.Count == 0)
			{
				GD.PrintErr("No hay opciones mostradas para seleccionar");
				return;
			}

			if (optionIndex < 0 || optionIndex >= _currentShownOptions.Count)
			{
				GD.PrintErr($"Índice de opción inválido: {optionIndex} (hay {_currentShownOptions.Count} opciones)");
				return;
			}

			// Marcar que se está procesando una selección
			_isProcessingOption = true;

			var selectedOption = _currentShownOptions[optionIndex];

			GD.Print($"✅ Opción seleccionada: {selectedOption.Text}");

			// CRÍTICO: Detener el timer ANTES de ejecutar cualquier callback
			// Esto previene que el timer expire y ejecute la opción por defecto
			if (_isTimedDecisionActive || _timedDecisionTimer != null)
			{
				if (_timedDecisionTimer != null)
			{
				_timedDecisionTimer.Stop();
					GD.Print("[DialogSystem] ✅ Timer de decisión detenido");
				}
				_isTimedDecisionActive = false;
				_currentTimedDecisionEntry = null;
				
				// También ocultar la UI del temporizador visual
				if (_dialogBox != null && IsInstanceValid(_dialogBox))
				{
					_dialogBox.HideTimedDecisionUI();
				}
				
				GD.Print("[DialogSystem] ✅ Timer de decisión en tiempo detenido y limpiado");
			}

			// Ocultar opciones primero
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.HideOptions();
			}

			// CRÍTICO: Limpiar opciones mostradas después de seleccionar
			_currentShownOptions = null;

			// CRÍTICO: Guardar referencia al diálogo actual antes de ejecutar la acción
			// Esto nos permite verificar si se inició un nuevo diálogo
			var previousDialog = _currentDialog;
			int previousDialogIndex = _currentDialogIndex;

			// Ejecutar la acción de la opción (esto puede iniciar un nuevo diálogo)
			try
			{
				selectedOption.OnSelected?.Invoke();
			}
			catch (System.Exception ex)
			{
				GD.PrintErr($"❌ Error al ejecutar acción de opción: {ex.Message}");
			}
			
			// CRÍTICO: Verificar si se inició un nuevo diálogo
			// Si el diálogo cambió (referencia diferente o índice diferente), NO avanzar automáticamente
			bool newDialogStarted = (_currentDialog != previousDialog || 
				(_currentDialog != null && previousDialog != null && _currentDialogIndex != previousDialogIndex));
			
			if (newDialogStarted)
			{
				GD.Print("[DialogSystem] ✅ Nuevo diálogo iniciado desde la opción, NO avanzando automáticamente");
				// CRÍTICO: Permitir nuevas selecciones después de procesar
				_isProcessingOption = false;
				return; // Salir temprano - el nuevo diálogo ya se está mostrando
			}

			// IMPORTANTE: Si hay diálogos siguientes para esta opción, usarlos
			// Si no, y la opción no inició un nuevo diálogo, continuar con el siguiente diálogo normal
			if (selectedOption.NextDialogs != null && selectedOption.NextDialogs.Count > 0)
			{
				// Guardar el diálogo original y el índice para poder volver después
				_originalDialog = _currentDialog;
				_originalDialogIndex = _currentDialogIndex;

				// Guardar los diálogos ramificados
				_branchDialogs = selectedOption.NextDialogs;

				// Reemplazar temporalmente el diálogo actual con los ramificados
				_currentDialog = _branchDialogs;
				_currentDialogIndex = 0;

				// Mostrar el primer diálogo de la rama
				ShowCurrentDialogEntry();

				GD.Print($"✅ Iniciando rama de diálogo con {_branchDialogs.Count} entradas");
			}
			else if (_isDialogActive)
			{
				// Solo avanzar si el diálogo sigue activo Y estaba activo antes (no se inició uno nuevo)
				// Ejecutar callback OnEnd del diálogo actual antes de avanzar
				ExecuteDialogEndCallback();
				// Continuar con el siguiente diálogo normal
				AdvanceDialog(skipEndCallback: true);
			}

			// CRÍTICO: Permitir nuevas selecciones después de procesar
			_isProcessingOption = false;
		}

		/// <summary>
		/// Cambia la emoción de un personaje específico con transición suave y sin parpadeos
		/// </summary>
		/// <param name="characterId">ID del personaje</param>
		/// <param name="emotion">Nueva emoción</param>
		private void ChangeCharacterEmotion(string characterId, Emotion emotion)
		{
			if (_characters.ContainsKey(characterId))
			{
				var character = _characters[characterId];

				// Verificar si la emoción es diferente a la anterior
				if (_previousEmotions.ContainsKey(characterId) && _previousEmotions[characterId] == emotion)
				{
					GD.Print($"Emoción {emotion} ya es la actual para {characterId}, no se hace transición");
					return;
				}

				// Si el personaje es un Node2D, hacer transición suave de la imagen
				if (character is Node2D node2D && IsInstanceValid(node2D))
				{
					// Crear tween para transición suave de la imagen sin parpadeos
					var tween = CreateTween();

					// Transición suave sin hacer invisible la imagen
					// Primero hacer un ligero fade out (no completamente invisible)
					tween.TweenProperty(node2D, "modulate:a", 0.7f, 0.2f)
						.SetEase(Tween.EaseType.Out)
						.SetTrans(Tween.TransitionType.Cubic);

					// Cambiar emoción en el medio de la transición
					tween.TweenCallback(Callable.From(() =>
					{
						if (character is GodotObject godotObject && IsInstanceValid(godotObject))
						{
							character.ChangeEmotion(emotion);
							EmitSignal(SignalName.CharacterEmotionChange, characterId, (int)emotion);
						}
					}));

					// Luego hacer fade in completo
					tween.TweenProperty(node2D, "modulate:a", 1.0f, 0.2f)
						.SetEase(Tween.EaseType.Out)
						.SetTrans(Tween.TransitionType.Cubic);

					// Guardar la emoción actual
					_previousEmotions[characterId] = emotion;

					GD.Print($"Emoción cambiada para {characterId}: {emotion} (con transición suave sin parpadeos)");
				}
				else
				{
					// Si no es Node2D, cambiar directamente
					character.ChangeEmotion(emotion);
					EmitSignal(SignalName.CharacterEmotionChange, characterId, (int)emotion);
					_previousEmotions[characterId] = emotion;
					GD.Print($"Emoción cambiada para {characterId}: {emotion}");
				}
			}
			else
			{
				GD.PrintErr($"Personaje no encontrado: {characterId}");
			}
		}


		/// <summary>
		/// Avanza al siguiente diálogo
		/// </summary>
		/// <param name="skipEndCallback">Si es true, no ejecuta el callback OnEnd (útil cuando ya se ejecutó antes)</param>
		private void AdvanceDialog(bool skipEndCallback = false)
		{
			// CRÍTICO: Ejecutar callback OnEnd del diálogo actual antes de avanzar (a menos que se indique lo contrario)
			if (!skipEndCallback)
			{
				ExecuteDialogEndCallback();
			}

			_currentDialogIndex++;

			// IMPORTANTE: Si estamos en una rama de diálogos y terminamos, volver al diálogo principal
			if (_branchDialogs != null && _currentDialogIndex >= _currentDialog.Count)
			{
				GD.Print("✅ Rama de diálogo completada, volviendo al diálogo principal");

				// Restaurar el diálogo original
				if (_originalDialog != null)
				{
					_currentDialog = _originalDialog;
					_currentDialogIndex = _originalDialogIndex + 1; // Continuar desde el siguiente diálogo
					_branchDialogs = null;
					_originalDialog = null;
					_originalDialogIndex = 0;

					// Continuar con el siguiente diálogo del diálogo original
					if (_currentDialogIndex < _currentDialog.Count)
					{
						ShowCurrentDialogEntry();
					}
					else
					{
						// Si no hay más diálogos, terminar
						EndDialog();
					}
				}
				else
				{
					// Si no hay diálogo original, terminar
					_branchDialogs = null;
					EndDialog();
				}
				return;
			}

			ShowCurrentDialogEntry();
		}

		/// <summary>
		/// Termina el diálogo actual
		/// </summary>
		private void EndDialog()
		{
			_isDialogActive = false;

			// IMPORTANTE: Restaurar tamaño y detener animación de hablar de todos los personajes cuando termine el diálogo
			// CRÍTICO: NO ocultar automáticamente los personajes - cada escena debe manejar la visibilidad explícitamente
			// Esto evita que personajes se oculten cuando se necesitan en diálogos consecutivos
			// Verificar que el personaje sea válido antes de usarlo para evitar errores de objeto eliminado
			foreach (var characterPair in _characters.ToList()) // Crear copia para evitar modificación durante iteración
			{
				var character = characterPair.Value;

				// Verificar que el personaje sea válido antes de usarlo
				if (character == null)
				{
					// El personaje fue eliminado, desregistrarlo
					_characters.Remove(characterPair.Key);
					GD.Print($"⚠️ Personaje {characterPair.Key} es null, desregistrado");
					continue;
				}

				// Verificar si es un GodotObject y si es válido
				if (character is GodotObject godotObject && !GodotObject.IsInstanceValid(godotObject))
				{
					// El personaje fue eliminado, desregistrarlo
					_characters.Remove(characterPair.Key);
					GD.Print($"⚠️ Personaje {characterPair.Key} ya no es válido, desregistrado");
					continue;
				}

				// IMPORTANTE: Usar la interfaz IEmotionalCharacter para detener animaciones y restaurar tamaño
				// Esto permite que cualquier personaje que implemente IEmotionalCharacter pueda detener animaciones
				if (character != null && GodotObject.IsInstanceValid(character as GodotObject))
				{
					try
					{
						character.StopSpeakingAnimation();
						// Restaurar tamaño usando método público de Character
						if (character is Character characterObj)
						{
							characterObj.RestoreBaseScale();
							// CRÍTICO: NO ocultar automáticamente - cada escena maneja la visibilidad
							// characterObj.Hide(0.5f); // REMOVIDO - cada escena debe ocultar explícitamente
							GD.Print($"✅ Animación de hablar detenida y tamaño restaurado para {characterPair.Key} (visibilidad manejada por escena)");
						}
						GD.Print($"✅ Animación de hablar detenida y tamaño restaurado para {characterPair.Key}");
					}
					catch (System.Exception ex)
					{
						GD.PrintErr($"❌ Error al detener animación de hablar para {characterPair.Key}: {ex.Message}");
						// Desregistrar el personaje si hay error
						_characters.Remove(characterPair.Key);
					}
				}
			}

			// Limpiar personaje que habla actualmente
			_currentSpeakingCharacterId = null;

			// Verificar que el DialogBox esté disponible antes de usarlo
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				// ⚠️ CRÍTICO: Ocultar opciones ANTES de ocultar el diálogo
				// Esto asegura que el panel de opciones siempre quede invisible cuando se finaliza un diálogo
				_dialogBox.HideOptions();
				_dialogBox.HideDialog();
			}

			// Limpiar opciones pendientes y mostradas si las hay
			_pendingOptions = null;
			_currentShownOptions = null;

			// Ocultar el Control hijo
			var dialogControl = GetNodeOrNull<Control>("DialogControl");
			if (dialogControl != null)
			{
				dialogControl.Visible = false;
			}

			EmitSignal(SignalName.DialogFinished);
		}

		/// <summary>
		/// Maneja el evento cuando el diálogo termina de escribir
		/// </summary>
		private void OnDialogFinished()
		{
			// IMPORTANTE: NO ejecutar OnEnd aquí - debe ejecutarse solo cuando el usuario hace click
			// y es el último diálogo. El OnEnd se ejecutará en OnPressNextButton cuando sea apropiado.
			
			GD.Print("DialogBox terminó de escribir, esperando input del usuario");
		}

		/// <summary>
		/// Maneja el evento cuando se presiona continuar
		/// IMPORTANTE: DialogBox solo emite ContinuePressed cuando el texto terminó de escribirse completamente
		/// Si el texto aún se está escribiendo, DialogBox llama a SkipTyping() internamente sin emitir señal
		/// </summary>
		private void OnContinuePressed()
		{
			if (_isDialogActive)
			{
				// CRÍTICO: OnContinuePressed solo se ejecuta cuando el texto ya terminó de escribirse
				// (DialogBox solo emite la señal cuando _isTyping es false)
				// Por lo tanto, podemos llamar directamente a OnPressNextButton
				OnPressNextButton();
			}
		}

		/// <summary>
		/// Se ejecuta cuando el texto ya está completo y el usuario presiona para avanzar al siguiente texto
		/// Este método se ejecuta SOLO cuando el texto terminó de escribirse completamente
		/// Llama al callback OnPressNextButton del DialogEntry activo si existe
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// </summary>
		private void OnPressNextButton()
		{
			if (!_isDialogActive)
			{
				return;
			}
			
			// CRÍTICO: Verificar que el texto realmente terminó de escribirse (doble verificación)
			if (_dialogBox != null && IsInstanceValid(_dialogBox) && _dialogBox.IsTyping())
			{
				// Si aún está escribiendo, no ejecutar este método
				// Esto no debería pasar porque DialogBox solo emite ContinuePressed cuando terminó
				GD.Print("[DialogSystem] ⚠️ OnPressNextButton llamado pero el texto aún se está escribiendo, ignorando...");
				return;
			}
			
			// CRÍTICO: Verificar si el diálogo actual tiene opciones que deben mostrarse
			if (_currentDialogIndex < _currentDialog.Count)
			{
				var currentEntry = _currentDialog[_currentDialogIndex];
				
				// Verificar si hay opciones de verdad/mentira pendientes
				if (currentEntry.IsTruthLieDecision)
				{
					if (_pendingTruthLieEntry != null)
					{
						GD.Print("[DialogSystem] ⚠️ Hay opciones de verdad/mentira pendientes, NO avanzando - forzando verificación");
						// Forzar verificación inmediata de opciones
						CheckAndShowTruthLieOptions();
						return;
					}
					// Si no hay pendiente pero hay opciones visibles, no avanzar
					if (_dialogBox != null && IsInstanceValid(_dialogBox) && _dialogBox.HasOptionsVisible())
					{
						GD.Print("[DialogSystem] ⚠️ Hay opciones de verdad/mentira visibles, NO avanzando");
						return;
					}
				}
				// Verificar si hay opciones normales pendientes
				else if (currentEntry.Options != null && currentEntry.Options.Count > 0)
				{
					if (_pendingOptions != null && _pendingOptions.Count > 0)
					{
						GD.Print("[DialogSystem] ⚠️ Hay opciones pendientes, NO avanzando - forzando verificación");
						// Forzar verificación inmediata de opciones
						CheckAndShowOptions();
						return;
					}
					// Si no hay pendiente pero hay opciones visibles, no avanzar
					if (_dialogBox != null && IsInstanceValid(_dialogBox) && _dialogBox.HasOptionsVisible())
					{
						GD.Print("[DialogSystem] ⚠️ Hay opciones visibles, NO avanzando");
						return;
					}
				}
			}
			
			// CRÍTICO: Verificar si es el último diálogo ANTES de avanzar
			bool isLastDialog = (_currentDialogIndex + 1) >= _currentDialog.Count;
			
			// CRÍTICO: Ejecutar callback OnPressNextButton del DialogEntry activo si existe
			if (_currentDialogIndex < _currentDialog.Count)
			{
				var currentEntry = _currentDialog[_currentDialogIndex];
				if (currentEntry.OnPressNextButton != null)
				{
					try
					{
						currentEntry.OnPressNextButton();
						GD.Print("[DialogSystem] ✅ OnPressNextButton callback ejecutado para el diálogo actual");
					}
					catch (System.Exception ex)
					{
						GD.PrintErr($"[DialogSystem] ❌ Error al ejecutar OnPressNextButton callback: {ex.Message}");
					}
				}
				
				// CRÍTICO: Si es el último diálogo, ejecutar OnEnd ANTES de avanzar
				// El flujo correcto es: último -> texto terminó -> click siguiente -> es último? -> ejecutar End -> avanzar (termina)
				if (isLastDialog && currentEntry.OnEnd != null)
				{
					try
					{
						GD.Print("[DialogSystem] ✅ Último diálogo detectado, ejecutando OnEnd callback");
						currentEntry.OnEnd();
						GD.Print("[DialogSystem] ✅ OnEnd callback ejecutado");
					}
					catch (System.Exception ex)
					{
						GD.PrintErr($"[DialogSystem] ❌ Error al ejecutar OnEnd callback: {ex.Message}");
					}
				}
			}
			
			GD.Print("[DialogSystem] ✅ OnPressNextButton: Texto completo, avanzando al siguiente diálogo");
			
			// Avanzar al siguiente diálogo (si no es el último, o si es el último, terminará el diálogo)
			AdvanceDialog(skipEndCallback: true); // Skip porque ya ejecutamos OnEnd arriba si era necesario
		}

		/// <summary>
		/// Timer para decisiones en tiempo
		/// </summary>
		private Timer _timedDecisionTimer;
		
		/// <summary>
		/// Entrada de diálogo actual con decisión en tiempo (para acceder cuando se agota el tiempo)
		/// </summary>
		private DialogEntry _currentTimedDecisionEntry;
		
		/// <summary>
		/// Indica si hay una decisión en tiempo activa
		/// </summary>
		private bool _isTimedDecisionActive = false;
		
		/// <summary>
		/// Muestra opciones de verdad/mentira
		/// </summary>
		private void ShowTruthLieOptions(DialogEntry entry)
		{
			if (_dialogBox == null || !IsInstanceValid(_dialogBox)) return;
			if (_dialogBox.IsTyping()) return; // Esperar a que termine de escribir
			
			// Crear opciones de verdad/mentira
			var truthOption = new DialogOption(
				entry.TruthOptionText ?? "Decir la Verdad",
				() => {
					// CRÍTICO: Detener el timer ANTES de ejecutar cualquier callback
					// Esto previene que el timer expire y ejecute la opción por defecto
					if (_isTimedDecisionActive || _timedDecisionTimer != null)
					{
						if (_timedDecisionTimer != null)
						{
							_timedDecisionTimer.Stop();
							GD.Print("[DialogSystem] ✅ Timer detenido (verdad seleccionada)");
						}
						_isTimedDecisionActive = false;
						_currentTimedDecisionEntry = null;
						
						// También ocultar la UI del temporizador visual
					if (_dialogBox != null && IsInstanceValid(_dialogBox))
					{
							_dialogBox.HideTimedDecisionUI();
					}
					}
					
					// CRÍTICO: NO ocultar opciones aquí - se ocultarán en OnOptionSelected
					// Esto evita que RestorePreviousMusic se llame dos veces
					_currentShownOptions = null;
					
					// Ejecutar la acción (esto puede iniciar un nuevo diálogo)
					entry.OnTruthSelected?.Invoke();
					GD.Print("[DialogSystem] Verdad seleccionada");
				}
			);
			
			var lieOption = new DialogOption(
				entry.LieOptionText ?? "Mentir",
				() => {
					// CRÍTICO: Detener el timer ANTES de ejecutar cualquier callback
					// Esto previene que el timer expire y ejecute la opción por defecto
					if (_isTimedDecisionActive || _timedDecisionTimer != null)
					{
						if (_timedDecisionTimer != null)
						{
							_timedDecisionTimer.Stop();
							GD.Print("[DialogSystem] ✅ Timer detenido (mentira seleccionada)");
						}
						_isTimedDecisionActive = false;
						_currentTimedDecisionEntry = null;
						
						// También ocultar la UI del temporizador visual
					if (_dialogBox != null && IsInstanceValid(_dialogBox))
					{
							_dialogBox.HideTimedDecisionUI();
					}
					}
					
					// CRÍTICO: NO ocultar opciones aquí - se ocultarán en OnOptionSelected
					// Esto evita que RestorePreviousMusic se llame dos veces
					_currentShownOptions = null;
					
					// Ejecutar la acción (esto puede iniciar un nuevo diálogo)
					entry.OnLieSelected?.Invoke();
					GD.Print("[DialogSystem] Mentira seleccionada");
				}
			);
			
			var truthLieOptions = new List<DialogOption> { truthOption, lieOption };
			
			// MEJORADO: Verificar si también es una decisión en tiempo
			bool isTimed = entry.IsTimedDecision && entry.TimeLimit > 0f;
			float timeLimit = isTimed ? entry.TimeLimit : 0f;
			
			// Ejecutar callback cuando se muestran las opciones
			if (entry.OnShowTruthLieDecision != null)
			{
				try
				{
					entry.OnShowTruthLieDecision.Invoke();
					GD.Print("[DialogSystem] ✅ Callback OnShowTruthLieDecision ejecutado");
				}
				catch (System.Exception ex)
				{
					GD.PrintErr($"[DialogSystem] ❌ Error al ejecutar OnShowTruthLieDecision: {ex.Message}");
				}
			}
			
			// Mostrar opciones con estilo especial (verdad/mentira)
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.ShowOptions(truthLieOptions, OnOptionSelected, isTimed: isTimed, timeLimit: timeLimit, isTruthLie: true);
				_currentShownOptions = truthLieOptions;
			}
			
			// MEJORADO: Si es una decisión en tiempo, configurar el timer pero NO iniciarlo
			// El timer se iniciará cuando termine la animación del texto duplicado
			if (isTimed)
			{
				// Guardar referencia a la entrada para el timer
				_currentTimedDecisionEntry = entry;
				_isTimedDecisionActive = false; // NO activo todavía - se activará cuando termine la animación
				
				// Crear y configurar timer (pero NO iniciarlo todavía)
				if (_timedDecisionTimer == null)
				{
					_timedDecisionTimer = new Timer();
					_timedDecisionTimer.OneShot = true;
					AddChild(_timedDecisionTimer);
					_timedDecisionTimer.Timeout += OnTimedDecisionExpired;
				}
				
				_timedDecisionTimer.WaitTime = entry.TimeLimit;
				// CRÍTICO: NO iniciar el timer aquí - esperar a que termine la animación
				// El timer se iniciará cuando DialogBox emita la señal TimedDecisionReadyToStart
				
				// Conectar a la señal de DialogBox para iniciar el timer cuando termine la animación
				if (_dialogBox != null && IsInstanceValid(_dialogBox))
				{
					// Desconectar primero para evitar múltiples conexiones
					if (_dialogBox.IsConnected(DialogBox.SignalName.TimedDecisionReadyToStart, new Callable(this, MethodName.OnTimedDecisionReadyToStart)))
					{
						_dialogBox.Disconnect(DialogBox.SignalName.TimedDecisionReadyToStart, new Callable(this, MethodName.OnTimedDecisionReadyToStart));
					}
					_dialogBox.Connect(DialogBox.SignalName.TimedDecisionReadyToStart, new Callable(this, MethodName.OnTimedDecisionReadyToStart));
				}
				
				GD.Print($"[DialogSystem] Decisión verdad/mentira en tiempo configurada (esperando animación): {entry.TimeLimit} segundos");
			}
		}
		
		/// <summary>
		/// Muestra opciones con tiempo limitado
		/// </summary>
		private void ShowTimedOptions(DialogEntry entry)
		{
			if (_dialogBox == null || !IsInstanceValid(_dialogBox)) return;
			if (_dialogBox.IsTyping()) return; // Esperar a que termine de escribir
			
			if (entry.Options == null || entry.Options.Count == 0)
			{
				GD.PrintErr("[DialogSystem] No hay opciones para decisión en tiempo");
				return;
			}
			
			// Guardar referencia a la entrada actual
			_currentTimedDecisionEntry = entry;
			_isTimedDecisionActive = true;
			
			// Mostrar opciones con timer
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.ShowOptions(entry.Options, OnOptionSelected, isTimed: true, timeLimit: entry.TimeLimit, isTruthLie: false);
				_currentShownOptions = entry.Options;
			}
			
			// Crear y configurar timer (pero NO iniciarlo todavía)
			// El timer se iniciará cuando termine la animación del texto duplicado
			if (_timedDecisionTimer == null)
			{
				_timedDecisionTimer = new Timer();
				_timedDecisionTimer.OneShot = true;
				AddChild(_timedDecisionTimer);
				_timedDecisionTimer.Timeout += OnTimedDecisionExpired;
			}
			
			_timedDecisionTimer.WaitTime = entry.TimeLimit;
			// CRÍTICO: NO iniciar el timer aquí - esperar a que termine la animación
			// El timer se iniciará cuando DialogBox emita la señal TimedDecisionReadyToStart
			
			// Conectar a la señal de DialogBox para iniciar el timer cuando termine la animación
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				// Desconectar primero para evitar múltiples conexiones
				if (_dialogBox.IsConnected(DialogBox.SignalName.TimedDecisionReadyToStart, new Callable(this, nameof(OnTimedDecisionReadyToStart))))
				{
					_dialogBox.TimedDecisionReadyToStart -= OnTimedDecisionReadyToStart;
				}
				_dialogBox.TimedDecisionReadyToStart += OnTimedDecisionReadyToStart;
			}
			
			GD.Print($"[DialogSystem] Decisión en tiempo configurada (esperando animación): {entry.TimeLimit} segundos");
		}
		
		/// <summary>
		/// Se llama cuando DialogBox notifica que la animación terminó y el temporizador puede iniciarse
		/// </summary>
		private void OnTimedDecisionReadyToStart(float timeLimit)
		{
			if (!_isTimedDecisionActive && _currentTimedDecisionEntry != null)
			{
				GD.Print($"[DialogSystem] ✅ Animación terminada - iniciando temporizador: {timeLimit} segundos");
				_isTimedDecisionActive = true;
				
				if (_timedDecisionTimer != null)
				{
					_timedDecisionTimer.Start();
				}
			}
		}
		
		/// <summary>
		/// Se llama cuando se agota el tiempo de una decisión
		/// </summary>
		private void OnTimedDecisionExpired()
		{
			if (!_isTimedDecisionActive || _currentTimedDecisionEntry == null)
			{
				return;
			}
			
			GD.Print("[DialogSystem] ⏰ Tiempo agotado para decisión en tiempo");
			
			// CRÍTICO: Guardar referencia a la entrada antes de limpiar
			var entry = _currentTimedDecisionEntry;
			
			// CRÍTICO: Ocultar opciones y UI de tiempo inmediatamente
			if (_dialogBox != null && IsInstanceValid(_dialogBox))
			{
				_dialogBox.HideOptions();
				_dialogBox.HideTimedDecisionUI();
			}
			
			// Limpiar opciones mostradas
			_currentShownOptions = null;
			
			// Ejecutar callback OnTimeExpired si existe
			if (entry.OnTimeExpired != null)
			{
				entry.OnTimeExpired.Invoke();
			}
			
			// CRÍTICO: Después del callback, seleccionar la opción por defecto si existe
			int defaultIndex = entry.DefaultOptionIndex;
			if (entry.Options != null && defaultIndex >= 0 && defaultIndex < entry.Options.Count)
			{
				GD.Print($"[DialogSystem] Seleccionando opción por defecto: índice {defaultIndex}");
				
				// CRÍTICO: Usar las opciones de la entrada, no las mostradas (que ya se limpiaron)
				var selectedOption = entry.Options[defaultIndex];
				
				// Ejecutar la acción de la opción directamente
				try
				{
					selectedOption.OnSelected?.Invoke();
				}
				catch (System.Exception ex)
				{
					GD.PrintErr($"❌ Error al ejecutar acción de opción por defecto: {ex.Message}");
				}
				
				// Verificar si hay diálogos siguientes o continuar
				if (selectedOption.NextDialogs != null && selectedOption.NextDialogs.Count > 0)
				{
					_originalDialog = _currentDialog;
					_originalDialogIndex = _currentDialogIndex;
					_branchDialogs = selectedOption.NextDialogs;
					_currentDialog = _branchDialogs;
					_currentDialogIndex = 0;
					ShowCurrentDialogEntry();
				}
				else if (_isDialogActive)
				{
					ExecuteDialogEndCallback();
					AdvanceDialog(skipEndCallback: true);
				}
			}
			else
			{
				GD.PrintErr("[DialogSystem] No se pudo seleccionar opción por defecto: índice inválido");
				// Continuar con el siguiente diálogo
				AdvanceDialog();
			}
			
			// Limpiar estado
			_isTimedDecisionActive = false;
			_currentTimedDecisionEntry = null;
			if (_timedDecisionTimer != null)
			{
				_timedDecisionTimer.Stop();
			}
		}

		/// <summary>
		/// Verifica si hay un diálogo activo
		/// </summary>
		/// <returns>True si hay un diálogo activo</returns>
		public bool IsDialogActive()
		{
			return _isDialogActive;
		}

		/// <summary>
		/// Maneja la entrada de teclado para adelantar el diálogo
		/// </summary>
		/// <param name="event">Evento de entrada</param>
		public override void _Input(InputEvent @event)
		{
			if (!_isDialogActive) return;

			if (@event is InputEventKey keyEvent && keyEvent.Pressed)
			{
				if (keyEvent.Keycode == SkipKey)
				{
					HandleSkipInput();
				}
			}
		}

		/// <summary>
		/// Maneja la lógica de skip del diálogo
		/// Usando las mejores prácticas SOLID, KISS, SRP, DRY
		/// </summary>
		private void HandleSkipInput()
		{
			if (_dialogBox == null || !IsInstanceValid(_dialogBox)) return;

			// CRÍTICO: No permitir saltar si hay opciones visibles y aún no se ha elegido
			if (_dialogBox.HasOptionsVisible())
			{
				GD.Print("[DialogSystem] No se puede saltar: hay opciones visibles esperando selección");
				return;
			}

			// Si está escribiendo, terminar el texto inmediatamente
			if (_dialogBox.IsTyping())
			{
				_dialogBox.SkipTyping();
			}
			else
			{
				// Si ya terminó de escribir, avanzar al siguiente diálogo
				AdvanceDialog();
			}
		}
	}

	/// <summary>
	/// Clase que representa una entrada de diálogo usando el patrón Builder
	/// Permite construir diálogos de forma fluida: new DialogEntry("texto").WithCharacter("id").Start(() => {}).End(() => {})
	/// </summary>
	public class DialogEntry
	{
		/// <summary>
		/// ID del personaje que habla (opcional)
		/// </summary>
		public string CharacterId { get; set; }

		/// <summary>
		/// Texto del diálogo
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Emoción del personaje durante este diálogo (opcional)
		/// </summary>
		public Emotion? Emotion { get; set; }

		/// <summary>
		/// Posición del personaje en pantalla (opcional)
		/// </summary>
		public Vector2? Position { get; set; }

		/// <summary>
		/// Opciones de respuesta para el jugador (opcional)
		/// Si hay opciones, se mostrarán en lugar del botón continuar
		/// </summary>
		public List<DialogOption> Options { get; set; }

		/// <summary>
		/// Acción a ejecutar cuando se muestra este diálogo (opcional)
		/// Se ejecuta justo antes de mostrar el texto
		/// </summary>
		public System.Action OnShow { get; set; }

		/// <summary>
		/// Acción a ejecutar cuando termina este diálogo (opcional)
		/// Se ejecuta cuando el diálogo termina (después de que termine de escribir o cuando se avance)
		/// </summary>
		public System.Action OnEnd { get; set; }
		
		/// <summary>
		/// Acción a ejecutar cuando el texto ya está completo y el usuario presiona para avanzar al siguiente texto (opcional)
		/// Se ejecuta SOLO cuando el texto terminó de escribirse completamente y el usuario presiona el botón continuar
		/// </summary>
		public System.Action OnPressNextButton { get; set; }
		
		/// <summary>
		/// Indica si este diálogo requiere una decisión en tiempo limitado
		/// </summary>
		public bool IsTimedDecision { get; set; }
		
		/// <summary>
		/// Tiempo límite en segundos para la decisión (solo si IsTimedDecision es true)
		/// </summary>
		public float TimeLimit { get; set; }
		
		/// <summary>
		/// Acción a ejecutar cuando se agota el tiempo (opcional)
		/// Si no se especifica, se selecciona la primera opción por defecto
		/// </summary>
		public System.Action OnTimeExpired { get; set; }
		
		/// <summary>
		/// Índice de la opción por defecto si se agota el tiempo (0 = primera opción)
		/// </summary>
		public int DefaultOptionIndex { get; set; } = 0;
		
		/// <summary>
		/// Indica si este diálogo requiere una decisión de verdad/mentira
		/// </summary>
		public bool IsTruthLieDecision { get; set; }
		
		/// <summary>
		/// Texto para la opción de verdad (solo si IsTruthLieDecision es true)
		/// </summary>
		public string TruthOptionText { get; set; }
		
		/// <summary>
		/// Texto para la opción de mentira (solo si IsTruthLieDecision es true)
		/// </summary>
		public string LieOptionText { get; set; }
		
		/// <summary>
		/// Acción a ejecutar cuando se elige la verdad (solo si IsTruthLieDecision es true)
		/// </summary>
		public System.Action OnTruthSelected { get; set; }
		
		/// <summary>
		/// Acción a ejecutar cuando se elige mentir (solo si IsTruthLieDecision es true)
		/// </summary>
		public System.Action OnLieSelected { get; set; }
		
		/// <summary>
		/// Indica si este diálogo es un pensamiento interno del protagonista
		/// Si es true, el texto se mostrará en itálica
		/// </summary>
		public bool IsInternalThought { get; set; }
		
		/// <summary>
		/// Indica si este diálogo es hablado por el protagonista (no es pensamiento)
		/// Si es true, se mostrará el nombre del protagonista (por defecto "Tu")
		/// </summary>
		public bool IsProtagonistSpeech { get; set; }
		
		/// <summary>
		/// Nombre personalizado del protagonista cuando habla (para localización/personalización)
		/// Si es null o vacío, se usa "Tu" por defecto
		/// </summary>
		public string ProtagonistName { get; set; }
		
		/// <summary>
		/// Acción a ejecutar cuando se muestran las opciones de verdad/mentira
		/// Se ejecuta cuando aparecen las opciones en pantalla
		/// </summary>
		public System.Action OnShowTruthLieDecision { get; set; }

		/// <summary>
		/// Crea una nueva entrada de diálogo con el texto mínimo necesario
		/// </summary>
		/// <param name="text">Texto del diálogo</param>
		public DialogEntry(string text)
		{
			Text = text;
			CharacterId = null;
			Emotion = null;
			Position = null;
			Options = null;
			OnShow = null;
			OnEnd = null;
			OnPressNextButton = null;
			IsTimedDecision = false;
			TimeLimit = 0f;
			OnTimeExpired = null;
			DefaultOptionIndex = 0;
			IsTruthLieDecision = false;
			TruthOptionText = null;
			LieOptionText = null;
			OnTruthSelected = null;
			OnLieSelected = null;
			IsInternalThought = false;
			OnShowTruthLieDecision = null;
		}

		/// <summary>
		/// Constructor de compatibilidad para mantener código existente
		/// </summary>
		/// <param name="text">Texto del diálogo</param>
		/// <param name="characterId">ID del personaje (opcional)</param>
		/// <param name="emotion">Emoción del personaje (opcional)</param>
		/// <param name="position">Posición del personaje (opcional)</param>
		/// <param name="options">Opciones de respuesta (opcional)</param>
		/// <param name="onShow">Acción a ejecutar cuando se muestra este diálogo (opcional)</param>
		public DialogEntry(string text, string characterId = null, Emotion? emotion = null, Vector2? position = null, List<DialogOption> options = null, System.Action onShow = null)
		{
			Text = text;
			CharacterId = characterId;
			Emotion = emotion;
			Position = position;
			Options = options;
			OnShow = onShow;
			OnEnd = null;
			OnPressNextButton = null;
			IsTimedDecision = false;
			TimeLimit = 0f;
			OnTimeExpired = null;
			DefaultOptionIndex = 0;
			IsTruthLieDecision = false;
			TruthOptionText = null;
			LieOptionText = null;
			OnTruthSelected = null;
			OnLieSelected = null;
			IsInternalThought = false;
			IsProtagonistSpeech = false;
			ProtagonistName = null;
			OnShowTruthLieDecision = null;
		}

		/// <summary>
		/// Establece el ID del personaje que habla
		/// </summary>
		/// <param name="characterId">ID del personaje</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithCharacter(Character character)
		{
			CharacterId = character.CharacterId;
			return this;
		}

		/// <summary>
		/// Establece la emoción del personaje durante este diálogo
		/// </summary>
		/// <param name="emotion">Emoción del personaje</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithEmotion(Emotion emotion)
		{
			Emotion = emotion;
			return this;
		}

		/// <summary>
		/// Establece la posición del personaje en pantalla
		/// </summary>
		/// <param name="position">Posición del personaje</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithPosition(Vector2 position)
		{
			Position = position;
			return this;
		}

		/// <summary>
		/// Establece las opciones de respuesta para el jugador
		/// </summary>
		/// <param name="options">Lista de opciones de respuesta</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithOptions(List<DialogOption> options)
		{
			Options = options;
			return this;
		}

		/// <summary>
		/// Establece la acción a ejecutar cuando se muestra este diálogo (antes de mostrar el texto)
		/// </summary>
		/// <param name="onStart">Acción a ejecutar cuando se muestra el diálogo</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry Start(System.Action onStart)
		{
			OnShow = onStart;
			return this;
		}

		/// <summary>
		/// Establece la acción a ejecutar cuando termina este diálogo (después de que termine de escribir o cuando se avance)
		/// </summary>
		/// <param name="onEnd">Acción a ejecutar cuando termina el diálogo</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry End(System.Action onEnd)
		{
			OnEnd = onEnd;
			return this;
		}
		
		/// <summary>
		/// Establece la acción a ejecutar cuando el texto ya está completo y el usuario presiona para avanzar al siguiente texto
		/// </summary>
		/// <param name="onPressNextButton">Acción a ejecutar cuando el usuario presiona el botón continuar (solo cuando el texto terminó)</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry OnNextButton(System.Action onPressNextButton)
		{
			OnPressNextButton = onPressNextButton;
			return this;
		}
		
		/// <summary>
		/// Configura este diálogo como una decisión en tiempo limitado
		/// </summary>
		/// <param name="timeLimit">Tiempo límite en segundos</param>
		/// <param name="onTimeExpired">Acción a ejecutar cuando se agota el tiempo (opcional)</param>
		/// <param name="defaultOptionIndex">Índice de la opción por defecto si se agota el tiempo (0 = primera opción)</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithTimedDecision(float timeLimit, System.Action onTimeExpired = null, int defaultOptionIndex = 0)
		{
			IsTimedDecision = true;
			TimeLimit = timeLimit;
			OnTimeExpired = onTimeExpired;
			DefaultOptionIndex = defaultOptionIndex;
			return this;
		}
		
		/// <summary>
		/// Configura este diálogo como una decisión de verdad/mentira
		/// </summary>
		/// <param name="truthText">Texto para la opción de verdad</param>
		/// <param name="lieText">Texto para la opción de mentira</param>
		/// <param name="onTruthSelected">Acción a ejecutar cuando se elige la verdad</param>
		/// <param name="onLieSelected">Acción a ejecutar cuando se elige mentir</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithTruthLieDecision(string truthText, string lieText, System.Action onTruthSelected, System.Action onLieSelected)
		{
			IsTruthLieDecision = true;
			TruthOptionText = truthText;
			LieOptionText = lieText;
			OnTruthSelected = onTruthSelected;
			OnLieSelected = onLieSelected;
			return this;
		}
		
		/// <summary>
		/// Marca este diálogo como un pensamiento interno del protagonista
		/// El texto se mostrará en itálica automáticamente
		/// </summary>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry AsInternalThought()
		{
			IsInternalThought = true;
			return this;
		}
		
		/// <summary>
		/// Marca este diálogo como hablado por el protagonista (no es pensamiento)
		/// Se mostrará el nombre especificado (o "Tu" por defecto) como nombre del personaje para distinguirlo de pensamientos
		/// </summary>
		/// <param name="name">Nombre personalizado del protagonista (opcional, por defecto "Tu"). Útil para localización/personalización</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry AsProtagonistSpeech(string name = "Tu")
		{
			IsProtagonistSpeech = true;
			ProtagonistName = name;
			return this;
		}
		
		/// <summary>
		/// Establece la acción a ejecutar cuando se muestran las opciones de verdad/mentira
		/// Se ejecuta cuando aparecen las opciones en pantalla
		/// </summary>
		/// <param name="onShowDecision">Acción a ejecutar cuando se muestran las opciones</param>
		/// <returns>Esta instancia para encadenar métodos (Builder pattern)</returns>
		public DialogEntry WithShowTruthLieDecision(System.Action onShowDecision)
		{
			OnShowTruthLieDecision = onShowDecision;
			return this;
		}
	}

	/// <summary>
	/// Estructura que representa una opción de respuesta en un diálogo
	/// </summary>
	public struct DialogOption
	{
		/// <summary>
		/// Texto de la opción que se mostrará al jugador
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Acción a ejecutar cuando se selecciona esta opción
		/// </summary>
		public System.Action OnSelected { get; set; }

		/// <summary>
		/// Diálogos siguientes que se mostrarán después de seleccionar esta opción (opcional)
		/// Si es null, se continúa con el siguiente diálogo normal
		/// </summary>
		public List<DialogEntry> NextDialogs { get; set; }

		/// <summary>
		/// Crea una nueva opción de diálogo
		/// </summary>
		/// <param name="text">Texto de la opción</param>
		/// <param name="onSelected">Acción a ejecutar cuando se selecciona</param>
		/// <param name="nextDialogs">Diálogos siguientes (opcional)</param>
		public DialogOption(string text, System.Action onSelected, List<DialogEntry> nextDialogs = null)
		{
			Text = text;
			OnSelected = onSelected;
			NextDialogs = nextDialogs;
		}
	}
}