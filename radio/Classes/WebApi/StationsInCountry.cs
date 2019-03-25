using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using radio.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace radio.Classes.WebApi
{
    [Route("/api/radio/countrysearch")]
    [ApiController]
    public class StationsInCountry : Controller
    {
        #region Models
        #endregion

        #region Constructor
        public StationsInCountry() { }
        #endregion

        /// <summary>
        /// Sucht die Stationen eines Landes über die Dirble-Api
        /// </summary>
        /// <param name="country">Länderkürzel</param>
        /// <param name="selectedpage">Seite der Ergebnisse</param>
        /// <returns>Liste der gefundenen Stationen</returns>
        [HttpGet("{country}/{selectedpage?}")]
        [Route("StationsInCountry")]
        public async Task<IList<DirbleRadioInCountryModel>> GetStationsInCountry(string country, int selectedpage = 0)
        {
            //Token abrufen
            AppTokenService tokenService = new AppTokenService();
            IList<AppTokenModel> appToken = await tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return new List<DirbleRadioInCountryModel>(); }

            //Daten abfragen
            if (country == null) { country = "de"; }
            string url = "http://api.dirble.com/v2/countries/" + country + "/stations?token=" + appToken[0].DirbleToken + "&page=" + selectedpage + "&per_page=10";

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
                    return new List<DirbleRadioInCountryModel>();
                }

                if (apiResponse == string.Empty) { return new List<DirbleRadioInCountryModel>(); }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    DirbleRadioInCountryModel[] apiRadioArray = JsonConvert.DeserializeObject<DirbleRadioInCountryModel[]>(apiResponse);
                    return new List<DirbleRadioInCountryModel>(apiRadioArray);
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new List<DirbleRadioInCountryModel>();
                }
            }
        }
    }
}
