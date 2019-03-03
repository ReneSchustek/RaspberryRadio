using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OpenWeather.Classes
{
    public class GetCityData
    {
        /// <summary>
        /// Lädt das Wetter für einen Ort
        /// </summary>
        /// <param name="token">ApiToken für openWeather</param>
        /// <param name="name">Ortsname</param>
        /// <param name="country">Ländername</param>
        /// <param name="cityId">CityId von openWeather</param>
        /// <remarks>Städte werden normaler beim ersten Start in die db geladen</remarks>
        /// <returns>Json</returns>
        public async Task<string> Get(string token = null, string name = null, string country = null, string cityId = null)
        {

            string url = "https://api.openweathermap.org/data/2.5/weather?q=" + name + "&appid=" + token;
            string content = string.Empty;

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse res = (HttpWebResponse)await req.GetResponseAsync())
            using (Stream stream = res.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                content = await reader.ReadToEndAsync();
            }

            return content;
        }
    }
}
