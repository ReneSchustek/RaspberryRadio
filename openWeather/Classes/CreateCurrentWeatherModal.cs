using Database.Model;
using OpenWeather.Classes.WeatherData;
using OpenWeather.Model;
using OpenWeather.Templates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class CreateCurrentWeatherModal
    {
        /// <summary>
        /// Erzeugt das Modal für das aktuelle Wetter
        /// </summary>
        /// <param name="apiCurrentWeather"></param>
        /// <returns></returns>
        public async Task<string> CreateModal(IList<OpenWeatherSavedCitiesModel> openWeatherSavedCities)
        {
            string modalTemplate = OpenWeatherModalElement.Get();
            string changedContent = string.Empty;

            foreach (OpenWeatherSavedCitiesModel openWeatherSavedCity in openWeatherSavedCities)
            {
                //Abfrage der aktuellen Daten
                GetCurrentWeatherData getCurrentWeatherData = new GetCurrentWeatherData();
                ApiCurrentWeatherModel apiCurrentWeather = await getCurrentWeatherData.GetCurrent(openWeatherSavedCity.WeatherCitiesId, "de");

                //Uv-Index
                GetCurrentUvIndexData getCurrentUvIndex = new GetCurrentUvIndexData();
                ApiCurrentUvIndexModel apiCurrentUvIndex = await getCurrentUvIndex.GetUvIndex(openWeatherSavedCity.WeatherCitiesId);

                if (apiCurrentWeather == null) { return string.Empty; }


                changedContent += modalTemplate;

                //Sonnenauf- und untergang von lon in DateTime umrechnen
                DateTime defDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

                DateTime sunrise = defDateTime.AddSeconds(apiCurrentWeather.Sys.Sunrise);
                DateTime sunset = defDateTime.AddSeconds(apiCurrentWeather.Sys.Sunset);

                //Beschreibung der Bewölkung
                string cloudDescription = ChangeWeatherData.CreateCloudsDescription(apiCurrentWeather.Clouds.All);

                //Geschwindigkeit von m/s in km/h
                double windspeed = apiCurrentWeather.Wind.Speed * 3.6;

                changedContent = changedContent.Replace("{{WeatherCityName}}", apiCurrentWeather.Name);

                changedContent = changedContent.Replace("{{WeatherCloudinessDescription}}", cloudDescription);
                changedContent = changedContent.Replace("{{WeatherCloudinessPercent}}", apiCurrentWeather.Clouds.All.ToString());
                changedContent = changedContent.Replace("{{WeatherSunrise}}", sunrise.ToShortTimeString());
                changedContent = changedContent.Replace("{{WeatherSunset}}", sunset.ToShortTimeString());
                changedContent = changedContent.Replace("{{WeatherPressure}}", apiCurrentWeather.Main.Pressure.ToString());
                changedContent = changedContent.Replace("{{WeatherSpeed}}", windspeed.ToString());
                changedContent = changedContent.Replace("{{WeatherDeg}}", ChangeWeatherData.CreateWindDeg(apiCurrentWeather.Wind.Deg));
                changedContent = changedContent.Replace("{{WeatherHumidity}}", apiCurrentWeather.Main.Humidity.ToString());
                changedContent = changedContent.Replace("{{WeatherUv}}", apiCurrentUvIndex.Value.ToString());
            }

            return changedContent;
        }


    }
}
