using Database.Model;
using OpenWeather.Classes.WeatherData;
using OpenWeather.Model;
using OpenWeather.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class CreateWeatherCarousel
    {
        /// <summary>
        /// Erzeugt das Element Carousel für das Wetter und gleichzeitig das entsprechende Modal
        /// </summary>
        /// <param name="openWeatherSavedCities">Gespeicherte Städte</param>
        /// <returns>HTML-Element</returns>
        public async Task<string> CreateCarousel(IList<OpenWeatherSavedCitiesModel> openWeatherSavedCities)
        {
            string carouselTemplate = OpenWeatherCarouselElement.Get();

            string carouselContent = "<div id=\"openWeatherIndicators\" class=\"carousel slide\">";

            carouselContent += "<ol class=\"carousel-indicators\">";

            for (int i = 0; i < openWeatherSavedCities.Count; i++)
            {
                if (i == 0)
                {
                    carouselContent += "<li data-target=\"#openWeatherIndicators\" data-slide-to=\"" + i + "\" class=\"active\"></li>";
                    continue;
                }

                carouselContent += "<li data-target=\"#openWeatherIndicators\" data-slide-to=\"" + i + "\"></li>";

            }

            carouselContent += "</ol>";
            carouselContent += "<div class=\"carousel-inner\">";

            int counter = 0;

            foreach (OpenWeatherSavedCitiesModel openWeatherSavedCity in openWeatherSavedCities)
            {
                //Abfrage der aktuellen Daten
                GetCurrentWeatherData getCurrentWeatherData = new GetCurrentWeatherData();
                ApiCurrentWeatherModel apiCurrentWeather = await getCurrentWeatherData.GetCurrent(openWeatherSavedCity.WeatherCitiesId, "de");

                if (counter == 0) { carouselContent += "<div class=\"carousel-item active\">"; }
                else { carouselContent += "<div class=\"carousel-item\">"; }

                counter++;

                string weatherDescription = ChangeWeatherData.CreateWeatherDescription(apiCurrentWeather.Weather[0].Description);

                string changedContent = carouselTemplate;

                //Anzeige erzeugen
                changedContent = changedContent.Replace("{{WeatherCityName}}", apiCurrentWeather.Name);
                changedContent = changedContent.Replace("{{WeatherImage}}", apiCurrentWeather.Weather[0].Icon);
                changedContent = changedContent.Replace("{{WeatherDescription}}", weatherDescription);
                changedContent = changedContent.Replace("{{WeatherTemprature}}", apiCurrentWeather.Main.Temp.ToString());

                carouselContent += changedContent;
                carouselContent += "</div>";
            }

            carouselContent += "</div>";

            carouselContent += "</div>";

            return carouselContent;
        }
    }
}
