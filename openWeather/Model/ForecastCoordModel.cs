using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ForecastCoordModel
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }
    }
}
