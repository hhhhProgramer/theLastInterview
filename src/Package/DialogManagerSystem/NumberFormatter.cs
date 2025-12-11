using Godot;

namespace SlimeKingdomChronicles.Core.UI
{
    /// <summary>
    /// Helper estático para formatear números grandes de manera legible
    /// Convierte números como 5000 a "5K", 1000000 a "1M", etc.
    /// </summary>
    public static class NumberFormatter
    {
        /// <summary>
        /// Formatea un número entero a formato legible con sufijos (K, M, B, T)
        /// </summary>
        /// <param name="number">Número a formatear</param>
        /// <returns>String formateado (ej: "5K", "1.2M", "3.5B")</returns>
        public static string FormatNumber(int number)
        {
            if (number < 1000)
            {
                return number.ToString();
            }
            
            if (number < 1000000)
            {
                float kValue = number / 1000f;
                return FormatWithDecimals(kValue, "K");
            }
            
            if (number < 1000000000)
            {
                float mValue = number / 1000000f;
                return FormatWithDecimals(mValue, "M");
            }
            
            if (number < 1000000000L)
            {
                float bValue = number / 1000000000f;
                return FormatWithDecimals(bValue, "B");
            }
            
            // Para números muy grandes (trillones)
            float tValue = number / 1000000000000f;
            return FormatWithDecimals(tValue, "T");
        }
        
        /// <summary>
        /// Formatea un número flotante a formato legible con sufijos
        /// </summary>
        /// <param name="number">Número a formatear</param>
        /// <returns>String formateado (ej: "5.2K", "1.5M")</returns>
        public static string FormatNumber(float number)
        {
            if (number < 1000)
            {
                return number.ToString("F0");
            }
            
            if (number < 1000000)
            {
                float kValue = number / 1000f;
                return FormatWithDecimals(kValue, "K");
            }
            
            if (number < 1000000000)
            {
                float mValue = number / 1000000f;
                return FormatWithDecimals(mValue, "M");
            }
            
            if (number < 1000000000L)
            {
                float bValue = number / 1000000000f;
                return FormatWithDecimals(bValue, "B");
            }
            
            // Para números muy grandes (trillones)
            float tValue = number / 1000000000000f;
            return FormatWithDecimals(tValue, "T");
        }
        
        /// <summary>
        /// Formatea un número con decimales apropiados según el valor
        /// </summary>
        /// <param name="value">Valor a formatear</param>
        /// <param name="suffix">Sufijo (K, M, B, T)</param>
        /// <returns>String formateado con decimales apropiados</returns>
        private static string FormatWithDecimals(float value, string suffix)
        {
            if (value >= 100)
            {
                // Para valores >= 100, mostrar sin decimales
                return $"{(int)value}{suffix}";
            }
            else if (value >= 10)
            {
                // Para valores >= 10, mostrar 1 decimal
                return $"{value:F1}{suffix}";
            }
            else
            {
                // Para valores < 10, mostrar 2 decimales
                return $"{value:F2}{suffix}";
            }
        }
        
        /// <summary>
        /// Formatea un número con un formato específico para monedas
        /// </summary>
        /// <param name="amount">Cantidad de dinero</param>
        /// <returns>String formateado para monedas (ej: "$5K", "$1.2M")</returns>
        public static string FormatCurrency(int amount)
        {
            return $"${FormatNumber(amount)}";
        }
        
        /// <summary>
        /// Formatea un número con un formato específico para cantidades de items
        /// </summary>
        /// <param name="quantity">Cantidad de items</param>
        /// <returns>String formateado para cantidades (ej: "5K peces", "1.2M piedras")</returns>
        public static string FormatQuantity(int quantity, string itemName = "")
        {
            string formattedNumber = FormatNumber(quantity);
            return string.IsNullOrEmpty(itemName) ? formattedNumber : $"{formattedNumber} {itemName}";
        }
    }
}
