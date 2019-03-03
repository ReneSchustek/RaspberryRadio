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
    /// Aktualisiert aktuelle Wetter
    /// </summary>
    public class TimedSendCurrentWeather : BackgroundService
    {
        #region Models
        private readonly IServiceScopeFactory _scopeFactory;
        #endregion

        #region Contstructor
        public TimedSendCurrentWeather(IServiceScopeFactory scopeFactory)
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

                        IHubContext<SendOpenWeatherCurrent> hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<SendOpenWeatherCurrent>>();

                        RefreshCurrentWeather refreshForecastWeather = new RefreshCurrentWeather(hubContext);
                        await refreshForecastWeather.RefreshAsync();
                    }

                    await Task.Delay(TimeSpan.FromMinutes(20), stoppingToken);
                }
                catch (Exception ex) { WriteLog.Write("TimedSendCurrentWeather: " + ex.ToString()); }
            }

        }
    }
}
