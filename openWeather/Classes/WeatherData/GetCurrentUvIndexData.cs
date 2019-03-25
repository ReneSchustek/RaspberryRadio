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
    public class GetCurrentUvIndexData
    {
        #region Models
        private readonly AppTokenService _tokenService;
        #endregion

        #region Constructor
        public GetCurrentUvIndexData()
        {
            _tokenService = new AppTokenService();
        }
        #endregion

        /// <summary>
        /// Liest den UV-Index für den Ort aus
        /// </summary>
        /// <param name="cityId">CityId</param>
        /// <returns></returns>
        public async Task<ApiCurrentUvIndexModel> GetUvIndex(int cityId)
        {
            //Token abrufen
            IList<AppTokenModel> appToken = await _tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return new ApiCurrentUvIndexModel(); }

            //Lat Lon abrufen
            OpenWeatherCityService openWeatherCityService = new OpenWeatherCityService();
            OpenWeatherCityModel openWeatherCity = await openWeatherCityService.ReadByCityIdAsync(cityId);

            //Daten abfragen            
            string url = "http://api.openweathermap.org/data/2.5/uvi?appid=" + appToken + "&lat=" + openWeatherCity.Lat.ToString() + "&lon=" + openWeatherCity.Lon.ToString();

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
                    return new ApiCurrentUvIndexModel();
                }

                /*api Response sollte wie folgt aussehen: 
                 */


                if (apiResponse == string.Empty) { return new ApiCurrentUvIndexModel(); }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    ApiCurrentUvIndexModel apiCurrentUvIndex = JsonConvert.DeserializeObject<ApiCurrentUvIndexModel>(apiResponse);
                    return apiCurrentUvIndex;
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new ApiCurrentUvIndexModel();
                }
            }
        }
    }
}
