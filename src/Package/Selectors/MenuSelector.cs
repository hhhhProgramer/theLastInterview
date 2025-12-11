using Godot;
using System.Collections.Generic;

namespace Aprendizdemago.Package.Selectors
{
    /// <summary>
    /// Selector de menú refactorizado usando las mejores prácticas
    /// CRÍTICO: Usa anclas para posicionamiento responsive
    /// Usando las mejores prácticas SOLID, KISS, SRP, DRY
    /// </summary>
    public partial class MenuSelector : Control
    {
        private HBoxContainer _cardsContainer;
        public List<Card> _cards = new List<Card>(); // Público para compatibilidad con NavigationCardCreator
        
        /// <summary>
        /// Constructor de MenuSelector
        /// </summary>
        /// <param name="width">Ancho del selector</param>
        /// <param name="height">Alto del selector</param>
        public MenuSelector(float width, float height)
        {
            // CRÍTICO: Configurar anclas ANTES de agregar al árbol
            SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            
            CustomMinimumSize = new Vector2(width, height);
            
            // Configurar mouse filter
            MouseFilter = Control.MouseFilterEnum.Stop;
        }
        
        public override void _Ready()
        {
            base._Ready();
            SetupSelector();
        }
        
        /// <summary>
        /// Configura la estructura del selector
        /// </summary>
        private void SetupSelector()
        {
            // Crear contenedor para las cartas
            _cardsContainer = new HBoxContainer();
            _cardsContainer.Name = "CardsContainer";
            _cardsContainer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            _cardsContainer.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            _cardsContainer.SizeFlagsVertical = Control.SizeFlags.ExpandFill;
            
            // Configurar separación entre cartas
            _cardsContainer.AddThemeConstantOverride("separation", 20);
            
            // Alinear cartas al centro
            _cardsContainer.Alignment = BoxContainer.AlignmentMode.Center;
            
            AddChild(_cardsContainer);
        }
        
        /// <summary>
        /// Agrega una tarjeta al selector
        /// </summary>
        /// <param name="card">Tarjeta a agregar</param>
        public void AddCard(Card card)
        {
            if (card == null)
            {
                GD.PrintErr("[MenuSelector] Intento de agregar tarjeta null");
                return;
            }
            
            if (_cardsContainer == null)
            {
                GD.PrintErr("[MenuSelector] CardsContainer no inicializado");
                return;
            }
            
            // CRÍTICO: Configurar anclas de la tarjeta ANTES de agregarla
            card.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.TopLeft);
            
            // Agregar a la lista y al contenedor
            _cards.Add(card);
            _cardsContainer.AddChild(card);
            
            GD.Print($"[MenuSelector] Tarjeta agregada: {card.Name}");
        }
        
        /// <summary>
        /// Obtiene todas las tarjetas del selector
        /// </summary>
        /// <returns>Lista de tarjetas</returns>
        public List<Card> GetCards()
        {
            return new List<Card>(_cards);
        }
        
        /// <summary>
        /// Limpia todas las tarjetas del selector
        /// </summary>
        public void ClearCards()
        {
            foreach (var card in _cards)
            {
                if (IsInstanceValid(card))
                {
                    card.QueueFree();
                }
            }
            _cards.Clear();
        }
    }
}
