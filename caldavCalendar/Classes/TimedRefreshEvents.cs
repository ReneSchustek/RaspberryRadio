using CaldavCalendar.Classes.WebSocket;
using Helper.Classes;
using Helper.Classes.BackgroundService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CaldavCalendar.Classes
{
    /// <summary>
    /// Aktualisiert die Termine alle 10 Minuten
    /// </summary>
    public class TimedRefreshEvents : BackgroundService
    {

        #region Models
        private readonly IServiceScopeFactory _scopeFactory;
        #endregion

        #region Constructor
        public TimedRefreshEvents(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        #endregion

        /// <summary>
        /// Aktualisiert die Termine als Hintergrunddienst
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
                        IHubContext<SendCalendar> hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<SendCalendar>>();

                        GetCalendarEvents getCalendarEvents = new GetCalendarEvents();
                        string calendarEvents = await getCalendarEvents.GetEvents();

                        await hubContext.Clients.All.SendAsync("OnEventPublish", calendarEvents);

                    }

                    await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                }
                catch (Exception ex) { WriteLog.Write("TimesRefreshEvents: " + ex.ToString()); }
            }
        }
    }
}
