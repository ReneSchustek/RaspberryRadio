using Database.Model;
using Database.Services;
using Helper.Classes;
using Helper.Classes.BackgroundService;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScriptures.Classes
{
    /// <summary>
    /// Hintergrunddienst zum Aktualisieren des Tagestexts
    /// </summary>
    public class TimedLoadService : BackgroundService
    {
        #region Models
        #endregion

        #region Constructor
        public TimedLoadService() { }
        #endregion

        /// <summary>
        /// Holt den Tagestext und aktualisiert diesen
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {


                    CheckDailyScriptures checkDailyScriptures = new CheckDailyScriptures();

                    if (await checkDailyScriptures.CheckSameDayAsync() == false)
                    {
                        string today = DateTime.Now.ToShortDateString();

                        //Immer alle aktualisieren
                        DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                        IList<DailyScriptureLanguageModel> dailyScriptureLanguages = await dailyScriptureLanguageService.ReadAllAsync();

                        DailyScriptureService dailyScriptureService = new DailyScriptureService();

                        foreach (DailyScriptureLanguageModel dailyScriptureLanguage in dailyScriptureLanguages)
                        {
                            DailyScriptureModel dailyScripture = await GetDailyScriptureFromWeb.GetAsync(dailyScriptureLanguage, today);
                            await dailyScriptureService.CreateAsync(dailyScripture);
                        }
                    }


                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }
                catch (Exception ex) { WriteLog.Write("TimedRefreshService: " + ex.ToString()); }
            }
        }

    }
}
