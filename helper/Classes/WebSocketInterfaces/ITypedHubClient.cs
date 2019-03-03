using System.Threading.Tasks;

namespace Helper.Classes.WebSocket
{
    /// <summary>
    /// Interface um Daten an den Client zu senden
    /// </summary>
    public interface ITypedHubClient
    {
        Task BroadcastMessage(string name, string message);

    }
}
