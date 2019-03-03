using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class ForecastSysModel
    {
        [JsonProperty("pod")]
        public string Pod { get; set; }
    }
}
