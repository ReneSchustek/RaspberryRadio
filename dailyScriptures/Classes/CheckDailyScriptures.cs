using Database.Model;
using Database.Services;
using Helper.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyScriptures.Classes
{
    public class CheckDailyScriptures
    {
        #region Models

        #endregion

        #region Constructor
        public CheckDailyScriptures() { }
        #endregion

        /// <summary>
        /// Prüft, ob der Tagestext von heute stammt
        /// </summary>
        /// <returns>False = nicht von heute, True = von heute</returns>
        public async Task<Boolean> CheckSameDayAsync()
        {
            try
            {
                DailyScriptureLanguageService dailyScripturesLanguageService = new DailyScriptureLanguageService();
                IList<DailyScriptureLanguageModel> dailyScriptureLanguages = await dailyScripturesLanguageService.ReadAllAsync();

                DailyScriptureService dailyScriptureService = new DailyScriptureService();

                DateTime today = DateTime.Now;

                bool check = true;

                foreach (DailyScriptureLanguageModel dailyScriptureLanguage in dailyScriptureLanguages)
                {
                    if (check == false) { break; }

                    DailyScriptureModel dailyScripture = await dailyScriptureService.ReadByLanguageAsync(dailyScriptureLanguage.Language);

                    DateTime scriptureDate = (DateTime)dailyScripture.UpdatedDate;

                    //Vergleiche den Tag;
                    if (scriptureDate.Day != today.Day) { check = false; }
                }

                return check;
            }
            catch (Exception ex)
            {
                WriteLog.Write("CheckDailyScriptures: " + ex.ToString(), "error");
                return false;
            }
        }
    }
}
