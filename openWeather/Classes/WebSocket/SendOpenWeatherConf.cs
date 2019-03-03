using Helper.Classes.WebSocket;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace OpenWeather.Classes.WebSocket
{
    public class SendOpenWeatherConf : Hub<ITypedHubClient>
    {
        //Hub für Wetter Conf
        public async Task SendMessage(string name, string message)
        {
            await Clients.All.BroadcastMessage(name, message);
        }
    }
}
