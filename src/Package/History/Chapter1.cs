using Godot;

namespace SlimeKingdomChronicles.src.Core.History
{
    /// <summary>
    /// Primer capítulo de la historia que contiene 3 escenas sobre el encuentro del aldeano con el slime
    /// </summary>
    public class Chapter1 : IChapter
    {
        private readonly ChapterScene[] _scenes;
        
        /// <summary>
        /// Nombre del capítulo
        /// </summary>
        public string ChapterName => "El Encuentro";
        
        /// <summary>
        /// Número total de escenas en el capítulo
        /// </summary>
        public int TotalScenes => _scenes.Length;
        
        /// <summary>
        /// Inicializa el primer capítulo con las 3 escenas de la historia
        /// </summary>
        public Chapter1()
        {
            _scenes = new ChapterScene[]
            {
                CreateScene1(),
                CreateScene2(),
                CreateScene3()
            };
        }
        
        /// <summary>
        /// Obtiene la información de una escena específica
        /// </summary>
        /// <param name="sceneIndex">Índice de la escena (0-based)</param>
        /// <returns>Información de la escena o null si el índice es inválido</returns>
        public ChapterScene GetScene(int sceneIndex)
        {
            if (sceneIndex < 0 || sceneIndex >= _scenes.Length)
                return null;
                
            return _scenes[sceneIndex];
        }
        
        /// <summary>
        /// Verifica si el capítulo tiene más escenas después del índice dado
        /// </summary>
        /// <param name="currentSceneIndex">Índice de la escena actual</param>
        /// <returns>True si hay más escenas, False si es la última</returns>
        public bool HasNextScene(int currentSceneIndex)
        {
            return currentSceneIndex + 1 < _scenes.Length;
        }
        
        /// <summary>
        /// Crea la primera escena: "La Decisión del Aldeano"
        /// </summary>
        /// <returns>Escena configurada con título, texto e imagen</returns>
        private ChapterScene CreateScene1()
        {
            return new ChapterScene
            {
                Title = "Capítulo 1: La Decisión del Aldeano",
                StoryText = "El bullicio del pueblo me agobiaba. Comerciantes gritando, carros haciendo ruido, gente corriendo de un lado a otro... necesitaba paz. Decidí alejarme, adentrarme al Bosque de las Sombras donde nadie me molestaría.\n\nConstruí mi cabaña cerca de un arroyo cristalino. Cada día pescaba, recolectaba frutas y exploraba. La vida era simple, tranquila, exactamente lo que quería.\n\nPero esa noche... esa noche cambió todo.",
                ImagePath = "res://src/History/Images/image-1.png",
                TextSpeed = 25.0f,
                PauseAfterText = 3.0f
            };
        }
        
        /// <summary>
        /// Crea la segunda escena: "La Tormenta y la Cueva"
        /// </summary>
        /// <returns>Escena configurada con título, texto e imagen</returns>
        private ChapterScene CreateScene2()
        {
            return new ChapterScene
            {
                Title = "Capítulo 2: La Tormenta y la Cueva",
                StoryText = "La tormenta comenzó al atardecer. Rayos iluminaban el cielo, truenos retumbaban entre los árboles. Me refugié en mi cabaña, pero algo me llamaba desde afuera.\n\nUna luz tenue brillaba entre los árboles. No era el resplandor de un rayo, sino algo más suave, más mágico. Mi curiosidad me empujó a salir.\n\nSiguiendo la luz, llegué a una cueva que nunca había visto antes. En su interior, cristales dorados brillaban con una luz cálida, y en el centro había un altar de piedra con runas antiguas.",
                ImagePath = "res://src/History/Images/image-2.png",
                TextSpeed = 25.0f,
                PauseAfterText = 3.0f
            };
        }
        
        /// <summary>
        /// Crea la tercera escena: "El Encuentro con el Slime"
        /// </summary>
        /// <returns>Escena configurada con título, texto e imagen</returns>
        private ChapterScene CreateScene3()
        {
            return new ChapterScene
            {
                Title = "Capítulo 3: El Encuentro con el Slime",
                StoryText = "Y ahí estaba él.\n\nUn pequeño slime verde, tembloroso y débil, escondido detrás de una roca. Su cuerpo gelatinoso brillaba suavemente, y en su centro, una pequeña gema dorada pulsaba con una luz tenue, como si fuera su corazón latiendo.\n\nLos aldeanos siempre decían que los slimes eran monstruos peligrosos, pero este pequeño ser solo me inspiraba compasión. Se acercó tímidamente, confiando en mi voz suave.\n\nLe di de mi comida, y su gema brilló más intensamente, como si estuviera sonriendo. En ese momento, supe que no podía dejarlo solo. Lo llevé a mi cabaña, le di refugio y calor.",
                ImagePath = "res://src/History/Images/image-3.png",
                TextSpeed = 25.0f,
                PauseAfterText = 4.0f
            };
        }
    }
}
