using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyScriptures.Classes.WebApi
{
    [Route("api/dailyScripturesLanguages")]
    [ApiController]
    public class ApiDailyScriptureLanguages : Controller
    {
        #region Models

        #endregion

        #region Constructor
        public ApiDailyScriptureLanguages() { }
        #endregion

        /// <summary>
        /// Listet alle gespeicherten Sprachen auf
        /// </summary>
        /// <returns>IList<DailyScriptureLanguageModel><returns>
        [HttpGet]
        public async Task<IList<DailyScriptureLanguageModel>> LoadAsync()
        {
            try
            {
                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                return await dailyScriptureLanguageService.ReadAllAsync();
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiDailyScriptureLanguages Load: " + ex.ToString(), "error");

                return new List<DailyScriptureLanguageModel>();
            }
        }

        /// <summary>
        /// Liest eine bestimmte ID aus
        /// </summary>
        /// <returns>DailyScriptureLanguageModel</returns>
        [HttpGet("{id}")]
        public async Task<DailyScriptureLanguageModel> LoadByIdAsync(int id)
        {
            try
            {
                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                return await dailyScriptureLanguageService.ReadByIdAsync(id);
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiDailyScriptureLanguages Load: " + ex.ToString(), "error");

                return new DailyScriptureLanguageModel();
            }
        }

        /// <summary>
        /// Erzeugt oder Aktualisiert eine Sprache
        /// </summary>
        [HttpPost]
        public async Task<int> CreateAsync(DailyScriptureLanguageModel dailyScriptureLanguage)
        {
            try
            {
                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                return await dailyScriptureLanguageService.CreateAsync(dailyScriptureLanguage);
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiDailyScriptureLanguages Create: " + ex.ToString(), "error");

                return 0;
            }
        }

        /// <summary>
        /// Löscht eine Sprache für die Tagestexte
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<DailyScriptureLanguageModel> DeleteAsync(int id)
        {
            try
            {
                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                DailyScriptureLanguageModel dailyScriptureLanguage = await dailyScriptureLanguageService.ReadByIdAsync(id);
                await dailyScriptureLanguageService.DeleteAsync(id);

                return dailyScriptureLanguage;

            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiDailyScriptureLanguages Create: " + ex.ToString(), "error");

                return new DailyScriptureLanguageModel();
            }
        }
    }
}
