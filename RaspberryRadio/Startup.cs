using CaldavCalendar.Classes;
using CaldavCalendar.Classes.WebSocket;
using DailyScriptures.Classes;
using DailyScriptures.Classes.WebSocket;
using Database.Classes;
using Helper.Classes.BackgroundService;
using Helper.Classes.BackgroundService.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenWeather.Classes.WeatherData;
using OpenWeather.Classes.WebSocket;

namespace RaspberryRadio
{
    public class Startup
    {
        #region Models
        private readonly string _connectionString;
        public IConfiguration Configuration { get; }
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        #endregion

        #region Constructor
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
            _connectionString = CreateConnectionString.Create();
        }
        #endregion

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc();

            string dbPath = CreateConnectionString.Create();

            services.AddDbContext<DatabaseContext>(options => options.UseSqlite(dbPath));

            services.AddSignalR();
            services.AddAntiforgery();

            // Hintergrunddienste
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            //Sendet alle 60 Sekunden den Tagestext an die Clients
            services.AddSingleton<IHostedService, TimedRefreshService>();

            //Prüft, ob die Tagestexte aktuell sind.
            services.AddSingleton<IHostedService, TimedLoadService>();


            //Sendet alle 15 Minuten das aktuelle Wetter an die Clients
            services.AddSingleton<IHostedService, TimedSendCurrentWeather>();

            //Sendet alle 20 Minuten die Wettervorhersage an die Clients
            services.AddSingleton<IHostedService, TimedSendForecastWeather>();

            //Sendet alle 10 Minuten die Kalendereinträge an die Clients
            services.AddSingleton<IHostedService, TimedRefreshEvents>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            //SignalR Routes
            app.UseSignalR(routes =>
            {
                routes.MapHub<SendDailyScripture>("/dailyScriptureHub");
                routes.MapHub<SendOpenWeatherConf>("/openWeatherConfHub");
                routes.MapHub<SendOpenWeatherCurrent>("/currentWeatherHub");
                routes.MapHub<SendOpenWeatherForecast>("/forecastWeatherHub");
                routes.MapHub<SendCalendar>("/calendarHub");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }          

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
