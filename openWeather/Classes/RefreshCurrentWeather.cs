using Database.Model;
using Database.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using OpenWeather.Classes.WeatherData;
using OpenWeather.Classes.WebSocket;
using OpenWeather.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class RefreshCurrentWeather
    {
        #region Models
        private readonly IHubContext<SendOpenWeatherCurrent> _hubContext;
        #endregion

        #region Constructor
        public RefreshCurrentWeather(IHubContext<SendOpenWeatherCurrent> hubContext)
        {
            _hubContext = hubContext;
        }
        #endregion

        /// <summary>
        /// Ruft die aktuellen Wetterdaten für die Aktualisierung ab, passt diese an und sendet diese an die Clients
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            OpenWeatherSavedCitiesService openWeatherSavedCitiesService = new OpenWeatherSavedCitiesService();
            IList<OpenWeatherSavedCitiesModel> openWeatherSavedCities = await openWeatherSavedCitiesService.ReadAllAsync();

            GetCurrentWeatherData getCurrentWeatherData = new GetCurrentWeatherData();

            foreach (OpenWeatherSavedCitiesModel openWeatherSavedCity in openWeatherSavedCities)
            {
                ApiCurrentWeatherModel apiCurrentWeather = await getCurrentWeatherData.GetCurrent(openWeatherSavedCity.WeatherCitiesId, "de");

                if (apiCurrentWeather == null) { continue; }

                //Uv-Index
                GetCurrentUvIndexData getCurrentUvIndex = new GetCurrentUvIndexData();
                ApiCurrentUvIndexModel apiCurrentUvIndex = await getCurrentUvIndex.GetUvIndex(openWeatherSavedCity.WeatherCitiesId);

                string windDeg = ChangeWeatherData.CreateWindDeg(apiCurrentWeather.Wind.Deg);
                double windspeed = apiCurrentWeather.Wind.Speed * 3.6;

                //Sonnenauf- und untergang von lon in DateTime umrechnen
                DateTime defDateTime = new DateTime(1970, 1, 1, 0, 0, 0);

                DateTime sunrise = defDateTime.AddSeconds(apiCurrentWeather.Sys.Sunrise);
                DateTime sunset = defDateTime.AddSeconds(apiCurrentWeather.Sys.Sunset);

                //Beschreibung der Bewölkung
                string cloudDescription = ChangeWeatherData.CreateCloudsDescription(apiCurrentWeather.Clouds.All);

                //Nur benötige Daten in einfacher Form senden
                WsCurrentModel wsCurrentModel = new WsCurrentModel
                {
                    Deg = windDeg,
                    Description = apiCurrentWeather.Weather[0].Description,
                    Humidity = apiCurrentWeather.Main.Humidity,
                    Icon = apiCurrentWeather.Weather[0].Icon,
                    Name = apiCurrentWeather.Name,
                    Pressure = apiCurrentWeather.Main.Pressure,
                    Speed = windspeed,
                    Sunrise = sunrise.ToShortTimeString(),
                    Sunset = sunset.ToShortTimeString(),
                    Temp = apiCurrentWeather.Main.Temp,
                    UvIndex = apiCurrentUvIndex.Value,
                    CloudinessDescr = cloudDescription,
                    CloudinessPerc = apiCurrentWeather.Clouds.All
                };

                //Daten versenden
                string message = JsonConvert.SerializeObject(wsCurrentModel);
                await _hubContext.Clients.All.SendAsync("OnCurrentWeatherPublish", message);
            }

        }
    }
}
