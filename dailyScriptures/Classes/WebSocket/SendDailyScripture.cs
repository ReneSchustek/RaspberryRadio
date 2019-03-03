using Helper.Classes.WebSocket;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DailyScriptures.Classes.WebSocket
{
    /// <summary>
    /// Hub für den Tagestext
    /// </summary>
    public class SendDailyScripture : Hub<ITypedHubClient>
    {
        public async Task SendMessage(string name, string message)
        {
            await Clients.All.BroadcastMessage(name, message);
        }
    }
}
