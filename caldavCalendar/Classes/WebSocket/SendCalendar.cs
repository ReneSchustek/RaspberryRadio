using Helper.Classes.WebSocket;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace CaldavCalendar.Classes.WebSocket
{
    /// <summary>
    /// Hub für Kalendar
    /// </summary>
    public class SendCalendar : Hub<ITypedHubClient>
    {
        public async Task SendMessage(string name, string message)
        {
            await Clients.All.BroadcastMessage(name, message);
        }
    }
}
