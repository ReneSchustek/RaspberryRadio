using DailyScriptures.Classes.WebSocket;
using Helper.Classes;
using Helper.Classes.BackgroundService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DailyScriptures.Classes
{
    /// <summary>
    /// Aktualisiert alle 60 Sekunden den Tagestext
    /// </summary>
    public class TimedRefreshService : BackgroundService
    {
        #region Models
        private readonly IServiceScopeFactory _scopeFactory;
        #endregion

        #region Contstructor
        public TimedRefreshService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        #endregion

        /// <summary>
        /// Aktualisiert den Tagestext als Hintergrunddienst
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (IServiceScope scope = _scopeFactory.CreateScope())
                    {

                        IHubContext<SendDailyScripture> hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<SendDailyScripture>>();

                        RefreshClients refreshClients = new RefreshClients(hubContext);
                        await refreshClients.RefreshAsync();
                    }

                    await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
                }
                catch (Exception ex) { WriteLog.Write("TimedRefreshService: " + ex.ToString()); }
            }

        }
    }
}
