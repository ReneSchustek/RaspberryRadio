using DailyScriptures.Templates;
using Database.Model;
using Helper.Classes;
using System;
using System.Collections.Generic;

namespace DailyScriptures.Classes
{
    public class CreateDailyScriptureCarousel
    {
        /// <summary>
        /// Erzeugt das Element Carousel für den Tagestext
        /// </summary>
        /// <param name="dailyScriptures">Liste der Tagestexte</param>
        /// <returns>HTML-String</returns>
        public string CreateCarousel(IList<DailyScriptureModel> dailyScriptures)
        {
            try
            {
                string carouselTemplate = DailyScriptureCarouselElement.Get();

                string carouselContent = "<div id=\"dailyScriptureIndicators\" class=\"carousel slide\">";

                carouselContent += "<ol class=\"carousel-indicators\">";

                for (int i = 0; i < dailyScriptures.Count; i++)
                {
                    if (i == 0)
                    {
                        carouselContent += "<li data-target=\"#dailyScriptureIndicators\" data-slide-to=\"" + i + "\" class=\"active\"></li>";
                        continue;
                    }

                    carouselContent += "<li data-target=\"#dailyScriptureIndicators\" data-slide-to=\"" + i + "\"></li>";

                }

                carouselContent += "</ol>";
                carouselContent += "<div class=\"carousel-inner\">";

                int counter = 0;

                foreach (DailyScriptureModel dailyScripture in dailyScriptures)
                {
                    if (counter == 0) { carouselContent += "<div class=\"carousel-item active\">"; }
                    else { carouselContent += "<div class=\"carousel-item\">"; }

                    counter++;
                    string changedContent = carouselTemplate;
                    changedContent = changedContent.Replace("{{Language}}", dailyScripture.Language);
                    changedContent = changedContent.Replace("{{Title}}", dailyScripture.Title);
                    changedContent = changedContent.Replace("{{Text}}", dailyScripture.Text);

                    carouselContent += changedContent;
                    carouselContent += "</div>";
                }

                carouselContent += "</div>";

                carouselContent += "</div>";

                return carouselContent;

            }
            catch (Exception ex)
            {
                WriteLog.Write("Index CreateDailyScriptureCarousel: " + ex.ToString(), "error");
                return string.Empty;
            }
        }
    }
}
