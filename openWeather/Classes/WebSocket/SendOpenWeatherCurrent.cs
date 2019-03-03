using Helper.Classes.WebSocket;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OpenWeather.Classes.WebSocket
{
    /// <summary>
    /// Hub für CurrentWeather
    /// </summary>
    public class SendOpenWeatherCurrent : Hub<ITypedHubClient>
    {
        public async Task SendMessage(string name, string message)
        {
            await Clients.All.BroadcastMessage(name, message);
        }
    }
}
