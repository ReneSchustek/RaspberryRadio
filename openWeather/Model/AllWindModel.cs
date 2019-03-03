using Newtonsoft.Json;

namespace OpenWeather.Model
{
    public partial class AllWindModel
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("deg")]
        public long Deg { get; set; }
    }
}
