using Database.Model;
using Database.Services;
using Helper.Classes;
using Newtonsoft.Json;
using OpenWeather.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace OpenWeather.Classes.WeatherData
{
    public class GetForecastWeatherData
    {
        #region Models
        private readonly AppTokenService _tokenService;
        #endregion

        #region Constructor
        public GetForecastWeatherData()
        {
            _tokenService = new AppTokenService();
        }
        #endregion

        /// <summary>
        /// Wettervorhersage abfragen
        /// </summary>
        /// <param name="cityId">OpenWeather CityId</param>
        /// <param name="lang">Sprache in Kurzform (z.B. de)</param>
        /// <returns>ApiCurrentModel</returns>
        public async Task<ApiForecastWeatherModel> GetForecast(int cityId, string lang = null)
        {
            //Token abrufen
            IList<AppTokenModel> appToken = await _tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return new ApiForecastWeatherModel(); }

            //Daten abfragen
            if (lang == null) { lang = "de"; }
            string url = "http://api.openweathermap.org/data/2.5/forecast?id=" + cityId + "&lang=" + lang + "&units=metric&appid=" + appToken[0].OpenWeatherToken;

            string apiResponse = string.Empty;

            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(url);
                    var response = await client.GetAsync(url);

                    apiResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new ApiForecastWeatherModel();
                }

                /* api Response sollte wie folgt aussehen: 
                 * {
                 * "cod":"200",
                 * "message":0.0042,
                 * "cnt":40,
                 * "list":[
                 *          {
                 *              "dt":1550826000,
                 *              "main":{
                 *                  "temp":7.01,
                 *                  "temp_min":6.63,
                 *                  "temp_max":7.01,
                 *                  "pressure":1033.37,
                 *                  "sea_level":1033.37,
                 *                  "grnd_level":978.88,
                 *                  "humidity":96,
                 *                  "temp_kf":0.37
                 *              },
                 *              "weather":[{
                 *                  "id":500,
                 *                  "main":"Rain",
                 *                  "description":"Leichter Regen",
                 *                  "icon":"10d"
                 *              }],
                 *              "clouds":{"all":48},
                 *              "wind":{
                 *                  "speed":3.51,
                 *                  "deg":8.50049
                 *              },
                 *              "rain":{"3h":0.2475},
                 *              "sys":{"pod":"d"},
                 *              "dt_txt":"2019-02-22 09:00:00"
                 *          },
                 *          {
                 *              "dt":1550836800,
                 *              "main":{
                 *                  "temp":7.47, ... */


                if (apiResponse == string.Empty) { return new ApiForecastWeatherModel(); }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    ApiForecastWeatherModel apiCurrentWeather = JsonConvert.DeserializeObject<ApiForecastWeatherModel>(apiResponse);
                    return apiCurrentWeather;
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new ApiForecastWeatherModel();
                }
            }
        }
    }
}
