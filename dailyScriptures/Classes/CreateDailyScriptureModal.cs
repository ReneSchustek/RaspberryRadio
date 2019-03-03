using DailyScriptures.Templates;
using Database.Model;
using System.Collections.Generic;

namespace DailyScriptures.Classes
{
    public class CreateDailyScriptureModal
    {
        /// <summary>
        /// Erzeugt das Element Modal für den Tagestext
        /// </summary>
        /// <param name="dailyScriptures">Liste der Tagestexte</param>
        /// <returns>HTML-Element</returns>
        public string CreateModal(IList<DailyScriptureModel> dailyScriptures)
        {
            string modalTemplate = DailyScriptureModalElement.Get();
            string result = string.Empty;

            foreach (DailyScriptureModel dailyScripture in dailyScriptures)
            {
                string changedContent = modalTemplate;
                changedContent = changedContent.Replace("{{Language}}", dailyScripture.Language);
                changedContent = changedContent.Replace("{{Text}}", dailyScripture.Text);
                changedContent = changedContent.Replace("{{Comment}}", dailyScripture.Comment);

                result += changedContent;
            }

            return result;
        }
    }
}
