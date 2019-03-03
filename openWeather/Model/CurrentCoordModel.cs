using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class CurrentCoordModel
    {
        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("lat")]
        public double Lat { get; set; }
    }
}
