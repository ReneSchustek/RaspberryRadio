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
    [Route("/api/radio/station")]
    [ApiController]
    public class GetSpecificStation : Controller
    {

        #region Models
        #endregion

        #region Constructor
        public GetSpecificStation() { }
        #endregion

        /// <summary>
        /// Sucht einen speziellen Sender über die Dirble-Api
        /// </summary>
        /// <param name="id">Station-Id</param>
        /// <returns>gefundene Station</returns>
        [HttpGet("{id}")]
        [Route("SpecificStation")]
        public async Task<DirbleRadioSpecificModel> GetStation(int id)
        {
            //Token abrufen
            AppTokenService tokenService = new AppTokenService();
            IList<AppTokenModel> appToken = await tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return new DirbleRadioSpecificModel(); }

            //Daten abfragen
            string url = "http://api.dirble.com/v2/station/" + id.ToString() + "?token=" + appToken[0].DirbleToken;

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
                    return new DirbleRadioSpecificModel();
                }

                if (apiResponse == string.Empty) { return new DirbleRadioSpecificModel(); }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    return JsonConvert.DeserializeObject<DirbleRadioSpecificModel>(apiResponse);
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new DirbleRadioSpecificModel();
                }
            }
        }
    }
}
