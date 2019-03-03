using Database.Model;
using OpenWeather.Classes.WeatherData;
using OpenWeather.Model;
using OpenWeather.Templates;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class CreateForecastWeather
    {
        /// <summary>
        /// Erzeugt den Slider für die Wettervorhersage
        /// </summary>
        /// <param name="apiCurrentWeather"></param>
        /// <returns></returns>
        public async Task<string> CreateForecast(IList<OpenWeatherSavedCitiesModel> openWeatherSavedCities)
        {
            string forecastCityStartElement = OpenWeatherForecastElement.GetCityStartElement();
            string forecastCityEndElement = OpenWeatherForecastElement.GetCityEndElement();
            string forecastElement = OpenWeatherForecastElement.GetForecastElement();

            CultureInfo culture = new CultureInfo("de-DE");

            DateTime defDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

            string changedContent = string.Empty;

            //Nur bestimmte Zeiten anzeigen
            string[] onlySelectedTimes = new string[] { "06:00", "12:00", "18:00", "21:00" };

            int count = 0;

            foreach (OpenWeatherSavedCitiesModel openWeatherSavedCity in openWeatherSavedCities)
            {
                if (count == 4) { count = 0; }
                count++;

                //Abfragen der aktuellen Daten
                GetForecastWeatherData getForecastWeatherData = new GetForecastWeatherData();
                ApiForecastWeatherModel apiForecastWeather = await getForecastWeatherData.GetForecast(openWeatherSavedCity.WeatherCitiesId, "de");
                string forecastContent = string.Empty;

                int rowCounter = 0;

                foreach (ForecastListModel forecastList in apiForecastWeather.List)
                {
                    DateTime forecastTime = defDateTime.AddSeconds(forecastList.Dt);

                    //Wenn andere Zeiten als vorgeben, weiter
                    if (!onlySelectedTimes.Contains(forecastTime.ToShortTimeString())) { continue; }

                    rowCounter++;

                    if (rowCounter == 1) { changedContent += forecastCityStartElement; }

                    string changedForecastContent = forecastElement;

                    string shortWeekday = culture.DateTimeFormat.GetShortestDayName(forecastTime.DayOfWeek);
                    string weatherDescription = ChangeWeatherData.CreateWeatherDescription(forecastList.Weather[0].Description);

                    changedForecastContent = changedForecastContent.Replace("{{TimeSpan}}", forecastList.Dt.ToString());
                    changedForecastContent = changedForecastContent.Replace("{{ForecastCity}}", apiForecastWeather.City.Name);
                    changedForecastContent = changedForecastContent.Replace("{{ForecastTime}}", shortWeekday + " " + forecastTime.ToShortTimeString());
                    changedForecastContent = changedForecastContent.Replace("{{ForecastImage}}", forecastList.Weather[0].Icon);
                    changedForecastContent = changedForecastContent.Replace("{{ForecastDescription}}", weatherDescription);
                    changedForecastContent = changedForecastContent.Replace("{{ForecastTemp}}", forecastList.Main.Temp.ToString());

                    if (rowCounter < 4) { changedForecastContent = changedForecastContent.Replace("{{ForecastClass}}", "weather-forecast-container"); }
                    else { changedForecastContent = changedForecastContent.Replace("{{ForecastClass}}", ""); }

                    changedContent += changedForecastContent;

                    if (rowCounter == 4) { changedContent += forecastCityEndElement; rowCounter = 0; }
                }

                count = rowCounter;
            }

            if (count < 4 && count != 0) { changedContent += forecastCityEndElement; }

            return changedContent;
        }
    }
}
