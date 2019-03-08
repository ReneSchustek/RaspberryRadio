using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeather.Classes.WebApi
{
    /// <summary>
    /// Durchsucht OpenWeather nach Städten
    /// </summary>
    [Route("/api/openWeather/search")]
    [ApiController]
    public class SearchResponse : Controller
    {
        #region Models
        #endregion

        #region Constructor
        public SearchResponse() { }
        #endregion

        /// <summary>
        /// Sucht nach einer Stadt in einem Land
        /// </summary>
        /// <param name="name">Name der Stadt</param>
        /// <param name="country">Länderkürzel</param>
        /// <returns>IList<OpenWeatherCityModel></returns>
        [HttpGet("{name}/{county?}")]
        [Route("NameInCountry")]
        public async Task<IList<OpenWeatherCityModel>> SearchCityAsync(string name, string country = "")
        {
            try
            {
                IList<OpenWeatherCityModel> result = null;

                OpenWeatherCityService openWeatherCityService = new OpenWeatherCityService();

                if (country == "" || country == string.Empty || country == null) { result = await openWeatherCityService.ReadByNameAsync(name); }
                else { result = await openWeatherCityService.ReadByNameCountryAsync(name, country); }

                return result;
            }
            catch (Exception ex)
            {
                WriteLog.Write("SearchResponse SearchCity: " + ex.ToString());

                return new List<OpenWeatherCityModel>();
            }
        }

        /// <summary>
        /// Listet alle Städte eines Landes auf
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        [HttpGet("{country}")]
        [Route("InCountry")]
        public async Task<IList<OpenWeatherCityModel>> SearchCityByCountryAsync(string country)
        {
            try
            {
                OpenWeatherCityService openWeatherCityService = new OpenWeatherCityService();

                return await openWeatherCityService.ReadByCountryAsync(country);
            }
            catch (Exception ex)
            {
                WriteLog.Write("SearchResponse SearchCityByCountry: " + ex.ToString());

                return new List<OpenWeatherCityModel>();
            }
        }
    }
}
