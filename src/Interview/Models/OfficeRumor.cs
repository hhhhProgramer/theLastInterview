namespace TheLastInterview.Interview.Models
{
    /// <summary>
    /// Modelo que representa un rumor de la oficina
    /// </summary>
    public class OfficeRumor
    {
        /// <summary>
        /// ID único del rumor
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Texto del rumor
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OfficeRumor(string id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}

