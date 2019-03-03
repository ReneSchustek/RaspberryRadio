using System.Text.RegularExpressions;

namespace Helper.Classes
{
    /// <summary>
    /// Entfernt HTML-Tags aus einem string
    /// </summary>
    public static class RemoveHtml
    {
        const string pattern = "<.*?>";

        /// <summary>
        /// Entfernt alle HTML Tags
        /// </summary>
        /// <param name="input">String mit Tags</param>
        /// <returns>String ohne Tags</returns>
        public static string Remove(string input)
        {
            return Regex.Replace(input, pattern, string.Empty);
        }
    }
}
