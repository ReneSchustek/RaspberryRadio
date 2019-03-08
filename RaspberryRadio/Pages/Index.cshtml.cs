using CaldavCalendar.Classes;
using DailyScriptures.Classes;
using Database.Classes;
using Database.Model;
using Database.Services;
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

namespace RaspberryRadio.Pages
{
    public class IndexModel : PageModel
    {
        #region BindingProperties
        [BindProperty]
        public string DailyScriptureCarouselContent { get; private set; }
        [BindProperty]
        public string DailyScriptureModalContent { get; private set; }

        [BindProperty]
        public string OpenWeatherCarouselContent { get; set; }
        [BindProperty]
        public string OpenWeatherModalContent { get; set; }
        [BindProperty]
        public string OpenWeatherForecastContent { get; set; }
        [BindProperty]
        public string CalendarEventContent { get; set; }

        #endregion

        #region Models
        //Konfiguration
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

        //Radio
        

        //Global
        private readonly IHubContext<SendOpenWeatherConf> _hubContext;
        public IBackgroundTaskQueue _queue { get; }
        public string _refreshCitiesBackgroundId;

        //Services
        private readonly DailyScriptureLanguageService _dailyScriptureLanguageService;
        private readonly AppTokenService _tokenService;
        private readonly OpenWeatherCityService _openWeatherCityService;
        private readonly OpenWeatherSavedCitiesService _openWeatherSavedCitiesService;
        private readonly CalendarService _calendarService;
        #endregion

        #region Constructor
        public IndexModel(DatabaseContext db, IHubContext<SendOpenWeatherConf> hubContext, IBackgroundTaskQueue queue)
        {
            //Konfiguration
            _hubContext = hubContext;

            _queue = queue;

            _dailyScriptureLanguageService = new DailyScriptureLanguageService();
            _tokenService = new AppTokenService();
            _openWeatherCityService = new OpenWeatherCityService();
            _openWeatherSavedCitiesService = new OpenWeatherSavedCitiesService();
            _calendarService = new CalendarService();
        }
        #endregion

        public async Task OnGetAsync()
        {
            /* *****************************
             * Startseite
             * ***************************** */

            //Tagestext prüfen            
            CheckDailyScriptures checkDailyScriptures = new CheckDailyScriptures();
            bool sameDay = await checkDailyScriptures.CheckSameDayAsync();

            if(!sameDay)
            {
                DailyScriptureLanguageService dailyScriptureLanguageService = new DailyScriptureLanguageService();
                IList<DailyScriptureLanguageModel> dailyScriptureLanguages = await dailyScriptureLanguageService.ReadAllAsync();

                foreach(DailyScriptureLanguageModel dailyScriptureLanguage in dailyScriptureLanguages) { await GetDailyScriptureFromWeb.GetAsync(dailyScriptureLanguage); }
            }            

            //Tagestext auslesen
            DailyScriptureService dailyScriptureService = new DailyScriptureService();
            IList<DailyScriptureModel> dailyScriptures = await dailyScriptureService.ReadAllAsync();

            //Tagestext erstellen
            CreateDailyScriptureCarousel createDailyScriptureCarousel = new CreateDailyScriptureCarousel();
            DailyScriptureCarouselContent = createDailyScriptureCarousel.CreateCarousel(dailyScriptures);
            CreateDailyScriptureModal createDailyScriptureModal = new CreateDailyScriptureModal();
            DailyScriptureModalContent = createDailyScriptureModal.CreateModal(dailyScriptures);

            //Wetter auslesen
            OpenWeatherSavedCitiesService openWeatherSavedCitiesService = new OpenWeatherSavedCitiesService();
            IList<OpenWeatherSavedCitiesModel> openWeatherSavedCities = await openWeatherSavedCitiesService.ReadAllAsync();

            //Keine Stadt erstellt -> Berlin
            if (openWeatherSavedCities.Count == 0)
            {
                OpenWeatherCityService openWeatherCityService = new OpenWeatherCityService();
                IList<OpenWeatherCityModel> openWeatherCities = await openWeatherCityService.ReadByNameCountryAsync("Berlin", "DE");

                if (openWeatherCities.Count > 0)
                {
                    OpenWeatherSavedCitiesModel openWeatherSavedCity = new OpenWeatherSavedCitiesModel
                    {
                        WeatherCitiesId = openWeatherCities[0].CityId
                    };

                    openWeatherSavedCities.Add(openWeatherSavedCity);
                }
                else { openWeatherSavedCities = new List<OpenWeatherSavedCitiesModel>(); }
            }

            //Wetter erstellen
            CreateWeatherCarousel createWeatherCarousel = new CreateWeatherCarousel();
            OpenWeatherCarouselContent = await createWeatherCarousel.CreateCarousel(openWeatherSavedCities);

            CreateCurrentWeatherModal createCurrentWeatherModal = new CreateCurrentWeatherModal();
            OpenWeatherModalContent = await createCurrentWeatherModal.CreateModal(openWeatherSavedCities);

            //Wettervorhersage erstellen
            CreateForecastWeather createForecastWeather = new CreateForecastWeather();
            OpenWeatherForecastContent = await createForecastWeather.CreateForecast(openWeatherSavedCities);

            //Kalendartermine erstellen
            GetCalendarEvents getCalendarEvents = new GetCalendarEvents();
            CalendarEventContent = await getCalendarEvents.GetEvents();

            /* *****************************
             * Konfiguration
             * ***************************** */

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
            Calendars = await _calendarService.ReadAllAsync();
        }

