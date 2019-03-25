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
    public class GetCurrentWeatherData
    {
        #region Models
        private readonly AppTokenService _tokenService;
        #endregion

        #region Constructor
        public GetCurrentWeatherData()
        {
            _tokenService = new AppTokenService();
        }
        #endregion

        /// <summary>
        /// Aktuelles Wetter abfragen
        /// </summary>
        /// <param name="cityId">OpenWeather CityId</param>
        /// <param name="lang">Sprache in Kurzform (z.B. de)</param>
        /// <returns>ApiCurrentModel</returns>
        public async Task<ApiCurrentWeatherModel> GetCurrent(int cityId, string lang = null)
        {
            //Token abrufen
            IList<AppTokenModel> appToken = await _tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return new ApiCurrentWeatherModel(); }

            //Daten abfragen
            if (lang == null) { lang = "de"; }
            string url = "http://api.openweathermap.org/data/2.5/weather?id=" + cityId + "&lang=" + lang + "&units=metric&appid=" + appToken[0].OpenWeatherToken;

            string apiResponse = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(url);
                    HttpResponseMessage response = await client.GetAsync(url);

                    apiResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new ApiCurrentWeatherModel();
                }

                /*api Response sollte wie folgt aussehen: 
                 * { "coord":{"lon":10.97,"lat":50.27},
                 * "weather":[{"id":800,"main":"Clear","description":"Klarer Himmel","icon":"01n"}],
                 * "base":"stations",
                 * "main":{"temp":276.15,"pressure":1023,"humidity":91,"temp_min":276.15,"temp_max":276.15},
                 * "visibility":10000,"wind":{"speed":2.1,"deg":230},
                 * "clouds":{"all":0},
                 * "dt":1550685000,
                 * "sys":{"type":1,"id":1313,"message":0.0033,"country":"DE","sunrise":1550643426,"sunset":1550680995},
                 * "id":2939951,
                 * "name":"Coburg",
                 * "cod":200} */


                if (apiResponse == string.Empty) { return new ApiCurrentWeatherModel(); }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    ApiCurrentWeatherModel apiCurrentWeather = JsonConvert.DeserializeObject<ApiCurrentWeatherModel>(apiResponse);
                    return apiCurrentWeather;
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new ApiCurrentWeatherModel();
                }
            }
        }
    }
}
