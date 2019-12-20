using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ToolsPack.String
{
    /// <summary>
    /// Generate a string
    /// </summary>
    public static class DiacriticsRemover
    {
        private static Random random = new Random();

        /// <summary>
        /// Remove accent from string "Léo lacrève" => "Leo lacreve"
        /// </summary>
        /// <param name="length">length of the random string</param>
        /// <param name="chars">characters set</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}