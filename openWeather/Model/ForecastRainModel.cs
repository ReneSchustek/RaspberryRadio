using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ForecastRainModel
    {
        [JsonProperty("3h", NullValueHandling = NullValueHandling.Ignore)]
        public double The3H { get; set; }
    }
}
