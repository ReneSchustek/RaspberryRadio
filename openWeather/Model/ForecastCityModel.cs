using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ForecastCityModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("coord")]
        public ForecastCoordModel Coord { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
