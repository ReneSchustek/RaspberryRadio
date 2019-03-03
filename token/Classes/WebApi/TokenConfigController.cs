using Database.Classes;
using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Token.Classes
{
    [Route("/api/config/token")]
    public class TokenConfigController : Controller
    {
        #region Models
        private readonly AppTokenService _tokenService;
        #endregion

        #region Constructor
        public TokenConfigController()
        {
            _tokenService = new AppTokenService();
        }
        #endregion

        /// <summary>
        /// Liest alle Token aus und gibt diese zurück
        /// </summary>
        /// <returns>List<AppTokenModel></returns>
        [HttpGet]
        public async Task<IList<AppTokenModel>> ReadAllAsync()
        {
            try { return await _tokenService.ReadAllAsync(); }
            catch (Exception ex)
            {
                WriteLog.Write("Api TokenConfig ReadAll: " + ex.ToString());
                return new List<AppTokenModel>();
            }
        }

        /// <summary>
        /// Liest die Token nach Id aus und gibt diese zurück
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>AppTokenModel</returns>
        [HttpGet("{id}")]
        public async Task<AppTokenModel> ReadAsync(int id)
        {
            try
            {
                if (id == 0) { throw new Exception("Keine Id angegeben!"); }

                return await _tokenService.ReadByIdAsync(id);
            }
            catch (Exception ex)
            {
                WriteLog.Write("Api TokenConfig Read: " + ex.ToString());
                return new AppTokenModel();
            }
        }

        /// <summary>
        /// Löscht einen Token
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>id</returns>
        [HttpPost("delete/{id}")]
        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                if (id == 0) { throw new Exception("Keine Id angegeben!"); }

                AppTokenModel token = await _tokenService.ReadByIdAsync(id);
                return await _tokenService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                WriteLog.Write("Api TokenConfig Delete: " + ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// Ändert einen Token
        /// </summary>
        /// <param name="changedToken">AppTokenModel</param>
        /// <returns>AppTokenModel</returns>
        [HttpPost("edit/{language}")]
        public async Task<AppTokenModel> EditAsync(AppTokenModel changedToken)
        {
            try
            {
                if (changedToken == null)
                {
                    throw new Exception("Keine Daten angegeben.");
                }

                int id = await _tokenService.UpdateAsync(changedToken, changedToken.Id);
                return await _tokenService.ReadByIdAsync(id);
            }
            catch (Exception ex)
            {
                WriteLog.Write("Api TokenConfig Edit: " + ex.ToString());
                return new AppTokenModel();
            }
        }
    }
}
