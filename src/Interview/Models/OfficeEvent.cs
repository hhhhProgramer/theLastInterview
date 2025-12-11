namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Modelo que representa un evento "Meta-Oficina" aleatorio
    /// </summary>
    public class OfficeEvent
    {
        /// <summary>
        /// ID único del evento
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Texto del evento
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OfficeEvent(string id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

