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
        /// <param name="pos">Favoriten Position</param>
        /// <returns>gefundene Station</returns>
        [HttpGet("{id}/{pos}")]
        [Route("SpecificStation")]
        public async Task<bool> GetStation(int id, int pos)
        {
            //Token abrufen
            AppTokenService tokenService = new AppTokenService();
            IList<AppTokenModel> appToken = await tokenService.ReadAllAsync();
            if (appToken.Count < 1) { return false; }

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
                    return false;
                }

                if (apiResponse == string.Empty) { return false; }

                //Zu Objekt konvertieren und zurückgeben
                try
                {
                    DirbleRadioSpecificModel station = JsonConvert.DeserializeObject<DirbleRadioSpecificModel>(apiResponse);
                    RadioFavModel favModel = new RadioFavModel();
                    favModel.Pos = pos;
                    favModel.Name = station.Name;



                    if (station.Name.Length > 8) { favModel.FavoriteName = station.Name.Substring(0, 8) + "..."; }
                    else { favModel.FavoriteName = station.Name; }


                    foreach (DirbleStreamModel stream in station.Streams)
                    {
                        if (stream.ContentType != "audio/mpeg") { continue; }

                        favModel.Url = stream.Stream.ToString();

                        break;
                    }

                    favModel.ImageUrl = "/images/micro.jpg";
                    DirbleImageModel image = station.Image;
                    if (image.Url.ToString() != "" && image.Url.ToString() != null && image.Url.ToString() != string.Empty)
                    {
                        favModel.ImageUrl = image.Url.ToString();
                    }

                    RadioService radioService = new RadioService();
                    int dbId = await radioService.CreateAsync(favModel);

                    return true;
                }
                catch (Exception ex)
                {
                    WriteLog.Write(ex.ToString(), "error");
                    return false;
                }
            }
        }
    }
}
