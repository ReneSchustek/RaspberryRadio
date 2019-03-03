using Database.Model;
using Database.Services;
using Helper.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class ReadSavedCities
    {
        public async Task<IList<OpenWeatherCityModel>> Read()
        {
            IList<OpenWeatherCityModel> result = new List<OpenWeatherCityModel>();

            try
            {
                OpenWeatherCityService openWeatherCityService = new OpenWeatherCityService();
                OpenWeatherSavedCitiesService openWeatherSavedCitiesService = new OpenWeatherSavedCitiesService();

                IList<OpenWeatherSavedCitiesModel> savedCities = await openWeatherSavedCitiesService.ReadAllAsync();

                foreach (OpenWeatherSavedCitiesModel savedCity in savedCities)
                {
                    OpenWeatherCityModel city = await openWeatherCityService.ReadByCityIdAsync(savedCity.WeatherCitiesId);
                    if (city == null) { continue; }

                    result.Add(city);
                }
            }
            catch (Exception ex)
            {
                WriteLog.Write("ReadSavedCities: " + ex.ToString(), "error");
            }

            return result;
        }
    }
}
