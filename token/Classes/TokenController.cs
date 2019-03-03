using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Token.Classes
{
    public class TokenController : Controller
    {
        #region Models
        private readonly AppTokenService _tokenService;
        #endregion

        #region Constructor
        public TokenController()
        {
            _tokenService = new AppTokenService();
        }
        #endregion

        /// <summary>
        /// Gibt die gespeicherten Tokens zurück
        /// </summary>
        /// <returns>AppTokenModel</returns>
        [HttpGet]
        public async Task<IList<AppTokenModel>> Read()
        {
            try
            {
                IList<AppTokenModel> tokens = await _tokenService.ReadAllAsync();

                return tokens;

            }
            catch (Exception ex)
            {
                WriteLog.Write("Api Token Read: " + ex.ToString());
                return new List<AppTokenModel>();

            }
        }
    }
}
