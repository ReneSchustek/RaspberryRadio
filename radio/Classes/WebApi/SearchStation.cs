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
    [Route("/api/radio/search")]
    [ApiController]
    public class SearchStation: Controller
    {

        #region Models
        #endregion

        #region Constructor
        public SearchStation() { }
        #endregion

        [HttpGet("{searchString}/{countryCode?}/{selectedpage?}")]
        [Route("StationsInCountry")]
        public async Task<IList<DirbleRadioSearchModel>> Search(string searchString, string countryCode = "DE", int selectedpage = 0)
        {
            //Token abrufen
            AppTokenService tokenService = new AppTokenService();
            IList<AppTokenModel> appToken = await tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return new List<DirbleRadioSearchModel>(); }

            string url = "http://api.dirble.com/v2/search?token=" + appToken[0].DirbleToken;

            string apiResponse = string.Empty;

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(url);

                    Dictionary<string, string> values = new Dictionary<string, string>
                    {
                        { "query", searchString },
                        { "country", countryCode },
                        { "page", selectedpage.ToString() }
                    };

                    FormUrlEncodedContent content = new FormUrlEncodedContent(values);

                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    apiResponse = await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new List<DirbleRadioSearchModel>();
                }

                if (apiResponse == string.Empty) { return new List<DirbleRadioSearchModel>(); }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    DirbleRadioSearchModel[] apiRadioArray = JsonConvert.DeserializeObject<DirbleRadioSearchModel[]>(apiResponse);
                    return new List<DirbleRadioSearchModel>(apiRadioArray);
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return new List<DirbleRadioSearchModel>();
                }
            }
        }
    }
}
