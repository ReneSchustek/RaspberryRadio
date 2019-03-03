using CaldavCalendar.Classes;
using DailyScriptures.Classes;
using Database.Model;
using Database.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenWeather.Classes;
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
        #endregion

        #region Constructor
        public IndexModel() { }
        #endregion

        public async Task OnGetAsync()
        {
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
        }
    }
}