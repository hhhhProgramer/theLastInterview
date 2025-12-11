namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Modelo que representa una interrupción incómoda durante la entrevista
    /// </summary>
    public class OfficeInterruption
    {
        /// <summary>
        /// ID único de la interrupción
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Texto de la interrupción
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Fuente de la interrupción (ej: "Sistema", "Llamada", "Pop-up")
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OfficeInterruption(string id, string text, string source = "Sistema")
        {
            Id = id;
            Text = text;
            Source = source;
        }
    }
}

