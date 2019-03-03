using Database.Model;
using Database.Services;
using Microsoft.AspNetCore.SignalR;
using OpenWeather.Classes.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class RefreshForecastWeather
    {
        #region Models
        private readonly IHubContext<SendOpenWeatherForecast> _hubContext;
        #endregion

        #region Constructor
        public RefreshForecastWeather(IHubContext<SendOpenWeatherForecast> hubContext)
        {
            _hubContext = hubContext;
        }
        #endregion

        /// <summary>
        /// Ruft die Wettervorhersage für die Aktualisierung ab, passt diese an und sendet diese an die Clients
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            OpenWeatherSavedCitiesService openWeatherSavedCitiesService = new OpenWeatherSavedCitiesService();
            IList<OpenWeatherSavedCitiesModel> openWeatherSavedCities = await openWeatherSavedCitiesService.ReadAllAsync();

            CreateForecastWeather createForecastWeather = new CreateForecastWeather();
            string message = await createForecastWeather.CreateForecast(openWeatherSavedCities);

            //Senden
            await _hubContext.Clients.All.SendAsync("OnForecasttWeatherPublish", message);
        }
    }
}
