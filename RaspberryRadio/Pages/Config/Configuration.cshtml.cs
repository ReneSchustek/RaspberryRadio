﻿using Database.Classes;
using Database.Model;
using Database.Services;
using Hangfire;
using Helper.Classes;
using Helper.Classes.BackgroundService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using OpenWeather.Classes;
using OpenWeather.Classes.WebSocket;
using RadioStation.Pages.Config;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RaspberryRadio.Pages.Config
{
    public class ConfigurationModel : PageModel
    {
        #region Models     
        //DailyScriptures
        [BindProperty]
        public DailyScriptureLanguageModel DailyScriptureLanguage { get; set; }
        [BindProperty]
        public IList<DailyScriptureLanguageModel> DailyScriptureLanguages { get; set; }

        //Token
        [BindProperty]
        public AppTokenModel Token { get; set; }
        [BindProperty]
        public IList<AppTokenModel> Tokens { get; set; }

        //OpenWeather
        [BindProperty]
        public OpenWeatherCityModel OpenWeatherCity { get; set; }
        [BindProperty]
        public IList<OpenWeatherCityModel> OpenWeatherCities { get; set; }
        [BindProperty]
        public IList<OpenWeatherCityModel> SavedOpenWeatherCities { get; set; }
        [BindProperty]
        public String OpenWeatherCountry { get; set; }
        [BindProperty]
        public string OpenWeatherCityId { get; set; }

        //Kalender
        [BindProperty]
        public CalendarModel Calendar { get; set; }
        [BindProperty]
        public IList<CalendarModel> Calendars { get; set; }

        //Global
        private readonly IHubContext<SendOpenWeatherConf> _hubContext;
        public IBackgroundTaskQueue _queue { get; }
        public string _refreshCitiesBackgroundId;

        //Services
        private readonly DailyScriptureLanguageService _dailyScriptureLanguageService;
        private readonly AppTokenService _tokenService;
        private readonly OpenWeatherCityService _openWeatherCityService;
        private readonly OpenWeatherSavedCitiesService _openWeatherSavedCitiesService;
        private readonly CalendarService _calenderService;
        #endregion

        #region Constructor
        public ConfigurationModel(DatabaseContext db, IHubContext<SendOpenWeatherConf> hubContext, IBackgroundTaskQueue queue)
        {
            _hubContext = hubContext;

            _queue = queue;

            _dailyScriptureLanguageService = new DailyScriptureLanguageService();
            _tokenService = new AppTokenService();
            _openWeatherCityService = new OpenWeatherCityService();
            _openWeatherSavedCitiesService = new OpenWeatherSavedCitiesService();
            _calenderService = new CalendarService();

        }
        #endregion

        /// <summary>
        /// Daten laden
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public async Task OnGetAsync(string response = null)
        {
            //Tagestextsprachen laden
            DailyScriptureLanguages = await _dailyScriptureLanguageService.ReadAllAsync();

            Tokens = await _tokenService.ReadAllAsync();

            //Token laden
            if (Tokens.Count == 0) { Token = new AppTokenModel { Id = 0, DirbleToken = String.Empty, OpenWeatherToken = String.Empty }; }
            else { Token = Tokens[0]; }

            //Gespeicherte Länder auslesen
            ReadSavedCities readSavedCities = new ReadSavedCities();
            SavedOpenWeatherCities = await readSavedCities.Read();

            //Kalender auslesen
            Calendars = await _calenderService.ReadAllAsync();

        }

        /// <summary>
        /// Speichert die Sprachen für den Tagestext
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSaveDailyScriptureAsync()
        {
            try
            {
                int id = 0;

                DailyScriptureLanguageModel newLanguage = new DailyScriptureLanguageModel();

                //Update oder neu anlegen
                if (DailyScriptureLanguage.Id.ToString() != "0")
                {
                    id = await _dailyScriptureLanguageService.UpdateAsync(DailyScriptureLanguage, DailyScriptureLanguage.Id);
                    newLanguage = await _dailyScriptureLanguageService.ReadByIdAsync(id);
                }
                else
                {
                    id = await _dailyScriptureLanguageService.CreateAsync(DailyScriptureLanguage);
                    newLanguage = await _dailyScriptureLanguageService.ReadByIdAsync(id);
                }

                ResponseObject okResponse = new ResponseObject
                {
                    Action = "save",
                    State = "ok",
                    Id = id,
                    Message = newLanguage.Language + " wurde gespeichert."
                };

                WriteLog.Write("Change Config: Id: " + id + "; " + okResponse.Message, "info");

                return RedirectToPage("/Config/Configuration", new { response = "save", state = "ok", saved = DailyScriptureLanguage.Language });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveDailyScripture: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        /// <summary>
        /// Löscht eine Sprache für den Tagestext
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteDailyScriptureAsync(int id)
        {
            try
            {
                DailyScriptureLanguageModel result = await _dailyScriptureLanguageService.ReadByIdAsync(id);
                int resultId = await _dailyScriptureLanguageService.DeleteAsync(id);

                return RedirectToPage("/Config/Configuration", new { response = "delete", state = "ok", save = DailyScriptureLanguage.Language });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration DeleteDailyScripture: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        /// <summary>
        /// Speichert die Tokens für Dirble und OpenWeather
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSaveTokenAsync()
        {
            try
            {
                int id = 0;

                AppTokenModel newToken = new AppTokenModel();

                //Update oder neu anlegen
                if (Token.Id.ToString() != "0")
                {
                    id = await _tokenService.UpdateAsync(Token, Token.Id);
                    newToken = await _tokenService.ReadByIdAsync(id);
                }
                else
                {
                    id = await _tokenService.CreateAsync(Token);
                    newToken = await _tokenService.ReadByIdAsync(id);
                }

                ResponseObject okResponse = new ResponseObject
                {
                    Action = "save",
                    State = "ok",
                    Id = id,
                    Message = "Die Token wurden gespeichert."
                };

                WriteLog.Write("Change Config: Id: " + id + "; " + okResponse.Message, "info");

                return RedirectToPage("/Config/Configuration", new { response = "save", state = "ok", saved = "Token" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveTokens: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        // <summary>
        // Liest die Städte neu ein
        // </summary>
        // <returns>Leitet zur Seite weiter</returns>
        public IActionResult OnPostRefreshCitiesAsync()
        {
            try
            {
                ReadCitiesFromJson readCitiesFromJson = new ReadCitiesFromJson(_hubContext);

                _refreshCitiesBackgroundId = BackgroundJob.Schedule(() => readCitiesFromJson.ReadAsync(JobCancellationToken.Null), TimeSpan.FromSeconds(1));

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration RefreshCities: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        /// <summary>
        /// Bricht das Einlesen der Städte ab
        /// </summary>
        /// <returns>Leitet zur Seite weiter</returns>
        public IActionResult OnPostAbortRefreshCitiesAsync()
        {
            //ToDo: Hangfire abbrechen

            return RedirectToPage("/Config/Configuration");
        }

        /// <summary>
        /// Speichert eine Stadt für die Wettervorhersage
        /// </summary>
        /// <returns>Leitet zur Seite weiter</returns>
        /// <remarks>Wenn die Stadt bereits vorhanden ist, wird diese aktualisiert</remarks>
        public async Task<IActionResult> OnPostSaveOpenWeatherCityAsync(int id, bool home)
        {
            try
            {
                //Speichern
                OpenWeatherSavedCitiesModel openWeatherSavedCity = new OpenWeatherSavedCitiesModel
                {
                    WeatherCitiesId = Convert.ToInt32(OpenWeatherCityId)
                };

                id = await _openWeatherSavedCitiesService.CreateAsync(openWeatherSavedCity);

                ResponseObject okResponse = new ResponseObject
                {
                    Action = "save",
                    State = "ok",
                    Id = id,
                    Message = "Die Stadt wurden gespeichert."
                };

                WriteLog.Write("Change Config: Id: " + id + "; " + okResponse.Message, "info");

                return RedirectToPage("/Config/Configuration", new { response = "save", state = "ok", saved = "Wettervorhersage" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveOpenWeatherCity: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        /// <summary>
        /// Stadt für die Wettervorhersage löschen
        /// </summary>
        /// <param name="cityId">CityId</param>
        /// <returns>Leitet zur Seiter weiter</returns>
        public async Task<IActionResult> OnPostDeleteOpenWeatherCityAsync(int cityId)
        {
            try
            {
                OpenWeatherSavedCitiesModel result = await _openWeatherSavedCitiesService.ReadByWeatherCitiesId(cityId);
                int resultId = await _openWeatherSavedCitiesService.DeleteAsync(result.Id);

                return RedirectToPage("/Config/Configuration", new { response = "delete", state = "ok", saved = DailyScriptureLanguage.Language });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration DeleteOpenWeatherCity: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        /// <summary>
        /// Liest das Länderkürzel aus dem gewählten Land
        /// </summary>
        /// <param name="selected">Gewählter Eintrag Format: Ländername (Kürzel)</param>
        /// <returns>Länderkürzel</returns>
        private string GetOpenWeatherCountry(string selected)
        {
            string[] stringArr = selected.Split('(');
            return stringArr[1].Trim().Substring(0, stringArr[1].Length - 1);
        }

        /// <summary>
        /// Speichert einen Kalendereintrag
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostSaveCalenderAsync()
        {
            try
            {
                int id = 0;

                CalendarModel newCalender = new CalendarModel();

                //Update oder neu anlegen
                if (Calendar.Id.ToString() != "0")
                {
                    id = await _calenderService.UpdateAsync(Calendar, Calendar.Id);
                    newCalender = await _calenderService.ReadByIdAsync(id);
                }
                else
                {
                    id = await _calenderService.CreateAsync(Calendar);
                    newCalender = await _calenderService.ReadByIdAsync(id);
                }

                ResponseObject okResponse = new ResponseObject
                {
                    Action = "save",
                    State = "ok",
                    Id = id,
                    Message = newCalender.Name + " wurde gespeichert."
                };

                WriteLog.Write("Change Config: Id: " + id + "; " + okResponse.Message, "info");

                return RedirectToPage("/Config/Configuration", new { response = "save", state = "ok", saved = Calendar.Name });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveCalender: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }

        /// <summary>
        /// Löscht einen Kalendereintrag
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteCalenderAsync(int id)
        {
            try
            {
                CalendarModel result = await _calenderService.ReadByIdAsync(id);
                int resultId = await _calenderService.DeleteAsync(id);

                return RedirectToPage("/Config/Configuration", new { response = "delete", state = "ok", save = Calendar.Name });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration DeleteCelender: " + ex.ToString(), "error");

                ResponseObject errResponse = new ResponseObject
                {
                    Action = "save",
                    State = "err",
                    Id = 0
                };

                return RedirectToPage("/Config/Configuration");
            }
        }
    }
}