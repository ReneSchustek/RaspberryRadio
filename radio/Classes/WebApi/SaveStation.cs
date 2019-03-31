using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace radio.Classes.WebApi
{
    [Route("/api/radio/save")]
    [ApiController]
    public class SaveStation : Controller
    {
        #region Models
        #endregion

        #region Constructor
        public SaveStation() { }
        #endregion

        [HttpGet("{pos}/{name}/{favname}/{url}/{imgurl}")]
        [Route("SaveStation")]
        public async Task<bool> Save(int pos, string name, string favName, string url, string imgUrl)
        {
            try
            {
                RadioFavModel radioFav = new RadioFavModel
                {
                    FavoriteName = favName,
                    ImageUrl = imgUrl,
                    Name = name,
                    Pos = pos,
                    Url = url
                };

                RadioService radioService = new RadioService();                
                int id = await radioService.CreateAsync(radioFav);

                return true;
            }
            catch(Exception ex)
            {
                WriteLog.Write(ex.ToString(), "error");
                return false;
            }
        }
    }
}