        /* *****************************
         * Konfiguration Scripte
         * ***************************** */

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
                
                return RedirectToPage(new { response = "save", state = "ok", message = DailyScriptureLanguage.Language, area = "dailyscripture" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveDailyScripture: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = DailyScriptureLanguage.Language, area = "dailyscripture" });
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

                return RedirectToPage(new { response = "delete", state = "ok", message = "Sprache", area = "dailyscripture" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration DeleteDailyScripture: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "delete", state = "err", message = "Sprache", area = "dailyscripture" });
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

                return RedirectToPage(new { response = "save", state = "ok", message = "Token", area = "token" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveTokens: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = "Token", area = "token" });
            }
        }

        // <summary>
        // Liest die Städte neu ein
        // </summary>
        // <returns>Leitet zur Seite weiter</returns>
        public async Task<IActionResult> OnPostRefreshCitiesAsync()
        {
            try
            {
                ReadCitiesFromJson readCitiesFromJson = new ReadCitiesFromJson(_hubContext);

                await readCitiesFromJson.ReadAsync();

                return RedirectToPage(new { response = "save", state = "ok", message = "Städte", area = "weather" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration RefreshCities: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = "Städte", area = "weather" });
            }
        }

        /// <summary>
        /// Bricht das Einlesen der Städte ab
        /// </summary>
        /// <returns>Leitet zur Seite weiter</returns>
        public IActionResult OnPostAbortRefreshCitiesAsync()
        {
            //ToDo: Background Prozess abbrechen

            return RedirectToPage(new { response = "cancel", state = "ok", message = "Städte", area = "weather" });
        }

        /// <summary>
        /// Speichert eine Stadt für die Wettervorhersage
        /// </summary>
        /// <returns>Leitet zur Seite weiter</returns>
        /// <remarks>Wenn die Stadt bereits vorhanden ist, wird diese aktualisiert</remarks>
        public async Task<IActionResult> OnPostSaveOpenWeatherCityAsync(int id)
        {
            try
            {
                //Speichern
                OpenWeatherSavedCitiesModel openWeatherSavedCity = new OpenWeatherSavedCitiesModel
                {
                    WeatherCitiesId = Convert.ToInt32(OpenWeatherCityId)
                };

                id = await _openWeatherSavedCitiesService.CreateAsync(openWeatherSavedCity);

                return RedirectToPage(new { response = "save", state = "ok", message = "Stadt", area = "weather" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveOpenWeatherCity: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = "Stadt", area = "weather" });
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

                return RedirectToPage(new { response = "delete", state = "ok", message = "Stadt", area = "weather" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration DeleteOpenWeatherCity: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = "Stadt", area = "weather" });
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
        public async Task<IActionResult> OnPostSaveCalendarAsync()
        {
            try
            {
                int id = 0;

                CalendarModel newCalendar = new CalendarModel();

                //Update oder neu anlegen
                if (Calendar.Id.ToString() != "0")
                {
                    id = await _calendarService.UpdateAsync(Calendar, Calendar.Id);
                    newCalendar = await _calendarService.ReadByIdAsync(id);
                }
                else
                {
                    id = await _calendarService.CreateAsync(Calendar);
                    newCalendar = await _calendarService.ReadByIdAsync(id);
                }
                return RedirectToPage(new { response = "save", state = "ok", message = "Kalender", area = "calendar" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration SaveCalendar: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = "Kalender", area = "calendar" });
            }
        }

        /// <summary>
        /// Löscht einen Kalendereintrag
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteCalendarAsync(int id)
        {
            try
            {
                CalendarModel result = await _calendarService.ReadByIdAsync(id);
                int resultId = await _calendarService.DeleteAsync(id);

                return RedirectToPage(new { response = "delete", state = "err", message = "Kalender", area = "calendar" });
            }
            catch (Exception ex)
            {
                WriteLog.Write("Configuration DeleteCelender: " + ex.ToString(), "error");
                return RedirectToPage(new { response = "save", state = "err", message = "Kalender", area = "calendar" });
            }
        }
    }
}
 
 