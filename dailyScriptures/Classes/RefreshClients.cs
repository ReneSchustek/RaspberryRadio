using DailyScriptures.Classes.WebSocket;
using Database.Model;
using Database.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyScriptures.Classes
{
    /// <summary>
    /// Websocket Message für den Tagestext
    /// </summary>
    public class RefreshClients
    {

        #region Models
        private readonly IHubContext<SendDailyScripture> _hubContext;
        #endregion

        #region Constructor
        public RefreshClients(IHubContext<SendDailyScripture> hubContext)
        {
            _hubContext = hubContext;
        }
        #endregion

        /// <summary>
        /// Sendet Tagestext zu den Clients
        /// </summary>
        /// <returns></returns>
        public async Task RefreshAsync()
        {
            DailyScriptureService dailyScriptureService = new DailyScriptureService();
            IList<DailyScriptureModel> dailyScriptureModels = await dailyScriptureService.ReadAllAsync();

            foreach (DailyScriptureModel dailyScriptureModel in dailyScriptureModels)
            {
                string message = JsonConvert.SerializeObject(dailyScriptureModels);
                await _hubContext.Clients.All.SendAsync("OnDailyScripturePublish", message);
            }
        }
    }
}
