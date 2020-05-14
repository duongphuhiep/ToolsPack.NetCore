using System.Globalization;
using System.Text;

namespace ToolsPack.String
{
    /// <summary>
    /// Remove accent from string
    /// </summary>
    public static class DiacriticsRemover
    {
        /// <summary>
        /// Remove accent from string "Léo lacrève" => "Leo lacreve"
        /// </summary>
        /// <returns></returns>
        public static string RemoveDiacritics(this string s)
        {
            if (s == null) return null;
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