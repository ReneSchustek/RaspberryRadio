using Database.Model;
using Database.Services;
using Helper.Classes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyScriptures.Classes.WebApi
{
    /// <summary>
    /// Api zum erneuten einlesen der Tagestexte
    /// </summary>
    [Route("api/loadDailyScriptures")]
    [ApiController]
    public class ApiLoadDailyScriptures : Controller
    {
        #region Models

        #endregion

        #region Constructor
        public ApiLoadDailyScriptures() { }
        #endregion

        /// <summary>
        /// Aktualisiert alle Tagestexte
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<string>> LoadAsync()
        {
            try
            {
                IList<string> resultList = new List<string>();
                string today = DateTime.Now.ToShortDateString();

                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                IList<DailyScriptureLanguageModel> dailyScriptureLanguages = await dailyScriptureLanguageService.ReadAllAsync();

                DailyScriptureService dailyScriptureService = new DailyScriptureService();

                foreach (DailyScriptureLanguageModel dailyScriptureLanguage in dailyScriptureLanguages)
                {
                    DailyScriptureModel dailyScripture = await GetDailyScriptureFromWeb.GetAsync(dailyScriptureLanguage, today);

                    int dbEntry = await dailyScriptureService.CreateAsync(dailyScripture);
                    string message = dailyScriptureLanguage.Language + " wurde aktualisiert. (" + dbEntry + ")\n";

                    resultList.Add(message);
                }

                return resultList;
            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiLoadDailyScriptures Load: " + ex.ToString(), "error");

                IList<string> error = new List<string>
                {
                    "Es ist ein Fehler aufgetreten"
                };

                return error;
            }
        }

        /// <summary>
        /// Aktualisiert den Tagestext einer bestimmten Sprache
        /// </summary>
        [HttpGet("{language}")]
        public async Task<string> LoadByLanguageAsync(string language)
        {
            try
            {
                string today = DateTime.Now.ToShortDateString();

                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                DailyScriptureLanguageModel dailyScriptureLanguage = await dailyScriptureLanguageService.ReadByLanguageAsync(language);

                DailyScriptureService dailyScriptureService = new DailyScriptureService();
                DailyScriptureModel dailyScripture = await GetDailyScriptureFromWeb.GetAsync(dailyScriptureLanguage, today);

                int dbEntry = await dailyScriptureService.CreateAsync(dailyScripture);
                return dailyScriptureLanguage.Language + " wurde aktualisiert. (" + dbEntry + ")\n";

            }
            catch (Exception ex)
            {
                WriteLog.Write("ApiLoadDailyScriptures LoadByLanguage: " + ex.ToString(), "error");
                return "Es ist ein Fehler aufgetreten";
            }
        }

    }
}
