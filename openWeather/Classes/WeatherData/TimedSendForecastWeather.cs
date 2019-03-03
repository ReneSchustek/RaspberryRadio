using Helper.Classes;
using Helper.Classes.BackgroundService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using OpenWeather.Classes.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeather.Classes.WeatherData
{
    /// <summary>
    /// Aktualisiert alle 20 Min das aktuelle Wetter
    /// </summary>
    public class TimedSendForecastWeather : BackgroundService
    {
        #region Models
        private readonly IServiceScopeFactory _scopeFactory;
        #endregion

        #region Contstructor
        public TimedSendForecastWeather(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {

                        IHubContext<SendOpenWeatherForecast> hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<SendOpenWeatherForecast>>();

                        RefreshForecastWeather refreshForecastWeather = new RefreshForecastWeather(hubContext);
                        await refreshForecastWeather.RefreshAsync();
                    }

                    await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
                }
                catch (Exception ex) { WriteLog.Write("TimedSendForecastWeather: " + ex.ToString()); }
            }

        }
    }
}
